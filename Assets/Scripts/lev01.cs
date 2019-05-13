using UnityEngine;
using System.Collections;
using Design; //Loads the custom UI from Scripts/Library/design.cs
using Character; //Loads the custom UI from Scripts/Library/character.cs
using Mission; //Loads the mission from Scripts/Library/mission.cs
using CharControl; // Loads the inputs from Scripts/Library/input_keylistener.cs
using ProjectSpaceWalk;
using System;

[RequireComponent(typeof(AudioSource))]
public class lev01 : MonoBehaviour {

	[SerializeField] private ImageCaptureManager _imageCaptureManager;
	[SerializeField] private PlayerController _playerController;
	private Action _sequence;

	public GameObject object_door = null,
	object_hatch = null,
	object_here01 = null,
	object_here02 = null,
	object_close_door = null,
	object_camera = null;

	public Camera cam_cutscene01 = null,
		cam_avatar = null;

	private AudioSource music_background = null,
	sound_engine = null,
	sound_engine_inside = null,
	astronaut_breathing = null,
	heart_beat = null;

	private AudioClip sound_door_open_source = null,
	sound_beep_high = null;

	private float oBackground = 0.0f,
		oShuttleName = 0.0f,
		oCrewNames = 0.0f;

	private bool isIntro = true,
		isPlayerView = false,
		isVisor = false;

	private Texture2D button_blank = null, 
	button_blue = null, 
	button_blue_hover = null;

	Vector3 originpos;
	Quaternion originquat;

	private float timeLeft = 8.0f;
	private bool isTimeLeft = false;

	private int health_counter = 0;
	private float opactiy_vision = 0.0f;

	public Collider target;

	private bool isDone = false, isObjective = false;

	private int dMin = 0, dSec = 0, timeCounter = 0;

	//************************************************************************
	// Use this for initialization
	void Start () {
		// Add background Music to the main menu
		music_background = (AudioSource)gameObject.AddComponent ("AudioSource");
		AudioClip music_source;
		music_source = (AudioClip)Resources.Load ("sounds/mm_level01");
		music_background.clip = music_source;
		music_background.Play ();
		music_background.loop = true;
		music_background.volume = 0.2f;

		// Add capsule's engine sound effects (SFX)
		sound_engine = (AudioSource)gameObject.AddComponent ("AudioSource");
		AudioClip sound_engine_source;
		sound_engine_source = (AudioClip)Resources.Load ("sounds/rocket-thrusters");
		sound_engine.clip = sound_engine_source;
		sound_engine.Play ();
		sound_engine.loop = true;
		sound_engine.volume = 0.1f;

		// Inside Engine sound
		sound_engine_inside = (AudioSource)gameObject.AddComponent ("AudioSource");
		AudioClip sound_engine_inside_source;
		sound_engine_inside_source = (AudioClip)Resources.Load ("sounds/capsule-inside");
		sound_engine_inside.clip = sound_engine_inside_source;
		sound_engine_inside.Play ();
		sound_engine_inside.loop = true;
		sound_engine_inside.volume = 0f;


		//Astronaut Breathing noise
		astronaut_breathing = (AudioSource)gameObject.AddComponent ("AudioSource");
		AudioClip sound_astronaut_breathing;
		sound_astronaut_breathing = (AudioClip)Resources.Load ("sounds/astronaut-breathing");
		astronaut_breathing.clip = sound_astronaut_breathing;
		astronaut_breathing.Play ();
		astronaut_breathing.loop = true;
		astronaut_breathing.volume = 0f;

		//heart-beat
		heart_beat = (AudioSource)gameObject.AddComponent ("AudioSource");
		AudioClip sound_heart_beat;
		sound_heart_beat = (AudioClip)Resources.Load ("sounds/heart-beat");
		heart_beat.clip = sound_heart_beat;
		heart_beat.Play ();
		heart_beat.loop = true;
		heart_beat.volume = 0f;

		// AudioClip sound_door_open_source;
		sound_door_open_source = (AudioClip)Resources.Load ("sounds/door-open");

		// Sound beep
		sound_beep_high = (AudioClip)Resources.Load ("sounds/beep-high");

		// camera settings
		cam_cutscene01.enabled = true;
		cam_avatar.enabled = false;

		isIntro = true;
		isPlayerView = false;
		isVisor = false;

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

		button_blue = (Texture2D)Resources.Load("images/button-blue");
		button_blue_hover = (Texture2D)Resources.Load("images/button-blue_hover");
		button_blank = (Texture2D)Resources.Load("images/button-blank");

		mission.ISMENU = false;
		mission.LEVELPIVOT = 0; // initilize objective
		_sequence = SECTION00;
		mission.IS_OBJ_DONE = false;
		mission.ISFAIL = false;
		Time.timeScale = 1;
		character.HEALTH = 100;

		originpos = cam_avatar.transform.position;
		originquat = cam_avatar.transform.rotation;

		isDone = false;
		isObjective = false;
	}


