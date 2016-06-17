using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


using AASharp;


[ExecuteInEditMode]
public sealed class DateTimeSettings : MonoBehaviour
{
	private decimal jd;

	private static DateTimeSettings instance;

	public static DateTimeSettings INSTANCE { get { return instance; } }

	private double HOURS_PER_DAY = 24d;
	private double MINUTES_PER_DAY = 1440d;
	private double SECONDS_PER_DAY = 86400d;
	private double MILLISECONDS_PER_DAY = 86400000d;


	public bool gregorianCalendar;
	public bool playMode;
	public int timeScale;
	public bool useUTC = false;

	public DateTime localDatetime;

	public TimeScaleOption selectedTimeScale;


	public void SelectTimeScaleOption(int index){
		selectedTimeScale = TimeScaleOption.Values[index];
		timeScale = selectedTimeScale.SecondsPerSecond;
	}

	public void ToggleUtcLocalTime(){
		useUTC = !useUTC;	
	}

	public string UtcOrLocal(){
		return useUTC ? "UTC" : "Local time";
	}

	public bool IsAnyTimeScaleOptionSelected(){
		return this.enabled; //selectedTimeScale != TimeScaleOption.NONE; 
	}

	public class TimeScaleOption {

		//public static readonly TimeScaleOption NONE = new TimeScaleOption(0, "-" );
		public static readonly TimeScaleOption NATURAL = new TimeScaleOption(1, "Natural" );
		public static readonly TimeScaleOption ONE_MINUTE_PER_SECOND = new TimeScaleOption(60, "One minute per second" );
		public static readonly TimeScaleOption ONE_HOUR_PER_SECOND = new TimeScaleOption(3600, "One hour per second" );
		public static readonly TimeScaleOption ONE_DAY_PER_SECOND = new TimeScaleOption(86400, "One day per second" );
		public static readonly TimeScaleOption ONE_WEEK_PER_SECOND = new TimeScaleOption(86400*7, "One week per second" );
		public static readonly TimeScaleOption ONE_MONTH_PER_SECOND = new TimeScaleOption(86400*30, "One month per second" );
		public static readonly TimeScaleOption ONE_YEAR_PER_SECOND = new TimeScaleOption(86400*365, "One year per second" );

		public TimeScaleOption(int secondsPerSecond, string label){
			this.secondsPerSecond = secondsPerSecond;
			this.label            = label;
		}

		public static List<TimeScaleOption> Values{
			get
			{ 
				return new List<TimeScaleOption> {
					NATURAL, ONE_MINUTE_PER_SECOND, ONE_HOUR_PER_SECOND, ONE_DAY_PER_SECOND, 
					ONE_WEEK_PER_SECOND, ONE_MONTH_PER_SECOND, ONE_YEAR_PER_SECOND
				};
			}
		}

		private readonly int secondsPerSecond;
		public int SecondsPerSecond{ get { return secondsPerSecond; }}

		private readonly string label;
		public string Label{ get { return label; }}

		public static string[] GetLabels(){
			List<string> labels = new List<string> (); 
			foreach(TimeScaleOption option in Values){				
				labels.Add (option.Label);
			}
			return labels.ToArray();
		}


	};
		
	void Awake(){	
		if (instance == null) {
			instance = this;
		}


		Reset ();
		gregorianCalendar = true;
		playMode = false;

		SelectTimeScaleOption (0);
	}		

	void Start(){
		
	}

	void Update(){
		if (playMode) {
			Play ((decimal)Time.deltaTime);
		}
	}

	public void Play(decimal deltaTime){
		decimal scale = Convert.ToDecimal (timeScale);
		decimal secondsPerDay = 86400m;

		jd =  Decimal.Add(jd, deltaTime * scale / secondsPerDay); 
	}


	public void Reset(){
		DateTime dt   = DateTime.Now;
		localDatetime = dt;

		if (useUTC) {
			dt = TimeZoneInfo.ConvertTimeToUtc (dt);
		}

		double dayDec = dt.Day + dt.Hour / HOURS_PER_DAY + dt.Minute / MINUTES_PER_DAY +
						dt.Second / SECONDS_PER_DAY + dt.Millisecond / MILLISECONDS_PER_DAY;
		
		jd = Convert.ToDecimal(AASDate.DateToJD (dt.Year, dt.Month, dayDec, gregorianCalendar));
	}


	public int Year(){		
		return (int)Date().Year;
	}

	public int Month(){		
		return  (int)Date().Month;
	}

	public int Day(){		
		return  (int)Date().Day;
	}

	public int Hour(){		
		return  (int)Date().Hour;
	}

	public int Minute(){		
		return  (int) Date().Minute;
	}

	public float Second(){		
		return (float) Date().Second;
	}


	public AASDate Date (){
		return new AASDate (Convert.ToDouble(jd), gregorianCalendar);
	}


	public void UpdateJd(int year, int month, int day, int hour, int minute, double second)
	{
		//if we are using local time, let us convert the input to UTC
		if (!useUTC) {
			int secondInt  = (int) Math.Truncate (second);
			int milisecond = (int) ((second - secondInt)*1000);
			try{
				DateTime utc = new DateTime(year, month, day, hour, minute, secondInt, milisecond);
				utc.ToUniversalTime ();

				year  = utc.Year;
				month = utc.Month;
				day   = utc.Day;
				hour  = utc.Hour;
				minute = utc.Minute;
				second = utc.Second + utc.Millisecond / 1000.0d;

				localDatetime = utc.ToLocalTime ();
			}catch(ArgumentOutOfRangeException a){
				
			}
		}

		double dayDec = day + hour / HOURS_PER_DAY + minute / MINUTES_PER_DAY + second / SECONDS_PER_DAY;

		jd = Convert.ToDecimal( AASDate.DateToJD (year, month, dayDec, gregorianCalendar) );

	}

	public decimal JulianDay(){
		return jd;
	}

}