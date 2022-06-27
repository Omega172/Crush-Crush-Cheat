using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class MioIntroduction : GenericIntroduction
{
	// Token: 0x0600001B RID: 27 RVA: 0x0000327C File Offset: 0x0000147C
	public MioIntroduction(Girl girl) : base(girl)
	{
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00003288 File Offset: 0x00001488
	private void InitHighscore(IntroProvider provider, Sprite sprite)
	{
		this._highScore = new GameObject();
		this._highScore.AddComponent<Image>();
		this._highScore.layer = provider.TextBox.gameObject.layer;
		this._highScore.GetComponent<Image>().sprite = sprite;
		this._highScore.transform.SetParent(provider.transform.Find("Mask"));
		this._highScore.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(620f, 132f);
		this._highScore.GetComponent<Image>().rectTransform.localPosition = new Vector3(0f, 10f, 0f);
		this._highScore.SetActive(false);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00003354 File Offset: 0x00001554
	public override void Update(IntroProvider provider)
	{
		base.Update(provider);
		if (this._isCrush || this._state >= 3)
		{
			this._highScore.SetActive(this._currentTime % 1f < 0.5f);
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000033A0 File Offset: 0x000015A0
	public override void OnClick(IntroProvider provider)
	{
		base.OnClick(provider);
		if (this._state == this._finalState && this._highScore != null)
		{
			this._highScore.SetActive(false);
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000033D8 File Offset: 0x000015D8
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		yield return base.Initialize(provider, albumImage);
		this.InitHighscore(provider, provider.Sprites[provider.Sprites.Length - 1]);
		yield break;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003410 File Offset: 0x00001610
	public override void Destroy(IntroProvider provider)
	{
		base.Destroy(provider);
		UnityEngine.Object.Destroy(this._highScore);
		this._highScore = null;
	}

	// Token: 0x0400000C RID: 12
	private GameObject _highScore;
}
