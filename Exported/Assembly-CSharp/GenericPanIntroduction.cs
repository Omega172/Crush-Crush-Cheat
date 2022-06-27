using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class GenericPanIntroduction : GenericIntroduction
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002E54 File Offset: 0x00001054
	public GenericPanIntroduction(Girl girl, bool pansRight) : base(girl)
	{
		this._panSign = ((!pansRight) ? -1 : 1);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002E70 File Offset: 0x00001070
	public override void Update(IntroProvider provider)
	{
		if (this._state == 2)
		{
			if (this._currentTime < 5f)
			{
				float num = Utilities.Easing.CubicEaseInOut(this._currentTime / 5f, 0f, 1f, 1f);
				provider.Portrait.transform.localPosition = new Vector3((float)(this._panSign * 123) - (float)(this._panSign * 243) * num, 0f, 0f);
				this._currentTime += Time.deltaTime;
			}
		}
		else
		{
			base.Update(provider);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002F14 File Offset: 0x00001114
	public override void OnClick(IntroProvider provider)
	{
		base.OnClick(provider);
		if (this._isCrush)
		{
			provider.Portrait.rectTransform.localPosition = new Vector3(0f, 0f);
			provider.Portrait.rectTransform.sizeDelta = new Vector2(776f, 360f);
		}
		else if (this._state == 2)
		{
			this._currentTime = 0f;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002F90 File Offset: 0x00001190
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		yield return base.Initialize(provider, albumImage);
		provider.Portrait.rectTransform.localPosition = new Vector3((float)(this._panSign * 123), 0f);
		provider.Portrait.rectTransform.sizeDelta = new Vector2(1076f, 360f);
		yield break;
	}

	// Token: 0x04000009 RID: 9
	private int _panSign;
}
