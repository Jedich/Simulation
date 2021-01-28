﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Sensor : MonoBehaviour {
	internal GameObject physical;
	internal Rigidbody physicalRigidbody;
	public GameObject pointPrefab;
	public GameObject physicalPrefab;
	public Transform pointCloudHandler;
	internal RaycastHit hit = new RaycastHit();
	public Pool pool;
	public Vector3 relativePosition;
	public abstract void Behaviour();
	public abstract void Snapshot();
	public abstract void Init();
	
	public RaycastHit Ray(Transform spawnedSample, Rigidbody spawnedSampleRigidbody, RaycastHit hit) {
		if (Physics.Raycast(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward, out hit, CarPreferences.current.rayDist) && hit.transform != null) {
			//Debug.DrawRay(spawnedSample.position, spawnedSampleRigidbody.rotation * Vector3.forward * hit.distance, Color.yellow);
			return hit;
		}
		throw new EntryPointNotFoundException("hit not registered");
	}

	public Pool GeneratePool(int sizeX, int sizeY) {
		Pool thisPool = new Pool(new Dictionary<string, Point>(), sizeX, sizeY);
		StartCoroutine(RunGeneration(thisPool));
		return thisPool;
	}

	private IEnumerator RunGeneration(Pool pool) {
		int counter = 0;
		int iter = 0, jter = 0;
		for (float i = 0; i <= pool.SizeY; i += CarPreferences.current.stepY) {
			jter = 0;
			for (float j = 0; j <= pool.SizeX; j += CarPreferences.current.stepX) {
				int index = iter * pool.SizeY + jter;
				pool.Map[iter.ToString() + "-" + jter.ToString()] = new Point(iter, jter);
				counter++;
				jter++;
			}
			Debug.Log("pooling...");
			yield return new WaitForEndOfFrame();
			iter++;
		}
		Debug.Log("pooling finished.");
		pool.isCompleted = true;
	}

	private void Start() {
		physical = Instantiate(Load.instance.physicalPrefab, gameObject.transform);
		physical.transform.position = new Vector3(0, 1, 0);
		pointCloudHandler = Instantiate(new GameObject("pointCloud").transform);
		physicalRigidbody = physical.GetComponent<Rigidbody>();
		Init();
	}

	private void FixedUpdate() {
		if (pool.IsCompleted) {
			Behaviour();
		}
	}
	
	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) Snapshot();
	}
	
	/*public Color Coloring(Vector3 originalPos = new Vector3()) {
		switch (CarPreferences.map["coloring"]) {
			case 0:
				return Color.red;
			case 1:
				//Debug.Log(Color.HSVToRGB(Vector3.Distance(originalPos, Load.instance.carBodyInst.transform.position) / 8f, 1, 1));
				return Color.HSVToRGB(Vector3.Distance(originalPos, Load.instance.carBodyInst.transform.position) / 8f, 1, 1);
			default:
				return Color.black;
		}
	}*/
}