using UnityEngine;

/*
 * We treat each logic as a seperate sequence so that we can play any sequence individually.
 * This calss act as the base class for each sequence. So that it initiallizes some of the common properties of each sequence
*/

namespace ProjectSpaceWalk
{
	public abstract class SequenceBase : ISequence
	{
		public Camera _cutSceneCamera;
		public GameObject _cutSceneCameraPlaceHolder;
		public Camera _fpCamera;
		public GameObject _fpCameraPlaceHolder;
		public Camera _planetIntroCamera;
		public Planet _planetToVisit;
		public GameObject _player;
		public bool _isfadeOut;
		public float _fadeValue = 1;

		public ISequenceController Controller { get; private set; }

		public SequenceBase(ISequenceController controller)
		{
			Controller = controller;
			_cutSceneCamera = CaptureSequenceLevelObjects.Instance.cam_cutscene01;
			_cutSceneCameraPlaceHolder = CaptureSequenceLevelObjects.Instance.introScenePlaceHolder;
			_fpCamera = CaptureSequenceLevelObjects.Instance.cam_avatar;
			_fpCameraPlaceHolder = CaptureSequenceLevelObjects.Instance.fpCameraPlaceHolder;
			_planetIntroCamera = CaptureSequenceLevelObjects.Instance.planetIntroCamera;
			_player = CaptureSequenceLevelObjects.Instance.spaceShip;
		}

		public abstract void Destroy ();

		public abstract void Initialize ();

		public abstract void OnGUI ();

		public abstract void Update ();
	}
}