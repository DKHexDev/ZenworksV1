using System.Collections.Generic;
using Sandbox;

namespace ZenWorks.Factions
{
	[Faction("mpf")]
	public class FactionMpf : Faction
	{
		public override string Name => "Protection Civile";
		public override string Description => "Faction de la Protection Civile";
		public override Color Color => Color.Parse( "#1e517b" ).GetValueOrDefault("#FFFFFF");
		public override List<string> Models => new() { "models/citizen/citizen.vmdl" };
		public override string Image => "/ui/factions/mpf.png";
	}
}
