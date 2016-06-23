using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

using AASharp;
using MathUtils;

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
public class StarModel : CelestialBody{
	public int starID;
	public int hip;
	public int? hd;
	public Int32? hr;
	public string gliese;
	public string bayerFlamsteed;
	public string properName;
	public double ra;
	public double dec;
	public double distance;
	public double pmra;
	public double pmdecc;
	public double rv;
	public float mag;
	public float absMag;
	public string spectrum;
	public float colorIndex;
	public double x;
	public double y;
	public double z;
	public double vx;
	public double vy;
	public double vz;

	public StarModel(string[] data){
		try{
			int len = data.Length;
			int.TryParse(data [Star.StarID], out starID);
			int.TryParse( data [Star.HIP], out hip ) ;
			double.TryParse(data [Star.RA], out ra);
			double.TryParse(data [Star.Dec], out dec);
			float.TryParse(data [Star.Mag], out mag );
			float.TryParse(data [Star.ColorIndex], out colorIndex);
			float.TryParse(data [Star.AbsMag], out absMag ) ;
			double.TryParse(data[Star.Distance], out distance);
			bayerFlamsteed = data[Star.BayerFlamsteed];
			properName = data[Star.ProperName];
			gliese = data[Star.Gliese];
			float.TryParse(data[Star.ColorIndex], out colorIndex);

			spectrum = data[Star.Spectrum];

			equatorialCoords = new EquatorialCoords(new HourAngle(ra), new DegreesAngle(dec));

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

	public override string ToString ()
	{
		return string.Format ("Star");
	}

	public Vector3 GetEquatorialRectangularCoords(){
		/*
		double x = Math.Cos(dec * M.DEG2RAD) * Math.Sin(ra*15.0d *M.DEG2RAD);
		double y = Math.Sin(dec * M.DEG2RAD);
		double z = -Math.Cos(dec * M.DEG2RAD) * Math.Cos(ra*15.0d *M.DEG2RAD);
		*/
		float x = Mathf.Cos((float)dec * Mathf.Deg2Rad) * Mathf.Sin((float)ra*15.0f *Mathf.Deg2Rad);
		float y = Mathf.Sin((float)dec * Mathf.Deg2Rad);
		float z = -Mathf.Cos((float)dec * Mathf.Deg2Rad) * Mathf.Cos((float)ra*15.0f *Mathf.Deg2Rad);
		//return new Vector3 ((float)x, (float) y, (float) z);
		return new Vector3 (x, y, z);
	}
	/*
	public LocalCoords LocalCoords(double jd, LocationData location){
		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		double H = theta0Apparent - location.longitude/15d - ra;

		AAS2DCoordinate localCoords = AASCoordinateTransformation.Equatorial2Horizontal (H, dec, location.latitude);
		double azimuth = AASCoordinateTransformation.MapTo0To360Range (localCoords.X + 180d);

		return new LocalCoords(new DegreesAngle(azimuth), new DegreesAngle(localCoords.Y));
	}*/

	public Vector3 GetLocalRectangularCoordinates(double jd, LocationData location){
		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		double H = theta0Apparent - location.longitude/15d - this.ra;

		AAS2DCoordinate local = AASCoordinateTransformation.Equatorial2Horizontal (H, this.dec, location.latitude);
		double az = local.X;
		double alt = local.Y;
		double x = Math.Cos(alt * M.DEG2RAD) * Math.Sin(az*15.0f *M.DEG2RAD);
		double y = Math.Sin(alt * M.DEG2RAD);
		double z = -Math.Cos(alt * M.DEG2RAD) * Math.Cos(az*15.0f *M.DEG2RAD);

		return new Vector3( (float)x, (float)y, (float)z );
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


	public void Update(double jd, LocationData location){
		this.jd = jd;
		this.location = location;

		this.localCoords = GetLocalCoords ();
	}

	public override string GetBodyDetails(){
		string s = string.Format("Type: STAR\n");

		if (!string.IsNullOrEmpty (properName.Trim())) {
			s += string.Format("Name: {0}\n", properName);
		}
		s += string.Format("Hipparcos catalogue: {0}\n", hip);

		if(!string.IsNullOrEmpty (bayerFlamsteed.Trim())){
			s += string.Format("Bayer-Flamsteed catalogue: {0}\n", bayerFlamsteed);
		}
		if (!string.IsNullOrEmpty (gliese.Trim())) {
			s += string.Format("Gliese catalogue: {0}\n", gliese );
		}


		s += string.Format("RA/Dec: {0} / {1}\n", equatorialCoords.RA.ToString(), equatorialCoords.Declination.ToString());

		this.localCoords = GetLocalCoords ();
		s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
		s += string.Format("Spectrum: {0}\n", spectrum );
		s += string.Format("Color index: {0}\n", colorIndex );
		s += string.Format("Distance: {0} light-years\n", distance.ToString("##.000") );

		return s;
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

