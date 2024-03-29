﻿using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

using AASharp;
using MathUtils;





public class SkyModel  {


	public static SkyModel INSTANCE;

	private List<StarModel> stars;

	//reverse Mapping HIP index -> array index (used in constellations rendering)
	private int[] reverseMapping;

	private List<Constellation> constellations;// { get; set;} 

	private Dictionary<string, PlanetModel> planets;

	//will be used in picking stars: eq coords -> star Id
	private Dictionary<EquatorialCoords, int> starPositionsDictionary { get; set;}

	private Dictionary<EquatorialCoords, PlanetModel> planetPositionsDictionary { get; set;}



	public enum CelestialBodyType
	{
		SUN, MOON, PLANET, STAR
	}



	private SunModel sun;

	private MoonModel moon;

	private LocationData location;
	private double jd;


	public SkyModel(double julianDay, LocationData location){
			
		if (INSTANCE == null) {
			INSTANCE = this;
		}

		this.location = location;
		this.jd =  julianDay;

		sun               = new SunModel (jd, location);
		moon        	  = new MoonModel (jd, location);
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


		starPositionsDictionary = new Dictionary<EquatorialCoords, int> ();
		planetPositionsDictionary = new Dictionary<EquatorialCoords, PlanetModel> ();


	}

	public double GetJD(){
		return jd;
	}

	public LocationData GetLocation(){
		return location;
	}

	public Vector3 GetEarthAxis(){
		double colatitude = 90.0d - location.latitude;
		double y = Math.Cos (colatitude * M.DEG2RAD);
		double z = Math.Sin (colatitude * M.DEG2RAD);
		return new Vector3(0.0f, (float)y, (float)z);
	}

	public void Update(double jd, LocationData location){
		this.jd = jd;
		this.location = location;

		foreach (KeyValuePair<string, PlanetModel> pair in planets) {
			pair.Value.Update(jd, location);
		}

		sun.Update (jd, location);
		moon.Update (jd, location);
	}


	public void PopulateStarPositionsDictionary(){
		foreach (StarModel star in stars) {
			DegreesAngle dec = new DegreesAngle (star.dec);
			HourAngle ra = new HourAngle (star.ra);

			EquatorialCoords eq = new EquatorialCoords (ra, dec);

			starPositionsDictionary [eq] = star.starID - 1;

		}			
	}



	public CelestialBody FindCelestialBody(EquatorialCoords eq){
		int index = 0;

		//is it a star?
		if (starPositionsDictionary.TryGetValue(eq, out index)){			
			return stars [ index ] as StarModel;	
		}

		//is it a planet?
		PlanetModel planet = null;
		foreach(KeyValuePair<string, PlanetModel> current in planets){
			if(current.Value.EquatorialCoords.EqualsThreshold(eq, 5)){
				planet = current.Value;
				break;
			}
		}		
		if(planet != null){ 
			return planet as PlanetModel;
		}


		//is it the moon?
		if(moon.EquatorialCoords.Equals(eq)){
			return moon as MoonModel;
		}

		//is it the sun?
		if (sun.EquatorialCoords.Equals (eq)) {
			return sun as SunModel;
		}

		return null;
	}



	public PlanetModel FindPlanet(EquatorialCoords eq){
		PlanetModel planet = null;
		if( planetPositionsDictionary.TryGetValue(eq, out planet) ){
			return planet;
		}
		return null;
	}

	public StarModel FindStar(EquatorialCoords eq){

		int index = 0;

		if (starPositionsDictionary.TryGetValue(eq, out index)){			
			return stars [ index ];	
		}

		return null;
	}


	//hour angle of the aries point
	public double GetHourAngleOfAriesPoint(){
		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		double H = theta0Apparent - location.longitude/15d;

		return H;
	}


	public EquatorialCoords Horizontal2Equatorial(double azimuth, double altitude){
		AAS2DCoordinate coords = AASCoordinateTransformation.Horizontal2Equatorial (azimuth+180, altitude, location.latitude);

		//right ascension (alpha) = apparent sidereal time at Greenwich (theta) - Local hour angle (H) - observer's longitude (L, positive west, negative east from Greenwich)
		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		double ra = theta0Apparent - coords.X - location.longitude / 15d;

		return new EquatorialCoords(new HourAngle(ra), new DegreesAngle( coords.Y ));
	}


	public LocalCoords Equatorial2Horizontal(double ra, double dec){
		double theta0Apparent = AASSidereal.ApparentGreenwichSiderealTime (jd);

		//hour angle in hours
		double H = theta0Apparent - location.longitude/15d - ra;

		AAS2DCoordinate localCoords = AASCoordinateTransformation.Equatorial2Horizontal (H, dec, location.latitude);
		double azimuth = AASCoordinateTransformation.MapTo0To360Range (localCoords.X + 180d);

		return new LocalCoords(new DegreesAngle(azimuth), new DegreesAngle(localCoords.Y));
	}



	public static LocalCoords Rectangular2Horizontal(double x, double y, double z){
		double az = Math.Atan2 (x, z) * M.RAD2DEG;
		double alt = Math.Atan2 (y, Math.Sqrt (x * x + z * z)) *M.RAD2DEG;
		return new LocalCoords (new DegreesAngle (az), new DegreesAngle (alt));
	}





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
