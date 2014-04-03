using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TGPDemo : MonoBehaviour
{
	public bool rotate = true;
	public GameObject rotateGroup;
	public float rotationSpeed = 50.0f;
	
	public Texture[] rampTextures;
	public GUITexture rampUI;
	private int rampIndex = 0;
	
	public GUIText qualityLabel;
	
	private Material[] matsSimple;
	private Material[] matsOutline;
	private Material[] matsAll;
	private GameObject sceneLight;
	
	public Shader[] shaders;
	
	void Start ()
	{
		//Get Materials
		GameObject astrella = GameObject.Find("TGPDemo_Astrella").gameObject;
		Renderer[] rs = astrella.GetComponentsInChildren<Renderer>();
		List<Material> matsS = new List<Material>();
		List<Material> matsO = new List<Material>();
		List<Material> matsA = new List<Material>();
		foreach(Renderer r in rs)
		{
			foreach(Material m in r.materials)
			{
				if(m.shader.name.Contains("Outline"))
					matsO.Add(m);
				else if(m.shader.name.Contains("Toony"))
					matsS.Add(m);
				
				if(m.shader.name.Contains("Toony"))
					matsA.Add(m);
			}
		}
		matsSimple = matsS.ToArray();
		matsOutline = matsO.ToArray();
		matsAll = matsA.ToArray();
		
		//Light Rotation
		sceneLight = GameObject.Find("_Light");
		lightRotX = sceneLight.transform.eulerAngles.x;
		lightRotY = sceneLight.transform.eulerAngles.y;
		
		//Quality
		qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
		
		//WarmUp
		Shader.WarmupAllShaders();
	}
	
	Vector3 lastMousePos;
	
	void SwitchRotation()
	{
		rotate = !rotate;
	}
	
	void Update ()
	{
		if(rotate)
		{
			rotateGroup.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
			rotY = rotateGroup.transform.eulerAngles.y;
		}
		
		float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheel != 0)
		{
			zoom -= scrollWheel;
			zoom = Mathf.Clamp(zoom, 1f, 3f);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, zoom);
		}
		
		if(Input.mousePosition.x < (Screen.width*0.8f) && Input.mousePosition.x > (Screen.width*0.2f) && Input.GetMouseButton(0))
		{
			Vector3 diff = lastMousePos - Input.mousePosition;
			Camera.main.transform.Translate(diff * Time.deltaTime * 0.2f);
		}
		
		lastMousePos = Input.mousePosition;
	}
	
	float zoom = 2f;
	float rotY = 0f;
	float lightRotX = 0f;
	float lightRotY = 0f;
	void OnGUI()
	{
		//Zoom
		zoom = GUI.VerticalSlider(new Rect(Screen.width - 24, 16, 10, 224), zoom, 1f, 3f);
		if(GUI.changed)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, zoom);
			GUI.changed = false;
		}
		
		//Rotation
		GUI.enabled = !rotate;
		rotY = GUI.HorizontalSlider(new Rect(16, 170, 128, 10), rotY, 0f, 360f);
		GUI.enabled = true;
		if(GUI.changed && !rotate)
		{
			rotateGroup.transform.eulerAngles = new Vector3(0,rotY,0);
			GUI.changed = false;
		}
		
		//Light
		lightRotY = GUI.HorizontalSlider(new Rect(16, 224, 128, 10), lightRotY, 0f, 360f);
		GUI.enabled = true;
		if(GUI.changed)
		{
			sceneLight.transform.eulerAngles = new Vector3(sceneLight.transform.eulerAngles.x,lightRotY,0);
			GUI.changed = false;
		}
		
		lightRotX = GUI.HorizontalSlider(new Rect(16, 244, 128, 10), lightRotX, -90f, 90f);
		GUI.enabled = true;
		if(GUI.changed)
		{
			sceneLight.transform.eulerAngles = new Vector3(lightRotX,sceneLight.transform.eulerAngles.y,0);
			GUI.changed = false;
		}
	}
	
	private bool bump = true;
	private bool spec = true;
	private bool outline = true;
	private bool outline_cst = false;
	private bool rim = false;
	
	void ReloadShader()
	{
		string strOutline = "Normal";
		if(outline)
			strOutline = outline_cst ? "OutlineConst" : "Outline";
		
		// Shader name
		string strShader = "Basic";
		if(bump && spec)
			strShader = "Bumped Specular";
		else if(spec)
			strShader = "Specular";
		else if(bump)
			strShader = "Bumped";
		
		if(rim)
			strShader += " Rim";
		
		
		string shader = "Toony Gooch Pro/" + strOutline + "/OneDirLight/" + strShader;
		Shader foundShader = FindShader(shader);
		if(foundShader == null)
		{
			Debug.LogError("SHADER NOT FOUND: " + shader);
			return;
		}
		
		// Outline Materials
		for(int i = 0; i < matsOutline.Length; i++)
			matsOutline[i].shader = foundShader;
		
		// No Outlines
		shader = "Toony Gooch Pro/Normal/OneDirLight/" + strShader;
		foundShader = FindShader(shader);
		if(foundShader == null)
		{
			Debug.LogError("SHADER NOT FOUND: " + shader);
			return;
		}
		
		for(int i = 0; i < matsSimple.Length; i++)
			matsSimple[i].shader = foundShader;
	}
	
	private Shader FindShader(string name)
	{
		foreach(Shader s in shaders)
		{
			if(s.name == name)
			{
				return s;
			}
		}
		
		Debug.LogError("SHADER NOT FOUND: " + name);
		return null;
	}
	
	public GUIT_Button subOutlines;
	
	void SwitchOutline()
	{
		outline = !outline;
		ReloadShader();
	}
	
	void SwitchOutlineCst()
	{
		outline_cst = !outline_cst;
		ReloadShader();
	}
	
	void SwitchSpec()
	{
		spec = !spec;
		ReloadShader();
	}
	
	void SwitchBump()
	{
		bump = !bump;
		ReloadShader();
	}
	
	void SwitchRim()
	{
		rim = !rim;
		ReloadShader();
	}
	
	void NextRamp()
	{
		rampIndex++;
		if(rampIndex >= rampTextures.Length)
			rampIndex = 0;
		
		UpdateRamp();
	}
	void PrevRamp()
	{
		rampIndex--;
		if(rampIndex < 0)
			rampIndex = rampTextures.Length-1;
		
		UpdateRamp();
	}
	void UpdateRamp()
	{
		rampUI.texture = rampTextures[rampIndex];
		foreach(Material m in matsAll)
		{
			m.SetTexture("_Ramp", rampTextures[rampIndex]);
		}
	}
	
	void NextQuality()
	{
		QualitySettings.IncreaseLevel(true);
		qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
	}
	void PrevQuality()
	{
		QualitySettings.DecreaseLevel(true);
		qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
	}
}
