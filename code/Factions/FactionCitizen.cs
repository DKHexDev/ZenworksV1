using System.Collections.Generic;
using Sandbox;

namespace ZenWorks.Factions
{
	[Faction("citizen")]
	public class FactionCitizen : Faction
	{
		public override string Name => "Citoyen";
		
		public override string Description => "La faction par défaut du Half Life 2 Roleplay";
		
		public override Color Color => Color.Parse( "#284387" ).GetValueOrDefault("#FFFFFF");
		
		public override List<string> Models => new() { "models/citizen/citizen.vmdl" };
		
		public override string Image => "/ui/factions/citizen.jpg";
	}
}
