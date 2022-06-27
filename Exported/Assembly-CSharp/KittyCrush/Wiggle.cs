using System;
using UnityEngine;

namespace KittyCrush
{
	// Token: 0x0200013B RID: 315
	public class Wiggle : MonoBehaviour
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0004B800 File Offset: 0x00049A00
		private void Awake()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0004B810 File Offset: 0x00049A10
		private void Update()
		{
			if (this.enabled || this.AlwaysOn)
			{
				this.currentTime += Time.deltaTime;
				this.rectTransform.localEulerAngles = new Vector3(0f, 0f, (float)(((double)this.currentTime % 4.71238911151886 >= 2.356194496154785) ? 0 : 1) * Mathf.Sin(this.currentTime * 32f) * 10f);
			}
			else if (this.rectTransform.localEulerAngles.z != 0f)
			{
				this.rectTransform.localEulerAngles = Vector3.zero;
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0004B8D0 File Offset: 0x00049AD0
		public void Enable()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.currentTime = 0f;
			this.rectTransform.localScale = 1.1f * Vector3.one;
			this.enabled = true;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0004B91C File Offset: 0x00049B1C
		public void Disable()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.rectTransform.localScale = Vector3.one;
			this.rectTransform.localEulerAngles = Vector3.zero;
			this.enabled = false;
		}

		// Token: 0x0400088D RID: 2189
		private RectTransform rectTransform;

		// Token: 0x0400088E RID: 2190
		private float currentTime;

		// Token: 0x0400088F RID: 2191
		public bool AlwaysOn;

		// Token: 0x04000890 RID: 2192
		private new bool enabled;
	}
}
