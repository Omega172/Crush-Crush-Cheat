using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public abstract class MobileNotificationsManager : MonoBehaviour
{
	// Token: 0x06000561 RID: 1377 RVA: 0x0002C7A8 File Offset: 0x0002A9A8
	private void Start()
	{
		this.CancelAllNotifications();
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
	public virtual void Init()
	{
		this.HandleLegacyPrefs();
		this.ShowPopup();
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0002C7C0 File Offset: 0x0002A9C0
	protected virtual bool ShouldShowPopup()
	{
		return global::PlayerPrefs.GetInt("DO-NOT-BACKUPMNM_SEEN_PP", 0) == 0;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002C7D0 File Offset: 0x0002A9D0
	private void ShowPopup()
	{
		if (!this.ShouldShowPopup())
		{
			return;
		}
		GameObject gameObject = GameState.CurrentState.transform.Find("Popups/Notifications Popup").gameObject;
		gameObject.SetActive(true);
		global::PlayerPrefs.SetInt("DO-NOT-BACKUPMNM_SEEN_PP", 1);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002C820 File Offset: 0x0002AA20
	protected bool HasPermission()
	{
		return !Settings.NotificationsDisabled && this.HasNativePermission();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0002C838 File Offset: 0x0002AA38
	protected virtual bool HasNativePermission()
	{
		return true;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0002C83C File Offset: 0x0002AA3C
	public virtual void AskForPermission(Action<bool> callback)
	{
		callback(true);
		this.SaveLocalPermissionState(true);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0002C84C File Offset: 0x0002AA4C
	public void SaveLocalPermissionState(bool newPermissionState)
	{
		global::PlayerPrefs.SetInt("SettingsNotifications", (!newPermissionState) ? 0 : 1);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0002C870 File Offset: 0x0002AA70
	public virtual void AddNotification(NotificationData notificationData)
	{
		if (notificationData == null || !this.HasPermission())
		{
			return;
		}
		notificationData.FireTime = this.AdjustDateTime(notificationData.FireTime);
		if (DateTime.Compare(DateTime.Now, notificationData.FireTime) >= 0)
		{
			return;
		}
		try
		{
			this.AddNativeNotification(notificationData);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception when posting notifications: " + ex.Message);
			Debug.LogError("Stack Trace: " + ex.StackTrace);
		}
	}

	// Token: 0x0600056A RID: 1386
	protected abstract void AddNativeNotification(NotificationData notificationData);

	// Token: 0x0600056B RID: 1387 RVA: 0x0002C910 File Offset: 0x0002AB10
	public virtual void CancelNotification(MobileNotificationType notificationType)
	{
		string notificationKey = this.GetNotificationKey(notificationType);
		if (this.IsNotificationListType(notificationType))
		{
			List<int> list = this.LoadListIDs(notificationType);
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (int id in list)
			{
				this.CancelNativeNotification(id);
			}
		}
		else
		{
			this.CancelNativeNotification(this.LoadID(notificationType));
		}
		global::PlayerPrefs.DeleteKey(notificationKey, false);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002C9B8 File Offset: 0x0002ABB8
	public void CancelAllNotifications()
	{
		this.CancelNotification(MobileNotificationType.UserAway);
		this.CancelAllScheduledNotifications();
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0002C9C8 File Offset: 0x0002ABC8
	public virtual void CancelAllScheduledNotifications()
	{
	}

	// Token: 0x0600056E RID: 1390
	protected abstract void CancelNativeNotification(int id);

	// Token: 0x0600056F RID: 1391 RVA: 0x0002C9CC File Offset: 0x0002ABCC
	private string GetNotificationKey(MobileNotificationType notificationType)
	{
		return "DO-NOT-BACKUP_MNM_ID_TYPE_" + notificationType.ToString();
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
	protected void SaveID(MobileNotificationType notificationType, int id)
	{
		if (id < 0)
		{
			return;
		}
		if (this.IsNotificationListType(notificationType))
		{
			this.SaveIDToList(notificationType, id);
		}
		else
		{
			this.SaveSingleID(notificationType, id);
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0002CA10 File Offset: 0x0002AC10
	private void SaveSingleID(MobileNotificationType notificationType, int id)
	{
		if (this.IsNotificationListType(notificationType))
		{
			Debug.LogWarning("Trying to save a list type into non-list one is not allowed.");
			return;
		}
		global::PlayerPrefs.SetInt(this.GetNotificationKey(notificationType), id);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0002CA4C File Offset: 0x0002AC4C
	private void SaveIDToList(MobileNotificationType notificationType, int id)
	{
		if (!this.IsNotificationListType(notificationType))
		{
			Debug.LogWarning("Trying to save a non list type into one is not allowed.");
			return;
		}
		string text = global::PlayerPrefs.GetString(this.GetNotificationKey(notificationType), string.Empty);
		text = ((!(text == string.Empty)) ? (text + "`" + id) : id.ToString());
		global::PlayerPrefs.SetString(this.GetNotificationKey(notificationType), text);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
	protected int LoadID(MobileNotificationType notificationType)
	{
		if (this.IsNotificationListType(notificationType))
		{
			Debug.LogWarning("Trying to load a list type as an item is not allowed.");
			return -1;
		}
		return global::PlayerPrefs.GetInt(this.GetNotificationKey(notificationType), -1);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0002CAFC File Offset: 0x0002ACFC
	protected List<int> LoadListIDs(MobileNotificationType notificationType)
	{
		if (!this.IsNotificationListType(notificationType))
		{
			Debug.LogWarning("Trying to load a non list type as list is allowed.");
			return null;
		}
		string @string = global::PlayerPrefs.GetString(this.GetNotificationKey(notificationType), string.Empty);
		List<int> list = new List<int>();
		string[] array = @string.Split(new char[]
		{
			'`'
		});
		for (int i = 0; i < array.Length; i++)
		{
			int item;
			if (!int.TryParse(array[i], out item))
			{
				break;
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0002CB7C File Offset: 0x0002AD7C
	public void Test_PrintIDs()
	{
		List<int> list = this.LoadListIDs(MobileNotificationType.PhoneFlingNewText);
		string text = "[UserAway: " + this.LoadID(MobileNotificationType.UserAway).ToString() + "] ";
		foreach (int num in list)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" [",
				(MobileNotificationType)(num >> 24),
				" ID: ",
				num & 16777215,
				" Raw: ",
				num,
				"] "
			});
		}
		Debug.LogError("MNM Count: " + list.Count + " ");
		Debug.LogError("MNM IDs : " + text);
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002CC88 File Offset: 0x0002AE88
	public void DoOnApplicationPause(bool pauseState)
	{
		this.CancelAllNotifications();
		if (pauseState)
		{
			this.AddPhoneFlingNotifications();
			this.AddOfflineNotification();
		}
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0002CCA4 File Offset: 0x0002AEA4
	private void AddOfflineNotification()
	{
		string title = "Crush Crush misses you ♥";
		string text = "Check out all the progress you've made!";
		NotificationData notificationData = new NotificationData(MobileNotificationType.UserAway, title, text, DateTime.Now.AddHours(48.0), string.Empty, "icon_small_default", "icon_offline", 0);
		this.AddNotification(notificationData);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
	public void AddPhoneFlingNotifications()
	{
		List<NotificationData> phoneFlingNotifications = GameState.GetCellphone().GetPhoneFlingNotifications();
		if (phoneFlingNotifications == null || phoneFlingNotifications.Count == 0)
		{
			return;
		}
		foreach (NotificationData notificationData in phoneFlingNotifications)
		{
			this.AddNotification(notificationData);
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0002CD74 File Offset: 0x0002AF74
	protected bool IsNotificationListType(MobileNotificationType notificationType)
	{
		return this.notificationListTypes.Contains(notificationType);
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0002CD84 File Offset: 0x0002AF84
	protected virtual DateTime AdjustDateTime(DateTime dateTime)
	{
		int hour = dateTime.Hour;
		if (hour <= 8)
		{
			dateTime = dateTime.AddHours((double)(8 - hour));
		}
		else if (hour > 20)
		{
			dateTime = DateTime.Today.AddDays(1.0).AddHours(8.0);
		}
		return dateTime;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0002CDE4 File Offset: 0x0002AFE4
	protected string GetNotificationGroupType(MobileNotificationType notificationType)
	{
		if (notificationType != MobileNotificationType.UserAway)
		{
			if (notificationType == MobileNotificationType.PhoneFlingNewText)
			{
				return "cc_phoneFling";
			}
			if (notificationType != MobileNotificationType.Debug)
			{
			}
		}
		return "cc_offlineProgress";
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0002CE18 File Offset: 0x0002B018
	protected int GetNotificationId(NotificationData notificationData)
	{
		return (int)((int)notificationData.NotificationType << 24 | (MobileNotificationType)notificationData.ID);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0002CE2C File Offset: 0x0002B02C
	private void HandleLegacyPrefs()
	{
		if (global::PlayerPrefs.HasKey("MNM_SEEN_PP"))
		{
			global::PlayerPrefs.DeleteKey("MNM_SEEN_PP", false);
			global::PlayerPrefs.SetInt("DO-NOT-BACKUPMNM_SEEN_PP", 1);
			GameState.CurrentState.QueueSave();
		}
		if (global::PlayerPrefs.HasKey("MNM_ID_TYPE_6"))
		{
			global::PlayerPrefs.DeleteKey("MNM_ID_TYPE_6", false);
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x0400055A RID: 1370
	internal const string KeyHasShownPermissionPopupBefore = "DO-NOT-BACKUPMNM_SEEN_PP";

	// Token: 0x0400055B RID: 1371
	private const string KeyPrefixNotificationID = "MNM_ID_TYPE_";

	// Token: 0x0400055C RID: 1372
	protected const int _earliestNotificationHour = 8;

	// Token: 0x0400055D RID: 1373
	protected const int _latestNotificationHour = 20;

	// Token: 0x0400055E RID: 1374
	private readonly List<MobileNotificationType> notificationListTypes = new List<MobileNotificationType>
	{
		MobileNotificationType.PhoneFlingNewText
	};
}
