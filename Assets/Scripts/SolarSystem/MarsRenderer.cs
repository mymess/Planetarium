using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsRenderer : PlanetRenderer
{



	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets()["Mars"] as MarsModel;
	}

}
