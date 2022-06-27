using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011A RID: 282
public class SmallBio : MonoBehaviour
{
	// Token: 0x060006E5 RID: 1765 RVA: 0x0003C110 File Offset: 0x0003A310
	public void Init(Balance.GirlName girl)
	{
		base.transform.Find("Dialog/Catara").gameObject.SetActive(girl == Balance.GirlName.Catara);
		base.transform.Find("Dialog/Girl 1/Title").GetComponent<Text>().text = Translations.TranslateGirlName(girl);
		string girlName = girl.ToLowerFriendlyString();
		this._biosBundle = (this._achievementsBundle = null);
		GameState.AssetManager.GetBundle("universe/bios", false, delegate(AssetBundle bundle)
		{
			this._biosBundle = bundle;
			string text = this._nameToTextDic[girl];
			if (girl == Balance.GirlName.Explora)
			{
				int num = DateTime.Now.Year - 1995 - 1;
				if (DateTime.Now.Month > 8 || (DateTime.Now.Month == 8 && DateTime.Now.Day >= 16))
				{
					num++;
				}
				text = text.Replace("%AGE%", num.ToString());
			}
			this.transform.Find("Dialog/Girl 1/Background/Text").GetComponent<Text>().text = text;
			this.transform.Find("Dialog/Girl 1/Background/Image").GetComponent<Image>().sprite = this._biosBundle.LoadAsset<Sprite>("bios_thumbs_" + girlName);
			this.transform.Find("Dialog/Girl 1/Full Body").GetComponent<Image>().sprite = this._biosBundle.LoadAsset<Sprite>("bios_fullBody_" + girlName);
			this.TryEnableGameObject();
		});
		GameState.AssetManager.GetBundle("universe/achievements", false, delegate(AssetBundle bundle)
		{
			this._achievementsBundle = bundle;
			this.transform.Find("Dialog/Girl 1/Background/Icon").GetComponent<Image>().sprite = this._achievementsBundle.LoadAsset<Sprite>("achievements_" + girlName);
			this.TryEnableGameObject();
		});
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0003C1D4 File Offset: 0x0003A3D4
	private void TryEnableGameObject()
	{
		if (this._biosBundle != null && this._achievementsBundle != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0003C210 File Offset: 0x0003A410
	private void OnDisable()
	{
		if (this._biosBundle != null)
		{
			GameState.AssetManager.UnloadBundle(this._biosBundle, true);
		}
		if (this._achievementsBundle != null)
		{
			GameState.AssetManager.UnloadBundle(this._achievementsBundle, true);
		}
	}

	// Token: 0x040006D8 RID: 1752
	private const string DaryaBody = "Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">100,069</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Diamond</color>\n\nThis majestic and powerful entity hails from a distant galaxy, and can abiogenerate.  In other words - she makes you Diamonds.";

	// Token: 0x040006D9 RID: 1753
	private const string CharlotteBody = "Height: <color=\"black\">4'11\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Gothic Lolita</color>\n\nOpen the Crypt to unlock this gothic gal, all of her outfits, and the grim new Job - \"Grave Digger\".";

	// Token: 0x040006DA RID: 1754
	private const string OdangoBody = "Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">Ver 2.0</color>\nBust: <color=\"black\">A</color>\nType: <color=\"black\">Rice Cooker</color>\n\nAn advanced rice cooker from Japan - Odango has advanced kawaii software and a passion for all things rice!";

	// Token: 0x040006DB RID: 1755
	private const string ShibukiBody = "Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">24</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Ninja</color>\n\nA student of the ancient Shinobi arts, this naughty ninja will steal your heart. Or possibly stab it. ";

	// Token: 0x040006DC RID: 1756
	private const string SirinaBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">21 quintillion ops</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">A.I. Assistant</color>\n\nA cutting edge AI Assistant to organize your calendar, brighten your day, and delete your browsing history during emergencies.";

	// Token: 0x040006DD RID: 1757
	private const string CataraBody = "Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">LightningCat</color>\n\nA feisty feline with a need for speed! She will (quickly) teach you the mysterious art of Speed Dating.";

	// Token: 0x040006DE RID: 1758
	private const string VellatrixBody = "Height: <color=\"black\">5'6\"</color>\nAge: <color=\"black\">394</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Headmaster</color>\n\nThe Headmaster of Snogwarts can teach you things you'll never forget. Dark, terrible, sexy things...";

	// Token: 0x040006DF RID: 1759
	private const string RoxxyBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">20</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Dancing Queen</color>\n\nA raving, misbehaving, dance diva! She's here for a good time, not a long time, so show her your moves!";

	// Token: 0x040006E0 RID: 1760
	private const string TessaBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">133 Dog Years</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Best Friend</color>\n\nThis lovely loyal lady is dying for a tummy rub! You two are sure to have a ball!";

	// Token: 0x040006E1 RID: 1761
	private const string ClaudiaBody = "Height: <color=\"black\">5'8\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Brooding</color>\n\nA super serious, grim dark sweetheart with a sword.";

	// Token: 0x040006E2 RID: 1762
	private const string RosaBody = "Height: <color=\"black\">5'10\"</color>\nAge: <color=\"black\">31</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Dramatic</color>\n\nThis beautiful woman cares for the sick, protects the children, and dances her heart out. Go to her!";

	// Token: 0x040006E3 RID: 1763
	private const string JulietBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">18</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Royalty</color>\n\nA vain but vivacious girl of taste and class. Don’t be mediocre.";

	// Token: 0x040006E4 RID: 1764
	private const string WendyBody = "Height: <color=\"black\">5'5\"</color>\nAge: <color=\"black\">23</color>\nBust: <color=\"black\">E</color>\nType: <color=\"black\">Delivery Gal</color>\n\nBig and tasty - Wendy is the foodie beauty looking for fun and delicious surprises.";

	// Token: 0x040006E5 RID: 1765
	private const string RuriBody = "Height: <color=\"black\">5'</color>\nAge: <color=\"black\">20</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Hacker</color>\n\nGamer / Hacker with an \"ice cream\" personality. Sweet and cold.";

	// Token: 0x040006E6 RID: 1766
	private const string GenericaBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">19 Forever</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Ghost</color>\n\nAn apparition of a lost love. Can you rescue her, even now?";

	// Token: 0x040006E7 RID: 1767
	private const string SawyerBody = "Height: <color=\"black\">5'5\"</color>\nAge: <color=\"black\">22</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Athlete</color>\n\nWarm-hearted and down to earth, this athlete is your perfect girl next door.";

	// Token: 0x040006E8 RID: 1768
	private const string SuzuBody = "Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Fox Spirit</color>\n\nA shy and mysterious Kitsune. She'll bring you joy and good fortune... if you can earn her trust.";

	// Token: 0x040006E9 RID: 1769
	private const string LustatBody = "Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">??</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Vampire</color>\n\nA vamp as quirky as she is bloodthirsty! Time spent with her would not be wasted...";

	// Token: 0x040006EA RID: 1770
	private const string ExploraBody = "Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">%AGE%</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Browser</color>\n\nHer days may be numbered, but her love is like the internet - forever...";

	// Token: 0x040006EB RID: 1771
	private const string EsperBody = "Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">24</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Psychic</color>\n\nShe may have the Second Sight, but she's only got eyes for you...";

	// Token: 0x040006EC RID: 1772
	private const string ReneeBody = "Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">25</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Childhood Friend</color>\n\nShe loved you before you were cool.";

	// Token: 0x040006ED RID: 1773
	private const string MalloryBody = "Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Wholesome</color>\n\nNo matter how bad things get... There'll be another sunrise tomorrow.";

	// Token: 0x040006EE RID: 1774
	private const string LakeBody = "Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Artist</color>\n\nA sensitive artist with a head full of thoughts. Deep, deep thoughts.";

	// Token: 0x040006EF RID: 1775
	private AssetBundle _biosBundle;

	// Token: 0x040006F0 RID: 1776
	private AssetBundle _achievementsBundle;

	// Token: 0x040006F1 RID: 1777
	private readonly Dictionary<Balance.GirlName, string> _nameToTextDic = new Dictionary<Balance.GirlName, string>
	{
		{
			Balance.GirlName.Darya,
			"Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">100,069</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Diamond</color>\n\nThis majestic and powerful entity hails from a distant galaxy, and can abiogenerate.  In other words - she makes you Diamonds."
		},
		{
			Balance.GirlName.Charlotte,
			"Height: <color=\"black\">4'11\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Gothic Lolita</color>\n\nOpen the Crypt to unlock this gothic gal, all of her outfits, and the grim new Job - \"Grave Digger\"."
		},
		{
			Balance.GirlName.Odango,
			"Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">Ver 2.0</color>\nBust: <color=\"black\">A</color>\nType: <color=\"black\">Rice Cooker</color>\n\nAn advanced rice cooker from Japan - Odango has advanced kawaii software and a passion for all things rice!"
		},
		{
			Balance.GirlName.Shibuki,
			"Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">24</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Ninja</color>\n\nA student of the ancient Shinobi arts, this naughty ninja will steal your heart. Or possibly stab it. "
		},
		{
			Balance.GirlName.Sirina,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">21 quintillion ops</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">A.I. Assistant</color>\n\nA cutting edge AI Assistant to organize your calendar, brighten your day, and delete your browsing history during emergencies."
		},
		{
			Balance.GirlName.Catara,
			"Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">LightningCat</color>\n\nA feisty feline with a need for speed! She will (quickly) teach you the mysterious art of Speed Dating."
		},
		{
			Balance.GirlName.Vellatrix,
			"Height: <color=\"black\">5'6\"</color>\nAge: <color=\"black\">394</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Headmaster</color>\n\nThe Headmaster of Snogwarts can teach you things you'll never forget. Dark, terrible, sexy things..."
		},
		{
			Balance.GirlName.Roxxy,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">20</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Dancing Queen</color>\n\nA raving, misbehaving, dance diva! She's here for a good time, not a long time, so show her your moves!"
		},
		{
			Balance.GirlName.Tessa,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">133 Dog Years</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Best Friend</color>\n\nThis lovely loyal lady is dying for a tummy rub! You two are sure to have a ball!"
		},
		{
			Balance.GirlName.Claudia,
			"Height: <color=\"black\">5'8\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Brooding</color>\n\nA super serious, grim dark sweetheart with a sword."
		},
		{
			Balance.GirlName.Rosa,
			"Height: <color=\"black\">5'10\"</color>\nAge: <color=\"black\">31</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Dramatic</color>\n\nThis beautiful woman cares for the sick, protects the children, and dances her heart out. Go to her!"
		},
		{
			Balance.GirlName.Juliet,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">18</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Royalty</color>\n\nA vain but vivacious girl of taste and class. Don’t be mediocre."
		},
		{
			Balance.GirlName.Wendy,
			"Height: <color=\"black\">5'5\"</color>\nAge: <color=\"black\">23</color>\nBust: <color=\"black\">E</color>\nType: <color=\"black\">Delivery Gal</color>\n\nBig and tasty - Wendy is the foodie beauty looking for fun and delicious surprises."
		},
		{
			Balance.GirlName.Ruri,
			"Height: <color=\"black\">5'</color>\nAge: <color=\"black\">20</color>\nBust: <color=\"black\">B</color>\nType: <color=\"black\">Hacker</color>\n\nGamer / Hacker with an \"ice cream\" personality. Sweet and cold."
		},
		{
			Balance.GirlName.Generica,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">19 Forever</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Ghost</color>\n\nAn apparition of a lost love. Can you rescue her, even now?"
		},
		{
			Balance.GirlName.Sawyer,
			"Height: <color=\"black\">5'5\"</color>\nAge: <color=\"black\">22</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Athlete</color>\n\nWarm-hearted and down to earth, this athlete is your perfect girl next door."
		},
		{
			Balance.GirlName.Suzu,
			"Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">19</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Fox Spirit</color>\n\nA shy and mysterious Kitsune. She'll bring you joy and good fortune... if you can earn her trust."
		},
		{
			Balance.GirlName.Lustat,
			"Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">??</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Vampire</color>\n\nA vamp as quirky as she is bloodthirsty! Time spent with her would not be wasted..."
		},
		{
			Balance.GirlName.Explora,
			"Height: <color=\"black\">5'3\"</color>\nAge: <color=\"black\">%AGE%</color>\nBust: <color=\"black\">DD</color>\nType: <color=\"black\">Browser</color>\n\nHer days may be numbered, but her love is like the internet - forever..."
		},
		{
			Balance.GirlName.Esper,
			"Height: <color=\"black\">5'2\"</color>\nAge: <color=\"black\">24</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Psychic</color>\n\nShe may have the Second Sight, but she's only got eyes for you..."
		},
		{
			Balance.GirlName.Renee,
			"Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">25</color>\nBust: <color=\"black\">D</color>\nType: <color=\"black\">Childhood Friend</color>\n\nShe loved you before you were cool."
		},
		{
			Balance.GirlName.Mallory,
			"Height: <color=\"black\">5'1\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Wholesome</color>\n\nNo matter how bad things get... There'll be another sunrise tomorrow."
		},
		{
			Balance.GirlName.Lake,
			"Height: <color=\"black\">5'4\"</color>\nAge: <color=\"black\">21</color>\nBust: <color=\"black\">C</color>\nType: <color=\"black\">Artist</color>\n\nA sensitive artist with a head full of thoughts. Deep, deep thoughts."
		}
	};
}
