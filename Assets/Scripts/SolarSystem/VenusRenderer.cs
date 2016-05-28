using UnityEngine;
using System.Collections;

public class VenusRenderer : PlanetRenderer  {

	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets ()["Venus"] as VenusModel;
	}
}
