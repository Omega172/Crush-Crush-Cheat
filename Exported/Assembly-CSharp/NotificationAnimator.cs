using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000137 RID: 311
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x0600080F RID: 2063 RVA: 0x0004B3D8 File Offset: 0x000495D8
	private void Start()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0004B3E8 File Offset: 0x000495E8
	public void Update()
	{
		this.currentTime = (this.currentTime + Time.deltaTime) % 1f;
		int num = (int)(this.currentTime * (float)this.icons.Length * 2f);
		this.image.sprite = this.icons[num % this.icons.Length];
	}

	// Token: 0x04000883 RID: 2179
	public Sprite[] icons;

	// Token: 0x04000884 RID: 2180
	private float currentTime;

	// Token: 0x04000885 RID: 2181
	private Image image;
}
