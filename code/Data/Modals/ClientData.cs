using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sandbox;

namespace ZenWorks.Data.Modals
{
	public class ClientData
	{
		public string Name { get; set; }
		public long SteamId { get; set; }
		
		public Dictionary<int, CharacterData> Characters { get; set; }
	}
}
