using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Modules {
	class Lidar : Sensor {
		private RaycastHit currentRay;
		GameObject inst;
		private float degY, degX, stepY, stepX;
		private ParticleSystem.Particle temp;
		private Transform physicalTransform, carBodyTransform;

		protected override void Init() {
			degY = CarPreferences.current.degreeY;
			degX = CarPreferences.current.degreeX;
			stepY = CarPreferences.current.stepY;
			stepX = CarPreferences.current.stepX;
			relativePosition = new Vector3(0, 0.25f, 0);
			physicalTransform = physical.transform;
			carBodyTransform = transform;
			pool = GeneratePool((int) degX, (int) degY);
		}

		public override void Behaviour() {
			string currentPoint;
			int iter = 0, jter = 0;
			List<ParticleSystem.Particle> testList = new List<ParticleSystem.Particle>();
			for (float i = 0; i <= degY; i += stepY) {
				jter = 0;
				for (float j = 0; j <= degX; j += stepX) {
					currentPoint = iter.ToString() + "-" + jter.ToString();
					int index = iter * (int)degY + jter;
					physicalRigidbody.rotation = Quaternion.Euler(i - degY / 2, j - degX / 2, 0);
					try {
						currentRay = Ray(physicalTransform, physicalRigidbody, hit);
					}
					catch (EntryPointNotFoundException) {
						pool.map[currentPoint].isPresent = false;
						continue;
					}
					pool.map[currentPoint].isPresent = true;
					pool.map[currentPoint].obstacle = false;
					pool.map[currentPoint].position = currentRay.point;
					if (Load.instance.isObserving) {
						temp = new ParticleSystem.Particle();
						temp.position = currentRay.point;
						temp.startSize = 0.05f;
						testList.Add(temp);
					}
					jter++;
				}
				iter++;
			}
			physicalRigidbody.rotation = Quaternion.Euler(0, 0, 0);
			if (Load.instance.isObserving) {
				Load.instance.pointCloud.Clear();
				Load.instance.pointCloud.SetParticles(testList.ToArray());
			}
			physicalTransform.position = carBodyTransform.position + relativePosition;
		}

		public override void Snapshot() {
			StreamWriter writer = new StreamWriter(Application.dataPath + "/snap.xyz");
			for (float i = 0; i <= 90; i += 0.5f) {
				for (float j = 0; j <= 360; j++) {
					physicalRigidbody.rotation = Quaternion.Euler(i - 90 / 2, j - 360 / 2, 0);
					writer.Flush();
					RaycastHit currentRay = Ray(physicalTransform, physicalRigidbody, hit);
					try {
						writer.WriteLine(currentRay.point.x + " " + currentRay.point.y + " " + currentRay.point.z);
					}
					catch (EntryPointNotFoundException) {
					}
				}
			}
		}
	}
}