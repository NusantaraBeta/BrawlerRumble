// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class SS_Joystick : MonoBehaviour {
//////////////////////////////////////////////////////////////
// SS_Joystick.js
// Penelope iPhone Tutorial
//
// SS_Joystick creates a movable SS_Joystick (via GUITexture) that 
// handles touch input, taps, and phases. Dead zones can control
// where the SS_Joystick input gets picked up and can be normalized.
//
// Optionally, you can enable the touchPad property from the editor
// to treat this SS_Joystick as a TouchPad. A TouchPad allows the finger
// to touch down at any point and it tracks the movement relatively 
// without moving the graphic
//////////////////////////////////////////////////////////////

// A simple class for bounding how far the GUITexture will move
private class Boundary 
{
	public Vector2 min = Vector2.zero;
	public Vector2 max = Vector2.zero;
}

static private SS_Joystick[] SS_Joysticks;					// A static collection of all SS_Joysticks
static private bool  enumeratedSS_Joysticks = false;
static private float tapTimeDelta = 0.3f;				// Time allowed between taps

public bool  touchPad; 									// Is this a TouchPad?
public Rect touchZone;
public Vector2 deadZone = Vector2.zero;						// Control when position is output
public bool  normalize = false; 							// Normalize output after the dead-zone?
public Vector2 position; 									// [-1, 1] in x,y
public int tapCount;											// Current tap count

private int lastFingerId= -1;								// Finger last used for this SS_Joystick
private float tapTimeWindow;							// How much time there is left for a tap to occur
private Vector2 fingerDownPos;

private GUITexture gui;								// SS_Joystick graphic
private Rect defaultRect;								// Default position / extents of the SS_Joystick graphic
private Boundary guiBoundary = new Boundary();			// Boundary for SS_Joystick graphic
private Vector2 guiTouchOffset;						// Offset to apply to touch input
private Vector2 guiCenter;							// Center of SS_Joystick

void  Start (){
	// Cache this component at startup instead of looking up every frame	
	gui = GetComponent< GUITexture >();
	
	// Store the default rect for the gui, so we can snap back to it
	defaultRect = gui.pixelInset;	
    
    defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5f;
    defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5f;

    transform.position = Vector2.zero;
        
	if ( touchPad )
	{
		// If a texture has been assigned, then use the rect ferom the gui as our touchZone
		if ( gui.texture )
			touchZone = defaultRect;
	}
	else
	{				
		// This is an offset for touch input to match with the top left
		// corner of the GUI
		guiTouchOffset.x = defaultRect.width * 0.5f;
		guiTouchOffset.y = defaultRect.height * 0.5f;
		
		// Cache the center of the GUI, since it doesn't change
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		
		// Let's build the GUI boundary, so we can clamp SS_Joystick movement
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	}
}

void  Disable (){
#if UNITY_3_5
        gameObject.active = false;
#else
        gameObject.SetActive(false);
#endif

	enumeratedSS_Joysticks = false;
}

void  ResetSS_Joystick (){
	// Release the finger control and set the SS_Joystick back to the default position
	gui.pixelInset = defaultRect;
	lastFingerId = -1;
	position = Vector2.zero;
	fingerDownPos = Vector2.zero;
	
	if (touchPad) {
	    Color color = gui.color;
        color.a = 0.025f;
	    gui.color = color;
	}
		
}

bool IsFingerDown (){
	 return (lastFingerId != -1);
}
	
void  LatchedFinger (  int fingerId   ){
	// If another SS_Joystick has latched this finger, then we must release it
	if ( lastFingerId == fingerId )
		ResetSS_Joystick();
}

void  Update (){	
	if ( !enumeratedSS_Joysticks )
	{
		// Collect all SS_Joysticks in the game, so we can relay finger latching messages
		SS_Joysticks = FindObjectsOfType(typeof( SS_Joystick )) as SS_Joystick[];
		enumeratedSS_Joysticks = true;
	}	
		
	int count = Input.touchCount;
	
	// Adjust the tap time window while it still available
	if ( tapTimeWindow > 0 )
		tapTimeWindow -= Time.deltaTime;
	else
		tapCount = 0;
	
	if ( count == 0 )
		ResetSS_Joystick();
	else
	{
		for(int i = 0;i < count; i++)
		{
			Touch touch = Input.GetTouch(i);			
			Vector2 guiTouchPos = touch.position - guiTouchOffset;
	
			bool shouldLatchFinger = false;
			if ( touchPad )
			{				
				if ( touchZone.Contains( touch.position ) )
					shouldLatchFinger = true;
			}
			else if ( gui.HitTest( touch.position ) )
			{
				shouldLatchFinger = true;
			}		
	
			// Latch the finger if this is a new touch
			if ( shouldLatchFinger && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
			{
				
				if ( touchPad )
				{
	                Color color = gui.color;
                    color.a = 0.15f;
	                gui.color = color;
					
					lastFingerId = touch.fingerId;
					fingerDownPos = touch.position;
				}
				
				lastFingerId = touch.fingerId;
				
				// Accumulate taps if it is within the time window
				if ( tapTimeWindow > 0 )
					tapCount++;
				else
				{
					tapCount = 1;
					tapTimeWindow = tapTimeDelta;
				}
											
				// Tell other SS_Joysticks we've latched this finger
				foreach( SS_Joystick j in SS_Joysticks )
				{
					if ( j != this )
						j.LatchedFinger( touch.fingerId );
				}						
			}				
	
			if ( lastFingerId == touch.fingerId )
			{	
				// Override the tap count with what the iPhone SDK reports if it is greater
				// This is a workaround, since the iPhone SDK does not currently track taps
				// for multiple touches
				if ( touch.tapCount > tapCount )
					tapCount = touch.tapCount;
				
				if ( touchPad )
				{	
					// For a touchpad, let's just set the position directly based on distance from initial touchdown
					position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
					position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
				}
				else
				{					
					// Change the location of the SS_Joystick graphic to match where the touch is
				    Rect rect = gui.pixelInset;
					rect.x = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
					rect.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
				    gui.pixelInset = rect;
				}
				
				if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
					ResetSS_Joystick();					
			}			
		}
	}
	
	if ( !touchPad )
	{
		// Get a value between -1 and 1 based on the SS_Joystick graphic location
		position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
		position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
	}
	
	// Adjust for dead zone	
	float absoluteX = Mathf.Abs( position.x );
	float absoluteY = Mathf.Abs( position.y );
	
	if ( absoluteX < deadZone.x )
	{
		// Report the SS_Joystick as being at the center if it is within the dead zone
		position.x = 0;
	}
	else if ( normalize )
	{
		// Rescale the output after taking the dead zone into account
		position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
	}
		
	if ( absoluteY < deadZone.y )
	{
		// Report the SS_Joystick as being at the center if it is within the dead zone
		position.y = 0;
	}
	else if ( normalize )
	{
		// Rescale the output after taking the dead zone into account
		position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
	}
}
}