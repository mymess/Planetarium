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
		north.transform.localPosition = new Vector3(0f, radius, 0f);
		east.transform.localPosition = new Vector3 (radius, 0f, 0f);
		west.transform.localPosition = new Vector3 (-radius, 0f, 0f);
		south.transform.localPosition = new Vector3 (0f, -radius, 0f);
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
		north.transform.localScale = new Vector3(scale, scale, 1.0f);
		east.transform.localScale  = new Vector3(scale, scale, 1.0f);
		west.transform.localScale  = new Vector3(scale, scale, 1.0f);
		south.transform.localScale = new Vector3(scale, scale, 1.0f);
	}

}
