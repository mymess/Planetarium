using UnityEngine;
using System.Collections;
using System;
using MathUtils;

public abstract class PlanetRenderer : MonoBehaviour {


	protected SimController sim;

	protected abstract PlanetModel GetModel();

	private PlanetModel model;

	private float distance;

	private float scale = 40f;


	// Use this for initialization
	void Start () {
		sim = SimController.INSTANCE;
		distance = .8f * sim.radius;
		model = GetModel ();

		SetPosition ();
		SetScale ();
	}
	
	// Update is called once per frame
	void Update () {		
		
		SetPosition ();

		SetRotation ();

		SetScale ();

	}

	public void SetPosition(){
		try{
			Vec3D v = model.GetRectangularFromEquatorialCoords();
			transform.localPosition = new Vector3 ((float)v.x,(float) v.y,(float) v.z) * distance;


		}catch(NullReferenceException n){
			
		}
	}

	private void SetRotation(){
		//double p = model.GetParallacticAngle ();
		//transform.localRotation = Quaternion.identity;


		transform.localRotation.SetLookRotation( Camera.main.transform.position );

		//transform.localRotation = Quaternion.AngleAxis((float)p-90f, Vector3.forward);
	}

	protected  void SetScale(){
		if (sim.exaggeratedBodies) {
			transform.localScale = new Vector3 (50.0f, 50.0f, 50.0f);
		} else {
			double diameter = 2*model.GetSemidiameter ();
			float appDiameter = (float)diameter * sim.diametersScale;
			transform.localScale = new Vector3 (appDiameter, appDiameter, appDiameter);

		}
	}




}
