using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Requirement
{
	// Token: 0x06000700 RID: 1792 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
	private Requirement(string text, Requirement.UpdateRequirementDelegate requirement, Requirement.RequirementType type)
	{
		this.requirementText = text;
		this.UpdateRequirement = requirement;
		this.Type = type;
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0003C8E4 File Offset: 0x0003AAE4
	private Requirement(string text, Requirement.UpdateRequirementDelegate requirement, Requirement.RequirementType type, Girl girl, int giftCount, int dateCount)
	{
		this.requirementText = text;
		this.UpdateRequirement = requirement;
		this.Type = type;
		this.girl = girl;
		this.giftCount = giftCount;
		this.dateCount = dateCount;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0003C934 File Offset: 0x0003AB34
	public static int IndexFromDateType(Requirement.DateType type)
	{
		switch (type)
		{
		case Requirement.DateType.MoonlightStroll:
			return 0;
		default:
			if (type == Requirement.DateType.MovieTheater)
			{
				return 1;
			}
			if (type != Requirement.DateType.Beach)
			{
				return 0;
			}
			return 3;
		case Requirement.DateType.Sightseeing:
			return 2;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x0003C978 File Offset: 0x0003AB78
	// (set) Token: 0x06000704 RID: 1796 RVA: 0x0003C980 File Offset: 0x0003AB80
	public int Index { get; set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x0003C98C File Offset: 0x0003AB8C
	// (set) Token: 0x06000706 RID: 1798 RVA: 0x0003C994 File Offset: 0x0003AB94
	public Requirement.GiftType Gift { get; set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x0003C9A0 File Offset: 0x0003ABA0
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x0003C9A8 File Offset: 0x0003ABA8
	public Requirement.DateType Date { get; set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x0003C9B4 File Offset: 0x0003ABB4
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x0003C9BC File Offset: 0x0003ABBC
	public int DateCount { get; set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
	// (set) Token: 0x0600070C RID: 1804 RVA: 0x0003CA44 File Offset: 0x0003AC44
	public bool Dirty
	{
		get
		{
			if (this.girl != null)
			{
				if (this.Type == Requirement.RequirementType.Gift && this.lastValue != this.girl.GiftCount[this.Index])
				{
					return true;
				}
				if (this.Type == Requirement.RequirementType.Date && this.lastValue != this.girl.DateCount[this.Index])
				{
					return true;
				}
			}
			return this.dirty;
		}
		set
		{
			this.dirty = value;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0003CA50 File Offset: 0x0003AC50
	public string Text
	{
		get
		{
			this.dirty = false;
			if (this.Type == Requirement.RequirementType.Gift && this.giftCount == 1 && this.girl.GirlName == Balance.GirlName.Esper && Math.Min(this.giftCount, this.girl.GiftCount[this.Index]) == 0)
			{
				return "Unknown Gift";
			}
			if (this.Type == Requirement.RequirementType.Gift && this.giftCount > 1)
			{
				this.lastValue = this.girl.GiftCount[this.Index];
				if (Math.Min(this.giftCount, this.girl.GiftCount[this.Index]) == 0 && this.girl.GirlName == Balance.GirlName.Esper)
				{
					return string.Format("0/{0} Unknown Gift", this.giftCount.ToString("n0"));
				}
				if (this.giftCount < 1000)
				{
					return string.Format("{0}/{1} {2}", Math.Min(this.giftCount, this.girl.GiftCount[this.Index]).ToString("n0"), this.giftCount.ToString("n0"), this.requirementText);
				}
				return string.Format("{0}/{1}\n{2}", Math.Min(this.giftCount, this.girl.GiftCount[this.Index]).ToString("n0"), this.giftCount.ToString("n0"), this.requirementText);
			}
			else
			{
				if (this.Type != Requirement.RequirementType.Date || this.dateCount <= 1)
				{
					return this.requirementText;
				}
				this.lastValue = this.girl.DateCount[this.Index];
				if (Math.Min(this.dateCount, this.girl.DateCount[this.Index]) == 0 && this.girl.GirlName == Balance.GirlName.Esper)
				{
					return string.Format("0/{0} Unknown Date", this.dateCount.ToString("n0"));
				}
				if (this.dateCount < 1000)
				{
					return string.Format("{0}/{1} {2}", Math.Min(this.dateCount, this.girl.DateCount[this.Index]).ToString("n0"), this.dateCount.ToString("n0"), this.requirementText);
				}
				return string.Format("{0}/{1}\n{2}", Math.Min(this.dateCount, this.girl.DateCount[this.Index]).ToString("n0"), this.dateCount.ToString("n0"), this.requirementText);
			}
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600070E RID: 1806 RVA: 0x0003CD0C File Offset: 0x0003AF0C
	// (set) Token: 0x0600070F RID: 1807 RVA: 0x0003CD14 File Offset: 0x0003AF14
	public Requirement.UpdateRequirementDelegate UpdateRequirement { get; private set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000710 RID: 1808 RVA: 0x0003CD20 File Offset: 0x0003AF20
	// (set) Token: 0x06000711 RID: 1809 RVA: 0x0003CD28 File Offset: 0x0003AF28
	public Requirement.RequirementType Type { get; private set; }

	// Token: 0x06000712 RID: 1810 RVA: 0x0003CD34 File Offset: 0x0003AF34
	public static Requirement NewPrestigeRequirement(float prestige, bool consume = false)
	{
		string translation;
		if (consume)
		{
			translation = Translations.GetTranslation("requirements_2_0", "Consume {0}x Reset Boost");
		}
		else
		{
			translation = Translations.GetTranslation("requirements_1_0", "{0}x Reset Boost");
		}
		string text = string.Format(translation, prestige.ToString("n0"));
		Requirement.UpdateRequirementDelegate requirement = () => GameState.CurrentState.TimeMultiplier.Value >= ((!consume) ? prestige : (prestige + 1f));
		return new Requirement(text, requirement, (!consume) ? Requirement.RequirementType.Prestige : Requirement.RequirementType.PrestigeConsume);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0003CDC4 File Offset: 0x0003AFC4
	public static Requirement NewSkillRequirement(Requirement.Skill skill, int level)
	{
		GameState.UnlockedHobbies = Mathf.Max(GameState.UnlockedHobbies, (int)(skill + 1));
		string text = string.Format("Lvl {0} {1}", level.ToString(), Translations.TranslateSkill(skill));
		Requirement.UpdateRequirementDelegate requirement = () => Skills.SkillLevel[(int)skill] >= level;
		return new Requirement(text, requirement, Requirement.RequirementType.Skill);
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0003CE34 File Offset: 0x0003B034
	public static Requirement NewSkillGildRequirement(int total)
	{
		GameState.UnlockedHobbies = Mathf.Max(GameState.UnlockedHobbies, total);
		string text = (total != 1) ? ((total != 12) ? string.Format("Gild Any {0} Hobbies", total.ToString()) : "Gild All Hobbies") : "Gild Any 1 Hobby";
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			int num = 0;
			foreach (Hobby2 hobby in Hobby2.ActiveHobbies)
			{
				if (hobby.Gilded)
				{
					num++;
				}
			}
			return num >= total;
		};
		return new Requirement(text, requirement, Requirement.RequirementType.Skill);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0003CEBC File Offset: 0x0003B0BC
	public static Requirement NewAllSkillRequirement(int level)
	{
		GameState.UnlockedHobbies = 12;
		string text = string.Format("All Hobbies Lvl {0}", level.ToString());
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			foreach (int num in Skills.SkillLevel)
			{
				if (num < level)
				{
					return false;
				}
			}
			return true;
		};
		return new Requirement(text, requirement, Requirement.RequirementType.Skill);
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0003CF08 File Offset: 0x0003B108
	public static Requirement NewMoneyRequirement(double money, bool consume = false)
	{
		string text = (money != 1337.0) ? Utilities.ToPrefixedNumber(money, true, false) : "1337";
		Requirement.UpdateRequirementDelegate requirement = () => GameState.Money.Value >= money;
		return new Requirement(text, requirement, Requirement.RequirementType.Money);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0003CF64 File Offset: 0x0003B164
	public static Requirement NewJobRequirement(short id, int level)
	{
		JobModel jobModel = Universe.Jobs[id];
		Requirement.JobType job = (Requirement.JobType)(1 << (int)(id - 1));
		short[] unlockHobby = jobModel.UnlockHobby;
		if (unlockHobby != null)
		{
			for (int i = 0; i < unlockHobby.Length; i++)
			{
				GameState.UnlockedHobbies = Mathf.Max(GameState.UnlockedHobbies, (int)(unlockHobby[i] - 2));
			}
		}
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			Job2 job;
			if ((Job2.AvailableJobs & job) != Requirement.JobType.None)
			{
				using (List<Job2>.Enumerator enumerator = Job2.ActiveJobs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						job = enumerator.Current;
						if (level > 0 && job.JobType == job && job.Level >= level)
						{
							return true;
						}
						if (level == 0 && job.JobType == job && (job.Experience > 0L || job.Level > 0))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		};
		string text = Translations.TranslateJob(job, 0);
		bool flag = false;
		if (Translations.PreferredLanguage != Translations.Language.Hindi && Translations.PreferredLanguage != Translations.Language.Chinese && Translations.PreferredLanguage != Translations.Language.Japanese && Translations.PreferredLanguage != Translations.Language.Russian)
		{
			flag = true;
		}
		if (flag)
		{
			string str = text.Substring(0, 1);
			text = text.ToLowerInvariant().Substring(1, text.Length - 1);
			text = str + text;
		}
		if (level == 0)
		{
			return new Requirement(string.Format(Translations.GetTranslation("requirements_0_0", "Work at {0}"), text), requirement, Requirement.RequirementType.Job);
		}
		return new Requirement(Translations.TranslateJob(job, level + 1) + "\n(" + text + ")", requirement, Requirement.RequirementType.Job);
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
	public static Requirement NewJobGildRequirement(int total)
	{
		string text = (total != 1) ? string.Format("Gild Any {0} Jobs", total.ToString()) : "Gild Any 1 Job";
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			int num = 0;
			foreach (Job2 job in Job2.ActiveJobs)
			{
				if (job.Gilded)
				{
					num++;
				}
			}
			return num >= total;
		};
		return new Requirement(text, requirement, Requirement.RequirementType.Job);
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0003D10C File Offset: 0x0003B30C
	public static Requirement NewGiftRequirement(int index, Girl girl, Requirement.GiftType gift)
	{
		Requirement.UpdateRequirementDelegate requirement = () => girl.GiftCount[index] > 0;
		return new Requirement(Translations.TranslateGift(gift, girl.GirlName), requirement, Requirement.RequirementType.Gift)
		{
			Gift = gift
		};
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0003D15C File Offset: 0x0003B35C
	public static Requirement NewGiftRequirement(int index, Girl girl, Requirement.GiftType gift, int count)
	{
		Requirement.UpdateRequirementDelegate requirement = () => girl.GiftCount[index] >= count;
		return new Requirement(Translations.TranslateGift(gift, girl.GirlName), requirement, Requirement.RequirementType.Gift, girl, count, 0)
		{
			Gift = gift
		};
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0003D1C0 File Offset: 0x0003B3C0
	public static Requirement.GiftType GiftFromId(short giftId)
	{
		Requirement.GiftType result = Requirement.GiftType.Shell;
		switch (giftId)
		{
		case 1:
			result = Requirement.GiftType.Shell;
			break;
		case 2:
			result = Requirement.GiftType.Rose;
			break;
		case 3:
			result = Requirement.GiftType.HandLotion;
			break;
		case 4:
			result = Requirement.GiftType.Donut;
			break;
		case 5:
			result = Requirement.GiftType.FruitBasket;
			break;
		case 6:
			result = Requirement.GiftType.Chocolates;
			break;
		case 7:
			result = Requirement.GiftType.Book;
			break;
		case 8:
			result = Requirement.GiftType.Earrings;
			break;
		case 9:
			result = Requirement.GiftType.Drink;
			break;
		case 10:
			result = Requirement.GiftType.Flowers;
			break;
		case 11:
			result = Requirement.GiftType.Cake;
			break;
		case 12:
			result = Requirement.GiftType.PlushyToy;
			break;
		case 13:
			result = Requirement.GiftType.TeaSet;
			break;
		case 14:
			result = Requirement.GiftType.Shoes;
			break;
		case 15:
			result = Requirement.GiftType.CutePuppy;
			break;
		case 16:
			result = Requirement.GiftType.Necklace;
			break;
		case 17:
			result = Requirement.GiftType.DesignerBag;
			break;
		case 18:
			result = Requirement.GiftType.NewCar;
			break;
		case 19:
			result = Requirement.GiftType.Christmas;
			break;
		case 20:
			result = Requirement.GiftType.SchoolUniform;
			break;
		case 21:
			result = Requirement.GiftType.BathingSuit;
			break;
		case 22:
			result = Requirement.GiftType.Unique;
			break;
		case 23:
			result = Requirement.GiftType.DiamondRing;
			break;
		case 24:
			result = Requirement.GiftType.USB;
			break;
		case 25:
			result = Requirement.GiftType.Potion;
			break;
		case 26:
			result = Requirement.GiftType.MagicCandles;
			break;
		case 27:
			result = Requirement.GiftType.EnchantedScarf;
			break;
		case 28:
			result = Requirement.GiftType.BewitchedJam;
			break;
		case 29:
			result = Requirement.GiftType.MysticSlippers;
			break;
		case 30:
			result = Requirement.GiftType.Lingerie;
			break;
		case 31:
			result = Requirement.GiftType.Nude;
			break;
		}
		return result;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0003D398 File Offset: 0x0003B598
	public static Requirement NewGiftRequirement(int index, Girl girl, short giftId, int count = 1)
	{
		Requirement.GiftType gift = Requirement.GiftFromId(giftId);
		Requirement.UpdateRequirementDelegate requirement = () => girl.GiftCount[index] >= count;
		return new Requirement(Translations.TranslateGift(gift, girl.GirlName), requirement, Requirement.RequirementType.Gift, girl, count, 0)
		{
			Gift = gift
		};
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0003D404 File Offset: 0x0003B604
	public static Requirement NewDateRequirement(int index, Girl girl, short dateId, int count = 1)
	{
		Requirement.DateType dateType = Universe.Dates[dateId].DateType;
		Requirement.UpdateRequirementDelegate requirement = () => girl.DateCount[index] >= count;
		return new Requirement(Translations.TranslateDate(dateType), requirement, Requirement.RequirementType.Date, girl, 0, count)
		{
			Date = dateType,
			DateCount = count
		};
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0003D47C File Offset: 0x0003B67C
	public static Requirement NewAchievementRequirement(Girl girl, int count)
	{
		string translation = Translations.GetTranslation("requirements_8_0", "{0} Achievements");
		Requirement.UpdateRequirementDelegate requirement = () => GameState.GetAchievements().AchievementCount >= count;
		return new Requirement(string.Format(translation, count.ToString()), requirement, Requirement.RequirementType.Achievement);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0003D4CC File Offset: 0x0003B6CC
	public static Requirement NewDiamondRequirement(Girl girl, int count, bool consume = false)
	{
		string translation = Translations.GetTranslation("requirements_6_0", "1 Diamond");
		if (count > 1)
		{
			translation = Translations.GetTranslation("requirements_32_0", "{0} Diamonds");
		}
		Requirement.UpdateRequirementDelegate requirement = () => GameState.Diamonds.Value >= count;
		return new Requirement(string.Format(translation, count.ToString()), requirement, Requirement.RequirementType.Diamond);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0003D538 File Offset: 0x0003B738
	public static Requirement NewJobsMaxLevelRequirement()
	{
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			foreach (Job2 job in Job2.ActiveJobs)
			{
				if (job.JobType != Requirement.JobType.Digger && job.JobType != Requirement.JobType.Planter)
				{
					if (job.Level != job.MaxLevel)
					{
						return false;
					}
				}
			}
			return true;
		};
		return new Requirement(Translations.GetTranslation("requirements_4_0", "Max Level All Jobs"), requirement, Requirement.RequirementType.Job);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0003D57C File Offset: 0x0003B77C
	public static Requirement NewFullAlbumRequirement(Girl girl)
	{
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			bool flag = true;
			short num = 1;
			while (num < girl.Data.Id && flag)
			{
				Girl girl2 = Girl.FindGirl(Utilities.GirlFromString(Universe.Girls[num].Name));
				if (!(girl2 == null))
				{
					if (girl2.Love < 9 || (girl2.LifetimeDates & (Requirement.DateType.MoonlightStroll | Requirement.DateType.Sightseeing | Requirement.DateType.MovieTheater | Requirement.DateType.Beach)) != (Requirement.DateType.MoonlightStroll | Requirement.DateType.Sightseeing | Requirement.DateType.MovieTheater | Requirement.DateType.Beach))
					{
						flag = false;
					}
				}
				num += 1;
			}
			return flag;
		};
		return new Requirement(Translations.GetTranslation("requirements_7_0", "Full Memory Album"), requirement, Requirement.RequirementType.Album);
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0003D5BC File Offset: 0x0003B7BC
	public static Requirement NewDateCountRequirement(int count)
	{
		string translation = Translations.GetTranslation("requirements_30_0", "Lifetime Dates");
		Requirement.UpdateRequirementDelegate requirement = () => GameState.DateCount >= count;
		return new Requirement(string.Format("{0} {1}", count.ToString("n0"), translation), requirement, Requirement.RequirementType.TotalDates);
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0003D618 File Offset: 0x0003B818
	public static Requirement NewGiftCountRequirement(int count)
	{
		string translation = Translations.GetTranslation("requirements_5_0", "{0} Lifetime Gifts");
		Requirement.UpdateRequirementDelegate requirement = () => GameState.GiftCount >= count;
		return new Requirement(string.Format(translation, count.ToString("n0")), requirement, Requirement.RequirementType.TotalGifts);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0003D670 File Offset: 0x0003B870
	public static Requirement NewTimeBlockRequirement(int count)
	{
		string translation = Translations.GetTranslation("requirements_3_0", "Consume {0} Time Block");
		Requirement.UpdateRequirementDelegate requirement = () => FreeTime.Free >= count;
		return new Requirement(string.Format(translation, count.ToString()), requirement, Requirement.RequirementType.Time);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0003D6C0 File Offset: 0x0003B8C0
	public static Requirement NewGirlsAtLoverRequirement(int count)
	{
		string format = "{0} Girls At Lover";
		Requirement.UpdateRequirementDelegate requirement = delegate()
		{
			if (Girl.ActiveGirls == null)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < Girl.ActiveGirls.Count; i++)
			{
				if (Girl.ActiveGirls[i] != null && Girl.ActiveGirls[i].Love >= 9)
				{
					num++;
				}
			}
			return num >= count;
		};
		return new Requirement(string.Format(format, count.ToString()), requirement, Requirement.RequirementType.GirlsAtLover);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0003D708 File Offset: 0x0003B908
	public override string ToString()
	{
		return this.requirementText;
	}

	// Token: 0x040006FD RID: 1789
	private string requirementText;

	// Token: 0x040006FE RID: 1790
	private int giftCount;

	// Token: 0x040006FF RID: 1791
	private int dateCount;

	// Token: 0x04000700 RID: 1792
	private Girl girl;

	// Token: 0x04000701 RID: 1793
	private int lastValue = -1;

	// Token: 0x04000702 RID: 1794
	private bool dirty = true;

	// Token: 0x0200011F RID: 287
	public enum RequirementType
	{
		// Token: 0x0400070B RID: 1803
		Skill,
		// Token: 0x0400070C RID: 1804
		Money,
		// Token: 0x0400070D RID: 1805
		Job,
		// Token: 0x0400070E RID: 1806
		Hobby,
		// Token: 0x0400070F RID: 1807
		Time,
		// Token: 0x04000710 RID: 1808
		Affection,
		// Token: 0x04000711 RID: 1809
		Gift,
		// Token: 0x04000712 RID: 1810
		Date,
		// Token: 0x04000713 RID: 1811
		Heart,
		// Token: 0x04000714 RID: 1812
		Achievement,
		// Token: 0x04000715 RID: 1813
		Diamond,
		// Token: 0x04000716 RID: 1814
		Prestige,
		// Token: 0x04000717 RID: 1815
		PrestigeConsume,
		// Token: 0x04000718 RID: 1816
		Album,
		// Token: 0x04000719 RID: 1817
		TotalDates,
		// Token: 0x0400071A RID: 1818
		TotalGifts,
		// Token: 0x0400071B RID: 1819
		MoneyConsume,
		// Token: 0x0400071C RID: 1820
		DiamondConsume,
		// Token: 0x0400071D RID: 1821
		JobGild,
		// Token: 0x0400071E RID: 1822
		HobbyGild,
		// Token: 0x0400071F RID: 1823
		AllJobs,
		// Token: 0x04000720 RID: 1824
		AllHobbies,
		// Token: 0x04000721 RID: 1825
		GirlsAtLover,
		// Token: 0x04000722 RID: 1826
		Unknown
	}

	// Token: 0x02000120 RID: 288
	public enum Skill
	{
		// Token: 0x04000724 RID: 1828
		Suave,
		// Token: 0x04000725 RID: 1829
		Funny,
		// Token: 0x04000726 RID: 1830
		Buff,
		// Token: 0x04000727 RID: 1831
		TechSavvy,
		// Token: 0x04000728 RID: 1832
		Tenderness,
		// Token: 0x04000729 RID: 1833
		Motivation,
		// Token: 0x0400072A RID: 1834
		Wisdom,
		// Token: 0x0400072B RID: 1835
		Badass,
		// Token: 0x0400072C RID: 1836
		Smart,
		// Token: 0x0400072D RID: 1837
		Angst,
		// Token: 0x0400072E RID: 1838
		Mysterious,
		// Token: 0x0400072F RID: 1839
		Lucky
	}

	// Token: 0x02000121 RID: 289
	[Flags]
	public enum JobType
	{
		// Token: 0x04000731 RID: 1841
		None = 0,
		// Token: 0x04000732 RID: 1842
		Burger = 1,
		// Token: 0x04000733 RID: 1843
		Restaurant = 2,
		// Token: 0x04000734 RID: 1844
		Cleaning = 4,
		// Token: 0x04000735 RID: 1845
		Lifeguard = 8,
		// Token: 0x04000736 RID: 1846
		Art = 16,
		// Token: 0x04000737 RID: 1847
		Computers = 32,
		// Token: 0x04000738 RID: 1848
		Zoo = 64,
		// Token: 0x04000739 RID: 1849
		Hunting = 128,
		// Token: 0x0400073A RID: 1850
		Casino = 256,
		// Token: 0x0400073B RID: 1851
		Sports = 512,
		// Token: 0x0400073C RID: 1852
		Legal = 1024,
		// Token: 0x0400073D RID: 1853
		Movies = 2048,
		// Token: 0x0400073E RID: 1854
		Space = 4096,
		// Token: 0x0400073F RID: 1855
		Slaying = 8192,
		// Token: 0x04000740 RID: 1856
		Love = 16384,
		// Token: 0x04000741 RID: 1857
		Wizard = 32768,
		// Token: 0x04000742 RID: 1858
		Digger = 65536,
		// Token: 0x04000743 RID: 1859
		Planter = 131072
	}

	// Token: 0x02000122 RID: 290
	[Flags]
	public enum OutfitType
	{
		// Token: 0x04000745 RID: 1861
		None = 0,
		// Token: 0x04000746 RID: 1862
		Monster = 1,
		// Token: 0x04000747 RID: 1863
		Animated = 2,
		// Token: 0x04000748 RID: 1864
		DeluxeWedding = 4,
		// Token: 0x04000749 RID: 1865
		Christmas = 262144,
		// Token: 0x0400074A RID: 1866
		SchoolUniform = 524288,
		// Token: 0x0400074B RID: 1867
		BathingSuit = 1048576,
		// Token: 0x0400074C RID: 1868
		Unique = 2097152,
		// Token: 0x0400074D RID: 1869
		DiamondRing = 4194304,
		// Token: 0x0400074E RID: 1870
		Lingerie = 536870912,
		// Token: 0x0400074F RID: 1871
		Nude = 1073741824,
		// Token: 0x04000750 RID: 1872
		All = 1616642050
	}

	// Token: 0x02000123 RID: 291
	[Flags]
	public enum GiftType
	{
		// Token: 0x04000752 RID: 1874
		None = 0,
		// Token: 0x04000753 RID: 1875
		Shell = 1,
		// Token: 0x04000754 RID: 1876
		Rose = 2,
		// Token: 0x04000755 RID: 1877
		HandLotion = 4,
		// Token: 0x04000756 RID: 1878
		Donut = 8,
		// Token: 0x04000757 RID: 1879
		FruitBasket = 16,
		// Token: 0x04000758 RID: 1880
		Chocolates = 32,
		// Token: 0x04000759 RID: 1881
		Book = 64,
		// Token: 0x0400075A RID: 1882
		Earrings = 128,
		// Token: 0x0400075B RID: 1883
		Drink = 256,
		// Token: 0x0400075C RID: 1884
		Flowers = 512,
		// Token: 0x0400075D RID: 1885
		Cake = 1024,
		// Token: 0x0400075E RID: 1886
		PlushyToy = 2048,
		// Token: 0x0400075F RID: 1887
		TeaSet = 4096,
		// Token: 0x04000760 RID: 1888
		Shoes = 8192,
		// Token: 0x04000761 RID: 1889
		CutePuppy = 16384,
		// Token: 0x04000762 RID: 1890
		Necklace = 32768,
		// Token: 0x04000763 RID: 1891
		DesignerBag = 65536,
		// Token: 0x04000764 RID: 1892
		NewCar = 131072,
		// Token: 0x04000765 RID: 1893
		Christmas = 262144,
		// Token: 0x04000766 RID: 1894
		SchoolUniform = 524288,
		// Token: 0x04000767 RID: 1895
		BathingSuit = 1048576,
		// Token: 0x04000768 RID: 1896
		Unique = 2097152,
		// Token: 0x04000769 RID: 1897
		DiamondRing = 4194304,
		// Token: 0x0400076A RID: 1898
		USB = 8388608,
		// Token: 0x0400076B RID: 1899
		Potion = 16777216,
		// Token: 0x0400076C RID: 1900
		MagicCandles = 33554432,
		// Token: 0x0400076D RID: 1901
		EnchantedScarf = 67108864,
		// Token: 0x0400076E RID: 1902
		BewitchedJam = 134217728,
		// Token: 0x0400076F RID: 1903
		MysticSlippers = 268435456,
		// Token: 0x04000770 RID: 1904
		Lingerie = 536870912,
		// Token: 0x04000771 RID: 1905
		Nude = 1073741824,
		// Token: 0x04000772 RID: 1906
		Apple = 16,
		// Token: 0x04000773 RID: 1907
		CD = 2,
		// Token: 0x04000774 RID: 1908
		Java = 8,
		// Token: 0x04000775 RID: 1909
		Crypto = 64,
		// Token: 0x04000776 RID: 1910
		Magnet = 128,
		// Token: 0x04000777 RID: 1911
		Pizza = 256,
		// Token: 0x04000778 RID: 1912
		RAM = 16384,
		// Token: 0x04000779 RID: 1913
		Potato = 2,
		// Token: 0x0400077A RID: 1914
		BabyChick = 16,
		// Token: 0x0400077B RID: 1915
		Telescope = 256,
		// Token: 0x0400077C RID: 1916
		HerbalTea = 4096,
		// Token: 0x0400077D RID: 1917
		Soup = 16384,
		// Token: 0x0400077E RID: 1918
		Lozenge = 131072,
		// Token: 0x0400077F RID: 1919
		Medicine = 8388608
	}

	// Token: 0x02000124 RID: 292
	[Flags]
	public enum DateType
	{
		// Token: 0x04000781 RID: 1921
		MoonlightStroll = 1,
		// Token: 0x04000782 RID: 1922
		CoffeeShop = 2,
		// Token: 0x04000783 RID: 1923
		Sightseeing = 4,
		// Token: 0x04000784 RID: 1924
		MovieTheater = 8,
		// Token: 0x04000785 RID: 1925
		Beach = 16
	}

	// Token: 0x02000239 RID: 569
	// (Invoke) Token: 0x060011E8 RID: 4584
	public delegate bool UpdateRequirementDelegate();
}
