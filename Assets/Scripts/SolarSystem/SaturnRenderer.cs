using UnityEngine;
using System.Collections;

public class SaturnRenderer : PlanetRenderer {


	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets()["Saturn"] as SaturnModel;
	}

}
