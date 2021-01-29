using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Modules;
using UnityEngine;
using UnityEngine.UI;
using Gyroscope = Modules.Gyroscope;

public class Load : MonoBehaviour {
	public Gyroscope existingGyro;
	Vector3 direction;
	public GameObject gyroPrefab, cameraPrefab;
	public GameObject carBodyInst;
	public Sensor sensor = null;
	public GameObject pointPrefab;
	public GameObject physicalPrefab;
	public ParticleSystem pointCloud;
	public ParticleSystem obstaclePointCloud;
	public bool isObserving = false;
	public Image vision;
	public static Load instance = null;
	public RenderTexture camTexture;
	internal CameraSensor cameraSensor;
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
					GameObject camera;
					camera = Instantiate(cameraPrefab, carBodyInst.transform);
					carBodyInst.AddComponent(typeof(CameraSensor));
					cameraSensor = carBodyInst.GetComponent<CameraSensor>();
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

