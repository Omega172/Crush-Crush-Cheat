using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000094 RID: 148
public class FreeTime : MonoBehaviour
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000297 RID: 663 RVA: 0x0001266C File Offset: 0x0001086C
	public static int Free
	{
		get
		{
			return FreeTime.TimeSlots + FreeTime.PurchasedTime - FreeTime.JobTime - FreeTime.HobbyTime - FreeTime.DateTime - FreeTime.EventTime;
		}
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00012694 File Offset: 0x00010894
	private void Start()
	{
		GameState.UniverseReady += new ReactiveProperty<bool>.Changed(this.InitSlots);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x000126B4 File Offset: 0x000108B4
	private void OnDestroy()
	{
		GameState.UniverseReady -= new ReactiveProperty<bool>.Changed(this.InitSlots);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x000126D4 File Offset: 0x000108D4
	private void InitSlots(bool universeReady = true)
	{
		if (!universeReady)
		{
			return;
		}
		if (this.Slots != null && this.Slots.Length > 0)
		{
			for (int i = 0; i < this.Slots.Length; i++)
			{
				UnityEngine.Object.Destroy(this.Slots[i]);
			}
			this.Slots = null;
		}
		if (FreeTime.TimeSlots + FreeTime.PurchasedTime > 25)
		{
			if ((global::PlayerPrefs.GetInt("Tutorial", 0) & 2) == 0)
			{
				GameState.GetIntroScreen().TimeBlockStart();
			}
			base.transform.Find("Icons").gameObject.SetActive(true);
		}
		else
		{
			this.Slots = new Image[Mathf.Min(51, FreeTime.TimeSlots + FreeTime.PurchasedTime)];
			for (int j = 0; j < this.Slots.Length; j++)
			{
				this.Slots[j] = UnityEngine.Object.Instantiate<GameObject>(this.WhiteSquare).GetComponent<Image>();
				this.Slots[j].transform.SetParent(base.transform);
				this.Slots[j].rectTransform.localPosition = new Vector3((float)(16 * j), 0f, 0f);
				this.Slots[j].gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0001281C File Offset: 0x00010A1C
	private void Update()
	{
		if ((this.Slots == null || FreeTime.TimeSlots + FreeTime.PurchasedTime != this.Slots.Length) && FreeTime.TimeSlots + FreeTime.PurchasedTime <= 51)
		{
			this.InitSlots(true);
		}
		if (FreeTime.TimeSlots + FreeTime.PurchasedTime > 25)
		{
			if (this.freeTimeText == null)
			{
				this.freeTimeText = base.transform.Find("Icons/Free Text").GetComponent<Text>();
				this.jobTimeText = base.transform.Find("Icons/Job Text").GetComponent<Text>();
				this.hobbyTimeText = base.transform.Find("Icons/Hobby Text").GetComponent<Text>();
				this.dateTimeText = base.transform.Find("Icons/Date Text").GetComponent<Text>();
			}
			if (this.freeTimeText != null && this.freeCache != FreeTime.Free)
			{
				Text text = this.freeTimeText;
				string format = "x{0}";
				int num = this.freeCache = FreeTime.Free;
				text.text = string.Format(format, num.ToString());
			}
			if (this.jobTimeText != null && this.jobCache != FreeTime.JobTime)
			{
				Text text2 = this.jobTimeText;
				string format2 = "x{0}";
				int num2 = this.jobCache = FreeTime.JobTime;
				text2.text = string.Format(format2, num2.ToString());
			}
			if (this.hobbyTimeText != null && this.hobbyCache != FreeTime.HobbyTime)
			{
				Text text3 = this.hobbyTimeText;
				string format3 = "x{0}";
				int num3 = this.hobbyCache = FreeTime.HobbyTime;
				text3.text = string.Format(format3, num3.ToString());
			}
			if (this.dateTimeText != null && this.dateCache != FreeTime.DateTime + FreeTime.EventTime)
			{
				Text text4 = this.dateTimeText;
				string format4 = "x{0}";
				int num4 = this.dateCache = FreeTime.DateTime + FreeTime.EventTime;
				text4.text = string.Format(format4, num4.ToString());
			}
		}
		else
		{
			int num5 = 0;
			int num6 = 0;
			while (num6 < FreeTime.Free && num5 < this.Slots.Length)
			{
				this.Slots[num5].sprite = this.Empty;
				num6++;
				num5++;
			}
			int num7 = 0;
			while (num7 < FreeTime.JobTime && num5 < this.Slots.Length)
			{
				this.Slots[num5].sprite = this.Job;
				num7++;
				num5++;
			}
			int num8 = 0;
			while (num8 < FreeTime.HobbyTime && num5 < this.Slots.Length)
			{
				this.Slots[num5].sprite = this.Hobby;
				num8++;
				num5++;
			}
			int num9 = 0;
			while (num9 < FreeTime.DateTime + FreeTime.EventTime && num5 < this.Slots.Length)
			{
				this.Slots[num5].sprite = this.Date;
				num9++;
				num5++;
			}
		}
		if (FreeTime.JobTime < 0)
		{
			FreeTime.JobTime = 0;
		}
		if (FreeTime.HobbyTime < 0)
		{
			FreeTime.HobbyTime = 0;
		}
		if (FreeTime.DateTime < 0)
		{
			FreeTime.DateTime = 0;
		}
		if (FreeTime.EventTime < 0)
		{
			FreeTime.EventTime = 0;
		}
	}

	// Token: 0x040002F9 RID: 761
	private const int MAX_SLOTS = 51;

	// Token: 0x040002FA RID: 762
	public static int JobTime;

	// Token: 0x040002FB RID: 763
	public static int HobbyTime;

	// Token: 0x040002FC RID: 764
	public static int DateTime;

	// Token: 0x040002FD RID: 765
	public static int EventTime;

	// Token: 0x040002FE RID: 766
	public static int TimeSlots = 16;

	// Token: 0x040002FF RID: 767
	public static int PurchasedTime;

	// Token: 0x04000300 RID: 768
	private Image[] Slots;

	// Token: 0x04000301 RID: 769
	public GameObject WhiteSquare;

	// Token: 0x04000302 RID: 770
	public Sprite Date;

	// Token: 0x04000303 RID: 771
	public Sprite Job;

	// Token: 0x04000304 RID: 772
	public Sprite Hobby;

	// Token: 0x04000305 RID: 773
	public Sprite Empty;

	// Token: 0x04000306 RID: 774
	private Text freeTimeText;

	// Token: 0x04000307 RID: 775
	private Text jobTimeText;

	// Token: 0x04000308 RID: 776
	private Text hobbyTimeText;

	// Token: 0x04000309 RID: 777
	private Text dateTimeText;

	// Token: 0x0400030A RID: 778
	private int freeCache = -1;

	// Token: 0x0400030B RID: 779
	private int jobCache = -1;

	// Token: 0x0400030C RID: 780
	private int hobbyCache = -1;

	// Token: 0x0400030D RID: 781
	private int dateCache = -1;
}
