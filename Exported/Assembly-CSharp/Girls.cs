using System;
using System.Collections;
using System.Collections.Generic;
using SadPanda.Platforms;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009F RID: 159
public class Girls : MonoBehaviour
{
	// Token: 0x06000384 RID: 900 RVA: 0x0001AFA0 File Offset: 0x000191A0
	public void SetSprite(Sprite sprite, Girl girl)
	{
		Transform transform = base.transform.Find("Girl Information/Girl Popup/Girl Popup Pose/Animated");
		SkeletonGraphic component = transform.GetComponent<SkeletonGraphic>();
		transform.gameObject.SetActive(girl.Clothing == Requirement.OutfitType.Animated && component.skeletonDataAsset != null);
		base.transform.Find("Girl Information/Girl Popup/Girl Popup Pose").GetComponent<Image>().enabled = (girl.Clothing != Requirement.OutfitType.Animated);
		if (sprite != null && !this.Image.gameObject.activeSelf)
		{
			this.Image.gameObject.SetActive(true);
		}
		string text = (!(sprite == null)) ? sprite.name : string.Empty;
		if (GameState.NSFW && this.ImageOffsets.ContainsKey(text + "_nsfw"))
		{
			text += "_nsfw";
		}
		if (Girls.CurrentGirl != null && (Girls.CurrentGirl.GirlName == Balance.GirlName.Alpha || Girls.CurrentGirl.GirlName == Balance.GirlName.Bearverly || Girls.CurrentGirl.GirlName == Balance.GirlName.Generica) && Girls.CurrentGirl.Love == 9 && Girls.CurrentGirl.Clothing == Requirement.OutfitType.None)
		{
			text += "2";
		}
		if (!this.ImageOffsets.ContainsKey(text))
		{
			text += ".png";
		}
		if (sprite != null && this.ImageOffsets.ContainsKey(text))
		{
			RectTransform component2 = this.Image.GetComponent<RectTransform>();
			if (sprite.name.Contains("LOTS") && !sprite.name.Contains("bonchovy") && !sprite.name.Contains("spectrum") && !sprite.name.Contains("quillzone") && !sprite.name.Contains("jelle") && !sprite.name.Contains("pamu") && !sprite.name.Contains("luna") && !sprite.name.Contains("bear") && !sprite.name.Contains("charlotte") && !sprite.name.Contains("odango") && !sprite.name.Contains("shibuki") && !sprite.name.Contains("sirina") && !sprite.name.Contains("catara") && !sprite.name.Contains("vellatrix") && !sprite.name.Contains("peanut") && !sprite.name.Contains("roxxy") && !sprite.name.Contains("tessa") && !sprite.name.Contains("claudia") && !sprite.name.Contains("rosa") && !sprite.name.Contains("juliet") && !sprite.name.Contains("wendy") && !sprite.name.Contains("ruri") && !sprite.name.Contains("generica") && !sprite.name.Contains("suzu") && !sprite.name.Contains("lustat") && !sprite.name.Contains("sawyer") && !sprite.name.Contains("explora") && !sprite.name.Contains("esper") && !sprite.name.Contains("renee") && !sprite.name.Contains("mallory") && sprite.name.StartsWith("girl"))
			{
				Girls.CurrentGirl.GetSprite(Girl.SpriteType.LikesYou, new Action<Sprite, Girl>(this.SetSprite));
				this.loverEyes.sprite = sprite;
				this.loverEyes.rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
				this.loverEyes.rectTransform.localPosition = new Vector3((float)this.ImageOffsets[text].LeftOffset, (float)(388 - this.ImageOffsets[text].Width) - sprite.rect.height, 0f);
				this.loverEyes.gameObject.SetActive(true);
			}
			else if (sprite.name.StartsWith("girl") || sprite.name.StartsWith("LINGERIE") || sprite.name.StartsWith("NUDE") || sprite.name.StartsWith("XMAS") || sprite.name.Contains("ayano") || Girls.CurrentGirl.GirlName > Balance.GirlName.QPiddy)
			{
				this.Image.sprite = sprite;
				this.Image.transform.Find("Lover Eyes").gameObject.SetActive(false);
				component2.sizeDelta = new Vector2((float)this.ImageOffsets[text].Width, 388f);
				component2.localPosition = new Vector3((float)(this.leftOffset + this.ImageOffsets[text].LeftOffset), (float)this.topOffset, 0f);
			}
			else
			{
				this.Image.sprite = sprite;
				this.Image.transform.Find("Lover Eyes").gameObject.SetActive(false);
				component2.sizeDelta = new Vector2((float)this.ImageOffsets[text].Width, 388f);
				component2.localPosition = new Vector3((float)(this.leftOffset + this.ImageOffsets[text].LeftOffset - 2), (float)this.topOffset, 0f);
			}
		}
		else if (sprite == null)
		{
			this.Image.color = new Color(1f, 1f, 1f, 0f);
		}
		else
		{
			this.Image.sprite = sprite;
			this.Image.rectTransform.localPosition = new Vector3((float)this.leftOffset, (float)this.topOffset, 0f);
			if (this.lastGirlSprite != null && this.lastGirlSprite != girl)
			{
				this.lastGirlSprite.UnloadAssets();
			}
			this.lastGirlSprite = girl;
		}
		if (sprite != null)
		{
			this.pokeStartX = this.Image.rectTransform.localPosition.x;
		}
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0001B6F4 File Offset: 0x000198F4
	private void Start()
	{
		Girl[] componentsInChildren = base.GetComponentsInChildren<Girl>(true);
		this.ImageOffsets = Universe.ImageOffsets;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
			componentsInChildren[i].transform.Find("Name").gameObject.SetActive(false);
		}
		GameState.UniverseReady += new ReactiveProperty<bool>.Changed(this.LoadAssets);
		Translations.CurrentLanguage += new ReactiveProperty<int>.Changed(this.UpdateLanguage);
		this.loverEyes = this.Image.transform.Find("Lover Eyes").GetComponent<Image>();
		for (int j = 0; j < 4; j++)
		{
			int index = j;
			base.transform.Find("Girl Information/Requirements").GetChild(j).Find("Speed").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.CompleteSpeedRequirement(index);
			});
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001B824 File Offset: 0x00019A24
	private void UpdateLanguage(int newLanguage)
	{
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		Girls.CurrentGirl.ResetRequirements();
		this.SetGirlInternal(Girls.CurrentGirl, false, false, false, true, false, 0L, false, false, true);
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001B864 File Offset: 0x00019A64
	private void OnDestroy()
	{
		GameState.UniverseReady -= new ReactiveProperty<bool>.Changed(this.LoadAssets);
		Translations.CurrentLanguage -= new ReactiveProperty<int>.Changed(this.UpdateLanguage);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001B8A8 File Offset: 0x00019AA8
	private void LoadAssets(bool universeReady)
	{
		if (universeReady)
		{
			GameState.CurrentState.StartCoroutine(this.LoadAssets());
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001B8C4 File Offset: 0x00019AC4
	public void ResetLoveAnimation()
	{
		this.loveAnimation = 1f;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001B8D4 File Offset: 0x00019AD4
	private void Update()
	{
		this.UpdateTalkPokeCooldowns();
		if (this.pokeAnimation < 0.5f)
		{
			if (this.pokeAnimation + Time.deltaTime >= 0.5f)
			{
				this.Image.transform.localPosition = new Vector3(this.pokeStartX, (float)this.topOffset, 0f);
			}
			else
			{
				this.Image.transform.localPosition = new Vector3(this.pokeStartX + 5f * Mathf.Sin(20f * this.pokeAnimation), (float)this.topOffset + this.pokeScale * Mathf.Cos(35f * this.pokeAnimation), 0f);
			}
		}
		if (this.pokeAnimation < 1f)
		{
			if (!this.Poke.gameObject.activeSelf)
			{
				this.Poke.gameObject.SetActive(true);
			}
			if (this.pokeAnimation + Time.deltaTime >= 1f)
			{
				this.Poke.transform.localScale = new Vector3(1f, 1f, 1f);
				this.Image.transform.localPosition = new Vector3(this.pokeStartX, (float)this.topOffset, 0f);
			}
			else
			{
				float num = Utilities.Easing.ElasticEaseOut(this.pokeAnimation, 0f, 1f, 1f);
				this.Poke.transform.localScale = new Vector3(num, num, 0f);
			}
		}
		else if (this.pokeAnimation > 3f && this.pokeAnimation < 3.5f)
		{
			this.Poke.color = new Color(1f, 1f, 1f, 1f - (this.pokeAnimation - 3f) * 2f);
		}
		else if (this.pokeAnimation > 3.5f && this.Poke.gameObject.activeSelf)
		{
			this.Poke.gameObject.SetActive(false);
			this.Poke.color = new Color(1f, 1f, 1f, 1f);
		}
		this.pokeAnimation += Time.deltaTime;
		if (Girls.CurrentGirl != null)
		{
			this.heartTempo = (this.heartTempo + Time.deltaTime * (1f + 1.5f * (float)Girls.CurrentGirl.Love)) % 6.2831855f;
			float num2 = 1f + 0.08f * Mathf.Sin(this.heartTempo);
			this.AffectionPointer.rectTransform.localScale = new Vector3(num2, num2, 1f);
			if (this.loverEyes.gameObject.activeSelf || Girls.CurrentGirl.CurrentSpriteType == Girl.SpriteType.LikesYouLots)
			{
				if (this.loveAnimation > 0f)
				{
					this.loveAnimation -= Time.deltaTime;
				}
				else
				{
					Girls.CurrentGirl.CurrentPose = ((Girls.CurrentGirl.Love != 0) ? Girls.CurrentGirl.GetSprite(Girl.SpriteType.LikesYou) : Girls.CurrentGirl.GetSprite(Girl.SpriteType.Scenario));
					Girls.CurrentGirl.GetSprite(Girls.CurrentGirl.CurrentSpriteType, new Action<Sprite, Girl>(this.SetSprite));
				}
			}
			if (!(this.Image.sprite == null) && this.Image.sprite.GetInstanceID() != 0)
			{
				if (this.Image.color.a == 0f)
				{
					this.Image.color = new Color(1f, 1f, 1f, 1f);
				}
			}
		}
		if (this.fadingImages.Count != 0)
		{
			for (int i = 0; i < this.fadingImages.Count; i++)
			{
				if (this.fadingImages[i].color.a == 0f)
				{
					this.fadingImages[i].transform.parent.Find("Unlock System").GetComponent<ParticleSystem>().Play();
					GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
				}
				if (this.fadingImages[i].color.a + Time.deltaTime > 1f)
				{
					this.fadingImages[i].color = new Color(1f, 1f, 1f, 1f);
					this.fadingImages.RemoveAt(i--);
				}
				else
				{
					this.fadingImages[i].color = new Color(1f, 1f, 1f, this.fadingImages[i].color.a + Time.deltaTime);
				}
			}
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001BDF0 File Offset: 0x00019FF0
	public void SetGirlOutfit()
	{
		this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, false, 0L, true, false, false);
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0001BE14 File Offset: 0x0001A014
	public void SetGirl()
	{
		this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, false, 0L, false, false, false);
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0001BE38 File Offset: 0x0001A038
	public void SetGirl(Girl girl)
	{
		if (girl == null)
		{
			if (!(Girls.CurrentGirl != null))
			{
				return;
			}
			girl = Girls.CurrentGirl;
		}
		GameState.Voiceover.LoadBundle(girl.GirlName);
		if (girl.transform.Find("Notification") != null)
		{
			girl.transform.Find("Notification").gameObject.SetActive(false);
		}
		if (girl.GirlName >= Balance.GirlName.Darya && IntroScreen.TutorialState < IntroScreen.State.JobsActive)
		{
			return;
		}
		if (girl.Requirements != null)
		{
			foreach (Requirement requirement in girl.Requirements)
			{
				requirement.Dirty = true;
			}
		}
		if (Girls.CurrentGirl != null)
		{
			girl.DisplayFollowUp = false;
		}
		if (GameState.CurrentState != null)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.HoverSelect06);
		}
		if (girl.Hearts == 0L && girl.Love == 0)
		{
			if (!Settings.IntrosDisabled)
			{
				this.LaunchIntro(girl, true);
				GameState.CurrentState.CheckCellphoneUnlock();
				return;
			}
			girl.Hearts = 1L;
			girl.SetIcon();
			Album.Add(girl.Data, 0);
			if (girl.GirlName == Balance.GirlName.Ayano)
			{
				Translations.UpdateGirlNames();
			}
			GameState.CurrentState.CheckCellphoneUnlock();
		}
		this.SetGirlInternal(girl, false, false, false, true, false, 0L, false, false, false);
		this.HideInteractions();
		this.loveAnimation = 1f;
		if (global::PlayerPrefs.GetInt("CurrentGirl", 0) != (int)(Girls.CurrentGirl.Data.Id - 1))
		{
			global::PlayerPrefs.SetInt("CurrentGirl", (int)(Girls.CurrentGirl.Data.Id - 1));
			GameState.CurrentState.QueueSave();
		}
		Girl.LastGirl = girl.GirlName;
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600038E RID: 910 RVA: 0x0001C018 File Offset: 0x0001A218
	public int TotalUnlockedGirlCount
	{
		get
		{
			return this.unlockedGirls.Count;
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001C034 File Offset: 0x0001A234
	public IEnumerator LoadAssets()
	{
		this.UnlockGirls();
		Girl[] girls = base.GetComponentsInChildren<Girl>(true);
		if (girls == null || girls.Length == 0)
		{
			yield break;
		}
		foreach (int girl in this.unlockedGirls)
		{
			if (girls[girl].transform.Find("Unlocked") != null)
			{
				girls[girl].transform.Find("Unlocked").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				girls[girl].transform.Find("Name").gameObject.SetActive(true);
				girls[girl].transform.Find("Requirement").gameObject.SetActive(false);
			}
			girls[girl].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
		for (int i = 0; i < this.unlockedGirls.Count; i++)
		{
			int girl2 = this.unlockedGirls[i];
			if (girls[girl2].transform.Find("Unlocked") != null)
			{
				girls[girl2].LoadAssets(girl2 != 0, (int)(girls[girl2].Data.Id - 1) == global::PlayerPrefs.GetInt("CurrentGirl", 0) && IntroScreen.TutorialState != IntroScreen.State.GirlsActive);
			}
		}
		for (int j = 0; j < this.unlockedGirls.Count; j++)
		{
			int girl3 = this.unlockedGirls[j];
			if (girl3 != 0)
			{
				if (girls[girl3].transform.Find("Unlocked") != null)
				{
					girls[girl3].LoadAssets(false, false);
				}
			}
		}
		this._loadedAssets = true;
		yield break;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001C050 File Offset: 0x0001A250
	public void UnlockGirls()
	{
		if (Girl.FindGirl(Balance.GirlName.Cassie) == null)
		{
			return;
		}
		Girl[] componentsInChildren = base.GetComponentsInChildren<Girl>(true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		if (Girls.UnlockedGirlCount != this.unlockedGirlsCount)
		{
			int num = this.unlockedGirlsCount;
			while (num < Girls.UnlockedGirlCount && num < componentsInChildren.Length)
			{
				this.UnlockGirl(num);
				num++;
			}
			for (int i = Girls.UnlockedGirlCount; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
			}
			this.StoreState();
			this.unlockedGirlsCount = Girls.UnlockedGirlCount;
			Kongregate.SubmitStat("UnlockedGirls", (long)this.unlockedGirlsCount);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Darya) == Playfab.PlayfabItems.Darya)
		{
			this.UnlockGirl(19);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.JelleQuillzone) != (Playfab.PlayfabItems)0L || (Girl.FindGirl(Balance.GirlName.Elle).LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None || TaskManager.IsCompletedEventSet(156))
		{
			this.UnlockGirl(20);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.JelleQuillzone) != (Playfab.PlayfabItems)0L || (Girl.FindGirl(Balance.GirlName.Quill).LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None || TaskManager.IsCompletedEventSet(157))
		{
			this.UnlockGirl(21);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.BonchovySpectrum) != (Playfab.PlayfabItems)0L || (Girl.FindGirl(Balance.GirlName.Bonnibel).LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None || TaskManager.IsCompletedEventSet(159))
		{
			this.UnlockGirl(22);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.BonchovySpectrum) != (Playfab.PlayfabItems)0L || (Girl.FindGirl(Balance.GirlName.Iro).LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None || TaskManager.IsCompletedEventSet(158))
		{
			this.UnlockGirl(23);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Charlotte) != (Playfab.PlayfabItems)0L)
		{
			this.UnlockGirl(24);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Odango) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			83,
			122,
			173
		}))
		{
			this.UnlockGirl(25);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Shibuki) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			88,
			137
		}))
		{
			this.UnlockGirl(26);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Sirina) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			92,
			131,
			182
		}))
		{
			this.UnlockGirl(27);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Catara) != (Playfab.PlayfabItems)0L)
		{
			this.UnlockGirl(28);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Vellatrix) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			95,
			125,
			170
		}))
		{
			this.UnlockGirl(29);
		}
		if (Universe.CellphoneGirls != null && Universe.CellphoneGirls.ContainsKey(1))
		{
			string @string = Universe.CellphoneGirls[1].PathPref.GetString(string.Empty);
			if (!string.IsNullOrEmpty(@string) && @string.Contains("`") && @string.Split(new char[]
			{
				'`'
			}).Length > 238)
			{
				this.UnlockGirl(30);
			}
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Roxxy) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			98,
			176
		}))
		{
			this.UnlockGirl(31);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Tessa) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			101,
			134
		}))
		{
			this.UnlockGirl(32);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Claudia) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			104,
			164
		}))
		{
			this.UnlockGirl(33);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Rosa) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			107,
			149
		}))
		{
			this.UnlockGirl(34);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Juliet) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			110,
			153
		}))
		{
			this.UnlockGirl(35);
		}
		if (((Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(113)) && Universe.CellphoneGirls != null && Universe.CellphoneGirls.ContainsKey(2))
		{
			string string2 = Universe.CellphoneGirls[2].PathPref.GetString(string.Empty);
			if (!string.IsNullOrEmpty(string2) && string2.Contains("`") && string2.Split(new char[]
			{
				'`'
			}).Length > 200)
			{
				this.UnlockGirl(36);
			}
		}
		if (((Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(116)) && Universe.CellphoneGirls != null && Universe.CellphoneGirls.ContainsKey(7))
		{
			string string3 = Universe.CellphoneGirls[7].PathPref.GetString(string.Empty);
			if (!string.IsNullOrEmpty(string3) && string3.Contains("`") && string3.Split(new char[]
			{
				'`'
			}).Length > 258)
			{
				this.UnlockGirl(37);
			}
		}
		if (((Playfab.AwardedItems & Playfab.PlayfabItems.Generica) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(119)) && Universe.CellphoneGirls != null && Universe.CellphoneGirls.ContainsKey(3))
		{
			string string4 = Universe.CellphoneGirls[3].PathPref.GetString(string.Empty);
			if (!string.IsNullOrEmpty(string4) && string4.Contains("`") && string4.Split(new char[]
			{
				'`'
			}).Length > 199)
			{
				this.UnlockGirl(38);
			}
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Suzu) != (Playfab.PlayfabItems)0L)
		{
			this.UnlockGirl(39);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Lustat) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
		{
			128,
			167
		}))
		{
			this.UnlockGirl(40);
		}
		if (((Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(140)) && Universe.CellphoneGirls != null && Universe.CellphoneGirls.ContainsKey(10))
		{
			PhoneModel.PhonePathPref pathPref = Universe.CellphoneGirls[10].PathPref;
			if (pathPref != null && pathPref.Pref != null && pathPref.Pref.NumMessages >= 361)
			{
				this.UnlockGirl(41);
			}
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Explora) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(143))
		{
			this.UnlockGirl(42);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Esper) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(146))
		{
			this.UnlockGirl(43);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Renee) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(161))
		{
			PhoneModel.PhonePathPref pathPref2 = Universe.CellphoneGirls[16].PathPref;
			if (pathPref2 != null && pathPref2.Pref != null && pathPref2.Pref.NumMessages >= 381)
			{
				this.UnlockGirl(44);
			}
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Mallory) != (Playfab.PlayfabItems)0L)
		{
			this.UnlockGirl(45);
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.Lake) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(179))
		{
			PhoneModel.PhonePathPref pathPref3 = Universe.CellphoneGirls[11].PathPref;
			if (pathPref3 != null && pathPref3.Pref != null && pathPref3.Pref.NumMessages >= 422)
			{
				this.UnlockGirl(46);
			}
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
	public void LaunchIntro(Girl girl, bool firstMeeting = true)
	{
		IntroProvider cutSceneProvider = GameState.GetCutSceneProvider();
		if (firstMeeting)
		{
			girl.Hearts = 1L;
			if (girl.GirlName == Balance.GirlName.Karma && Girl.FindGirl(Balance.GirlName.Sutra) != null)
			{
				Girl.FindGirl(Balance.GirlName.Sutra).SetHearts(1L, false);
			}
			if (girl.GirlName == Balance.GirlName.Sutra && Girl.FindGirl(Balance.GirlName.Karma) != null)
			{
				Girl.FindGirl(Balance.GirlName.Karma).SetHearts(1L, false);
			}
		}
		if (girl.GirlName == Balance.GirlName.Sutra)
		{
			girl = Girl.FindGirl(Balance.GirlName.Karma);
		}
		Balance.GirlName girlName = girl.GirlName;
		switch (girlName)
		{
		case Balance.GirlName.Cassie:
			cutSceneProvider.Initialize(new CassieIntroduction(girl));
			return;
		case Balance.GirlName.Mio:
			cutSceneProvider.Initialize(new MioIntroduction(girl));
			return;
		default:
			if (girlName == Balance.GirlName.Bearverly)
			{
				cutSceneProvider.Initialize(new GenericPanIntroduction(girl, true));
				return;
			}
			if (girlName != Balance.GirlName.Mallory)
			{
				cutSceneProvider.Initialize(new GenericIntroduction(girl));
				return;
			}
			cutSceneProvider.Initialize(new MalloryIntroduction(girl));
			return;
		case Balance.GirlName.Elle:
			cutSceneProvider.Initialize(new GenericPanIntroduction(girl, false));
			return;
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
	public void UpdateHeartText(bool changeLanguage = false)
	{
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		if (this.lastHeartText == Girls.CurrentGirl.Hearts && this.lastHeartReq == Girls.CurrentGirl.HeartRequirement && !changeLanguage && this.lastHeartGirl == Girls.CurrentGirl.GirlName)
		{
			return;
		}
		this.lastHeartText = Girls.CurrentGirl.Hearts;
		this.lastHeartReq = Girls.CurrentGirl.HeartRequirement;
		this.lastHeartGirl = Girls.CurrentGirl.GirlName;
		if (Girls.CurrentGirl.Love < 9)
		{
			if (Girls.CurrentGirl.HeartRequirement > 100000000L)
			{
				this.HeartValue.text = string.Format("{1}% ({0})", Girls.CurrentGirl.Hearts.ToString("n0"), (Math.Floor((double)Girls.CurrentGirl.Hearts / (double)Girls.CurrentGirl.HeartRequirement * 1000.0) / 10.0).ToString("0.#"));
			}
			else
			{
				this.HeartValue.text = string.Format("{0}/{1}", Girls.CurrentGirl.Hearts.ToString("n0"), Girls.CurrentGirl.HeartRequirement.ToString("n0"));
			}
		}
		else
		{
			this.HeartValue.text = "100" + Translations.GetTranslation("everything_else_3_1", "% complete");
		}
		this.StatBonus.gameObject.SetActive(Skills.SkillLevel[Girls.CurrentGirl.FavoriteSkill] > 0 && Girls.CurrentGirl.Love < 9);
		this.UpdateAffectionPointerPosition(Girls.CurrentGirl);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0001CB8C File Offset: 0x0001AD8C
	private void CheckUnlockGirls()
	{
		if (Girls.UnlockedGirlCount == 1 && Skills.SkillLevel[3] > 0)
		{
			Girls.UnlockedGirlCount = 2;
		}
		if (Girls.UnlockedGirlCount == 2 && Skills.SkillLevel[5] > 1)
		{
			Girls.UnlockedGirlCount = 3;
		}
		if (Girls.UnlockedGirlCount == 3 && Skills.SkillLevel[6] > 1)
		{
			Girls.UnlockedGirlCount = 4;
		}
		if (Girls.UnlockedGirlCount == 4 && Skills.SkillLevel[2] > 5)
		{
			Girls.UnlockedGirlCount = 5;
		}
		if (Girls.UnlockedGirlCount == 5 && Skills.SkillLevel[7] > 5)
		{
			Girls.UnlockedGirlCount = 6;
		}
		if (Girls.UnlockedGirlCount == 6 && Skills.SkillLevel[0] > 9)
		{
			Girls.UnlockedGirlCount = 7;
		}
		if (Girls.UnlockedGirlCount == 7 && Skills.SkillLevel[4] > 11)
		{
			Girls.UnlockedGirlCount = 8;
		}
		if (Girls.UnlockedGirlCount == 8 && Skills.SkillLevel[8] > 13)
		{
			Girls.UnlockedGirlCount = 9;
		}
		if (Girls.UnlockedGirlCount == 9 && Skills.SkillLevel[1] > 16)
		{
			Girls.UnlockedGirlCount = 10;
		}
		if (Girls.UnlockedGirlCount == 10 && GameState.Money.Value > 100000000.0)
		{
			Girls.UnlockedGirlCount = 11;
		}
		if (Girls.UnlockedGirlCount == 11 && Skills.SkillLevel[9] > 24)
		{
			Girls.UnlockedGirlCount = 12;
		}
		if (Girls.UnlockedGirlCount == 12 && Skills.SkillLevel[11] > 27)
		{
			Girls.UnlockedGirlCount = 13;
		}
		if (Girls.UnlockedGirlCount == 13 && (Job2.AvailableJobs & Requirement.JobType.Wizard) != Requirement.JobType.None)
		{
			Girls.UnlockedGirlCount = 14;
		}
		if (Girls.UnlockedGirlCount == 14 && (Job2.AvailableJobs & Requirement.JobType.Wizard) != Requirement.JobType.None)
		{
			for (int i = 0; i < Job2.ActiveJobs.Count; i++)
			{
				Job2 job = Job2.ActiveJobs[i];
				if (job.JobType == Requirement.JobType.Wizard && (job.Experience > 0L || job.Level > 1))
				{
					Girls.UnlockedGirlCount = 15;
				}
			}
		}
		if ((Girls.UnlockedGirlCount == 15 || Girls.UnlockedGirlCount == 16) && Skills.SkillLevel[10] > 54)
		{
			Girls.UnlockedGirlCount = 17;
		}
		if (Girl.FindGirl(Balance.GirlName.Sutra) != null && Girl.FindGirl(Balance.GirlName.Karma) != null && Girl.FindGirl(Balance.GirlName.Sutra).Love == 9 && Girl.FindGirl(Balance.GirlName.Karma).Love == 9)
		{
			Girls.UnlockedGirlCount = 18;
		}
		if (Girl.FindGirl(Balance.GirlName.DarkOne) != null && Girl.FindGirl(Balance.GirlName.DarkOne).Love == 9)
		{
			Girls.UnlockedGirlCount = 19;
		}
		if (this.unlockedGirlsCount != Girls.UnlockedGirlCount)
		{
			this.UnlockGirls();
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001CE74 File Offset: 0x0001B074
	public bool IsUnlocked(Balance.GirlName girl)
	{
		return this.unlockedGirls.Contains((int)girl);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0001CE84 File Offset: 0x0001B084
	private void UnlockGirl(int girl)
	{
		if (this.unlockedGirls.Contains(girl))
		{
			return;
		}
		this.unlockedGirls.Add(girl);
		Girl[] componentsInChildren = base.GetComponentsInChildren<Girl>(true);
		if (!componentsInChildren[girl].gameObject.activeSelf)
		{
			return;
		}
		if (componentsInChildren[girl].GetComponent<Image>().color.a != 1f && componentsInChildren[girl].Love == 0 && componentsInChildren[girl].Hearts == 0L && (componentsInChildren[girl].GirlName <= (Balance.GirlName)this.unlockedGirlsCount || componentsInChildren[girl].GirlName == Balance.GirlName.Darya))
		{
			if (componentsInChildren[girl].GirlName == Balance.GirlName.Ayano)
			{
				Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("achievements_407_0", "You unlocked Generica!"));
			}
			else
			{
				Notifications.AddNotification(Notifications.NotificationType.Message, string.Format(Translations.GetTranslation("everything_else_106_0", "You unlocked {0}"), Translations.TranslateGirlName(componentsInChildren[girl].GirlName)));
			}
			Utilities.SendAnalytic(Utilities.AnalyticType.Unlock, componentsInChildren[girl].GirlName.ToFriendlyString());
		}
		if (componentsInChildren[girl].transform.Find("Unlocked") != null)
		{
			this.fadingImages.Add(componentsInChildren[girl].transform.Find("Unlocked").GetComponent<Image>());
			if (this._loadedAssets)
			{
				componentsInChildren[girl].LoadAssets(false, false);
			}
			componentsInChildren[girl].transform.Find("Name").gameObject.SetActive(true);
			componentsInChildren[girl].transform.Find("Requirement").gameObject.SetActive(false);
			componentsInChildren[girl].transform.Find("Store Button").gameObject.SetActive(false);
			componentsInChildren[girl].transform.Find("Store Requirement").gameObject.SetActive(false);
			if (componentsInChildren[girl].transform.Find("Currently Unavailable") != null)
			{
				componentsInChildren[girl].transform.Find("Currently Unavailable").gameObject.SetActive(false);
			}
		}
		if (GameState.CurrentState != null)
		{
			Utilities.SendGirlUnlockedEvent(componentsInChildren[girl].GirlName);
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
	private void SetGirlInternal(Girl girl, bool poke = false, bool talk = false, bool gift = false, bool greeting = false, bool date = false, long heartPayout = 0L, bool outfit = false, bool followup = false, bool translate = false)
	{
		if (Skills.SkillLevel == null || Girl.ActiveGirls == null || Girl.ActiveGirls.Count == 0)
		{
			return;
		}
		if (girl == null)
		{
			return;
		}
		if ((girl.GirlName == Balance.GirlName.DarkOne || girl.GirlName == Balance.GirlName.QPiddy) && girl.Love == 9)
		{
			return;
		}
		if (Girls.CurrentGirl != null)
		{
			if (Girl.LastGirl != girl.GirlName)
			{
				this.pokeAnimation = 3.5f;
				this.Poke.gameObject.SetActive(false);
				this.InteractionBox.LevelUp = false;
			}
			else
			{
				if (girl.Love == 9 && !poke && !talk && !gift && !date && !outfit && !greeting)
				{
					return;
				}
				if (this.InteractionBox.LevelUp)
				{
					return;
				}
			}
		}
		if (girl.GirlName == Balance.GirlName.Cassie && girl.Love == 0 && IntroScreen.TutorialState == IntroScreen.State.GirlsActive)
		{
			girl.SetHearts(1L, false);
			GameState.GetIntroScreen().CassieIntro();
		}
		this.CheckUnlockGirls();
		if (!date && base.transform.Find("Popups/Date Overlay").gameObject.activeInHierarchy)
		{
			return;
		}
		if (girl == null)
		{
			return;
		}
		bool flag = girl.GirlName >= Balance.GirlName.Charlotte;
		if (this.GiftButton.interactable != ((float)girl.Love >= 2f || flag))
		{
			this.GiftButton.interactable = ((float)girl.Love >= 2f || flag);
		}
		if (this.DateButton.interactable != (float)girl.Love >= 4f)
		{
			this.DateButton.interactable = ((float)girl.Love >= 4f);
		}
		this.UpdateGifts();
		bool flag2 = girl.MeetsRequirements() && girl.Hearts >= girl.HeartRequirement;
		if (girl != null && (girl.Clothing == Requirement.OutfitType.Nude || girl.Clothing == Requirement.OutfitType.Lingerie) && !GameState.NSFW)
		{
			girl.RequestClothing(Requirement.OutfitType.None, null);
			greeting = true;
		}
		if (!poke && !talk && !gift && !date && !outfit && !this.InteractionBox.LevelUp && girl == Girls.CurrentGirl && !translate && !greeting)
		{
			if (girl != null)
			{
				this.UpdateRequirements(girl);
			}
			if (!flag2)
			{
				return;
			}
		}
		Girls.CurrentGirl = girl;
		base.transform.Find("Girl Information/Girl Popup/Orbs").gameObject.SetActive(Girls.CurrentGirl.GirlName == Balance.GirlName.Esper);
		base.transform.Find("Heart System").GetComponent<ParticleSystemRenderer>().material.mainTexture = ((girl.GirlName != Balance.GirlName.Explora) ? ((girl.GirlName != Balance.GirlName.Odango) ? this.PlusHeart : this.PlusRice) : this.PlusPixel);
		Girls.CurrentGirl.ResetRequirements();
		this.AffectionPointer.color = this.AffectionColors[girl.Love];
		this.AffectionText.text = ((Girl.LoveLevel)girl.Love).ToFriendlyString();
		if (!base.gameObject.activeInHierarchy && !greeting)
		{
			return;
		}
		if (!flag2 || girl.Love == 9)
		{
			if (poke)
			{
				this.InteractionBox.Init(girl, girl.Poke(), false, 0L, false, "Close");
			}
			else if (talk)
			{
				int num = 0;
				for (int i = 0; i < Girl.ActiveGirls.Count; i++)
				{
					if (Girl.ActiveGirls[i] != null && Girl.ActiveGirls[i].Love >= 9)
					{
						num++;
					}
				}
				int num2 = Skills.SkillLevel[Girls.CurrentGirl.FavoriteSkill];
				double num3 = Math.Pow((double)(20 + num2 * num2), 1.0 + (double)num / 6.5);
				long heartPayout2 = (num3 <= 9.223372036854776E+18) ? ((long)num3) : long.MaxValue;
				this.InteractionBox.Init(girl, girl.Talk(), false, heartPayout2, false, "Close");
			}
			else if (gift)
			{
				this.InteractionBox.Init(girl, girl.Gift(), false, heartPayout, false, "Close");
			}
			else if (date)
			{
				this.InteractionBox.Init(girl, girl.Date(), false, heartPayout, false, "Close");
			}
			else if (outfit && Girls.CurrentGirl.PendingClothing != Requirement.OutfitType.None)
			{
				this.InteractionBox.Init(girl, girl.Gift(Girls.CurrentGirl.PendingClothing), false, 0L, false, "Close");
			}
			else if (greeting)
			{
				this.InteractionBox.Init(girl, girl.Greeting(), true, 0L, false, "Close");
			}
		}
		else if (poke)
		{
			this.pokeAnimation = 3.5f;
			this.Poke.gameObject.SetActive(false);
		}
		this.UpdateHeartText(false);
		Girl.SpriteType currentSpriteType = girl.CurrentSpriteType;
		if (girl.Love != 9 && girl.MeetsRequirements() && girl.Hearts >= girl.HeartRequirement && !GameState.CurrentState.transform.Find("Popups/Girl Introduction").gameObject.activeInHierarchy && !GameState.CurrentState.transform.Find("Popups/Final Outro").gameObject.activeInHierarchy && !this.pendingOutro)
		{
			if (!this.InteractionBox.LevelUp || followup)
			{
				string text;
				string text2;
				string text3;
				girl.GetText(out text, out text2, out text3);
				InteractionBox interactionBox = this.InteractionBox;
				string buttonText = text2;
				interactionBox.Init(Girls.CurrentGirl, text, false, 0L, true, buttonText);
			}
		}
		else if (girl.Love == 9 && greeting)
		{
			girl.GetSprite(Girl.SpriteType.LikesYouLots);
		}
		if ((poke || gift || date || talk) && currentSpriteType != girl.CurrentSpriteType)
		{
			girl.CurrentSpriteType = currentSpriteType;
		}
		if (greeting || outfit || poke || talk || gift || date || (flag2 && girl.Love != 9))
		{
			if (outfit && girl.CurrentSpriteType == Girl.SpriteType.LikesYouLots)
			{
				girl.GetSprite(Girl.SpriteType.LikesYou, new Action<Sprite, Girl>(this.SetSprite));
			}
			else
			{
				girl.GetSprite((girl.Love != 0) ? girl.CurrentSpriteType : Girl.SpriteType.Scenario, new Action<Sprite, Girl>(this.SetSprite));
			}
		}
		this.GirlInformation.SetActive(true);
		this.UpdateRequirements(girl);
		if (girl.MeetsRequirements() && Girls.CurrentGirl.HeartRequirement != Girls.CurrentGirl.Hearts && !this.IntroVisible())
		{
			GameState.GetIntroScreen().AffectionStart();
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0001D820 File Offset: 0x0001BA20
	private bool IntroVisible()
	{
		for (int i = 0; i < GameState.CurrentState.transform.childCount; i++)
		{
			if (GameState.CurrentState.transform.GetChild(i).name.Contains("Intro") && GameState.CurrentState.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0001D894 File Offset: 0x0001BA94
	private void UpdateAffectionPointerPosition(Girl girl)
	{
		float num = Math.Min(1f, ((float)girl.Love + Mathf.Min(1f, (float)girl.Hearts / (float)girl.HeartRequirement)) / 9f);
		this.AffectionPointer.rectTransform.localPosition = new Vector3(-210f + num * 226f, 79f, 0f);
		this.AffectionText.rectTransform.localPosition = ((Translations.CurrentLanguage.Value != 8) ? new Vector3(-210f + num * 226f, 104f, 0f) : new Vector3(-210f + num * 206f, 104f, 0f));
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001D95C File Offset: 0x0001BB5C
	public void UpdateSpeedRequirements()
	{
		if (!this.IsUnlocked(Balance.GirlName.Catara) && !GameState.DebugSpeedDating)
		{
			return;
		}
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		if (Girls.CurrentGirl.CachedLevel == null)
		{
			return;
		}
		for (int i = 0; i < Girls.CurrentGirl.CachedLevel.Requirements.Length; i++)
		{
			GirlModel.GirlRequirement girlRequirement = Girls.CurrentGirl.CachedLevel.Requirements[i];
			Button component = this.Requirements[i].transform.Find("Speed").GetComponent<Button>();
			if (girlRequirement.RequirementType == Requirement.RequirementType.Date)
			{
				double num = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.DateCount[i - 1]) * (double)Universe.Dates[girlRequirement.Id].Price;
				long num2 = (num <= 9.223372036854776E+18) ? ((long)num) : long.MaxValue;
				component.gameObject.SetActive(num2 > 0L);
				component.interactable = (GameState.Money.Value > (double)num2 && num2 > 0L && FreeTime.Free >= Universe.Dates[girlRequirement.Id].TimeBlocks);
			}
			else if (girlRequirement.RequirementType == Requirement.RequirementType.Gift)
			{
				double num3 = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.GiftCount[i - 1]) * (double)Universe.Gifts[girlRequirement.Id].Price;
				long num4 = (num3 <= 9.223372036854776E+18) ? ((long)num3) : long.MaxValue;
				component.gameObject.SetActive(num4 > 0L);
				component.interactable = (GameState.Money.Value > (double)num4 && num4 > 0L);
			}
			else
			{
				component.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001DB68 File Offset: 0x0001BD68
	private void CompleteSpeedRequirement(int index)
	{
		if (!this.IsUnlocked(Balance.GirlName.Catara) && !GameState.DebugSpeedDating)
		{
			return;
		}
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		GirlModel.GirlRequirement girlRequirement = Girls.CurrentGirl.CachedLevel.Requirements[index];
		Button component = this.Requirements[index].transform.Find("Speed").GetComponent<Button>();
		if (girlRequirement.RequirementType == Requirement.RequirementType.Date)
		{
			double num = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.DateCount[index - 1]) * (double)Universe.Dates[girlRequirement.Id].Price;
			long num2 = (num <= 9.223372036854776E+18) ? ((long)num) : long.MaxValue;
			double num3 = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.DateCount[index - 1]) * (double)Universe.Dates[girlRequirement.Id].Hearts;
			long num4 = (num3 <= 9.223372036854776E+18) ? ((long)num3) : long.MaxValue;
			GameState.Money.Value -= (double)num2;
			GameState.DateCount += (int)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.DateCount[index - 1]);
			Girls.CurrentGirl.DateCount[index - 1] = (int)girlRequirement.Quantity;
			Requirement.DateType dateType = Universe.Dates[girlRequirement.Id].DateType;
			Girls.CurrentGirl.GiveDate(dateType, 0);
			Girls.CurrentGirl.FinishDate();
			long heartPayout = num4;
			this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, true, heartPayout, false, false, false);
			Girls.CurrentGirl.StoreState();
			GameState.CurrentState.QueueSave();
			if (!Settings.PopupsDisabled)
			{
				GameState.CurrentState.transform.Find("Popups/Memory Album").GetComponent<Album>().InitDatePopup(Girls.CurrentGirl, Requirement.IndexFromDateType(dateType));
			}
		}
		else if (girlRequirement.RequirementType == Requirement.RequirementType.Gift)
		{
			double num5 = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.GiftCount[index - 1]) * (double)Universe.Gifts[girlRequirement.Id].Price;
			long num6 = (num5 <= 9.223372036854776E+18) ? ((long)num5) : long.MaxValue;
			double num7 = (double)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.GiftCount[index - 1]) * (double)Universe.Gifts[girlRequirement.Id].GetHearts(Girls.CurrentGirl.GirlName);
			long num8 = (num7 <= 9.223372036854776E+18) ? ((long)num7) : long.MaxValue;
			GameState.Money.Value -= (double)num6;
			GameState.GiftCount += (int)Math.Max(0L, girlRequirement.Quantity - (long)Girls.CurrentGirl.GiftCount[index - 1]);
			Girls.CurrentGirl.GiftCount[index - 1] = (int)girlRequirement.Quantity;
			Girls.CurrentGirl.GiveGift(Requirement.GiftFromId(girlRequirement.Id), 0);
			long heartPayout = num8;
			this.SetGirlInternal(Girls.CurrentGirl, false, false, true, false, false, heartPayout, false, false, false);
			Girls.CurrentGirl.StoreState();
			GameState.CurrentState.QueueSave();
		}
		component.interactable = false;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
	public void UpdateRequirements(Girl girl)
	{
		this.UpdateSpeedRequirements();
		if (!base.transform.Find("Girl Information/Requirements").gameObject.activeInHierarchy)
		{
			return;
		}
		if (girl.Love < 9)
		{
			this.Requirements[0].SetActive(true);
			if (this.lastHeartRequirement != girl.HeartRequirement)
			{
				this.Requirements[0].GetComponentInChildren<Text>().text = string.Format("{0}", girl.HeartRequirement.ToString("n0"));
				this.lastHeartRequirement = girl.HeartRequirement;
			}
			this.Requirements[0].transform.Find("Requirement Check").gameObject.SetActive(girl.Hearts >= girl.HeartRequirement);
			this.Requirements[0].transform.Find("Requirement Icon").gameObject.GetComponent<Image>().sprite = this.HeartIcon;
			if (girl.Requirements != null)
			{
				for (int i = 0; i < girl.Requirements.Length; i++)
				{
					if (girl.Requirements[i].Dirty)
					{
						this.Requirements[i + 1].SetActive(true);
						this.Requirements[i + 1].GetComponentInChildren<Text>().text = girl.Requirements[i].Text;
						this.Requirements[i + 1].transform.Find("Requirement Icon").gameObject.GetComponent<Image>().sprite = this.GetRequirementSprite(girl.Requirements[i]);
					}
					this.Requirements[i + 1].transform.Find("Requirement Check").gameObject.SetActive(girl.Requirements[i].UpdateRequirement());
				}
				for (int j = girl.Requirements.Length + 1; j < this.Requirements.Length; j++)
				{
					this.Requirements[j].gameObject.SetActive(false);
				}
			}
			else
			{
				for (int k = 1; k < this.Requirements.Length; k++)
				{
					this.Requirements[k].gameObject.SetActive(false);
				}
			}
			this.PurchasePhoneFling.SetActive(false);
		}
		else if (girl.Love == 9)
		{
			this.lastHeartRequirement = long.MaxValue;
			this.Requirements[0].SetActive(true);
			this.Requirements[0].GetComponentInChildren<Text>().text = Translations.GetTranslation("everything_else_7_0", "You did it!");
			this.Requirements[0].transform.Find("Requirement Check").gameObject.SetActive(true);
			this.Requirements[0].transform.Find("Requirement Icon").gameObject.GetComponent<Image>().sprite = this.HeartIcon;
			for (int l = 1; l < this.Requirements.Length; l++)
			{
				this.Requirements[l].gameObject.SetActive(false);
			}
			if ((girl.GirlName == Balance.GirlName.Cassie && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Cassie) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Mio && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Mio) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Quill && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Quill) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Elle && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Elle) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Iro && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Iro) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Bonnibel && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bonnibel) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Fumi && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Fumi) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Bearverly && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bearverly) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Nina && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Nina) == (Playfab.PhoneFlingPurchases)0L) || (girl.GirlName == Balance.GirlName.Alpha && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Alpha) == (Playfab.PhoneFlingPurchases)0L))
			{
				this.PurchasePhoneFling.transform.Find("Text").GetComponent<Text>().text = string.Format("Now that you've reached Lover level you can unlock {0}'s Phone Fling conversation!", Translations.TranslateGirlName(Girls.CurrentGirl.GirlName));
				this.PurchasePhoneFling.SetActive(true);
			}
			else
			{
				this.PurchasePhoneFling.SetActive(false);
			}
		}
		else
		{
			for (int m = 0; m < this.Requirements.Length; m++)
			{
				this.Requirements[m].gameObject.SetActive(false);
			}
			this.PurchasePhoneFling.SetActive(false);
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0001E39C File Offset: 0x0001C59C
	public void OnEnable()
	{
		if (Girls.CurrentGirl != null)
		{
			this.UpdateRequirements(Girls.CurrentGirl);
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001E3BC File Offset: 0x0001C5BC
	public void ScrollToPF()
	{
		if (Girls.CurrentGirl.GirlName == Balance.GirlName.Cassie)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(952f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Mio)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1012f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Quill)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1072f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Elle)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1132f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Iro)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1192f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Bonnibel)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1252f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Fumi)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1312f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Bearverly)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1372f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Nina)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1432f));
		}
		else if (Girls.CurrentGirl.GirlName == Balance.GirlName.Alpha)
		{
			base.StartCoroutine(this.OpenCellphoneAndScroll(1492f));
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001E54C File Offset: 0x0001C74C
	private IEnumerator OpenCellphoneAndScroll(float y)
	{
		Transform cellphone = GameState.CurrentState.transform.Find("Popups/Cellphone");
		Transform contactsList = cellphone.Find("Phone UI/Contacts/List/Content");
		cellphone.gameObject.SetActive(true);
		while (contactsList.childCount < 23)
		{
			yield return null;
		}
		contactsList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, y, 0f);
		yield break;
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001E570 File Offset: 0x0001C770
	public void UpdateTalkPokeCooldowns()
	{
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		Transform transform = base.transform.Find("Girl Information/Interaction Buttons");
		if (Girls.CurrentGirl != Girls.PreviousGirl)
		{
			Girls.CurrentGirl.ResetRequirements();
			Girls.PreviousGirl = Girls.CurrentGirl;
		}
		this.DoCooldown(transform.Find("Talk Button/Text").GetComponent<Text>(), Translations.TranslateTalkText(Girls.CurrentGirl.Love), Girls.CurrentGirl.TalkCooldown, Girls.CurrentGirl.TalkCooldownLength);
		this.DoCooldown(transform.Find("Gift Button/Text").GetComponent<Text>(), Translations.GetTranslation("everything_else_9_1", "Gift"), Girls.CurrentGirl.GiftCooldown, Girls.CurrentGirl.GiftCooldownLength);
		this.DoCooldown(transform.Find("Date Button/Text").GetComponent<Text>(), Translations.GetTranslation("everything_else_9_2", "Date"), Girls.CurrentGirl.DateCooldown, Girls.CurrentGirl.DateCooldownLength);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001E674 File Offset: 0x0001C874
	private void DoCooldown(Text buttonText, string defaultText, float cooldown, float cooldownLength)
	{
		Image component = buttonText.transform.parent.Find("Cooldown").GetComponent<Image>();
		string text = defaultText;
		if (cooldown > 0f)
		{
			float num = cooldown / (GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier);
			if (num > 60f)
			{
				text = string.Format("{0}:{1}s", Mathf.Floor(num / 60f).ToString("n0"), (num % 60f).ToString("n0"));
			}
			else
			{
				text = string.Format("{0}s", num.ToString("0.0"));
			}
			component.fillAmount = cooldown / cooldownLength;
		}
		else if (component.fillAmount != 0f)
		{
			component.fillAmount = 0f;
		}
		if (buttonText.text != text)
		{
			buttonText.text = text;
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001E764 File Offset: 0x0001C964
	public void OnClickAccept()
	{
		if (Girls.CurrentGirl.MeetsRequirements() && (Girls.CurrentGirl.Hearts >= Girls.CurrentGirl.HeartRequirement || Girls.CurrentGirl.Love == 9))
		{
			if (!Girls.CurrentGirl.DisplayFollowUp)
			{
				Girls.CurrentGirl.DisplayFollowUp = true;
				this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, false, 0L, false, true, false);
				return;
			}
			Girls.CurrentGirl.DisplayFollowUp = false;
			if (Girls.CurrentGirl.Love == 8)
			{
				if (GameState.CurrentState != null)
				{
					Utilities.SendGirlLoverEvent(Girls.CurrentGirl.GirlName);
				}
				if (Girls.CurrentGirl.GirlName == Balance.GirlName.QPiddy)
				{
					this.pendingOutro = true;
					if (!Settings.PopupsDisabled)
					{
						if (GameState.NSFW)
						{
							IntroProvider cutSceneProvider = GameState.GetCutSceneProvider();
							FinalInteraction finalInteraction = new FinalInteraction(Girls.CurrentGirl, Girls.CurrentGirl.Data.SexyDataList);
							finalInteraction.SetOnIntroductionDoneCallback(new Action(cutSceneProvider.LaunchOutro));
							cutSceneProvider.Initialize(finalInteraction);
						}
						else
						{
							this.KissPopup.SetOnIntroductionDoneCallback(new Action(GameState.GetCutSceneProvider().LaunchOutro));
							GameState.CurrentState.StartCoroutine(this.KissPopup.InitAsync(Girls.CurrentGirl, Girl.SpriteType.Lover));
						}
					}
					else
					{
						GameState.GetCutSceneProvider().LaunchOutro();
					}
					Album.Add(Girls.CurrentGirl.Data, 3);
					return;
				}
				GameState.CurrentState.QueueSave();
				Girls.CurrentGirl.AdvanceRelationship();
				this.HideInteractions();
				if (Girls.CurrentGirl.Love == 9 && Girls.CurrentGirl.GirlName == Balance.GirlName.Bearverly)
				{
					GameState.Voiceover.LoadBundle(Balance.GirlName.Bearverly);
				}
				if (Girls.CurrentGirl.Love == 9 && (Girls.CurrentGirl.GirlName == Balance.GirlName.Alpha || Girls.CurrentGirl.GirlName == Balance.GirlName.Bearverly))
				{
					Girls.CurrentGirl.LoadAssets(true, false);
				}
			}
			else
			{
				Girls.CurrentGirl.AdvanceRelationship();
			}
			if (Girls.CurrentGirl.GirlName == Balance.GirlName.Cassie && IntroScreen.TutorialState < IntroScreen.State.HobbiesActive)
			{
				GameState.CurrentState.transform.Find("Popups/Intro Tutorial").GetComponent<IntroScreen>().HobbyStart();
			}
		}
		GameState.CurrentState.CheckCellphoneUnlock();
		Utilities.SendAnalytic(Utilities.AnalyticType.Girl, string.Format("{0}{1}", Girls.CurrentGirl.GirlName.ToFriendlyString(), Girls.CurrentGirl.Love.ToString()));
		Girls.CurrentGirl.Hearts = 0L;
		this.SetGirlInternal(Girls.CurrentGirl, false, false, false, true, false, 0L, false, false, false);
		if (Girls.CurrentGirl.Love == 9)
		{
			Album.Add(Girls.CurrentGirl.Data, 3);
			if (GameState.NSFW)
			{
				if (!Settings.PopupsDisabled)
				{
					GameState.GetCutSceneProvider().Initialize(new FinalInteraction(Girls.CurrentGirl, Girls.CurrentGirl.Data.SexyDataList));
				}
			}
			else if (!Settings.PopupsDisabled)
			{
				GameState.CurrentState.StartCoroutine(this.KissPopup.InitAsync(Girls.CurrentGirl, Girl.SpriteType.Lover));
			}
			GameState.GetIntroScreen().FinalImageStart();
			if (Girls.CurrentGirl.GirlName == Balance.GirlName.DarkOne && (!GameState.NSFW || Settings.PopupsDisabled))
			{
				this.SetGirl(Girl.FindGirl(Balance.GirlName.Sutra));
			}
		}
		else if (Girls.CurrentGirl.Love == 7)
		{
			Album.Add(Girls.CurrentGirl.Data, 2);
			if (!Settings.PopupsDisabled)
			{
				GameState.CurrentState.StartCoroutine(this.KissPopup.InitAsync(Girls.CurrentGirl, Girl.SpriteType.Kiss));
			}
		}
		else if (Girls.CurrentGirl.Love == 4)
		{
			Album.Add(Girls.CurrentGirl.Data, 1);
			if (!Settings.PopupsDisabled)
			{
				GameState.CurrentState.StartCoroutine(this.KissPopup.InitAsync(Girls.CurrentGirl, Girl.SpriteType.Friendship));
			}
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001EB60 File Offset: 0x0001CD60
	private Sprite GetRequirementSprite(Requirement requirement)
	{
		if (requirement.Type == Requirement.RequirementType.Hobby)
		{
			return this.HobbyIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Job)
		{
			return this.JobIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Money)
		{
			return this.MoneyIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Skill)
		{
			return this.StatsIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Time)
		{
			return this.TimeIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Heart)
		{
			return this.HeartIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Diamond)
		{
			return this.DiamondIcon;
		}
		if (requirement.Type == Requirement.RequirementType.Gift)
		{
			int gift = (int)requirement.Gift;
			if (Girls.CurrentGirl.GiftCount[requirement.Index] == 0 && Girls.CurrentGirl.GirlName == Balance.GirlName.Esper)
			{
				return this.UnknownIcon;
			}
			for (int i = 0; i < 32; i++)
			{
				if ((gift >> i & 1) != 0)
				{
					switch (Girls.CurrentGirl.GirlName)
					{
					case Balance.GirlName.Explora:
						return Universe.Gifts[(short)(i + 1)].ExploraSprite;
					case Balance.GirlName.Mallory:
						return Universe.Gifts[(short)(i + 1)].MallorySprite;
					}
					return Universe.Gifts[(short)(i + 1)].Sprite;
				}
			}
		}
		else
		{
			if (requirement.Type == Requirement.RequirementType.TotalGifts)
			{
				return Universe.Gifts[6].Sprite;
			}
			if (requirement.Type == Requirement.RequirementType.Affection)
			{
				return this.StatsIcon;
			}
			if (requirement.Type == Requirement.RequirementType.Date)
			{
				int date = (int)requirement.Date;
				if (Girls.CurrentGirl.DateCount[requirement.Index] == 0 && Girls.CurrentGirl.GirlName == Balance.GirlName.Esper)
				{
					return this.UnknownIcon;
				}
				for (int j = 0; j < 32; j++)
				{
					if ((date >> j & 1) != 0)
					{
						return this.DateIcons[j];
					}
				}
			}
			else
			{
				if (requirement.Type == Requirement.RequirementType.TotalDates)
				{
					return this.DateIcons[4];
				}
				if (requirement.Type == Requirement.RequirementType.Prestige || requirement.Type == Requirement.RequirementType.PrestigeConsume)
				{
					return this.PrestigeIcon;
				}
				if (requirement.Type == Requirement.RequirementType.Achievement)
				{
					return this.AchievementIcon;
				}
				if (requirement.Type == Requirement.RequirementType.Album)
				{
					return this.AlbumIcon;
				}
				if (requirement.Type == Requirement.RequirementType.GirlsAtLover)
				{
					return this.LoverIcon;
				}
			}
		}
		return this.TimeIcon;
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001EDE4 File Offset: 0x0001CFE4
	public void BumpAffection()
	{
		if (Girls.CurrentGirl != null && Girls.CurrentGirl.TalkCooldown <= 0f)
		{
			this.SetGirlInternal(Girls.CurrentGirl, false, true, false, false, false, 0L, false, false, false);
			Girl currentGirl = Girls.CurrentGirl;
			float num = 10f;
			Girls.CurrentGirl.TalkCooldown = num;
			currentGirl.TalkCooldownLength = num;
			GameState.RegisterUpdate(Girls.CurrentGirl);
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Talk);
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001EE6C File Offset: 0x0001D06C
	public void PokeGirl()
	{
		if (Girls.CurrentGirl != null && Girls.CurrentGirl.PokeCooldown <= 0f)
		{
			this.SetGirlInternal(Girls.CurrentGirl, true, false, false, false, false, 0L, false, false, false);
			Girl currentGirl = Girls.CurrentGirl;
			float num = 3f;
			Girls.CurrentGirl.PokeCooldown = num;
			currentGirl.PokeCooldownLength = num;
			GameState.RegisterUpdate(Girls.CurrentGirl);
			GameState.PokeCount++;
			Kongregate.SubmitStat("PokeCount", (long)GameState.PokeCount);
			this.pokeAnimation = 0f;
			this.Poke.color = new Color(1f, 1f, 1f, 1f);
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Tickle);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001EF34 File Offset: 0x0001D134
	public void GiftGirl(long heartPayout, bool outfit)
	{
		if (Girls.CurrentGirl != null && Girls.CurrentGirl.GiftCooldown <= 0f)
		{
			if (outfit)
			{
				this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, false, heartPayout, true, false, false);
			}
			else
			{
				this.SetGirlInternal(Girls.CurrentGirl, false, false, true, false, false, heartPayout, false, false, false);
			}
			if (!outfit)
			{
				Girl currentGirl = Girls.CurrentGirl;
				float num = 30f;
				Girls.CurrentGirl.GiftCooldown = num;
				currentGirl.GiftCooldownLength = num;
			}
			GameState.RegisterUpdate(Girls.CurrentGirl);
			this.SetAffectionTab(true);
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001EFDC File Offset: 0x0001D1DC
	public void DateGirl(Date date)
	{
		if (Girls.CurrentGirl != null && Girls.CurrentGirl.DateCooldown <= 0f)
		{
			long hearts = date.Data.Hearts;
			this.SetGirlInternal(Girls.CurrentGirl, false, false, false, false, true, hearts, false, false, false);
			GameState.RegisterUpdate(Girls.CurrentGirl);
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001F044 File Offset: 0x0001D244
	private void SetAffectionTab(bool visible)
	{
		base.transform.Find("Girl Information/Affection Tab").gameObject.SetActive(visible);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001F06C File Offset: 0x0001D26C
	public void HideInteractions()
	{
		this.StatInteraction.SetActive(false);
		this.DateInteraction.SetActive(false);
		this.GiftInteraction.SetActive(false);
		this.SetAffectionTab(true);
		this.StatButton.transform.Find("Close").gameObject.SetActive(false);
		this.DateButton.transform.Find("Close").gameObject.SetActive(false);
		this.GiftButton.transform.Find("Close").gameObject.SetActive(false);
		this.StatButton.transform.Find("Text").gameObject.SetActive(true);
		this.DateButton.transform.Find("Text").gameObject.SetActive(true);
		this.GiftButton.transform.Find("Text").gameObject.SetActive(true);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001F164 File Offset: 0x0001D364
	public void ClickDate()
	{
		if (Girls.CurrentGirl.DateCooldown > 0f)
		{
			return;
		}
		bool activeInHierarchy = this.DateInteraction.activeInHierarchy;
		this.HideInteractions();
		this.DateButton.transform.Find("Text").gameObject.SetActive(activeInHierarchy);
		this.DateButton.transform.Find("Close").gameObject.SetActive(!activeInHierarchy);
		if (activeInHierarchy)
		{
			base.transform.Find("Girl Information/Requirements").gameObject.SetActive(true);
		}
		else
		{
			this.DateInteraction.gameObject.SetActive(true);
			this.SetAffectionTab(false);
			this.UpdateGifts();
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001F220 File Offset: 0x0001D420
	public void ClickGift()
	{
		if (Girls.CurrentGirl.GiftCooldown > 0f)
		{
			return;
		}
		bool activeInHierarchy = this.GiftInteraction.activeInHierarchy;
		this.HideInteractions();
		this.GiftButton.transform.Find("Text").gameObject.SetActive(activeInHierarchy);
		this.GiftButton.transform.Find("Close").gameObject.SetActive(!activeInHierarchy);
		if (activeInHierarchy)
		{
			base.transform.Find("Girl Information/Requirements").gameObject.SetActive(true);
		}
		else
		{
			this.GiftInteraction.GetComponent<Gifting>().Init(0);
			this.SetAffectionTab(false);
			this.UpdateGifts();
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001F2DC File Offset: 0x0001D4DC
	public void ClickStats()
	{
		bool activeInHierarchy = this.StatInteraction.activeInHierarchy;
		this.HideInteractions();
		this.StatButton.transform.Find("Text").gameObject.SetActive(activeInHierarchy);
		this.StatButton.transform.Find("Close").gameObject.SetActive(!activeInHierarchy);
		if (activeInHierarchy)
		{
			base.transform.Find("Girl Information/Requirements").gameObject.SetActive(true);
		}
		else
		{
			this.StatInteraction.GetComponent<GirlStats>().Init(Girls.CurrentGirl.Data);
			this.SetAffectionTab(false);
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001F388 File Offset: 0x0001D588
	public void ClickGirl()
	{
		if (Girls.CurrentGirl != null)
		{
			if (Girls.CurrentGirl.Hearts == Girls.CurrentGirl.HeartRequirement || Girls.CurrentGirl.Love == 9)
			{
				this.PokeGirl();
			}
			else if (Girls.CurrentGirl.Hearts + (long)Girls.UnlockedGirlCount >= Girls.CurrentGirl.HeartRequirement)
			{
				Girls.CurrentGirl.Hearts = Girls.CurrentGirl.HeartRequirement;
				this.SetGirl();
				GameState.CurrentState.QueueSave();
			}
			else
			{
				Girls.CurrentGirl.Hearts += (long)Girls.UnlockedGirlCount;
				GameState.CurrentState.QueueSave();
			}
			this.UpdateHeartText(false);
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001F44C File Offset: 0x0001D64C
	public void UpdateGifts()
	{
		this.GiftInteraction.GetComponent<Gifting>().UpdateGifts();
		if (this.DateInteraction.activeInHierarchy)
		{
			Date[] componentsInChildren = this.DateInteraction.GetComponentsInChildren<Date>();
			foreach (Date date in componentsInChildren)
			{
				date.GetComponent<Button>().interactable = ((double)date.Data.Price <= GameState.Money.Value || date.Data.DiamondCost <= GameState.Diamonds.Value);
			}
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001F4E4 File Offset: 0x0001D6E4
	public void HeartParticles(int count)
	{
		if (!Settings.ParticlesDisabled && base.transform.Find("Heart System") != null)
		{
			int count2 = Math.Min(25, count);
			base.transform.Find("Heart System").GetComponent<ParticleSystem>().Emit(count2);
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001F53C File Offset: 0x0001D73C
	public void StoreState()
	{
		global::PlayerPrefs.SetInt("GirlsUnlocked", Math.Max(1, Girls.UnlockedGirlCount));
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001F554 File Offset: 0x0001D754
	public void LoadState()
	{
		Girls.UnlockedGirlCount = Math.Max(1, global::PlayerPrefs.GetInt("GirlsUnlocked", 1));
		this.CheckUnlockGirls();
	}

	// Token: 0x040003B1 RID: 945
	public Image AffectionPointer;

	// Token: 0x040003B2 RID: 946
	public Text AffectionText;

	// Token: 0x040003B3 RID: 947
	private Color[] AffectionColors = new Color[]
	{
		new Color(0.09803922f, 0.2f, 0.59607846f),
		new Color(0.2f, 0.4f, 1f),
		new Color(0.4f, 1f, 1f),
		new Color(0.4f, 1f, 0f),
		new Color(1f, 0.8f, 0f),
		new Color(1f, 0.4f, 0f),
		new Color(1f, 0f, 0f),
		new Color(1f, 0.19607843f, 0.28627452f),
		new Color(1f, 0.39215687f, 0.5764706f),
		new Color(1f, 0.39215687f, 0.5764706f)
	};

	// Token: 0x040003B4 RID: 948
	public Image Image;

	// Token: 0x040003B5 RID: 949
	public Text HeartValue;

	// Token: 0x040003B6 RID: 950
	public Text StatBonus;

	// Token: 0x040003B7 RID: 951
	public InteractionBox InteractionBox;

	// Token: 0x040003B8 RID: 952
	public GameObject GirlInformation;

	// Token: 0x040003B9 RID: 953
	public GameObject StatInteraction;

	// Token: 0x040003BA RID: 954
	public GameObject GiftInteraction;

	// Token: 0x040003BB RID: 955
	public GameObject DateInteraction;

	// Token: 0x040003BC RID: 956
	public Image Poke;

	// Token: 0x040003BD RID: 957
	public static int UnlockedGirlCount = 1;

	// Token: 0x040003BE RID: 958
	public Sprite HobbyIcon;

	// Token: 0x040003BF RID: 959
	public Sprite TimeIcon;

	// Token: 0x040003C0 RID: 960
	public Sprite JobIcon;

	// Token: 0x040003C1 RID: 961
	public Sprite MoneyIcon;

	// Token: 0x040003C2 RID: 962
	public Sprite StatsIcon;

	// Token: 0x040003C3 RID: 963
	public Sprite HeartIcon;

	// Token: 0x040003C4 RID: 964
	public Sprite DiamondIcon;

	// Token: 0x040003C5 RID: 965
	public Sprite PrestigeIcon;

	// Token: 0x040003C6 RID: 966
	public Sprite AchievementIcon;

	// Token: 0x040003C7 RID: 967
	public Sprite AlbumIcon;

	// Token: 0x040003C8 RID: 968
	public Sprite LoverIcon;

	// Token: 0x040003C9 RID: 969
	public Sprite UnknownIcon;

	// Token: 0x040003CA RID: 970
	public Texture PlusHeart;

	// Token: 0x040003CB RID: 971
	public Texture PlusRice;

	// Token: 0x040003CC RID: 972
	public Texture PlusPixel;

	// Token: 0x040003CD RID: 973
	public static Girl PreviousGirl;

	// Token: 0x040003CE RID: 974
	public static Girl CurrentGirl;

	// Token: 0x040003CF RID: 975
	private Image loverEyes;

	// Token: 0x040003D0 RID: 976
	private int leftOffset = 79;

	// Token: 0x040003D1 RID: 977
	private int topOffset = -232;

	// Token: 0x040003D2 RID: 978
	private Girl lastGirlSprite;

	// Token: 0x040003D3 RID: 979
	private Dictionary<string, ImageOffset> ImageOffsets = new Dictionary<string, ImageOffset>();

	// Token: 0x040003D4 RID: 980
	private float pokeAnimation = 3.5f;

	// Token: 0x040003D5 RID: 981
	private float heartTempo;

	// Token: 0x040003D6 RID: 982
	private float loveAnimation;

	// Token: 0x040003D7 RID: 983
	private float pokeStartX = 79f;

	// Token: 0x040003D8 RID: 984
	private float pokeScale = 5f;

	// Token: 0x040003D9 RID: 985
	private List<Image> fadingImages = new List<Image>();

	// Token: 0x040003DA RID: 986
	private List<int> unlockedGirls = new List<int>();

	// Token: 0x040003DB RID: 987
	private bool _loadedAssets;

	// Token: 0x040003DC RID: 988
	private int unlockedGirlsCount;

	// Token: 0x040003DD RID: 989
	public Button GiftButton;

	// Token: 0x040003DE RID: 990
	public Button DateButton;

	// Token: 0x040003DF RID: 991
	public Button StatButton;

	// Token: 0x040003E0 RID: 992
	public Kiss KissPopup;

	// Token: 0x040003E1 RID: 993
	private long lastHeartText = -1L;

	// Token: 0x040003E2 RID: 994
	private long lastHeartReq = -1L;

	// Token: 0x040003E3 RID: 995
	private Balance.GirlName lastHeartGirl = Balance.GirlName.Unknown;

	// Token: 0x040003E4 RID: 996
	public GameObject[] Requirements;

	// Token: 0x040003E5 RID: 997
	public GameObject PurchasePhoneFling;

	// Token: 0x040003E6 RID: 998
	private long lastHeartRequirement = -1L;

	// Token: 0x040003E7 RID: 999
	public bool pendingOutro;

	// Token: 0x040003E8 RID: 1000
	public Sprite[] DateIcons;
}
