using UnityEngine;

namespace Modules {
	public class Gyroscope : MonoBehaviour {
		public Vector3 angularVel, acceleration, lastVelocity, rotation;
		private Rigidbody parentRigid;
		void Start() {
			parentRigid = transform.parent.gameObject.GetComponent<Rigidbody>();
		}
		private void FixedUpdate() {
			angularVel = parentRigid.angularVelocity;
			rotation -= angularVel;
			acceleration = (parentRigid.velocity - lastVelocity) / Time.fixedDeltaTime;
			lastVelocity = parentRigid.velocity;
		}
	}
}