using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
public class CassieIntroduction : GenericIntroduction
{
	// Token: 0x06000001 RID: 1 RVA: 0x000020EC File Offset: 0x000002EC
	public CassieIntroduction(Girl girl) : base(girl)
	{
		if (!this._assets.Contains("eventCGs00_pigeon1"))
		{
			this._assets.Add("eventCGs00_pigeon1");
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002128 File Offset: 0x00000328
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		yield return base.Initialize(provider, albumImage);
		this._finalState += 2;
		yield break;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002160 File Offset: 0x00000360
	public override void OnClick(IntroProvider provider)
	{
		if (this._state < 6)
		{
			base.OnClick(provider);
		}
		this._currentTime = 0f;
		if (this._state == 2)
		{
			provider.Portrait.sprite = provider.Sprites[0];
			this._maskImage = new GameObject().AddComponent<Image>();
			this._maskImage.gameObject.name = "Internal Mask";
			this._maskImage.transform.SetParent(provider.transform.Find("Mask"));
			this._maskImage.rectTransform.sizeDelta = provider.Portrait.rectTransform.sizeDelta;
			this._maskImage.rectTransform.anchoredPosition3D = Vector3.zero;
			this._maskImage.sprite = provider.Sprites[0];
			this._maskImage.gameObject.AddComponent<Mask>().showMaskGraphic = false;
			this._auxImage = new GameObject().AddComponent<Image>();
			this._auxImage.gameObject.layer = provider.TextBox.gameObject.layer;
			this._auxImage.sprite = provider.Sprites[5];
			this._auxImage.color = new Color(0.007843138f, 0.8352941f, 0.8980392f, 1f);
			this._auxImage.transform.SetParent(this._maskImage.transform);
			this._auxImage.rectTransform.anchoredPosition3D = new Vector3(-85f, 35f, 0f);
			this._auxImage.gameObject.SetActive(true);
		}
		else if (this._state == 3)
		{
			this._auxImage.rectTransform.sizeDelta = provider.Portrait.rectTransform.sizeDelta;
			provider.Portrait.sprite = provider.Sprites[0];
			this._auxImage.sprite = provider.Sprites[2];
			this._auxImage.color = Color.white;
			this._auxImage.rectTransform.anchoredPosition3D = Vector3.zero;
		}
		else if (this._state == 4)
		{
			this._auxImage.gameObject.SetActive(false);
			provider.Portrait.sprite = provider.Sprites[3];
			provider.Background.color = new Color(0.8392157f, 0.9647059f, 0.61960787f);
			this._state++;
		}
		else if (this._state == 5)
		{
			this._auxImage.gameObject.SetActive(false);
			provider.Portrait.sprite = provider.Sprites[3];
			provider.Background.color = new Color(0.8392157f, 0.9647059f, 0.61960787f);
		}
		else if (this._state == 6)
		{
			this._auxImage.gameObject.SetActive(true);
			this._auxImage.rectTransform.sizeDelta = new Vector2(276f, 166f);
			this._auxImage.rectTransform.anchoredPosition3D = new Vector3(250f, -96f, 0f);
			this._auxImage.sprite = provider.Sprites[4];
			provider.Portrait.sprite = provider.Sprites[0];
			provider.Background.color = this.BackgroundColor;
			this._state++;
		}
		else if (this._state == 7)
		{
			provider.Portrait.sprite = provider.Sprites[0];
			provider.TextBox.text = string.Empty;
			this._state++;
		}
		else if (this._state >= 8)
		{
			this.Skip(provider);
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000253C File Offset: 0x0000073C
	public override void HandleCrush(IntroProvider provider)
	{
		this._isCrush = (this._state == 3);
		if (this._isCrush)
		{
			this._currentTime = 0f;
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.BadReaction);
		}
		provider.Crush.gameObject.SetActive(this._isCrush);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002598 File Offset: 0x00000798
	public override void Update(IntroProvider provider)
	{
		if (this._state == 8)
		{
			if (this._currentTime < 2f)
			{
				this._currentTime += Time.deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, this._currentTime / 2f);
				if (this._auxImage != null)
				{
					this._auxImage.rectTransform.localPosition = new Vector3(250f + 900f * num, -92f - 100f * num, 0f);
				}
				if (this._currentTime > 1f)
				{
					this.Skip(provider);
				}
			}
		}
		else
		{
			base.Update(provider);
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002658 File Offset: 0x00000858
	protected override void Skip(IntroProvider provider)
	{
		if (IntroScreen.TutorialState < IntroScreen.State.IntroduceJobs)
		{
			GameState.GetIntroScreen().FirstIntro();
		}
		base.Skip(provider);
		if (this._auxImage != null)
		{
			this._auxImage.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000026A4 File Offset: 0x000008A4
	public override void Destroy(IntroProvider provider)
	{
		base.Destroy(provider);
		if (this._auxImage != null)
		{
			UnityEngine.Object.Destroy(this._auxImage.gameObject);
			this._auxImage = null;
		}
		if (this._maskImage != null)
		{
			UnityEngine.Object.Destroy(this._maskImage.gameObject);
			this._maskImage = null;
		}
	}

	// Token: 0x04000001 RID: 1
	private Image _auxImage;

	// Token: 0x04000002 RID: 2
	private Image _maskImage;
}
