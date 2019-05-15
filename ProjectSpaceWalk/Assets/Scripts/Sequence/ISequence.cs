/*
 * This is the skeleton of an individual sequence.
 */

namespace ProjectSpaceWalk
{
	public interface ISequence
	{
		ISequenceController Controller{ get; }
		void Destroy();
		void Initialize();
		void OnGUI();
		void Update();
	}
}