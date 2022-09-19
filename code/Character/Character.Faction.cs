using Sandbox;
using ZenWorks.Factions;

namespace ZenWorks
{
	public partial class Character
	{
		/// <summary>
		/// Faction of the character
		/// </summary>
		[Net] public Faction Faction { get; set; }
		
		public void SetFaction(Faction faction)
		{
			if ( faction == null ) return;
			
			Faction oldFaction = Faction;
			Faction = faction;

			if ( oldFaction != null && oldFaction.LibraryName == Faction.LibraryName )
				return;
			
			if ( oldFaction != null && oldFaction.LibraryName != Faction.LibraryName )
				Faction.OnTransferred( this );
			
			if ( oldFaction == null )
				Faction.OnSpawn(this);
			
			if (Host.IsServer)
				SendFactionToClient( To.Single( this ) );
		}
		
		private void SendFactionToClient(To? to = null)
		{
			RPCs.SetClientFaction(to ?? To.Single(this), this, Faction.LibraryName);
		}
	}
}
