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
	public double latitude = 90.0;
	public double altitude = 100;

	private LocationData lastLocation;
	public LocationData location;

	public bool playMode = true;
	public float timeScale = 500f;


	void Awake () {
		
		if (instance == null) {
			instance = this;
		} 

		double dayDec = day + (double)hour / 24 + (double)minute / 60 + sec / 86400;; 
		jd = AASDate.DateToJD (year, month, dayDec, true);
		lastJD = jd;
		location = new LocationData (longitude, latitude, altitude);
		lastLocation = new LocationData (longitude, latitude, altitude);

		skyModel = new SkyModel(jd, location);

		//stars
		ParseStarsRaw ();
		ParseConstellations ();



	}

	void Start(){
		
		UpdateJD ();
		UpdateLocation ();

		skyModel.Update (jd, location);

		RotateSkyGlobe ();

	}

	void Update () {		
		if (playMode) {
			jd += timeScale * Time.deltaTime / 86400f;
			UpdateLocation ();

			skyModel.Update (jd, location);

			RotateSkyGlobe ();

		} else {			
			//if (IsTimeOrLocationUpdated ()) {
				UpdateJD ();
				UpdateLocation ();

				skyModel.Update (jd, location);

				RotateSkyGlobe ();

				
			//}
		}			

		lastJD = jd;
		UpdateLastLocation ();
	}


	private void RotateSkyGlobe(){
		//reset
		gameObject.transform.rotation = Quaternion.identity;

		//correction for latitude		
		double colatitude = 90.0d - location.latitude;

		//topocentric vector of earth axis
		Vector3 earthAxis = skyModel.GetEarthAxis ();

		Debug.Log ("earthAxis --> "+earthAxis.ToString());

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
		location = new LocationData (longitude, latitude, altitude);
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
