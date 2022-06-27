using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AssetBundles;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000159 RID: 345
public class Universe : MonoBehaviour
{
	// Token: 0x06000999 RID: 2457 RVA: 0x000503FC File Offset: 0x0004E5FC
	public static IEnumerator InitFirst()
	{
		if (!Universe.loaded)
		{
			AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync("universe/gamedata", false);
			yield return bundleRequest;
			if (bundleRequest.AssetBundle == null)
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Fatal error, could not load universe/gamedata");
				yield break;
			}
			AssetBundle gamedata = bundleRequest.AssetBundle;
			yield return Universe.LoadResources(gamedata);
			yield return Universe.LoadGifts(gamedata);
			yield return Universe.LoadOutfits(gamedata);
			yield return Universe.LoadHobbies(gamedata);
			yield return Universe.LoadDates(gamedata);
			yield return Universe.LoadJobs(gamedata);
			yield return Universe.LoadStats(gamedata);
			yield return Universe.LoadGirls(gamedata);
			yield return Universe.LoadAchievements(gamedata);
			yield return Universe.LoadCellphone(gamedata);
			yield return Universe.LoadCredits(gamedata);
			yield return Universe.LoadTasks(gamedata);
			yield return Universe.LoadEvents(gamedata);
			yield return Universe.LoadGiftIcons();
			yield return Universe.LoadOutfitIcons();
			Universe.loaded = true;
		}
		yield break;
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00050410 File Offset: 0x0004E610
	public IEnumerator Init(Action onSuccess)
	{
		yield return Universe.InitFirst();
		foreach (KeyValuePair<short, PhoneModel> fling in Universe.CellphoneGirls)
		{
			fling.Value.ConvertSave();
		}
		this.PopulateUniverseData(onSuccess);
		yield break;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0005043C File Offset: 0x0004E63C
	private static IEnumerator LoadResources(AssetBundle gamedata)
	{
		AssetBundleRequest resourcesAsset = gamedata.LoadAssetAsync<TextAsset>("Resources");
		yield return resourcesAsset;
		List<string[]> resourceData = Universe.FromCsv((resourcesAsset.asset as TextAsset).text);
		for (int i = 1; i < resourceData.Count; i++)
		{
			ResourceModel model = new ResourceModel(resourceData[i]);
			if (model.Id != 0)
			{
				Universe.Resources.Add(model.Id, model);
				Universe.StringToResource.Add(model.Name, model);
			}
		}
		Debug.Log("Loaded " + Universe.Resources.Count.ToString() + " resources from .csv");
		yield break;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00050460 File Offset: 0x0004E660
	private static IEnumerator LoadTasks(AssetBundle ltedata)
	{
		AssetBundleRequest tasksAsset = ltedata.LoadAssetAsync<TextAsset>("LTE_Tasks");
		yield return tasksAsset;
		List<string[]> tasksData = Universe.FromCsv((tasksAsset.asset as TextAsset).text);
		for (int i = 1; i < tasksData.Count; i++)
		{
			Universe.Tasks.Add(short.Parse(tasksData[i][1]), new TaskData(tasksData[i]));
		}
		Debug.Log("Loaded " + Universe.Tasks.Count.ToString() + " tasks from .csv");
		yield break;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00050484 File Offset: 0x0004E684
	private static IEnumerator LoadEvents(AssetBundle ltedata)
	{
		AssetBundleRequest eventsAsset = ltedata.LoadAssetAsync<TextAsset>("LTE_Events");
		yield return eventsAsset;
		List<string[]> eventsData = Universe.FromCsv((eventsAsset.asset as TextAsset).text);
		for (int i = 1; i < eventsData.Count; i++)
		{
			if (!string.IsNullOrEmpty(eventsData[i][0]))
			{
				List<TaskManager.EventRewardType> rewardType = new List<TaskManager.EventRewardType>();
				List<long> rewardAmount = new List<long>();
				for (int j = i; j < eventsData.Count; j++)
				{
					if (!string.IsNullOrEmpty(eventsData[j][0]) && j != i)
					{
						break;
					}
					if (!string.IsNullOrEmpty(eventsData[j][10]))
					{
						rewardType.Add((TaskManager.EventRewardType)((byte)Enum.Parse(typeof(TaskManager.EventRewardType), eventsData[j][10])));
					}
					if (!string.IsNullOrEmpty(eventsData[j][11]))
					{
						rewardAmount.Add(long.Parse(eventsData[j][11]));
					}
				}
				Universe.Events.Add(short.Parse(eventsData[i][1]), new EventData(eventsData[i], rewardType, rewardAmount));
				Universe.Events[short.Parse(eventsData[i][1])].Validate();
			}
		}
		Debug.Log("Loaded " + Universe.Events.Count.ToString() + " events from .csv");
		yield break;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000504A8 File Offset: 0x0004E6A8
	private static IEnumerator LoadGiftIcons()
	{
		AssetBundleAsync giftsIcons = GameState.AssetManager.GetBundleAsync("universe/gift_icons", false);
		yield return giftsIcons;
		if (giftsIcons.AssetBundle == null)
		{
			yield break;
		}
		foreach (KeyValuePair<short, GiftModel> model in Universe.Gifts)
		{
			Sprite giftIcon = giftsIcons.AssetBundle.LoadAsset<Sprite>(model.Value.SpriteKey);
			model.Value.Sprite = giftIcon;
			if (!string.IsNullOrEmpty(model.Value.ExploraSpriteKey))
			{
				giftIcon = giftsIcons.AssetBundle.LoadAsset<Sprite>(model.Value.ExploraSpriteKey);
				model.Value.ExploraSprite = giftIcon;
			}
			if (!string.IsNullOrEmpty(model.Value.MallorySpriteKey))
			{
				giftIcon = giftsIcons.AssetBundle.LoadAsset<Sprite>(model.Value.MallorySpriteKey);
				model.Value.MallorySprite = giftIcon;
			}
		}
		yield break;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x000504BC File Offset: 0x0004E6BC
	private static IEnumerator LoadGifts(AssetBundle gamedata)
	{
		AssetBundleRequest giftsAsset = gamedata.LoadAssetAsync<TextAsset>("Gifts");
		yield return giftsAsset;
		List<string[]> giftsData = Universe.FromCsv((giftsAsset.asset as TextAsset).text);
		for (int i = 1; i < giftsData.Count; i++)
		{
			GiftModel model = new GiftModel(giftsData[i]);
			if (model.Id != 0)
			{
				Universe.Gifts.Add(model.Id, model);
			}
		}
		yield break;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x000504E0 File Offset: 0x0004E6E0
	private static IEnumerator LoadOutfitIcons()
	{
		AssetBundleAsync outfitIcons = GameState.AssetManager.GetBundleAsync("universe/gift_icons", false);
		yield return outfitIcons;
		if (outfitIcons.AssetBundle == null)
		{
			yield break;
		}
		foreach (KeyValuePair<short, OutfitModel> model in Universe.Outfits)
		{
			Sprite outfitIcon = outfitIcons.AssetBundle.LoadAsset<Sprite>(model.Value.SpriteKey);
			model.Value.Sprite = outfitIcon;
		}
		Debug.Log("Loaded " + Universe.Outfits.Count.ToString() + " outfits from .csv");
		yield break;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x000504F4 File Offset: 0x0004E6F4
	private static IEnumerator LoadOutfits(AssetBundle gamedata)
	{
		AssetBundleRequest outfitAsset = gamedata.LoadAssetAsync<TextAsset>("Outfits");
		yield return outfitAsset;
		List<string[]> outfitData = Universe.FromCsv((outfitAsset.asset as TextAsset).text);
		for (int i = 1; i < outfitData.Count; i++)
		{
			OutfitModel model = new OutfitModel(outfitData[i]);
			if (model.Id != 0)
			{
				Universe.Outfits.Add(model.Id, model);
			}
		}
		yield break;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00050518 File Offset: 0x0004E718
	private static IEnumerator LoadHobbies(AssetBundle gamedata)
	{
		AssetBundleRequest hobbiesAsset = gamedata.LoadAssetAsync<TextAsset>("Hobbies");
		yield return hobbiesAsset;
		List<string[]> hobbiesData = Universe.FromCsv((hobbiesAsset.asset as TextAsset).text);
		for (int i = 1; i < hobbiesData.Count; i++)
		{
			HobbyModel model = new HobbyModel(hobbiesData[i]);
			if (model.Id != 0)
			{
				Universe.Hobbies.Add(model.Id, model);
			}
		}
		Debug.Log("Loaded " + Universe.Hobbies.Count.ToString() + " hobbies from .csv");
		yield break;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0005053C File Offset: 0x0004E73C
	private static IEnumerator LoadDates(AssetBundle gamedata)
	{
		AssetBundleRequest datesAsset = gamedata.LoadAssetAsync<TextAsset>("Dates");
		yield return datesAsset;
		List<string[]> datesData = Universe.FromCsv((datesAsset.asset as TextAsset).text);
		for (int i = 1; i < datesData.Count; i++)
		{
			DateModel model = new DateModel(datesData[i]);
			if (model.Id != 0)
			{
				Universe.Dates.Add(model.Id, model);
			}
		}
		Debug.Log("Loaded " + Universe.Dates.Count.ToString() + " dates from .csv");
		yield break;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00050560 File Offset: 0x0004E760
	private static IEnumerator LoadJobs(AssetBundle gamedata)
	{
		AssetBundleRequest jobsAsset = gamedata.LoadAssetAsync<TextAsset>("Jobs");
		yield return jobsAsset;
		List<string[]> jobsData = Universe.FromCsv((jobsAsset.asset as TextAsset).text);
		int line = 1;
		while (line < jobsData.Count)
		{
			int start = line;
			line++;
			while (line < jobsData.Count && string.IsNullOrEmpty(jobsData[line][1]))
			{
				line++;
			}
			List<string[]> jobStates = new List<string[]>();
			for (int i = start; i < line; i++)
			{
				jobStates.Add(jobsData[i]);
			}
			JobModel model = new JobModel(jobStates);
			if (model.Id != 0)
			{
				Universe.Jobs.Add(model.Id, model);
			}
		}
		Debug.Log("Loaded " + Universe.Jobs.Count + " jobs from .csv");
		yield break;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00050584 File Offset: 0x0004E784
	private static IEnumerator LoadStats(AssetBundle gamedata)
	{
		AssetBundleRequest statsAsset = gamedata.LoadAssetAsync<TextAsset>("Stats");
		yield return statsAsset;
		List<string[]> statsData = Universe.FromCsv((statsAsset.asset as TextAsset).text);
		for (int i = 1; i < statsData.Count; i++)
		{
			StatsModel model = new StatsModel(statsData[i]);
			if (model.Id != 0)
			{
				Universe.Stats.Add(model.Id, model);
			}
		}
		Debug.Log("Loaded " + Universe.Stats.Count.ToString() + " stats from .csv");
		yield break;
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000505A8 File Offset: 0x0004E7A8
	private static IEnumerator LoadGirls(AssetBundle gamedata)
	{
		AssetBundleRequest girlsAsset = gamedata.LoadAssetAsync<TextAsset>("Girls");
		yield return girlsAsset;
		List<string[]> girlsData = Universe.FromCsv((girlsAsset.asset as TextAsset).text);
		int line = 1;
		while (line < girlsData.Count)
		{
			int start = line;
			line++;
			while (line < girlsData.Count && string.IsNullOrEmpty(girlsData[line][1]))
			{
				line++;
			}
			List<string[]> girlContent = new List<string[]>();
			for (int i = start; i < line; i++)
			{
				girlContent.Add(girlsData[i]);
			}
			GirlModel model = (!(girlContent[0][0] != "QPernikiss")) ? new QPiddyModel(gamedata, girlContent) : new GirlModel(gamedata, girlContent);
			if (model.Id != 0)
			{
				Universe.Girls.Add(model.Id, model);
			}
			while (line < girlsData.Count && (girlsData[line][0] == "Name" || string.IsNullOrEmpty(girlsData[line][1])))
			{
				line++;
			}
		}
		Debug.Log("Loaded " + Universe.Girls.Count.ToString() + " girls from .csv");
		yield break;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000505CC File Offset: 0x0004E7CC
	private static IEnumerator LoadAchievements(AssetBundle gamedata)
	{
		AssetBundleRequest achievementsAsset = gamedata.LoadAssetAsync<TextAsset>("Achievements");
		yield return achievementsAsset;
		List<string[]> achievementData = Universe.FromCsv((achievementsAsset.asset as TextAsset).text);
		for (int i = 1; i < achievementData.Count; i++)
		{
			if (!string.IsNullOrEmpty(achievementData[i][0]))
			{
				AchievementModel model = new AchievementModel(achievementData[i]);
				if (model.Id != 0 && model.Target >= 0)
				{
					Universe.Achievements.Add(model.Id, model);
				}
			}
		}
		Debug.Log("Loaded " + Universe.Achievements.Count.ToString() + " achievements from .csv");
		yield break;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x000505F0 File Offset: 0x0004E7F0
	private static IEnumerator LoadCellphone(AssetBundle gamedata)
	{
		AssetBundleRequest cellphoneAsset = gamedata.LoadAssetAsync<TextAsset>("Cellphone");
		yield return cellphoneAsset;
		List<string[]> cellphoneData = Universe.FromCsv((cellphoneAsset.asset as TextAsset).text);
		for (int i = 1; i < cellphoneData.Count; i++)
		{
			if (!string.IsNullOrEmpty(cellphoneData[i][0]))
			{
				PhoneModel model = new PhoneModel(gamedata, cellphoneData[i]);
				if (model.Id != 0)
				{
					Universe.CellphoneGirls.Add(model.Id, model);
				}
			}
		}
		Debug.Log("Loaded " + Universe.CellphoneGirls.Count.ToString() + " cellphone characters from .csv");
		yield break;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00050614 File Offset: 0x0004E814
	private static IEnumerator LoadCredits(AssetBundle gamedata)
	{
		AssetBundleRequest creditsAsset = gamedata.LoadAssetAsync<TextAsset>("Credits");
		yield return creditsAsset;
		List<string[]> creditsData = Universe.FromCsv((creditsAsset.asset as TextAsset).text);
		List<string> names = null;
		string sprite = string.Empty;
		string category = string.Empty;
		int i = 1;
		while (i < creditsData.Count)
		{
			if (string.IsNullOrEmpty(creditsData[i][0]) || string.IsNullOrEmpty(creditsData[i][1]) || string.IsNullOrEmpty(creditsData[i][3]))
			{
				goto IL_1C1;
			}
			if (!string.IsNullOrEmpty(category))
			{
				Universe.Credits.Add(new CreditsModel(category, names, sprite));
			}
			if (Universe.IsCorrectPlatform(creditsData[i][3]))
			{
				category = creditsData[i][0];
				names = new List<string>();
				sprite = ((!string.IsNullOrEmpty(creditsData[i][2])) ? creditsData[i][2] : string.Empty);
				goto IL_1C1;
			}
			sprite = (category = string.Empty);
			names = null;
			IL_1EA:
			i++;
			continue;
			IL_1C1:
			if (names != null)
			{
				names.Add(creditsData[i][1]);
				goto IL_1EA;
			}
			goto IL_1EA;
		}
		if (names != null)
		{
			Universe.Credits.Add(new CreditsModel(category, names, sprite));
		}
		yield break;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00050638 File Offset: 0x0004E838
	private static IEnumerator LoadImageOffsets(AssetBundle gamedata)
	{
		AssetBundleRequest imageOffsetsAsset = gamedata.LoadAssetAsync<TextAsset>("ImageOffsets");
		yield return imageOffsetsAsset;
		List<string[]> imageOffsetsData = Universe.FromCsv((imageOffsetsAsset.asset as TextAsset).text);
		for (int i = 0; i < imageOffsetsData.Count; i++)
		{
			if (!string.IsNullOrEmpty(imageOffsetsData[i][0]))
			{
				if (!string.IsNullOrEmpty(imageOffsetsData[i][1]) || !string.IsNullOrEmpty(imageOffsetsData[i][2]))
				{
					string name = imageOffsetsData[i][0];
					int leftOffset;
					int width;
					if (!int.TryParse(imageOffsetsData[i][1], out leftOffset) || !int.TryParse(imageOffsetsData[i][2], out width))
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"Could not parse ints from: ",
							imageOffsetsData[i][0],
							" ",
							imageOffsetsData[i][1],
							" ",
							imageOffsetsData[i][2]
						}));
					}
					else
					{
						bool isNSFWAsset = imageOffsetsData[i].Length >= 4 && !string.IsNullOrEmpty(imageOffsetsData[i][3]);
						ImageOffset imageOffset = new ImageOffset(leftOffset, width);
						ImageOffset existingImageOffset;
						if (Universe.ImageOffsets.TryGetValue(name, out existingImageOffset))
						{
							if (isNSFWAsset && GameState.NSFW)
							{
								Universe.ImageOffsets[name] = imageOffset;
							}
						}
						else
						{
							Universe.ImageOffsets.Add(name, imageOffset);
						}
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0005065C File Offset: 0x0004E85C
	private static bool IsCorrectPlatform(string platforms)
	{
		if (string.IsNullOrEmpty(platforms))
		{
			return false;
		}
		string[] array = platforms.Split(new char[]
		{
			','
		});
		string text = Application.platform.ToString().ToLowerInvariant();
		text = ((!(text == "webgl")) ? text : "nutaku");
		foreach (string text2 in array)
		{
			if (text2.ToLowerInvariant() == "all" || text2.ToLowerInvariant() == text)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00050704 File Offset: 0x0004E904
	public void InitializeDateImages()
	{
		GameState.AssetManager.GetBundle("universe/dates", false, delegate(AssetBundle dateBundle)
		{
			GameState.AssetManager.GetBundle("universe/date_icons", false, delegate(AssetBundle iconBundle)
			{
				foreach (KeyValuePair<short, DateModel> keyValuePair in Universe.Dates)
				{
					keyValuePair.Value.Icon = iconBundle.LoadAsset<Sprite>(keyValuePair.Value.IconSpriteKey);
					keyValuePair.Value.Sprite1 = dateBundle.LoadAsset<Sprite>("DATEimg_" + keyValuePair.Value.SpriteKey);
					Sprite sprite = dateBundle.LoadAsset<Sprite>("DATEimg_" + keyValuePair.Value.SpriteKey + "2");
					keyValuePair.Value.Sprite2 = ((!(sprite == null)) ? sprite : keyValuePair.Value.Sprite1);
				}
			});
		});
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00050734 File Offset: 0x0004E934
	public void PopulateUniverseData(Action onSuccess)
	{
		GameState.AssetManager.GetBundle("universe/hobbies_jobs", false, delegate(AssetBundle bundle)
		{
			Transform parent = this.transform.Find("Jobs/Scroll View/Content Panel");
			for (int i = 0; i < Universe.Jobs.Count; i++)
			{
				Requirement.JobType jobType = (Requirement.JobType)(1 << i);
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.JobPrefab, parent);
				Job2 component = gameObject.GetComponent<Job2>();
				component.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
				component.JobType = jobType;
				component.Data = Universe.Jobs[(short)(i + 1)];
				component.JobSprite1 = bundle.LoadAsset<Sprite>("jobview_" + component.Data.SpriteKey);
				component.JobSprite2 = bundle.LoadAsset<Sprite>("jobview_" + component.Data.SpriteKey + "1");
			}
			Transform parent2 = this.transform.Find("Hobbies/Scroll View/Content Panel");
			for (int j = 0; j < Universe.Hobbies.Count; j++)
			{
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(this.HobbyPrefab, parent2);
				Hobby2 component2 = gameObject2.GetComponent<Hobby2>();
				component2.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
				component2.Data = Universe.Hobbies[(short)(j + 1)];
				component2.HobbySprite1 = bundle.LoadAsset<Sprite>("hobbyview_" + component2.Data.SpriteKey);
				component2.HobbySprite2 = bundle.LoadAsset<Sprite>("hobbyview_" + component2.Data.SpriteKey + "1");
			}
			List<short> list = new List<short>();
			foreach (KeyValuePair<short, DateModel> keyValuePair in Universe.Dates)
			{
				list.Add(keyValuePair.Key);
			}
			list.Sort();
			Transform parent3 = this.transform.Find("Girls/Popups/Dating/Dialog/Scroll View/Viewport/Content");
			foreach (short key in list)
			{
				DateModel data = Universe.Dates[key];
				GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(this.DatePrefab, parent3);
				Date component3 = gameObject3.GetComponent<Date>();
				component3.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
				component3.Data = data;
			}
			this.InitializeDateImages();
			List<short> girlIds = new List<short>();
			foreach (KeyValuePair<short, GirlModel> keyValuePair2 in Universe.Girls)
			{
				girlIds.Add(keyValuePair2.Key);
			}
			girlIds.Sort();
			GameState.AssetManager.GetBundle("universe/girl_icons", false, delegate(AssetBundle iconBundle)
			{
				Transform transform = this.transform.Find("Girls/Girl List/Scroll View/Content Panel");
				for (int k = 0; k < girlIds.Count; k++)
				{
					string name = Universe.Girls[girlIds[k]].Name;
					Balance.GirlName girlName = Utilities.GirlFromString(name);
					GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(this.GirlPrefab, transform);
					Girl girl = gameObject4.GetComponent<Girl>();
					girl.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
					girl.GirlName = girlName;
					girl.Data = Universe.Girls[(short)(k + 1)];
					girl.GetComponent<Image>().sprite = iconBundle.LoadAsset<Sprite>("girllist_icon" + name + "LOCKED");
					girl.transform.Find("Unlocked").GetComponent<Image>().sprite = iconBundle.LoadAsset<Sprite>("girllist_icon" + name + "UNLOCKED");
					if (girlName == Balance.GirlName.Explora)
					{
						girl.transform.gameObject.AddComponent<ExploraSpinner>().Spinner = iconBundle.LoadAsset<Sprite>("spinner");
					}
					girl.transform.Find("Unlocked").GetComponent<Button>().onClick.AddListener(delegate()
					{
						GameState.GetGirlScreen().SetGirl(girl);
					});
					girl.transform.Find("Name").GetComponent<Text>().color = girl.Data.TextColor;
					girl.transform.Find("Requirement").GetComponent<Text>().text = Translations.GetGirlRequirement(girlName);
					girl.DateOffsets = new int[4];
					for (int l = 0; l < 4; l++)
					{
						girl.DateOffsets[l] = this.dateOffsets[k * 4 + l];
					}
				}
				if (this.FindGirl(Balance.GirlName.Ayano, transform) != null)
				{
					this.FindGirl(Balance.GirlName.Ayano, transform).Portrait = iconBundle.LoadAsset<Sprite>("girllist_iconAyano1UNLOCKED");
					this.FindGirl(Balance.GirlName.Ayano, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconAyano2UNLOCKED");
				}
				if (this.FindGirl(Balance.GirlName.Bearverly, transform) != null)
				{
					this.FindGirl(Balance.GirlName.Bearverly, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconBearverly2UNLOCKED");
				}
				if (this.FindGirl(Balance.GirlName.Alpha, transform) != null)
				{
					this.FindGirl(Balance.GirlName.Alpha, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconAlpha2UNLOCKED");
				}
				if (this.FindGirl(Balance.GirlName.Eva, transform) != null)
				{
					this.FindGirl(Balance.GirlName.Eva, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconEva2UNLOCKED");
				}
				if (this.FindGirl(Balance.GirlName.DarkOne, transform) != null)
				{
					this.FindGirl(Balance.GirlName.DarkOne, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconDarkOneGONE");
				}
				if (this.FindGirl(Balance.GirlName.QPiddy, transform) != null)
				{
					this.FindGirl(Balance.GirlName.QPiddy, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconQPernikissGONE");
				}
				if (this.FindGirl(Balance.GirlName.Generica, transform) != null)
				{
					this.FindGirl(Balance.GirlName.Generica, transform).LoverIcon = iconBundle.LoadAsset<Sprite>("girllist_iconGenerica2UNLOCKED");
				}
				if (onSuccess != null)
				{
					onSuccess();
				}
			});
		});
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00050774 File Offset: 0x0004E974
	private Girl FindGirl(Balance.GirlName name, Transform contentPanel)
	{
		for (int i = 0; i < contentPanel.childCount; i++)
		{
			Girl component = contentPanel.GetChild(i).GetComponent<Girl>();
			if (component.GirlName == name)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000507B4 File Offset: 0x0004E9B4
	public static List<string[]> FromCsv(string text)
	{
		List<string[]> list = new List<string[]>();
		using (StringReader stringReader = new StringReader(text))
		{
			string text2 = string.Empty;
			do
			{
				text2 = stringReader.ReadLine();
				if (!string.IsNullOrEmpty(text2))
				{
					string[] array = text2.Replace("\\n", "\n").Split(Universe.delimiter);
					if (array.Length > 0)
					{
						list.Add(array);
					}
				}
			}
			while (!string.IsNullOrEmpty(text2));
		}
		return list;
	}

	// Token: 0x04000988 RID: 2440
	public GameObject JobPrefab;

	// Token: 0x04000989 RID: 2441
	public GameObject HobbyPrefab;

	// Token: 0x0400098A RID: 2442
	public GameObject GirlPrefab;

	// Token: 0x0400098B RID: 2443
	public GameObject DatePrefab;

	// Token: 0x0400098C RID: 2444
	private int[] dateOffsets = new int[]
	{
		-200,
		50,
		-182,
		-10,
		-230,
		50,
		-64,
		-140,
		-229,
		100,
		-80,
		197,
		-227,
		50,
		-188,
		230,
		-202,
		50,
		17,
		-58,
		-227,
		60,
		-10,
		0,
		-208,
		60,
		-42,
		223,
		-208,
		41,
		18,
		108,
		-242,
		154,
		-126,
		-66,
		-233,
		100,
		-60,
		100,
		-257,
		60,
		-40,
		-60,
		-222,
		40,
		-171,
		264,
		-213,
		36,
		14,
		222,
		-167,
		66,
		14,
		-60,
		-205,
		66,
		14,
		-37,
		-249,
		66,
		0,
		21,
		-255,
		66,
		14,
		44,
		-255,
		66,
		14,
		-135,
		-214,
		66,
		64,
		-135,
		-214,
		66,
		64,
		-135,
		-214,
		66,
		64,
		72,
		-232,
		66,
		64,
		117,
		-232,
		66,
		64,
		-73,
		-232,
		66,
		64,
		39,
		-193,
		90,
		-5,
		10,
		-232,
		90,
		54,
		198,
		-232,
		74,
		-42,
		29,
		-218,
		74,
		0,
		61,
		-218,
		74,
		0,
		120,
		-201,
		74,
		0,
		9,
		-201,
		74,
		0,
		-180,
		-201,
		74,
		0,
		-180,
		-201,
		74,
		0,
		-35,
		-201,
		74,
		0,
		-35,
		-201,
		74,
		40,
		15,
		-202,
		74,
		40,
		-37,
		-202,
		74,
		40,
		-37,
		-202,
		74,
		40,
		-37,
		-192,
		74,
		40,
		-152,
		-192,
		74,
		40,
		5,
		-219,
		74,
		40,
		57,
		-227,
		74,
		40,
		-217,
		-180,
		74,
		0,
		72,
		-180,
		74,
		0,
		129,
		-180,
		74,
		0,
		206,
		-180,
		74,
		0,
		-200,
		-180,
		74,
		0,
		84
	};

	// Token: 0x0400098D RID: 2445
	private static bool loaded = false;

	// Token: 0x0400098E RID: 2446
	internal static Dictionary<string, ResourceModel> StringToResource = new Dictionary<string, ResourceModel>();

	// Token: 0x0400098F RID: 2447
	public static Dictionary<short, ResourceModel> Resources = new Dictionary<short, ResourceModel>();

	// Token: 0x04000990 RID: 2448
	public static Dictionary<short, GiftModel> Gifts = new Dictionary<short, GiftModel>();

	// Token: 0x04000991 RID: 2449
	public static Dictionary<short, OutfitModel> Outfits = new Dictionary<short, OutfitModel>();

	// Token: 0x04000992 RID: 2450
	public static Dictionary<short, HobbyModel> Hobbies = new Dictionary<short, HobbyModel>();

	// Token: 0x04000993 RID: 2451
	public static Dictionary<short, DateModel> Dates = new Dictionary<short, DateModel>();

	// Token: 0x04000994 RID: 2452
	public static Dictionary<short, JobModel> Jobs = new Dictionary<short, JobModel>();

	// Token: 0x04000995 RID: 2453
	public static Dictionary<short, GirlModel> Girls = new Dictionary<short, GirlModel>();

	// Token: 0x04000996 RID: 2454
	public static Dictionary<short, StatsModel> Stats = new Dictionary<short, StatsModel>();

	// Token: 0x04000997 RID: 2455
	public static Dictionary<short, AchievementModel> Achievements = new Dictionary<short, AchievementModel>();

	// Token: 0x04000998 RID: 2456
	public static Dictionary<short, PhoneModel> CellphoneGirls = new Dictionary<short, PhoneModel>();

	// Token: 0x04000999 RID: 2457
	public static Dictionary<short, TaskData> Tasks = new Dictionary<short, TaskData>();

	// Token: 0x0400099A RID: 2458
	public static Dictionary<short, EventData> Events = new Dictionary<short, EventData>();

	// Token: 0x0400099B RID: 2459
	public static List<CreditsModel> Credits = new List<CreditsModel>();

	// Token: 0x0400099C RID: 2460
	public static Dictionary<string, ImageOffset> ImageOffsets = new Dictionary<string, ImageOffset>();

	// Token: 0x0400099D RID: 2461
	private static readonly char[] delimiter = new char[]
	{
		'`'
	};
}
