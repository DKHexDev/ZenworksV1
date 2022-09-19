using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;
using ZenWorks.Factions;
using ZenWorks.UI.Components;
using ZenWorks.UI.Components.Buttons;

namespace ZenWorks.UI.MainMenus.Pages
{
	[NavigatorTarget("/mainmenu/load")]
	public class Load : Panel
	{
		VirtualScrollPanel Canvas;
		
		public Load()
		{
			StyleSheet.Load( "/UI/MainMenus/Pages/Pages.scss" );
			
			var container = Add.Panel( "Container" );
			container.AddChild( out Canvas, "Canvas" );
			
			Canvas.Layout.ItemWidth = Length.Percent( 33.33f ).GetValueOrDefault();
			Canvas.Layout.ItemHeight = Length.Percent( 50f ).GetValueOrDefault();
			Canvas.Layout.Columns = 3;

			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var button = (ButtonImage)data;
				cell.AddChild( button );
			};
		}

		public override void Tick()
		{
			base.Tick();

			var client = Local.Client;
			if ( client == null ) return;

			var characters = DataManager.GetCharacters( Local.Client );
			if ( characters == null ) return;

			if ( characters.Count == Canvas.Data.Count ) return;
			
			foreach ( var data in characters )
			{
				var faction = Faction.GetFaction( data.Value.Faction );
				Canvas.Data.Add( new ButtonImage( data.Value.Name, () =>
				{
					ConsoleSystem.Run( "zw_char_load", data.Value.IndexData );
					MainMenu.Current.Delete();
				}, faction.Image, true ) );
			}

			Canvas.NeedsRebuild = true;
		}
	}
}
