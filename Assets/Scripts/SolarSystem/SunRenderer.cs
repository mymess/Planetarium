using UnityEngine;
using System.Collections;

using MathUtils;

public class SunRenderer : MonoBehaviour {

	private SunModel sun;

	private SimController sim;

	// Use this for initialization
	void Start () {

		sim = SimController.instance;

		sun = sim.skyModel.GetSun();

		float size = transform.localScale.magnitude;
		float diameter = (float)sun.GetDiameter ();
		transform.localScale = new Vector3 (size*diameter, size*diameter, size*diameter);


		SetPosition ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetPosition(){
		Vec3D pos = sun.GetRectangularLocalPosition ();
		float x = .5f*sim.radius*(float)pos.x;
		float y = .5f*sim.radius*(float)pos.y;
		float z = .5f*sim.radius*(float)pos.z;

		//transform.position = new Vector3(x, y, z);
		gameObject.transform.position = new Vector3(x, y, z);
	}


}
