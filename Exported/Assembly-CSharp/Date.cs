using System;
using System.Collections;
using SadPanda.Platforms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200010B RID: 267
public class Date : MonoBehaviour, IUpdateable
{
	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x00035DAC File Offset: 0x00033FAC
	public Requirement.DateType DateType
	{
		get
		{
			return this.Data.DateType;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00035DBC File Offset: 0x00033FBC
	private void Start()
	{
		base.GetComponent<Button>().onClick.RemoveAllListeners();
		base.GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.transform.Find("Girls/Popups/Date Buy Popup").GetComponent<GiftPurchase>().Init(this);
		});
		base.transform.Find("Title").GetComponent<Text>().text = Translations.TranslateDate(this.DateType);
		base.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}", this.Data.Price.ToString("n0"));
		base.transform.Find("Time Text").GetComponent<Text>().text = this.Data.TimeBlocks.ToString();
		base.transform.Find("Date Icon").GetComponent<Image>().sprite = this.Data.Icon;
		this.DatePopup = GameState.CurrentState.transform.Find("Girls/Popups/Date Overlay").gameObject;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00035EC8 File Offset: 0x000340C8
	public bool OnDate(int timeCost, int dateCount = 1)
	{
		if (this.completeButton == null)
		{
			this.completeButton = this.DatePopup.transform.Find("Dialog/Progress Container/Complete Button").gameObject;
		}
		this.completeButton.GetComponent<Button>().onClick.RemoveAllListeners();
		this.completeButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnCompleteButton));
		if (this.againButton == null)
		{
			this.againButton = this.DatePopup.transform.Find("Dialog/Progress Container/Again Button").gameObject;
		}
		this.againButton.GetComponent<Button>().onClick.RemoveAllListeners();
		this.againButton.GetComponent<Button>().onClick.AddListener(delegate()
		{
			if (this.countText != null)
			{
				this.countText.gameObject.SetActive(false);
			}
			this.OnClose();
			if (FreeTime.Free >= this.Data.TimeBlocks)
			{
				if (GameState.Money.Value < (double)this.Data.Price)
				{
					return;
				}
				GameState.Money.Value -= (double)this.Data.Price;
				this.OnDate(this.Data.TimeBlocks, 1);
			}
			else
			{
				Utilities.PurchaseTimeBlocks(timeCost - FreeTime.Free);
			}
		});
		if (this.countText == null)
		{
			this.countText = this.DatePopup.transform.Find("Dialog/Progress Container/Amount Text").gameObject;
		}
		this.countText.gameObject.SetActive(false);
		string name = string.Empty;
		if (Girls.CurrentGirl.GirlName == Balance.GirlName.Explora)
		{
			this.DatePopup.transform.Find("Dialog/Progress Container/ProgressImage Background").gameObject.SetActive(false);
			name = "Dialog/Progress Container/ProgressImage Background Explora";
		}
		else
		{
			this.DatePopup.transform.Find("Dialog/Progress Container/ProgressImage Background Explora").gameObject.SetActive(false);
			name = "Dialog/Progress Container/ProgressImage Background";
		}
		this.progressBackground = this.DatePopup.transform.Find(name).gameObject;
		this.progressBar = this.progressBackground.transform.Find("DateProgressBar").GetComponent<Image>();
		this.dateTimeCost = timeCost;
		if (FreeTime.Free >= this.dateTimeCost && this.currentTime == 0f)
		{
			bool flag = Girl.FindGirl(Balance.GirlName.QPiddy).Love != 9 && Girls.UnlockedGirlCount >= 15 && Girl.FindGirl(Balance.GirlName.QPiddy).Love != 9;
			this.DatePopup.transform.Find("Dialog/Date Image").GetComponent<Image>().sprite = ((!flag) ? this.Data.Sprite1 : this.Data.Sprite2);
			Image component = this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<Image>();
			int num = Requirement.IndexFromDateType(this.DateType);
			if (this._currentOffset == num && this._currentGirl == Girls.CurrentGirl.GirlName)
			{
				component.gameObject.SetActive(true);
			}
			else
			{
				component.gameObject.SetActive(false);
				GameState.CurrentState.StartCoroutine(this.LoadDateOverlayAsync(Girls.CurrentGirl.GirlName, Girl.SpriteType.Moonlight + num));
			}
			this._currentOffset = num;
			this._currentGirl = Girls.CurrentGirl.GirlName;
			this.completeButton.SetActive(false);
			this.againButton.SetActive(false);
			this.progressBackground.SetActive(true);
			this.DatePopup.SetActive(true);
			FreeTime.DateTime += this.dateTimeCost;
			GameState.RegisterUpdate(this);
			return true;
		}
		return false;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0003622C File Offset: 0x0003442C
	private IEnumerator LoadDateOverlayAsync(Balance.GirlName girlName, Girl.SpriteType type)
	{
		Girl girl = Girl.FindGirl(girlName);
		if (girl == null)
		{
			yield break;
		}
		Image GirlOverlay = this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<Image>();
		int GirlOverlayIndex = Requirement.IndexFromDateType(this.DateType);
		Girl.LoadImageAsyncRequest spriteRequest = new Girl.LoadImageAsyncRequest(girl, type);
		yield return spriteRequest.GetSpriteAsync();
		GirlOverlay.sprite = spriteRequest.Sprite;
		GirlOverlay.color = new Color(1f, 1f, 1f, 0f);
		this.loadedBundle = spriteRequest.Bundle;
		if (GirlOverlay.sprite != null)
		{
			float scaling = 260f / GirlOverlay.sprite.rect.size.y;
			this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<PixelationEffect>().SetWidth(scaling * 766f);
			GirlOverlay.rectTransform.sizeDelta = scaling * GirlOverlay.sprite.rect.size;
			if (Girls.CurrentGirl.DateOffsets.Length > GirlOverlayIndex)
			{
				GirlOverlay.rectTransform.localPosition = new Vector3((float)Girls.CurrentGirl.DateOffsets[GirlOverlayIndex], 0f, 0f);
			}
			else
			{
				GirlOverlay.rectTransform.localPosition = new Vector3(0f, 0f, 0f);
			}
			GirlOverlay.gameObject.SetActive(true);
		}
		else
		{
			GirlOverlay.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00036264 File Offset: 0x00034464
	public GameState.UpdateType PerformUpdate(float dt)
	{
		float num = (float)this.Data.Time;
		if (Girls.CurrentGirl.GirlName == Balance.GirlName.Explora)
		{
			num = Mathf.Max(5f * GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier, num);
		}
		if (this.timeRemaining == null)
		{
			this.timeRemaining = this.DatePopup.transform.Find("Dialog/Progress Container/Time Remaining").GetComponent<Text>();
		}
		if (this.currentTime + dt >= num && !this.completeButton.activeInHierarchy)
		{
			this.completeButton.SetActive(true);
			if (GameState.Money.Value >= (double)this.Data.Price)
			{
				GameState.UnregisterUpdate(this);
				this.againButton.SetActive(this.dateTimeCost != 0);
			}
			this.progressBackground.SetActive(false);
			this.againButton.GetComponent<Button>().interactable = (GameState.Money.Value >= (double)this.Data.Price);
			this.timeRemaining.text = string.Empty;
			Requirement requirement = null;
			foreach (Requirement requirement2 in Girls.CurrentGirl.Requirements)
			{
				if (requirement2 != null && requirement2.Type == Requirement.RequirementType.Date && requirement2.Date == this.DateType)
				{
					requirement = requirement2;
				}
			}
			if (requirement != null && Girls.CurrentGirl.DateCount[requirement.Index] + 1 < requirement.DateCount && Girls.CurrentGirl.Love < 9)
			{
				this.countText.SetActive(true);
				this.countText.GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_18_1", "Gone On:") + "\n{0}/{1}", (Girls.CurrentGirl.DateCount[requirement.Index] + 1).ToString("n0"), requirement.DateCount.ToString("n0"));
			}
			else
			{
				this.countText.SetActive(false);
			}
			this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<PixelationEffect>().SetEffect(1f);
		}
		else if (this.completeButton.activeInHierarchy && !this.againButton.activeSelf)
		{
			if (GameState.Money.Value >= (double)this.Data.Price)
			{
				this.againButton.SetActive(this.dateTimeCost != 0);
				this.againButton.GetComponent<Button>().interactable = true;
				GameState.UnregisterUpdate(this);
			}
		}
		else
		{
			this.currentTime = Mathf.Min(num, this.currentTime + dt);
			if (this.progressBar == null)
			{
				this.progressBar = this.DatePopup.transform.Find("Dialog/Progress Container/ProgressImage Background/DateProgressBar").GetComponent<Image>();
			}
			float num2 = this.currentTime / num;
			if (Girls.CurrentGirl.GirlName == Balance.GirlName.Explora)
			{
				num2 = Mathf.Round(num2 * 34f) / 34.6f + 0.00882353f;
			}
			this.progressBar.fillAmount = num2;
			float num3 = Mathf.Round(10f * this.currentTime / ((float)GameState.PurchasedAdMultiplier * GameState.CurrentState.TimeMultiplier.Value)) / 50f;
			this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<PixelationEffect>().SetEffect((Girls.CurrentGirl.GirlName != Balance.GirlName.Explora) ? 1f : num3);
			this.timeRemaining.text = string.Format(Translations.GetTranslation("everything_else_97_0", "Time Remaining: {0}") + "s", Mathf.RoundToInt((num - this.currentTime) / (GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier)).ToString());
		}
		return GameState.UpdateType.None;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00036674 File Offset: 0x00034874
	public void OnClose()
	{
		this.DatePopup.gameObject.SetActive(false);
		this.currentTime = 0f;
		Girls.CurrentGirl.GiveDate(this.DateType, 1);
		GameState.CurrentState.transform.Find("Girls").GetComponent<Girls>().DateGirl(this);
		GameState.DateCount++;
		Kongregate.SubmitStat("DateCount", (long)GameState.DateCount);
		GameState.GetGirlScreen().HideInteractions();
		FreeTime.DateTime -= this.dateTimeCost;
		GameState.UpdatePanels(GameState.UpdateType.Skill);
		GameState.UnregisterUpdate(this);
		Image component = this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<Image>();
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0003672C File Offset: 0x0003492C
	private void UnloadAssets()
	{
		Image component = this.DatePopup.transform.Find("Dialog/Date Image/Girl Overlay").GetComponent<Image>();
		if (this.loadedBundle != null)
		{
			GameState.AssetManager.UnloadBundle(this.loadedBundle, true);
		}
		this.loadedBundle = null;
		component.sprite = null;
		this._currentGirl = Balance.GirlName.Unknown;
		this._currentOffset = -1;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00036794 File Offset: 0x00034994
	public void SaveCurrentTime()
	{
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00036798 File Offset: 0x00034998
	private void OnCompleteButton()
	{
		Girls.CurrentGirl.FinishDate();
		this.OnClose();
		this.UnloadAssets();
	}

	// Token: 0x04000665 RID: 1637
	public DateModel Data;

	// Token: 0x04000666 RID: 1638
	private GameObject DatePopup;

	// Token: 0x04000667 RID: 1639
	private GameObject completeButton;

	// Token: 0x04000668 RID: 1640
	private GameObject completeButtonBG;

	// Token: 0x04000669 RID: 1641
	private GameObject againButton;

	// Token: 0x0400066A RID: 1642
	private GameObject progressBackground;

	// Token: 0x0400066B RID: 1643
	private GameObject countText;

	// Token: 0x0400066C RID: 1644
	private Image progressBar;

	// Token: 0x0400066D RID: 1645
	private float currentTime;

	// Token: 0x0400066E RID: 1646
	private int dateTimeCost;

	// Token: 0x0400066F RID: 1647
	private Balance.GirlName _currentGirl = Balance.GirlName.Unknown;

	// Token: 0x04000670 RID: 1648
	private int _currentOffset = -1;

	// Token: 0x04000671 RID: 1649
	private Text timeRemaining;

	// Token: 0x04000672 RID: 1650
	private AssetBundle loadedBundle;
}
