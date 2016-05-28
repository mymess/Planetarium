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


public static class Utils{
	public static int? toNullifiableInt32(string s){
		int i;
		if (Int32.TryParse(s, out i)) return i;
		return null;
	}

	public static double? toNullifiableDouble(string s){
		double f;
		if (Double.TryParse (s, out f))
			return f;
		return null;
	}

}



public class SimController : MonoBehaviour{

	public SkyModel skyModel;

	public Dictionary<string, PlanetModel> planets;

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

	public double longitude = 40.0;
	public double latitude = 0;

	public LocationData lastLocation;
	public LocationData location;



	void Awake () {
		
		if (instance == null) {
			instance = this;
		} 

		double dayDec = day + (double)hour / 24 + (double)minute / 60 + sec / 86400;; 
		jd = AASDate.DateToJD (year, month, dayDec, true);
		location = new LocationData (longitude, latitude, 100);
		lastLocation = new LocationData (longitude, latitude, 100);

		skyModel = gameObject.GetComponent<SkyModel>();

		SetupSolarSystem ();

		ParseStarsRaw ();
		ParseConstellations ();



	}

	void Start(){
		

	}

	void Update () {
		
	}
		

	public bool IsUpdated(){
		return false; //lastJD != jd || !lastLocation.Equals (location);
	}

	private void SetupSolarSystem(){
		SunModel sun              = new SunModel (jd, location);
		MoonModel moon        	  = new MoonModel (jd, location);
		MercuryModel mercuryModel = new MercuryModel (jd, location);
		VenusModel venusModel     = new VenusModel(jd, location);
		MarsModel marsModel       = new MarsModel (jd, location);
		JupiterModel jupiterModel = new JupiterModel (jd, location);
		SaturnModel saturnModel   = new SaturnModel (jd, location);
		UranusModel uranusModel   = new UranusModel(jd, location);
		NeptuneModel neptuneModel = new NeptuneModel(jd, location);

		planets = new Dictionary<string, PlanetModel> ();
		planets["Mercury"] = mercuryModel;
		planets["Venus"]   = venusModel;
		planets["Mars"]    = marsModel;
		planets["Jupiter"] = jupiterModel;
		planets["Saturn"]  = saturnModel;
		planets["Uranus"]  = uranusModel;
		planets["Neptune"] = neptuneModel;

		skyModel.SetPlanets (planets);
		skyModel.SetSun (sun);
		skyModel.SetMoon (moon);

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
