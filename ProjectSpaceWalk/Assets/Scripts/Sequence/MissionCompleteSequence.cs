using Design;
using UnityEngine;

/*
 * This sequence shows mission completion stats
*/

namespace ProjectSpaceWalk
{
	public class MissionCompleteSequence : SequenceBase
	{
		private Texture2D button_blank = null, 
		button_blue = null, 
		button_blue_hover = null;

		public MissionCompleteSequence(ISequenceController controller)
			: base(controller)
		{ }

		public override void Destroy ()
		{

		}

		public override void Initialize ()
		{
			button_blue = (Texture2D)Resources.Load("images/button-blue");
			button_blue_hover = (Texture2D)Resources.Load("images/button-blue_hover");
			button_blank = (Texture2D)Resources.Load("images/button-blank");
		}

		public override void OnGUI ()
		{
			design.MissionSuccess(design.Font_Futura, button_blank, button_blue, button_blue_hover, "level_02");
		}

		public override void Update ()
		{

		}
	}
}