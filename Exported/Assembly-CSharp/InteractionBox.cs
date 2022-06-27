using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CB RID: 203
public class InteractionBox : MonoBehaviour
{
	// Token: 0x06000465 RID: 1125 RVA: 0x00021738 File Offset: 0x0001F938
	private void Update()
	{
		if (Girls.CurrentGirl != this.activeGirl)
		{
			this.LevelUp = false;
			base.gameObject.SetActive(false);
		}
		if (this.currentTime < 0.5f)
		{
			this.currentTime += Time.deltaTime;
		}
		else if (this.currentTime < 1f)
		{
			float num = Utilities.Easing.CubicEaseInOut((this.currentTime - 0.5f) * 2f, 0f, 1f, 1f);
			base.transform.localScale = new Vector3(num, num, 1f);
			this.currentTime += Time.deltaTime;
		}
		else if (this.currentTime <= 100f)
		{
			this.currentTime = 101f;
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00021834 File Offset: 0x0001FA34
	private float GetBestFitSize(string text)
	{
		if (Translations.CurrentLanguage.Value == 10 || Translations.CurrentLanguage.Value == 1)
		{
			return 16f;
		}
		this.SpeechText.cachedTextGenerator.Invalidate();
		Vector2 size = (this.SpeechText.transform as RectTransform).rect.size;
		TextGenerationSettings generationSettings = this.SpeechText.GetGenerationSettings(size);
		generationSettings.scaleFactor = 1f;
		if (!this.SpeechText.cachedTextGenerator.Populate(text, generationSettings))
		{
			Debug.LogError("Failed to generate fit size");
		}
		return (float)this.SpeechText.cachedTextGenerator.fontSizeUsedForBestFit;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000218E4 File Offset: 0x0001FAE4
	public void Init(Girl girl, string text, bool delay = false, long heartPayout = 0L, bool levelUp = false, string buttonText = "Close")
	{
		if (Translations.CurrentLanguage.Value == 10 || Translations.CurrentLanguage.Value == 1)
		{
			this.SpeechText.resizeTextForBestFit = false;
			this.SpeechText.fontSize = ((text.Length <= 65) ? 16 : 14);
		}
		else
		{
			this.SpeechText.resizeTextForBestFit = true;
		}
		this.activeGirl = girl;
		this.NameText.text = Translations.TranslateGirlName(girl.GirlName);
		this.SpeechText.text = text;
		float bestFitSize = this.GetBestFitSize(text);
		this.SpeechText.lineSpacing = ((bestFitSize > 14f) ? 1f : 1.2f);
		Debug.Log("Text is " + this.SpeechText.fontSize);
		base.transform.localScale = ((!delay) ? new Vector3(1f, 1f, 1f) : new Vector3(0f, 0f, 1f));
		base.gameObject.SetActive(true);
		this.currentTime = ((!delay) ? 1f : 0f);
		base.transform.Find("Content/Accept Button").GetComponent<Pulsing>().enabled = levelUp;
		if (levelUp && this.Hidden)
		{
			this.UpdateOpacity(false);
		}
		else if (this.Hidden)
		{
			this.UpdateOpacity(true);
		}
		if (girl.Love == 9)
		{
			heartPayout = 0L;
		}
		heartPayout = Math.Min(heartPayout, girl.HeartRequirement - girl.Hearts);
		base.transform.Find("Content/Heart Value Icon").gameObject.SetActive(heartPayout != 0L && !this.Hidden && !levelUp);
		base.transform.Find("Content/Heart Text").gameObject.SetActive(heartPayout != 0L && !this.Hidden && !levelUp);
		base.transform.Find("Content/Heart Text").GetComponent<Text>().text = string.Format("x {0}", heartPayout.ToString("n0"));
		if (heartPayout == 9223372036854775807L || (double)girl.Hearts + (double)heartPayout > 9.223372036854776E+18)
		{
			girl.Hearts = girl.HeartRequirement;
		}
		else if ((double)girl.Hearts + (double)heartPayout < 0.0)
		{
			girl.Hearts = 1L;
		}
		else
		{
			girl.Hearts += heartPayout;
		}
		base.transform.Find("Hide Button").gameObject.SetActive(!levelUp);
		base.transform.Find("Content/Accept Button/Accept Text").GetComponent<Text>().text = ((!(buttonText == "Close")) ? buttonText : Translations.GetTranslation("everything_else_10_0", "Close"));
		this.LevelUp = levelUp;
		if (girl.GirlName == Balance.GirlName.Explora && text == "ROFLCOPTER!")
		{
			if (!this._isRofling)
			{
				this._isRofling = true;
				base.transform.Find("Content/Rofl").gameObject.SetActive(true);
				base.transform.Find("Content/Body").gameObject.SetActive(false);
			}
		}
		else if (this._isRofling)
		{
			this._isRofling = false;
			base.transform.Find("Content/Rofl").gameObject.SetActive(false);
			base.transform.Find("Content/Body").gameObject.SetActive(true);
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00021CC0 File Offset: 0x0001FEC0
	public void OnClose()
	{
		if (this.LevelUp)
		{
			this.LevelUp = false;
			base.gameObject.SetActive(false);
			GameState.GetGirlScreen().OnClickAccept();
		}
		else
		{
			base.gameObject.SetActive(false);
			GameState.GetGirlScreen().SetGirl();
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00021D10 File Offset: 0x0001FF10
	public void Toggle()
	{
		if (this.LevelUp)
		{
			return;
		}
		this.UpdateOpacity(!this.Hidden);
		this.Hidden = !this.Hidden;
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00021D48 File Offset: 0x0001FF48
	public void Show()
	{
		this.UpdateOpacity(false);
		this.Hidden = false;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00021D58 File Offset: 0x0001FF58
	public void Hide()
	{
		if (this.LevelUp)
		{
			return;
		}
		this.UpdateOpacity(true);
		this.Hidden = true;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00021D74 File Offset: 0x0001FF74
	public void UpdateOpacity(bool hidden)
	{
		base.transform.Find("Content").gameObject.SetActive(!hidden);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00021DA0 File Offset: 0x0001FFA0
	public void HandleOfflinePopupDismissed()
	{
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00021DA4 File Offset: 0x0001FFA4
	public void OnBackButtonOverride()
	{
	}

	// Token: 0x04000478 RID: 1144
	public Text SpeechText;

	// Token: 0x04000479 RID: 1145
	public Text NameText;

	// Token: 0x0400047A RID: 1146
	public bool Hidden;

	// Token: 0x0400047B RID: 1147
	public bool LevelUp;

	// Token: 0x0400047C RID: 1148
	private Girl activeGirl;

	// Token: 0x0400047D RID: 1149
	private float currentTime;

	// Token: 0x0400047E RID: 1150
	private bool _isRofling;
}
