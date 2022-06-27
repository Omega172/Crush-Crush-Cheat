using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class BackToSchool : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00003458 File Offset: 0x00001658
	private void Update()
	{
		Utilities.CheckCachedServerTime(null);
		if (Utilities.TimeRequested)
		{
			return;
		}
		TimeSpan timeOffset = Utilities.TimeOffset;
		this.tick += Time.deltaTime;
		if (this.tick > 1f)
		{
			this.tick -= 1f;
			TimeSpan end = this.endTime - DateTime.UtcNow + timeOffset;
			this.totalSeconds = Mathf.FloorToInt((float)end.TotalSeconds);
			string text = Utilities.CreateCountdown(end);
			base.transform.Find("Text").GetComponent<Text>().text = text;
			if (this.saleSplash == null)
			{
				this.saleSplash = GameState.CurrentState.transform.Find("Store/Purchase Diamonds Popup/Summer Sale");
				this.saleCountdown = GameState.CurrentState.transform.Find("Store/Sale Countdown");
			}
			if (this.saleSplash != null)
			{
				this.saleSplash.Find("Ends In").GetComponent<Text>().text = "Ends in " + text;
			}
			if (this.saleCountdown != null)
			{
				this.saleCountdown.Find("Text").GetComponent<Text>().text = text.Substring(0, text.Length - 3);
			}
		}
		if (this.totalSeconds < 0 && GameState.CurrentState != null)
		{
			GameState.CurrentState.transform.Find("Store/Purchase Diamonds Popup/Summer Sale").gameObject.SetActive(false);
			GameState.CurrentState.transform.Find("Store/Sale Countdown").gameObject.SetActive(false);
		}
	}

	// Token: 0x0400000D RID: 13
	private int totalSeconds;

	// Token: 0x0400000E RID: 14
	private DateTime endTime = new DateTime(2017, 9, 5, 4, 0, 0, DateTimeKind.Utc);

	// Token: 0x0400000F RID: 15
	private float tick;

	// Token: 0x04000010 RID: 16
	private Transform saleSplash;

	// Token: 0x04000011 RID: 17
	private Transform saleCountdown;
}
