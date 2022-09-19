using System;
using Sandbox;
using Sandbox.UI;

namespace ZenWorks.UI.Components
{
	public class Overlay : Panel
	{
		public static Overlay Current { get; private set; }
		
		public Overlay(string overlay)
		{
			Current = this;
			StyleSheet.Load( $"/UI/Components/Overlay.scss" );
			Style.BackgroundImage = Texture.Load( FileSystem.Mounted, $"/ui/overlay/{overlay}", true );
			
			Log.Info( Style.BackgroundImage );
		}

		public void SetOverlay( string overlay )
		{
			if ( String.IsNullOrEmpty( overlay ) ) return;
			Style.BackgroundImage = Texture.Load( FileSystem.Mounted, $"/ui/overlay/{overlay}", true );
		}

		public override void OnDeleted()
		{
			base.OnDeleted();
			Current = null;
		}
	}
}
