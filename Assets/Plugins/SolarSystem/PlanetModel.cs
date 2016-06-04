using UnityEngine;
using System.Collections;
using System;

using AASharp;

using MathUtils;

public abstract class PlanetModel : SolarSystemBody{

	public abstract double GetEclipticLongitude (double jd);
	public abstract double GetEclipticLatitude (double jd);


	protected abstract AASEllipticalObject GetEllipticalObject();

	protected double jde;
	protected double jdeCorrected;

	//geometric ecliptic longitude and latitude
	protected double eclipticLongitude;
	protected double eclipticLatitude;

	//corrected ecliptic longitude and latitude
	protected double eclipticLongitudeCorrected;
	protected double eclipticLatitudeCorrected;

	public double distanceToSun;

	public double apparentRa;
	public double apparentDec;

	protected double aberrationConstant = 20.49552f/3600.0f;


	protected double eclipticLongitudeEarth;
	protected double eclipticLatitudeEarth;

	protected double lambdaAberration;
	protected double betaAberration;

	//FK5
	protected double lambdaFK5;
	protected double betaFK5;

	//Earth's orbit obliquity
	protected double epsilon;

	public PlanetModel(double jd, LocationData location) : base(jd, location){		
		Update ();
	}

	protected sealed override void Update(){

		//geometric positions of planet and Earth
		CalculateGeometricPositions();

		//light time correction
		CorrectForLightTavelTime();

		//correct for aberration
		CorrectForAberration ();

		//correct for FK5 system
		CorrectForFK5SystemReduction();

		CalculateTopocentricPosition ();

	}

	protected void CalculateGeometricPositions(){
		eclipticLongitude = GetEclipticLongitude (jd);
		eclipticLatitude  = GetEclipticLatitude (jd);
		double planetSunDistance = GetRadiusVector (jd);

		eclipticLongitudeEarth  = AASEarth.EclipticLongitude (jd);
		eclipticLatitudeEarth   = AASEarth.EclipticLatitude (jd);
		double earthSunDistance = AASEarth.RadiusVector (jd);

		vectorToEarth = VectorToEarth (eclipticLongitude, eclipticLatitude, planetSunDistance, eclipticLongitudeEarth, eclipticLatitudeEarth, earthSunDistance);

	}

	protected void CorrectForLightTavelTime(){
		//
		double tau = AASElliptical.DistanceToLightTime (vectorToEarth.Length());
		jde = jd - tau;

		eclipticLongitudeCorrected = GetEclipticLongitude (jde); 			
		eclipticLatitudeCorrected = GetEclipticLatitude (jde);
		distanceToSun = GetRadiusVector (jde);


		vectorToEarthCorrected = VectorToEarth (eclipticLongitudeCorrected, 
			eclipticLatitudeCorrected, distanceToSun, eclipticLongitudeEarth, eclipticLatitudeEarth, AASEarth.RadiusVector(jd));
		double tauCorrected    = AASElliptical.DistanceToLightTime (vectorToEarthCorrected.Length());

		jdeCorrected = jd - tauCorrected;
		eclipticLongitudeCorrected = GetEclipticLongitude (jdeCorrected);
		eclipticLatitudeCorrected  = GetEclipticLatitude (jdeCorrected);
		distanceToSun = GetRadiusVector (jdeCorrected);
	}

	protected void CorrectForAberration(){
		double e = GetEarthEccentricity (jd);
		double pi = GetEarthPerihelionLongitude(jd);

		double theta = AASSun.GeometricEclipticLongitude (jdeCorrected);

		double appGeoLon = GetGeocentricLongitude(vectorToEarthCorrected.x, vectorToEarthCorrected.y);
		double appGeoLat = GetGeocentricLatitude(vectorToEarthCorrected.x, vectorToEarthCorrected.y, vectorToEarthCorrected.z);


		double deltaLambda = GetAberrationDeltaLongitude (theta, appGeoLon, appGeoLat, e, pi);
		double deltaBeta = GetAberrationDeltaLatitude (theta, appGeoLon, appGeoLat, e, pi);


		lambdaAberration = appGeoLon + deltaLambda;
		betaAberration = appGeoLat + deltaBeta;
	}

