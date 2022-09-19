using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ZenWorks.UI.MainMenus
{
	public class MainMenuHeader : Panel
	{
		private Panel Profil, ProfilInfos;
		private Image SteamAvatar;
		private Label SteamName, SteamId;
		
		public MainMenuHeader()
		{
			StyleSheet.Load( "/UI/MainMenus/MainMenu.scss" );

			Profil = Add.Panel( "Profil" );
			SteamAvatar = Profil.Add.Image( "", "Avatar" );
			ProfilInfos = Profil.Add.Panel( "Infos" );
			SteamName = ProfilInfos.Add.Label( "" );
			SteamId = ProfilInfos.Add.Label( "" );
		}

		public override void Tick()
		{
			base.Tick();

			var client = Local.Client;
			if ( client == null ) return;
			
			SteamAvatar.Texture = Texture.LoadAvatar( client.PlayerId );
			SteamName.Text = $"Bonjour, {client.Name}";
			SteamId.Text = client.PlayerId.ToString();
		}
	}
}
