using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour {
	public string t = "";
	bool turn = false;
	public GameObject carBody, defaultWheel, waypoint;
	public GameObject[] randomGameObjects;
	public int randomObjNumber;
	public GameObject distantPoint;
	public float distance = 100000;
	private CarPreferences Car;
	public float steerDirection;
	public bool back = false;
	public LineRenderer nearest;
	public int state;
	public Text pos;
	Vector3 direction;
	public GameObject[] IRprefabs;
	public static List<Vector3> points = new List<Vector3>();
	public static GameObject[] pointsToDelete;
	List<GameObject> floorArr = new List<GameObject>();
	List<GameObject> wallArr = new List<GameObject>();
	public void Updater() {
		Car = new CarPreferences();
	}
	public IEnumerator Clearing() {
		yield return new WaitForSeconds(3);
		for (int i = 0; i < pointsToDelete.Length; i++)
			if (pointsToDelete[i] != null && pointsToDelete[i].transform.position == new Vector3(0, -1, 0))
				Destroy(pointsToDelete[i]);
	}
	public void DistantPointSearch(List<GameObject> floorArr, List<GameObject> wallArr){
				foreach (GameObject point in floorArr)
			point.GetComponent<Renderer>().material.color = Color.green;
		foreach (GameObject point in wallArr) {
			point.GetComponent<Renderer>().material.color = Color.red;
			if (Vector3.Distance(point.transform.position, IRprefabs[0].transform.position) < distance) {
				distance = Vector3.Distance(point.transform.position, IRprefabs[0].transform.position);
				distantPoint = point;
			}
		}
	}
	private void Update() {
		nearest.GetComponent<LineRenderer>().SetPosition(0, IRprefabs[0].transform.position);
		nearest.GetComponent<LineRenderer>().SetPosition(1, distantPoint.transform.position);
		pos.text = distantPoint.transform.position.ToString();
		IRprefabs[0].transform.position = new Vector3(IRprefabs[0].transform.parent.position.x, IRprefabs[0].transform.parent.position.y + 0.25f, IRprefabs[0].transform.parent.position.z);
	}

	public void FixedUpdate() {
		distance = 100;
		if (Car.pref["Tier"] == 0) {
			BasicSensor lidar = new BasicSensor(IRprefabs[0], IRprefabs[1]);
			DistantPointSearch(lidar.floorPoints, lidar.obstPoints);
			lidar.singleCast();
		} else if (Car.pref["Tier"] == 2) {
			Tier1LIDAR lidar = new Tier1LIDAR(IRprefabs[0], IRprefabs[1]);
			lidar.vertSpin();
			DistantPointSearch(lidar.floorPoints, lidar.obstPoints);
		}
	}

	public void Start() {
		StartCoroutine(Clearing());
		Car = new CarPreferences();
		Time.fixedDeltaTime = Car.pref["updateDelay"];
		System.Array.Resize(ref pointsToDelete, (int)(Car.pref["degreeX"] * Car.pref["degreeY"]));
		GameObject temp;
		temp = Instantiate(carBody);
		IRprefabs[0] = Instantiate(IRprefabs[2], temp.transform);
		IRprefabs[0].transform.position = new Vector3(temp.transform.position.x, 0f, temp.transform.position.z);
		temp.transform.position = new Vector3(0, 1f, 0);
		IRprefabs[0].transform.parent = temp.transform;
		Camera.main.transform.parent = temp.transform;
		CameraMovement.target = temp.transform;
		Vector3 position = new Vector3();
		position.z = (randomObjNumber + 1) * 5 + 5;
		position.x = 0;
		waypoint.transform.position = position;
		for (int i = 0; i < randomObjNumber; i++) {
			GameObject toSpawn = randomGameObjects[Random.Range(0, randomGameObjects.Length - 1)];
			GameObject spawned = Instantiate(toSpawn);
			Vector3 pos;
			pos.z = i * 5 + 5;
			if (Random.Range(-2, 1) < 0)
				pos.x = Random.Range(-6f, -1f);
			else pos.x = Random.Range(1f, 6);
			pos.y = spawned.transform.localScale.y / 2 - 0.7f;
			spawned.transform.position = pos;
		}
		
	}
}
class CarPreferences {
	private StreamReader read;
	public Dictionary<string, float> pref = new Dictionary<string, float>();
	public CarPreferences() {
		read = File.OpenText(Application.dataPath + "/baseSettings.ini");
		string allLines = read.ReadToEnd();
		read.Close();
		string[] currLine = allLines.Trim().Split('\n');
		for (int i = 0; i < currLine.Length; i++)
			if (currLine[i] != "<end>")
				pref.Add(currLine[i].Substring(0, currLine[i].IndexOf("=")), float.Parse(currLine[i].Substring(currLine[i].IndexOf("=") + 1)));
	}
}
class BasicSensor {
	public CarPreferences Car = new CarPreferences();
	public string tierOutput = Application.dataPath + "/pointInformation.txt";
	public List<GameObject> currentPoints = new List<GameObject>();
	[SerializeField]
	public List<GameObject> floorPoints = new List<GameObject>();
	[SerializeField]
	public List<GameObject> obstPoints = new List<GameObject>();
	public GameObject spawned, point, hitted;
	public BasicSensor(GameObject t, GameObject p) {
		spawned = t;
		point = p;
	}
	public void singleCast() {
		//if (Cat() != new Vector3(0, 0, 0))
		//{
		//Load.Draw(point, Cat());
		//LocationInfo loc = new LocationInfo();
		//File.WriteAllText(tierOutput, loc.timestamp.ToString() + " " + spawned.transform.rotation + "\n");
		//}
	}
	public Vector3 Cat(CarPreferences Car, RaycastHit hit) {
		if (Physics.Raycast(spawned.transform.position, spawned.GetComponent<Rigidbody>().rotation * Vector3.forward, out hit, Car.pref["rayDist"]) && hit.transform != null) {
			Debug.DrawRay(spawned.transform.position, spawned.GetComponent<Rigidbody>().rotation * Vector3.forward * hit.distance, Color.yellow);
			hitted = hit.collider.gameObject;
			return hit.point;
		} else return Vector3.zero;
	}
}
class Tier1LIDAR : BasicSensor {
	public Tier1LIDAR(GameObject t, GameObject p) : base(t, p) {}
	public void vertSpin() {
		int counter = 0;
		RaycastHit hit = new RaycastHit();
		CarPreferences Car = new CarPreferences();
		string f = "";
		for (float i = 0; i <= Car.pref["degreeY"]; i += Car.pref["stepY"]) {
			for (float j = 0; j <= Car.pref["degreeX"]; j += Car.pref["stepX"]) {
				spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(i - Car.pref["degreeY"] / 2, j - Car.pref["degreeX"] / 2, 0);
				GameObject temp = null;
				if (Load.pointsToDelete[counter] == null) {
					temp = Load.Instantiate(point);
					Load.pointsToDelete[counter] = temp;
				}
				Vector3 cat = Cat(Car, hit);
				temp = Load.pointsToDelete[counter];
				if (cat != Vector3.zero) {
					temp.transform.position = cat;
					f += j + "	" + hit.distance + "\n";
					if (hitted.gameObject.layer == 9) {
						floorPoints.Add(temp);
					} else {
						obstPoints.Add(temp);
					}
				} else temp.transform.position = new Vector3(0, -1, 0);
				counter++;
			}
		}
		Camera.main.GetComponent<Load>().t = f;
		f = "";
		counter = 0;
		spawned.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
	}
}