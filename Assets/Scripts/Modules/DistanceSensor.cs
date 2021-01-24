using UnityEngine;

public class DistanceSensor : Sensor {
	private RaycastHit currentRay;
	private Point point;

	public override void Init() {
		point = new Point(0, 0, Instantiate(Load.instance.pointPrefab, pointCloudHandler));
	}
	public override void Behaviour() {
		currentRay = Ray(physical.transform, physicalRigidbody, hit);
		point.physical.transform.position = currentRay.point;
	}

	public override void Snapshot() {
		throw new System.NotImplementedException();
	}
}