	void Awake()
	{
		mission.ISFAIL = false;
	}

	//************************************************************************
	// Update is called once per frame
	void Update () {
		INPUT_PAUSE(); // PAUSE SCREEN: If the user press the "ESC" command on their keyboard, the game pauses.

		if(mission.ISFAIL == true){
			Time.timeScale = 0; // stops the game
			sound_engine.mute = true;
			sound_engine_inside.mute = true;
			music_background.mute = true;
			astronaut_breathing.mute = true;
			isVisor = false;
			opactiy_vision = 0.0f;
		}
		if((mission.ISFAIL == false)&&(isDone == false))
		{
			System.TimeSpan t = System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad);

			dMin = 0 - t.Minutes; // 2 minutes
			dSec = 59 - t.Seconds; // 59 seconds

			// Print out the countdown at the game ui
			design.TIMER_FORMATTED = string.Format("{1:D2}:{2:D2}", t.Hours, dMin, dSec);
	

			if((t.Seconds < 5)&&(isObjective == true)){
				timeCounter += 1;
			}

			if(timeCounter >= 5){
				isObjective = false;
				timeCounter = 0;
			}



			// The cutscene introducing the level
			LEVEL_INTRO_UPDATE ();


			VISOR_CONTROL(); // Controls the visor
			
			if(_sequence != null)
			{
				isObjective = true;
				_sequence();
			}

//			
//			switch(mission.LEVELPIVOT)
//			{
//			case 0:
//				SECTION00();
//				break;
//			case 1:
//				isObjective = true;
//				SECTION01();
//				break;
//			case 2:
//				isObjective = true;
//				SECTION02();
//				break;
//			case 3:
//				isObjective = true;
//				SECTION03();
//				break;
//			case 4:
//				isObjective = true;
//				SECTION04();
//				break;
//			}



			if(mission.LEVELPIVOT >= 2){
				switch(isVisor){
				case false:
					//If the visor is off
					health_counter++;

					heart_beat.mute = false;
					heart_beat.volume = 1.0f;
					astronaut_breathing.volume = 0.0f;
					music_background.volume = 0.0f;
					sound_engine_inside.volume = 0.0f;

					if(opactiy_vision < 0.8f)
						opactiy_vision += (Time.deltaTime);
					if(health_counter == 50 ) {
						character.health -= 20;
						health_counter = 0;
					}
					break;
					
				case true:
					//If the visor is on
					health_counter = 0;
					if(opactiy_vision > 0f)
						opactiy_vision -= (Time.deltaTime);
					if(opactiy_vision == 0f)
						opactiy_vision = 0f;

					heart_beat.mute = true;

					break;
				}

				// set on camera ui
				if (Input.GetKeyDown(KeyCode.C)){
					_imageCaptureManager.Enabled = !_imageCaptureManager.Enabled;
					_imageCaptureManager.OnCaptured += (int e) =>
					{
						if(e == 1){
							mission.IS_OBJ_DONE = true;
							mission.LEVELPIVOT = 3; // Next objective (3)
							_sequence = SECTION03;
							audio.PlayOneShot(sound_beep_high);
							audio.Play();
						}
					};
				}
			}
		}
	}

	// ************************************************************************
	// OnGUI is called for rendering and handling GUI events.
	void OnGUI() {

		// Display MISSION ACCOMPLISH screen
		if(isDone == true)
		{
			design.MissionSuccess(design.Font_Futura, button_blank, button_blue, button_blue_hover, "level_01");
		}

		// Display MISSION FAIL screen
		if((character.HEALTH <= 0)||((dMin == 0)&&(dSec == 0))){
			mission.ISFAIL = true;
			design.MissionAbort(design.Font_Futura, button_blank, button_blue, button_blue_hover, "level_01");
		}


		// Display the main menu options
		if((mission.ISMENU == true)&&(isDone == false)){
			design.StatePause(design.Font_Futura, button_blank, button_blue, button_blue_hover);
		}


		// Objectives 2 or greater must display a warning message if the user turns off the visor
		if((mission.LEVELPIVOT >= 2)){
			VISOR_WARNING();
		}

		CAM_BACKGROUND_GUI();

		// Play the intro cutscene
		if(isIntro == true)
			LEVEL_INTRO_GUI ();

		// Display the gameplay UI
		if(isPlayerView == true)
		{
			LEVEL_PLAY_GUI (); // Display the health meter, objectives, and other UI elements
		}
	}

	// ************************************************************************
	// Camera background color is black
	private void CAM_BACKGROUND_GUI(){
		cam_cutscene01.backgroundColor = Color.black;
		cam_avatar.backgroundColor = Color.black;
	}

	// ************************************************************************
	// Introduction of the level 
	private void LEVEL_INTRO_GUI(){
		design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), Color.black, oBackground);
		GUI.Label (new Rect ((Screen.width/2) - 20, 10, Screen.width/2, 55), "GEMINI 4", design.StyleText(design.Font_Futura, 35, TextAnchor.MiddleRight, new Color(1,1,1,oShuttleName)));
		GUI.Label (new Rect ((Screen.width/2) - 20, 45, Screen.width/2, 55), "James A. McDivitt, Command Pilot", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleRight, new Color(1,1,1,oCrewNames)));
		GUI.Label (new Rect ((Screen.width/2) - 20, 75, Screen.width/2, 55), "Edward H. White II, Pilot", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleRight, new Color(1,1,1,oCrewNames)));
	}

	// ************************************************************************
	private void LEVEL_PLAY_GUI(){
		//collide_hatch.renderer.enabled = true;
		design.GamePlayUI();

		// Displays the objectives at the top-right mid-corner
		if(isObjective == true)
			design.LevelStatusUI();

		// Turn on the visor
		if(isVisor == true)
			design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), new Color(155/255f,144/255f,101/255f), 0.2f);

		cam_cutscene01.enabled = false;
		cam_avatar.enabled = true;
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
	private void LEVEL_INTRO_UPDATE(){
		if(((Time.timeSinceLevelLoad) > 1)&&((Time.timeSinceLevelLoad) <= 8)) {
			if(oShuttleName < 1.0f)
				oShuttleName += Time.deltaTime;
		}
		
		if(((Time.timeSinceLevelLoad) > 3)&&((Time.timeSinceLevelLoad) <= 8)) {
			sound_engine.volume = 0.3f;
			if(oCrewNames < 1.0f)
				oCrewNames += Time.deltaTime;
		}
		
		if(((Time.timeSinceLevelLoad) > 5)&&((Time.timeSinceLevelLoad) <= 8)) {
			sound_engine.volume = 0.6f;
			if(oBackground < 1.0f)
				oBackground += Time.deltaTime;
		}
		
		if(Time.timeSinceLevelLoad > 8) {
			isIntro = false;
			isPlayerView = true;
			sound_engine.Stop();
			sound_engine_inside.volume = 1.0f;
			astronaut_breathing.volume = 0.5f;
		}
	}

	// ************************************************************************
	// Pauses the game
	private void INPUT_PAUSE(){
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button16)) {
			switch(mission.ISMENU){
			case true:
				Time.timeScale = 1; //continues the game
				sound_engine.mute = false;
				sound_engine_inside.mute = false;
				music_background.mute = false;
				astronaut_breathing.mute = false;
				break;
			case false:
				Time.timeScale = 0; //freeze the game
				sound_engine.mute = true;
				sound_engine_inside.mute = true;
				music_background.mute = true;
				astronaut_breathing.mute = true;
				break;
			}
			mission.ISMENU = !mission.ISMENU; //toggles the pause menu
		}
	}
	// ************************************************************************
	private void VISOR_CONTROL()
	{
		if (Input.GetKeyDown (KeyCode.V)) {
			isVisor = !isVisor;
		}
	}

	private void SECTION00(){
		if (Input.GetKeyDown(KeyCode.V)){
			audio.PlayOneShot(sound_beep_high);
			audio.Play();
			mission.IS_OBJ_DONE = true;
			mission.LEVELPIVOT = 1; // Next objective (1)
			_sequence = SECTION01;
			
			// Show the glowing ball to open the hatch
			object_hatch.SetActive(false);
			object_hatch.SetActive(true);
			object_hatch.animation.wrapMode = WrapMode.Loop;
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
				bool hit = Physics.Raycast(cam_avatar.ScreenPointToRay(Input.mousePosition), out hitInfo);
				if (hit) {
					if(hitInfo.collider.gameObject.CompareTag("guide01_hatch")){
						object_hatch.SetActive(false);
						object_door.animation.Play();
						object_hatch.animation.wrapMode = WrapMode.Once;
						audio.PlayOneShot(sound_door_open_source);
						audio.Play();

						mission.IS_OBJ_DONE = true;
						mission.LEVELPIVOT = 2; // Next objective (2)
						_sequence = SECTION02;

						audio.PlayOneShot(sound_beep_high);
						audio.Play();
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
			if(!_playerController.Enabled)
			{
				_playerController.Enabled = true;
			}

			//INPUT_KEYS ();
			mission.IS_OBJ_DONE = false;
			isTimeLeft = true;
			object_here01.SetActive(true);
			object_here01.animation.wrapMode = WrapMode.Loop;
		}
	}

	// ************************************************************************
	// Object 03
	private void SECTION03()
	{
		//INPUT_KEYS ();
		mission.IS_OBJ_DONE = false;
		object_here01.SetActive(false);
		object_here01.renderer.enabled = false;

		object_here02.SetActive(true);
		object_here02.animation.wrapMode = WrapMode.Loop;

		if(Input.GetMouseButton(0)){
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(cam_avatar.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit) {
				if(hitInfo.collider.gameObject.CompareTag("guide03_camera")){
					object_here02.SetActive(false);
					object_here02.renderer.enabled = false;

					object_camera.SetActive(true);
					object_camera.renderer.enabled = true;

					object_close_door.SetActive(true);
					object_close_door.animation.wrapMode = WrapMode.Loop;

					mission.IS_OBJ_DONE = true;
					mission.LEVELPIVOT = 4; // Next objective (4)
					_sequence = SECTION04;
					_imageCaptureManager.Enabled = false;
					audio.PlayOneShot(sound_beep_high);
					audio.Play();
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
			bool hit = Physics.Raycast(cam_avatar.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit) {
				if(hitInfo.collider.gameObject.CompareTag("guide04_return")){
					audio.PlayOneShot(sound_beep_high);
					audio.Play();

					isDone = true;
					Time.timeScale = 0; //freeze the game
					sound_engine.mute = true;
					sound_engine_inside.mute = true;
					music_background.mute = true;
					astronaut_breathing.mute = true;
				}
			}
		}
	}
}
