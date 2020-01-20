using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VehicleInfo;

public class Load : MonoBehaviour {
	public string t = "";
	public Gyroscope existingGyro;
	public GameObject carBody, defaultWheel;
	public Transform pointCloud, objCloud;
	public GameObject[] randomGameObjects;
	public int randomObjNumber;
	public GameObject distantPoint;
	public float distance = 100000;
	private CarPreferences Car;
	public float steerDirection;
	public bool back = false;
	public int state;
	public Text pos;
	GameObject temp;
	Vector3 direction;
	public GameObject[] IRprefabs;
	public GameObject gyroPrefab;
	public static List<Point> currentPoints = new List<Point>();
	public static Dictionary<string, GameObject> pointsToDelete = new Dictionary<string, GameObject>();
	public static GameObject carBodyInst;
	public Sensor sensor;
	IGetBehaviour lidar;
	public void Updater() {
		Car = new CarPreferences();
	}
	// public IEnumerator Clearing() {
	// 	yield return new WaitForSeconds(3);
	// 	for (int i = 0; i < pointsToDelete.Count; i++)
	// 		if (pointsToDelete.ElementAt(i).Value != null && pointsToDelete.ElementAt(i).Value.transform.position == new Vector3(0, -1, 0)) {
	// 			// if(currentPoints.Contains(pointsToDelete[i].GetComponent<Point>()))
	// 			currentPoints.Remove(pointsToDelete.ElementAt(i).Value.GetComponent<Point>());
	// 			Destroy(pointsToDelete.ElementAt(i).Value);
	// 		}
	// }
	private void Update() {
		if (Input.GetKeyDown(KeyCode.S))
			lidar.Snapshot();
	}

	public void FixedUpdate() {
		IRprefabs[0].transform.position = new Vector3(IRprefabs[0].transform.parent.position.x, IRprefabs[0].transform.parent.position.y + 0.6f, IRprefabs[0].transform.parent.position.z);
		lidar.Behaviour();
	}

	public void Start() {
		//StartCoroutine(Clearing());
		Car = new CarPreferences();
		Time.fixedDeltaTime = Car.pref["updateDelay"];
		//System.Array.Resize(ref pointsToDelete, (int)(Car.pref["degreeX"] * Car.pref["degreeY"]));
		temp = Instantiate(carBody);
		carBodyInst = temp;
		if (Car.pref["Gyro"] == 1) {
			GameObject gyro;
			gyro = Instantiate(gyroPrefab, temp.transform);
			existingGyro = gyro.GetComponent<Gyroscope>();
		}
		IRprefabs[0] = Instantiate(IRprefabs[2], temp.transform);
		if (Car.pref["Tier"] == 1) {
			IRprefabs[0].AddComponent(typeof(ViewSensor));
			sensor = IRprefabs[0].GetComponent<ViewSensor>();
		} else if (Car.pref["Tier"] == 2) {
			IRprefabs[0].AddComponent(typeof(Tier1LIDAR));
			sensor = IRprefabs[0].GetComponent<Tier1LIDAR>();
		}
		sensor.point = IRprefabs[1];
		sensor.spawned = IRprefabs[0];
		sensor.pointCloudHandler = pointCloud;
		lidar = sensor;
		IRprefabs[0].transform.position = new Vector3(temp.transform.position.x, 0f, temp.transform.position.z);
		temp.transform.position = new Vector3(0, 1f, 0);
		IRprefabs[0].transform.parent = temp.transform;
		Camera.main.transform.parent = temp.transform;
		CameraMovement.target = temp.transform;
		// Vector3 position = new Vector3();
		//position.z = (randomObjNumber + 1) * 5 + 5;
		// position.x = 0;
		//waypoint.transform.position = position;
		// for (int i = 0; i < randomObjNumber; i++) {
		// 	GameObject toSpawn = randomGameObjects[Random.Range(0, randomGameObjects.Length - 1)];
		// 	GameObject spawned = Instantiate(toSpawn, objCloud);
		// 	Vector3 pos;
		// 	pos.z = i * 5 + 5;
		// 	if (Random.Range(-2, 1) < 0)
		// 		pos.x = Random.Range(-6f, -1f);
		// 	else pos.x = Random.Range(1f, 6);
		// 	pos.y = spawned.transform.localScale.y / 2 - 0.7f;
		// 	spawned.transform.position = pos;
		// }
		
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
interface IGetBehaviour {
	void Behaviour();
	void Snapshot();
}
public abstract class Sensor : MonoBehaviour, IGetBehaviour {
	public GameObject point, spawned;
	public Transform pointCloudHandler;
	public abstract void Behaviour();
	public abstract void Snapshot();
}