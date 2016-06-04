using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class SolarSystemRenderer : MonoBehaviour {


	private SimController sim;

	private SkyModel skyModel;

	private Dictionary<double, double> distances;


	private Dictionary<string, Type> map = new Dictionary<string, Type>(){
		{"Mercury", typeof(MercuryRenderer)},
		{"Venus", typeof(VenusRenderer)},
		{"Mars", typeof(MarsRenderer)},
		{"Jupiter", typeof(JupiterRenderer)},
		{"Saturn", typeof(SaturnRenderer)},
		{"Uranus", typeof(UranusRenderer)},
		{"Neptune", typeof(NeptuneRenderer)}
	};


	// Use this for initialization
	void Start () {
		sim = SimController.instance;
		skyModel = sim.skyModel;

		//DrawSun ();
		//DrawMoon ();

		//DrawPlanets();
	}




	// Update is called once per frame
	void Update () {

	}



	/*
	void DrawSun(){
		GameObject sphere = GameObject.Find ("Sun/Sphere");
		sphere.AddComponent<SunRenderer> ();
	}

	void DrawMoon(){
		GameObject sphere = GameObject.Find ("Moon/Sphere");
		sphere.AddComponent<MoonRenderer> ();
	}

	void DrawPlanets(){
		foreach (KeyValuePair<string, PlanetModel> planet in skyModel.GetPlanets()) {
			string name = planet.Key;
			GameObject sphere = GameObject.Find (name+"/Sphere");
			if(name.Equals("Mercury")){
				sphere.AddComponent<MercuryRenderer>();
			}else if(name.Equals("Venus")){
				sphere.AddComponent<VenusRenderer>();
			}else if(name.Equals("Mars")){
				sphere.AddComponent<MarsRenderer>();
			}else if(name.Equals("Jupiter")){
				sphere.AddComponent<JupiterRenderer>();
			}else if(name.Equals("Saturn")){
				sphere.AddComponent<SaturnRenderer>();
			}else if(name.Equals("Uranus")){
				sphere.AddComponent<UranusRenderer>();
			}else if(name.Equals("Neptune")){
				sphere.AddComponent<NeptuneRenderer>();
			}

		}
	}
	*/



}
