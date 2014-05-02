using UnityEngine;
using UnityEditor;
using System.Collections;

public class GraphEditorWindow : EditorWindow {
	private Color kGridMinorColorDark = new Color(0f, 0f, 0f, 0.18f);
	private Color kGridMajorColorDark = new Color(0f, 0f, 0f, 0.28f);
	private bool gridDrag;
	protected Vector2 scroll;
	private Vector2 scrollView= new Vector2(10000,10000);

	private void OnGUI(){
		GUIStyle verticalScrollbar =new GUIStyle( GUI.skin.verticalScrollbar);
		GUI.skin.verticalScrollbar = GUIStyle.none;
		GUIStyle horizontalScrollbar = GUI.skin.horizontalScrollbar;
		GUI.skin.horizontalScrollbar = GUIStyle.none;

		scroll = GUI.BeginScrollView (new Rect(0,0,position.width,position.height), scroll, new Rect (0, 0, scrollView.x, scrollView.y), false, false);
		DrawGrid (position, scroll);
		OnGraphGUI ();
		GUI.EndScrollView ();

		GUI.skin.verticalScrollbar = verticalScrollbar;
		GUI.skin.horizontalScrollbar = horizontalScrollbar;
		HandleEvents ();
	}
	
	protected virtual void OnGraphGUI(){
	}

	protected virtual void HandleEvents(){
		Event e = Event.current;
		switch (e.type) {
		case EventType.mouseDown:
			gridDrag = e.button == 2;
			break;
		case EventType.mouseUp:
			gridDrag = false;
			break;
		case EventType.mouseDrag:
			if (gridDrag) {
				scroll -= e.delta;
				scroll.x=Mathf.Clamp(scroll.x,0,Mathf.Infinity);
				scroll.y=Mathf.Clamp(scroll.y,0,Mathf.Infinity);
				e.Use ();
			}
			break;
		}

		
		wantsMouseMove=true;
		if(e.isMouse){
			e.Use();
		}
	}

	protected virtual void DrawGrid(Rect rect,Vector2 scroll)
	{
		if (Event.current.type != EventType.Repaint){
			return;
		}
		this.DrawGridLines(rect,scroll,12f,kGridMinorColorDark);
		this.DrawGridLines(rect,scroll,120f, kGridMajorColorDark);
	}
	
	private void DrawGridLines(Rect rect,Vector2 scroll, float gridSize, Color gridColor)
	{
		Handles.color = gridColor;
		for (float i =0; i < rect.width +scroll.x; i = i + gridSize){
			Handles.DrawLine(new Vector2(i, 0), new Vector2(i, position.height+scroll.y));
		}
		for (float j = 0; j < position.height+scroll.y; j = j + gridSize){
			Handles.DrawLine(new Vector2(0, j), new Vector2(rect.width+scroll.x, j));
		}
	}
	
	public void DrawConnection (Vector3 start, Vector3 end,Rect rect, Color color)
	{
		if (Event.current.type != EventType.repaint) {
			return;
		}
		
		Vector3 cross = Vector3.Cross ((start - end).normalized, Vector3.forward);
		start = start + cross * 12f;
		
		Texture2D tex = (Texture2D)UnityEditor.Graphs.Styles.connectionTexture.image;
		Handles.color = color;
		Handles.DrawAAPolyLine (tex, 5f, new Vector3[] { start, end });
		
		Vector3 vector3 = end - start;
		Vector3 vector31 = vector3.normalized;
		Vector3 vector32 = (vector3 * 0.5f) + start;
		vector32 = vector32 - (cross * 0.5f);
		Vector3 vector33 = vector32 + vector31;
		if(rect.Contains(vector33))
			DrawArrow (color, cross, vector31, vector33);
		
	}
	
	public void DrawCurvyConnection (Vector3 start, Vector3 end,Rect rect, Color color)
	{
		if (Event.current.type != EventType.repaint) {
			return;
		}
		
		Vector3 cross = Vector3.Cross ((start - end).normalized, Vector3.forward);
		start = start + cross * 12f;
		
		Texture2D tex = (Texture2D)UnityEditor.Graphs.Styles.connectionTexture.image;
		
		Vector3[] vector3Array;
		Vector3[] vector3Array1;
		GetAngularConnectorValues (start, end, out vector3Array, out vector3Array1);
		DrawRoundedPolyLine(vector3Array, vector3Array1, tex, color,3);
	}
	
