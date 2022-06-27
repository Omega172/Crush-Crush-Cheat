using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F2 RID: 242
public class Notifications : MonoBehaviour
{
	// Token: 0x06000582 RID: 1410 RVA: 0x0002CF1C File Offset: 0x0002B11C
	public static bool CheckNotification(Achievements.Achievement achievement)
	{
		return achievement.Complete();
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0002CF24 File Offset: 0x0002B124
	public static void Clear()
	{
		Notifications.pendingNotifications.Clear();
		Notifications.achievementTime = 5f;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002CF3C File Offset: 0x0002B13C
	public static void AddAchievement()
	{
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.TimeBlock);
		Notifications.achievementTime = 0f;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002CF5C File Offset: 0x0002B15C
	public static void AddNotification(Notifications.NotificationType type, string text)
	{
		if (type == Notifications.NotificationType.Achievement)
		{
			Debug.LogError("Achievement is calling AddNotification incorrectly!");
		}
		else if (type == Notifications.NotificationType.Message)
		{
			Notifications.pendingNotifications.Enqueue(new Notifications.Notification(type, text));
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002CF8C File Offset: 0x0002B18C
	public static void AddNotification(Achievements.Achievement achievement)
	{
		Notifications.pendingNotifications.Enqueue(new Notifications.Notification(Notifications.NotificationType.Achievement, achievement.Notification));
		GameState.GetAchievements().StoreState();
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0002CFC4 File Offset: 0x0002B1C4
	private void Start()
	{
		this.notificationTime = 3.5f;
		this.NotificationOrder = new Text[this.NotificationTexts.Length];
		for (int i = 0; i < this.NotificationTexts.Length; i++)
		{
			this.NotificationTexts[i].text = string.Empty;
			this.NotificationOrder[i] = this.NotificationTexts[i];
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0002D02C File Offset: 0x0002B22C
	private void Update()
	{
		if (this.notificationTime == 3.5f)
		{
			if (Notifications.pendingNotifications.Count > 0)
			{
				Notifications.Notification notification = Notifications.pendingNotifications.Dequeue();
				this.NotificationOrder[0].text = notification.Text;
				this.NotificationOrder[0].color = new Color(1f, 1f, 1f, 1f);
				this.NotificationOrder[0].rectTransform.localPosition = new Vector3(940f, 240f);
				this.notificationTime = 0f;
			}
		}
		else
		{
			this.notificationTime = Mathf.Min(3.5f, this.notificationTime + Time.deltaTime);
			if (this.notificationTime < 0.25f)
			{
				for (int i = 1; i < this.NotificationOrder.Length; i++)
				{
					this.NotificationOrder[i].color = new Color(1f, 1f, 1f, 1f - ((float)i + this.notificationTime * 4f) / (float)this.NotificationOrder.Length);
					this.NotificationOrder[i].rectTransform.localPosition = new Vector3(-50f, 240f + 19f * ((float)i + this.notificationTime * 4f - 1f));
				}
			}
			else if (this.notificationTime < 1f)
			{
				float t = (this.notificationTime - 0.25f) * 1.33f;
				float x = 940f - 990f * Utilities.Easing.CubicEaseInOut(t, 0f, 1f, 1f);
				this.NotificationOrder[0].rectTransform.localPosition = new Vector3(x, 240f, 0f);
			}
			else if (this.notificationTime == 3.5f)
			{
				for (int j = 0; j < this.NotificationOrder.Length; j++)
				{
					this.NotificationOrder[j].rectTransform.localPosition = new Vector3(-50f, (float)(240 + 19 * j));
				}
				Text text = this.NotificationOrder[this.NotificationOrder.Length - 1];
				for (int k = this.NotificationOrder.Length - 1; k > 0; k--)
				{
					this.NotificationOrder[k] = this.NotificationOrder[k - 1];
				}
				this.NotificationOrder[0] = text;
				for (int l = 0; l < this.NotificationOrder.Length; l++)
				{
					this.NotificationOrder[l].color = new Color(1f, 1f, 1f, 1f - (float)(l - 1) / (float)this.NotificationOrder.Length);
				}
			}
		}
		if (Notifications.achievementTime < 5f)
		{
			Notifications.achievementTime = Mathf.Min(5f, Notifications.achievementTime + Time.deltaTime);
			if (Notifications.achievementTime < 0.5f)
			{
				float num = Utilities.Easing.CubicEaseInOut(Notifications.achievementTime * 2f, 0f, 1f, 1f);
				this.AchievementDropdown.transform.localPosition = new Vector3(17f, 326f - num * 55f, 0f);
			}
			else if (Notifications.achievementTime > 4.5f)
			{
				float num2 = Utilities.Easing.CubicEaseInOut((Notifications.achievementTime - 4.5f) * 2f, 0f, 1f, 1f);
				this.AchievementDropdown.transform.localPosition = new Vector3(17f, 271f + num2 * 55f, 0f);
			}
		}
	}

	// Token: 0x04000567 RID: 1383
	private static Queue<Notifications.Notification> pendingNotifications = new Queue<Notifications.Notification>();

	// Token: 0x04000568 RID: 1384
	private static float achievementTime = 5f;

	// Token: 0x04000569 RID: 1385
	private Text[] NotificationOrder;

	// Token: 0x0400056A RID: 1386
	private float notificationTime;

	// Token: 0x0400056B RID: 1387
	public Text[] NotificationTexts;

	// Token: 0x0400056C RID: 1388
	public GameObject AchievementDropdown;

	// Token: 0x020000F3 RID: 243
	public enum NotificationType
	{
		// Token: 0x0400056E RID: 1390
		Achievement,
		// Token: 0x0400056F RID: 1391
		Message
	}

	// Token: 0x020000F4 RID: 244
	private struct Notification
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x0002D3EC File Offset: 0x0002B5EC
		public Notification(Notifications.NotificationType type, string text)
		{
			this.Type = type;
			this.Text = text;
		}

		// Token: 0x04000570 RID: 1392
		public Notifications.NotificationType Type;

		// Token: 0x04000571 RID: 1393
		public string Text;
	}
}
