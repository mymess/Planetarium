using UnityEngine;
using System.Collections;

public class MercuryRenderer : PlanetRenderer{

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		return skyModel.GetPlanets()["Mercury"] as MercuryModel;
	}

}
