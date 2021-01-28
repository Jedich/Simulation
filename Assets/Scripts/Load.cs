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
	public Sensor sensor = null;
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
		Time.fixedDeltaTime = CarPreferences.current.updateDelay;
		foreach (var sensorStr in CarPreferences.current.sensors) {
			switch (sensorStr) {
				case "SENSOR_PROXIM":
					carBodyInst.AddComponent(typeof(DistanceSensor));
					sensor = carBodyInst.GetComponent<DistanceSensor>();
					break;
				case "SENSOR_LIDAR":
					carBodyInst.AddComponent(typeof(Lidar));
					sensor = carBodyInst.GetComponent<Lidar>();
					break;
				case "SENSOR_CAMERA":
					carBodyInst.AddComponent(typeof(CameraSensor));
					break;
				case "SENSOR_GYRO":
					GameObject gyro;
					gyro = Instantiate(gyroPrefab, carBodyInst.transform);
					existingGyro = gyro.GetComponent<Gyroscope>();
					break;
			}
		}
		if (sensor != null) {
			sensor.physicalPrefab = physicalPrefab;
			sensor.pointPrefab = pointPrefab;
		}
		Camera.main.transform.parent = carBodyInst.transform;
		CameraMovement.target = carBodyInst.transform;
	}
}
