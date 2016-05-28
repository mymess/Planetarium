using UnityEngine;
using System.Collections;

public class NeptuneRenderer  : PlanetRenderer{

	protected override PlanetModel GetModel ()
	{
		return sim.skyModel.GetPlanets () ["Neptune"] as NeptuneModel;
	}
}
