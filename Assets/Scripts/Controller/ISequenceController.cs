using UnityEngine;

/*
 *This is the skeleton of a sequence controller.
 *Any class which is going to excecute a sequence should implement this inteface
 */

namespace ProjectSpaceWalk
{
	public interface ISequenceController
	{
		void AddSequence(ISequence sequence);
	}
}