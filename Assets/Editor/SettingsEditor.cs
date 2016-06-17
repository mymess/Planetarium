using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(GeneralSettings))]
public sealed class SettingsEditor : Editor {

	GeneralSettings settings;

	bool constellationsMenuChanged;


	void OnEnable()
	{
		settings = (GeneralSettings) target;
	}

	public override void OnInspectorGUI()
	{
		DisplayConstellationSettings ();

		DisplayMouseHudSettings ();
	}

	private void DisplayMouseHudSettings (){
		EditorGUI.BeginChangeCheck ();

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		GUILayoutOption[] options = { };
		settings.ShowMouseHud = EditorGUILayout.BeginToggleGroup("Mouse HUD", settings.ShowMouseHud);

		settings.MouseHudColor = EditorGUILayout.ColorField("Color", settings.MouseHudColor, options);

		EditorGUILayout.EndToggleGroup ();

		settings.MouseHudChanged = EditorGUI.EndChangeCheck ();

	}


	private void DisplayConstellationSettings()
	{
		EditorGUI.BeginChangeCheck ();

		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		GUILayoutOption[] options = { };
		settings.DisplayConstellations = EditorGUILayout.BeginToggleGroup("Constellations", settings.DisplayConstellations);
		settings.ShowConstellationNames = EditorGUILayout.ToggleLeft ("Show names", settings.ShowConstellationNames, options);
		settings.ConstellationsColor = EditorGUILayout.ColorField("Color", settings.ConstellationsColor, options);

		settings.ConstellationLineWidth = EditorGUILayout.FloatField ("Line width: ", settings.ConstellationLineWidth);

		EditorGUILayout.EndToggleGroup ();

		settings.ConstellationSettingsChanged = EditorGUI.EndChangeCheck ();

	}
}
