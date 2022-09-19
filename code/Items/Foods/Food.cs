using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Sandbox;

namespace ZenWorks.Items.Foods
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class FoodAttribute : ItemAttribute { }

	public abstract partial class Food : Item
	{
		public override bool CanTake => true;
		public override bool CanDrop => true;

		public virtual bool CanEat => true;
		public virtual bool CanDrink => true;

		public virtual float Hunger => 0f;
		public virtual float Thirst => 0f;

		public virtual string EatSound => null;
		public virtual string DrinkSound => null;

		public void Eat( Character character )
		{
			if ( !CanEat || character == null ) return;
			
			if ( character.Hunger + Hunger > 100 ) character.Hunger = 100f;
			else character.Hunger += Hunger;

			if ( !String.IsNullOrEmpty( EatSound ) )
				Sound.FromEntity( EatSound, character );
			
			OnEat(character);
		}

		public virtual void OnEat( Character character )
		{
			
		}

		public void Drink( Character character )
		{
			if ( !CanDrink || character == null ) return;
			
			if ( character.Thirst + Thirst > 100 ) character.Thirst = 100f;
			else character.Thirst += Thirst;
			
			if ( !String.IsNullOrEmpty( DrinkSound ) )
				Sound.FromEntity( DrinkSound, character );

			OnDrink(character);
		}

		public virtual void OnDrink( Character character )
		{
			
		}
	}
}
