using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

class Tier1LIDAR : Sensor, IGetBehaviour {
	int boundCount = 0;
	public float coordinatesX = 0, coordinatesY = 0;
	GameObject pointInstance = null;
	Vector3 cat;
	Rigidbody spawnedRigidbody;
	public List<GameObject> pool = new List<GameObject>();
	GameObject inst;
	bool poolCompleted = false;
	void Start() {
		spawnedRigidbody = spawned.GetComponent<Rigidbody>();
		hit = new RaycastHit();
		degY = CarPreferences.map["degreeY"];
		degX = CarPreferences.map["degreeX"];
		stepY = CarPreferences.map["stepY"];
		stepX = CarPreferences.map["stepX"];
		// for (int i = 0; i < (degX / stepX) * (degY / stepY); i++) {
		// 	//Debug.Log((degX / stepX) * (degY / stepY));
		// 	pool.Add(Instantiate(point, pointCloudHandler));
		// }
		poolCompleted = true;
	}
	float degY, degX, stepY, stepX;
	public override void Behaviour() {
		if (poolCompleted) {
			boundCount = 0;
			for (float i = 0; i <= degY; i += stepY) {
				for (float j = 0; j <= degX; j += stepX) {
					spawnedRigidbody.rotation = Quaternion.Euler(i - degY / 2, j - degX / 2, 0);
					cat = Cat(spawned.transform, spawnedRigidbody, hit);
					pointInstance = null;
					if (!Load.pointsToDelete.ContainsKey(i.ToString() + j.ToString())) {
						if (boundCount < CarPreferences.map["cycleBound"]) {
							pointInstance =Instantiate(point, pointCloudHandler);
							//pool.RemoveAt(0);
							pointInstance.name = "point-" + j + "-" + i;
							Load.pointsToDelete.Add(i.ToString() + j.ToString(), pointInstance);
							pointInstance.GetComponent<Point>().PointC(j, i);
							boundCount++;
						}

					} else pointInstance = Load.pointsToDelete[i.ToString() + j.ToString()];
					if (pointInstance != null) {
						if (cat != Vector3.zero) {
							pointInstance.SetActive(true);
							pointInstance.transform.position = cat;
						} else pointInstance.SetActive(false);
					}
				}
				//yield return new WaitForEndOfFrame();
			}
			if (CarPreferences.map["coloring"] == 0) {
				for (int i = 0; i < Load.currentPoints.Count; i++) {
					Load.currentPoints[i].Search();
				}
			}
			spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
		}
	}
	public override void Snapshot() {
		StreamWriter writer = new StreamWriter(Application.dataPath + "/snap.xyz");
		for (float i = 0; i <= 90; i += 0.5f) {
			for (float j = 0; j <= 360; j++) {
				spawnedRigidbody.rotation = Quaternion.Euler(i - 90 / 2, j - 360 / 2, 0);
				writer.Flush();
				Vector3 cat = Cat(spawned.transform, spawnedRigidbody, hit);
				if (cat != Vector3.zero)
					writer.WriteLine(cat.x + " " + cat.y + " " + cat.z);
			}
		}
	}
}
//spawnedSample.position, spawnedSample.rotation * Vector3.forward, out hit, 10