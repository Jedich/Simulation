using System.Collections.Generic;
using UnityEngine;

namespace Modules {
	[System.Serializable]
	public class Pool {
		public Dictionary<string, Point> map { get; private set; }
		public int sizeX { get; private set; }
		public int sizeY { get; private set; }

		public bool isCompleted;

		public Pool(Dictionary<string, Point> pool, int sizeX, int sizeY) {
			map = pool;
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			isCompleted = false;
		}


		public void CalculateObstacles() {
			foreach (var point in map) {
				if (point.Value.isPresent) {
					string upper = point.Value.gridPosX.ToString() + "-" + (point.Value.gridPosY + 1).ToString();
					string lower = point.Value.gridPosX.ToString() + "-" + (point.Value.gridPosY - 1).ToString();
					if (point.Value.gridPosY + 1 <= sizeY && point.Value.gridPosY - 1 >= 0) {
						if (map[upper].isPresent && map[lower].isPresent) {
							if (map[lower].obstacle) {
								point.Value.obstacle = true;
							}
							else {
								float angle = Vector3.Angle(map[lower].position - point.Value.position,
									map[upper].position - point.Value.position);
								if (angle < CarPreferences.current.maxAngle) {
									point.Value.obstacle = true;
								}
							}
						}
					}
				}
			}
		}
	}
}