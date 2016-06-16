using UnityEngine;
using UnityEditor;
using System.Collections;


public class DateTimeWindow : EditorWindow {


	int year;

	[Range(1, 12)]
	int month;

	[Range(1, 31)]
	int day;

	bool groupEnabled;
	bool gregorianCalendar = true;

	[MenuItem("Window/Time settings")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(DateTimeWindow));
	}

	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);


		//groupEnabled = EditorGUILayout.BeginToggleGroup ("Date", groupEnabled);
		year = EditorGUILayout.IntField ("Year: ", year);
		month = EditorGUILayout.IntField ("Month: ", month);
		day = EditorGUILayout.IntField ("Day: ", day);

		gregorianCalendar = EditorGUILayout.Toggle ("Use Gregorian calendar", gregorianCalendar);
		//myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		//EditorGUILayout.EndToggleGroup ();
	}
}
