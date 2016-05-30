using UnityEngine;
using System.Collections;


using MathUtils;

public class SkyGlobeRotator : MonoBehaviour {


	private SimController sim;

	// Use this for initialization
	void Start () {
		sim = SimController.instance;
		LocationData location = sim.GetLocation ();
		double angle = location.latitude;
		//transform.rotation = new Quaternion( 1.0f, 0.0f, 0.0f, angle);

		Debug.Log (string.Format("Angle {0}", angle));
	}
	
	// Update is called once per frame
	void Update () {
		double jd = sim.GetJD ();
		LocationData location = sim.GetLocation ();

		if (sim.IsUpdated ()) {
			double angle = location.latitude;
			Quaternion q = new Quaternion (1.0f, 0.0f, 0.0f, (float)angle);
			//transform.rotation = q;

			Debug.Log (string.Format("Angle {0}", angle));
			Debug.Log (string.Format("Quaternion {0}", q));
		}
			


	}
}
