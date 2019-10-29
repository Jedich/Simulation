using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VehicleInfo;

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
	GameObject temp;
	Vector3 direction;
	public GameObject[] IRprefabs;
	public GameObject gyroPrefab;
	public static List<Point> currentPoints = new List<Point>();
	public static List<Vector3> points = new List<Vector3>();
	public static GameObject[] pointsToDelete;
	public List<Point> observe;
	public void Updater() {
		Car = new CarPreferences();
	}
	public IEnumerator Clearing() {
		yield return new WaitForSeconds(3);
		for (int i = 0; i < pointsToDelete.Length; i++)
			if (pointsToDelete[i] != null && pointsToDelete[i].transform.position == new Vector3(0, -1, 0)){
				// if(currentPoints.Contains(pointsToDelete[i].GetComponent<Point>()))
				currentPoints.Remove(pointsToDelete[i].GetComponent<Point>());
				Destroy(pointsToDelete[i]);
			}
	}
	private void Update() {
		observe = currentPoints;
		// nearest.GetComponent<LineRenderer>().SetPosition(0, IRprefabs[0].transform.position);
		// nearest.GetComponent<LineRenderer>().SetPosition(1, distantPoint.transform.position);
		// pos.text = distantPoint.transform.position.ToString();
		IRprefabs[0].transform.position = new Vector3(IRprefabs[0].transform.parent.position.x, IRprefabs[0].transform.parent.position.y + 0.25f, IRprefabs[0].transform.parent.position.z);
	}

	public void FixedUpdate() {
		distance = 100;
				if (Car.pref["Tier"] == 0) {
			ViewSensor lidar;
			lidar = new ViewSensor(IRprefabs[0], IRprefabs[1]);
			lidar.Behaviour();
		} else if (Car.pref["Tier"] == 2) {
			Tier1LIDAR lidar;
			lidar = new Tier1LIDAR(IRprefabs[0], IRprefabs[1]);
			lidar.Behaviour();
		}
		
	}

	public void Start() {
		StartCoroutine(Clearing());
		Car = new CarPreferences();
		Time.fixedDeltaTime = Car.pref["updateDelay"];
		System.Array.Resize(ref pointsToDelete, (int)(Car.pref["degreeX"] * Car.pref["degreeY"]));
		temp = Instantiate(carBody);
		if(Car.pref["Gyro"] == 1)
			Instantiate(gyroPrefab, temp.transform);
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
public class CarPreferences {
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
