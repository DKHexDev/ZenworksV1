namespace ZenWorks.Abilities;

public interface IAbility
{
	string Name { get; set; }
	string Description { get; set; }
	
	Color Color { get; set; }
	
	int currentPoints { get; set; }
	int maxPoints { get; set; }
}
