using UnityEngine;
using System.Collections;

public class UranusRenderer : PlanetRenderer{

	protected override void Awake(){
		base.Awake ();
	}


	protected override PlanetModel GetModel ()
	{
		return skyModel.GetPlanets () ["Uranus"] as UranusModel;
	}
}
