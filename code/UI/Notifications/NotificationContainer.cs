using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;

namespace ZenWorks.UI.Notifications
{
	public partial class NotificationContainer : Panel
	{
		public static NotificationContainer Current { get; private set; }

		private TimeSince TimeWaitingCheck;
		private List<Notification> Notifications { get; init; }
		private List<Notification> NotificationsWaiting { get; init; }

		public NotificationContainer()
		{
			StyleSheet.Load( "/UI/Notifications/Notifications.scss" );
			Notifications = new List<Notification>();
			NotificationsWaiting = new List<Notification>();
			TimeWaitingCheck = 0f;
			Current = this;
		}

		public void Notify( string text, float timeBeforeDelete = 10f, string icon = null, string sound = null )
		{
			Notification notification = new Notification( this, text, timeBeforeDelete, icon, sound );

			if ( Notifications.Count >= 5 )
			{
				NotificationsWaiting.Add( notification );
				return;
			}

			Notifications.Add( notification );
			AddChild( notification );
		}

		public override void OnDeleted()
		{
			base.OnDeleted();
			Current = null;
		}

		public override void Tick()
		{
			base.Tick();

			if ( TimeWaitingCheck < 5f ) return;
			if ( Notifications.Count >= 5 ) return;
			if ( NotificationsWaiting.Count < 1 ) return;

			for ( int i = Notifications.Count; i < 6; i++ )
			{
				Notifications.Add( NotificationsWaiting.First() );
				AddChild( NotificationsWaiting.First() );
				Notifications[0] = null;
			}
		}
	}
}
