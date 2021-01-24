using System;
using UnityEngine;
public abstract class Sensor : MonoBehaviour, IGetBehaviour {
	public GameObject point, spawned;
	public Transform pointCloudHandler;
	public RaycastHit hit = new RaycastHit();
	public abstract void Behaviour();
	public abstract void Snapshot();
	
	public RaycastHit Ray(Transform spawnedSample, Rigidbody spawnedSampleRigidbody, RaycastHit hit) {
		if (Physics.Raycast(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward, out hit, CarPreferences.map["rayDist"]) && hit.transform != null) {
			//Debug.DrawRay(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward * hit.distance, Color.yellow);
			return hit;
		}
		throw new NullReferenceException("hit not registered");
	}
}