	protected void CorrectForFK5SystemReduction(){
		double appGeoLon = GetGeocentricLongitude(vectorToEarthCorrected.x, vectorToEarthCorrected.y);
		double appGeoLat = GetGeocentricLatitude(vectorToEarthCorrected.x, vectorToEarthCorrected.y, vectorToEarthCorrected.z);

		double deltaLambdaFK5 = AASFK5.CorrectionInLongitude (appGeoLon, appGeoLat, jdeCorrected);
		double deltaBetaFK5 = AASFK5.CorrectionInLatitude (appGeoLon, jdeCorrected);

		lambdaFK5 = lambdaAberration + deltaLambdaFK5;
		betaFK5 = betaAberration + deltaBetaFK5;

		//correct for nutation
		double deltaPhi = AASNutation.NutationInLongitude (jdeCorrected)/3600;
		double deltaEpsilon = AASNutation.NutationInObliquity (jdeCorrected)/3600;
		epsilon = AASNutation.TrueObliquityOfEcliptic (jdeCorrected);
		double lambdaNutation = lambdaFK5 + deltaPhi;
		double betaNutation = betaFK5 + deltaEpsilon;

		//x=RA in decimal hours, y=dec in degrees
		AAS2DCoordinate equatorial  = AASCoordinateTransformation.Ecliptic2Equatorial(lambdaNutation, betaNutation, epsilon);

		equatorialCoords.RA = new HourAngle(equatorial.X);
		equatorialCoords.Declination = new DegreesAngle (equatorial.Y);
	}




	protected double GetTimeInJulianCenturies2000( double jd ){
		return (jd - 2451545.0) / 36525.0;
	}

	protected double GetEarthEccentricity(double jd){
		double T = GetTimeInJulianCenturies2000(jde);
		return .016708617 - .000042037 * T - .0000001236 * T * T;
	}


	protected double GetEarthPerihelionLongitude(double jde){
		double T = GetTimeInJulianCenturies2000(jde);
		return 102.93735 + 1.71953*T + 0.00046*T*T;
	}


	protected double GetGeocentricLongitude(double x, double y){
		return AASCoordinateTransformation.MapTo0To360Range (Math.Atan2(y, x)*M.RAD2DEG);
	}

	protected double GetGeocentricLatitude(double x, double y, double z){
		return Math.Atan2(z, Math.Sqrt(x*x + y*y))*M.RAD2DEG;
	}

	//theta = geometric longitude of the sun
	//e = eccentricity of earths orbit
	//pi= longitude of earths perihelion in degrees
	//lambda = geometric longitude in degrees
	//beta = geometric latitude in degrees
	protected double GetAberrationDeltaLongitude(double theta, double lambda, double beta, double e, double pi){		
		return (-aberrationConstant * Math.Cos ((theta - lambda) * Mathf.Deg2Rad) +
			e * aberrationConstant * Math.Cos ((pi - lambda) * M.DEG2RAD)) / Math.Cos (beta * M.DEG2RAD);
	}

	//theta = geometric longitude of the sun
	//e = eccentricity of earths orbit
	//pi= longitude of earths perihelion in degrees
	//lambda = geometric longitude in degrees
	//beta = geometric latitude in degrees
	protected double GetAberrationDeltaLatitude(double theta, double lambda, double beta, double e, double pi){		
		return -aberrationConstant * Math.Sin (beta * M.DEG2RAD) 
			* (Math.Sin ((theta - lambda) * M.DEG2RAD) - e * Math.Sin ((pi - lambda) * M.DEG2RAD));
	}

	public double GeometricDistanceToEarth(){
		return vectorToEarth.Length();
	}


	public double GetApparentRadiusVector(){
		return distanceToSun;
	}


	/**
	 * eclipticLongitude ecliptic longitude of the planet
	 * ecLat
	 * 
	 **/
	Vec3D VectorToEarth (double ecLon, double ecLat, double radiusV, double ecLonEarth, double ecLatEarth, double radiusEarth){
		Vec3D v = new Vec3D ();

		v.x = radiusV * Math.Cos (ecLat* M.DEG2RAD) * Math.Cos (ecLon * M.DEG2RAD) 
			- radiusEarth * Math.Cos(ecLatEarth* M.DEG2RAD) * Math.Cos(ecLonEarth* M.DEG2RAD);

		v.y = radiusV * Math.Cos (ecLat* M.DEG2RAD) * Math.Sin (ecLon * M.DEG2RAD) 
			- radiusEarth * Math.Cos(ecLatEarth* M.DEG2RAD) * Math.Sin(ecLonEarth* M.DEG2RAD);

		v.z = radiusV * Math.Sin (ecLat * M.DEG2RAD) - radiusEarth * Math.Sin(ecLatEarth* M.DEG2RAD);				

		return v;
	}


	public AASEllipticalPlanetaryDetails GetPlanetaryDetails(double jd){		
		return AASElliptical.Calculate (jd, GetEllipticalObject ());
	}

	public double Magnitude(){
		double phaseAngle = AASIlluminatedFraction.PhaseAngle(
			GetRadiusVector(jdeCorrected), 
			AASEarth.RadiusVector(jd), 
			vectorToEarthCorrected.Length());

		return AASIlluminatedFraction.MarsMagnitudeMuller(
			AASMars.RadiusVector(jdeCorrected), 
					vectorToEarthCorrected.Length(), 
					phaseAngle
			);
	}

	public double GetParallacticAngle(){
		return AASParallactic.ParallacticAngle (localHourAngle, 
			location.latitude, 
			equatorialCoords.Declination.Get ());
	}

}
