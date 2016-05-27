using UnityEngine;
using System.Collections;
using System;

using AASharp;

namespace MathUtils{
	
	public class HourAngle{


		public double hours;

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
			return FromDecimal (hours);
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
				Hours().ToString().PadLeft(2,'0'), 
				Minutes().ToString().PadLeft(2,'0'), 
				Seconds().ToString().PadLeft(2,'0'));
		}
	}

	public class DegreesAngle{
		//public long degrees;
		//public long min;
		//public double sec;

		private double degrees;

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


		public override string ToString(){
			return string.Format ("{0}º {1}' {2}\"", 
				Degrees().ToString().PadLeft(2,'0'), 
				Minutes().ToString().PadLeft(2,'0'), 
				Seconds().ToString().PadLeft(2,'0'));
		}
	}

	public class EquatorialCoords{
		public HourAngle RA = new HourAngle();
		public DegreesAngle Declination = new DegreesAngle();

	}

	public class LocalCoords{
		public DegreesAngle Azimuth = new DegreesAngle();
		public DegreesAngle Altitude = new DegreesAngle();
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

		public void Log(){
			Debug.Log (ToString ());
		}
	}

}