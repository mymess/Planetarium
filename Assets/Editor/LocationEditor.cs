using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(LocationSettings))]
public class LocationEditor : Editor {

	LocationSettings locSettings;

	private bool west;
	private bool east;

	private int lonDegrees;
	private int lonMinutes;
	private float lonSeconds;

	private int latDegrees;
	private int latMinutes;
	private float latSeconds;


	private static string LONGITUDE_DEGREES = "LON_DEG";
	private static string LONGITUDE_MINUTES = "LON_MIN";
	private static string LONGITUDE_SECONDS = "LON_SEC";

	private static string LATITUDE_DEGREES = "LAT_DEG";
	private static string LATITUDE_MINUTES = "LAT_MIN";
	private static string LATITUDE_SECONDS = "LAT_SEC";

	private static string SEARCH_INPUT = "SEARCH_INPUT";

	private bool toggleNorthSouth = false;
	private bool toggleWestEast   = false;

	private int resultIndex = 0;

	void OnEnable()
	{
		locSettings = (LocationSettings) target;
	}

	public override void OnInspectorGUI()
	{
		DisplayCitySearch ();

		GUILayout.Space (10);

		DisplayLongitudeSettings();

		GUILayout.Space (10);

		DisplayLatitudeSettings ();

		GUILayout.Space (10);

		DisplayAltitudeSettings ();

		GUILayout.Space (10);

	}

	void DisplayCitySearch(){
		GUILayout.Label ("City", EditorStyles.boldLabel);
		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		int textFieldWidth = 30;
		int textFieldHeight = 18;
		GUI.SetNextControlName(SEARCH_INPUT);
		GUILayoutOption[] textFieldoptions = new GUILayoutOption[]{ GUILayout.MinWidth(textFieldWidth), GUILayout.MinHeight(textFieldHeight) };
		locSettings.searchInput = EditorGUILayout.TextField ( "Search: ", locSettings.searchInput, myStyle, textFieldoptions);

		List<LocationSettings.City> searchResults = new List<LocationSettings.City> ();

		if(!string.IsNullOrEmpty(locSettings.searchInput.Trim())){
			searchResults = locSettings.Search ();
		}
			
		string[] resultsArray = new string[1]{""};


		if (searchResults.Count > 0) {					
			resultsArray = new string[searchResults.Count];
			for (int i = 0; i < searchResults.Count; i++) {
				resultsArray [i] = searchResults [i].ToString ();
			}
		} else {
			resultIndex = 0;
		}

		GUILayoutOption[] dropdownOptions = new GUILayoutOption[]{ GUILayout.MinWidth(260), GUILayout.MinHeight(textFieldHeight), GUILayout.ExpandWidth(true)  };
		GUIStyle style    = new GUIStyle ("Popup");
		style.alignment   = TextAnchor.MiddleRight;
		style.fontStyle = FontStyle.Bold;


		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		resultIndex = EditorGUILayout.Popup(resultIndex, resultsArray, style, dropdownOptions);
		GUILayout.EndHorizontal ();

		if(searchResults.Count > 0){
			LocationSettings.City city = searchResults[resultIndex];
			locSettings.Latitude  = city.latitude;
			locSettings.Longitude = -city.longitude;
			locSettings.Altitude  = city.altitude;
			Repaint ();
		}
	}


	void DisplayAltitudeSettings(){
		int textFieldWidth = 30;
		int textFieldHeight = 18;

		GUILayout.Label ("Altitude", EditorStyles.boldLabel);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(GUILayout.MinWidth(110));
		GUILayoutOption[] textFieldoptions = new GUILayoutOption[]{ GUILayout.MinWidth(textFieldWidth), GUILayout.MinHeight(textFieldHeight) };

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		locSettings.Altitude = EditorGUILayout.FloatField ( "", locSettings.Altitude, myStyle, textFieldoptions);

		GUILayout.Label (" meters", GUI.skin.label);
		EditorGUILayout.EndHorizontal (  );
		GUILayout.EndVertical();
	}

