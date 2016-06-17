using UnityEngine;

using System;
using System.Collections;
using UnityEngine.UI;
using MathUtils;


public class CoordinatesHUD : MonoBehaviour {


	private Text raGUI;
	private Text declinationGUI;
	private Text azimuthGUI;
	private Text altitudeGUI;

	private Text fovGUI;

	public GameObject ra;
	public GameObject dec;
	public GameObject alt;
	public GameObject az;

	public GameObject fov;

	private Component[] textGUIs;

	private SkyModel skyModel;

	private SimController sim;

	private Color hudColor;


	// Use this for initialization
	void Start () {		
		raGUI = ra.GetComponent<Text> ();
		declinationGUI = dec.GetComponent<Text> ();
		azimuthGUI = az.GetComponent<Text> ();
		altitudeGUI = alt.GetComponent<Text> ();

		fovGUI = fov.GetComponent<Text> ();

		skyModel = SimController.INSTANCE.skyModel;
		sim = SimController.INSTANCE;
	}
	
	// Update is called once per frame
	void Update () {
		try{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = SimController.INSTANCE.radius;

			Vector3 pointingAt = Camera.main.ScreenToWorldPoint (mousePos);


			LocalCoords local = SkyModel.Rectangular2Horizontal ((double)pointingAt.x, (double)pointingAt.y, (double)pointingAt.z);

			EquatorialCoords equatorial = skyModel.Horizontal2Equatorial (
				local.Azimuth.Get(), 
				local.Altitude.Get()
			);

			double paralacticAngle = skyModel.GetMoon().GetParallacticAngle();

			if(Input.GetMouseButtonUp(0)){
				StarModel star = skyModel.FindStar(equatorial);
				Debug.Log("Clicked at: "  );
				Debug.Log("RA:" + equatorial.RA.Get());
				Debug.Log("Decl:" + equatorial.Declination.Get());
			}


			raGUI.text = "RA: " + equatorial.RA.ToString ();
			declinationGUI.text = "Dec: " + equatorial.Declination.ToString ();
			azimuthGUI.text = "Az: " + local.Azimuth.To0To360Range().ToString ();
			altitudeGUI.text = "Alt: " + local.Altitude.ToString ();
			fovGUI.text = "FOV: " + Camera.main.fieldOfView + "º";

			raGUI.color = declinationGUI.color = 
				azimuthGUI.color = altitudeGUI.color = 
					fovGUI.color = sim.Settings.MouseHudColor;


		}catch(NullReferenceException n){
			
		}

	}
}
