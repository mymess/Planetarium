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
		sim = SimController.instance;
		distance = .5f * sim.radius;
		model = GetModel ();

		SetPosition ();
		SetScale ();
	}
	
	// Update is called once per frame
	void Update () {		
		SetPosition ();
	}

	public void SetPosition(){
		try{
			Vec3D v = model.GetRectangularLocalPosition ();
			gameObject.transform.position = new Vector3 ((float)v.x,(float) v.y,(float) v.z) * distance;
		}catch(NullReferenceException n){
			
		}
	}

	void SetScale(){
		gameObject.transform.localScale = new Vector3 (scale, scale, scale);
	}
}
