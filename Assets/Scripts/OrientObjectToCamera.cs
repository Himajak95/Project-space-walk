using UnityEngine;

public sealed class OrientObjectToCamera : MonoBehaviour
{
	private void OnRenderObject()
	{
		transform.LookAt (-Camera.current.transform.position);
	}
}