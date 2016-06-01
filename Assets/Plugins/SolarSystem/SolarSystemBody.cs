using UnityEngine;
using System.Collections;
using System;

using AASharp;
using MathUtils;

public abstract class SolarSystemBody{
	

	public abstract string GetName ();
	public abstract string GetTextureFilepath ();

	//JD
	protected double jd;

	//observers location
	protected LocationData location;

	public EquatorialCoords equatorialCoords = new EquatorialCoords();
	public LocalCoords localCoords = new LocalCoords();


	public SolarSystemBody(double jd, LocationData location){
		this.jd = jd;
		this.location = location;
	}


	public void Update(double jd, LocationData location){
		this.jd = jd;
		this.location = location;
		Update ();
	}

	//Calculations are to be performed here
	protected abstract void Update ();


	public Vec3D GetRectangularLocalPosition(){
		double az  = localCoords.Azimuth.Get ();
		double alt = localCoords.Altitude.Get ();

		return Vec3D.PolarToRectangular(az, alt);
	}

	protected void CalculateTopocentricPosition (){

		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		double H = theta0Apparent - location.longitude/15d - equatorialCoords.RA.Get();

		AAS2DCoordinate local = AASCoordinateTransformation.Equatorial2Horizontal (H, equatorialCoords.Declination.Get(), location.latitude);

		localCoords.Azimuth = DegreesAngle.FromDecimalTo0To360Range(180.0f + local.X);
		localCoords.Altitude = DegreesAngle.FromDecimalTo0To360Range (local.Y);
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

}

