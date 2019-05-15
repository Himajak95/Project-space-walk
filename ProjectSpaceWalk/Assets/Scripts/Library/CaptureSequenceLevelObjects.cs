using UnityEngine;

/*
 * This class holds most of the objects required to excecute the sequence.
 * So that we can use this class as a module.
 */

namespace ProjectSpaceWalk
{
	public class CaptureSequenceLevelObjects : MonoBehaviour
	{
		public static CaptureSequenceLevelObjects Instance{ get; private set; }

		public PlayerController _playerController;
		public ImageCaptureManager _imageCaptureManager;

		public GameObject doorPlaceHolder,
		fpCameraPlaceHolder,
		introScenePlaceHolder,
		spaceShip,
		object_door = null,
		object_hatch = null,
		object_here01 = null,
		object_here02 = null,
		object_close_door = null,
		object_camera = null;
		
		public Camera cam_cutscene01 = null,
		planetIntroCamera,
		cam_avatar = null;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			object_hatch.SetActive(false);
			object_hatch.renderer.enabled = false;
			object_door.animation.Stop();
			
			object_here01.SetActive(false);
			object_here01.renderer.enabled = false;
			
			object_here02.SetActive(false);
			object_here02.renderer.enabled = false;
			
			object_close_door.SetActive(false);
			object_close_door.renderer.enabled = false;
			
			object_camera.SetActive(false);
			object_camera.renderer.enabled = false;
		}

		private void OnDestroy()
		{
			if (Instance != null && Instance == this)
			{
				Instance = null;
			}
		}
	}
}