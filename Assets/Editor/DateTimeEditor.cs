using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;


[CustomEditor(typeof(DateTimeSettings))]
public class DateTimeEditor : Editor {

	DateTimeSettings dt;

	private int year;
	private int month;
	private int day;

	private int hour;
	private int minute;
	private float second;

	Texture playTex;
	Texture pauseTex;
	Texture playPauseTex;

	double lastUpdate;

	private static string TIME_SCALE = "TimeScale";
	private int timeScaleIndex = 0;

	void OnEnable()
	{
		dt = (DateTimeSettings)target;
		dt.Reset ();
		playTex = Resources.Load ("Textures/play-button") as Texture;
		pauseTex = Resources.Load ("Textures/pause-button") as Texture;
		playPauseTex = playTex;
		lastUpdate = EditorApplication.timeSinceStartup;
	}
		

	private Texture2D LoadPNG(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); 
		}else{
			Debug.Log ("FILE not found-> " + filePath);
		}
		return tex;
	}


	//Refresh is done here
	public override void OnInspectorGUI()
	{
		GUILayout.Label ("Date/Time", EditorStyles.boldLabel);

		DisplayDateSettings ();
		DisplayTimeSettings ();

		DisplayTimeOptions ();

		GUILayout.Space (5);

		DisplayPlayMode ();

		GUILayout.Space (20);
	}

	void DisplayTimeOptions(){
		if (!dt.playMode){
			dt.UpdateJd (year, month, day, hour, minute, (double)second);
		}
		if (GUILayout.Button ("NOW")) {
			dt.Reset ();
		}
		GUILayout.Space (5);
		EditorGUILayout.BeginHorizontal ();
		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(65), GUILayout.Height(40) };
		GUIStyle style = new GUIStyle ();

		bool pressed = GUILayout.Button (dt.UtcOrLocal(), "Button", options);

		if (pressed) {
			dt.ToggleUtcLocalTime ();

			Repaint ();
		}

		GUILayout.Space (30);
		dt.gregorianCalendar = EditorGUILayout.ToggleLeft ("Gregorian calendar", dt.gregorianCalendar, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal ();

	}


	void DisplayPlayMode(){
		GUILayout.Label ("Play mode", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));


		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(65), GUILayout.Height(40) };
		dt.playMode = GUILayout.Toggle (dt.playMode, playPauseTex, "Button", options);

		if (dt.playMode) {						
			playPauseTex = pauseTex;
			decimal deltaTime = Convert.ToDecimal (EditorApplication.timeSinceStartup - lastUpdate);
			dt.Play ( deltaTime );

			Repaint ();
		} else {	
			Repaint ();
			playPauseTex = playTex;
		}


		string intFieldlabel = "Time scale (seconds/s): ";
		var textDimensions = GUI.skin.label.CalcSize(new GUIContent(intFieldlabel));
		EditorGUIUtility.labelWidth = textDimensions.x;

		options			  = new GUILayoutOption[]{ GUILayout.MinWidth(190), GUILayout.ExpandWidth(true) };
		GUIStyle myStyle  = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;
		myStyle.fixedWidth = 60;

		myStyle.stretchWidth = true;
		//GUILayout.FlexibleSpace();

		GUILayout.BeginVertical();

		//Time scale text field
		EditorGUI.BeginDisabledGroup(dt.IsAnyTimeScaleOptionSelected()); //<---
			GUI.SetNextControlName(TIME_SCALE);

		dt.timeScale = EditorGUILayout.IntField (intFieldlabel, dt.timeScale, myStyle,  options);

		EditorGUI.EndDisabledGroup(); 

		//Time scale dropdown
		GUIStyle style    = new GUIStyle ("Popup");
		style.alignment   = TextAnchor.MiddleRight;
		style.fontStyle   = FontStyle.Bold;
		style.stretchWidth=true;
		GUILayoutOption[] dropdownOptions = new GUILayoutOption[]{ GUILayout.MinWidth(190), GUILayout.MinHeight(20), GUILayout.ExpandWidth(true) };
		timeScaleIndex = EditorGUILayout.Popup(timeScaleIndex, DateTimeSettings.TimeScaleOption.GetLabels(), style, dropdownOptions);
		dt.SelectTimeScaleOption (timeScaleIndex);

		Repaint ();


		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		//needed for play mode
		lastUpdate = EditorApplication.timeSinceStartup;
	}





	void DisplayTimeSettings(){
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));

		string hourLabel = "Hour:";
		string minLabel = "Minute:";
		string secLabel = "Second:";

		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(90) };

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;


		var textDimensions = GUI.skin.label.CalcSize(new GUIContent(hourLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;

		options = new GUILayoutOption[]{ GUILayout.Width(60) };
		hour  = EditorGUILayout.IntField ( hourLabel, dt.Hour(), myStyle, options);
		GUILayout.FlexibleSpace();

		textDimensions = GUI.skin.label.CalcSize(new GUIContent(minLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;
		options = new GUILayoutOption[]{ GUILayout.Width(70) };
		minute = EditorGUILayout.IntField ( minLabel, dt.Minute(), myStyle, options);

		GUILayout.FlexibleSpace();

		textDimensions = GUI.skin.label.CalcSize(new GUIContent(secLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;
		options = new GUILayoutOption[]{ GUILayout.Width(100) };

		string secondStr = dt.Second ().ToString ("##.###");
		float.TryParse (secondStr, out second);
		second = EditorGUILayout.FloatField (secLabel, second, myStyle, options);

		EditorGUILayout.EndHorizontal (  );
		GUILayout.EndVertical();

	}


	void DisplayDateSettings(){
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal(GUILayout.MaxWidth(500));

		string yearLabel  = "Year:";
		string monthLabel = "Month:";
		string dayLabel   = "Day:";

		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width(80) };

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;


		var textDimensions = GUI.skin.label.CalcSize(new GUIContent(yearLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;

		year  = EditorGUILayout.IntField ( yearLabel, dt.Year(), myStyle, options);
		GUILayout.FlexibleSpace();

		textDimensions = GUI.skin.label.CalcSize(new GUIContent(monthLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;

		month = EditorGUILayout.IntField ( monthLabel, dt.Month(), myStyle,options);

		GUILayout.FlexibleSpace();

		textDimensions = GUI.skin.label.CalcSize(new GUIContent(dayLabel));
		EditorGUIUtility.labelWidth = textDimensions.x;


		day = EditorGUILayout.IntField (dayLabel, dt.Day(), myStyle,options);

		EditorGUILayout.EndHorizontal (  );
		GUILayout.EndVertical();
	}

}
