using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public float ZoomAmount = 0;
	public float MaxToClamp = 10;
	public float ROTSpeed = 10;
	public float speedMod = 1;
	public static Transform target;
	private Vector3 lastPosition;
	void Update() {
		transform.LookAt(target);
		ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
		ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
		var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp - Mathf.Abs(ZoomAmount));
		gameObject.transform.Translate(0, 0, translate * ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
		if(Input.GetMouseButton(0)) {
			transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * speedMod);
			transform.RotateAround(target.position, transform.up, Input.GetAxis("Mouse X") * speedMod);
		}
	}
}