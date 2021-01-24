using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point {
	public bool obstacle = false;
	private int posX;
	private int posY;
	public GameObject physical;
	
	public Point(int X, int Y, GameObject physical) {
		posX = X;
		posY = Y;
		this.physical = physical;
	}
}