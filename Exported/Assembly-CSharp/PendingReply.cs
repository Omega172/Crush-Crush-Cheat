using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class PendingReply : MonoBehaviour
{
	// Token: 0x060006E0 RID: 1760 RVA: 0x0003BE50 File Offset: 0x0003A050
	private void Start()
	{
		this.text1 = base.transform.Find("Background/Text 1").GetComponent<RectTransform>();
		this.text2 = base.transform.Find("Background/Text 2").GetComponent<RectTransform>();
		this.text3 = base.transform.Find("Background/Text 3").GetComponent<RectTransform>();
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0003BEB0 File Offset: 0x0003A0B0
	private void Update()
	{
		this.text1.localPosition = new Vector3(-6.8f, 6f + 12f * Mathf.Max(0f, Mathf.Sin(3f * this.currentTime - 0.5f)));
		this.text2.localPosition = new Vector3(3.2f, 6f + 12f * Mathf.Max(0f, Mathf.Sin(3f * this.currentTime)));
		this.text3.localPosition = new Vector3(13.2f, 6f + 12f * Mathf.Max(0f, Mathf.Sin(3f * this.currentTime + 0.5f)));
		this.currentTime += Time.deltaTime;
	}

	// Token: 0x040006D4 RID: 1748
	private RectTransform text1;

	// Token: 0x040006D5 RID: 1749
	private RectTransform text2;

	// Token: 0x040006D6 RID: 1750
	private RectTransform text3;

	// Token: 0x040006D7 RID: 1751
	private float currentTime;
}
