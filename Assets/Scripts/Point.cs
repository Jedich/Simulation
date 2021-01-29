using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : IComparer<Point> {
	public bool obstacle = false;
	public int gridPosX, gridPosY;
	public bool isPresent = true;
	public Vector3 position;
	public float distance;
	public Point(int gridPosX, int gridPosY) {
		this.gridPosX = gridPosX;
		this.gridPosY = gridPosY;
	}

	public int Compare(Point x, Point y) {
		if (x.distance > y.distance) {
			return 1;
		}
		if(x.distance < y.distance) {
			return -1;
		}
		return 0;
	}
}