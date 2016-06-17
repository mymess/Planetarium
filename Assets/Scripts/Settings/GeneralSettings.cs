using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GeneralSettings : MonoBehaviour {

	Color constellationsColor = Color.red;
	public Color ConstellationsColor { get{ return constellationsColor; } set { constellationsColor = value; }}

	bool displayConstellations;
	public bool DisplayConstellations { get { return displayConstellations; }  set { displayConstellations = value; }}

	float constellationLineWidth;
	public float ConstellationLineWidth { get { return constellationLineWidth; }  set { constellationLineWidth = value; }}

	bool showConstellationNames;
	public bool ShowConstellationNames { get { return showConstellationNames; } set { showConstellationNames = value; }} 

	bool constellationSettingsChanged;
	public bool ConstellationSettingsChanged { get { return constellationSettingsChanged; } set { constellationSettingsChanged = value; }} 

	bool showPlanetNames;
	public bool ShowPlanetNames { get { return showPlanetNames; } set { showPlanetNames = value; } }

	bool showStarNames;
	public bool ShowStarNames { get { return showStarNames; } set { showStarNames = value; } }


	bool showMouseHud;
	public bool ShowMouseHud { get { return showMouseHud; } set { showMouseHud = value; } }

	Color mouseHudColor ;
	public Color MouseHudColor { get{ return mouseHudColor; } set { mouseHudColor = value; }}

	bool mouseHudChanged;
	public bool MouseHudChanged { get { return mouseHudChanged; } set { mouseHudChanged = value; } }

	void Start () {
		displayConstellations = false;
		constellationsColor = new Color(1f, 0f, 0f, 0f);
		constellationLineWidth = .35f;
		mouseHudColor = Color.white;
		mouseHudColor.a = 0f;
	}
	

	void Update () {
		
	}

	public bool ShouldConstellationLinesBeRedrawn(){
		return ConstellationSettingsChanged;
	}
}
