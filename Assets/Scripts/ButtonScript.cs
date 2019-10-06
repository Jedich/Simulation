﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour {
	public GameObject pos;
	private void Start() {
		Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("points"));
	}
	private void Update() {
		if(Input.GetKeyDown(KeyCode.R))
			Restart();
		if(Input.GetKeyDown(KeyCode.Space))
			Pause();
	}
	public void CheckInput() {
		Camera.main.cullingMask = 1 << 10;
		Camera.main.backgroundColor = Color.black;
		pos.SetActive(true);
	}
	public void OffOutput() {
		Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("points"));
		Camera.main.backgroundColor = Color.white;
		pos.SetActive(false);
	}
	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void Pause() {
		if(Time.timeScale == 0)
			Time.timeScale = 1;
		else Time.timeScale = 0;
	}
}
