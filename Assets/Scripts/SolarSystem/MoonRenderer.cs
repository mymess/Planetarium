using UnityEngine;
using System.Collections;
using System;

using MathUtils;

public class MoonRenderer : MonoBehaviour {

	private SimController sim;

	private MoonModel moon;

	public float scale = 60;


	void Awake(){
		
	}


	// Use this for initialization
	void Start () {

		sim = SimController.instance;
		moon = sim.skyModel.GetMoon();


		//transform.rotation.SetLookRotation(Camera.main.transform.position);

		transform.Rotate (new Vector3 (0, 90, 0));

		SetScale ();	

		SetPosition ();

		Debug.Log (string.Format ("x={0} y={1} z {2}", transform.position.x, transform.position.y, transform.position.z));
	}
	
	// Update is called once per frame
	void Update () {		
		SetPosition ();
		SetScale ();

		transform.localRotation.SetLookRotation( Camera.main.transform.position );
	}


	void SetPosition(){
		try{

			transform.localRotation =  Quaternion.Euler(new Vector3(0, 90.0f, 0));
			transform.rotation.SetLookRotation(Camera.main.transform.position);

			Vec3D pos = moon.GetRectangularFromEquatorialCoords();
			float x = .8f*sim.radius*(float)pos.x;
			float y = .8f*sim.radius*(float)pos.y;
			float z = .8f*sim.radius*(float)pos.z;

			transform.localPosition = new Vector3 (x, y, z);



		}catch(NullReferenceException n){
		}
	}

	private  void SetScale(){
		if (sim.exaggeratedBodies) {
			transform.localScale = new Vector3 (50.0f, 50.0f, 50.0f);
		} else {
			double diameter = 2*moon.GetSemidiameter () / 30;
			float appDiameter = (float)diameter * sim.diametersScale;
			transform.localScale = new Vector3 (appDiameter, appDiameter, appDiameter);
		}
	}


}
