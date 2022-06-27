using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009A RID: 154
public class Gifting : MonoBehaviour
{
	// Token: 0x06000313 RID: 787 RVA: 0x00016E44 File Offset: 0x00015044
	private void Start()
	{
		this.InitPrefabs();
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00016E4C File Offset: 0x0001504C
	private void InitPrefabs()
	{
		if (this.GiftPrefabs[0] != null)
		{
			return;
		}
		for (int i = 0; i < this.GiftPrefabs.Length; i++)
		{
			this.GiftPrefabs[i] = (Gift)UnityEngine.Object.Instantiate(this.Prefab, base.transform.Find("Dialog/Gifts"));
			this.GiftPrefabs[i].transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000315 RID: 789 RVA: 0x00016EC8 File Offset: 0x000150C8
	// (set) Token: 0x06000316 RID: 790 RVA: 0x00016ED0 File Offset: 0x000150D0
	public int Page { get; private set; }

	// Token: 0x06000317 RID: 791 RVA: 0x00016EDC File Offset: 0x000150DC
	public void Init(int page)
	{
		this.Page = page;
		this.InitPrefabs();
		base.gameObject.SetActive(true);
		int num = page * 8;
		int num2 = (page + 1) * 8;
		Requirement.GiftType[] giftTypes = Balance.GetGiftTypes(Girls.CurrentGirl);
		Requirement.OutfitType[] outfitTypes = Balance.GetOutfitTypes(Girls.CurrentGirl);
		bool flag = false;
		for (int i = num; i < Mathf.Min(num2, giftTypes.Length + outfitTypes.Length); i++)
		{
			int num3 = (int)((i < giftTypes.Length) ? giftTypes[i] : ((Requirement.GiftType)outfitTypes[i - giftTypes.Length]));
			for (int j = 0; j < 64; j++)
			{
				if (num3 == 1 << j)
				{
					break;
				}
			}
			this.GiftPrefabs[i % 8].gameObject.SetActive(true);
			if (i >= giftTypes.Length)
			{
				Requirement.OutfitType outfitType = outfitTypes[i - giftTypes.Length];
				foreach (KeyValuePair<short, OutfitModel> keyValuePair in Universe.Outfits)
				{
					if (keyValuePair.Value.LegacyId == (int)outfitType)
					{
						this.GiftPrefabs[i % 8].Init(keyValuePair.Value);
					}
				}
				flag = true;
			}
			else
			{
				this.GiftPrefabs[i % 8].Init(giftTypes[i]);
			}
		}
		for (int k = giftTypes.Length + outfitTypes.Length; k < num2; k++)
		{
			this.GiftPrefabs[k % 8].gameObject.SetActive(false);
		}
		int num4 = Mathf.CeilToInt((float)(giftTypes.Length + outfitTypes.Length) / 8f);
		this.nextPage = Mathf.Min(num4 - 1, page + 1);
		this.prevPage = Mathf.Max(0, page - 1);
		base.transform.Find("Dialog/Page Text").GetComponent<Text>().text = string.Format("Pg {0}/{1}", (page + 1).ToString(), num4.ToString());
		this.UpdateGifts();
		if (flag)
		{
			base.transform.Find("Dialog/Disclaimer").GetComponent<Text>().text = (Girl.IsLteGirl(Girls.CurrentGirl.GirlName) ? "Outfits wearable at Lover level (LTE required)" : "Outfits wearable at Lover level");
		}
		base.transform.Find("Dialog/Disclaimer").gameObject.SetActive(flag);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00017160 File Offset: 0x00015360
	public void NextPage()
	{
		this.Init(this.nextPage);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00017170 File Offset: 0x00015370
	public void PrevPage()
	{
		this.Init(this.prevPage);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00017180 File Offset: 0x00015380
	public void UpdateGifts()
	{
		if (base.gameObject.activeInHierarchy)
		{
			Gift[] componentsInChildren = base.GetComponentsInChildren<Gift>();
			foreach (Gift gift in componentsInChildren)
			{
				if (gift.OutfitType != Requirement.OutfitType.None && Girls.CurrentGirl.Love == 9)
				{
					bool flag = (Playfab.InventoryObjects & (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude) & gift.OutfitType) != Requirement.OutfitType.None;
					flag |= ((Playfab.InventoryObjects & gift.OutfitType) != Requirement.OutfitType.None && (Girls.CurrentGirl.Data.Id < 18 || Girls.CurrentGirl.Data.Id == 31));
					if ((Balance.GetOutfitDiamondCost(gift.OutfitType, Girls.CurrentGirl.GirlName) <= GameState.Diamonds.Value && Balance.GetOutfitDiamondCost(gift.OutfitType, Girls.CurrentGirl.GirlName) > 0) || (Girls.CurrentGirl.LifetimeOutfits & gift.OutfitType) != Requirement.OutfitType.None || flag)
					{
						gift.GetComponent<Button>().interactable = true;
						gift.SetCheck();
					}
					else
					{
						gift.GetComponent<Button>().interactable = false;
					}
				}
				else
				{
					gift.GetComponent<Button>().interactable = (gift.OutfitType == Requirement.OutfitType.None && ((double)gift.GiftData.Price <= GameState.Money.Value || gift.GiftData.GetGiftDiamondCost(1) <= GameState.Diamonds.Value));
				}
			}
		}
	}

	// Token: 0x0400035B RID: 859
	public Gift Prefab;

	// Token: 0x0400035C RID: 860
	private Gift[] GiftPrefabs = new Gift[8];

	// Token: 0x0400035D RID: 861
	private int nextPage;

	// Token: 0x0400035E RID: 862
	private int prevPage;
}
