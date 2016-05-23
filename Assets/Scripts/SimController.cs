using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions; 

using System;
using System.Runtime.Serialization;
using System.IO;
using SimpleJSON;





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

	public static SimController simController = null; 

	public float radius = 800.0f;

	void Awake () {
		
		if (simController == null) {
			simController = this;
		} 
		skyModel = gameObject.GetComponent<SkyModel>();

		ParseStarsRaw ();


		ParseConstellations ();
	}
	

	void Update () {
		
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
				Debug.Log ("index --> "+ starmodel.hip);
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




}
