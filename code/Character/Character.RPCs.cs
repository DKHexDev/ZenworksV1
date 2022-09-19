using System;
using System.Linq;
using Sandbox;
using ZenWorks.Items;
using ZenWorks.UI.Components;
using ZenWorks.UI.Notifications;
using ZenWorks.UI.TabMenu;

namespace ZenWorks
{
	public partial class Character
	{
		[ClientRpc]
		public void AddItem( string libraryName, int withStack = 1 )
		{
			if ( String.IsNullOrEmpty( libraryName ) ) return;

			Item item = Utils.GetObjectByType<Item>( Utils.GetTypeByLibraryName<Item>( libraryName ) );
			item.SetStack( withStack );

			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;

			inventory.AddItem( item );
		}

		[ClientRpc]
		public void ClearInventory()
		{
			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;
			inventory.ClearItems();
		}

		[ClientRpc]
		public void SplitItem( int index )
		{
			if ( index == -1 ) return;
			
			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;
			if ( !inventory.ItemExist( index ) ) return;
			
			inventory.SplitItem( index );
		}

		[ClientRpc]
		public void RemoveItem( string libraryName )
		{
			if ( String.IsNullOrEmpty( libraryName ) ) return;
			
			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;
			if ( !inventory.ItemExist( libraryName ) ) return;

			var item = inventory.FindItem( libraryName );
			inventory.RemoveItem( item );
		}		
		
		[ClientRpc]
		public void RemoveItem( int index, bool allStack = false )
		{
			if ( index == -1 ) return;

			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;
			if ( !inventory.ItemExist( index ) ) return;
			
			inventory.RemoveItem( index, allStack );
		}

		[ClientRpc]
		public void MoveItem( int index, int indexTarget )
		{
			var inventory = Inventory as Inventory;
			if ( inventory == null ) return;

			inventory.MoveItem( index, indexTarget );
		}

		[ClientRpc]
		public void WithOverlay( string overlay )
		{
			var hud = Local.Hud;
			if ( hud == null ) return;

			var currentOverlay = Overlay.Current;
			if ( currentOverlay != null )
				currentOverlay.Delete();

			hud.AddChild( new Overlay( overlay ) );
		}


	}
}
