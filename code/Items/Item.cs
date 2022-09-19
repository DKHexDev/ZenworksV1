using System;
using Sandbox;
using ZenWorks.Entities;

namespace ZenWorks.Items
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
	public class ItemAttribute : Attribute
	{
		public ItemAttribute() { }
	}
	
	public abstract class Item : BaseNetworkable, IItem
	{
		public string LibraryName { get; protected set; }
		public Entity Owner { get; protected set; }
		public Entity LastOwner { get; protected set; }
		
		public virtual string DisplayName { get; protected set; }
		public virtual string Description { get; protected set; }
		public virtual string Model { get; protected set; }

		public virtual string TakeSound { get; protected set; } = null;
		public virtual string DropSound { get; protected set; } = null;

		public Item()
		{
			LibraryName = Utils.GetLibraryName( GetType() );
		}

		public virtual bool CanTake { get; protected set; } = true;
		public virtual bool CanDrop { get; protected set; } = true;
		public virtual int Stackable { get; protected set; } = 1;

		public virtual int Stack { get; private set; } = 1;
		
		public void Take( Character character )
		{
			if ( !CanTake ) return;
			
			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;
			
			character.AddItem( To.Single( character ), LibraryName, Stack );
			inventory.AddItem( this );

			if ( !String.IsNullOrEmpty( TakeSound ) )
				Sound.FromEntity( TakeSound, character );

			OnTake(character);
		}

		public virtual void OnTake( Character character )
		{
			Owner = character;
		}

		public void Drop( Character character )
		{
			if ( !CanDrop ) return;
			
			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;
			
			if ( !inventory.ItemExist( this ) ) return;

			var tr = Trace.Ray( character.EyePosition, character.EyePosition + character.EyeRotation.Forward * 200 )
				.UseHitboxes()
				.Ignore( character )
				.Size( 2 )
				.Run();
			
			var ent = new ItemEntity(this);
			ent.Position = tr.EndPosition;
			ent.Rotation = Rotation.From( new Angles( 0, character.EyeRotation.Angles().yaw, 0 ) );

			if ( !String.IsNullOrEmpty( DropSound ) )
				Sound.FromEntity( DropSound, character );

			OnDrop(character);
		}

		public virtual void OnDrop(Character character)
		{
			LastOwner = Owner;
			Owner = null;
		}

		public void Remove(Character character)
		{
			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;
			
			character.RemoveItem( To.Single( character ), LibraryName );
			inventory.RemoveItem( this );

			OnRemove();
		}
		
		public bool IsStackable()
		{
			return Stackable > 1;
		}

		public void AddStack( int amount )
		{
			if ( Stackable <= 1 ) return;
			if ( Stack + amount > Stackable ) return;
			Stack += amount;
		}

		public void SetStack( int amount )
		{
			if ( Stackable <= 1 ) return;
			Stack = amount;
		}

		public void RemoveStack( int amount )
		{
			if ( Stack <= 1 ) return;
			Stack -= amount;
		}

		public virtual void OnRemove()
		{
				
		}
		
		public void Delete()
		{
			
		}

		public virtual void Simulate( Client owner )
		{
			
		}
	}
}
