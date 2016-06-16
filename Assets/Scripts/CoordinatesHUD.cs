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

	private Text parGUI;

	public GameObject ra;
	public GameObject dec;
	public GameObject alt;
	public GameObject az;

	public GameObject par;

	private Component[] textGUIs;

	private SkyModel skyModel;

	// Use this for initialization
	void Start () {		
		raGUI = ra.GetComponent<Text> ();
		declinationGUI = dec.GetComponent<Text> ();
		azimuthGUI = az.GetComponent<Text> ();
		altitudeGUI = alt.GetComponent<Text> ();

		parGUI = par.GetComponent<Text> ();

		skyModel = SimController.INSTANCE.skyModel;
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

				if(star != null){
					parGUI.text = "Hipparcos index: " + star.hip;
					Debug.Log("Hipparcos index: " + star.hip);
				}else{
					Debug.Log("nothing found");
				}
			}


			raGUI.text = "RA: " + equatorial.RA.ToString ();
			declinationGUI.text = "Dec: " + equatorial.Declination.ToString ();
			azimuthGUI.text = "Az: " + local.Azimuth.To0To360Range().ToString ();
			altitudeGUI.text = "Alt: " + local.Altitude.ToString ();
			//parGUI.text = "Moon q: " + paralacticAngle;




		}catch(NullReferenceException n){
			
		}

	}
}
