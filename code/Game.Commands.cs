using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Sandbox;
using ZenWorks.Entities;
using ZenWorks.Factions;
using ZenWorks.Items;
using ZenWorks.Items.Foods;
using Http = Sandbox.Internal.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ZenWorks.Data.Modals;

namespace ZenWorks
{
	public partial class Game
	{
		[ConCmd.Server( "zw_char_infos" )]
		public static void ZwCharInfos()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			Log.Info( $"Votre nom : {character?.Name}" );
			Log.Info( $"Votre description : {character?.Description}" );
			Log.Info( $"Votre faction : {character.Faction.Name} (LibraryName : {character.Faction.LibraryName})" );
		}

		[ConCmd.Server( "zw_char_respawn" )]
		public static void ZwCharRespawn()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;
			
			character.Respawn();
		}
		
		[ConCmd.Server( "zw_char_self_sethealth" )]
		public static void ZwCharSelfSetHealth(string health)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			character.Health = health.ToFloat();
		}
		
		[ConCmd.Server( "zw_char_self_setarmor" )]
		public static void ZwCharSelfSetArmor(string armor)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			character.Armor = armor.ToFloat();
		}

				
		[ConCmd.Server( "zw_char_self_sethunger" )]
		public static void ZwCharSelfSetHunger(string hunger)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			character.Hunger = hunger.ToFloat();
		}
		
		[ConCmd.Server( "zw_char_self_setthirst" )]
		public static void ZwCharSelfSetThirst(string thirst)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			character.Thirst = thirst.ToFloat();
		}
		
		[ConCmd.Server( "zw_spawn_item" )]
		public static void ZwSpawnItem( string libraryName )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;
			
			var itemType = TypeLibrary.Create<Item>(Utils.GetTypeByLibraryName<Item>( libraryName ));
			if ( itemType == null ) return;

			var tr = Trace.Ray( character.EyePosition, character.EyePosition + character.EyeRotation.Forward * 200 )
				.UseHitboxes()
				.Ignore( character )
				.Size( 2 )
				.Run();

			if ( itemType is Item item )
			{
				var ent = new ItemEntity(item);
				ent.Position = tr.EndPosition;
				ent.Rotation = Rotation.From( new Angles( 0, character.EyeRotation.Angles().yaw, 0 ) );
			}
		}
		
		[ConCmd.Server( "zw_use_item_entity" )]
		public static void ZwUseItemEntity( string action )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var tr = Trace.Ray( character.EyePosition, character.EyePosition + character.EyeRotation.Forward * 200 )
				.UseHitboxes()
				.Ignore( character )
				.Size( 2 )
				.Run();

			if ( tr.Entity.IsValid() && tr.Entity is ItemEntity ent )
			{
				if ( ent.Item is Food food )
				{
					if ( food.CanDrink && action == "drink" )
					{
						food.Drink( character );
						ent.Delete();
					} 
					else if ( !food.CanDrink && action == "drink" )
						Log.Info( "[ERREUR] L'item de nourriture que vous visez n'est pas buvable !" );
						
					if ( food.CanEat && action == "eat" )
					{
						food.Eat( character );
						ent.Delete();
					}
					else if ( !food.CanEat && action == "eat" )
						Log.Info( "[ERREUR] L'item de nourriture que vous visez n'est pas mangable !" );

				}

				if ( ent.Item.CanTake && action == "take" )
				{
					ent.Item.Take( character );
					ent.Delete();
				}
			}
			else Log.Info( "[ERREUR] L'item que vous visez n'est pas valide !" );
		}

		[ConCmd.Server( "zw_char_show_my_items" )]
		public static void ZwCharShowMyItems()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			Log.Info( $"Inventory : {inventory}" );
			if ( inventory == null ) return;

			if ( inventory.CountItems() <= 0 )
			{
				Log.Info( "Votre inventaire est vide." );
				return;
			}
			
			foreach ( var item in inventory.Items )
				Log.Info( $"[{item.Key}] - {(item.Value != null ? item.Value.DisplayName : item.Value)} {(item.Value != null && item.Value.IsStackable() ? $"({item.Value.Stack} Stack)" : "")}" );
		}

		[ConCmd.Server( "zw_move_item_inventory" )]
		public static void ZwMoveItemInventory( int index, int indexTarget )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;

			character.MoveItem( To.Single( character ), index, indexTarget );
			inventory.MoveItem( index, indexTarget );
		}

		[ConCmd.Server( "zw_use_item_inventory" )]
		public static void ZwUseItemInventory( int indexItem, string action, bool allStack = false )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;

			if ( !inventory.ItemExist( indexItem ) ) return;

			var item = inventory.FindItem( indexItem );

			if ( item == null ) return;
			
			if ( item is Food food )
			{
				if ( food.CanDrink && action == "drink" )
				{
					food.Drink( character );
					character.RemoveItem( To.Single( character ), indexItem );
					inventory.RemoveItem( indexItem );
				}

				if ( food.CanEat && action == "eat" )
				{
					food.Eat( character );
					character.RemoveItem( To.Single( character ), indexItem );
					inventory.RemoveItem( indexItem );
				}
			}

			if ( item.CanDrop && action == "drop" )
			{
				item.Drop( character );
				character.RemoveItem( To.Single( character ), indexItem, allStack );
				inventory.RemoveItem( indexItem, allStack );
			}
		}

		[ConCmd.Server( "zw_split_item_inventory" )]
		public static void ZwSplitItemInventory( int indexItem )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;

			if ( !inventory.ItemExist( indexItem ) ) return;

			character.SplitItem( To.Single( character ), indexItem );
			inventory.SplitItem( indexItem );
		}

		[ConCmd.Server( "zw_give_item" )]
		public static void ZwGiveItem(string libraryName, int amount)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;

			for ( int i = 0; i < amount; i++ )
			{
				var item = Utils.GetObjectByType<Item>( Utils.GetTypeByLibraryName<Item>( libraryName ) );
				if ( item == null ) return;
				
				Log.Info( $"Give {item.DisplayName} - {item.Stack} " );
				character.AddItem( To.Single( character ), libraryName );
				inventory.AddItem( item );
			}
		}

		[ConCmd.Server( "zw_clear_inventory" )]
		public static void ZwClearInventory()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			var inventory = character.Inventory as Inventory;
			if ( inventory == null ) return;
			
			character.ClearInventory(To.Single( character ));
			inventory.ClearItems();
		}

		[ConCmd.Server( "zw_notification" )]
		public static void ZwNotification(string text, float timeBeforeDelete = 10f, string icon = null, string sound = null)
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;

			character.Notify( text, timeBeforeDelete, icon == null ? null : $"/ui/icon/{icon}", sound );
		}

		[ConCmd.Server( "zw_change_faction" )]
		public static void ZwChangeFaction( string libraryName )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			var character = client.Pawn as Character;
			if ( character == null ) return;
			
			character.SetFaction( Faction.GetFaction( $"zw_faction_{libraryName}" ) );
		}

		[ConCmd.Server( "zw_char_create" )]
		public static void ZwCharCreate( string name, string description, string model, string faction )
		{
			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			if ( String.IsNullOrEmpty( name ) || String.IsNullOrWhiteSpace( name ) ) return;
			if ( String.IsNullOrEmpty( description ) || String.IsNullOrWhiteSpace( description ) ) return;
			if ( String.IsNullOrEmpty( model ) || String.IsNullOrWhiteSpace( model ) ) return;
			if ( String.IsNullOrEmpty( faction ) || String.IsNullOrWhiteSpace( faction ) ) return;

			var clientData = DataManager.GetClient( client );
			if ( clientData == null ) return;

			var characters = DataManager.GetCharacters( client );
			if ( characters == null ) return;

			CharacterData data = new CharacterData
			{
				Name = name,
				Description = description,
				Model = model,
				Money = 0f,
				Armor = 0f,
				Hunger = 100f,
				Thirst = 100f,
				Faction = faction,
				OwnerId = client.PlayerId,
				IndexData = characters.Count + 1
			};
			
			var character = new Character( data );
			client.Pawn = character;

			character.Notify( "Zenworks est en cours de développement, des bugs peuvent être présents.", 20f, "warning.png", null );
			
			// Get all of the spawnpoints
			var spawnpoints = Entity.All.OfType<SpawnPoint>();

			// chose a random one
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			// if it exists, place the pawn there
			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				character.Transform = tx;
			}
			
			character.Faction.OnCharacterCreated( character );
			character.Save();
		}

		[ConCmd.Server( "zw_char_load" )]
		public static void ZwCharLoad( int index )
		{
			CharacterData data = new CharacterData();

			var client = ConsoleSystem.Caller;
			if ( client == null ) return;

			if ( client.Pawn != null && client.Pawn is Character currentCharacter )
			{
				if ( currentCharacter.IndexData == index )
				{
					RPCs.ClientNotify( To.Single( client ),
						"Vous utiliser déjà ce personnage !", 10f, "warning.png" );
					return;
				}
				
				currentCharacter.Save();
				currentCharacter.Delete();
				client.Pawn = null;
			}

			var characters = DataManager.GetCharacters( client );
			if ( characters == null )
			{
				RPCs.ClientNotify( To.Single( client ),
					"Un problème est survenue, impossible de récupérer vos personnages !", 10f, "warning.png" );
				return;
			}

			if ( !characters.ContainsKey( index ) )
			{
				RPCs.ClientNotify( To.Single( client ), "Un problème est survenue, le personnage n'a pas été trouvé !",
					10f, "warning.png" );
				return;
			}

			data = characters[index];

			var character = new Character( data );
			client.Pawn = character;

			character.Notify( "Zenworks est en cours de développement, des bugs peuvent être présents.", 20f,
				"warning.png", null );

			// Get all of the spawnpoints
			var spawnpoints = Entity.All.OfType<SpawnPoint>();

			// chose a random one
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			// if it exists, place the pawn there
			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				character.Transform = tx;
			}
		}

		/*[ConCmd.Server( "zw_http_get" )]
		public async static void ZwHttpGet( string url )
		{
			if ( String.IsNullOrEmpty( url ) || String.IsNullOrWhiteSpace( url ) ) return;
			
			var http = new Http(new Uri( url ));
			Log.Info( "Chargement en cours, veuillez patienter..." );
			var json = await http.GetStringAsync();
			
			Log.Info( json );
		}*/
	}
}
