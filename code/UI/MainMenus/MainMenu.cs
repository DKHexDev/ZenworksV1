using Sandbox.UI;

namespace ZenWorks.UI.MainMenus
{
	public class MainMenu : Panel
	{
		public static MainMenu Current { get; private set; }

		public MainMenuHeader Header { get; private set; }
		public MainMenuContent Content { get; private set; }
		public MainMenuFooter Footer { get; private set; }
		
		public MainMenu()
		{
			Current = this;
			StyleSheet.Load( "UI/MainMenus/MainMenu.scss" );

			Header = AddChild<MainMenuHeader>();
			Content = AddChild<MainMenuContent>();
			Footer = AddChild<MainMenuFooter>();
		}

		public override void OnDeleted()
		{
			base.OnDeleted();
			Current = null;
		}
	}
}
