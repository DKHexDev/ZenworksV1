using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ZenWorks.UI.Components.Buttons
{
	public class ButtonImage : Button
	{	
		
		public ButtonImage( string text, Action onClick = null, string image = null, bool isAnimated = true ) : base(
			text, null, onClick )
		{
			StyleSheet.Load( "/UI/Components/Buttons/ButtonImage.scss" );

			if ( image != null )
			{
				Style.BackgroundImage = Texture.Load( FileSystem.Mounted, image );
				AddClass( "HasImage" );
			}

			if ( isAnimated )
				AddClass( "isAnimated" );
		}
	}
}
