using Sandbox;

namespace ZenWorks.Items.Foods
{
	[Library("zw_item_food_watermelon")]
	[Food]
	public partial class WaterMelon : Food
	{
		public override string DisplayName => "Melon";
		public override string Description => "Un melon de couleur vert.";
		public override string Model => "models/sbox_props/watermelon/watermelon.vmdl";

		public override string EatSound => "eating.cracker";

		public override bool CanDrink => false;
		public override bool CanEat => true;

		public override float Hunger => 50f;

		public override int Stackable { get; protected set; } = 5; 

		public override void OnTake( Character character )
		{
			base.OnTake(character);
		}
		
		public override void OnEat( Character character )
		{
			base.OnEat(character);
		}
	}
}
