using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;


[Serializable]
public class Star{

	public static int StarID = 0;
	public static int HIP = 1;
	public static int HD = 2;
	public static int HR  = 3;
	public static int Gliese = 4;
	public static int BayerFlamsteed = 5;
	public static int ProperName = 6;
	public static int RA = 7;
	public static int Dec = 8;
	public static int Distance = 9;
	public static int PMRA = 10;
	public static int PMDec = 11;
	public static int RV = 12;
	public static int Mag = 13;
	public static int AbsMag = 14;
	public static int Spectrum = 15;
	public static int ColorIndex = 16;
	public static int X = 17;
	public static int Y = 18;
	public static int Z = 19;
	public static int VX = 20;
	public static int VY = 21;
	public static int VZ = 22;
}



//["25", "25", "224750", "9077", "", "", "", "0.00529102", "-44.29029741", "72.7802037845706", "58.36", "-108.64", "3", "6.28", "1.96993366361766", "G3IV", "0.763", "52.09682", "0.07216", "-50.82198", "-2.4597e-05", "2.0556e-05", "-2.9579e-05"],
public class StarModel{
	public int starID;
	public int hip;
	public int? hd;
	public Int32? hr;
	public string gliese;
	public string bayerFlamsteed;
	public string properName;
	public float ra;
	public float dec;
	public double? distance;
	public double? pmra;
	public double? pmdecc;
	public double? rv;
	public float mag;
	public float absMag;
	public string spectrum;
	public float colorIndex;
	public double? x;
	public double? y;
	public double? z;
	public double? vx;
	public double? vy;
	public double? vz;

	public StarModel(string[] data){
		try{
			int len = data.Length;
			int.TryParse(data [Star.StarID], out starID);
			int.TryParse( data [Star.HIP], out hip ) ;
			float.TryParse(data [Star.RA], out ra);
			float.TryParse(data [Star.Dec], out dec);
			float.TryParse(data [Star.Mag], out mag );
			float.TryParse(data [Star.ColorIndex], out colorIndex);
			float.TryParse(data [Star.AbsMag], out absMag ) ;
			spectrum = Star.Spectrum<len? data [Star.Spectrum] :null ;

		}catch(FormatException pe){
			Debug.Log (string.Format("EXCEPTION: {0}", pe.ToString()));
		}
	}

	public float getClampedMagnitude(){
		return 2.7f/(mag + 5.4f);
	}

	public void Log(){
		Debug.Log (string.Format("StardId {0} {1} {2} {3} {4} {5} {6}", starID, hip, ra, dec, mag, absMag, spectrum));
	}

	public Vector3 GetNormalizedPosition(){
		float x = Mathf.Cos(dec *Mathf.Deg2Rad) * Mathf.Sin(ra*15.0f *Mathf.Deg2Rad);
		float y = Mathf.Sin(dec *Mathf.Deg2Rad);
		float z = -Mathf.Cos(dec *Mathf.Deg2Rad) * Mathf.Cos(ra*15.0f *Mathf.Deg2Rad);
		return new Vector3 (x, y, z);
	}

	public Color GetStarRGB(){
		//RED
		// y = -0,0921x5 + 0,3731x4 - 0,3497x3 - 0,285x2 + 0,5327x + 0,8217            
		float red = -.0921f*Mathf.Pow(colorIndex, 5.0f ) + .3731f*Mathf.Pow(colorIndex, 4.0f) - .3497f*Mathf.Pow(colorIndex,3.0f) 
		- .285f*Mathf.Pow(colorIndex, 2.0f) + .5327f*colorIndex + .8217f;            
		if (red>1.0f) {
			red = 1.0f;
		}

		//GREEN
		//y = -0,1054x6 + 0,229x5 + 0,1235x4 - 0,3529x3 - 0,2605x2 + 0,398x + 0,8626
		float green = -.1054f*Mathf.Pow(colorIndex, 6.0f) + .229f*Mathf.Pow(colorIndex, 5.0f ) + .1235f*Mathf.Pow(colorIndex, 4.0f) 
			- .3529f*Mathf.Pow(colorIndex, 3.0f) - .2605f * Mathf.Pow(colorIndex, 2.0f ) + .398f*colorIndex + .8626f;           

		//BLUE
		float blue = 0.0f;
		//for the interval [-0.40, 0.40]
		//y = 1.0f
		//for the interval (0.40,  1.85]
		//y = -1,9366x6 + 12,037x5 - 30,267x4 + 39,134x3 - 27,148x2 + 9,0945x - 0,1475
		//for the interval (1,85-2.0]
		//y = 0.0f
		if( colorIndex <= .40f){
			blue = 1.0f;
		}
		if( colorIndex>.40 && colorIndex<=1.85f){
			blue = -1.9366f*Mathf.Pow(colorIndex, 6.0f) + 12.037f*Mathf.Pow(colorIndex, 5.0f) - 30.267f*Mathf.Pow(colorIndex, 4.0f)
				+ 39.134f * Mathf.Pow(colorIndex, 3.0f) -27.148f*Mathf.Pow(colorIndex, 2.0f) + 9.0945f*colorIndex - .1475f;
		}
		if( colorIndex>1.85f ){
			blue = 0.0f;                
		}

		return new Color(red, green, blue);
	}
}


public class Constellation{

	private List<int[]> lines;

	private string name;

	private string abbr;


	public Constellation(){
		lines = new List<int[]> ();
	}

	public void AddLine(int[] line){
		lines.Add (line);
	}

	public void SetName(string name){
		this.name = name;
	}

	public void SetAbbr(string abbr){
		this.abbr = abbr;
	}

	public string GetAbbr(){
		return abbr;
	}



	public List<int[]> GetLines(){
		return lines;
	}

	public void Log(){
		string s = string.Format("{0}: [", abbr);
		foreach(int[] line in lines){			
			s += string.Format ("[{0},{1}], ", line [0], line [1]);
		}
		s += "]";

		Debug.Log(s);
	}

}

[Serializable]
public class SkyModel : MonoBehaviour {

	private List<StarModel> stars;// { get; set;}

	//reverse Mapping HIP index -> array index (used in constellations rendering)
	private int[] reverseMapping;

	private List<Constellation> constellations;// { get; set;} 

	private Dictionary<string, PlanetModel> planets;

	private SunModel sun;

	private MoonModel moon;


	public List<StarModel> GetStars(){ return stars; }
	public void SetStars(List<StarModel> stars){ this.stars = stars; }

	public int[] GetReverseMapping(){ return reverseMapping; }
	public void SetReverseMapping(int[] reverseMapping){ this.reverseMapping = reverseMapping; }

	public List<Constellation> GetConstellations(){ return constellations; }
	public void SetConstellations(List<Constellation> constellations){ this.constellations = constellations; }

	public Dictionary<string, PlanetModel>  GetPlanets(){ return planets; }
	public void SetPlanets(Dictionary<string, PlanetModel>  planets){ this.planets = planets; }


	public SunModel GetSun(){ return sun; }
	public void SetSun(SunModel sun){ this.sun = sun; }

	public MoonModel GetMoon(){ return moon; }
	public void SetMoon(MoonModel moon){ this.moon = moon; }
}
