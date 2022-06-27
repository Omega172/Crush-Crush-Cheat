using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class PixelationEffect : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x00003DD4 File Offset: 0x00001FD4
	public void SetEffect(float lerp)
	{
		lerp = Mathf.Clamp01(lerp);
		this.SetPixelation(base.GetComponent<Image>(), lerp);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003DEC File Offset: 0x00001FEC
	public void SetWidth(float width)
	{
		base.GetComponent<Image>().material.SetFloat("_Width", width);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003E04 File Offset: 0x00002004
	private void SetPixelation(Image target, float lerp)
	{
		if (lerp == 1f)
		{
			target.material.SetFloat("_Pixel1", 1f);
			target.material.SetFloat("_Pixel2", 1f);
		}
		else if (lerp <= 0.25f)
		{
			target.material.SetFloat("_Pixel1", 16f);
			target.material.SetFloat("_Pixel2", 8f);
			target.material.SetFloat("_Lerp", 1f - lerp * 4f);
		}
		else if (lerp < 1f)
		{
			target.material.SetFloat("_Pixel1", 8f);
			target.material.SetFloat("_Pixel2", 1f);
			target.material.SetFloat("_Lerp", 1f - (lerp - 0.25f) / 0.75f);
		}
	}
}
