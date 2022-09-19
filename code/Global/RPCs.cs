using System;
using Sandbox;
using ZenWorks.Factions;
using ZenWorks.UI.Notifications;

namespace ZenWorks.Global
{
	public partial class RPCs
	{
		[ClientRpc]
		public static void SetClientFaction(Character character, string libraryName)
		{
			if (character == null || !character.IsValid()) return;
			if ( String.IsNullOrEmpty( libraryName ) || String.IsNullOrWhiteSpace( libraryName ) ) return;
			character.SetFaction(Utils.GetObjectByType<Faction>(Utils.GetTypeByLibraryName<Faction>( libraryName )));
		}
		
		[ClientRpc]
		public static void ClientNotify(string text, float timeBeforeDelete = 10f, string icon = null, string sound = null)
		{
			if ( NotificationContainer.Current == null ) return;
			NotificationContainer.Current.Notify( text, timeBeforeDelete, icon, sound );
		}
	}
}
