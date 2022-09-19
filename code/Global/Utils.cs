using System;
using System.Collections.Generic;
using System.Linq;

using Sandbox;
using Sandbox.UI;

namespace ZenWorks.Global
{
	public static partial class Utils
	{
		/// <summary>
		/// Get all clients in game
		/// </summary>
		/// <returns></returns>
		public static List<Client> GetClients()
		{
			List<Client> clients = new List<Client>();

			foreach ( Client client in Client.All )
				clients.Add( client );

			return clients;
		}

		/// <summary>
		/// Get all characters in game
		/// </summary>
		/// <returns></returns>
		public static List<Character> GetCharacters()
		{
			List<Character> characters = new List<Character>();

			foreach ( Client client in GetClients() )
			{
				if ( client.Pawn != null && client.Pawn.IsValid() )
					characters.Add( client.Pawn as Character );
			}

			return characters;
		}

		/// <summary>
		/// Get client with PlayerId
		/// </summary>
		/// <param name="playerId">Client PlayerId</param>
		/// <returns></returns>
		public static Client GetClientById(long playerId)
		{
			Client client = Client.All.FirstOrDefault((cl) => cl.PlayerId == playerId, null);

			if ( client == null )
				return null;

			return client;
		}
		
		/// <summary>
		/// Returns an instance of the given type by the given type `Type`.
		/// </summary>
		/// <param name="type">A derived `Type` of the given type</param>
		/// <returns>Instance of the given type object</returns>
		public static T GetObjectByType<T>(Type type) => TypeLibrary.Create<T>(type);

		/// <summary>
		/// Returns the `Sandbox.LibraryAttribute`'s `Name` of the given `Type`.
		/// </summary>
		/// <param name="type">A `Type` that has a `Sandbox.LibraryAttribute`</param>
		/// <returns>`Sandbox.LibraryAttribute`'s `Name`</returns>
		public static string GetLibraryName(Type type) => TypeLibrary.GetDescription(type).ClassName.ToLower();
		
		public static Type GetTypeByLibraryName<T>(string name)
		{
			name = name.ToLower();

			foreach (Type type in GetTypes<T>())
			{
				if (GetLibraryName(type).Equals(name))
				{
					return type;
				}
			}

			return null;
		}
		
		public static List<Type> GetTypes<T>() => GetTypes<T>(null);
		
		public static List<Type> GetTypes<T>(Func<Type, bool> predicate)
		{
			IEnumerable<Type> types = TypeLibrary.GetTypes<T>().Where(t => !t.IsAbstract && !t.ContainsGenericParameters);

			if (predicate != null)
			{
				types = types.Where(predicate);
			}

			return types.ToList();
		}

		public static T GetAttribute<T>(Type type) where T : Attribute => TypeLibrary.GetAttribute<T>(type);
		
		public static bool HasAttribute<T>(Type type, bool inherit = false) where T : Attribute => type.IsDefined(typeof(T), inherit);

		public static void PositionAtCrosshair( this Panel panel )
		{
			panel.PositionAtCrosshair( Local.Pawn as Character );
		}

		public static void PositionAtCrosshair( this Panel panel, Character character )
		{
			if ( !character.IsValid() ) return;

			var eyePos = character.EyePosition;
			var eyeRot = character.EyeRotation;

			var tr = Trace.Ray( eyePos, eyePos + eyeRot.Forward * 1000 )
				.Size( 1.0f )
				.Ignore( character )
				.UseHitboxes()
				.Run();

			panel.PositionAtWorld( tr.EndPosition );
		}

		public static void PositionAtWorld( this Panel panel, Vector3 position )
		{
			var screenPos = position.ToScreen();

			if ( screenPos.z < 0 )
				return;

			panel.Style.Left = Length.Fraction( screenPos.x );
			panel.Style.Top = Length.Fraction( screenPos.y );
			panel.Style.Dirty();
		}
	}
}