	private void DrawArrow (Color color, Vector3 cross, Vector3 direction, Vector3 center)
	{
		float size = 6.0f;
		Vector3[] vector3Array = new Vector3[] {
			center + (direction * size),
			(center - (direction * size)) + (cross * size),
			(center - (direction * size)) - (cross * size),
			center + (direction * size)
		};
		
		Color color1 = color;
		CreateMaterial ();
		material.SetPass (0);
		
		GL.Begin (4);
		GL.Color (color1);
		GL.Vertex (vector3Array [0]);
		GL.Vertex (vector3Array [1]);
		GL.Vertex (vector3Array [2]);
		GL.End ();
	}
	
	private Material material;
	private void CreateMaterial ()
	{
		if (material != null)
			return;
		
		material = new Material ("Shader \"Lines/Colored Blended\" {" +
		                         "SubShader { Pass { " +
		                         "    Blend SrcAlpha OneMinusSrcAlpha " +
		                         "    ZWrite Off Cull Off Fog { Mode Off } " +
		                         "    BindChannels {" +
		                         "      Bind \"vertex\", vertex Bind \"color\", color }" +
		                         "} } }");
		material.hideFlags = HideFlags.HideAndDontSave;
		material.shader.hideFlags = HideFlags.HideAndDontSave;
	}
	
	public void GetAngularConnectorValues (Vector2 start, Vector2 end, out Vector3[] points, out Vector3[] tangents)
	{
		Vector2 vector2 = start - end;
		Vector2 vector21 = (vector2 / 2f) + end;
		Vector2 vector22 = new Vector2 (Mathf.Sign (vector2.x), Mathf.Sign (vector2.y));
		Vector2 vector23 = new Vector2 (Mathf.Min (Mathf.Abs (vector2.x / 2f), 5f), Mathf.Min (Mathf.Abs (vector2.y / 2f), 5f));
		points = new Vector3[] {
			start,
			new Vector3 (vector21.x + vector23.x * vector22.x, start.y),
			new Vector3 (vector21.x, start.y - vector23.y * vector22.y),
			new Vector3 (vector21.x, end.y + vector23.y * vector22.y),
			new Vector3 (vector21.x - vector23.x * vector22.x, end.y),
			end
		};
		Vector3[] vector3Array = new Vector3[4];
		Vector3 vector3 = points [1] - points [0];
		vector3Array [0] = ((vector3.normalized * vector23.x) * 0.6f) + points [1];
		Vector3 vector31 = points [2] - points [3];
		vector3Array [1] = ((vector31.normalized * vector23.y) * 0.6f) + points [2];
		Vector3 vector32 = points [3] - points [2];
		vector3Array [2] = ((vector32.normalized * vector23.y) * 0.6f) + points [3];
		Vector3 vector33 = points [4] - points [5];
		vector3Array [3] = ((vector33.normalized * vector23.x) * 0.6f) + points [4];
		tangents = vector3Array;
	}
	
	public void DrawRoundedPolyLine (Vector3[] points, Vector3[] tangets, Texture2D tex, Color color,float width)
	{
		Handles.color = color;
		for (int i = 0; i < (int)points.Length; i = i + 2) {
			Handles.DrawAAPolyLine (tex, width, new Vector3[] { points [i], points [i + 1] });
		}
		for (int j = 0; j < (int)tangets.Length; j = j + 2) {
			Handles.DrawBezier (points [j + 1], points [j + 2], tangets [j], tangets [j + 1], color, tex, width);
		}
	}
	
	private void GetCurvyConnectorValues (Vector2 start, Vector2 end, out Vector3[] points, out Vector3[] tangents)
	{
		points = new Vector3[] { start, end };
		tangents = new Vector3[2];
		float single = (start.y >= end.y ? 0.7f : 0.3f);
		single = 0.5f;
		float single1 = 1f - single;
		float single2 = 0f;
		if (start.x > end.x) {
			float single3 = -0.25f;
			single = single3;
			single1 = single3;
			float single4 = (start.x - end.x) / (start.y - end.y);
			if (Mathf.Abs (single4) > 0.5f) {
				float single5 = (Mathf.Abs (single4) - 0.5f) / 8f;
				single5 = Mathf.Sqrt (single5);
				single2 = Mathf.Min (single5 * 80f, 80f);
				if (start.y > end.y) {
					single2 = -single2;
				}
			}
		}
		Vector2 vector2 = start - end;
		float single6 = Mathf.Clamp01 ((vector2.magnitude - 10f) / 50f);
		tangents [0] = start + (new Vector2 ((end.x - start.x) * single + 30f, single2) * single6);
		tangents [1] = end + (new Vector2 ((end.x - start.x) * -single1 - 30f, -single2) * single6);
	}
}
