using System.Collections.Generic;
using Sandbox;

namespace ZenWorks.Factions
{
	[Faction("ota")]
	public class FactionOta : Faction
	{
		public override string Name => "Overwatch Trans Humans";
		
		public override string Description => "Vous êtes OTA, votre but est de répondre au directive de l'overwatch.";
		
		public override Color Color => Color.Parse( "#714d69" ).GetValueOrDefault("#FFFFFF");
		
		public override List<string> Models => new() { "models/citizen/citizen.vmdl" };
		
		public override string Image => "/ui/factions/ota.jpg";
	}
}
