using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour {
	public Gyroscope existingGyro;
	Vector3 direction;
	public GameObject gyroPrefab;
	public GameObject carBodyInst;
	public Sensor sensor;
	public GameObject pointPrefab;
	public GameObject physicalPrefab;
	public ParticleSystem pointCloud;
	public ParticleSystem obstaclePointCloud;
	public static Load instance = null;

	public void Start() {
		if (instance == null) {
			instance = this;
		}
		else if (instance == this) {
			Destroy(gameObject);
		}
		Time.fixedDeltaTime = CarPreferences.map["updateDelay"];

		if (CarPreferences.map["Gyro"] == 1) {
			GameObject gyro;
			gyro = Instantiate(gyroPrefab, carBodyInst.transform);
			existingGyro = gyro.GetComponent<Gyroscope>();
		}

		if (CarPreferences.map["Tier"] == 0) {
			carBodyInst.AddComponent(typeof(DistanceSensor));
			sensor = carBodyInst.GetComponent<DistanceSensor>();
		}
		else if (CarPreferences.map["Tier"] == 1) {
			carBodyInst.AddComponent(typeof(Lidar));
			sensor = carBodyInst.GetComponent<Lidar>();
		}
		if (CarPreferences.map["Tier"] != 2) {
			sensor.physicalPrefab = physicalPrefab;
			sensor.pointPrefab = pointPrefab;
		}
		Camera.main.transform.parent = carBodyInst.transform;
		CameraMovement.target = carBodyInst.transform;
	}
}
