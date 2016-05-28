using UnityEngine;
using System.Collections;

public class MercuryRenderer : PlanetRenderer{


	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets()["Mercury"] as MercuryModel;
	}

}
