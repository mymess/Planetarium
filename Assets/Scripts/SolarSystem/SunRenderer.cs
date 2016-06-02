using UnityEngine;
using System.Collections;

using MathUtils;

public class SunRenderer : MonoBehaviour {

	private SunModel sun;

	private SimController sim;

	private float distance;

	private Light dirLight;

	// Use this for initialization
	void Start () {

		sim = SimController.instance;
		distance = .8f * sim.radius;

		sun = sim.skyModel.GetSun();

		float size = 60.0f;//transform.localScale.magnitude;

		transform.localScale = new Vector3 (size, size, size);

		dirLight = GetComponent<Light> ();
		SetPosition ();

		//Debug.Log("light pos -> "+ dirLight.transform.position.ToString ());
	}

	void Update () {
		SetPosition ();
		transform.localRotation.SetLookRotation (-transform.position);
	}

	void SetPosition(){			
		Vec3D v = sun.GetRectangularLocalPosition ();
		transform.position = new Vector3 ((float)v.x, (float)v.y, (float)v.z) * distance;
	}


}
