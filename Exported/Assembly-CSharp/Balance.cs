using System;
using System.Collections.Generic;

// Token: 0x02000032 RID: 50
public static class Balance
{
	// Token: 0x0600012E RID: 302 RVA: 0x00009920 File Offset: 0x00007B20
	public static Requirement.GiftType[] GetGiftTypes(Girl girl)
	{
		bool flag = false;
		Balance.GirlName girlName = girl.GirlName;
		switch (girlName)
		{
		case Balance.GirlName.Explora:
			if (girl.Love < 9)
			{
				return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
			}
			return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.exploraDefaultGifts : Balance.exploraWizardGifts;
		default:
			switch (girlName)
			{
			case Balance.GirlName.Bearverly:
			{
				if (girl.Love != 8 || girl.GiftCount[3] != 0)
				{
					return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
				}
				List<Requirement.GiftType> list = new List<Requirement.GiftType>(((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts);
				foreach (int num in girl.GiftCount)
				{
					if (num == 1)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(Requirement.GiftType.Potion);
				}
				return list.ToArray();
			}
			default:
			{
				if (girlName != Balance.GirlName.Generica)
				{
					return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
				}
				if (girl.Love != 8 || girl.GiftCount[1] != 0)
				{
					return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
				}
				List<Requirement.GiftType> list2 = new List<Requirement.GiftType>(((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts);
				foreach (int num2 in girl.GiftCount)
				{
					if (num2 == 1)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list2.Add(Requirement.GiftType.Potion);
				}
				return list2.ToArray();
			}
			case Balance.GirlName.Alpha:
			{
				if (girl.Love != 8 || girl.GiftCount[3] != 0)
				{
					return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
				}
				List<Requirement.GiftType> list3 = new List<Requirement.GiftType>(((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts);
				foreach (int num3 in girl.GiftCount)
				{
					if (num3 == 1)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list3.Add(Requirement.GiftType.USB);
				}
				return list3.ToArray();
			}
			}
			break;
		case Balance.GirlName.Mallory:
		{
			if (girl.Love != 8)
			{
				return ((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts;
			}
			List<Requirement.GiftType> list4 = new List<Requirement.GiftType>(((Job2.AvailableJobs & Requirement.JobType.Wizard) == Requirement.JobType.None) ? Balance.defaultGifts : Balance.wizardGifts);
			foreach (int num4 in girl.GiftCount)
			{
				if (num4 == 1)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list4.Add(Requirement.GiftType.USB);
			}
			return list4.ToArray();
		}
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00009C78 File Offset: 0x00007E78
	public static Requirement.OutfitType[] GetOutfitTypes(Girl girl)
	{
		Balance.GirlName girlName = girl.GirlName;
		switch (girlName)
		{
		case Balance.GirlName.Cassie:
			return (!GameState.NSFW) ? Balance.defaultOutfitsWithDeluxe : Balance.defaultOutfitsWithDeluxeNsfw;
		case Balance.GirlName.Mio:
		case Balance.GirlName.Quill:
			return (!GameState.NSFW) ? Balance.defaultOutfitsWithDeluxeUnique : Balance.defaultOutfitsWithDeluxeUniqueNsfw;
		default:
			switch (girlName)
			{
			case Balance.GirlName.Suzu:
			case Balance.GirlName.Explora:
				return (!GameState.NSFW) ? Balance.defaultOutfitsWithAnimated : Balance.defaultOutfitsWithAnimatedNsfw;
			default:
				if (girlName != Balance.GirlName.Mallory)
				{
					return (!GameState.NSFW) ? Balance.defaultOutfits : Balance.defaultOutfitsNsfw;
				}
				return (!GameState.NSFW) ? Balance.defaultOutfitsWithUniqueAnimated : Balance.defaultOutfitsWithUniqueAnimatedNsfw;
			}
			break;
		case Balance.GirlName.Nutaku:
		case Balance.GirlName.Ayano:
		case Balance.GirlName.Nina:
			return (!GameState.NSFW) ? Balance.defaultOutfitsWithUnique : Balance.defaultOutfitsWithUniqueNsfw;
		case Balance.GirlName.DarkOne:
		case Balance.GirlName.QPiddy:
		case Balance.GirlName.Jelle:
		case Balance.GirlName.Quillzone:
		case Balance.GirlName.Bonchovy:
		case Balance.GirlName.Spectrum:
			return Balance.noOutfits;
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00009DBC File Offset: 0x00007FBC
	public static int GetOutfitDiamondCost(Requirement.OutfitType outfit, Balance.GirlName girl)
	{
		if (outfit == Requirement.OutfitType.Lingerie || outfit == Requirement.OutfitType.Nude)
		{
			return 10;
		}
		if (girl == Balance.GirlName.Odango || girl == Balance.GirlName.Shibuki || girl == Balance.GirlName.Sirina || girl == Balance.GirlName.Vellatrix || girl == Balance.GirlName.Roxxy || girl == Balance.GirlName.Tessa || girl == Balance.GirlName.Claudia || girl == Balance.GirlName.Rosa || girl == Balance.GirlName.Juliet || girl == Balance.GirlName.Wendy || girl == Balance.GirlName.Ruri || girl == Balance.GirlName.Generica || girl == Balance.GirlName.Lustat || girl == Balance.GirlName.Sawyer || girl == Balance.GirlName.Explora || girl == Balance.GirlName.Esper || girl == Balance.GirlName.Renee || girl == Balance.GirlName.Lake)
		{
			return 0;
		}
		switch (outfit)
		{
		case Requirement.OutfitType.Monster:
		case Requirement.OutfitType.DeluxeWedding:
			break;
		default:
			if (outfit != Requirement.OutfitType.Unique)
			{
				return 10;
			}
			break;
		}
		return 0;
	}

	// Token: 0x04000163 RID: 355
	private static Requirement.GiftType[] defaultGifts = new Requirement.GiftType[]
	{
		Requirement.GiftType.Shell,
		Requirement.GiftType.Rose,
		Requirement.GiftType.HandLotion,
		Requirement.GiftType.Donut,
		Requirement.GiftType.FruitBasket,
		Requirement.GiftType.Chocolates,
		Requirement.GiftType.Book,
		Requirement.GiftType.Earrings,
		Requirement.GiftType.Drink,
		Requirement.GiftType.Flowers,
		Requirement.GiftType.Cake,
		Requirement.GiftType.PlushyToy,
		Requirement.GiftType.TeaSet,
		Requirement.GiftType.Shoes,
		Requirement.GiftType.CutePuppy,
		Requirement.GiftType.Necklace,
		Requirement.GiftType.DesignerBag,
		Requirement.GiftType.NewCar
	};

	// Token: 0x04000164 RID: 356
	private static Requirement.GiftType[] wizardGifts = new Requirement.GiftType[]
	{
		Requirement.GiftType.Shell,
		Requirement.GiftType.Rose,
		Requirement.GiftType.HandLotion,
		Requirement.GiftType.Donut,
		Requirement.GiftType.FruitBasket,
		Requirement.GiftType.Chocolates,
		Requirement.GiftType.Book,
		Requirement.GiftType.Earrings,
		Requirement.GiftType.Drink,
		Requirement.GiftType.Flowers,
		Requirement.GiftType.Cake,
		Requirement.GiftType.PlushyToy,
		Requirement.GiftType.TeaSet,
		Requirement.GiftType.Shoes,
		Requirement.GiftType.CutePuppy,
		Requirement.GiftType.Necklace,
		Requirement.GiftType.DesignerBag,
		Requirement.GiftType.NewCar,
		Requirement.GiftType.MagicCandles,
		Requirement.GiftType.EnchantedScarf,
		Requirement.GiftType.BewitchedJam,
		Requirement.GiftType.MysticSlippers
	};

	// Token: 0x04000165 RID: 357
	private static Requirement.GiftType[] exploraDefaultGifts = new Requirement.GiftType[]
	{
		Requirement.GiftType.Shell,
		Requirement.GiftType.Rose,
		Requirement.GiftType.HandLotion,
		Requirement.GiftType.Donut,
		Requirement.GiftType.Chocolates,
		Requirement.GiftType.Book,
		Requirement.GiftType.Drink,
		Requirement.GiftType.Flowers,
		Requirement.GiftType.Cake,
		Requirement.GiftType.PlushyToy,
		Requirement.GiftType.TeaSet,
		Requirement.GiftType.Shoes,
		Requirement.GiftType.CutePuppy,
		Requirement.GiftType.Necklace,
		Requirement.GiftType.DesignerBag,
		Requirement.GiftType.NewCar
	};

	// Token: 0x04000166 RID: 358
	private static Requirement.GiftType[] exploraWizardGifts = new Requirement.GiftType[]
	{
		Requirement.GiftType.Shell,
		Requirement.GiftType.Rose,
		Requirement.GiftType.HandLotion,
		Requirement.GiftType.Donut,
		Requirement.GiftType.Chocolates,
		Requirement.GiftType.Book,
		Requirement.GiftType.Drink,
		Requirement.GiftType.Flowers,
		Requirement.GiftType.Cake,
		Requirement.GiftType.PlushyToy,
		Requirement.GiftType.TeaSet,
		Requirement.GiftType.Shoes,
		Requirement.GiftType.CutePuppy,
		Requirement.GiftType.Necklace,
		Requirement.GiftType.DesignerBag,
		Requirement.GiftType.NewCar,
		Requirement.GiftType.MagicCandles,
		Requirement.GiftType.EnchantedScarf,
		Requirement.GiftType.BewitchedJam,
		Requirement.GiftType.MysticSlippers
	};

	// Token: 0x04000167 RID: 359
	private static Requirement.OutfitType[] defaultOutfits = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas
	};

	// Token: 0x04000168 RID: 360
	private static Requirement.OutfitType[] defaultOutfitsWithUnique = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique
	};

	// Token: 0x04000169 RID: 361
	private static Requirement.OutfitType[] defaultOutfitsWithUniqueAnimated = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.Animated
	};

	// Token: 0x0400016A RID: 362
	private static Requirement.OutfitType[] defaultOutfitsWithAnimated = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Animated
	};

	// Token: 0x0400016B RID: 363
	private static Requirement.OutfitType[] defaultOutfitsWithDeluxe = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.DeluxeWedding
	};

	// Token: 0x0400016C RID: 364
	private static Requirement.OutfitType[] defaultOutfitsWithDeluxeUnique = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.DeluxeWedding
	};

	// Token: 0x0400016D RID: 365
	private static Requirement.OutfitType[] defaultOutfitsNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas
	};

	// Token: 0x0400016E RID: 366
	private static Requirement.OutfitType[] defaultOutfitsWithUniqueNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique
	};

	// Token: 0x0400016F RID: 367
	private static Requirement.OutfitType[] defaultOutfitsWithUniqueAnimatedNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.Animated
	};

	// Token: 0x04000170 RID: 368
	private static Requirement.OutfitType[] defaultOutfitsWithAnimatedNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Animated
	};

	// Token: 0x04000171 RID: 369
	private static Requirement.OutfitType[] defaultOutfitsWithDeluxeNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.DeluxeWedding
	};

	// Token: 0x04000172 RID: 370
	private static Requirement.OutfitType[] defaultOutfitsWithDeluxeUniqueNsfw = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.DeluxeWedding
	};

