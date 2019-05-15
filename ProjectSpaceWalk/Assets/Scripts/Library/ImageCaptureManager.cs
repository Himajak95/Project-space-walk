using UnityEngine;
using Design;
using System;

/*
 * This class is responsible for capturing images.
 */

namespace ProjectSpaceWalk
{
	public sealed class ImageCaptureManager : MonoBehaviour
	{
		private bool _enabled;
		private Texture2D camera_ui;
		private int snapshot = 0; // counts the camera snapshot
		private AudioClip sound_camera;
		private float cameraFlashOpacity = 0.0f;

		public event Action<int> OnCaptured;

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
					snapshot = 0;
					_enabled = value;
				}
			}
		}

		private void OnGUI()
		{
			if (_enabled)
			{
				design.DrawRectangle (new Rect (0,0,Screen.width, Screen.height), new Color(1f,1f,1f,1f), cameraFlashOpacity);
				GUI.DrawTexture(new Rect(0, 0, camera_ui.width, camera_ui.height), camera_ui);
			}
		}

		private void Start()
		{
			// Camera snapshot sound effects
			sound_camera = (AudioClip)Resources.Load("sounds/camera-snapshot");
			camera_ui = (Texture2D)Resources.Load("images/ui-camera");
		}

		private void Update()
		{
			if (!_enabled)
			{
				return;
			}

			if (Input.GetMouseButtonDown(0) && cameraFlashOpacity == 0){
				cameraFlashOpacity = 1;
				audio.PlayOneShot(sound_camera);
				audio.Play();

			}

			if (cameraFlashOpacity > 0)
			{
				cameraFlashOpacity -= 2 * Time.deltaTime;
				cameraFlashOpacity = Mathf.Clamp(cameraFlashOpacity, 0f, 1f);
				if(cameraFlashOpacity == 0)
				{
					snapshot += 1;
					if(OnCaptured != null)
					{
						OnCaptured(snapshot);
					}
				}
			}
		}
	}
}