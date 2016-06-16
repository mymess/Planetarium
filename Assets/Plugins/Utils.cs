using UnityEngine;
using System.Collections;
using System;

using AASharp;

namespace MathUtils{
	
	public class HourAngle{


		private double hours;

		private double THRESHOLD = .0018519d;

		public HourAngle(){
		}

		public HourAngle(double hours){		
			this.hours = AASCoordinateTransformation.MapTo0To24Range (hours);
		}
		public HourAngle(long hour, long min, double sec){
			int sign = Math.Sign ((decimal)hour);
			this.hours = (double)hour + sign * (double)min / 60d + sign * (double)sec / 3600d;		
		}

		public double ToDecimalDegrees(){
			return hours * 15.0d + Minutes() * 900.0d + Seconds() * 54000.0d;
		}

		public long Hours(){
			return (long)Math.Truncate (hours);
		}

		public long Minutes(){
			double absH = Math.Abs (hours);
			long h = (long)Math.Truncate (absH);
			long min = (long)Math.Truncate((absH-h) * 60d);
			return min;
		}

		public double Seconds(){
			double absH = Math.Abs (hours);
			long h = (long)Math.Truncate (absH);
			long min = (long)Math.Truncate((absH-h) * 60d);
			double sec = ((absH - h) * 60d - min) * 60;
			return Math.Abs(sec);
		}


		public double Get(){
			return hours; 
		}

		public static HourAngle FromDecimalDegrees(double deg){
			deg = AASCoordinateTransformation.MapTo0To360Range (deg);
			double hours = deg / 15.0d;
			return new HourAngle (hours);
		}


		public override bool Equals (object obj)
		{

			if (obj == null || GetType() != obj.GetType()) {
				Debug.Log ("Unequal types");
				return false;
			}
			HourAngle other = null;		

			try{
				other = obj as HourAngle;				
			}catch(InvalidCastException i){
				Debug.Log ("Unequal types");
				return false;
			}

			double otherAngle = other.Get ();

			return hours > (otherAngle - THRESHOLD) && hours < (otherAngle + THRESHOLD);
		}


		public override int GetHashCode ()
		{			
			return (int)Math.Truncate(hours).GetHashCode();
		}

		public static HourAngle FromDecimal(double hours){

			hours = AASCoordinateTransformation.MapTo0To24Range (hours);

			long h = (long)Math.Truncate (hours);
			long m = (long)Math.Floor((hours - h) * 60d);
			double s = (((hours - h) * 60d) - m) * 60d;

			return new HourAngle(h, m ,s);
		}

		public override  string ToString(){
			return string.Format ("{0}h {1}m {2}s", 
				Hours().ToString("00").PadLeft(2,'0'), 
				Minutes().ToString().PadLeft(2,'0'), 
				Math.Round(Seconds(), 2).ToString("00.00"));
		}

		public void Log(){
			Debug.Log (this.ToString ());
		}
	}

	public class DegreesAngle{
		//public long degrees;
		//public long min;
		//public double sec;

		private double degrees;

		private double THRESHOLD = .0277777777d;

		public DegreesAngle(){
		}

		public DegreesAngle(double degrees){
			this.degrees = degrees;
		}

		public DegreesAngle(long degrees, long min, double sec){
			int sign = Math.Sign (degrees);
			this.degrees = degrees + sign*min/60.0d + sign*sec/3600d;
		}

		public double Get(){
			return degrees;
		}

		public DegreesAngle To0To360Range(){
			return new DegreesAngle(AASCoordinateTransformation.MapTo0To360Range (degrees));
		}

		public static DegreesAngle FromDecimalTo0To360Range(double deg){
			deg = AASCoordinateTransformation.MapTo0To360Range (deg);
			return new DegreesAngle(deg);
		}

		public long Degrees(){
			return (long)Math.Truncate (degrees);
		}

		public long Minutes(){
			long deg = (long)Math.Truncate (degrees);
			long min = (long)Math.Truncate((degrees-deg) * 60d);
			return Math.Abs(min);
		}

		public double Seconds(){
			long deg = (long)Math.Truncate (degrees);
			long min = (long)Math.Truncate((degrees-deg) * 60d);
			double sec = ((degrees - deg) * 60d - min) * 60;
			return Math.Abs(sec);
		}

		public override bool Equals (object obj)
		{
			if (obj == null || GetType() != obj.GetType()) {
				return false;
			}

			DegreesAngle other = null;

			try{
			 	other = obj as DegreesAngle;
			}catch(InvalidCastException i){

				Debug.Log ("Cast exception ");
				return false;
			}

			double otherAngle = other.Get ();

			return degrees > (otherAngle - THRESHOLD) && degrees < (otherAngle + THRESHOLD);
			
		}

		public override int GetHashCode ()
		{
			int deg = (int)Math.Truncate(degrees);
			return deg.GetHashCode ();
		}


		public override string ToString(){
			return string.Format ("{0:D2}º {1:D2}' {2:D2}\"", 
				Degrees().ToString("00").PadLeft(2,'0'), 
				Minutes().ToString("00").PadLeft(2,'0'), 
				Math.Round(Seconds(), 2).ToString("00.00"));
		}

