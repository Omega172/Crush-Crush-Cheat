using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000010 RID: 16
public class MonsterBios : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x000040E4 File Offset: 0x000022E4
	public void InitJelleQuillzone()
	{
		this.Init(Playfab.PlayfabItems.JelleQuillzone);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000040F4 File Offset: 0x000022F4
	public void InitBonchovySpectrum()
	{
		this.Init(Playfab.PlayfabItems.BonchovySpectrum);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00004104 File Offset: 0x00002304
	public void Init(Playfab.PlayfabItems item)
	{
		GameState.AssetManager.GetBundle("universe/bios", false, delegate(AssetBundle bundle)
		{
			this.bundle = bundle;
			Text component = this.transform.Find("Dialog/Girl 1/Title").GetComponent<Text>();
			Text component2 = this.transform.Find("Dialog/Girl 2/Title").GetComponent<Text>();
			Text component3 = this.transform.Find("Dialog/Girl 1/Background/Text").GetComponent<Text>();
			Text component4 = this.transform.Find("Dialog/Girl 2/Background/Text").GetComponent<Text>();
			Image girls = this.transform.Find("Dialog/Girls").GetComponent<Image>();
			Image component5 = this.transform.Find("Dialog/Girl 1/Background/Image").GetComponent<Image>();
			Image component6 = this.transform.Find("Dialog/Girl 2/Background/Image").GetComponent<Image>();
			Image component7 = this.transform.Find("Dialog/Girl 1/Background/Icon").GetComponent<Image>();
			Image component8 = this.transform.Find("Dialog/Girl 2/Background/Icon").GetComponent<Image>();
			Playfab.PlayfabItems item2 = item;
			if (item2 != Playfab.PlayfabItems.JelleQuillzone)
			{
				if (item2 == Playfab.PlayfabItems.BonchovySpectrum)
				{
					girls.sprite = bundle.LoadAsset<Sprite>("bios_fullBody_bonchovySpectrum");
					component2.text = Translations.TranslateGirlName(Balance.GirlName.Bonchovy);
					component4.text = "Height: <color=\"black\">6'7\"</color>\nAge: <color=\"black\">23</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Mermaid</color>\n\nYou and this beautiful siren were mer-maid for each other! But be careful, or you'll end up like any sea man thrown into the sea.";
					component8.sprite = this.BonchovyIcon;
					component6.sprite = bundle.LoadAsset<Sprite>("bios_thumbs_bonchovy");
					component.text = Translations.TranslateGirlName(Balance.GirlName.Spectrum);
					component3.text = "Height: <color=\"black\">5'6\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Centaur</color>\n\nYou'll have to earn your way to the \"centaur\" of this Monster Girl's heart! Spurs, whips and bridles are not advised.";
					component7.sprite = this.SpectrumIcon;
					component5.sprite = bundle.LoadAsset<Sprite>("bios_thumbs_spectrum");
				}
			}
			else
			{
				girls.sprite = bundle.LoadAsset<Sprite>("bios_fullBody_jelleQuillzone");
				component2.text = Translations.TranslateGirlName(Balance.GirlName.Quillzone);
				component4.text = "Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">1990</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Pizzakyatto</color>\n\nThis Monster Girl is stronger than dirt, and twice as unusual! An extremely rare species of Cat Girl from the wilds of Japan.";
				component8.sprite = this.QuillzoneIcon;
				component6.sprite = bundle.LoadAsset<Sprite>("bios_thumbs_quillzone");
				component.text = Translations.TranslateGirlName(Balance.GirlName.Jelle);
				component3.text = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">220</color>\nBust: <color=\"black\">E</color>\nType: <color=\"black\">Slime</color>\n\nSoft and sweet, this gel-based Monster Girl wants ALL your attention. Just don't get too \"absorbed\" into your work.";
				component7.sprite = this.JelleIcon;
				component5.sprite = bundle.LoadAsset<Sprite>("bios_thumbs_jelle");
			}
			if (GameState.NSFW)
			{
				GameState.AssetManager.GetBundle("universe/bios_nsfw", false, delegate(AssetBundle nsfwBundle)
				{
					this.nsfwBundle = nsfwBundle;
					if (nsfwBundle != null)
					{
						Playfab.PlayfabItems item3 = item;
						if (item3 != Playfab.PlayfabItems.JelleQuillzone)
						{
							if (item3 == Playfab.PlayfabItems.BonchovySpectrum)
							{
								girls.sprite = nsfwBundle.LoadAsset<Sprite>("bios_fullBody_bonchovySpectrum");
							}
						}
						else
						{
							girls.sprite = nsfwBundle.LoadAsset<Sprite>("bios_fullBody_jelleQuillzone");
						}
					}
				});
			}
			this.gameObject.SetActive(true);
		});
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00004144 File Offset: 0x00002344
	private void OnDisable()
	{
		if (this.bundle != null)
		{
			GameState.AssetManager.UnloadBundle(this.bundle, true);
		}
		if (this.nsfwBundle != null)
		{
			GameState.AssetManager.UnloadBundle(this.nsfwBundle, true);
		}
	}

	// Token: 0x04000024 RID: 36
	public Sprite SpectrumIcon;

	// Token: 0x04000025 RID: 37
	public Sprite BonchovyIcon;

	// Token: 0x04000026 RID: 38
	public Sprite QuillzoneIcon;

	// Token: 0x04000027 RID: 39
	public Sprite JelleIcon;

	// Token: 0x04000028 RID: 40
	private AssetBundle bundle;

	// Token: 0x04000029 RID: 41
	private AssetBundle nsfwBundle;
}
