using System.Linq;
using Sandbox;
using Sandbox.UI;
using ZenWorks.Items;
using ZenWorks.UI.Items;

namespace ZenWorks.Entities
{
	public partial class ItemEntity : Prop, IEntityHint, IUse
	{
		[Net] public Item Item { get; set; }
		
		public ItemEntity() { }

		public ItemEntity( Item item )
		{
			Name = item.LibraryName;
			Item = item;
			SetModel( item.Model );
		}
		
		public override void Spawn()
		{
			base.Spawn();
			
			SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);
			CollisionGroup = CollisionGroup.Always;
			SetInteractsAs(CollisionLayer.Debris);

			Tags.Add(IItem.Tag);
			Tags.Add( "Usable" );
		}

		public bool CanHint( Character character )
		{
			return false;
		}

		public Panel DisplayHint( Character character )
		{
			return new ItemInfo( Item, character );
		}

		public void HintTick( Character character )
		{
			throw new System.NotImplementedException();
		}

		public bool OnUse( Entity user )
		{
			if ( user is not Character ) return true;
			OpenUtilityMenu(To.Single( user ), this);
			return true;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
		
		[ClientRpc]
		private void OpenUtilityMenu(ItemEntity entity = null)
		{
			var hud = Local.Hud;
			if ( hud.ChildrenOfType<UtilityPanel>().ToArray().Length == 0 && entity != null )
				hud.AddChild( new UtilityPanel( Local.Client.Pawn as Character, entity ) );
		}

	}
}
