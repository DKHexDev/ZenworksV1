using System;
using System.Collections.Generic;
using Sandbox;
using ZenWorks.Data.Modals;
using ZenWorks.UI.Components;
using ZenWorks.UI.Notifications;

namespace ZenWorks.Factions
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class FactionAttribute : LibraryAttribute
	{
		public FactionAttribute( string name ) : base( $"zw_faction_{name}" ) { }
	}

	public abstract partial class Faction : BaseNetworkable, IFaction
	{
		public static Dictionary<string, Faction> All { get; set; } = new();
		
		public readonly string LibraryName;
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual Color Color { get; set; }
		public virtual List<string> Models { get; set; }
		public virtual string FootstepSound { get; set; } = "normal.footstep";
		public virtual string FootstepRunSound { get; set; } = "normal.footstep";

		public virtual string Overlay { get; set; } = "normal_overlay.png";

		public virtual string Image { get; set; } = null;

		public virtual float Salary { get; set; } = 0f;

		protected Faction()
		{
			LibraryName = Utils.GetLibraryName( GetType() );
			All[LibraryName] = this;
		}
		
		public virtual void OnSpawn(Character character)
		{/*
			Log.Info( $"{character.Name} a spawn dans la faction {Name}" );*/

			Log.Info( $"IsServer {Host.IsServer}" );
			Log.Info( $"IsClient {Host.IsClient}" );
			
			/*var notificationContainer = NotificationContainer.Current;
			
			if ( notificationContainer != null )
				notificationContainer.Notify( "test", 10f );

			var hud = Local.Hud;
			var overlay = UI.Components.Overlay.Current;

			// Creation of the faction overlay
			if ( overlay == null && hud != null ) hud.AddChild( new Overlay( Overlay ) );
			else if ( overlay != null && hud != null  ) overlay.SetOverlay( Overlay );*/
		}

		public virtual void OnPayed( Character character )
		{
			if ( Host.IsServer )
				character.Notify( $"Vous avez été payé {Salary} [currency]", 10f, "cash.png" );
		}

		public virtual void OnCharacterCreated( Character character )
		{
			if ( Host.IsServer )
				character.Notify( $"Vous avez spawn dans la faction '{Name}'" );
		}
		
		public virtual void OnTransferred( Character character )
		{
			if ( Host.IsServer )
				character.Notify( $"Vous avez été transféré dans la faction '{Name}'", 10f, "transfer.png" );
		}
		
		public static Faction GetFaction(string factionName)
		{
			if (string.IsNullOrEmpty(factionName))
				return null;

			Faction faction = All[factionName];

			if (faction == null)
				return null;
			
			Type type = Utils.GetTypeByLibraryName<Faction>(factionName);

			if (type == null)
				return null;

			return Utils.GetObjectByType<Faction>(type);
		}

		public static Faction GetFaction(Type factionType) => GetFaction(Utils.GetLibraryName(factionType));
	}
}
