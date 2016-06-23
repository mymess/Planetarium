using UnityEngine;
using System.Collections;

public class MercuryRenderer : PlanetRenderer{

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		if (skyModel == null) {
			Awake ();
		}
		return skyModel.GetPlanets()["Mercury"] as MercuryModel;
	}

}
