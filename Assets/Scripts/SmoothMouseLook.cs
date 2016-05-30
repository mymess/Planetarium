using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public bool activateOnClick = false;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;

	float curZoomPos, zoomTo; // curZoomPos will be the value
	float zoomFrom = 20f;


	void Update ()
	{	

		if (activateOnClick && Input.GetMouseButton (0)) {

			if (axes == RotationAxes.MouseXAndY) {			
				rotAverageY = 0f;
				rotAverageX = 0f;

				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;

				rotArrayY.Add (rotationY);
				rotArrayX.Add (rotationX);

				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt (0);
				}
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt (0);
				}

				for (int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY [j];
				}
				for (int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX [i];
				}

				rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;

				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);

				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			} else if (axes == RotationAxes.MouseX) {			
				rotAverageX = 0f;

				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;

				rotArrayX.Add (rotationX);

				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt (0);
				}
				for (int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX [i];
				}
				rotAverageX /= rotArrayX.Count;

				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;			
			} else {			
				rotAverageY = 0f;

				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;

				rotArrayY.Add (rotationY);

				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt (0);
				}
				for (int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY [j];
				}
				rotAverageY /= rotArrayY.Count;

				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);

				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}

		/***
		 *  ZOOM 
		 **/

		// Attaches the float y to scrollwheel up or down
		float y = Input.mouseScrollDelta.y;

		// If the wheel goes up it, decrement 5 from "zoomTo"
		if (y >= 1)
		{
			zoomTo -= 5f;
		}

		// If the wheel goes down, increment 5 to "zoomTo"
		else if (y >= -1) {
			zoomTo += 5f;
		}

		if(y>=1 || y<=-1){
			// creates a value to raise and lower the camera's field of view
			curZoomPos =  zoomFrom + zoomTo;

			curZoomPos = Mathf.Clamp (curZoomPos, 3.5f, 55f);	

			// Stops "zoomTo" value at the nearest and farthest zoom value you desire
			zoomTo = Mathf.Clamp (zoomTo, -15f, 30f);

			// Makes the actual change to Field Of View
			//Camera.main.fieldOfView = curZoomPos;
			Camera.main.fieldOfView = curZoomPos;
		}



	}

	void Start ()
	{		
		Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}