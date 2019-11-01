using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tier1LIDAR : ViewSensor, IGetBehaviour {
	int boundCount = 0, megaCount = 0;
	public float coordinatesX = 0, coordinatesY = 0;
	float cX, cY;
	void Start() {
		Car = new CarPreferences();
		hit = new RaycastHit();
	}
	public override void Behaviour() {
		int counter = 0;
		string f = "";
		GameObject temp = null;
		boundCount = 0;
		cX = coordinatesX;
		cY = coordinatesY;
		for (float i = cY; i <= Car.pref["degreeY"]; i += Car.pref["stepY"]) {
			for (float j = cX; j <= Car.pref["degreeX"]; j += Car.pref["stepX"]) {
				if (boundCount < Car.pref["cycleBound"]) {
					spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(i - Car.pref["degreeY"] / 2, j - Car.pref["degreeX"] / 2, 0);
					temp = null;
					if (Load.pointsToDelete.ContainsKey(i.ToString() + j.ToString())) {
						temp = Instantiate(point, pointCloudHandler);
						temp.name = "point-" + j + "-" + i;
						Load.pointsToDelete.Add(i.ToString() + j.ToString(), temp);
						temp.GetComponent<Point>().PointC(j, i);
					} else temp = Load.pointsToDelete[i.ToString() + j.ToString()];
					Vector3 cat = Cat(spawned, hit);
					if (cat != Vector3.zero) {
						temp.transform.position = cat;
						f += j + "	" + hit.distance + "\n";
					} else temp.transform.position = new Vector3(0, -1, 0);
					coordinatesX = j;
					coordinatesY = i;
					counter++;
					boundCount++;
				}
			}
		}
		for (int i = 0; i < Load.currentPoints.Count; i++) {
			Load.currentPoints[i].Search();
		}
		Camera.main.GetComponent<Load>().t = f;
		f = "";
		counter = 0;
		megaCount++;
		boundCount = 0;
		if (coordinatesY == Car.pref["degreeY"]) { coordinatesY = 0; megaCount = 0; }
		if (coordinatesX == Car.pref["degreeX"]) { coordinatesX = 0; megaCount = 0; }
		spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
	}
}