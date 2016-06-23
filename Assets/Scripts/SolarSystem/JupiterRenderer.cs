using UnityEngine;
using System.Collections;

public class JupiterRenderer : PlanetRenderer {
	

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		if (skyModel == null) {
			Awake ();
		}
		return skyModel.GetPlanets()["Jupiter"] as JupiterModel;
	}
}
