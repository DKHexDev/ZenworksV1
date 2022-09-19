using Sandbox.UI;
using ZenWorks.Items;

namespace ZenWorks.UI.Items
{
	public class ItemInfo : Panel
	{
		public ItemInfo(Item item, Character character)
		{
			StyleSheet.Load( "/UI/Items/Items.scss" );
			Utils.PositionAtCrosshair( this, character );
		}
	}
}
