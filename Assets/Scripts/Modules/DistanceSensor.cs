using UnityEngine;

namespace Modules {
	public class DistanceSensor : Sensor {
		private RaycastHit currentRay;
		private Point point;

		protected override void Init() {
		
		}
		public override void Behaviour() {
			currentRay = Ray(physical.transform, physicalRigidbody, hit);
			point = new Point(0, 0);
		}

		public override void Snapshot() {
			throw new System.NotImplementedException();
		}
	}
}
