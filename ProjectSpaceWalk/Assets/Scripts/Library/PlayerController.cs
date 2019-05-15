using UnityEngine;
using CharControl;

/*
 * This class is responsible for the First person camera movement.
 */

namespace ProjectSpaceWalk
{
	public sealed class PlayerController : MonoBehaviour
	{
		[SerializeField] private float _speed;

		private bool _enabled;
		private input_keylistener listener;

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if(_enabled != value)
				{
					_enabled = value;
				}
			}
		}

		private void Start()
		{
			listener = input_keylistener.GetListener ();
		}

		private void Update()
		{
			if (_enabled)
			{
				INPUT_KEYS ();
			}
		}

		// ************************************************************************
		// Control the character's camera
		private void INPUT_KEYS(){
			if (Input.GetKey(KeyCode.Q))
			{
				listener.RecordInputKey(13);
				transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
			}
			// Spin Left
			else if (Input.GetKey(KeyCode.E))
			{
				listener.RecordInputKey(13);
				transform.Rotate(Vector3.back * _speed * Time.deltaTime);
			}
			// Move Forward * Fast
			else if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				listener.RecordInputKey(14);
				transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			}
			// Move Forward
			else if (Input.GetKey(KeyCode.W))
			{
				listener.RecordInputKey(11);
				transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			}
			// Move Backward
			else if (Input.GetKey(KeyCode.S))
			{
				listener.RecordInputKey(11);
				transform.Translate(Vector3.back * _speed * Time.deltaTime);
			}
			// Turn Right
			else if (Input.GetKey(KeyCode.A))
			{
				listener.RecordInputKey(10);
				transform.Rotate(Vector3.left * _speed * Time.deltaTime);
			}
			// Turn Left
			else if (Input.GetKey(KeyCode.D))
			{
				listener.RecordInputKey(10);
				transform.Rotate(Vector3.up * _speed * Time.deltaTime);
			}
			// Tilt Up
			else if (Input.GetKey(KeyCode.R))
			{
				listener.RecordInputKey(12);
				transform.Rotate(Vector3.left * _speed * Time.deltaTime);
			}
			// Tilt Down
			else if (Input.GetKey(KeyCode.F))
			{
				listener.RecordInputKey(12);
				transform.Rotate(Vector3.right * _speed * Time.deltaTime);
			}
		}
	}
}