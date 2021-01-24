using System;
using UnityEngine;

public class PointGraphics : MonoBehaviour {
	public Material pointMat;

	private void Start() {
		pointMat = GetComponent<Renderer>().material;
	}
	void FixedUpdate() {
		transform.LookAt(transform.position - Camera.main.transform.position);
		Coloring((int) CarPreferences.map["coloring"]);
	}
	
	public void Coloring(int type) {
		switch (type) {
			case 0:
				pointMat.color = Color.red;
				break;
			case 1:
				pointMat.color = Color.HSVToRGB(Vector3.Distance(transform.position, Load.instance.carBodyInst.transform.position) / 8f, 1, 1);
				break;
		}
	}
}
