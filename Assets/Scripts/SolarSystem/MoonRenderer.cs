using UnityEngine;
using System.Collections;

using MathUtils;

public class MoonRenderer : MonoBehaviour {

	private SimController sim;

	private MoonModel moon;

	public float scale = 30;


	void Awake(){
		
	}


	// Use this for initialization
	void Start () {

		sim = SimController.instance;
		moon = sim.skyModel.GetMoon();
		SetPosition ();
		SetScale ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (sim.IsUpdated ()) {
			SetPosition ();
			SetScale ();
		}
	}


	void SetPosition(){
		Vec3D pos = moon.GetRectangularLocalPosition ();
		float x = .5f*sim.radius*(float)pos.x;
		float y = .5f*sim.radius*(float)pos.y;
		float z = .5f*sim.radius*(float)pos.z;
		gameObject.transform.position = new Vector3 (x, y, z);
	}

	void SetScale(){
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale);
	}


	IEnumerator Pause(float secs)
	{
		yield return new WaitForSeconds(secs);
	}
}
