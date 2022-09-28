using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;
using ZenWorks.Factions;
using ZenWorks.UI.Components;
using ZenWorks.UI.Components.Buttons;

namespace ZenWorks.UI.MainMenus.Pages
{
	[NavigatorTarget("/mainmenu/create")]
	public class Create : Panel
	{
		public static Create Current { get; private set; }

		private Label Title { get; set; }
		private Panel Container { get; set; }

		private int CurrentStep { get; set; } = 0;

		public Create()
		{
			Current = this;
			StyleSheet.Load( "/UI/MainMenus/Pages/Pages.scss" );

			Title = Add.Label( "Inconnu", "Title" );
			Container = Add.Panel( "Container" );
		}

		private void SwitchStep( int step )
		{
			switch ( step )
			{
				case 1:
					Title.Text = "Choix de la faction";
					Container.DeleteChildren( true );
					Container.AddChild<CreateStepOne>( "StepPage" );
					break;				
				case 2:
					Title.Text = "Votre personnage";
					Container.DeleteChildren( true );
					Container.AddChild<CreateStepTwo>( "StepPage" );
					break;
			}
		}

		public override void Tick()
		{
			base.Tick();

			var mainMenu = MainMenu.Current;
			if ( mainMenu == null || Container == null ) return;
			
			var step = mainMenu.Content.CurrentPanel.GetAttribute( "step", null );
			if ( step == null ) return;
			
			if ( Int32.Parse(step) == CurrentStep ) return;
			
			SwitchStep( Int32.Parse( step ) );
			CurrentStep = Int32.Parse( step );
		}
	}
	
	public class CreateStepOne : Panel
	{
		VirtualScrollPanel Canvas;

		public CreateStepOne()
		{
			var navigator = MainMenu.Current.Content;
			
			AddChild( out Canvas, "Canvas" );
			
			Canvas.Layout.ItemWidth = Length.Percent( 33.33f ).GetValueOrDefault();
			Canvas.Layout.ItemHeight = Length.Percent( 80f ).GetValueOrDefault();
			Canvas.Layout.Columns = 3;

			Canvas.OnCreateCell = ( cell, data ) =>
			{
				var button = (ButtonImage)data;
				cell.Style.Margin = Length.Pixels( 10f );
				cell.Style.MarginLeft = Length.Pixels( 0f );
				cell.Style.MarginRight = Length.Pixels( 0f );
				cell.AddChild( button );
			};

			foreach ( var row in Faction.All )
				Canvas.Data.Add( new ButtonImage( row.Value.Name, () => navigator.Navigate( $"/mainmenu/create?step=2&faction={row.Value.LibraryName}" ), row.Value.Image, true ) );
		}	
	}

	public class CreateStepTwo : Panel
	{
		private Form Form { get; set; }
		private Panel CoverFaction { get; set; }

		public CreateStepTwo()
		{
			var navigator = MainMenu.Current.Content;

			var container = Add.Panel("Container");
			CoverFaction = container.Add.Panel( "Cover" );
			Form = container.AddChild<Form>( "Form" );

			var nameField = Form.AddChild<Field>("Field");
			var nameEntry = nameField.AddChild<TextEntry>();
			
		}

		public override void Tick()
		{
			base.Tick();
			
			var navigator = MainMenu.Current.Content;
			if ( navigator == null ) return;

			var faction = Faction.GetFaction( navigator.CurrentPanel.GetAttribute( "faction" ) );
			if ( faction != null && CoverFaction != null )
				CoverFaction.Style.BackgroundImage = Texture.Load( FileSystem.Mounted, faction.Image );
			
			
			
		}
	}
}
