using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
		Time.fixedDeltaTime = CarPreferences.map["updateDelay"];
		//System.Array.Resize(ref pointsToDelete, (int)(CarPreferences.map["degreeX"] * CarPreferences.map["degreeY"]));
		temp = Instantiate(carBody);
		carBodyInst = temp;
		if (CarPreferences.map["Gyro"] == 1) {
			GameObject gyro;
			gyro = Instantiate(gyroPrefab, temp.transform);
			existingGyro = gyro.GetComponent<Gyroscope>();
		}
		IRprefabs[0] = Instantiate(IRprefabs[2], temp.transform);
		if (CarPreferences.map["Tier"] == 1) {
			IRprefabs[0].AddComponent(typeof(ViewSensor));
			sensor = IRprefabs[0].GetComponent<ViewSensor>();
		} else if (CarPreferences.map["Tier"] == 2) {
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

	}
}
