using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour {
	public GameObject pos, hideable;
	public Slider rWheel, lWheel, bothWheels;
	public Text rotationInfo;
	Gyroscope gyroscope;
	private void Start() {
		if (CarPreferences.map["GUI"] == 0)
			hideable.SetActive(false);
		else hideable.SetActive(true);
		Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("points"));
		if (gameObject.GetComponent<Load>().existingGyro != null)
			gyroscope = gameObject.GetComponent<Load>().existingGyro;
	}
	private void Update() {
		if (Input.GetKeyDown(KeyCode.R))
			Restart();
		if (Input.GetKeyDown(KeyCode.Space))
			Pause();
		if (Input.GetKeyDown(KeyCode.Minus))
			Time.timeScale -= 0.1f;
		if (Input.GetKeyDown(KeyCode.Plus))
			Time.timeScale += 0.1f;
		WheelDrive.motorMultiplier = lWheel.value / 10f;
		WheelDrive.angleMultiplier = rWheel.value / 10f;
		//rotationInfo.text = gyroscope.angularVel + "\n" + gyroscope.rotation + "\n" + gyroscope.acceleration;
	}
	public void CheckInput() {
		Camera.main.cullingMask = 1 << 10;
		Camera.main.backgroundColor = Color.black;
		//pos.SetActive(true);
	}
	public void OffOutput() {
		Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("points"));
		Camera.main.backgroundColor = Color.white;
		//pos.SetActive(false);
	}
	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void Pause() {
		if (Time.timeScale == 0)
			Time.timeScale = 1;
		else Time.timeScale = 0;
	}
	public void MainSlider() {
		rWheel.value = bothWheels.value;
		lWheel.value = bothWheels.value;
	}
}