using UnityEngine;
using Mission;
using Design;
using Character;

/*
 * This class act as the entry point to Level02.
 * Everything is controlled from here.
 */

namespace ProjectSpaceWalk
{
	public sealed class Level02Controller : MonoBehaviour, ISequenceController
	{
		//Hold the current sequence
		public ISequence CurrentSequence{ get; private set; }

		[SerializeField] private Planet[] _planetsToVisit;

		private Texture2D button_blank = null, 
		button_blue = null, 
		button_blue_hover = null;

		private float _time = 90f;
		private float _health = 100;

		private int index = 0;

		private bool _isGameOver;

		private void Awake()
		{
			Time.timeScale = 1;
			mission.ISMENU = false;
			character.health = 100;
			mission.ReduceHealth = false;
			mission.UpdateTimer = false;
		}

		// Method used to add a new sequence to execute
		public void AddSequence(ISequence sequence)
		{
			if (CurrentSequence != null)
			{
				CurrentSequence.Destroy();
				CurrentSequence = null;
			}

			if(sequence != null)
			{
				CurrentSequence = sequence;
				CurrentSequence.Initialize ();
			}
			else
			{
				index++;
				if(index < _planetsToVisit.Length)
				{
					AddSequence(new Level02IntroSequence(this, _planetsToVisit[index]));
					return;
				}
				else
				{
					AddSequence(new MissionCompleteSequence(this));
				}
			}
		}

		// ************************************************************************
		// Pauses the game
		private void INPUT_PAUSE(){
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button16)) {
				switch(mission.ISMENU){
				case true:
					Time.timeScale = 1; //continues the game
					AudioManager.Instance.sound_engine.mute = false;
					AudioManager.Instance.sound_engine_inside.mute = false;
					AudioManager.Instance.music_background.mute = false;
					AudioManager.Instance.astronaut_breathing.mute = false;
					break;
				case false:
					Time.timeScale = 0; //freeze the game
					AudioManager.Instance.sound_engine.mute = true;
					AudioManager.Instance.sound_engine_inside.mute = true;
					AudioManager.Instance.music_background.mute = true;
					AudioManager.Instance.astronaut_breathing.mute = true;
					break;
				}
				mission.ISMENU = !mission.ISMENU; //toggles the pause menu
			}
		}

		// Used to display all the UI related stuff
		private void OnGUI()
		{
			if (_isGameOver)
			{
				design.MissionAbort(design.Font_Futura, button_blank, button_blue, button_blue_hover, "level_02");
				return;
			}

			if(mission.ReduceHealth)
			{
				return;
			}

			// Display the main menu options
			if(mission.ISMENU == true)
			{
				design.StatePause(design.Font_Futura, button_blank, button_blue, button_blue_hover);
			}
			else if (CurrentSequence != null)
			{
				CurrentSequence.OnGUI();
			}
		}

		// Used for initialization
		private void Start()
		{
			button_blue = (Texture2D)Resources.Load("images/button-blue");
			button_blue_hover = (Texture2D)Resources.Load("images/button-blue_hover");
			button_blank = (Texture2D)Resources.Load("images/button-blank");

			AddSequence (new Level02PlanetIntroSequence (this, _planetsToVisit [0]));
		}

		// Called every frame
		private void Update()
		{
			if(mission.UpdateTimer && _time > 0)
			{
				_time -= Time.deltaTime;
				_time = Mathf.Clamp(_time, 0, 90);

				int seconds = (int)(_time % 60f);
				int minute = (int)(_time / 60f);
				int hour = (int)(minute / 60f);
				minute = minute % 60;

				design.TIMER_FORMATTED = string.Format("{1:D2}:{2:D2}", hour, minute, seconds);
				if(_time <= 0)
				{
					_isGameOver = true;
				}
			}

			if (mission.ReduceHealth && _health > 0)
			{
				_health -= 10 * Time.deltaTime;
				_health = Mathf.Clamp(_health, 0, 100);
				character.HEALTH = (int)_health;
				if(_health <= 0)
				{
					_isGameOver = true;
				}
			}

			INPUT_PAUSE(); // PAUSE SCREEN: If the user press the "ESC" command on their keyboard, the game pauses.

			if (CurrentSequence != null)
			{
				CurrentSequence.Update();
			}
		}
	}
}