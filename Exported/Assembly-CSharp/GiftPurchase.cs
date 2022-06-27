using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010F RID: 271
public class GiftPurchase : MonoBehaviour
{
	// Token: 0x0600068C RID: 1676 RVA: 0x0003738C File Offset: 0x0003558C
	private void Update()
	{
		this.PayMoney.interactable = (GameState.Money.Value >= this.moneyCost && this.moneyCost != 0.0);
		this.PayDiamonds.interactable = (GameState.Diamonds.Value >= this.diamondCost);
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x000373F0 File Offset: 0x000355F0
	private void Init()
	{
		this.MoneyText = base.transform.Find("Dialog/Money Cost Amount").GetComponent<Text>();
		this.Title = base.transform.Find("Dialog/Purchase Title").GetComponent<Text>();
		this.HeartPayout = base.transform.Find("Dialog/Heart Payout Text").GetComponent<Text>();
		this.TimeCost = base.transform.Find("Dialog/Time Cost Amount").GetComponent<Text>();
		this.DiamondText = base.transform.Find("Dialog/Diamond Cost Amount").GetComponent<Text>();
		this.ShippingText = base.transform.Find("Dialog/Shipping Text").GetComponent<Text>();
		this.ShippingText.gameObject.SetActive(false);
		this.QuantityText = base.transform.Find("Dialog/Quantity Text").GetComponent<Text>();
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000374CC File Offset: 0x000356CC
	public void Init(Gift gift)
	{
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		if (this.MoneyText == null)
		{
			this.Init();
		}
		this.selectedDate = null;
		this.selectedGift = gift;
		this.Title.text = string.Format(Translations.GetTranslation("everything_else_15_0", "Are you sure you want to buy:") + "\n" + Translations.GetTranslation("everything_else_15_1", "{0} for {1}?"), gift.Title, Translations.TranslateGirlName(Girls.CurrentGirl.GirlName));
		this.diamondCost = ((gift.OutfitType != Requirement.OutfitType.None) ? Balance.GetOutfitDiamondCost(gift.OutfitType, Girls.CurrentGirl.GirlName) : gift.GiftData.GetGiftDiamondCost(1));
		this.DiamondText.text = this.diamondCost.ToString();
		this.Icon.sprite = gift.GiftImage;
		this.quantity = 1;
		if (gift.OutfitType == Requirement.OutfitType.None)
		{
			this.CalculateShipping();
		}
		else
		{
			this.QuantityText.text = "x 1";
			this.MoneyText.text = "N/A";
			this.HeartPayout.text = Translations.GetTranslation("everything_else_16_1", "Gives:") + " 5";
			this.moneyCost = 0.0;
		}
		if (this.TimeIcon != null)
		{
			this.TimeIcon.gameObject.SetActive(false);
		}
		if (this.TimeCost != null)
		{
			this.TimeCost.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
		base.transform.Find("Dialog/Add Quantity Button").GetComponent<Button>().interactable = (gift.OutfitType == Requirement.OutfitType.None && gift.GiftType != Requirement.GiftType.Potion && gift.GiftType != Requirement.GiftType.USB);
		base.transform.Find("Dialog/Remove Quantity Button").GetComponent<Button>().interactable = (gift.OutfitType == Requirement.OutfitType.None && gift.GiftType != Requirement.GiftType.Potion && gift.GiftType != Requirement.GiftType.USB);
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0003770C File Offset: 0x0003590C
	public void Init(Date date)
	{
		if (Girls.CurrentGirl == null)
		{
			return;
		}
		if (this.MoneyText == null)
		{
			this.Init();
		}
		this.selectedDate = date;
		this.selectedGift = null;
		string arg = Translations.TranslateLongDate(date.DateType);
		if (!string.IsNullOrEmpty(date.Data.Phrase) && date.Data.Phrase != date.Data.Name)
		{
			arg = date.Data.Phrase;
		}
		this.Title.text = string.Format(Translations.GetTranslation("everything_else_17_0", "Are you sure you want to go on: \n{0} with {1}?").Replace("\\n", "\n"), arg, Translations.TranslateGirlName(Girls.CurrentGirl.GirlName));
		this.moneyCost = (double)date.Data.Price;
		this.MoneyText.text = Utilities.ToPrefixedNumber(this.moneyCost, false, false).Replace(".00", string.Empty);
		Text diamondText = this.DiamondText;
		int num = this.diamondCost = date.Data.DiamondCost;
		diamondText.text = num.ToString();
		this.Icon.sprite = date.Data.Icon;
		this.HeartPayout.text = string.Format("{0} {1}", Translations.GetTranslation("everything_else_16_1", "Gives:"), date.Data.Hearts.ToString("n0"));
		this.TimeIcon.gameObject.SetActive(true);
		this.TimeCost.gameObject.SetActive(true);
		this.TimeCost.text = date.Data.TimeBlocks.ToString();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x000378D8 File Offset: 0x00035AD8
	public void OnClickAccept()
	{
		this.OnClickClose();
		if (this.selectedGift != null)
		{
			if (GameState.Money.Value < this.moneyCost)
			{
				return;
			}
			GameState.Money.Value -= this.moneyCost;
			this.selectedGift.OnGift(this.quantity);
		}
		else if (this.selectedDate != null)
		{
			int timeBlocks = this.selectedDate.Data.TimeBlocks;
			if (FreeTime.Free >= timeBlocks)
			{
				if (GameState.Money.Value < (double)this.selectedDate.Data.Price)
				{
					return;
				}
				GameState.Money.Value -= (double)this.selectedDate.Data.Price;
				this.selectedDate.OnDate(this.selectedDate.Data.TimeBlocks, 1);
			}
			else
			{
				Utilities.PurchaseTimeBlocks(timeBlocks - FreeTime.Free);
			}
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x000379E0 File Offset: 0x00035BE0
	private bool IsInBundle(Requirement.OutfitType gift)
	{
		return !string.IsNullOrEmpty(Playfab.Promotion) && ((gift == Requirement.OutfitType.Christmas && Playfab.Promotion == "Christmas2016") || Playfab.Promotion == "NutakuAnniversary2017");
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00037A38 File Offset: 0x00035C38
	public void OnClickDiamond()
	{
		if (this.selectedGift != null || this.selectedDate != null)
		{
			Utilities.ConfirmPurchase(this.diamondCost, new Action(this.OnConfirmDiamond));
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00037A74 File Offset: 0x00035C74
	public void OnConfirmDiamond()
	{
		this.OnClickClose();
		if (this.selectedGift != null)
		{
			this.selectedGift.OnGift(this.quantity);
			if (this.selectedGift.OutfitType != Requirement.OutfitType.None)
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, string.Format("{0}{1}", Girls.CurrentGirl.GirlName.ToFriendlyString(), this.selectedGift.OutfitType.ToFriendlyString()));
			}
			else
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, this.selectedGift.GiftType.ToFriendlyString());
			}
		}
		else if (this.selectedDate != null)
		{
			this.selectedDate.OnDate(0, 1);
			Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, this.selectedDate.DateType.ToFriendlyString());
		}
		GameState.CurrentState.QueueQuickSave();
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00037B48 File Offset: 0x00035D48
	public void OnClickClose()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00037B58 File Offset: 0x00035D58
	private void CalculateShipping()
	{
		this.moneyCost = (double)this.selectedGift.GiftData.Price * (double)this.quantity;
		this.MoneyText.text = Utilities.ToPrefixedNumber(this.moneyCost, false, false);
		if (this.moneyCost == 0.0)
		{
			this.MoneyText.text = "N/A";
		}
		this.MoneyText.fontSize = ((this.MoneyText.text.Length <= 26) ? 16 : 14);
		this.diamondCost = this.selectedGift.GiftData.GetGiftDiamondCost(this.quantity);
		this.DiamondText.text = this.diamondCost.ToString("n0");
		this.QuantityText.text = string.Format("x {0}", Utilities.ToPrefixedNumber((double)this.quantity, true, false)).Replace(".00", string.Empty).Replace(".0", string.Empty);
		string format = Translations.GetTranslation("everything_else_15_0", "Are you sure you want to buy:") + "\n" + Translations.GetTranslation("everything_else_15_1", "{0} for {1}?");
		this.Title.text = string.Format(format, Translations.TranslateGift(this.selectedGift.GiftType, Girls.CurrentGirl.GirlName), Translations.TranslateGirlName(Girls.CurrentGirl.GirlName));
		this.ShippingText.text = string.Empty;
		double number = (double)this.selectedGift.GiftData.GetHearts(Girls.CurrentGirl.GirlName) * (double)this.quantity;
		this.HeartPayout.text = string.Format("{0} {1}", Translations.GetTranslation("everything_else_16_1", "Gives:"), Utilities.ToPrefixedNumber(number, false, false));
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00037D28 File Offset: 0x00035F28
	public void IncreaseQuantity()
	{
		if (this.selectedGift.GiftType == Requirement.GiftType.USB || this.selectedGift.GiftType == Requirement.GiftType.Potion)
		{
			this.quantity = 1;
			return;
		}
		if (this.quantity == 1)
		{
			this.quantity = 5;
		}
		else if (this.quantity == 5)
		{
			this.quantity = 10;
		}
		else if (this.quantity == 10)
		{
			this.quantity = 25;
		}
		else if (this.quantity == 25)
		{
			this.quantity = 50;
		}
		else if (this.quantity == 50)
		{
			this.quantity = 100;
		}
		else if (this.quantity == 100)
		{
			this.quantity = 1000;
		}
		else if (this.quantity == 1000)
		{
			this.quantity = 10000;
		}
		else if (this.quantity == 10000)
		{
			this.quantity = 100000;
		}
		else
		{
			this.quantity = 1000000;
		}
		this.CalculateShipping();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00037E54 File Offset: 0x00036054
	public void DecreaseQuantity()
	{
		if (this.quantity == 1)
		{
			return;
		}
		if (this.quantity == 5)
		{
			this.quantity = 1;
		}
		else if (this.quantity == 10)
		{
			this.quantity = 5;
		}
		else if (this.quantity == 25)
		{
			this.quantity = 10;
		}
		else if (this.quantity == 50)
		{
			this.quantity = 25;
		}
		else if (this.quantity == 100)
		{
			this.quantity = 50;
		}
		else if (this.quantity == 1000)
		{
			this.quantity = 100;
		}
		else if (this.quantity == 10000)
		{
			this.quantity = 1000;
		}
		else if (this.quantity == 100000)
		{
			this.quantity = 10000;
		}
		else
		{
			this.quantity = 100000;
		}
		this.CalculateShipping();
	}

	// Token: 0x04000684 RID: 1668
	private Text MoneyText;

	// Token: 0x04000685 RID: 1669
	private Text Title;

	// Token: 0x04000686 RID: 1670
	private Text HeartPayout;

	// Token: 0x04000687 RID: 1671
	private Text TimeCost;

	// Token: 0x04000688 RID: 1672
	private Text DiamondText;

	// Token: 0x04000689 RID: 1673
	private Text QuantityText;

	// Token: 0x0400068A RID: 1674
	private Text ShippingText;

	// Token: 0x0400068B RID: 1675
	public Image Icon;

	// Token: 0x0400068C RID: 1676
	public Image TimeIcon;

	// Token: 0x0400068D RID: 1677
	public Button PayMoney;

	// Token: 0x0400068E RID: 1678
	public Button PayDiamonds;

	// Token: 0x0400068F RID: 1679
	public GameObject ConfirmDiamondPurchase;

	// Token: 0x04000690 RID: 1680
	private Gift selectedGift;

	// Token: 0x04000691 RID: 1681
	private Date selectedDate;

	// Token: 0x04000692 RID: 1682
	private double moneyCost;

	// Token: 0x04000693 RID: 1683
	private int diamondCost;

	// Token: 0x04000694 RID: 1684
	private int quantity;
}
