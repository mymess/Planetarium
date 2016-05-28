using UnityEngine;
using System.Collections;

public class UranusRenderer : PlanetRenderer{



	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets () ["Uranus"] as UranusModel;
	}
}
