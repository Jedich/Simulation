using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour {
	public Vector3 angularVel, acceleration, lastVelocity;
	private Rigidbody parentRigid;
	void Start() {
		parentRigid = transform.parent.gameObject.GetComponent<Rigidbody>();
	}
	private void FixedUpdate() {
		angularVel = parentRigid.angularVelocity;
		acceleration = (parentRigid.velocity - lastVelocity) / Time.fixedDeltaTime;
		lastVelocity = parentRigid.velocity;
	}
}