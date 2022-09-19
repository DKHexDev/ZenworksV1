using System.Collections.Generic;
using Sandbox;

namespace ZenWorks.Factions
{
	public interface IFaction
	{
		string Name { get; set; }
		string Description { get; set; }
		float Salary { get; set; }
		Color Color { get; set; }
		List<string> Models { get; set; }
		string FootstepSound { get; set; }
		string FootstepRunSound { get; set; }
		
		string Image { get; set; }
		
		string Overlay { get; set; }
			
		void OnCharacterCreated( Character character );
		void OnSpawn( Character character );
		void OnTransferred( Character character );
	}
}
