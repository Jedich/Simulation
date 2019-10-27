using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour {
	public Vector3 AngularVel;
	public void Update(){
		AngularVel = gameObject.GetComponent<Rigidbody>().angularVelocity;
	}
}
