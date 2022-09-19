using Sandbox;
using Sandbox.UI;
using ZenWorks.UI.MainMenus;
using ZenWorks.UI.TabMenu;

namespace ZenWorks.UI.TabMenu
{
	public partial class Menu : TabContainer
	{
		/// <summary>
		/// The instance of the menu
		/// </summary>
		public static Menu Current { get; private set; }
		
		/// <summary>
		/// The inventory page in the tab menu
		/// </summary>
		public MenuInventory MenuInventory { get; private set; }
		
		/// <summary>
		/// The scoreboard page in the tab menu
		/// </summary>
		public MenuScoreboard MenuScoreboard { get; private set; }
		
		public Menu()
		{
			Current = this;
			StyleSheet.Load( "UI/TabMenu/Menu.scss" );

			MenuInventory = new MenuInventory();
			MenuScoreboard = new MenuScoreboard();
			
			AddTab( MenuInventory,"Inventaire", "Inventaire" );
			AddTab( new Panel(),"Personnages", "Personnages" );
			AddTab( MenuScoreboard,"Joueurs", "Joueurs" );
			AddTab( new Panel(), "Options", "Options" );

			foreach ( var tab in Tabs )
			{
				/*tab.Button.Text = tab.TabName;*/
				if ( tab.Button.Text == "Personnages" )
				{
					tab.Button.AddEventListener( "onclick", () =>
					{
						var hud = Local.Hud;
						if ( hud == null ) return;
						hud.AddChild<MainMenu>();
						MainMenu.Current.Content.Navigate( "/mainmenu/load" );
						Delete(true);
					} );	
				}
					
			}
				
		}

		public async override void Delete( bool immediate = false )
		{
			if ( !immediate )
			{
				if ( MenuInventory != null && MenuInventory.MainInventory != null )
				{
					MenuInventory.MainInventory.Delete();
					await Task.DelaySeconds( 0.8f );	
				}
			}
			
			base.Delete( immediate );
		}

		public override void OnDeleted()
		{
			base.OnDeleted();
			Current = null;
		}
	}
}
