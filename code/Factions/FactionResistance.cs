using System.Collections.Generic;
using Sandbox;

namespace ZenWorks.Factions
{
	[Faction("resistance")]
	public class FactionResistance : Faction
	{
		public override string Name => "Résistance";
		
		public override string Description => "Vous êtes de la rebellion, contre l'existence des combines sur la planète.";
		
		public override Color Color => Color.Parse( "#825e5c" ).GetValueOrDefault("#FFFFFF");
		
		public override List<string> Models => new() { "models/citizen/citizen.vmdl" };
		
		public override string Image => "/ui/factions/resistance.jpg";
	}
}