		public void Log(){
			Debug.Log (this.ToString ());
		}
	}

	public class EquatorialCoords{
		public HourAngle RA = new HourAngle();
		public DegreesAngle Declination = new DegreesAngle();

		public EquatorialCoords(){
		}

		public EquatorialCoords (HourAngle ra, DegreesAngle dec){
			RA = ra;
			Declination = dec;
		}

		public Vec3D ToRectangular(){

			return Vec3D.PolarToRectangular (RA.Get () * 15d, Declination.Get ());
			/*
			double x = Math.Sin (RA.Get()*15d * M.DEG2RAD) * Math.Cos(Declination.Get()*M.DEG2RAD);
			double y = Math.Sin (Declination.Get() * M.DEG2RAD);
			double z = Math.Cos (RA.Get()*15d * M.DEG2RAD) * Math.Cos(Declination.Get()*M.DEG2RAD);

			return new Vec3D(x, y, z);
			*/
		}


		public override bool Equals (object obj)
		{
			if (obj == null) {
				return false;
			}
			EquatorialCoords otherEq = null;

			try{
			 	otherEq = obj as EquatorialCoords;
			}catch(InvalidCastException i){
				Debug.Log ("Invalid cast");
				return false;
			}

			return RA.Equals (otherEq.RA) && Declination.Equals (otherEq.Declination);
		}

		//

		public override int GetHashCode ()
		{
			int hash = 23 * RA.GetHashCode ();
			hash = hash * 31 + Declination.GetHashCode ();
			return  hash;
		}

		public override string ToString ()
		{
			return string.Format ("RA: {0}\nDec: {1}", RA.ToString(), Declination.ToString());
		}

	}

	public class LocalCoords{
		public DegreesAngle Azimuth = new DegreesAngle();
		public DegreesAngle Altitude = new DegreesAngle();

		public LocalCoords(){
		}

		public LocalCoords(DegreesAngle az, DegreesAngle alt){
			this.Azimuth = az;
			this.Altitude = alt;
		}

		public Vec3D ToRectangular(){			
		
			return Vec3D.PolarToRectangular (Azimuth.Get (), Altitude.Get ());
			/*
			double x = Math.Sin (Azimuth.Get() * M.DEG2RAD) * Math.Cos(Altitude.Get()*M.DEG2RAD);
			double y = Math.Sin (Altitude.Get() * M.DEG2RAD);
			double z = Math.Cos (Azimuth.Get() * M.DEG2RAD) * Math.Cos(Altitude.Get()*M.DEG2RAD);
			return new Vec3D (x, y, z);
			*/
		}

	}


	public class M{
		public static double DEG2RAD = 0.017453292519943295;
		public static double RAD2DEG = 57.29577951308232;

		public static double[] HoursToHMS(double hours){
			double[] res = new double[3];

			hours = AASCoordinateTransformation.MapTo0To24Range (hours);

			res [0] = Math.Floor (hours);
			double min = (hours - res[0]) * 60.0d;
			res [1] = Math.Floor (min);
			res [2] = (min - res [1]) * 60d;

			return res;
		}

		public static double[] DegreesToHMS(double deg){
			double[] res = new double[3];

			res [0] = Math.Truncate (deg);
			double min = (deg - res[0]) * 60.0d;
			res [1] = Math.Floor (min);
			res [2] = (min - res [1]) * 60d;

			return res;


		}

	}

	public class LocationData{
		public double latitude;
		public double longitude;
		public double altitude;
		public double pressure = 1010;
		public double temperature = 10;

		public LocationData(double longitude, double latitude, double altitude){
			this.longitude = longitude;
			this.latitude = latitude;
			this.altitude = altitude;
		}

		public override bool Equals (System.Object obj)
		{
			if(obj == null){
				return false;
			}

			LocationData otherLocation = obj as LocationData;

			if ((System.Object)otherLocation == null)
			{
				return false;
			}
			return otherLocation.latitude == this.latitude && otherLocation.longitude == this.longitude;
		}
	}

	public class Vec3D{
		public double x;
		public double y;
		public double z;

		public Vec3D(){
		}

		public Vec3D(double x, double y, double z){
			this.x = x;
			this.y = y;
			this.z = z;
		}
			
		public double Length(){
			return Math.Sqrt (x * x + y * y + z * z);
		}


		public override string ToString ()
		{
			return string.Format ("x={0}, y={1}, z={2}", x, y, z);
		}

		public static Vec3D PolarToRectangular(double angleX, double angleY){
			double x = Math.Sin (angleX * M.DEG2RAD) * Math.Cos(angleY*M.DEG2RAD);
			double y = Math.Sin (angleY * M.DEG2RAD);
			double z = Math.Cos (angleX * M.DEG2RAD) * Math.Cos(angleY*M.DEG2RAD);

			return new Vec3D (x, y, z);
		}


		public void Log(){
			Debug.Log (ToString ());
		}
	}

}