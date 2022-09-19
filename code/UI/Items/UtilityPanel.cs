using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using ZenWorks.Entities;
using ZenWorks.Items;
using ZenWorks.Items.Foods;

namespace ZenWorks.UI.Items
{
	public partial class UtilityPanel : Panel
	{
		private Character Character { get; set; }
		private ItemEntity Entity { get; set; }
		private Item Item { get; set; }
		
		private Panel Border, Container;
		private List<Button> Buttons { get; set; } = new();

		private string LastOverPanel { get; set; }

		public UtilityPanel( Character character, ItemEntity entity )
		{
			Utils.PositionAtCrosshair( this, character );

			Character = character;
			Entity = entity;
			Item = entity.Item;
			
			Border = Add.Panel( "Border" );
			Container = Add.Panel( "Container" );

			if ( entity.Item != null && entity.Item is Food food )
			{
				if (food.CanDrink)
					Buttons.Add( Container.Add.Button("Boire", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_entity", "drink" );
						Delete( false );
					} ) );
				
				if (food.CanEat)
					Buttons.Add( Container. Add.Button("Manger", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_entity", "eat" );
						Delete( false );
					} ) );
			}

			if ( entity.Item != null )
			{
				if ( entity.Item.CanTake )
					Buttons.Add( Container.Add.Button("Prendre", "Button", () =>
					{
						ConsoleSystem.Run( "zw_use_item_entity", "take" );
						Delete( false );
					} ) );
			}

			Buttons.Add(Container.Add.Button("Quitter", "Button", () => Delete(false) ));

			foreach ( Button button in Buttons )
			{
				button.AddEventListener( "onmouseover", ( e ) =>
				{
					if (LastOverPanel != "textpanel" && e.Target.ElementName != "textpanel")
						PlaySound( "mouseover.normal" );

					LastOverPanel = e.Target.ElementName;
				} );
				button.AddEventListener( "onmouseout", () => LastOverPanel = null );
			}
		}

		public override void Tick()
		{
			base.Tick();
			
			var tr = Trace.Ray( Character.EyePosition, Character.EyePosition + Character.EyeRotation.Forward * 200 )
				.UseHitboxes()
				.Ignore( Character )
				.Size( 2 )
				.Run();
			
			if (!tr.Entity.Equals(Entity))
				Delete();
		}
	}
}