	void DisplayLatitudeSettings (){
		GUILayout.Label ("Latitude", EditorStyles.boldLabel);

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(300));

		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(90) };

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		GUIStyle shiftStyle = new GUIStyle ();
		shiftStyle.alignment = TextAnchor.UpperLeft; 
		shiftStyle.fontStyle = FontStyle.Bold;
		EditorGUIUtility.labelWidth = 0;

		int textFieldWidth  = 30;
		int textFieldHeight = 18;
		GUILayoutOption[] textFieldoptions = new GUILayoutOption[]{ GUILayout.MinWidth(textFieldWidth), GUILayout.MinHeight(textFieldHeight) };

		GUI.SetNextControlName(LATITUDE_DEGREES);
		latDegrees = EditorGUILayout.IntField ( "", locSettings.LatitudeDegrees(), myStyle, textFieldoptions);

		GUILayout.Label ("º", shiftStyle);

		//NORTH-SOUTH button
		options = new GUILayoutOption[]{ GUILayout.Width(50), GUILayout.Height(20) };
		toggleNorthSouth = GUILayout.Button(locSettings.NorthOrSouth(), options);

		GUILayout.FlexibleSpace();

		//Latitude minutes
		EditorGUIUtility.labelWidth = 0;
		options = new GUILayoutOption[]{ GUILayout.Width(textFieldWidth) };
		GUI.SetNextControlName(LATITUDE_MINUTES);
		latMinutes = EditorGUILayout.IntField ("", locSettings.LatitudeMinutes (), myStyle, textFieldoptions);

		GUILayout.Label ("'", shiftStyle);
		GUILayout.FlexibleSpace();

		EditorGUIUtility.labelWidth = 0;
		options = new GUILayoutOption[]{ GUILayout.Width(60) };

		string secondStr = locSettings.LatitudeSeconds ().ToString ("##.##");
		float.TryParse (secondStr, out latSeconds);
		
		//Latitude seconds
		GUI.SetNextControlName(LATITUDE_SECONDS);
		latSeconds = EditorGUILayout.FloatField ("", latSeconds, myStyle, textFieldoptions);

		GUILayout.Label ("\"", shiftStyle);

		EditorGUILayout.EndHorizontal (  );
		GUILayout.EndVertical();

		locSettings.UpdateLatitude (latDegrees, latMinutes, latSeconds, toggleNorthSouth);
	}

	void DisplayLongitudeSettings(){

		GUILayout.Label ("Longitude", EditorStyles.boldLabel);

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(300));

		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(90) };

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		GUIStyle shiftStyle = new GUIStyle ();
		shiftStyle.alignment = TextAnchor.UpperLeft; 
		shiftStyle.fontStyle = FontStyle.Bold;
		EditorGUIUtility.labelWidth = 0;

		int textFieldWidth = 30;
		int textFieldHeight = 18;

		GUILayoutOption[] textFieldoptions = new GUILayoutOption[]{ GUILayout.MinWidth(textFieldWidth), GUILayout.MinHeight(textFieldHeight) };

		GUI.SetNextControlName(LONGITUDE_DEGREES);
		lonDegrees = EditorGUILayout.IntField ( "", locSettings.LongitudeDegrees(), myStyle, textFieldoptions);

		GUILayout.Label ("º", shiftStyle);


		//WEST-EAST button
		options = new GUILayoutOption[]{ GUILayout.Width(50), GUILayout.Height(20) };
		toggleWestEast = GUILayout.Button(locSettings.WestOrEast(), options);

		GUILayout.FlexibleSpace();

		EditorGUIUtility.labelWidth = 0;
		options = new GUILayoutOption[]{ GUILayout.Width(textFieldWidth) };
		GUI.SetNextControlName(LONGITUDE_MINUTES);
		lonMinutes = EditorGUILayout.IntField ("", locSettings.LongitudeMinutes (), myStyle, textFieldoptions);

		GUILayout.Label ("'", shiftStyle);
		GUILayout.FlexibleSpace();

		EditorGUIUtility.labelWidth = 0;
		options = new GUILayoutOption[]{ GUILayout.Width(60) };

		string secondStr = locSettings.LongitudeSeconds ().ToString ("##.##");
		float.TryParse (secondStr, out lonSeconds);
		GUI.SetNextControlName(LONGITUDE_SECONDS);
		lonSeconds = EditorGUILayout.FloatField ("", lonSeconds, myStyle, textFieldoptions);

		GUILayout.Label ("\"", shiftStyle);
		EditorGUILayout.EndHorizontal (  );
		GUILayout.EndVertical();

		locSettings.UpdateLongitude (lonDegrees, lonMinutes, lonSeconds, toggleWestEast);

		if(IsLocationBeingEdited()){			
			GUI.FocusControl (SEARCH_INPUT);
			locSettings.searchInput = "";
			Repaint ();
		}

	}

	private bool IsLocationBeingEdited(){
		string focus = GUI.GetNameOfFocusedControl ();
		return LONGITUDE_DEGREES.Equals (focus) || LONGITUDE_MINUTES.Equals (focus) || LONGITUDE_SECONDS.Equals (focus)
		|| LATITUDE_DEGREES.Equals (focus) || LATITUDE_MINUTES.Equals (focus) || LATITUDE_SECONDS.Equals (focus)
			|| toggleNorthSouth || toggleWestEast;
	}

}
