using UnityEngine;
using System.Collections;

public class JupiterRenderer : PlanetRenderer {
	

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		return skyModel.GetPlanets()["Jupiter"] as JupiterModel;
	}
}
