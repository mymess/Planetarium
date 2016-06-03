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


public class SimController : MonoBehaviour{

	public SkyModel skyModel;

	//public Dictionary<string, PlanetModel> planets;

	public static SimController instance = null; 

	public float radius = 800.0f;

	private double lastJD;
	private double jd;

	public long year = 1987;
	public long month = 4;
	public long day = 10;

	public long hour = 10;
	public long minute = 0;
	public double sec = 0.0;

	public double longitude = 0.0;
	public double latitude = 40.0;
	public double altitude = 100;

	private LocationData lastLocation;
	public LocationData location;

	public bool playMode = false;
	private bool lastPlayMode;
	public float timeScale = 1500f;


	void Awake () {
		
		if (instance == null) {
			instance = this;
		} 

		UpdateJD ();
		UpdateLocation ();
		UpdateLastLocation ();

		lastJD = jd;

		lastPlayMode = playMode;

		skyModel = new SkyModel(jd, location);

		//stars
		ParseStarsRaw ();
		ParseConstellations ();

	}

	///2446895.91666667

	void Start(){

		Debug.Log (string.Format("JD: {0}", jd));
		Debug.Log (string.Format("Time: {0}:{1}:{2}", hour, minute, sec));
		skyModel.GetSun ().Log ();
		HourAngle H = new HourAngle (skyModel.GetSun ().localHourAngle);
		Debug.Log (string.Format("H {0}", H.ToString() ));

		AAS2DCoordinate local = AASCoordinateTransformation.Equatorial2Horizontal (skyModel.GetSun ().localHourAngle, 
			skyModel.GetSun().equatorialCoords.Declination.Get(), 
			location.latitude);


		Debug.Log (string.Format("local X {0}", local.X ));
		Debug.Log (string.Format("local Y {0}", local.Y ));
	}

	void Update () {		
		if (playMode) {
			jd += timeScale * Time.deltaTime / 86400f;
			UpdateLocation ();
			UpdateDateAndTime ();

			skyModel.Update (jd, location);
			//RotateSkyGlobe ();

		} else {			
			if (!IsPlayModeToggled ()){
			//if (IsTimeOrLocationUpdated ()) {				
				UpdateLocation ();
				UpdateJD ();	
				skyModel.Update (jd, location);
				//RotateSkyGlobe ();
				lastPlayMode = playMode;
			}				
			//}
		}			

		lastJD = jd;

		UpdateLastLocation ();
	}

	void LateUpdate(){
		RotateSkyGlobe ();
	}

	private bool IsPlayModeToggled(){
		return playMode != lastPlayMode;
	}


	private void UpdateDateAndTime(){
		AASDate date = new AASDate (jd, true);
		year = date.Year;
		month = date.Month;
		day = date.Day;
		hour = date.Hour;
		minute = date.Minute;
		sec = date.Second;

	}
		

	private void RotateSkyGlobe(){
		//reset
		gameObject.transform.rotation = Quaternion.identity;

		//correction for latitude		
		double colatitude = 90.0d - location.latitude;

		//topocentric vector of earth axis
		Vector3 earthAxis = skyModel.GetEarthAxis ();

		//correction for hour angle
		double hourAngleRotationInDegrees = skyModel.GetHourAngleOfAriesPoint () * 15d;

		gameObject.transform.Rotate (  earthAxis, (float)hourAngleRotationInDegrees);
		gameObject.transform.Rotate ((float) colatitude, 0.0f, 0.0f);
	}

	public bool IsTimeOrLocationUpdated(){	
		try{	
			return lastJD != jd || !location.Equals (lastLocation);
		}catch(NullReferenceException n){
			Debug.Log ("NPE en SiMController");
			return false;	
		}

	}

	public bool IsLocationUpdated(){
		return !location.Equals (lastLocation);
	}

	private void UpdateJD(){
		double dayDec = day + (double)hour / 24 + (double)minute / 60 + sec / 86400;; 
		jd = AASDate.DateToJD (year, month, dayDec, true);
	}

	private void UpdateLocation(){
		location = new LocationData (longitude, latitude, altitude);
	}

	private void UpdateLastLocation(){
		lastLocation = new LocationData (longitude, latitude, altitude);
	}

	public bool IsPlayMode(){
		return playMode;
	}
	public bool IsInspectorUpdated(){
		double dayDec = day + (double)hour / 24 + (double)minute / 60 + sec / 86400;; 
		jd = AASDate.DateToJD (year, month, dayDec, true);

		location.longitude = longitude;
		location.latitude  = latitude;
		location.altitude  = altitude;

		return lastJD != jd || !lastLocation.Equals (location);
	}



	public static bool IsReady(){
		return instance != null;
	}


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


	public double GetJD(){
		return jd;
	}

	public LocationData GetLocation(){
		return location;
	}
		

}
