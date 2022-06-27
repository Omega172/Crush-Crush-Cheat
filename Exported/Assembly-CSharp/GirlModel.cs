using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class GirlModel
{
	// Token: 0x06000927 RID: 2343 RVA: 0x0004D5B0 File Offset: 0x0004B7B0
	public GirlModel(AssetBundle gamedata, List<string[]> states)
	{
		string[] array = states[0];
		if (array.Length != 17)
		{
			Debug.LogError(array[0] + " did not load correctly.");
		}
		else
		{
			this.Name = array[0];
			this.Id = short.Parse(array[1]);
			for (int i = 0; i < states.Count; i++)
			{
				string text = states[i][2];
				switch (text)
				{
				case "UnlockRequirement":
					this.UnlockType = states[i][3];
					break;
				case "UnlockAmount":
					this.UnlockRequirement = states[i][3];
					break;
				case "Color":
					this.TextColor = this.ColorFromText(states[i][3]);
					break;
				case "IntroColor":
					this.IntroColor = this.ColorFromText(states[i][3]);
					break;
				}
			}
			this.UnlockType = array[2];
			this.UnlockRequirement = array[3];
		}
		this.Levels = new GirlModel.GirlLevel[states.Count];
		for (int j = 0; j < this.Levels.Length; j++)
		{
			this.Levels[j] = new GirlModel.GirlLevel(this.Name, states[j]);
		}
		this.Text = new List<GirlModel.GirlText>();
		this.IntroDataList = new List<GirlModel.IntroData>();
		this.SexyDataList = new List<GirlModel.IntroData>();
		this.OtherText = new Dictionary<string, List<GirlModel.GirlText>>();
		this._gamedata = gamedata;
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x0004D798 File Offset: 0x0004B998
	// (set) Token: 0x0600092A RID: 2346 RVA: 0x0004D7A0 File Offset: 0x0004B9A0
	public short Id { get; private set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x0004D7AC File Offset: 0x0004B9AC
	// (set) Token: 0x0600092C RID: 2348 RVA: 0x0004D7B4 File Offset: 0x0004B9B4
	public string Name { get; private set; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x0004D7C0 File Offset: 0x0004B9C0
	// (set) Token: 0x0600092E RID: 2350 RVA: 0x0004D7C8 File Offset: 0x0004B9C8
	public GirlModel.GirlLevel[] Levels { get; private set; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x0004D7D4 File Offset: 0x0004B9D4
	// (set) Token: 0x06000930 RID: 2352 RVA: 0x0004D7DC File Offset: 0x0004B9DC
	public string UnlockType { get; private set; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x0004D7E8 File Offset: 0x0004B9E8
	// (set) Token: 0x06000932 RID: 2354 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
	public string UnlockRequirement { get; private set; }

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000933 RID: 2355 RVA: 0x0004D7FC File Offset: 0x0004B9FC
	// (set) Token: 0x06000934 RID: 2356 RVA: 0x0004D804 File Offset: 0x0004BA04
	public Color TextColor { get; private set; }

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x0004D810 File Offset: 0x0004BA10
	// (set) Token: 0x06000936 RID: 2358 RVA: 0x0004D818 File Offset: 0x0004BA18
	public Color IntroColor { get; private set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x0004D824 File Offset: 0x0004BA24
	// (set) Token: 0x06000938 RID: 2360 RVA: 0x0004D82C File Offset: 0x0004BA2C
	public List<GirlModel.GirlText> Text { get; private set; }

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x0004D838 File Offset: 0x0004BA38
	// (set) Token: 0x0600093A RID: 2362 RVA: 0x0004D840 File Offset: 0x0004BA40
	public List<GirlModel.IntroData> IntroDataList { get; private set; }

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x0004D84C File Offset: 0x0004BA4C
	// (set) Token: 0x0600093C RID: 2364 RVA: 0x0004D854 File Offset: 0x0004BA54
	public List<GirlModel.IntroData> SexyDataList { get; private set; }

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x0004D860 File Offset: 0x0004BA60
	// (set) Token: 0x0600093E RID: 2366 RVA: 0x0004D868 File Offset: 0x0004BA68
	public Dictionary<string, List<GirlModel.GirlText>> OtherText { get; private set; }

	// Token: 0x0600093F RID: 2367 RVA: 0x0004D874 File Offset: 0x0004BA74
	private GirlModel.TextType TextTypeFromString(string type, string target)
	{
		switch (type)
		{
		case "Greet":
			return GirlModel.TextType.Greet;
		case "GreetNude":
			return GirlModel.TextType.GreetNude;
		case "LevelText":
			return GirlModel.TextType.LevelText1;
		case "LevelText2":
			return GirlModel.TextType.LevelText2;
		case "LevelText2NSFW":
			return GirlModel.TextType.LevelText2NSFW;
		case "LevelText3":
			return GirlModel.TextType.LevelText3;
		case "Poke":
			return GirlModel.TextType.Poke;
		case "Outfit":
			switch (target)
			{
			case "BathingSuit":
				return GirlModel.TextType.BathingSuit;
			case "SchoolUniform":
				return GirlModel.TextType.Seifuku;
			case "DiamondRing":
				return GirlModel.TextType.Wedding;
			case "Lingerie":
				return GirlModel.TextType.Lingerie;
			case "Nude":
				return GirlModel.TextType.Nude;
			case "Christmas":
				return GirlModel.TextType.Xmas;
			case "UniqueOutfit":
			case "Unique":
				return GirlModel.TextType.Unique;
			case "Monster":
				return GirlModel.TextType.Monster;
			}
			return GirlModel.TextType.AnyOutfit;
		case "Gift":
			return GirlModel.TextType.Gift;
		case "Date":
			switch (target)
			{
			case "MovieTheater":
				return GirlModel.TextType.MovieTheater;
			case "Beach":
				return GirlModel.TextType.Beach;
			case "Sightseeing":
				return GirlModel.TextType.Sightseeing;
			case "MoonlightStroll":
				return GirlModel.TextType.MoonlightStroll;
			}
			return GirlModel.TextType.MoonlightStroll;
		case "GreetAyano":
			return GirlModel.TextType.Ayano;
		case "PokeNude":
			return GirlModel.TextType.PokeNude;
		case "Talk":
			return (!target.ToLowerInvariant().Contains("idle")) ? GirlModel.TextType.Talk : GirlModel.TextType.Unknown;
		case "TalkNude":
			return GirlModel.TextType.TalkNude;
		case "TalkNSFW":
			return GirlModel.TextType.TalkNSFW;
		case "DateNSFW":
			return GirlModel.TextType.DateNSFW;
		case "GiftNSFW":
			return GirlModel.TextType.GiftNSFW;
		case "PokeNSFW":
			return GirlModel.TextType.PokeNSFW;
		case "SexyTimes":
			return GirlModel.TextType.FinaleNSFW;
		case "Intro":
			return GirlModel.TextType.Intro;
		case "IntroCrush":
			return GirlModel.TextType.IntroCrush;
		case "Outro":
			return GirlModel.TextType.Outro;
		case "SacrificeYourself":
			return GirlModel.TextType.SacrificeYourself;
		case "SacrificeYourselfCrush":
			return GirlModel.TextType.SacrificeYourselfCrush;
		case "SacrificeQPiddy":
			return GirlModel.TextType.SacrificeQPiddy;
		case "SacrificeQPiddyCrush":
			return GirlModel.TextType.SacrificeQPiddyCrush;
		case "Choice1":
		case "Choice2":
			return GirlModel.TextType.Choice;
		}
		return GirlModel.TextType.Unknown;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0004DC60 File Offset: 0x0004BE60
	public GirlModel.GirlText GetText(int id)
	{
		foreach (GirlModel.GirlText girlText in this.Text)
		{
			if (girlText.ID == id)
			{
				return girlText;
			}
		}
		return null;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0004DCD8 File Offset: 0x0004BED8
	public GirlModel.GirlText GetText(GirlModel.TextType type, int level)
	{
		if (type == GirlModel.TextType.LevelText2 && GameState.NSFW)
		{
			GirlModel.GirlText text = this.GetText(GirlModel.TextType.LevelText2NSFW, level);
			if (text != null)
			{
				return text;
			}
		}
		if (type == GirlModel.TextType.MovieTheater && GameState.NSFW)
		{
			GirlModel.GirlText text2 = this.GetText(GirlModel.TextType.DateNSFW, level);
			if (text2 != null)
			{
				return text2;
			}
		}
		List<GirlModel.GirlText> list = new List<GirlModel.GirlText>();
		foreach (GirlModel.GirlText girlText in this.Text)
		{
			if (girlText.IsMatch(type) && (girlText.MinLevel == -1 || (girlText.MinLevel <= level && girlText.MaxLevel >= level)))
			{
				list.Add(girlText);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[GirlModel.generator.Next(list.Count)];
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0004DDE0 File Offset: 0x0004BFE0
	private Color ColorFromText(string text)
	{
		string[] array = text.Split(new char[]
		{
			','
		});
		if (array.Length != 3)
		{
			Debug.Log("Failed to read color " + text);
			return Color.white;
		}
		return new Color(float.Parse(array[0]) / 255f, float.Parse(array[1]) / 255f, float.Parse(array[2]) / 255f);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0004DE50 File Offset: 0x0004C050
	public void CheckIfLoaded()
	{
		if (this._lazyLoaded)
		{
			return;
		}
		TextAsset textAsset = this._gamedata.LoadAsset<TextAsset>((!(this.Name == "Peanut") && !(this.Name == "Ruri") && !(this.Name == "Wendy") && !(this.Name == "Generica") && !(this.Name == "Sawyer") && !(this.Name == "Renee") && !(this.Name == "Lake")) ? this.Name : (this.Name + "Full"));
		if (textAsset == null)
		{
			Debug.LogError("There was no text sheet for " + this.Name);
		}
		else
		{
			List<string[]> list = Universe.FromCsv(textAsset.text);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i][2].Contains(","))
				{
					list[i][2] = list[i][2].Substring(0, list[i][2].IndexOf(","));
				}
				if (int.TryParse(list[i][2], out num))
				{
					if (num < 0)
					{
						Debug.LogError(string.Format("Text ID {0} was unexpected (expect ID > 0).", num.ToString()));
					}
					else
					{
						string text = list[i][0];
						GirlModel.TextType textType = this.TextTypeFromString(list[i][1], text);
						if (textType == GirlModel.TextType.Unknown)
						{
							if (!this.OtherText.ContainsKey(text))
							{
								this.OtherText[text] = new List<GirlModel.GirlText>();
							}
							this.OtherText[text].Add(new GirlModel.GirlText
							{
								ID = num,
								English = list[i][3]
							});
						}
						GirlModel.GirlText girlText = new GirlModel.GirlText
						{
							ID = num,
							English = list[i][3],
							Type = textType,
							MinLevel = -1,
							MaxLevel = -1
						};
						if (list[i].Length == 7 && !string.IsNullOrEmpty(list[i][4]))
						{
							girlText.AudioID = this.Name.ToLowerInvariant() + "_" + girlText.English;
							girlText.English = list[i][4];
						}
						if (!this.ParseIntro(girlText, list, i))
						{
							this.Text.Add(girlText);
							if (text.Contains("to"))
							{
								girlText.MinLevel = int.Parse(text.Substring(0, 1));
								girlText.MaxLevel = int.Parse(text.Substring(5, 1));
							}
							else if (text == "Endless")
							{
								girlText.MinLevel = 10;
								girlText.MaxLevel = int.MaxValue;
							}
							else if ((text.Length == 1 && char.IsNumber(text[0])) || text[0] == '-')
							{
								girlText.MinLevel = (girlText.MaxLevel = int.Parse(text));
							}
						}
					}
				}
			}
		}
		this._lazyLoaded = true;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0004E1D4 File Offset: 0x0004C3D4
	protected virtual bool IsIntroType(GirlModel.TextType textType)
	{
		return textType == GirlModel.TextType.FinaleNSFW || textType == GirlModel.TextType.Intro || textType == GirlModel.TextType.IntroCrush || textType == GirlModel.TextType.Outro || textType == GirlModel.TextType.SacrificeQPiddy || textType == GirlModel.TextType.SacrificeQPiddyCrush || textType == GirlModel.TextType.SacrificeYourself || textType == GirlModel.TextType.SacrificeYourselfCrush || textType == GirlModel.TextType.Choice;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0004E22C File Offset: 0x0004C42C
	protected virtual bool ParseIntro(GirlModel.GirlText girlText, List<string[]> textData, int i)
	{
		if (this.IsIntroType(girlText.Type))
		{
			string text = (textData[i].Length < 6 || string.IsNullOrEmpty(textData[i][5])) ? string.Empty : textData[i][5];
			text = ((!string.IsNullOrEmpty(text) || textData[i].Length < 5) ? text : textData[i][4]);
			GirlModel.IntroData introData = new GirlModel.IntroData
			{
				isCrush = (girlText.Type == GirlModel.TextType.IntroCrush || girlText.Type == GirlModel.TextType.SacrificeQPiddyCrush || girlText.Type == GirlModel.TextType.SacrificeYourselfCrush),
				ID = girlText.ID,
				English = girlText.English,
				Data = text
			};
			this.AddIntroToList(introData, girlText.Type);
			return true;
		}
		return false;
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0004E310 File Offset: 0x0004C510
	protected virtual void AddIntroToList(GirlModel.IntroData introData, GirlModel.TextType textType)
	{
		if (textType == GirlModel.TextType.FinaleNSFW)
		{
			this.SexyDataList.Add(introData);
		}
		else
		{
			this.IntroDataList.Add(introData);
		}
	}

	// Token: 0x04000913 RID: 2323
	private static System.Random generator = new System.Random();

	// Token: 0x04000914 RID: 2324
	private AssetBundle _gamedata;

	// Token: 0x04000915 RID: 2325
	private bool _lazyLoaded;

	// Token: 0x0200014B RID: 331
	public class GirlText
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0004E34C File Offset: 0x0004C54C
		public bool IsMatch(GirlModel.TextType type)
		{
			return this.Type == type || (type == GirlModel.TextType.Gift && GameState.NSFW && this.Type == GirlModel.TextType.GiftNSFW) || (type == GirlModel.TextType.Poke && GameState.NSFW && this.Type == GirlModel.TextType.PokeNSFW) || (type == GirlModel.TextType.Talk && GameState.NSFW && this.Type == GirlModel.TextType.TalkNSFW);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0004E3CC File Offset: 0x0004C5CC
		public override string ToString()
		{
			return string.Format("{0} {1}", this.ID, this.English);
		}

		// Token: 0x04000925 RID: 2341
		public int MinLevel;

		// Token: 0x04000926 RID: 2342
		public int MaxLevel;

		// Token: 0x04000927 RID: 2343
		public GirlModel.TextType Type;

		// Token: 0x04000928 RID: 2344
		public int ID;

		// Token: 0x04000929 RID: 2345
		public string English;

		// Token: 0x0400092A RID: 2346
		public string AudioID;
	}

	// Token: 0x0200014C RID: 332
	public class IntroData
	{
		// Token: 0x0400092B RID: 2347
		public bool isCrush;

		// Token: 0x0400092C RID: 2348
		public int ID;

		// Token: 0x0400092D RID: 2349
		public string English;

		// Token: 0x0400092E RID: 2350
		public string Data;

		// Token: 0x0400092F RID: 2351
		public bool Playable = true;

		// Token: 0x04000930 RID: 2352
		public bool DisplayOptions;
	}

	// Token: 0x0200014D RID: 333
	public struct GirlRequirement
	{
		// Token: 0x04000931 RID: 2353
		public Requirement.RequirementType RequirementType;

		// Token: 0x04000932 RID: 2354
		public short Id;

		// Token: 0x04000933 RID: 2355
		public long Quantity;
	}

	// Token: 0x0200014E RID: 334
	public class GirlLevel
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x0004E3FC File Offset: 0x0004C5FC
		public GirlLevel(string name, string[] csv)
		{
			this.Language = (Translations.Language)Translations.CurrentLanguage.Value;
			if (csv.Length != 17 || csv[5] != "Heart")
			{
				Debug.LogError(name + " level " + csv[4] + " did not load correctly.");
				this.Level = 0;
				this.Requirements = new GirlModel.GirlRequirement[0];
			}
			else
			{
				this.Level = short.Parse(csv[4]);
				List<GirlModel.GirlRequirement> list = new List<GirlModel.GirlRequirement>();
				try
				{
					if (!string.IsNullOrEmpty(csv[5]) && csv[5].Trim() != "None")
					{
						list.Add(this.BuildRequirement(csv[5], long.Parse(csv[6].Replace(",", string.Empty))));
					}
					if (!string.IsNullOrEmpty(csv[7]) && csv[7].Trim() != "None")
					{
						list.Add(this.BuildRequirement(csv[7], long.Parse(csv[8])));
					}
					if (!string.IsNullOrEmpty(csv[9]) && csv[9].Trim() != "None")
					{
						list.Add(this.BuildRequirement(csv[9], long.Parse(csv[10])));
					}
					if (!string.IsNullOrEmpty(csv[11]) && csv[11].Trim() != "None")
					{
						list.Add(this.BuildRequirement(csv[11], long.Parse(csv[12])));
					}
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
				this.Requirements = list.ToArray();
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0004E5C0 File Offset: 0x0004C7C0
		private GirlModel.GirlRequirement BuildRequirement(string requirement, long quantity)
		{
			switch (requirement)
			{
			case "Money":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Money,
					Id = Universe.StringToResource["Money"].Id,
					Quantity = quantity
				};
			case "Diamond":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Diamond,
					Id = Universe.StringToResource["Diamond"].Id,
					Quantity = quantity
				};
			case "Heart":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Heart,
					Id = 0,
					Quantity = quantity
				};
			case "MultiplierReset":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Prestige,
					Id = Universe.StringToResource["MultiplierReset"].Id,
					Quantity = quantity
				};
			case "MoneyConsume":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.MoneyConsume,
					Id = Universe.StringToResource["MoneyConsume"].Id,
					Quantity = quantity
				};
			case "MultiplierConsume":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.PrestigeConsume,
					Id = Universe.StringToResource["MultiplierConsume"].Id,
					Quantity = quantity
				};
			case "TimeBlockConsume":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Time,
					Id = Universe.StringToResource["TimeBlockConsume"].Id,
					Quantity = quantity
				};
			case "TotalGifts":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.TotalGifts,
					Id = Universe.StringToResource["TotalGifts"].Id,
					Quantity = quantity
				};
			case "TotalDates":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.TotalDates,
					Id = Universe.StringToResource["TotalDates"].Id,
					Quantity = quantity
				};
			case "JobGild":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.JobGild,
					Quantity = quantity
				};
			case "HobbyGild":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.HobbyGild,
					Quantity = quantity
				};
			case "AllJobs":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.AllJobs
				};
			case "AllHobbies":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.AllHobbies,
					Quantity = quantity
				};
			case "MemoryAlbum":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Album
				};
			case "Achievements":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.Achievement,
					Quantity = quantity
				};
			case "GirlsAtLover":
				return new GirlModel.GirlRequirement
				{
					RequirementType = Requirement.RequirementType.GirlsAtLover,
					Quantity = quantity
				};
			}
			foreach (KeyValuePair<short, HobbyModel> keyValuePair in Universe.Hobbies)
			{
				if (requirement == keyValuePair.Value.Resource.Name)
				{
					return new GirlModel.GirlRequirement
					{
						RequirementType = Requirement.RequirementType.Skill,
						Id = keyValuePair.Key,
						Quantity = quantity
					};
				}
			}
			foreach (KeyValuePair<short, JobModel> keyValuePair2 in Universe.Jobs)
			{
				if (requirement == keyValuePair2.Value.Name)
				{
					return new GirlModel.GirlRequirement
					{
						RequirementType = Requirement.RequirementType.Job,
						Id = keyValuePair2.Key,
						Quantity = quantity
					};
				}
			}
			foreach (KeyValuePair<short, DateModel> keyValuePair3 in Universe.Dates)
			{
				if (requirement == keyValuePair3.Value.Name)
				{
					return new GirlModel.GirlRequirement
					{
						RequirementType = Requirement.RequirementType.Date,
						Id = keyValuePair3.Key,
						Quantity = quantity
					};
				}
			}
			foreach (KeyValuePair<short, GiftModel> keyValuePair4 in Universe.Gifts)
			{
				if (requirement == keyValuePair4.Value.Name)
				{
					return new GirlModel.GirlRequirement
					{
						RequirementType = Requirement.RequirementType.Gift,
						Id = keyValuePair4.Key,
						Quantity = quantity
					};
				}
			}
			return new GirlModel.GirlRequirement
			{
				RequirementType = Requirement.RequirementType.Unknown,
				Id = 0,
				Quantity = 0L
			};
		}

		// Token: 0x04000934 RID: 2356
		public short Level;

		// Token: 0x04000935 RID: 2357
		public GirlModel.GirlRequirement[] Requirements;

		// Token: 0x04000936 RID: 2358
		public Translations.Language Language;
	}

	// Token: 0x0200014F RID: 335
	public enum TextType
	{
		// Token: 0x04000939 RID: 2361
		Greet,
		// Token: 0x0400093A RID: 2362
		LevelText1,
		// Token: 0x0400093B RID: 2363
		LevelText2,
		// Token: 0x0400093C RID: 2364
		LevelText2NSFW,
		// Token: 0x0400093D RID: 2365
		LevelText3,
		// Token: 0x0400093E RID: 2366
		Poke,
		// Token: 0x0400093F RID: 2367
		BathingSuit,
		// Token: 0x04000940 RID: 2368
		Seifuku,
		// Token: 0x04000941 RID: 2369
		Wedding,
		// Token: 0x04000942 RID: 2370
		Lingerie,
		// Token: 0x04000943 RID: 2371
		Nude,
		// Token: 0x04000944 RID: 2372
		Xmas,
		// Token: 0x04000945 RID: 2373
		Unique,
		// Token: 0x04000946 RID: 2374
		AnyOutfit,
		// Token: 0x04000947 RID: 2375
		Monster,
		// Token: 0x04000948 RID: 2376
		Gift,
		// Token: 0x04000949 RID: 2377
		MovieTheater,
		// Token: 0x0400094A RID: 2378
		Beach,
		// Token: 0x0400094B RID: 2379
		Sightseeing,
		// Token: 0x0400094C RID: 2380
		MoonlightStroll,
		// Token: 0x0400094D RID: 2381
		Ayano,
		// Token: 0x0400094E RID: 2382
		GreetNude,
		// Token: 0x0400094F RID: 2383
		PokeNude,
		// Token: 0x04000950 RID: 2384
		Talk,
		// Token: 0x04000951 RID: 2385
		TalkNude,
		// Token: 0x04000952 RID: 2386
		TalkNSFW,
		// Token: 0x04000953 RID: 2387
		PokeNSFW,
		// Token: 0x04000954 RID: 2388
		GiftNSFW,
		// Token: 0x04000955 RID: 2389
		DateNSFW,
		// Token: 0x04000956 RID: 2390
		Intro,
		// Token: 0x04000957 RID: 2391
		IntroCrush,
		// Token: 0x04000958 RID: 2392
		Outro,
		// Token: 0x04000959 RID: 2393
		SacrificeYourself,
		// Token: 0x0400095A RID: 2394
		SacrificeYourselfCrush,
		// Token: 0x0400095B RID: 2395
		SacrificeQPiddy,
		// Token: 0x0400095C RID: 2396
		SacrificeQPiddyCrush,
		// Token: 0x0400095D RID: 2397
		FinaleNSFW,
		// Token: 0x0400095E RID: 2398
		Unknown,
		// Token: 0x0400095F RID: 2399
		Choice
	}
}
