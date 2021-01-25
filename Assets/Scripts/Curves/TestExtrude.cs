using UnityEngine;
 
public class TestExtrude : MonoBehaviour
{
	[Header("Extrusion")]
	public Vector3 pos = Vector3.forward;
	public Quaternion q = Quaternion.identity;
	public Vector3 s = Vector3.one;
 
	public bool invertFaces = false;
	private Mesh mesh;
	private MeshFilter meshFilter;
	private MeshExtrusion.Edge[] edges;
	private Matrix4x4[] extrusion;
 
	private void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		mesh = meshFilter.mesh;
		edges = MeshExtrusion.BuildManifoldEdges(mesh);
 
		Matrix4x4 worldToLocal = transform.worldToLocalMatrix;
		extrusion = new Matrix4x4[] { worldToLocal * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one), worldToLocal * Matrix4x4.TRS(pos, Quaternion.identity, s) };
	}
 
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			MeshExtrusion.ExtrudeMesh(mesh, meshFilter.mesh, extrusion, edges, invertFaces);
	}
}