using System;

// Token: 0x020000F1 RID: 241
[Serializable]
public class NotificationData
{
	// Token: 0x0600057E RID: 1406 RVA: 0x0002CE8C File Offset: 0x0002B08C
	public NotificationData()
	{
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0002CEA0 File Offset: 0x0002B0A0
	public NotificationData(MobileNotificationType notificationType, string title, string text, DateTime fireTime, string subtitle = "", string smallIcon = "icon_small_default", string largeIcon = "", int id = 0)
	{
		this.NotificationType = notificationType;
		this.Title = title;
		this.Text = text;
		this.FireTime = fireTime;
		this.Subtitle = subtitle;
		this.SmallIcon = smallIcon;
		this.LargeIcon = largeIcon;
		this.ID = id;
	}

	// Token: 0x0400055F RID: 1375
	public MobileNotificationType NotificationType;

	// Token: 0x04000560 RID: 1376
	public int ID;

	// Token: 0x04000561 RID: 1377
	public string Title;

	// Token: 0x04000562 RID: 1378
	public string Text;

	// Token: 0x04000563 RID: 1379
	public DateTime FireTime;

	// Token: 0x04000564 RID: 1380
	public string Subtitle;

	// Token: 0x04000565 RID: 1381
	public string SmallIcon = "icon_small_default";

	// Token: 0x04000566 RID: 1382
	public string LargeIcon;
}
