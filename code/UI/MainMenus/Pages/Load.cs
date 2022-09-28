using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;
using ZenWorks.Data.Modals;
using ZenWorks.Factions;
using ZenWorks.UI.Components;
using ZenWorks.UI.Components.Buttons;

namespace ZenWorks.UI.MainMenus.Pages
{
	[NavigatorTarget( "/mainmenu/load" )]
	public partial class Load : Panel
	{
		public static Load Current { get; private set; }

		public VirtualScrollPanel Canvas;

		public Load()
		{
			Current = this;
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

		public override void OnDeleted()
		{
			base.OnDeleted();
			Current = null;
		}

		public override void Tick()
		{
			base.Tick();

			var client = Local.Client;
			if ( client == null ) return;

			var characters =
				JsonSerializer.Deserialize<Dictionary<int, CharacterData>>(
					client.GetValue<string>( "Characters", null ) );
			if ( characters == null ) return;

			if ( characters.Count == Canvas.Data.Count ) return;

			Canvas.Data.Clear();

			foreach ( var data in characters )
			{
				var faction = Faction.GetFaction( data.Value.Faction );
				Canvas.Data.Add( new ButtonImage( data.Value.Name, () =>
				{
					ConsoleSystem.Run( "zw_char_load", data.Key );
					MainMenu.Current.Delete();
				}, faction.Image, true ) );
			}

			Canvas.NeedsRebuild = true;
		}
	}
}
