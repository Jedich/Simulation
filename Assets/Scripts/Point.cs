using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	CarPreferences Car;
	public bool obstacle = false;
	public float[] coords = new float[2];
	public Point nearest1, nearest2;
	public float angle;
	public void PointC(float X, float Y) {
		Car = new CarPreferences();
		coords[0] = X;
		coords[1] = Y;
		if (!Load.currentPoints.Contains(this))
			Load.currentPoints.Add(this);
	}
	public void Search() {
		if (nearest1 == null || nearest2 == null)
			for (int i = 0; i < Load.currentPoints.Count; i++) {
				if (Load.currentPoints[i].coords[0] == coords[0]) {
					if (Load.currentPoints[i].coords[1] == coords[1] - Car.pref["stepY"])
						nearest1 = Load.currentPoints[i];
					else if (Load.currentPoints[i].coords[1] == coords[1] + Car.pref["stepY"])
						nearest2 = Load.currentPoints[i];
				}
			}
	}
	public void PointC() {
		nearest1 = null;
		nearest2 = null;
		angle = 0;
		obstacle = false;
		if (!Load.currentPoints.Contains(this))
			Load.currentPoints.Add(this);
	}
	void Start() {
		Car = new CarPreferences();

	}
	void Update() {

		if (nearest1 != null && nearest2 != null) {
			angle = Vector3.Angle(nearest1.gameObject.transform.position - gameObject.transform.position, nearest2.gameObject.transform.position - gameObject.transform.position);
			if (angle > Car.pref["maxAngle"] && nearest2.obstacle) {
				obstacle = true;
			} else {
				if (angle >= Car.pref["maxAngle"]) {
					obstacle = false;
				} else {
					obstacle = true;
				}
			}
			if (obstacle)
				gameObject.GetComponent<Renderer>().material.color = Color.red;
			else
				gameObject.GetComponent<Renderer>().material.color = Color.green;
		}
	}
}