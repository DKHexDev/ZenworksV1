using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ZenWorks.UI.Notifications
{
	public class Notification : Panel
	{
		private TimeSince TimeSinceCreated, TimeSinceProgress;
		private float TimeBeforeDelete { get; init; } = 0f;
		private Label Message { get; init; }
		private Image Icon { get; init; }
		private Panel ProgressTime { get; init; }
		
		public Notification(NotificationContainer parent, string text, float timeBeforeDelete = 10f, string icon = null, string sound = null )
		{
			var containerIcon = Add.Panel( "ContainerIcon" );
			var containerMessage = Add.Panel( "ContainerMessage" );

			Parent = parent;
			TimeSinceCreated = 0f;
			Message = containerMessage.Add.Label( text, "Message" );
			TimeBeforeDelete = timeBeforeDelete;
			Icon = icon == null ? null : containerIcon.Add.Image( $"/ui/icon/{icon}", "Icon" );

			ProgressTime = Add.Panel( "Progress" );
			ProgressTime.Style.Width = Length.Percent( 100f );
			
			if ( Icon == null )
				containerIcon.Style.Display = DisplayMode.None;
			
			if ( sound != null )
				Sound.FromScreen( sound );
		}

		public override void Tick()
		{
			base.Tick();
			
			if ( TimeSinceCreated > TimeBeforeDelete )
			{
				TimeSinceCreated = 0f;
				Delete();
			}

			if ( TimeSinceProgress > 1.0f )
			{
				ProgressTime.Style.Dirty();
				ProgressTime.Style.Width = Length.Percent( ProgressTime.Style.Width.GetValueOrDefault().Value - (100f / TimeBeforeDelete));

				TimeSinceProgress = 0f;
			}
		}
	}
}
