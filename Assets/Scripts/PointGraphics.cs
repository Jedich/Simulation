using System;
using UnityEngine;

public class PointGraphics : MonoBehaviour {
	void FixedUpdate() {
		transform.LookAt(transform.position - Camera.main.transform.position);
	}
	

}
