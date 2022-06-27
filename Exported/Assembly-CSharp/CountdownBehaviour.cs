using System;
using BlayFap;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class CountdownBehaviour : MonoBehaviour
{
	// Token: 0x06000A23 RID: 2595 RVA: 0x00053A04 File Offset: 0x00051C04
	private void Start()
	{
		this.text = new CachedObject<Text>(base.gameObject, "Text");
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00053A1C File Offset: 0x00051C1C
	private void Update()
	{
		if (this.EndDate.ToBinary() == 0L)
		{
			return;
		}
		Utilities.CheckCachedServerTime(null);
		if (Utilities.TimeRequested || !BlayFapClient.LoggedIn)
		{
			return;
		}
		this.tick += Time.deltaTime;
		if (this.tick > 1f)
		{
			this.tick -= 1f;
			TimeSpan end = this.EndDate - DateTime.UtcNow + Utilities.TimeOffset;
			if (end.TotalSeconds < 0.0)
			{
				if (this.OnCompleted != null)
				{
					this.OnCompleted();
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
			this.text.Object.text = ((!string.IsNullOrEmpty(this.Format)) ? string.Format(this.Format, Utilities.CreateCountdown(end)) : Utilities.CreateCountdown(end));
		}
	}

	// Token: 0x040009CB RID: 2507
	public DateTime EndDate = new DateTime(0L);

	// Token: 0x040009CC RID: 2508
	private CachedObject<Text> text;

	// Token: 0x040009CD RID: 2509
	private float tick = 1f;

	// Token: 0x040009CE RID: 2510
	public string Format;

	// Token: 0x040009CF RID: 2511
	public Action OnCompleted;
}
