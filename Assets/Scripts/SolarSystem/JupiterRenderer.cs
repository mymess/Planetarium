using UnityEngine;
using System.Collections;

public class JupiterRenderer : PlanetRenderer {
	


	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets()["Jupiter"] as JupiterModel;
	}
}
