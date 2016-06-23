using UnityEngine;
using System;
using System.Collections;
using AASharp;
using MathUtils;

public class SunModel : SolarSystemBody {


	public SunModel(double jd, LocationData location) : base(jd, location){			
		Update ();	
	}


	protected override void Update (){

		CalculateEquatorialPosition ();

		CalculateTopocentricPosition ();
	}


	private void CalculateEquatorialPosition (){
		double epsilon0 = GetDeltaEpsilon ();
		double theta = GetGeometricMeanLongitude ();

		double ar = Math.Atan2 (Math.Cos(epsilon0*M.DEG2RAD)*Math.Sin(theta*M.DEG2RAD), Math.Cos(theta*M.DEG2RAD)) * M.RAD2DEG;
		double dec = Math.Asin (Math.Sin(epsilon0*M.DEG2RAD)*Math.Sin(theta*M.DEG2RAD)) * M.RAD2DEG;

		equatorialCoords.RA          = HourAngle.FromDecimalDegrees (ar);
		equatorialCoords.Declination = new DegreesAngle(dec);
	}

	private double GetGeometricMeanLongitude (){
		double T = (jd - 2451545.0) / 36525;

		double T2 = T*T;

		double T3 = T2*T;

		//geom mean Longitude
		double L0 = 280.46645 + 36000.76983*T + .0003032*T2;
		//mean anomaly
		double meanAnomaly = 357.52910 + 35999.05030*T -0.0001559*T2 - 0.00000048*T3;

		//equation of center
		double C = (1.914600 - .004817 * T - .000014 * T2) * Math.Sin (meanAnomaly * M.DEG2RAD)
			+ (.019993 - .000101 * T) * Math.Sin (2d * meanAnomaly * M.DEG2RAD)
			+ .000290 * Math.Sin (3*meanAnomaly*M.DEG2RAD);

		return L0 + C;
	}

	//obliquity of the ecliptic
	private double GetEpsilon(){
		return AASNutation.MeanObliquityOfEcliptic (jd);
	}


	//obliquity of the ecliptic with correction
	private double GetEpsilonCorrected(){
		double T = (jd - 2451545.0) / 36525;
		double epsilon0 = AASNutation.MeanObliquityOfEcliptic (jd);
		double omega = 125.04 - 1934.136 * T;
		double deltaEpsilon = .00256 * Math.Cos (omega*M.DEG2RAD);

		return epsilon0 + deltaEpsilon;
	}


	private double GetApparentLongitudeForTrueDateEquinox(){
		double T = (jd - 2451545.0) / 36525;
		double omega = 125.04 - 1934.136 * T;
		double lambda = GetGeometricMeanLongitude () - .00569 - .00478 * Math.Sin(omega *M.DEG2RAD);

		return AASCoordinateTransformation.MapTo0To360Range (lambda);
	}

	public override string GetName ()
	{
		return "Sun";
	}

	public override double GetDistance(){		
		return AASEarth.RadiusVector (jd);
	}

	protected override double GetRadiusVector (double JD)
	{
		return AASEarth.RadiusVector (JD);
	}


	public override double GetSemidiameter ()
	{
		return AASDiameters.SunSemidiameterA (GetDistance());
	}


	public override string GetBodyDetails(){
		string s = string.Format("{0}\n", "SUN" );
		s += string.Format("Type: {0}\n", "Star" );
		s += string.Format("RA/Dec: {0} / {1}\n", equatorialCoords.RA.ToString(), equatorialCoords.Declination.ToString());

		LocalCoords localCoords = GetLocalCoords();

		s += string.Format("Az/Alt: {0} / {1}\n", localCoords.Azimuth.ToString(), localCoords.Altitude.ToString() );
		s += string.Format("Distance: {0} AU\n", GetDistance() );

		return s;
	}

}