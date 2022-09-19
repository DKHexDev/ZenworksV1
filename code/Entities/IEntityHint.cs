using Sandbox.UI;

namespace ZenWorks.Entities
{
	public interface IEntityHint
	{
		/// <summary>
		/// The max viewable distance of the hint.
		/// </summary>
		float HintDistance => 2048f;

		/// <summary>
		/// If we should show a glow around the entity.
		/// </summary>
		bool ShowGlow => true;

		/// <summary>
		/// Whether or not we can show the UI hint.
		/// </summary>
		bool CanHint(Character character);

		/// <summary>
		/// The hint we should display.
		/// </summary>
		Panel DisplayHint(Character character);

		/// <summary>
		/// Occurs on each tick if the hint is active.
		/// </summary>
		void HintTick(Character character);
	}
}
