using System;
using UnityEngine;
using UnityEngine.UI;

namespace KittyCrush
{
	// Token: 0x0200012E RID: 302
	public class AnimatedEnable : MonoBehaviour
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x0004A910 File Offset: 0x00048B10
		private void OnEnable()
		{
			this.currentTime = 0f;
			if (this.AnimateBackground)
			{
				base.GetComponent<Image>().color = new Color(0.40392157f, 0.08235294f, 0.2901961f, 0f);
			}
			if (this.AnimationType == 0)
			{
				this.x = base.transform.localPosition.x;
				this.TargetY = base.transform.localPosition.y;
				base.transform.Find("Dialog").transform.localPosition = new Vector3(this.x, this.StartY, 0f);
			}
			else if (this.AnimationType == 1)
			{
				base.transform.Find("Dialog").transform.localScale = new Vector3(0f, 0f, 1f);
			}
			else if (this.AnimationType == 2)
			{
				base.transform.localScale = new Vector3(0f, 0f, 1f);
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0004AA34 File Offset: 0x00048C34
		private void Update()
		{
			if (this.currentTime < 0.33f)
			{
				float num = Utilities.Easing.BackEaseOut(this.currentTime * 3f, 0f, 1f, 1f);
				if (this.AnimateBackground)
				{
					base.GetComponent<Image>().color = new Color(0.40392157f, 0.08235294f, 0.2901961f, Mathf.Min(0.5019608f, 0.5019608f * this.currentTime * 5f));
				}
				if (this.AnimationType == 0)
				{
					base.transform.Find("Dialog").transform.localPosition = new Vector3(this.x, this.StartY - (this.StartY - this.TargetY) * num, 0f);
				}
				else if (this.AnimationType == 1)
				{
					base.transform.Find("Dialog").transform.localScale = new Vector3(num, num, 1f);
				}
				else if (this.AnimationType == 2)
				{
					base.transform.localScale = new Vector3(num, num, 1f);
				}
				this.currentTime += Time.deltaTime;
			}
			else if (this.currentTime >= 0.33f && this.currentTime < 100f)
			{
				if (this.AnimateBackground)
				{
					base.GetComponent<Image>().color = new Color(0.40392157f, 0.08235294f, 0.2901961f, 0.5019608f);
				}
				if (this.AnimationType == 0)
				{
					base.transform.Find("Dialog").transform.localPosition = new Vector3(this.x, this.TargetY, 0f);
				}
				else if (this.AnimationType == 1)
				{
					base.transform.Find("Dialog").transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else if (this.AnimationType == 2)
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				this.currentTime = 100f;
			}
		}

		// Token: 0x0400086B RID: 2155
		public float StartY;

		// Token: 0x0400086C RID: 2156
		public float TargetY;

		// Token: 0x0400086D RID: 2157
		public bool AnimateBackground;

		// Token: 0x0400086E RID: 2158
		public int AnimationType;

		// Token: 0x0400086F RID: 2159
		private float currentTime;

		// Token: 0x04000870 RID: 2160
		private float x;
	}
}
