using UnityEngine;
using System.Collections;

public class SetSunLight : MonoBehaviour {

	public Renderer lightwall;

	Material sky;

	/*
	public Renderer water;

	public Transform stars;
	public Transform worldProbe;
	*/
	// Use this for initialization
	void Start () 
	{

		sky = RenderSettings.skybox;

	}

	bool lighton = false;

	// Update is called once per frame
	void Update () 
	{

		//stars.transform.rotation = transform.rotation;
		/*
		if (Input.GetKeyDown(KeyCode.T))
	    {

			lighton = !lighton;

		}


		if (lighton)
		{
			Color final = Color.white * Mathf.LinearToGammaSpace(5);
			lightwall.material.SetColor("_EmissionColor", final);
			DynamicGI.SetEmissive(lightwall, final);
		}
		else
		{
			Color final = Color.white * Mathf.LinearToGammaSpace(0);
			lightwall.material.SetColor("_EmissionColor", final);
			DynamicGI.SetEmissive(lightwall, final);
		}
	
		Vector3 tvec = Camera.main.transform.position;

		worldProbe.transform.position = tvec;

		water.material.mainTextureOffset = new Vector2(Time.time / 100, 0);
		water.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 80));

		*/
	}
}
