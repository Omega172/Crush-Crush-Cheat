using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class GenericIntroduction : Introduction
{
	// Token: 0x06000679 RID: 1657 RVA: 0x00036C2C File Offset: 0x00034E2C
	public GenericIntroduction()
	{
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00036C7C File Offset: 0x00034E7C
	public GenericIntroduction(Girl girl)
	{
		this.Init(girl, this.GetDefaultList(girl));
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00036CD8 File Offset: 0x00034ED8
	public GenericIntroduction(Girl girl, List<GirlModel.IntroData> introDataList)
	{
		this.Init(girl, introDataList);
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600067C RID: 1660 RVA: 0x00036D30 File Offset: 0x00034F30
	// (set) Token: 0x0600067D RID: 1661 RVA: 0x00036D38 File Offset: 0x00034F38
	public Girl Girl { get; private set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x00036D44 File Offset: 0x00034F44
	public Color BackgroundColor
	{
		get
		{
			return this._backgroundColor;
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00036D4C File Offset: 0x00034F4C
	public void SetOnIntroductionDoneCallback(Action onIntroductionDone)
	{
		this.OnIntroductionDone = onIntroductionDone;
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00036D58 File Offset: 0x00034F58
	protected virtual List<GirlModel.IntroData> GetDefaultList(Girl girl)
	{
		return girl.Data.IntroDataList;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00036D68 File Offset: 0x00034F68
	protected virtual void Init(Girl girl, List<GirlModel.IntroData> introDataList)
	{
		this._cachedIntroDataList = ((introDataList == null) ? girl.Data.IntroDataList : introDataList);
		this.Girl = girl;
		this._backgroundColor = this.Girl.Data.IntroColor;
		string str = this.Girl.GirlName.ToLowerFriendlyString() + "_";
		foreach (GirlModel.IntroData introData in this._cachedIntroDataList)
		{
			if (introData == null)
			{
				break;
			}
			this._lines.Add(new LocalizationContext(introData.English, str + introData.ID.ToString()));
			if (!this._assets.Contains(introData.Data))
			{
				this._assets.Add(introData.Data);
			}
		}
		this._finalState = this._cachedIntroDataList.Count;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00036E88 File Offset: 0x00035088
	protected virtual string GetAssetBundleName(string name)
	{
		return name + "/" + name + "_pinups_mobile";
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00036E9C File Offset: 0x0003509C
	public virtual IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		GameState.Voiceover.Stop();
		yield return GameState.Voiceover.LoadSpecialBundle(this.Girl.GirlName, false);
		string name = this.Girl.GirlName.ToLowerFriendlyString();
		if (this.Girl.GirlName == Balance.GirlName.Mio)
		{
			this._assets.Add("100m");
		}
		yield return provider.LoadIntroSpritesAsync(this.GetAssetBundleName(name), this._assets.ToArray());
		this.OnClick(provider);
		yield break;
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00036EC8 File Offset: 0x000350C8
	public virtual void OnClick(IntroProvider provider)
	{
		while (this._state != this._finalState && !this._cachedIntroDataList[this._state].Playable)
		{
			this._state++;
		}
		GameState.Voiceover.Stop();
		if (this._state == this._finalState)
		{
			if (this.Girl.GirlName == Balance.GirlName.Cassie && IntroScreen.TutorialState < IntroScreen.State.IntroduceJobs)
			{
				provider.transform.Find("Intro Text Box").gameObject.SetActive(false);
				provider.transform.Find("Accept Button").gameObject.SetActive(false);
				GameState.GetIntroScreen().FirstIntro();
			}
			else
			{
				this.Skip(provider);
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(this._cachedIntroDataList[this._state].Data))
			{
				for (int i = 0; i < this._assets.Count; i++)
				{
					if (this._assets[i] == this._cachedIntroDataList[this._state].Data)
					{
						provider.Portrait.sprite = provider.Sprites[i];
					}
				}
			}
			this.HandleCrush(provider);
			provider.TextBox.text = Translations.GetTranslation(this._lines[this._state].Id, this._lines[this._state].English);
			GameState.Voiceover.Play(this.Girl.GirlName, Voiceover.BundleType.Special, this._lines[this._state].Id, this.VoiceoverDelay);
			this._state++;
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0003709C File Offset: 0x0003529C
	public virtual void HandleCrush(IntroProvider provider)
	{
		this._isCrush = this._cachedIntroDataList[this._state].isCrush;
		if (this._isCrush)
		{
			this._currentTime = 0f;
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.BadReaction);
		}
		provider.Crush.gameObject.SetActive(this._isCrush);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00037104 File Offset: 0x00035304
	public virtual void Update(IntroProvider provider)
	{
		if (this._isCrush)
		{
			if (this._currentTime < 1f)
			{
				float num = Utilities.Easing.ElasticEaseOut(this._currentTime, 0f, 1f, 1f);
				provider.Portrait.transform.localPosition = new Vector3((float)IntroProvider.Shake * Mathf.Sin(20f * this._currentTime) * (1f - num), (float)IntroProvider.YOffset + (float)IntroProvider.Shake * Mathf.Cos(35f * this._currentTime) * (1f - num), 0f);
				provider.Crush.transform.localScale = new Vector3(num, num, 1f);
			}
			else if (this._currentTime + Time.deltaTime > 1f)
			{
				provider.Portrait.transform.localPosition = new Vector3(0f, (float)IntroProvider.YOffset);
				provider.Crush.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			if (this._currentTime > 2f)
			{
				provider.Crush.gameObject.SetActive(false);
			}
			else if (this._currentTime > 1.5f)
			{
				provider.Crush.color = new Color(1f, 1f, 1f, 1f - (this._currentTime - 1.5f) * 2f);
			}
			provider.Crush.transform.localEulerAngles = new Vector3(0f, 0f, this._currentTime * 10f);
		}
		this._currentTime += Time.deltaTime;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x000372D0 File Offset: 0x000354D0
	protected virtual void Skip(IntroProvider provider)
	{
		provider.Skip();
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000372D8 File Offset: 0x000354D8
	public virtual void OnSceneDone()
	{
		GameState.Voiceover.UnloadSpecialBundle(this.Girl.GirlName, false);
		if (this.OnIntroductionDone != null)
		{
			this.OnIntroductionDone();
		}
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00037314 File Offset: 0x00035514
	public virtual void Destroy(IntroProvider provider)
	{
		provider.Sprites = null;
		this._cachedIntroDataList = null;
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x00037324 File Offset: 0x00035524
	public virtual void UpdateTranslation(IntroProvider provider)
	{
		int index = (this._state <= 0) ? this._state : (this._state - 1);
		provider.TextBox.text = Translations.GetTranslation(this._lines[index].Id, this._lines[index].English);
	}

	// Token: 0x04000679 RID: 1657
	protected Color _backgroundColor = new Color(0.13333334f, 0.13333334f, 0.13333334f);

	// Token: 0x0400067A RID: 1658
	protected List<LocalizationContext> _lines = new List<LocalizationContext>();

	// Token: 0x0400067B RID: 1659
	protected List<string> _assets = new List<string>();

	// Token: 0x0400067C RID: 1660
	protected int _state;

	// Token: 0x0400067D RID: 1661
	protected int _finalState;

	// Token: 0x0400067E RID: 1662
	protected float _currentTime;

	// Token: 0x0400067F RID: 1663
	protected bool _isCrush;

	// Token: 0x04000680 RID: 1664
	protected List<GirlModel.IntroData> _cachedIntroDataList;

	// Token: 0x04000681 RID: 1665
	protected Action OnIntroductionDone;

	// Token: 0x04000682 RID: 1666
	protected float VoiceoverDelay = 0.2f;
}
