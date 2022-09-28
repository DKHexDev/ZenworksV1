using System.Collections.Generic;
using System.Text.Json;
using Sandbox;
using ZenWorks.Data.Modals;

namespace ZenWorks
{
	public static class DataManager
	{
		public static ClientData GetClient( Client client )
		{
			if ( !FileSystem.Data.DirectoryExists( "players" ) )
				return null;
			
			if ( !FileSystem.Data.FileExists( $"/players/{client.PlayerId}.json" ) )
				return null;

			return FileSystem.Data.CreateSubSystem( "/players/" ).ReadJson<ClientData>( $"{client.PlayerId}.json" );
		}

		public static void SaveClient( Client client )
		{
			if ( !FileSystem.Data.DirectoryExists( "players" ) )
				FileSystem.Data.CreateDirectory( "players" );

			var currentClientSave = GetClient( client );
			
			if ( currentClientSave == null )
			{
				FileSystem.Data.CreateSubSystem( "/players/" ).WriteJson( $"{client.PlayerId}.json", new ClientData()
				{
					Name = client.Name,
					SteamId = client.PlayerId,
					Characters = new Dictionary<int, CharacterData>()
				} );
				return;
			}

			currentClientSave.Name = client.Name;
			currentClientSave.SteamId = client.PlayerId;
			
			FileSystem.Data.CreateSubSystem( "/players/" ).WriteJson( $"{client.PlayerId}.json", currentClientSave );
		}
		
		public static Dictionary<int, CharacterData> GetCharacters( Client client )
		{
			if ( !FileSystem.Data.DirectoryExists( "players" ) )
				return null;

			if ( !FileSystem.Data.FileExists( $"/players/{client.PlayerId}.json" ) )
				return null;

			var characters = FileSystem.Data.CreateSubSystem( "/players/" ).ReadJson<ClientData>( $"{client.PlayerId}.json" ).Characters;

			foreach ( var data in characters )
				data.Value.IndexData = data.Key;
			
			return characters;
		}

		public static void SaveCharacter( Client client, CharacterData data )
		{
			if ( !FileSystem.Data.DirectoryExists( "players" ) )
				FileSystem.Data.CreateDirectory( "players" );
			
			if ( !FileSystem.Data.FileExists( $"/players/{client.PlayerId}.json" ) )
				return;

			var currentData = GetClient( client );
			if ( currentData == null ) return;

			if ( currentData.Characters.ContainsKey( data.IndexData ) ) currentData.Characters[data.IndexData] = data;
			else currentData.Characters.Add( currentData.Characters.Count + 1, data );
			
			FileSystem.Data.CreateSubSystem( "/players/" ).WriteJson( $"{client.PlayerId}.json", currentData );
			
			client.SetValue( "Characters", JsonSerializer.Serialize( GetCharacters( client ) ) );
		}	
	}
}
