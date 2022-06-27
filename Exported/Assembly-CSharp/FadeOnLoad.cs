using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000135 RID: 309
public class FadeOnLoad : MonoBehaviour
{
	// Token: 0x06000808 RID: 2056 RVA: 0x0004B1F0 File Offset: 0x000493F0
	private void Start()
	{
		this._image = base.GetComponent<Image>();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x0004B200 File Offset: 0x00049400
	private void Update()
	{
		if (this._image != null && this._image.sprite != null && this._image.color.a != 1f)
		{
			this._image.color = new Color(1f, 1f, 1f, Mathf.Min(1f, this._image.color.a + Time.deltaTime * 2f));
		}
	}

	// Token: 0x04000881 RID: 2177
	private Image _image;
}
