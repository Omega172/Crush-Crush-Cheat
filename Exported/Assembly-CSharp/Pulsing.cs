using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class Pulsing : MonoBehaviour
{
	// Token: 0x06000812 RID: 2066 RVA: 0x0004B44C File Offset: 0x0004964C
	private void Update()
	{
		this.currentTime += Time.deltaTime * 3f;
		if (this.currentTime > 6.2831855f)
		{
			this.currentTime -= 6.2831855f;
		}
		float num = 1f + 0.05f * Mathf.Sin(this.currentTime);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x04000886 RID: 2182
	private float currentTime;
}
