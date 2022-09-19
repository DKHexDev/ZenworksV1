using System;
using Sandbox;
using ZenWorks.UI.Notifications;

namespace ZenWorks
{
	public partial class Character
	{
		public void Notify( string text, float timeBeforeDelete = 10f, string icon = null, string sound = null )
		{
			if ( String.IsNullOrEmpty( text ) || String.IsNullOrWhiteSpace( text ) ) return;
			
			if ( Host.IsServer )
				SendNotifyToClient( To.Single( this ), text, timeBeforeDelete, icon, sound );
		}
		
		private void SendNotifyToClient( To? to = null, string text = null, float timeBeforeDelete = 10f, string icon = null, string sound = null)
		{
			RPCs.ClientNotify( to ?? To.Single(this), text, timeBeforeDelete, icon, sound );
		}	
	}
}
