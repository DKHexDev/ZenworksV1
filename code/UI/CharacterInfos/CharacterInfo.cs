using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ZenWorks.CharacterInfos
{
	public partial class CharacterInfo : Panel
	{
		public Panel Container, HealthBar, ArmorBar, StaminaBar, HungerBar, ThirstBar;
		public static CharacterInfo Current { get; private set; }

		public CharacterInfo()
		{
			Current = this;
			
			Container = Add.Panel( "Container" );
			
			/* Health */
			var containerHealthBar = Container.Add.Panel("BarContainer");
			HealthBar = containerHealthBar.Add.Panel("Bar");
			HealthBar.Style.BackgroundColor = Color.Parse( "#cf000f" ).GetValueOrDefault( "#FFFFFF" );
			
			/* Armor */
			var containerArmorBar = Container.Add.Panel( "BarContainer" );
			ArmorBar = containerArmorBar.Add.Panel( "Bar" );
			ArmorBar.Style.BackgroundColor = Color.Parse( "#1e8bc3" ).GetValueOrDefault( "#FFFFFF" );			
			
			/* Stamina */
			var containerStaminaBar = Container.Add.Panel( "BarContainer" );
			StaminaBar = containerStaminaBar.Add.Panel( "Bar" );
			StaminaBar.Style.BackgroundColor = Color.Parse( "#26a65b" ).GetValueOrDefault( "#FFFFFF" );			
			
			/* Hunger */
			var containerHungerBar = Container.Add.Panel( "BarContainer" );
			HungerBar = containerHungerBar.Add.Panel( "Bar" );
			HungerBar.Style.BackgroundColor = Color.Parse( "#e67e22" ).GetValueOrDefault( "#FFFFFF" ); 
			
			/* Thirst */
			var containerThirstBar = Container.Add.Panel( "BarContainer" );
			ThirstBar = containerThirstBar.Add.Panel( "Bar" );
			ThirstBar.Style.BackgroundColor = Color.Parse( "#4193a9" ).GetValueOrDefault( "#FFFFFF" );
		}

		public override void Tick()
		{
			base.Tick();
			
			var client = Local.Client;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			HealthBar.Style.Dirty();
			HealthBar.Style.Width = Length.Percent( character.Health );
			
			ArmorBar.Style.Dirty();
			ArmorBar.Style.Width = Length.Percent( character.Armor );
			
			StaminaBar.Style.Dirty();
			StaminaBar.Style.Width = Length.Percent( character.Stamina );	
			
			HungerBar.Style.Dirty();
			HungerBar.Style.Width = Length.Percent( character.Hunger );	
			
			ThirstBar.Style.Dirty();
			ThirstBar.Style.Width = Length.Percent( character.Thirst );
		}
	}
}
