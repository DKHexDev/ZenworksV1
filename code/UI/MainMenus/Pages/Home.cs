using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;
using ZenWorks.UI.Components;
using ZenWorks.UI.Components.Buttons;

namespace ZenWorks.UI.MainMenus.Pages
{
	[NavigatorTarget("/mainmenu/home")]
	public class Home : Panel
	{
		VirtualScrollPanel Canvas;

		public Home()
		{
			StyleSheet.Load( "/UI/MainMenus/Pages/Pages.scss" );

			var container = Add.Panel( "Container" );
			container.AddChild( out Canvas, "Canvas" );
			
			Canvas.Layout.ItemWidth = Length.Percent( 33.33f ).GetValueOrDefault();
			Canvas.Layout.ItemHeight = Length.Percent( 100f ).GetValueOrDefault();
			Canvas.Layout.Columns = 3;

			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var button = (ButtonImage)data;
				cell.AddChild( button );
			};

			Canvas.Data.Add( new ButtonImage( "Créer un personnage",
				() => (Parent as NavigatorPanel)?.Navigate( "/mainmenu/create/step1" ), "/ui/backgrounds/play.jpg", true ) );
			
			Canvas.Data.Add( new ButtonImage( "Charger un personnage", 
				() => (Parent as NavigatorPanel)?.Navigate( "/mainmenu/load" ), "/ui/backgrounds/folder.jpg" ) );
			
			Canvas.Data.Add( new ButtonImage( "Options", 
				() => (Parent as NavigatorPanel)?.Navigate( "/mainmenu/settings" ), "/ui/backgrounds/settings.jpg" ) );
		}
	}
}
