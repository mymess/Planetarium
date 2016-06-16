using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(GeneralSettings))]
public sealed class SettingsEditor : Editor {

	GeneralSettings settings;

	void OnEnable()
	{
		settings = (GeneralSettings) target;
	}

	public override void OnInspectorGUI()
	{
		DisplayConstellationSettings ();

	}


	private void DisplayConstellationSettings()
	{
		GUILayout.Label ("Constellations", EditorStyles.boldLabel);
		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleRight;

		GUILayoutOption[] options = { };
		settings.DisplayConstellations = EditorGUILayout.BeginToggleGroup("Constellations", settings.DisplayConstellations);

		settings.ConstellationsColor = EditorGUILayout.ColorField("Color", settings.ConstellationsColor, options);

		EditorGUILayout.EndToggleGroup ();
	}
}
