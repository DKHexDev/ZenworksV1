using Sandbox;

namespace ZenWorks
{
	public partial class Character
	{
		public bool IsUseDisabled()
		{
			return ActiveChild is IUse use && use.IsUsable( this );
		}

		protected override Entity FindUsable()
		{
			if ( IsUseDisabled() )
				return null;

			// First try a direct 0 width line
			var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (85 * Scale) )
				.WithTag( "Usable" )
				.Ignore( this )
				.Run();

			// Nothing found, try a wider search
			if ( !IsValidUseEntity( tr.Entity ) )
			{
				tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (85 * Scale) )
					.Radius( 2 )
					.WithTag( "Usable" )
					.Ignore( this )
					.Run();
			}

			// Still no good? Bail.
			if ( !IsValidUseEntity( tr.Entity ) ) return null;

			return tr.Entity;
		}

		protected override void UseFail()
		{
			if ( IsUseDisabled() )
				return;

			base.UseFail();
		}
	}
}
