using System;
using System.Collections.Generic;
using Sandbox;
using ZenWorks.Data.Modals;

namespace ZenWorks
{
	public partial class Character
	{
		public int IndexData { get; private set; } = -1;
		
		public void Save()
		{
			CharacterData data = GetCharacterData();
			if ( data == null ) return;
			DataManager.SaveCharacter( Client, data );
		}

		private CharacterData GetCharacterData()
		{
			return new CharacterData()
			{
				Armor = Armor,
				Description = Description,
				Faction = Faction.LibraryName,
				Hunger = Hunger,
				Money = Money,
				Thirst = Thirst,
				Name = Fullname,
				Model = Model.Name,
				OwnerId = Client.PlayerId,
				IndexData = IndexData
			};
		}
	}
}
