using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000110 RID: 272
public class GirlStats : MonoBehaviour
{
	// Token: 0x06000699 RID: 1689 RVA: 0x00037F64 File Offset: 0x00036164
	public void Init(GirlModel data)
	{
		StatsModel statsModel = Universe.Stats[data.Id];
		if (this.Name == null)
		{
			this.Name = base.transform.Find("Name").GetComponent<Text>();
			this.Age = base.transform.Find("Grid").GetChild(0).Find("Contents").GetComponent<Text>();
			this.Birthday = base.transform.Find("Grid").GetChild(1).Find("Contents").GetComponent<Text>();
			this.Hobby = base.transform.Find("Grid").GetChild(2).Find("Contents").GetComponent<Text>();
			this.BloodType = base.transform.Find("Grid").GetChild(3).Find("Contents").GetComponent<Text>();
			this.FavJob = base.transform.Find("Grid").GetChild(4).Find("Contents").GetComponent<Text>();
			this.FavFood = base.transform.Find("Grid").GetChild(5).Find("Contents").GetComponent<Text>();
			this.GiftPref = base.transform.Find("Grid").GetChild(6).Find("Contents").GetComponent<Text>();
			this.Occupation = base.transform.Find("Grid").GetChild(7).Find("Contents").GetComponent<Text>();
			this.LikedTraits = base.transform.Find("Grid").GetChild(8).Find("Contents").GetComponent<Text>();
			this.Bust = base.transform.Find("Grid").GetChild(9).Find("Contents").GetComponent<Text>();
		}
		string text = Translations.TranslateJob((Requirement.JobType)(1 << (int)(statsModel.FavouriteJob.Id - 1)), 0);
		bool flag = false;
		if (Translations.PreferredLanguage != Translations.Language.Hindi || Translations.PreferredLanguage != Translations.Language.Chinese || Translations.PreferredLanguage != Translations.Language.Japanese || Translations.PreferredLanguage != Translations.Language.Russian)
		{
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < text.Length - 1; i++)
			{
				if (i == 0)
				{
					text = text.Substring(0, 1).ToUpperInvariant() + text.Substring(1).ToLowerInvariant();
				}
				else if (text[i] == ' ')
				{
					text = text.Substring(0, i + 1) + text.Substring(i + 1, 1).ToUpperInvariant() + text.Substring(i + 2).ToLowerInvariant();
				}
			}
		}
		Balance.GirlName name = Utilities.GirlFromString(data.Name);
		int name2 = (int)name;
		string arg = name2.ToString();
		this.Name.text = Translations.GetTranslation("everything_else_13_0", "Name:") + " " + Translations.TranslateGirlName(name);
		if (name == Balance.GirlName.Explora)
		{
			int num = DateTime.Now.Year - 1995 - 1;
			if (DateTime.Now.Month > 8 || (DateTime.Now.Month == 8 && DateTime.Now.Day >= 16))
			{
				num++;
			}
			this.Age.text = num.ToString();
		}
		else
		{
			this.Age.text = Translations.GetTranslation(string.Format("stats_{0}_1", arg), statsModel.Age);
		}
		this.Birthday.text = Translations.GetTranslation(string.Format("stats_{0}_2", arg), statsModel.Birthday);
		this.Hobby.text = Translations.GetTranslation(string.Format("stats_{0}_3", arg), statsModel.FavouriteHobby);
		string text2 = Translations.TranslateBlood(statsModel.BloodType);
		if (text2.Contains("`"))
		{
			text2 = text2.Substring(0, text2.IndexOf('`')).Trim();
		}
		this.BloodType.text = text2;
		this.FavJob.text = text;
		this.FavFood.text = Translations.GetTranslation(string.Format("stats_{0}_6", arg), statsModel.FavouriteFood);
		this.GiftPref.text = Translations.TranslateGift((Requirement.GiftType)(1 << (int)(statsModel.FavouriteGift.Id - 1)), name);
		this.Occupation.text = Translations.GetTranslation(string.Format("stats_{0}_8", arg), statsModel.Occupation);
		this.LikedTraits.text = Translations.TranslateSkill((Requirement.Skill)(statsModel.FavouriteSkill.Id - 1));
		string text3 = Translations.TranslateBust(statsModel.Bust);
		if (text3.Contains("`"))
		{
			text3 = text3.Substring(0, text3.IndexOf('`')).Trim();
		}
		this.Bust.text = text3;
		if (name == Balance.GirlName.Mio)
		{
			this.Hobby.text = Translations.GetTranslation("stats_19_3", "Gaming");
		}
		if (name == Balance.GirlName.Iro)
		{
			Text favJob = this.FavJob;
			string translation = Translations.GetTranslation("stats_5_5", "Sports");
			this.Hobby.text = translation;
			favJob.text = translation;
		}
		if (name == Balance.GirlName.Nutaku && GameState.NSFW)
		{
			this.Hobby.text = Translations.GetTranslation("stats_20_3", "Sex");
			this.GiftPref.text = Translations.GetTranslation("stats_20_7", "Lingerie");
		}
		base.gameObject.SetActive(true);
		base.transform.Find("Memory Album").GetComponent<Button>().onClick.RemoveAllListeners();
		base.transform.Find("Memory Album").GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.transform.Find("Popups/Memory Album").GetComponent<Album>().InitPage((int)name);
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Clicky00);
		});
	}

	// Token: 0x04000695 RID: 1685
	private Text Name;

	// Token: 0x04000696 RID: 1686
	private Text Age;

	// Token: 0x04000697 RID: 1687
	private Text Birthday;

	// Token: 0x04000698 RID: 1688
	private Text Hobby;

	// Token: 0x04000699 RID: 1689
	private Text BloodType;

	// Token: 0x0400069A RID: 1690
	private Text FavJob;

	// Token: 0x0400069B RID: 1691
	private Text FavFood;

	// Token: 0x0400069C RID: 1692
	private Text GiftPref;

	// Token: 0x0400069D RID: 1693
	private Text Occupation;

	// Token: 0x0400069E RID: 1694
	private Text LikedTraits;

	// Token: 0x0400069F RID: 1695
	private Text Bust;
}
