using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ViewSensor : Sensor, IGetBehaviour {
	public CarPreferences Car = new CarPreferences();
	public RaycastHit hit = new RaycastHit();
	public string tierOutput = Application.dataPath + "/pointInformation.txt";
	public Vector3 shift;
	public List<GameObject> currentPoints = new List<GameObject>();
	public GameObject hitted;
	public Vector3 Cat(Transform spawnedSample, Rigidbody spawnedSampleRigidbody, RaycastHit hit) {
		if (Physics.Raycast(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward, out hit, Car.pref["rayDist"]) && hit.transform != null) {
			Debug.DrawRay(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward * hit.distance, Color.yellow);
			hitted = hit.collider.gameObject;
			return hit.point;
		} else return Vector3.zero;
	}
	public override void Behaviour() {}
	public override void Snapshot() {}
}