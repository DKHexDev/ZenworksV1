using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZenWorks.Data.Modals
{
	public class CharacterData
	{
		[JsonIgnore] 
		public int IndexData { get; set; }
		
		public string Name { get; set; }
		
		public string Description { get; set; }
		
		public string Model { get; set; }
		
		public float Money { get; set; }
		
		public float Armor { get; set; }
		
		public float Hunger { get; set; }
		
		public float Thirst { get; set; }
		
		public string Faction { get; set; }
		
		public long OwnerId { get; set; }
	}
}
