using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using ZenWorks.Items;
using ZenWorks.UI.Components;

namespace ZenWorks.UI.TabMenu
{
	public class MenuInventory : Panel
	{
		/// <summary>
		/// Main character inventory
		/// </summary>
		public InventoryGrid MainInventory { get; private set; }
		
		public MenuInventory()
		{
			var client = Local.Client;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;

			AddClass( "sheet" );

			MainInventory = new InventoryGrid( null, inventory.Size, new Vector2(100f, 100f), inventory.Items );
			
			AddChild( MainInventory );
		}

		[Event("zw_main_inventory.refresh")]
		public void RefreshInventory()
		{
			var cl = Local.Client;
			if ( cl == null ) return;
				
			var pawn = cl.Pawn as Character;
			if ( pawn == null ) return;
				
			var inv = pawn.Inventory as Inventory;
			if ( inv == null ) return;

			MainInventory.Build( inv.Items, false );
		}
	}
}
