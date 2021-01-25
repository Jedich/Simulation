using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool {
	public Dictionary<string, Point> map = new Dictionary<string, Point>();

	public Dictionary<string, Point> Map => map;

	public ParticleSystem.Particle[] testPool;

	public int SizeX => sizeX;

	public int SizeY => sizeY;

	public int sizeX, sizeY;

	public bool isCompleted;

	public bool IsCompleted {
		get => isCompleted;
		set => isCompleted = value;
	}

	public Pool(Dictionary<string, Point> pool, int sizeX, int sizeY) {
		map = pool;
		this.sizeX = sizeX;
		this.sizeY = sizeY;
		isCompleted = false;
		testPool = new ParticleSystem.Particle[sizeX/(int)CarPreferences.map["stepX"]*sizeY/(int)CarPreferences.map["stepY"]+1];
	}
}