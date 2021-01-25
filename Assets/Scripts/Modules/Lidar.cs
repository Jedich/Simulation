using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

class Lidar : Sensor {
	int boundCount = 0;
	public float coordinatesX = 0, coordinatesY = 0;
	GameObject pointInstance = null;
	private RaycastHit currentRay;
	public Dictionary<string, Point> poolMap = new Dictionary<string, Point>();
	GameObject inst;
	private float degY, degX, stepY, stepX;
	private ParticleSystem.Particle temp;

	public override void Init() {
		degY = CarPreferences.map["degreeY"];
		degX = CarPreferences.map["degreeX"];
		stepY = CarPreferences.map["stepY"];
		stepX = CarPreferences.map["stepX"];
		
		pool = GeneratePool((int) degX, (int) degY);

		poolMap = pool.Map;
	}

	public override void Behaviour() {
		boundCount = 0;
		string currentPoint;
		Load.instance.pointCloud.GetParticles(pool.testPool);
		int iter = 0, jter = 0;
		for (float i = 0; i <= degY; i += stepY) {
			for (float j = 0; j <= degX; j += stepX) {
				currentPoint = i.ToString() + j.ToString();
				int index = iter * (int)degY + jter;
				physicalRigidbody.rotation = Quaternion.Euler(i - degY / 2, j - degX / 2, 0);
				try {
					currentRay = Ray(physical.transform, physicalRigidbody, hit);
				}
				catch (EntryPointNotFoundException e) {
					//poolMap[currentPoint].physical.SetActive(false);
					temp = pool.testPool[index];
					temp.position = Vector3.zero;
					pool.testPool[index] = temp;
					continue;
				}
				temp = pool.testPool[index];
				temp.position = currentRay.point;
				temp.startColor = Coloring(temp.position);
				temp.startSize = 0.05f;
				pool.testPool[index] = temp;
				//poolMap[currentPoint].physical.SetActive(true);
				//poolMap[currentPoint].physical.transform.position = currentRay.point;
				jter++;
			}
			iter++;
		}
		physicalRigidbody.rotation = Quaternion.Euler(0, 0, 0);
		Load.instance.pointCloud.Clear();
		Load.instance.pointCloud.SetParticles(pool.testPool.ToArray());
	}

	public override void Snapshot() {
		StreamWriter writer = new StreamWriter(Application.dataPath + "/snap.xyz");
		for (float i = 0; i <= 90; i += 0.5f) {
			for (float j = 0; j <= 360; j++) {
				physicalRigidbody.rotation = Quaternion.Euler(i - 90 / 2, j - 360 / 2, 0);
				writer.Flush();
				RaycastHit currentRay = Ray(physical.transform, physicalRigidbody, hit);
				try {
					writer.WriteLine(currentRay.point.x + " " + currentRay.point.y + " " + currentRay.point.z);
				}
				catch (EntryPointNotFoundException) {
				}
			}
		}
	}
}
//spawnedSample.position, spawnedSample.rotation * Vector3.forward, out hit, 10