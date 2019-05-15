using UnityEngine;

namespace ProjectSpaceWalk
{
	public class Planet : MonoBehaviour
	{
		[SerializeField] private TextMesh _label;
		[SerializeField] private Renderer _meshRenderer;
		[SerializeField] private Material _orbitPathMaterial;
		[SerializeField] private Transform _origin;
		[SerializeField] private float _speed;

		[SerializeField] private Color _color;

		[SerializeField] private bool _createOrbitPath;

		public bool _enableOrbit;

		private float _radius;

		private void Start()
		{
			_meshRenderer.material.color = _color;
			_label.text = name;
			_label.color = _color;
			if (_enableOrbit)
			{
				_radius = Vector3.Distance (transform.position, _origin.position);
				Vector2 randomRadius = Random.insideUnitCircle * _radius;
				Vector3 initialPosition = new Vector3(randomRadius.x, transform.position.y, randomRadius.y) + _origin.position;
				transform.position = initialPosition;
				transform.LookAt (_origin);
			}

			if (_createOrbitPath)
			{
				CreateOrbitPath (_radius);
			}
		}

		private void Update()
		{
			if (_enableOrbit)
			{
				Orbit ();
			}
		}

		private void LateUpdate()
		{
			transform.LookAt (_origin);
		}
		
		private void CreateOrbitPath(float radius)
		{
			int vertexCount = 100;
			
			GameObject item = new GameObject ("OrbitPath_" + name);
			LineRenderer lineRenderer = item.AddComponent<LineRenderer> ();
			lineRenderer.material = _orbitPathMaterial;
			lineRenderer.SetWidth (4, 4);
			lineRenderer.SetVertexCount (vertexCount);
			
			for (int i = 0; i < vertexCount; i++)
			{
				float angle = 360.0f * ((float)i / (vertexCount - 1));
				float x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
				float z = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
				lineRenderer.SetPosition(i, new Vector3(x, transform.position.y, z));
			}
			item.transform.parent = _origin;
		}

		private void Orbit()
		{
			//transform.LookAt (_origin);
			transform.position += transform.right * _speed * Time.deltaTime;
			Vector3 direction = (transform.position - _origin.position).normalized;
			transform.position = direction * _radius;
		}
	}
}