using UnityEngine;
using Design;

/*
 * This is the planet system intro sequence where all the planets are shown.
*/

namespace ProjectSpaceWalk
{
	public class Level02PlanetIntroSequence : SequenceBase
	{
		private float _time = 0;

		public Level02PlanetIntroSequence(ISequenceController controller, Planet planetToVisit)
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
			_cutSceneCamera.gameObject.SetActive (false);
			_fpCamera.gameObject.SetActive (false);
			_planetIntroCamera.gameObject.SetActive (true);
		}

		public override void OnGUI ()
		{
			design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), Color.black, _fadeValue);
		}

		public override void Update ()
		{
			FadeControl ();
			_time += Time.deltaTime;

			if(_time >= 4 && !_isfadeOut)
			{
				_isfadeOut = true;
			}

			if(_time > 5)
			{
				Controller.AddSequence(new Level02IntroSequence(Controller, _planetToVisit));
			}
		}
	}
}