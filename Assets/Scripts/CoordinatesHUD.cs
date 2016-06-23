using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using MathUtils;


public class CoordinatesHUD : MonoBehaviour {


	private Text raDecGUI;
	private Text azimuthAltGUI;
	private Text fovGUI;
	private Text detailsGUI;

	public GameObject crosshair;
	public CrossHairRenderer crosshairRenderer;
	private float crosshairRadius;
	private float crosshairSize;

	public GameObject raDec;
	public GameObject azAlt;
	public GameObject fov;
	public GameObject details;

	private CelestialBody selectedBody;

	private SkyModel skyModel;

	private SimController sim;

	private Color hudColor;


	void Start () {		
		raDecGUI = raDec.GetComponent<Text> ();
		azimuthAltGUI = azAlt.GetComponent<Text> ();	
		fovGUI = fov.GetComponent<Text> ();
		detailsGUI = details.GetComponent<Text> ();

		skyModel = SimController.INSTANCE.skyModel;
		sim = SimController.INSTANCE;
		raDecGUI.color = azimuthAltGUI.color = fovGUI.color = sim.Settings.MouseHudColor;

		detailsGUI.text = "";
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


			DisplaySelectedBodyDetailsIfAny(equatorial);


			raDecGUI.text = string.Format("RA/Dec: {0} / {1}", equatorial.RA.ToString (), equatorial.Declination.ToString());

			azimuthAltGUI.text = string.Format("Az/Alt: {0} / {1}", local.Azimuth.To0To360Range().ToString (), local.Altitude.ToString());

			fovGUI.text = "FOV: " + Camera.main.fieldOfView + "º";

			raDecGUI.color = azimuthAltGUI.color = fovGUI.color = sim.Settings.MouseHudColor;
			

		}catch(NullReferenceException n){
			Debug.Log (n);
		}

	}


	public void SetHudText(string raDec, string azAlt, string fov){
		raDecGUI.text = raDec;
		azimuthAltGUI.text = azAlt;
		fovGUI.text = fov;
	}


	public void SetBodyDetailsText(CelestialBody body){
		this.selectedBody = body;
		if( body != null ){
			DisplayBodyDetailsByType ();
		}
	}

	void DisplaySelectedBodyDetailsIfAny(EquatorialCoords equatorial){
		if (Input.GetMouseButtonUp (0)) {
			//is it a solar system body?
			if (CheckSolarSystemBodyHit ()) {
				DisplayBodyDetailsByType ();
				DisplayCrossHair();
			} else {
				//is it a star?
				selectedBody = skyModel.FindStar(equatorial);
				if (selectedBody != null) {		
					StarModel star = (StarModel)selectedBody;
					crosshairRadius = 5f;
					crosshairSize = 50 / (3 + star.mag);

					DisplayBodyDetailsByType ();
					DisplayCrossHair ();
				} else {
					crosshair.SetActive (false);
					detailsGUI.text = "";
				}
			}
		}

		if (selectedBody != null) {			
			DisplayBodyDetailsByType ();
			DisplayCrossHair();
		}

	}

	private bool CheckSolarSystemBodyHit(){		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		bool ret = false;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider != null) {
				ret = true;
				string bodyTag = hit.transform.tag;

				if (bodyTag.Equals ("Sun")) {
					selectedBody = skyModel.GetSun();
				} else if (bodyTag.Equals ("Moon")) {
					selectedBody = skyModel.GetMoon ();
				} else if (bodyTag.Equals ("Mercury")) {
					selectedBody = skyModel.GetPlanets () ["Mercury"];
				} else if (bodyTag.Equals ("Venus")) {
					selectedBody = skyModel.GetPlanets () ["Venus"];
				} else if (bodyTag.Equals ("Mars")) {
					selectedBody = skyModel.GetPlanets () ["Mars"];
				} else if (bodyTag.Equals ("Jupiter")) {
					selectedBody = skyModel.GetPlanets () ["Jupiter"];
				}else if (bodyTag.Equals ("Saturn")) {
					selectedBody = skyModel.GetPlanets () ["Saturn"];
				}else if (bodyTag.Equals ("Uranus")) {
					selectedBody = skyModel.GetPlanets () ["Uranus"];
				}else if (bodyTag.Equals ("Neptune")) {
					selectedBody = skyModel.GetPlanets () ["Neptune"];
				}

				crosshairRadius = hit.collider.transform.localScale.magnitude;
				crosshairSize = hit.collider.transform.localScale.magnitude;


			}
		}

		return ret;
	}



	private void DisplayCrossHair(){
		crosshair.SetActive (true);
		EquatorialCoords eq = GetSelectedBodyEqCoords ();
		Vec3D rectangular = eq.ToRectangular ();
		crosshair.transform.localPosition = new Vector3((float)rectangular.x, (float)rectangular.y, -(float)rectangular.z) * sim.radius;


		float fov = Camera.main.fieldOfView;
		crosshairRenderer.scale = 0.03f*fov*fov+5f;
		crosshairRenderer.radius = crosshairRadius;
		crosshairRenderer.sizeOnScreen = crosshairSize;
	}


	private void DisplayBodyDetailsByType(){
		if(selectedBody != null){
			if (selectedBody is StarModel) {
				StarModel star = (StarModel)selectedBody;
				star.Update ((double)sim.dt.JulianDay (), sim.GetLocationData ());
				detailsGUI.text = star.GetBodyDetails ();

			} else {
				detailsGUI.text = selectedBody.GetBodyDetails ();
			}
		}	
	}

	private EquatorialCoords GetSelectedBodyEqCoords(){		
		return selectedBody.EquatorialCoords;
	}

	private LocalCoords GetSelectedBodyLocalCoords(){
		if (selectedBody is StarModel) {			
			StarModel star = (StarModel)selectedBody;
			star.Update ((double)sim.GetJD (), sim.GetLocationData ());
			return star.GetLocalCoords ();
		}
		return selectedBody.GetLocalCoords ();
	}


	/*
	private string GetMoonDetails(CelestialBody body){
		MoonModel moon = null;
		string s = "";

		try{
			moon = body as MoonModel;

			s = string.Format("{0}\n", "MOON" );
			//s += string.Format("Type: {0}\n", "Moon" );
			s += string.Format("RA/Dec: {0} / {1}\n", moon.EquatorialCoords.RA.ToString(), moon.EquatorialCoords.Declination.ToString());

			LocalCoords localCoords = skyModel.Equatorial2Horizontal (moon.equatorialCoords.RA.Get (), moon.equatorialCoords.Declination.Get ());

			s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
			s += string.Format("Distance: {0} km\n", moon.GetDistance() );

		}catch(InvalidCastException ie){
			Debug.Log ("Invalid cast. " + ie.ToString());
		}catch(Exception e){
			Debug.Log ("Exception" +  e);
		}

		return s;
	}

	private string GetSunDetails(CelestialBody body){
		SunModel sun = null;
		string s = "";

		try{
			sun = body as SunModel;
			s = string.Format("{0}\n", "SUN" );
			s += string.Format("Type: {0}\n", "Star" );
			s += string.Format("RA/Dec: {0} / {1}\n", sun.equatorialCoords.RA.ToString(), sun.equatorialCoords.Declination.ToString());

			LocalCoords localCoords = skyModel.Equatorial2Horizontal (sun.equatorialCoords.RA.Get (), sun.equatorialCoords.Declination.Get ());

			s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
			s += string.Format("Distance: {0} AU\n", sun.GetDistance() );

		}catch(InvalidCastException ie){
			Debug.Log ("Invalid cast. " + ie.ToString());
		}catch(Exception e){
			Debug.Log ("Exception" +  e);
		}


		return s;
	}

	private string GetPlanetDetails(CelestialBody body){
		PlanetModel planet = null;
		string s = "";

		try{
			planet = body as PlanetModel;

			s = string.Format("{0}\n", planet.GetName().ToUpper() );
			s += string.Format("Type: {0}\n", "Planet" );
			s += string.Format("RA/Dec: {0} / {1}\n", planet.equatorialCoords.RA.ToString(), planet.equatorialCoords.Declination.ToString());

			LocalCoords localCoords = skyModel.Equatorial2Horizontal (planet.equatorialCoords.RA.Get (), planet.equatorialCoords.Declination.Get ());

			s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
			s += string.Format("Distance: {0} AU\n", planet.GetDistance() );


		}catch(InvalidCastException ie){
			Debug.Log ("Invalid cast. " + ie.ToString());
		}catch(Exception e){
			Debug.Log ("Exception" +  e);
		}

		return s;		
	}



	private string GetStarDetails(CelestialBody body){
		StarModel star = null;
		string s = "";

		try{
			star = body as StarModel;

			s = string.Format("Type: STAR\n");

			if (!string.IsNullOrEmpty (star.properName.Trim())) {
				s += string.Format("Name: {0}\n", star.properName);
			}
			s += string.Format("Hipparcos catalogue: {0}\n", star.hip);

			if(!string.IsNullOrEmpty (star.bayerFlamsteed.Trim())){
				s += string.Format("Bayer-Flamsteed catalogue: {0}\n", star.bayerFlamsteed);
			}
			if (!string.IsNullOrEmpty (star.gliese.Trim())) {
				s += string.Format("Gliese catalogue: {0}\n", star.gliese );
			}


			s += string.Format("RA/Dec: {0} / {1}\n", new HourAngle((double)star.ra).ToString(), new DegreesAngle((double)star.dec).ToString());

			LocalCoords localCoords = skyModel.Equatorial2Horizontal ((double)star.ra, (double)star.dec);

			s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
			s += string.Format("Spectrum: {0}\n", star.spectrum );
			s += string.Format("Color index: {0}\n", star.colorIndex );
			s += string.Format("Distance: {0} light-years\n", star.distance.ToString("##.000") );

		}catch(InvalidCastException ie){
			Debug.Log ("Invalid cast. " + ie.ToString());
		}catch(Exception e){
			Debug.Log ("Exception" +  e);
		}
	
		return s;
	}

	*/


}
