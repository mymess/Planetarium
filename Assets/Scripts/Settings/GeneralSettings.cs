using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GeneralSettings : MonoBehaviour {

	Color constellationsColor = Color.red;
	public Color ConstellationsColor { get{ return constellationsColor; } set { constellationsColor = value; }}

	bool displayConstellations;
	public bool DisplayConstellations { get { return displayConstellations; }  set { displayConstellations = value; }}

	bool showConstellationNames;
	public bool ShowConstellationNames { get { return showConstellationNames; } set { showConstellationNames = value; }} 


	bool showPlanetNames;
	public bool ShowPlanetNames { get { return showPlanetNames; } set { showPlanetNames = value; } }

	bool showStarNames;
	public bool ShowStarNames { get { return showStarNames; } set { showStarNames = value; } }

	void OnEnabled(){
		constellationsColor = Color.red;
	}

	// Use this for initialization
	void Start () {
		displayConstellations = false;
		//constellationsColor = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
