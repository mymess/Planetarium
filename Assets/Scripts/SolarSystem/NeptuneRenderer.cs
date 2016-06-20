using UnityEngine;
using System.Collections;

public class NeptuneRenderer  : PlanetRenderer{

	protected override void Awake(){
		base.Awake ();
	}


	protected override PlanetModel GetModel ()
	{
		return skyModel.GetPlanets () ["Neptune"] as NeptuneModel;
	}
}
