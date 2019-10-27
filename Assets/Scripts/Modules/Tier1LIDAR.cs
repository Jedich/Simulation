using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tier1LIDAR : ViewSensor {
	List<GameObject> tempList = new List<GameObject>();
	public Tier1LIDAR(GameObject t, GameObject p) : base(t, p) {}
	public override void Behaviour() {
		int counter = 0;
		RaycastHit hit = new RaycastHit();
		CarPreferences Car = new CarPreferences();
		string f = "";
		GameObject temp = null;
		for (float i = 0; i <= Car.pref["degreeY"]; i += Car.pref["stepY"]) {
			for (float j = 0; j <= Car.pref["degreeX"]; j += Car.pref["stepX"]) {
				spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(i - Car.pref["degreeY"] / 2, j - Car.pref["degreeX"] / 2, 0);
				temp = null;
				if (Load.pointsToDelete[counter] == null) {
					temp = Instantiate(point);
					Load.pointsToDelete[counter] = temp;
					temp.GetComponent<Point>().PointC(j, i);
				} else temp = Load.pointsToDelete[counter];
				Vector3 cat = Cat(spawned, hit);
				if (cat != Vector3.zero) {
					temp.transform.position = cat;
					f += j + "	" + hit.distance + "\n";
				} else temp.transform.position = new Vector3(0, -1, 0);
				counter++;
			}
		}
		for (int i = 0; i < Load.currentPoints.Count; i++) {
			Load.currentPoints[i].Search();
		}
		Camera.main.GetComponent<Load>().t = f;
		f = "";
		counter = 0;
		spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
	}
}