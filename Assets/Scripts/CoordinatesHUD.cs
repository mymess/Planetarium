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

	public GameObject ra;
	public GameObject dec;
	public GameObject alt;
	public GameObject az;

	private Component[] textGUIs;

	private SkyModel skyModel;

	// Use this for initialization
	void Start () {		
		raGUI = ra.GetComponent<Text> ();
		declinationGUI = dec.GetComponent<Text> ();
		azimuthGUI = az.GetComponent<Text> ();
		altitudeGUI = alt.GetComponent<Text> ();

		skyModel = SimController.instance.skyModel;
	}
	
	// Update is called once per frame
	void Update () {
		try{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = SimController.instance.radius;

			Vector3 pointingAt = Camera.main.ScreenToWorldPoint (mousePos);


			LocalCoords local = SkyModel.Rectangular2Horizontal ((double)pointingAt.x, (double)pointingAt.y, (double)pointingAt.z);

			EquatorialCoords equatorial = skyModel.Horizontal2Equatorial (
				local.Azimuth.Get(), 
				local.Altitude.Get()
			);

			
			raGUI.text = "RA: " + equatorial.RA.ToString ();
			declinationGUI.text = "Dec: " + equatorial.Declination.ToString ();
			azimuthGUI.text = "Az: " + local.Azimuth.To0To360Range().ToString ();
			altitudeGUI.text = "Alt: " + local.Altitude.ToString ();
		}catch(NullReferenceException n){
			
		}

	}
}
