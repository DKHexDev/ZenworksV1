using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZenWorks.Data.Modals;
using ZenWorks.Entities;
using ZenWorks.Factions;
using ZenWorks.Items;

namespace ZenWorks
{
	public partial class Game : Sandbox.Game
	{
		public static Game Current { get; private set; }

		public Game()
		{
			Current = this;
			
			foreach ( var type in Utils.GetTypes<Faction>() ) 
					TypeLibrary.Create<Faction>( type );
			
			if ( IsServer )
			{
				_ = new Hud();
			}
			
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			DataManager.SaveClient( client );
		}
		
		

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );
			
			if ( cl.Pawn != null && cl.Pawn is Character character )
				character.Save();
			
			DataManager.SaveClient( cl );
		}

		protected override void OnDestroy()
		{
			if ( IsServer )
				foreach ( var client in All.OfType<Client>() )
				{
					if ( client.Pawn != null && client.Pawn is Character character )
						character.Save();
					DataManager.SaveClient( client );
				}
			
			base.OnDestroy();
		}
	}
}


