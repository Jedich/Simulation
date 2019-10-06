using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public Transform leftMesh, rightMesh;
	public bool motor;
	public bool steering;
}

public class CarController : MonoBehaviour {
	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;
	public static float motorMultiplier = 0, angleMultiplier = 0;
	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals() {
		Vector3 position;
		Quaternion rotation;
		foreach(AxleInfo axleInfo in axleInfos) {
			axleInfo.leftWheel.GetWorldPose(out position, out rotation);
			axleInfo.leftMesh.transform.position = position;
			axleInfo.leftMesh.transform.rotation = rotation;
			axleInfo.rightWheel.GetWorldPose(out position, out rotation);
			axleInfo.rightMesh.transform.position = position;
			axleInfo.rightMesh.transform.rotation = rotation /*new Quaternion(axleInfo.leftWheel.transform.parent.rotation.x + 90, axleInfo.leftWheel.transform.parent.rotation.y + 90, axleInfo.leftWheel.transform.parent.rotation.z, axleInfo.leftWheel.transform.parent.rotation.w)*/;
		}
	}

	public void FixedUpdate() {
		float motor = maxMotorTorque * motorMultiplier/* * Input.GetAxis("Vertical")*/;
		float steering = maxSteeringAngle * angleMultiplier/* * Input.GetAxis("Horizontal")*/;

		foreach(AxleInfo axleInfo in axleInfos) {
			if(axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if(axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyLocalPositionToVisuals();
		}
	}
}
 