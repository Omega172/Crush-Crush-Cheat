using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000132 RID: 306
public class ButtonToggle : MonoBehaviour
{
	// Token: 0x060007FB RID: 2043 RVA: 0x0004AF7C File Offset: 0x0004917C
	public void SetImage(bool active)
	{
		base.GetComponent<Image>().sprite = ((!active) ? this.DefaultImage : this.ActiveImage);
	}

	// Token: 0x04000878 RID: 2168
	public Sprite ActiveImage;

	// Token: 0x04000879 RID: 2169
	public Sprite DefaultImage;
}
