using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class MobileNotificationProxy : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000557 RID: 1367 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
	public MobileNotificationsManager NotificationsManager
	{
		get
		{
			return this._notificationsManager;
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0002C5EC File Offset: 0x0002A7EC
	private IEnumerator Start()
	{
		yield return null;
		this._notificationsManager = UnityEngine.Object.FindObjectOfType<MobileNotificationsManager>();
		yield break;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0002C608 File Offset: 0x0002A808
	public void SendTestNotification(int seconds)
	{
		NotificationData notificationData = new NotificationData
		{
			NotificationType = MobileNotificationType.Debug,
			FireTime = DateTime.Now.AddSeconds((double)seconds),
			Title = "Test notification: " + seconds,
			Text = "Test message arriving: " + DateTime.Now.AddSeconds((double)seconds),
			LargeIcon = "icon_offline"
		};
		Debug.LogWarning(string.Concat(new object[]
		{
			"Test notification should arrive: ",
			notificationData.FireTime,
			" From: ",
			this._notificationsManager.name
		}));
		this._notificationsManager.AddNotification(notificationData);
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
	public void CancelTestNotification()
	{
		Debug.LogWarning("Cancelling test notification.");
		this._notificationsManager.CancelNotification(MobileNotificationType.Debug);
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002C6E4 File Offset: 0x0002A8E4
	public void ScheduleNotifications()
	{
		this._notificationsManager.AddPhoneFlingNotifications();
		this._notificationsManager.Test_PrintIDs();
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0002C6FC File Offset: 0x0002A8FC
	public void PrintIDs()
	{
		this._notificationsManager.Test_PrintIDs();
	}

	// Token: 0x04000554 RID: 1364
	private MobileNotificationsManager _notificationsManager;
}
