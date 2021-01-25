using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point {
	public bool obstacle = false;
	public int gridPosX, gridPosY;
	public bool isPresent = true;
	public Vector3 position;
	public Point(int gridPosX, int gridPosY) {
		this.gridPosX = gridPosX;
		this.gridPosY = gridPosY;
	}
}