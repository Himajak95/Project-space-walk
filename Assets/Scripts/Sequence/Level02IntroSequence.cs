using UnityEngine;
using Design;

/*
 * This is the introduction sequence while entering a planet
*/

namespace ProjectSpaceWalk
{
	public class Level02IntroSequence : SequenceBase
	{
		private float _time = 0;
		
		private float oShuttleName = -1.0f,
		oCrewNames = -3.0f;

		public Level02IntroSequence(ISequenceController controller, Planet planetToVisit)
			: base(controller)
		{
			_planetToVisit = planetToVisit;
		}

		public override void Destroy ()
		{

		}

		private void FadeControl()
		{
			if (_isfadeOut)
			{
				if(_fadeValue < 1)
				{
					_fadeValue += Time.deltaTime;
					_fadeValue = Mathf.Clamp(_fadeValue, 0, 1);
				}
			}
			else if(_fadeValue > 0)
			{
				_fadeValue -= Time.deltaTime;
				_fadeValue = Mathf.Clamp(_fadeValue, 0, 1);
			}
		}

		public override void Initialize ()
		{

			CaptureSequenceLevelObjects.Instance.object_hatch.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_hatch.renderer.enabled = false;
			CaptureSequenceLevelObjects.Instance.object_door.animation.Stop();
			CaptureSequenceLevelObjects.Instance.object_door.transform.rotation = CaptureSequenceLevelObjects.Instance.doorPlaceHolder.transform.rotation;
			
			CaptureSequenceLevelObjects.Instance.object_here01.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_here01.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_here02.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_here02.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_close_door.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_close_door.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_camera.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_camera.renderer.enabled = false;

			CaptureSequenceLevelObjects.Instance._playerController.Enabled = false;
			CaptureSequenceLevelObjects.Instance._imageCaptureManager.Enabled = false;
			_planetToVisit._enableOrbit = false;
			_planetIntroCamera.gameObject.SetActive (false);
			_fpCamera.transform.position = _fpCameraPlaceHolder.transform.position;
			_fpCamera.transform.rotation = _fpCameraPlaceHolder.transform.rotation;
			_fpCamera.gameObject.SetActive (false);
			_cutSceneCamera.gameObject.SetActive (true);
			_player.transform.position = _planetToVisit.transform.position - _planetToVisit.transform.forward * 45;
			_player.transform.LookAt (_planetToVisit.transform);
			_cutSceneCamera.transform.position = _cutSceneCameraPlaceHolder.transform.position;
			_isfadeOut = false;
		}

		public override void OnGUI ()
		{
			LEVEL_INTRO_GUI ();
		}

		// ************************************************************************
		// Introduction of the level 
		private void LEVEL_INTRO_GUI(){
			GUI.Label (new Rect ((Screen.width / 2) - 20, 10, Screen.width / 2, 55), "GEMINI 4", design.StyleText (design.Font_Futura, 35, TextAnchor.MiddleRight, new Color (1, 1, 1, Mathf.Clamp (oShuttleName, 0, 1))));
			GUI.Label (new Rect ((Screen.width/2) - 20, 45, Screen.width/2, 55), "James A. McDivitt, Command Pilot", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleRight, new Color(1,1,1,Mathf.Clamp (oCrewNames, 0, 1))));
			GUI.Label (new Rect ((Screen.width/2) - 20, 75, Screen.width/2, 55), "Edward H. White II, Pilot", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleRight, new Color(1,1,1,Mathf.Clamp (oCrewNames, 0, 1))));
			design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), Color.black, _fadeValue);
		}

		public override void Update ()
		{
			FadeControl ();
			LEVEL_INTRO_UPDATE ();
			_time += Time.deltaTime;
			_player.transform.position += _player.transform.forward * 2 * Time.deltaTime;
			_cutSceneCamera.transform.LookAt (_player.transform.position);
		}

		// ************************************************************************
		private void LEVEL_INTRO_UPDATE(){
			if (oShuttleName < 1.0f)
			{
				oShuttleName += Time.deltaTime;
			}

			if(oCrewNames < 1.0f)
			{
				oCrewNames += Time.deltaTime;
			}
			
			if(_time > 6 && !_isfadeOut)
			{
				//_isfadeOut = true;
			}
			
			if(_time > 8) {
				AudioManager.Instance.sound_engine.Stop();
				AudioManager.Instance.sound_engine_inside.volume = 1.0f;
				AudioManager.Instance.astronaut_breathing.volume = 0.5f;
				Controller.AddSequence(new Level02ImageCapturingSequence(Controller, _planetToVisit));
			}
		}
	}
}