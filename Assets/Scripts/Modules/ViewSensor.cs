using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ViewSensor : MonoBehaviour {
	public CarPreferences Car = new CarPreferences();
	public string tierOutput = Application.dataPath + "/pointInformation.txt";
	public List<GameObject> currentPoints = new List<GameObject>();
	public GameObject spawned, point, hitted;
	public ViewSensor(GameObject t, GameObject p) {
		spawned = t;
		point = p;
	}
	public Vector3 Cat(GameObject spawnedSample, RaycastHit hit) {
		if (Physics.Raycast(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward, out hit, 10) && hit.transform != null) {
			Debug.DrawRay(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward * hit.distance, Color.yellow);
			hitted = hit.collider.gameObject;
			return hit.point;
		} else return Vector3.zero;
	}
	public virtual void Behaviour() {}
}
