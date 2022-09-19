using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using ZenWorks.CharacterInfos;
using ZenWorks.UI.MainMenus;
using ZenWorks.UI.Notifications;

namespace ZenWorks
{
	[Library]
	public class Hud : HudEntity<RootPanel>
	{
		public static Hud Current { get; private set; }

		public Hud()
		{
			Current = this;
			
			if ( !IsClient )
				return;
			
			RootPanel.StyleSheet.Load( "/UI/Hud.scss" );
			
			/*RootPanel.AddChild<ChatBox>();*/
			/*RootPanel.AddChild<CharacterInfo>();*/
			RootPanel.AddChild<MainMenu>();
			RootPanel.AddChild<NotificationContainer>();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Current = null;
		}
	}
}
