using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool {
	public Dictionary<string, Point> map = new Dictionary<string, Point>();

	public Dictionary<string, Point> Map => map;
	

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
	}

	public static Pool GetPoolInstance() {
		return Load.instance.carBodyInst.GetComponent<Sensor>().pool;
	}

	public List<ParticleSystem.Particle> CalculateObstacles() {
		List<ParticleSystem.Particle> obstacleList = new List<ParticleSystem.Particle>();
		foreach (var point in map) {
			if (point.Value.isPresent) {
				string upper = point.Value.gridPosX.ToString() + "-" + (point.Value.gridPosY + 1).ToString();
				string lower = point.Value.gridPosX.ToString() + "-" + (point.Value.gridPosY - 1).ToString();
				if (point.Value.gridPosY + 1 <= sizeY && point.Value.gridPosY - 1 >= 0) {
					if (map[upper].isPresent && map[lower].isPresent) {
						if (map[lower].obstacle) {
							point.Value.obstacle = true;
							ParticleSystem.Particle p = new ParticleSystem.Particle();
							p.position = point.Value.position;
							p.startSize = 0.05f;
							obstacleList.Add(p);
						}
						else {
							float angle = Vector3.Angle(map[lower].position - point.Value.position,
								map[upper].position - point.Value.position);
							//Debug.Log(angle);
							if (angle < CarPreferences.current.maxAngle) {
								ParticleSystem.Particle p = new ParticleSystem.Particle();
								p.position = point.Value.position;
								p.startSize = 0.05f;
								point.Value.obstacle = true;
								obstacleList.Add(p);
							}
						}
					}
				}
			}
		}
		return obstacleList;
	}
}