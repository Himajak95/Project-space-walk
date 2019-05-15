using UnityEngine;
using Mission;
using System;
using Design;
using Character;

/*
 * This is the sequence used to capture images of each planet. 
*/

namespace ProjectSpaceWalk
{
	public class Level02ImageCapturingSequence : SequenceBase
	{
		private Action _sequence;

		private bool isVisor = false;

		private float timeLeft = 8.0f;
		private bool isTimeLeft = false;

		private float opactiy_vision = 0.0f;

		public Level02ImageCapturingSequence(ISequenceController controller, Planet planetToVisit)
			: base(controller)
		{
			_planetToVisit = planetToVisit;
			mission.ISFAIL = false;
		}
		
		public override void Destroy ()
		{
			mission.ReduceHealth = false;
			mission.UpdateTimer = false;
			_planetToVisit._enableOrbit = true;
		}

		public override void Initialize ()
		{
			mission.UpdateTimer = true;
			mission.ISFAIL = false;
			InitVariables ();
			_planetToVisit._enableOrbit = false;
			_planetIntroCamera.gameObject.SetActive (false);
			_cutSceneCamera.gameObject.SetActive (false);
			_fpCamera.gameObject.SetActive (true);
			_player.transform.rotation = Quaternion.identity;
			_player.transform.position = _planetToVisit.transform.position + _planetToVisit.transform.up * 35;
		}

		public override void OnGUI ()
		{	
			 // Display MISSION FAIL screen
			//design.MissionAbort(design.Font_Futura, button_blank, button_blue, button_blue_hover);

			// Objectives 2 or greater must display a warning message if the user turns off the visor
			if((mission.LEVELPIVOT >= 2)){
				VISOR_WARNING();
			}
			
			CAM_BACKGROUND_GUI();
			LEVEL_PLAY_GUI (); // Display the health meter, objectives, and other UI elements
		}

		public override void Update ()
		{
			VISOR_CONTROL(); // Controls the visor
				
			if(_sequence != null)
			{
				_sequence();
			}

				if(mission.LEVELPIVOT >= 2){
					switch(isVisor){
					case false:
						AudioManager.Instance.heart_beat.mute = false;
						AudioManager.Instance.heart_beat.volume = 1.0f;
						AudioManager.Instance.astronaut_breathing.volume = 0.0f;
						AudioManager.Instance.music_background.volume = 0.0f;
						AudioManager.Instance.sound_engine_inside.volume = 0.0f;
						
						if(opactiy_vision < 0.8f)
							opactiy_vision += (Time.deltaTime);
						mission.ReduceHealth = true;
						break;
						
					case true:
						mission.ReduceHealth = false;
						if(opactiy_vision > 0f)
							opactiy_vision -= (Time.deltaTime);
						if(opactiy_vision == 0f)
							opactiy_vision = 0f;
						
						AudioManager.Instance.heart_beat.mute = true;
						AudioManager.Instance.astronaut_breathing.volume = 1.0f;
						AudioManager.Instance.music_background.volume = 1.0f;
						AudioManager.Instance.sound_engine_inside.volume = 1.0f;
						
						break;
					}
					
					// set on camera ui
					if (Input.GetKeyDown(KeyCode.C)){
						CaptureSequenceLevelObjects.Instance._imageCaptureManager.Enabled = !CaptureSequenceLevelObjects.Instance._imageCaptureManager.Enabled;
						CaptureSequenceLevelObjects.Instance._imageCaptureManager.OnCaptured += (int e) =>
						{
							if(e == 1){
								mission.IS_OBJ_DONE = true;
								mission.LEVELPIVOT = 3; // Next objective (3)
								_sequence = SECTION03;
								CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_beep_high);
								CaptureSequenceLevelObjects.Instance.audio.Play();
							}
						};
					}
			}
		}

		// ************************************************************************
		// Camera background color is black
		private void CAM_BACKGROUND_GUI(){
			CaptureSequenceLevelObjects.Instance.cam_cutscene01.backgroundColor = Color.black;
			CaptureSequenceLevelObjects.Instance.cam_avatar.backgroundColor = Color.black;
		}
		
