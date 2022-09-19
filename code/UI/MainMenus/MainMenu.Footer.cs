using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ZenWorks.UI.MainMenus
{
	public class MainMenuFooter : Panel
	{
		private Button BackButton;

		public MainMenuFooter()
		{
			StyleSheet.Load( "/UI/MainMenus/MainMenu.scss" );
			
			BackButton = Add.Button("Précédent", "FooterBtn", GoBack );
		}

		private void GoBack()
		{
			if ( (Parent as MainMenu)?.Content.CurrentUrl == "/mainmenu/home" && Local.Pawn != null )
				MainMenu.Current.Delete();
			else (Parent as MainMenu)?.Content?.GoBack(); 
		}

		public override void Tick()
		{
			base.Tick();

			if ( Parent is not MainMenu mainMenu || BackButton == null ) return;
			if ( (Parent as MainMenu)?.Content == null ) return;

			if ( mainMenu.Content.HasHistory() && BackButton.HasClass( "hidden" ) )
				BackButton.RemoveClass( "hidden" );

			if ( !mainMenu.Content.HasHistory() && !BackButton.HasClass( "hidden" ) )
				BackButton.AddClass( "hidden" );

			if ( mainMenu.Content.CurrentUrl == "/mainmenu/home" && Local.Pawn != null )
			{
				BackButton.Text = "Retourner au jeu";
				BackButton.Style.Dirty();
				BackButton.Style.Width = Length.Percent( 100f );
				BackButton.RemoveClass( "hidden" );
			}
			else
			{
				BackButton.Style.Dirty();
				BackButton.Style.Width = Length.Percent( 50f );
				BackButton.Text = "Précédent";
			}
				
		}
	}
}
