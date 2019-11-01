using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ViewSensor : Sensor, IGetBehaviour {
	public CarPreferences Car = new CarPreferences();
	public RaycastHit hit = new RaycastHit();
	public string tierOutput = Application.dataPath + "/pointInformation.txt";
	public List<GameObject> currentPoints = new List<GameObject>();
	public GameObject hitted;
	public Vector3 Cat(GameObject spawnedSample, RaycastHit hit) {
		if (Physics.Raycast(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward, out hit, 10) && hit.transform != null) {
			Debug.DrawRay(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward * hit.distance, Color.yellow);
			hitted = hit.collider.gameObject;
			return hit.point;
		} else return Vector3.zero;
	}
	public override void Behaviour() {}
}
