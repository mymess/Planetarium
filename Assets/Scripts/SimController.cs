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

	public SunModel sun;
	public MoonModel moon;

	public MercuryModel mercury;
	public VenusModel venus;
	public MarsModel mars;
	public JupiterModel jupiter;
	public SaturnModel saturn;
	public UranusModel uranus;
	public NeptuneModel neptune;

	public List<PlanetModel> planets;



	public static SimController simController = null; 

	public float radius = 800.0f;

	private double lastJD;
	private double jd;

	public long year = 1987;
	public long month = 4;
	public long day = 10;

	public long hour = 10;
	public long minute = 0;
	public double sec = 0.0;

	public LocationData lastLocation;
	public LocationData location;



	void Awake () {
		
		if (simController == null) {
			simController = this;
		} 

		double dayDec = day + (double)hour / 24 + (double)minute / 60 + sec / 86400;; 
		jd = AASDate.DateToJD (year, month, dayDec, true);
		location = new LocationData (10.0, 10.0,100);


		skyModel = gameObject.GetComponent<SkyModel>();
		ParseStarsRaw ();
		ParseConstellations ();

		SetupSolarSystem ();

	}

	void Start(){
		

	}

	void Update () {
		
	}


	public bool IsUpdated(){
		return lastJD != jd || !lastLocation.Equals (location);
	}

	private void SetupSolarSystem(){
		sun = new SunModel (jd, location);
		moon = new MoonModel (jd, location);
		mercury = new MercuryModel (jd, location);
		venus = new VenusModel(jd, location);
		mars = new MarsModel (jd, location);
		jupiter = new JupiterModel (jd, location);
		uranus = new UranusModel(jd, location);
		neptune = new NeptuneModel(jd, location);

		planets = new List<PlanetModel> ();
		planets.Add (mercury);
		planets.Add (venus);
		planets.Add (mars);
		planets.Add (jupiter);
		planets.Add (saturn);
		planets.Add (uranus);
		planets.Add (neptune);
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

		skyModel.setStars (stars);

		skyModel.setReverseMapping (reverseMapping);
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


		skyModel.setConstellations (constellations);
	}


	public double GetJD(){
		return jd;
	}

	public LocationData GetLocation(){
		return location;
	}


}
