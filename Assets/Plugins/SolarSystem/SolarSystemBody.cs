using UnityEngine;
using System.Collections;
using System;

using AASharp;
using MathUtils;

public enum SolarSystemBodyType{ MERCURY, VENUS, MARS, JUPITER, SATURN, URANUS, NEPTUNE, MOON, SUN};

public abstract class CelestialBody{
	protected int starindex;

	public int Index{get{return starindex; } set{ starindex = value;} }

}


public abstract class SolarSystemBody : CelestialBody {
	

	public abstract string GetName ();

	//Calculations are to be performed here
	protected abstract void Update ();

	protected abstract double GetRadiusVector(double JD);
	public abstract double GetDistance();

	public abstract double GetSemidiameter ();

	//JD
	protected double jd;

	//observers location
	protected LocationData location;
	protected Vec3D vectorToEarth;
	protected Vec3D vectorToEarthCorrected;


	public EquatorialCoords equatorialCoords = new EquatorialCoords();
	public LocalCoords localCoords = new LocalCoords();

	public double localHourAngle;

	public SolarSystemBody(double jd, LocationData location){
		this.jd = jd;
		this.location = location;
	}


	public void Update(double jd, LocationData location){
		this.jd = jd;
		this.location = location;
		Update ();
	}




	public Vec3D GetRectangularLocalPosition(){
		double az  = localCoords.Azimuth.Get ();
		double alt = localCoords.Altitude.Get ();

		Vec3D ret = new Vec3D ();
		ret.x = Math.Cos(alt*M.DEG2RAD)*Math.Sin (az * M.DEG2RAD);
		ret.y = Math.Sin (alt * M.DEG2RAD);
		ret.z = Math.Cos (alt * M.DEG2RAD) * Math.Cos(az*M.DEG2RAD);

		return ret;


		//return Vec3D.PolarToRectangular(az, alt);
	}


	public Vec3D GetRectangularFromEquatorialCoords(){
		double ra  = equatorialCoords.RA.Get() * 15d;
		double dec = equatorialCoords.Declination.Get();

		float x = Mathf.Cos((float)dec * Mathf.Deg2Rad) * Mathf.Sin((float)ra *Mathf.Deg2Rad);
		float y = Mathf.Sin((float)dec * Mathf.Deg2Rad);
		float z = -Mathf.Cos((float)dec * Mathf.Deg2Rad) * Mathf.Cos((float)ra *Mathf.Deg2Rad);


		return new Vec3D (x, y, z);
	}

	protected void CalculateTopocentricPosition (){

		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		localHourAngle = theta0Apparent - location.longitude/15d - equatorialCoords.RA.Get();

		AAS2DCoordinate local = AASCoordinateTransformation.Equatorial2Horizontal (localHourAngle, equatorialCoords.Declination.Get(), location.latitude);


		localCoords.Azimuth  = DegreesAngle.FromDecimalTo0To360Range(180.0f + local.X);
		localCoords.Altitude = new DegreesAngle (local.Y);
	}


	public void Log(){

		HourAngle ra = equatorialCoords.RA;
		DegreesAngle dec = equatorialCoords.Declination;
		DegreesAngle az = localCoords.Azimuth;
		DegreesAngle alt = localCoords.Altitude; 

		Debug.Log (string.Format("AR: {0}", ra.ToString()));
		Debug.Log (string.Format("Dec: {0}", dec.ToString()));
		Debug.Log (string.Format("Azimuth: {0}", az.ToString()));
		Debug.Log (string.Format("Alt: {0}", alt.ToString()));
	}

	//obliquity of the ecliptic
	protected double GetDeltaEpsilon(){		
		return AASNutation.MeanObliquityOfEcliptic(jd);
	}


	public Vec3D GetVectorToEarthCorrected(){
		return vectorToEarthCorrected;
	}


	public double GetParallacticAngle(){
		return AASParallactic.ParallacticAngle (localHourAngle, 
			location.latitude, 
			equatorialCoords.Declination.Get ());
	}
}