	// Token: 0x04000173 RID: 371
	private static Requirement.OutfitType[] defaultOutfitsMonster = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Monster
	};

	// Token: 0x04000174 RID: 372
	private static Requirement.OutfitType[] extendedOutfitsMonster = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.Monster
	};

	// Token: 0x04000175 RID: 373
	private static Requirement.OutfitType[] defaultOutfitsNsfwMonster = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Monster
	};

	// Token: 0x04000176 RID: 374
	private static Requirement.OutfitType[] extendedOutfitsNsfwMonster = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.SchoolUniform,
		Requirement.OutfitType.BathingSuit,
		Requirement.OutfitType.DiamondRing,
		Requirement.OutfitType.Lingerie,
		Requirement.OutfitType.Nude,
		Requirement.OutfitType.Christmas,
		Requirement.OutfitType.Unique,
		Requirement.OutfitType.Monster
	};

	// Token: 0x04000177 RID: 375
	private static Requirement.OutfitType[] uniqueOutfitOnly = new Requirement.OutfitType[]
	{
		Requirement.OutfitType.Unique
	};

	// Token: 0x04000178 RID: 376
	private static Requirement.OutfitType[] noOutfits = new Requirement.OutfitType[0];

	// Token: 0x02000033 RID: 51
	public enum GirlName
	{
		// Token: 0x0400017A RID: 378
		Unknown = -1,
		// Token: 0x0400017B RID: 379
		Cassie,
		// Token: 0x0400017C RID: 380
		Mio,
		// Token: 0x0400017D RID: 381
		Quill,
		// Token: 0x0400017E RID: 382
		Elle,
		// Token: 0x0400017F RID: 383
		Nutaku,
		// Token: 0x04000180 RID: 384
		Iro,
		// Token: 0x04000181 RID: 385
		Bonnibel,
		// Token: 0x04000182 RID: 386
		Ayano,
		// Token: 0x04000183 RID: 387
		Fumi,
		// Token: 0x04000184 RID: 388
		Bearverly,
		// Token: 0x04000185 RID: 389
		Nina,
		// Token: 0x04000186 RID: 390
		Alpha,
		// Token: 0x04000187 RID: 391
		Pamu,
		// Token: 0x04000188 RID: 392
		Luna,
		// Token: 0x04000189 RID: 393
		Eva,
		// Token: 0x0400018A RID: 394
		Karma,
		// Token: 0x0400018B RID: 395
		Sutra,
		// Token: 0x0400018C RID: 396
		DarkOne,
		// Token: 0x0400018D RID: 397
		QPiddy,
		// Token: 0x0400018E RID: 398
		Darya,
		// Token: 0x0400018F RID: 399
		Jelle,
		// Token: 0x04000190 RID: 400
		Quillzone,
		// Token: 0x04000191 RID: 401
		Bonchovy,
		// Token: 0x04000192 RID: 402
		Spectrum,
		// Token: 0x04000193 RID: 403
		Charlotte,
		// Token: 0x04000194 RID: 404
		Odango,
		// Token: 0x04000195 RID: 405
		Shibuki,
		// Token: 0x04000196 RID: 406
		Sirina,
		// Token: 0x04000197 RID: 407
		Catara,
		// Token: 0x04000198 RID: 408
		Vellatrix,
		// Token: 0x04000199 RID: 409
		Peanut,
		// Token: 0x0400019A RID: 410
		Roxxy,
		// Token: 0x0400019B RID: 411
		Tessa,
		// Token: 0x0400019C RID: 412
		Claudia,
		// Token: 0x0400019D RID: 413
		Rosa,
		// Token: 0x0400019E RID: 414
		Juliet,
		// Token: 0x0400019F RID: 415
		Wendy,
		// Token: 0x040001A0 RID: 416
		Ruri,
		// Token: 0x040001A1 RID: 417
		Generica,
		// Token: 0x040001A2 RID: 418
		Suzu,
		// Token: 0x040001A3 RID: 419
		Lustat,
		// Token: 0x040001A4 RID: 420
		Sawyer,
		// Token: 0x040001A5 RID: 421
		Explora,
		// Token: 0x040001A6 RID: 422
		Esper,
		// Token: 0x040001A7 RID: 423
		Renee,
		// Token: 0x040001A8 RID: 424
		Mallory,
		// Token: 0x040001A9 RID: 425
		Lake,
		// Token: 0x040001AA RID: 426
		ReneePF = 1000,
		// Token: 0x040001AB RID: 427
		NovaPF,
		// Token: 0x040001AC RID: 428
		QPernikiss = 18
	}
}
