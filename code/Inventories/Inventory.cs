using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sandbox;
using ZenWorks.Items;
using ZenWorks.Items.Foods;

namespace ZenWorks
{
	public partial class Inventory : BaseNetworkable, IBaseInventory
	{
		public Dictionary<int, Item> Items { get; private set; }
		public Vector2 Size { get; private set; }
		
		public Inventory()
		{
			Size = new Vector2( 6, 6 );
			Items = new Dictionary<int, Item>();
			
			for ( int i = 0; i < Size.x * Size.y; i++ )
				Items.Add( i, null );
		}

		public int CountItems()
		{
			return Items.Count( x => x.Value != null );
		}
		
		public bool CanAddItem()
		{
			return Items.Count( x => x.Value != null ) < Size.x * Size.y;
		}

		public bool ItemExist( Item item )
		{
			return Items.Count( x => x.Value != null && x.Value.Equals( item ) ) > 0;
		}

		public bool ItemExist( string libraryName )
		{
			return Items.Count( x => x.Value != null && x.Value.LibraryName == libraryName ) > 0;
		}

		public bool ItemExist( int index )
		{
			if ( !Items.ContainsKey( index ) )
				return false;

			if ( Items[index] == null )
				return false;

			return true;
		}

		public Item FindItem( Item item )
		{
			return Items.Where( x => x.Value != null && x.Value.LibraryName == item.LibraryName )
				.Select( x => x.Value )
				.First();
		}

		public Item FindItem( string libraryName )
		{
			return Items.Where( x => x.Value != null && x.Value.LibraryName == libraryName )
				.Select( x => x.Value )
				.First();
		}

		public Item FindItem( int index )
		{
			if ( !Items.ContainsKey( index ) )
				return null;
			
			return Items[index];
		}

		public void ClearItems()
		{
			for ( int i = 0; i < Size.y * Size.x; i++ )
				Items[i] = null;
				
			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}

		public void AddItem( Item item )
		{
			var stackableItem = Items
				.Where( x => x.Value != null && x.Value.LibraryName == item.LibraryName && x.Value.IsStackable() && x.Value.Stack < x.Value.Stackable )
				.Select( x => (KeyValuePair<int, Item>?)x )
				.FirstOrDefault();

			
			if ( stackableItem != null && stackableItem?.Value != null )
			{
				/* TODO - Finir l'ajout d'un même item stackable dans un item stackable disponible */
			}
			
			if ( !CanAddItem() ) return;
			if ( !Items.ContainsValue( null ) ) return;
			
			var keyEmpty = Items.Where(x => x.Value == null)
				.Select(x => x.Key)
				.FirstOrDefault();
			
			Items[keyEmpty] = item;

			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}

		public void SplitItem( int index )
		{
			if ( !CanAddItem() ) return;
			if ( !Items.ContainsKey( index ) ) return;
			if ( !Items.ContainsValue( null ) ) return;
			if ( !ItemExist( index ) ) return;
			if ( !Items[index].IsStackable() ) return;
			if ( Items[index].Stack <= 1 ) return;
			
			var keyEmpty = Items.Where(x => x.Value == null)
				.Select(x => x.Key)
				.FirstOrDefault();
			
			Items[keyEmpty] = Utils.GetObjectByType<Item>( Utils.GetTypeByLibraryName<Item>( Items[index].LibraryName ) );
			Items[index].RemoveStack( 1 );
			
			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}

		public void RemoveItem( Item item )
		{
			if ( !Items.ContainsValue( item ) ) return;

			var keyItem = Items.Where(x => x.Value.LibraryName == item.LibraryName )
				.Select(x => x.Key)
				.First();
			
			Items[keyItem] = null;
			
			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}

		public void RemoveItem( int index, bool allStack = false )
		{
			if ( !Items.ContainsKey( index ) ) return;
			if ( !ItemExist( index ) ) return;

			if ( Items[index].IsStackable() && Items[index].Stack > 1 && !allStack ) Items[index].RemoveStack( 1 );
			else Items[index] = null;

			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}

		public void MoveItem( int index, int indexTarget )
		{
			if ( !Items.ContainsKey( index ) || !Items.ContainsKey( indexTarget ) ) return;

			if (
				Items[index] != null && Items[indexTarget] != null &&
				Items[index].LibraryName == Items[indexTarget].LibraryName &&
				Items[index].IsStackable() &&
				Items[indexTarget].IsStackable() &&
				Items[indexTarget].Stack < Items[indexTarget].Stackable
			)
			{
				var diff = Items[indexTarget].Stackable - (Items[index].Stack + Items[indexTarget].Stack);
				
				switch (diff)
				{
					case < 0:
						Items[indexTarget].AddStack( Items[index].Stack - (-diff) );
						Items[index].SetStack( (-diff)  );
						break;
					case >= 0:
						Items[indexTarget].AddStack( Items[index].Stack );
						Items[index] = null;
						break;
				}
			}
			else
			{
				var item = Items[index];
				var targetItem = Items[indexTarget];

				Items[indexTarget] = item;
				Items[index] = targetItem;
			}
			
			if ( Host.IsClient )
				Event.Run( "zw_main_inventory.refresh" );
		}


		public void OnChildAdded( Entity child )
		{
			throw new System.NotImplementedException();
		}

		public void OnChildRemoved( Entity child )
		{
			throw new System.NotImplementedException();
		}

		public void DeleteContents()
		{
			throw new System.NotImplementedException();
		}

		public int Count()
		{
			throw new System.NotImplementedException();
		}

		public Entity GetSlot( int i )
		{
			throw new System.NotImplementedException();
		}

		public int GetActiveSlot()
		{
			throw new System.NotImplementedException();
		}

		public bool SetActiveSlot( int i, bool allowempty )
		{
			throw new System.NotImplementedException();
		}

		public bool SwitchActiveSlot( int idelta, bool loop )
		{
			throw new System.NotImplementedException();
		}

		public Entity DropActive()
		{
			throw new System.NotImplementedException();
		}

		public bool Drop( Entity ent )
		{
			throw new System.NotImplementedException();
		}

		public Entity Active { get; }
		public bool SetActive( Entity ent )
		{
			throw new System.NotImplementedException();
		}

		public bool Add( Entity ent, bool makeactive = false )
		{
			throw new System.NotImplementedException();
		}

		public bool Contains( Entity ent )
		{
			throw new System.NotImplementedException();
		}
	}
}
