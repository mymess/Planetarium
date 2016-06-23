using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsRenderer : PlanetRenderer
{
	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		if (skyModel == null) {
			Awake ();
		}
		return skyModel.GetPlanets()["Mars"] as MarsModel;
	}

}