		// ************************************************************************
		private void LEVEL_PLAY_GUI(){
			//collide_hatch.renderer.enabled = true;
			design.GamePlayUI();

			design.LevelStatusUI();
			
			// Turn on the visor
			if(isVisor == true)
				design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), new Color(155/255f,144/255f,101/255f), 0.2f);
		}
		
		// ************************************************************************
		// Displays the warning messaage if the user turns off the visor while playing the mission.
		private void VISOR_WARNING(){
			if (mission.ISFAIL)
			{
				return;
			}
			
			design.DrawRectangle (new Rect (0,0,Screen.width, Screen.height), new Color(181/255f,0f,0f,1f), opactiy_vision);
			design.DrawRectangle (new Rect (0,0,Screen.width, Screen.height), new Color(1f,1f,1f,1f), opactiy_vision);
			
			if(isVisor == false)
				GUI.Label (new Rect (0, Screen.height/2, Screen.width, 85), "WARNING!\nKeep your visor on!", design.StyleText(design.Font_Futura, 35, TextAnchor.MiddleCenter, new Color(224/255f,0/255f,0/255f,1f)));
		}

		// ************************************************************************
		private void VISOR_CONTROL()
		{
			if (Input.GetKeyDown (KeyCode.V)) {
				isVisor = !isVisor;
			}
		}

		private void InitVariables()
		{
			isVisor = false;
			
			CaptureSequenceLevelObjects.Instance.object_hatch.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_hatch.renderer.enabled = false;
			CaptureSequenceLevelObjects.Instance.object_door.animation.Stop();
			
			CaptureSequenceLevelObjects.Instance.object_here01.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_here01.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_here02.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_here02.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_close_door.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_close_door.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_camera.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_camera.renderer.enabled = false;
			
			mission.ISMENU = false;
			mission.LEVELPIVOT = 0; // initilize objective
			_sequence = SECTION00;
			mission.IS_OBJ_DONE = false;
			mission.ISFAIL = false;
			character.HEALTH = 100;
		}

		private void SECTION00(){
			if (Input.GetKeyDown(KeyCode.V)){
				CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_beep_high);
				CaptureSequenceLevelObjects.Instance.audio.Play();
				mission.IS_OBJ_DONE = true;
				mission.LEVELPIVOT = 1; // Next objective (1)
				_sequence = SECTION01;
				
				// Show the glowing ball to open the hatch
				CaptureSequenceLevelObjects.Instance.object_hatch.SetActive(false);
				CaptureSequenceLevelObjects.Instance.object_hatch.SetActive(true);
				CaptureSequenceLevelObjects.Instance.object_hatch.animation.wrapMode = WrapMode.Loop;
			}
		}
		
		// ************************************************************************
		// Object 01
		private void SECTION01(){
			
			mission.IS_OBJ_DONE = false;
			
			if((isVisor == true)&&(mission.LEVELPIVOT == 1)){
				//Click to open the hatch
				if(Input.GetMouseButton(0)){
					RaycastHit hitInfo = new RaycastHit();
					bool hit = Physics.Raycast(_fpCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
					if (hit) {
						if(hitInfo.collider.gameObject.CompareTag("guide01_hatch")){
							CaptureSequenceLevelObjects.Instance.object_hatch.SetActive(false);
							CaptureSequenceLevelObjects.Instance.object_door.animation.Play();
							CaptureSequenceLevelObjects.Instance.object_hatch.animation.wrapMode = WrapMode.Once;
							CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_door_open_source);
							CaptureSequenceLevelObjects.Instance.audio.Play();
							
							mission.IS_OBJ_DONE = true;
							mission.LEVELPIVOT = 2; // Next objective (2)
							_sequence = SECTION02;
							
							CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_beep_high);
							CaptureSequenceLevelObjects.Instance.audio.Play();
						}
					}
				}
			}
		}
		
		// ************************************************************************
		// Object 02
		private void SECTION02(){
			
			if(isTimeLeft == false)
				timeLeft -= Time.deltaTime;
			
			// Waiting for the capsule door to open
			if(timeLeft < 0){
				if(!CaptureSequenceLevelObjects.Instance._playerController.Enabled)
				{
					CaptureSequenceLevelObjects.Instance._playerController.Enabled = true;
				}
				
				//INPUT_KEYS ();
				mission.IS_OBJ_DONE = false;
				isTimeLeft = true;
				CaptureSequenceLevelObjects.Instance.object_here01.SetActive(true);
				CaptureSequenceLevelObjects.Instance.object_here01.animation.wrapMode = WrapMode.Loop;
			}
		}
		
		// ************************************************************************
		// Object 03
		private void SECTION03()
		{
			//INPUT_KEYS ();
			mission.IS_OBJ_DONE = false;
			CaptureSequenceLevelObjects.Instance.object_here01.SetActive(false);
			CaptureSequenceLevelObjects.Instance.object_here01.renderer.enabled = false;
			
			CaptureSequenceLevelObjects.Instance.object_here02.SetActive(true);
			CaptureSequenceLevelObjects.Instance.object_here02.animation.wrapMode = WrapMode.Loop;
			
			if(Input.GetMouseButton(0)){
				RaycastHit hitInfo = new RaycastHit();
				bool hit = Physics.Raycast(_fpCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
				if (hit) {
					if(hitInfo.collider.gameObject.CompareTag("guide03_camera")){
						CaptureSequenceLevelObjects.Instance.object_here02.SetActive(false);
						CaptureSequenceLevelObjects.Instance.object_here02.renderer.enabled = false;
						
						CaptureSequenceLevelObjects.Instance.object_camera.SetActive(true);
						CaptureSequenceLevelObjects.Instance.object_camera.renderer.enabled = true;
						
						CaptureSequenceLevelObjects.Instance.object_close_door.SetActive(true);
						CaptureSequenceLevelObjects.Instance.object_close_door.animation.wrapMode = WrapMode.Loop;
						
						mission.IS_OBJ_DONE = true;
						mission.LEVELPIVOT = 4; // Next objective (4)
						_sequence = SECTION04;
						CaptureSequenceLevelObjects.Instance._imageCaptureManager.Enabled = false;
						CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_beep_high);
						CaptureSequenceLevelObjects.Instance.audio.Play();
					}
				}
			}
		}
		
		
		// ************************************************************************
		// Object 04
		private void SECTION04()
		{
			//INPUT_KEYS ();
			
			if(Input.GetMouseButton(0)){
				RaycastHit hitInfo = new RaycastHit();
				bool hit = Physics.Raycast(_fpCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
				if (hit) {
					if(hitInfo.collider.gameObject.CompareTag("guide04_return")){
						CaptureSequenceLevelObjects.Instance.audio.PlayOneShot(AudioManager.Instance.sound_beep_high);
						CaptureSequenceLevelObjects.Instance.audio.Play();

						Controller.AddSequence(null);
					}
				}
			}
		}
	}
}