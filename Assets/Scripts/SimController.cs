using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions; 

using System;
using System.Runtime.Serialization;
using System.IO;
using SimpleJSON;

using AASharp;
using MathUtils;


public sealed class SimController : MonoBehaviour{

	public SkyModel skyModel;

	public GameObject constellations;

	public Canvas mouseHud;

	private static SimController instance = null;
	public static SimController INSTANCE { get { return instance; }}


	public float radius = 800.0f;

	private decimal lastJD;
	private LocationSettings lastLocation;

	public DateTimeSettings dt;
	public DateTimeSettings DT
	{ 
		get { return dt; }
	}

	public LocationSettings location;
	public LocationSettings Location { 
		get { return location; } 
	}


	public GeneralSettings settings;
	public GeneralSettings Settings{ get{ return settings; }}


	public bool exaggeratedBodies = false;
	public float diametersScale = .4f;


	void Awake () {		
		if(instance == null){
			instance = this;
		}
		skyModel = new SkyModel((double)dt.JulianDay(), ToLocationData(location));
	}

	private LocationData ToLocationData(LocationSettings location){
		 return new LocationData (location.Longitude, location.Latitude, (double)location.Altitude);
	}


	void Start(){
		//stars
		ParseStarsRaw ();
		ParseConstellations ();

		lastJD       = dt.JulianDay ();
		lastLocation = location;

		//Log();
	}

	void Log(){
		Debug.Log ("------- SUN ------- ");
		Debug.Log (string.Format("JD: {0}", dt.JulianDay()));
		Debug.Log (string.Format("Time: {0}:{1}:{2}", dt.Hour(), dt.Minute(), dt.Second()));
		skyModel.GetSun ().Log ();
		HourAngle H = new HourAngle (skyModel.GetSun ().localHourAngle);
		Debug.Log (string.Format("H {0}", H.ToString() ));

		AAS2DCoordinate local = AASCoordinateTransformation.Equatorial2Horizontal (skyModel.GetSun ().localHourAngle, 
			skyModel.GetSun().equatorialCoords.Declination.Get(), 
			location.Latitude);


		Debug.Log (string.Format("local X {0}", local.X ));
		Debug.Log (string.Format("local Y {0}", local.Y ));


		Debug.Log ("------- VENUS ------- ");
		VenusModel venus = skyModel.GetPlanets () ["Venus"] as VenusModel;
		venus.Log ();


		Debug.Log ("------- MOON ------- ");
		MoonModel moon = skyModel.GetMoon();
		moon.Log ();

		Debug.Log ("------- JUPITER ------- ");
		JupiterModel jup = skyModel.GetPlanets()["Jupiter"] as JupiterModel;

		Debug.Log("p "+ jup.GetParallacticAngle());

	}


	void Update () {
		if (dt.playMode || IsTimeOrLocationUpdated ()) {
			if( skyModel!=null ){
				skyModel.Update ((double)dt.JulianDay (), ToLocationData (location));
			}
		} 

		lastLocation = location;
		lastJD = dt.JulianDay ();

		UpdateSettings ();
	}

	void UpdateSettings(){
		constellations.SetActive (settings.DisplayConstellations);
		ConstellationLinesRenderer lr = constellations.GetComponent<ConstellationLinesRenderer> ();
		if (settings.ConstellationSettingsChanged) {
			lr.Redraw ();
		}

		mouseHud.enabled = settings.ShowMouseHud;
		if(settings.MouseHudChanged){
			
		}


	}

	void LateUpdate(){		
		RotateSkyGlobe ();
		//UpdateSettings ();
	}
		

	private void UpdateDateAndTime(){
		AASDate date = new AASDate ((double)dt.JulianDay(), true);
	}
		

	private void RotateSkyGlobe(){
		//reset
		gameObject.transform.rotation = Quaternion.identity;

		//correction for latitude		
		double colatitude = 90.0d - location.Latitude;

		//topocentric vector of earth axis
		Vector3 earthAxis = skyModel.GetEarthAxis ();

		//correction for hour angle
		double hourAngleRotationInDegrees = skyModel.GetHourAngleOfAriesPoint () * 15d;

		gameObject.transform.Rotate (  earthAxis, (float)hourAngleRotationInDegrees);
		gameObject.transform.Rotate ((float) colatitude, 0.0f, 0.0f);
	}


	public bool IsTimeOrLocationUpdated(){	
		try{	
			return lastJD != dt.JulianDay() || !location.Equals (lastLocation);
		}catch(NullReferenceException n){
			Debug.Log ("NPE en SiMController");
			return false;	
		}

	}

	public bool IsLocationUpdated(){
		return !location.Equals (lastLocation);
	}
		


	/*
	public bool IsPlayMode(){
		return playMode;
	}


	public bool IsInspectorUpdated(){
		double dayDec = (double)day + (double)hour / 24d + (double)minute / 1440d + sec / 86400d; 
		jd = AASDate.DateToJD (year, month, dayDec, true);

		location.longitude = longitude;
		location.latitude  = latitude;
		location.altitude  = altitude;

		return lastJD != jd || !lastLocation.Equals (location);
	}



	public static bool IsReady(){
		return instance != null;
	}
	*/

	private void ParseStarsRaw(){
		
		TextAsset asset = Resources.Load("stars") as TextAsset;

		string fs = asset.text;
		string[] fLines = Regex.Split ( fs, "\n|\r|\r\n" );

		List<StarModel> stars = new List<StarModel> ();

		int[] reverseMapping = new int[fLines.Length]; 



		foreach(string line in fLines){
			string rawLine = line.Replace ("[", "").Replace ("],", "").Replace ("]", "").Replace("\"", "");
			string[] data = rawLine.Split (',');

			StarModel starmodel = new StarModel (data);

			try{
				reverseMapping [ starmodel.hip ] = starmodel.starID - 1;
			}catch(IndexOutOfRangeException i){
				//Debug.Log ("index --> "+ starmodel.hip);
			}

			stars.Add (starmodel);
		}

		skyModel.SetStars (stars);

		skyModel.SetReverseMapping (reverseMapping);

		skyModel.PopulateStarDictionary ();
	}


	private void ParseConstellations(){
		TextAsset asset = Resources.Load("constellations") as TextAsset;

		string fs = asset.text;

		var json = JSON.Parse (fs);


		List<Constellation> constellations = new List<Constellation> ();

		foreach (KeyValuePair<string, JSONNode> item in json.AsObject){
			Constellation newConstellation = new Constellation ();
			newConstellation.SetAbbr (item.Key);

			var constellation = JSON.Parse (item.Value.ToString());

			foreach (var line in constellation.AsArray) {
				var segment = JSON.Parse(line.ToString ()).AsArray;
				int[] newLine = new int[2]{segment[0].AsInt, segment[1].AsInt};
				newConstellation.AddLine (newLine);
			}
			constellations.Add(newConstellation);
		}


		skyModel.SetConstellations (constellations);
	}


	public decimal GetJD(){
		return dt.JulianDay();
	}

	public LocationSettings GetLocation(){
		return location;
	}
		

}
