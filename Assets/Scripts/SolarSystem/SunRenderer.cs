using UnityEngine;
using System.Collections;

using MathUtils;

public class SunRenderer : MonoBehaviour {

	private SunModel sun;

	private SimController sim;

	private float distance;

	// Use this for initialization
	void Start () {

		sim = SimController.instance;
		distance = .8f * sim.radius;

		sun = sim.skyModel.GetSun();

		float size = 60.0f;//transform.localScale.magnitude;
		float diameter = (float)sun.GetDiameter ();
		transform.localScale = new Vector3 (size, size, size);


		SetPosition ();

	}

	void Update () {
		SetPosition ();
	}

	void SetPosition(){
		
		Vec3D v = sun.GetRectangularLocalPosition ();
		gameObject.transform.position = new Vector3 ((float)v.x,(float) v.y,(float) v.z) * distance;
	}


}
