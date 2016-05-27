using UnityEngine;
using System.Collections;

using MathUtils;

public class MoonRenderer : MonoBehaviour {

	private SimController sim;

	private MoonModel moon;

	public float scale = 30;

	// Use this for initialization
	void Start () {
		sim = SimController.simController;

		moon = sim.moon;



		SetPosition ();


		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale);
		//transform.position = sim.radius*new Vector3(x, y, z);

	}
	
	// Update is called once per frame
	void Update () {
		if (sim.IsUpdated ()) {
			SetPosition ();
		}
	}


	void SetPosition(){
		Vec3D pos = moon.GetRectangularLocalPosition ();
		float x = .5f*sim.radius*(float)pos.x;
		float y = .5f*sim.radius*(float)pos.y;
		float z = .5f*sim.radius*(float)pos.z;
		gameObject.transform.position = new Vector3 (x, y, z);
	}
}
