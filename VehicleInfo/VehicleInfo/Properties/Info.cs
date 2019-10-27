using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace VehicleInfo {
	public class CarPreferences {
		private StreamReader read;
		public Dictionary<string, float> pref = new Dictionary<string, float>();
		public CarPreferences() {
			read = File.OpenText(Application.dataPath + "/baseSettings.ini");
			string allLines = read.ReadToEnd();
			read.Close();
			string[] currLine = allLines.Trim().Split('\n');
			for(int i = 0; i < currLine.Length; i++)
				if(currLine[i] != "<end>")
					pref.Add(currLine[i].Substring(0, currLine[i].IndexOf("=")), float.Parse(currLine[i].Substring(currLine[i].IndexOf("=") + 1)));

		}
	}
	public class Gyroscope : MonoBehaviour {
		public Vector3 AngularVel;
		public void Update() {
			AngularVel = gameObject.GetComponent<Rigidbody>().angularVelocity;
		}
	}
	class ViewSensor : MonoBehaviour {
		[SerializeField]
		public List<GameObject> floorPoints = new List<GameObject>();
		[SerializeField]
		public List<GameObject> obstPoints = new List<GameObject>();
		public CarPreferences Car = new CarPreferences();
		public string tierOutput = Application.dataPath + "/pointInformation.txt";
		public List<GameObject> currentPoints = new List<GameObject>();
		public GameObject spawned, point, hitted;
		public ViewSensor(GameObject t, GameObject p) {
			spawned = t;
			point = p;
		}
		public Vector3 Cat(GameObject spawnedSample, RaycastHit hit) {
			if(Physics.Raycast(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward, out hit, 10) && hit.transform != null) {
				Debug.DrawRay(spawnedSample.transform.position, spawnedSample.GetComponent<Rigidbody>().rotation * Vector3.forward * hit.distance, Color.yellow);
				hitted = hit.collider.gameObject;
				return hit.point;
			} else return Vector3.zero;
		}
		public virtual void Behaviour() { }
	}
	class Tier1LIDAR : ViewSensor {
		public static GameObject[] pointsToDelete;
		public Tier1LIDAR(GameObject t, GameObject p) : base(t, p) { }
		public override void Behaviour() {
			int counter = 0;
			RaycastHit hit = new RaycastHit();
			CarPreferences Car = new CarPreferences();
			string f = "";
			for(float i = 0; i <= Car.pref["degreeY"]; i += Car.pref["stepY"]) {
				for(float j = 0; j <= Car.pref["degreeX"]; j += Car.pref["stepX"]) {
					spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(i - Car.pref["degreeY"] / 2, j - Car.pref["degreeX"] / 2, 0);
					GameObject temp = null;
					if(pointsToDelete[counter] == null) {
						temp = Instantiate(point);
						pointsToDelete[counter] = temp;
					}
					Vector3 cat = Cat(spawned, hit);
					temp = pointsToDelete[counter];
					if(cat != Vector3.zero) {
						temp.transform.position = cat;
						f += j + "	" + hit.distance + "\n";
						if(hitted.gameObject.layer == 9) {
							floorPoints.Add(temp);
						} else {
							obstPoints.Add(temp);
						}
					} else temp.transform.position = new Vector3(0, -1, 0);
					counter++;
				}
			}
			f = "";
			counter = 0;
			spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
