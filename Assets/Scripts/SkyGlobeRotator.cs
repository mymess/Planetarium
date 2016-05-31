using UnityEngine;
using System.Collections;
using System;

using MathUtils;

public class SkyGlobeRotator : MonoBehaviour {


	private SimController sim;

	private SkyModel skyModel;

	private Vector3 earthAxis;

	// Use this for initialization
	void Start () {
		sim = SimController.instance;
		skyModel = sim.skyModel;


		LocationData location = sim.GetLocation ();
		double angle = 90.0d-location.latitude;

		earthAxis = new Vector3 (0.0f, (float) Math.Sin (angle * M.DEG2RAD), (float)Math.Cos (angle * M.DEG2RAD));

		Quaternion q = Quaternion.Euler((float) angle, 0, 0);
		gameObject.transform.localRotation = q;


		//correction for hour angle
		//TODO



		Debug.Log (string.Format("Angle {0}", angle));
		Debug.Log (string.Format("Quaternion {0}", q));
	}
	
	// Update is called once per frame
	void Update () {
		double jd = skyModel.GetJD ();
		LocationData location = skyModel.GetLocation ();

		if (sim.IsPlayMode () && sim.IsLocationUpdated ()) {
			//reset
			gameObject.transform.rotation = Quaternion.identity;

			//correction for latitude		
			double latitudeRotationAngle = 90.0d - location.latitude;

			//gameObject.transform.Rotate( (float)latitudeRotationAngle, 0, 0 ) ;

			earthAxis = skyModel.GetEarthAxis ();

			//correction for hour angle
			double hourAngleRotationInDegrees = skyModel.GetHourAngleOfAriesPoint () * 15d;

			gameObject.transform.Rotate (0.0f, (float)hourAngleRotationInDegrees, 0.0f);
			gameObject.transform.Rotate ((float) latitudeRotationAngle, 0.0f, 0.0f);

		} else if (!sim.IsPlayMode() && sim.IsTimeOrLocationUpdated ()) {
			//reset
			gameObject.transform.rotation = Quaternion.identity;

			//correction for latitude
			double latitudeRotationAngle = 90.0d - location.latitude;

			//gameObject.transform.Rotate( (float)latitudeRotationAngle, 0, 0 ) ;

			earthAxis = skyModel.GetEarthAxis ();

			//correction for hour angle
			double hourAngleRotationInDegrees = skyModel.GetHourAngleOfAriesPoint () * 15d;

			gameObject.transform.Rotate(earthAxis,  (float)hourAngleRotationInDegrees);
			gameObject.transform.Rotate ((float) latitudeRotationAngle, 0.0f, 0.0f);


		}

	}
}
