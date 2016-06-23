using UnityEngine;

using System.Collections;

public class CrossHairRenderer : MonoBehaviour {

	public GameObject north;
	public GameObject east;
	public GameObject west;
	public GameObject south;


	public float radius = 3.0f;

	public float seconds_per_lap = 15.0f;

	public float scale = 1.0f;

	public float sizeOnScreen = 10.0f;

	// Use this for initialization
	void Start () {
		UpdateCrosshairRadius ();

		transform.LookAt (Camera.main.transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		UpdateCrosshairRadius ();
		SetCrosshairOrientation ();

		SetScale ();
	}


	public void UpdateCrosshairRadius (){

		float radiusUpdated = .1f*Camera.main.fieldOfView + radius;

		north.transform.localPosition = new Vector3(0f, radiusUpdated, 0f);
		east.transform.localPosition = new Vector3 (radiusUpdated, 0f, 0f);
		west.transform.localPosition = new Vector3 (-radiusUpdated, 0f, 0f);
		south.transform.localPosition = new Vector3 (0f, -radiusUpdated, 0f);
	}

	void SetCrosshairOrientation(){
		
		Vector3 z = new Vector3(
			Mathf.Sin(Time.time/seconds_per_lap * 360f*Mathf.Deg2Rad), 
			-Mathf.Cos(Time.time/seconds_per_lap * 360f*Mathf.Deg2Rad), 
			0.0f);

		Vector3 zWorld = Camera.main.transform.localToWorldMatrix * z;


		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, zWorld);
	}


	public void SetScale(){

		Vector3 a = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 b = new Vector3(a.x, a.y + sizeOnScreen, a.z);

		Vector3 aa = Camera.main.ScreenToWorldPoint(a);
		Vector3 bb = Camera.main.ScreenToWorldPoint(b);

		Vector3 localScale = Vector3.one * (aa - bb).magnitude;
		//transform.localScale = localScale;

		north.transform.localScale = 1.2f*localScale; //new Vector3(scale, scale, 1.0f);
		east.transform.localScale = 1.2f*localScale; //new Vector3(scale, scale, 1.0f);
		west.transform.localScale  = 1.2f*localScale; //new Vector3(scale, scale, 1.0f);
		south.transform.localScale = 1.2f*localScale; //new Vector3(scale, scale, 1.0f);

	}

}
