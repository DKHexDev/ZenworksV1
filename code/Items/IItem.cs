using System;
using Sandbox;

namespace ZenWorks.Items
{ 
	public interface IItem
	{
		static string Tag => "zw_item";
		
		string LibraryName { get; }
		
		Entity Owner { get; }

		string Description { get; }
		
		string TakeSound { get; }
		
		string DropSound { get; }

		bool CanTake { get; }
		
		bool CanDrop { get; }
		
		int Stackable { get; }
		
		int Stack { get; }

		void Take( Character character );
		
		void OnTake( Character character );

		void Drop( Character character );

		void OnDrop( Character character );
		
		void Remove( Character character );
		
		void OnRemove();

		void Delete();

		void Simulate( Client owner );
	}
}


