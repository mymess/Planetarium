using UnityEngine;
using System.Collections;

public class SaturnRenderer : PlanetRenderer {

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets()["Saturn"] as SaturnModel;
	}

}
