using Sandbox;
using Sandbox.UI;
using ZenWorks.UI.Components;
using ZenWorks.UI.Notifications;

namespace ZenWorks.UI.MainMenus
{
	public class MainMenuContent : NavigatorPanel
	{
		public MainMenuContent()
		{
			StyleSheet.Load( "/UI/MainMenus/MainMenu.scss" );
			Navigate( "/mainmenu/home" );
		}

		protected override void NotFound( string url )
		{
			base.NotFound( url );

			var client = Local.Client;
			if ( client == null ) return;
			
			if ( NotificationContainer.Current == null ) return;
			NotificationContainer.Current.Notify( "Action impossible !", 10f, "warning.png" );
		}
	}
}
