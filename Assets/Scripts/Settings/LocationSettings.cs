using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text.RegularExpressions; 
using AASharp;

[ExecuteInEditMode]
public sealed class LocationSettings : MonoBehaviour {


	private static LocationSettings instance;
	public static LocationSettings INSTANCE
	{
		get 
		{
			return instance; 
		}
	}

	private double longitude;
	public double Longitude {
		get { return longitude; }
		set { longitude = value; }
	}

	private double latitude;
	public double Latitude {
		get { return latitude; }
		set { latitude = value; }
	}


	private float altitude;
	public float Altitude {
		get { return altitude; }
		set { altitude = value; }
	}


	public string searchInput;

	private List<City> cities = new List<City>();

	private double MINUTES_PER_DEGREE = 60.0d;
	private double SECONDS_PER_DEGREE = 3600.0d;
	private double SECONDS_PER_MINUTE = 60.0d;

	public LocationSettings(){
		if (instance == null) {
			instance = this;
		}
	}

	void Awake(){

		longitude = 0.0d;
		latitude = 0.0d;	
	}

	void OnEnable(){
		LoadCities ();
	}

	void Update(){
		
	}


	public string NorthOrSouth(){
		return latitude >= 0.0d ? "N" : "S";
	}

	public string WestOrEast(){
		return longitude >= 0 ? "W" : "E";
	}


	public bool IsWest(){
		return longitude >= 0.0d;
	}

	public bool IsNorth(){
		return latitude >= 0.0d;
	}


	public int LongitudeDegrees(){			
		return (int) Math.Abs(Math.Truncate (MapToMinus180To180Range(longitude)));
	}

	public int LongitudeMinutes(){		
		double deg = Math.Abs(MapToMinus180To180Range (longitude));
		double min = (deg - LongitudeDegrees ()) * MINUTES_PER_DEGREE;
		return (int) Math.Truncate( min );
	}

	public double LongitudeSeconds(){
		double deg = Math.Abs(MapToMinus180To180Range (longitude));
		double min = (deg - LongitudeDegrees ()) * MINUTES_PER_DEGREE;
		return (min - LongitudeMinutes ()) * SECONDS_PER_MINUTE;
	}


	public int LatitudeDegrees(){
		return (int) Math.Abs(Math.Truncate (MapToMinus90To90Range(latitude)));
	}

	public int LatitudeMinutes(){
		double deg = Math.Abs(MapToMinus90To90Range (latitude));
		double min = (deg - LatitudeDegrees ()) * MINUTES_PER_DEGREE;
		return (int) Math.Truncate( min );
	}

	public double LatitudeSeconds(){
		double deg = Math.Abs(MapToMinus90To90Range (latitude));
		double min = (deg - LatitudeDegrees ()) * MINUTES_PER_DEGREE;
		return (min - LatitudeMinutes ()) * SECONDS_PER_MINUTE;
	}

	public void UpdateLongitude(int deg, int min, float sec, bool toggleWestEast){
		int sign  = (longitude >= 0.0d) ? 1:  -1;
		longitude = sign * ((double)deg + min / MINUTES_PER_DEGREE + sec / SECONDS_PER_DEGREE );
		if (toggleWestEast) {
			ToggleWestEast ();
		} 
	}

	public void UpdateLatitude(int deg, int min, float sec, bool toggleNorthSouth){
		int sign  = (latitude >= 0.0d) ? 1:  -1;
		latitude = sign * ((double)deg + (double)min / MINUTES_PER_DEGREE + (double)sec / SECONDS_PER_DEGREE);

		if (toggleNorthSouth || deg<0) {
			ToggleNorthSouth ();
		}
	}


	public void SetWest(){
		longitude = Math.Abs (longitude);
	}

	public void SetEast(){		
		longitude = -Math.Abs (longitude);
	}


	public void ToggleWestEast(){				
		longitude *= -1.0d;
		LoadCities ();
	}

	public void ToggleNorthSouth(){
		latitude *= -1.00d;
		cities.Clear ();
	}


	private double MapToMinus180To180Range(double lon){
		while (lon > 180.0d) {
			lon -= 360;
		}
		while (lon < -180.0d) {
			lon += 360;
		}
		return lon;
	}

	private double MapToMinus90To90Range(double lat){		
		int n = 0;
		while ((n + 1) * 90 < lat) {
			lat -= 2 * (n+1) * (lat-90);
			n++;
		}
		n = 0;
		while ((n + 1) * (-90) > lat) {
			lat += 2 * (n+1) * -(lat+90);
			n++;
		}
		return lat;
	}


	private void LoadCities(){
		TextAsset file = Resources.Load ("World_Cities_Location_table") as TextAsset;

		if (cities.Count == 0) {
			using (StringReader reader = new StringReader (file.text)) {
				string line = string.Empty;
				do {
					line = reader.ReadLine ();
					if (line != null) {
						string[] data = line.Replace("\"","").Split (';');
						if(!string.IsNullOrEmpty(data[2])){ 			//some city names are empty in the file, so skip them
							City city = new City ();
							city.country = data [1];
							city.name = data [2];
							double.TryParse (data [3], out city.latitude);
							double.TryParse (data [4], out city.longitude);
							float.TryParse (data [5], out city.altitude);
							cities.Add (city);
						}
					}

				} while (line != null);
			}
			cities.Sort ((x, y) => string.Compare (x.name, y.name));
		}

	}

	public List<City> Search(){
		List<City> result = cities.Where(city => city.name.StartsWith(searchInput, 
			StringComparison.InvariantCultureIgnoreCase) ).ToList();

		return result;
	}


	public class City{

		public string name;
		public string country;

		public double longitude;
		public double latitude;
		public float altitude;


		public void Log(){
			Debug.Log (string.Format("{0}, lon: {1}, lat{2}", this.ToString(), longitude, latitude));
		}

		public override string ToString ()
		{
			return String.Format ("{0}, {1}", this.name, this.country);
		}
	}




}
