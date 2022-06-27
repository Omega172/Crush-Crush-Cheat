using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000112 RID: 274
public interface Introduction
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600069F RID: 1695
	Girl Girl { get; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060006A0 RID: 1696
	Color BackgroundColor { get; }

	// Token: 0x060006A1 RID: 1697
	void Update(IntroProvider provider);

	// Token: 0x060006A2 RID: 1698
	void OnClick(IntroProvider provider);

	// Token: 0x060006A3 RID: 1699
	IEnumerator Initialize(IntroProvider provider, Sprite albumImage);

	// Token: 0x060006A4 RID: 1700
	void Destroy(IntroProvider provider);

	// Token: 0x060006A5 RID: 1701
	void UpdateTranslation(IntroProvider provider);
}
