using UnityEngine;

/*
 * This should be the place where all the audio used inside the game should process
 */

namespace ProjectSpaceWalk
{
	public sealed class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance{ get; private set; }

		[HideInInspector]
		public AudioSource music_background = null,
		sound_engine = null,
		sound_engine_inside = null,
		astronaut_breathing = null,
		heart_beat = null;

		[HideInInspector]
		public AudioClip sound_door_open_source = null,
		sound_beep_high = null;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			InitAudio ();
		}

		
		// Initializes all the audio required by the game
		private void InitAudio()
		{
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
		}

		private void OnDestroy()
		{
			if(Instance != null && Instance == this)
			{
				Instance = null;
			}
		}
	}
}