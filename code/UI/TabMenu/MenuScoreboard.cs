using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;
using ZenWorks.Factions;

namespace ZenWorks.UI.TabMenu
{
	public class MenuScoreboard : Panel
	{
		VirtualScrollPanel Canvas;

		public MenuScoreboard()
		{
			AddClass( "sheet" );

			var container = Add.Panel( "Container" );
			container.AddChild( out Canvas, "Canvas" );

			Canvas.Layout.Columns = 1;
			Canvas.Layout.ItemWidth = Length.Percent( 100f ).GetValueOrDefault();
			Canvas.Layout.ItemHeight = Length.Pixels( 150f ).GetValueOrDefault();
			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var entry = (MenuScoreboardEntry)data;
				cell.AddChild( entry );
			};
		}

		public override void Tick()
		{
			base.Tick();
			
			var isModified = false;
			
			foreach ( Client client in Client.All )
				if ( !Canvas.Data.Exists( x => (x as MenuScoreboardEntry)?.Client.PlayerId == client.PlayerId ) )
				{
					Canvas.Data.Add( new MenuScoreboardEntry( client ) );
					isModified = true;
				}
			
			foreach ( MenuScoreboardEntry entry in Canvas.Data )
				if ( !Client.All.Contains( entry.Client ) )
				{
					Canvas.Data.Clear();
					isModified = true;
					break;
				}

			if ( isModified ) Canvas.NeedsRebuild = true;
		}
	}

	public class MenuScoreboardEntry : Panel
	{
		public Client Client { get; private set; }

		private Panel Container, FactionContainer, AvatarContainer, InfosContainer;
		private Label FactionName, Name, Description;

		public MenuScoreboardEntry( Client client )
		{
			Client = client;
			FactionContainer = Add.Panel( "Faction" );
			FactionName = FactionContainer.Add.Label( client.Pawn is Character {Faction: { }} character ? character.Faction.Name : "Inconnu" );
			Container = Add.Panel("Container");
			AvatarContainer = Container.Add.Panel( "Avatar" );
			AvatarContainer.Style.BackgroundImage = Texture.LoadAvatar( Client.PlayerId );
			InfosContainer = Container.Add.Panel( "Infos" );
			Name = InfosContainer.Add.Label( client.Name, "Name" );
			Description = InfosContainer.Add.Label( client.PlayerId.ToString(), "Description" );
		}

		public override void Tick()
		{
			base.Tick();
			
			if ( Client == null ) return;
			
			var character = Client.Pawn as Character;

			if ( FactionContainer.Parent == null )
				FactionContainer = Add.Panel( "Faction" );

			FactionContainer.Style.BackgroundColor = character == null ? Color.Parse( "#828483" ) : character.Faction.Color;
			
			if ( FactionName.Parent == null )
				FactionName = FactionContainer.Add.Label( "Inconnu" );

			FactionName.Text = character == null ? "Inconnu" : character.Faction.Name;
			
			if ( Container.Parent == null )
				Container = Add.Panel("Container");
			
			Container.Style.BorderColor = character == null ? Color.Parse( "#828483" ) : character.Faction.Color;
			
			if ( AvatarContainer.Parent == null ) 
				AvatarContainer = Container.Add.Panel( "Avatar" );
			
			AvatarContainer.Style.BackgroundImage = character == null ? Texture.LoadAvatar( Client.PlayerId ) : Texture.Load( FileSystem.Mounted, character.Faction.Image );
				
			if ( InfosContainer.Parent == null )
				InfosContainer = Container.Add.Panel( "Infos" );
			
			if ( Name.Parent == null )
				Name = InfosContainer.Add.Label( "", "Name" );
			
			Name.Text = character == null ? Client.Name : character.Fullname;
			Name.Style.BorderColor = character == null ? Color.Parse( "#828483" ) : character.Faction.Color;
			
			if ( Description.Parent == null )
				Description = InfosContainer.Add.Label( "", "Description" );
			
			Description.Text = character == null ? Client.PlayerId.ToString() : character.Description;
		}
	}
}
