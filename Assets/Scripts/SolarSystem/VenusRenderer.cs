using UnityEngine;
using System.Collections;

public class VenusRenderer : PlanetRenderer  {

	protected override void Awake(){
		base.Awake ();
	}

	protected override PlanetModel GetModel ()
	{
		return skyModel.GetPlanets ()["Venus"] as VenusModel;
	}
}
