using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000105 RID: 261
public class Album : MonoBehaviour
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060005FE RID: 1534 RVA: 0x0002FC78 File Offset: 0x0002DE78
	public int Page
	{
		get
		{
			return this._currentPage;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x0002FC80 File Offset: 0x0002DE80
	public static int AlbumCount
	{
		get
		{
			return Album.AlbumPictures.Count;
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002FC8C File Offset: 0x0002DE8C
	public static void Clear()
	{
		Album.AlbumPictures.Clear();
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002FC98 File Offset: 0x0002DE98
	private void CacheImage(int bitIndex)
	{
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002FC9C File Offset: 0x0002DE9C
	public void AddDate(Requirement.DateType date, int girlIndex)
	{
		int item = Requirement.IndexFromDateType(date) + girlIndex * 32 + 4096;
		if (Album.AlbumPictures.Contains(item))
		{
			return;
		}
		Album.AlbumPictures.Add(item);
		this.CacheImage(girlIndex * 8 + Requirement.IndexFromDateType(date) + 4);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002FCEC File Offset: 0x0002DEEC
	public void AddPicture(int i)
	{
		if (Album.AlbumPictures.Contains(i))
		{
			return;
		}
		int item = Mathf.FloorToInt((float)i / 4f + float.Epsilon);
		if (!this._pages.Contains(item))
		{
			this._pages.Add(item);
		}
		this._pages.Sort();
		Album.AlbumPictures.Add(i);
		this.CacheImage(i / 4 * 8 + i % 4);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002FD60 File Offset: 0x0002DF60
	public static void Add(GirlModel data, int pinup)
	{
		Album component = GameState.CurrentState.transform.Find("Popups/Memory Album").GetComponent<Album>();
		int num = (int)(data.Id - 1);
		if (component != null)
		{
			component.AddPicture(num * 4 + pinup);
		}
		else
		{
			Debug.Log("Could not find the album object to add a picture.  Did the hierarchy change?");
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002FDB8 File Offset: 0x0002DFB8
	public static void Add(Requirement.DateType date, Girl girl)
	{
		Album component = GameState.CurrentState.transform.Find("Popups/Memory Album").GetComponent<Album>();
		if (girl.GirlName >= Balance.GirlName.Cassie)
		{
			component.AddDate(date, (int)girl.GirlName);
		}
		else
		{
			Debug.Log(string.Format("Could not find a girl with the name {0}.", girl.GirlName.ToFriendlyString()));
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002FE18 File Offset: 0x0002E018
	private void Update()
	{
		if (this.currentTime > 0f)
		{
			Transform transform = base.transform.Find("Dialog/Slider Mask/Images 1");
			float x = transform.transform.localPosition.x;
			this.currentTime = Mathf.Max(0f, this.currentTime - Time.deltaTime * 2f);
			float num = this.startingPosition * (1f - Utilities.Easing.CubicEaseOut(1f - this.currentTime, 0f, 1f, 1f));
			float num2 = x - num;
			transform.transform.localPosition = new Vector3(num, (float)this.yoffset, 0f);
			Transform transform2 = base.transform.Find("Dialog/Slider Mask/Images 2");
			transform2.localPosition = new Vector3(transform2.localPosition.x - num2, (float)this.yoffset, 0f);
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0002FF08 File Offset: 0x0002E108
	private void UnloadAssets()
	{
		for (int i = 0; i < 8; i++)
		{
			base.transform.Find("Dialog/Slider Mask/Images 2").GetChild(i).GetComponent<Image>().sprite = this.UnknownPicture;
		}
		if (this._bundlesToUnload.Count != 0)
		{
			foreach (AssetBundle bundle in this._bundlesToUnload)
			{
				GameState.AssetManager.UnloadBundle(bundle, true);
			}
			this._bundlesToUnload.Clear();
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0002FFC8 File Offset: 0x0002E1C8
	private void OnDisable()
	{
		for (int i = 0; i < 8; i++)
		{
			base.transform.Find("Dialog/Slider Mask/Images 1").GetChild(i).GetComponent<Image>().sprite = this.UnknownPicture;
			base.transform.Find("Dialog/Slider Mask/Images 2").GetChild(i).GetComponent<Image>().sprite = this.UnknownPicture;
		}
		if (this._currentBundle != null)
		{
			this._bundlesToUnload.Add(this._currentBundle);
		}
		if (this._currentBundleNsfw != null)
		{
			this._bundlesToUnload.Add(this._currentBundleNsfw);
		}
		this._currentBundleNsfw = (this._currentBundle = null);
		this.UnloadAssets();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00030090 File Offset: 0x0002E290
	private void Init()
	{
		this._girlCount = GameState.CurrentState.transform.Find("Girls").GetComponentsInChildren<Girl>(true).Length;
		for (int i = 0; i < 16; i++)
		{
			Transform images = (i >= 8) ? base.transform.Find("Dialog/Slider Mask/Images 2") : base.transform.Find("Dialog/Slider Mask/Images 1");
			Image image = this.GetImage(images, i % 8);
			if (image.GetComponent<Button>() == null)
			{
				image.gameObject.AddComponent(typeof(Button));
				image.gameObject.AddComponent(typeof(ButtonScale));
			}
			int temp = i % 8;
			Button button = image.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate()
			{
				this.InitPopup(button.GetComponent<Image>(), temp);
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.HoverSelect3A);
			});
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00030198 File Offset: 0x0002E398
	private Transform ActiveImages()
	{
		return base.transform.Find("Dialog/Slider Mask/Images 1");
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000301AC File Offset: 0x0002E3AC
	private Image GetImage(Transform images, int i)
	{
		return images.GetChild(i).GetComponent<Image>();
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x000301BC File Offset: 0x0002E3BC
	private void SwapImages()
	{
		Transform images = base.transform.Find("Dialog/Slider Mask/Images 1");
		Transform transform = base.transform.Find("Dialog/Slider Mask/Images 2");
		for (int i = 0; i < 8; i++)
		{
			Image image = this.GetImage(images, i);
			Image image2 = this.GetImage(transform, i);
			image2.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
			image2.sprite = image.sprite;
		}
		transform.transform.localPosition = new Vector3((float)this.xoffset, (float)this.yoffset, 0f);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00030258 File Offset: 0x0002E458
	public bool IsPinupUnlocked(int pinupPage, int image)
	{
		switch (pinupPage)
		{
		case 0:
			return (image == 0 && (((int)Playfab.AwardedItems & 5) != 0 || TaskManager.IsCompletedEventSet(68))) || (image == 1 && ((Playfab.AwardedItems & Playfab.PlayfabItems.Easter2017) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
			{
				24,
				73,
				142
			}))) || (image == 2 && ((Playfab.AwardedItems & Playfab.PlayfabItems.July2017) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
			{
				31,
				80,
				118
			}))) || (image == 3 && ((Playfab.AwardedItems & Playfab.PlayfabItems.BackToSchool2017) != (Playfab.PlayfabItems)0L || TaskManager.IsAnyEventCompleted(new int[]
			{
				7,
				45,
				90,
				124,
				155
			}))) || (image == 4 && (Playfab.AwardedItems & Playfab.PlayfabItems.Ayano2017) != (Playfab.PlayfabItems)0L) || (image == 5 && TaskManager.IsAnyEventCompleted(new int[]
			{
				2,
				65,
				103,
				145,
				181
			})) || (image == 6 && TaskManager.IsAnyEventCompleted(new int[]
			{
				4,
				52,
				94,
				127
			})) || (image == 7 && TaskManager.IsAnyEventCompleted(new int[]
			{
				6,
				53,
				97,
				160
			}));
		case 1:
			return (image == 0 && TaskManager.IsAnyEventCompleted(new int[]
			{
				11,
				60,
				100,
				133,
				163
			})) || (image == 1 && TaskManager.IsAnyEventCompleted(new int[]
			{
				18,
				66,
				106,
				136,
				169
			})) || (image == 2 && TaskManager.IsAnyEventCompleted(new int[]
			{
				19,
				67
			})) || (image == 3 && TaskManager.IsAnyEventCompleted(new int[]
			{
				20,
				69,
				109,
				178
			})) || (image == 4 && TaskManager.IsAnyEventCompleted(new int[]
			{
				21,
				70,
				130
			})) || (image == 5 && TaskManager.IsAnyEventCompleted(new int[]
			{
				22,
				71
			})) || (image == 6 && TaskManager.IsAnyEventCompleted(new int[]
			{
				23,
				72
			})) || (image == 7 && TaskManager.IsAnyEventCompleted(new int[]
			{
				25,
				74,
				115
			}));
		case 2:
			return (image == 0 && TaskManager.IsAnyEventCompleted(new int[]
			{
				26,
				75,
				112
			})) || (image == 1 && TaskManager.IsAnyEventCompleted(new int[]
			{
				27,
				76
			})) || (image == 2 && TaskManager.IsAnyEventCompleted(new int[]
			{
				44,
				86,
				121,
				152,
				184
			})) || (image == 3 && (Playfab.AwardedItems & Playfab.PlayfabItems.Winter2018) != (Playfab.PlayfabItems)0L) || (image == 4 && ((Playfab.AwardedItems & Playfab.PlayfabItems.Nutaku2019) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(139))) || (image == 5 && this.IsPinupUnlocked(2, 2) && GameState.NSFW) || (image == 6 && (Playfab.AwardedItems & Playfab.PlayfabItems.Winter2020) != (Playfab.PlayfabItems)0L) || (image == 7 && (Playfab.AwardedItems & (Playfab.PlayfabItems)((ulong)int.MinValue)) != (Playfab.PlayfabItems)0L);
		case 3:
			return (image == 0 && TaskManager.IsCompletedEventSet(151)) || (image == 1 && TaskManager.IsCompletedEventSet(166)) || (image == 2 && GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Suzu)) || (image == 3 && (Playfab.AwardedItems & Playfab.PlayfabItems.Anniversary2022) != (Playfab.PlayfabItems)0L) || (image == 4 && TaskManager.IsCompletedEventSet(172)) || (image == 5 && TaskManager.IsCompletedEventSet(175));
		default:
			return false;
		}
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0003065C File Offset: 0x0002E85C
	private IEnumerator InitMiscPage(int MiscPageNumber)
	{
		if (this.NameText != null)
		{
			if (MiscPageNumber == 3)
			{
				this.NameText.text = Translations.GetTranslation("everything_else_42_11", "Animated Pin-Ups");
			}
			else
			{
				this.NameText.text = Translations.GetTranslation("everything_else_42_11", "Pin-Ups");
			}
		}
		base.transform.Find("Dialog/Page Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_42_10", "Page {0}"), (this._currentPage + 1).ToString());
		base.transform.Find("Dialog/Previous Button").GetComponent<Button>().interactable = (this._pages.Count > 1);
		base.transform.Find("Dialog/Next Button").GetComponent<Button>().interactable = (this._pages.Count > 1);
		if (this._currentBundle != null)
		{
			this._bundlesToUnload.Add(this._currentBundle);
		}
		if (this._currentBundleNsfw != null)
		{
			this._bundlesToUnload.Add(this._currentBundleNsfw);
		}
		this._currentBundle = (this._currentBundleNsfw = null);
		this._currentGirl = Balance.GirlName.Unknown;
		this.UnloadAssets();
		this._loadingPage = MiscPageNumber;
		for (int i = 0; i < 8; i++)
		{
			this.GetImage(this.ActiveImages(), i).sprite = this.UnknownPicture;
		}
		AssetBundleAsync sfwBundle = GameState.AssetManager.GetBundleAsync("universe/promos", false);
		yield return sfwBundle;
		if (sfwBundle.AssetBundle == null)
		{
			yield break;
		}
		if (this._loadingPage != MiscPageNumber)
		{
			yield break;
		}
		AssetBundle nsfwBundle = null;
		if (GameState.NSFW)
		{
			AssetBundleAsync nsfwBundleRequest = GameState.AssetManager.GetBundleAsync("universe/promos_nsfw", false);
			yield return nsfwBundleRequest;
			if (this._loadingPage != MiscPageNumber)
			{
				yield break;
			}
			if (nsfwBundleRequest.AssetBundle != null)
			{
				nsfwBundle = nsfwBundleRequest.AssetBundle;
			}
		}
		int end = (MiscPageNumber + 1) * 8;
		for (int j = MiscPageNumber * 8; j < end; j++)
		{
			Image image = this.GetImage(this.ActiveImages(), j % 8);
			bool unlocked = this.IsPinupUnlocked(MiscPageNumber, j % 8);
			if (unlocked)
			{
				string sfwAssetName = this.sfwPinupNames[j];
				string nsfwAssetName = this.nsfwPinupNames[j];
				AssetBundleRequest imageRequest = (!GameState.NSFW || string.IsNullOrEmpty(nsfwAssetName) || !(nsfwBundle != null)) ? ((string.IsNullOrEmpty(sfwAssetName) || sfwBundle == null) ? null : sfwBundle.AssetBundle.LoadAssetAsync<Sprite>(sfwAssetName)) : nsfwBundle.LoadAssetAsync<Sprite>(nsfwAssetName);
				if (imageRequest != null)
				{
					yield return imageRequest;
					if (imageRequest != null && this._currentGirl == Balance.GirlName.Unknown)
					{
						if (this._loadingPage != MiscPageNumber)
						{
							yield break;
						}
						if (imageRequest.asset != null)
						{
							image.sprite = (imageRequest.asset as Sprite);
							this._girlSprites[j % 8] = image.sprite;
							image.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
							image.GetComponent<ButtonScale>().enabled = true;
						}
						else
						{
							image.sprite = this.UnknownPicture;
							image.GetComponent<Button>().transition = Selectable.Transition.None;
							image.GetComponent<ButtonScale>().enabled = false;
						}
					}
				}
			}
			else if (this._currentGirl == Balance.GirlName.Unknown)
			{
				image.sprite = this.UnknownPicture;
				image.GetComponent<Button>().transition = Selectable.Transition.None;
				image.GetComponent<ButtonScale>().enabled = false;
			}
		}
		if (sfwBundle.AssetBundle != null)
		{
			GameState.AssetManager.UnloadBundle(sfwBundle.AssetBundle);
		}
		if (nsfwBundle != null)
		{
			GameState.AssetManager.UnloadBundle(nsfwBundle);
		}
		yield break;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00030688 File Offset: 0x0002E888
	public void InitPage(int page)
	{
		if (page == 0)
		{
			this._currentPage = 0;
		}
		else
		{
			for (int i = 0; i < this._pages.Count; i++)
			{
				if (this._pages[i] == page)
				{
					this._currentPage = i;
				}
			}
		}
		if (this._girlCount == 0)
		{
			this.Init();
		}
		base.gameObject.SetActive(true);
		page = Mathf.Max(0, page);
		if (page >= this._girlCount)
		{
			base.StartCoroutine(this.InitMiscPage(page - this._girlCount));
			return;
		}
		int firstPhoto = 4 * page;
		for (int j = 0; j < 8; j++)
		{
			Image image = this.GetImage(this.ActiveImages(), j);
			image.sprite = this.UnknownPicture;
			image.GetComponent<Button>().transition = Selectable.Transition.None;
		}
		List<short> list = new List<short>();
		foreach (KeyValuePair<short, GirlModel> keyValuePair in Universe.Girls)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort();
		Balance.GirlName girlName = Utilities.GirlFromString(Universe.Girls[list[page]].Name);
		base.StartCoroutine(this.LoadAlbumAssets(firstPhoto, girlName));
		if (this.NameText != null)
		{
			this.NameText.text = Translations.TranslateGirlName(girlName);
		}
		base.transform.Find("Dialog/Page Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_42_10", "Page {0}"), (this._currentPage + 1).ToString());
		int pinupPageCount = this.GetPinupPageCount();
		for (int k = 0; k < pinupPageCount; k++)
		{
			if (!this._pages.Contains(this._girlCount + k))
			{
				this._pages.Add(this._girlCount + k);
			}
		}
		this._pages.Sort();
		base.transform.Find("Dialog/Previous Button").GetComponent<Button>().interactable = (this._pages.Count > 1);
		base.transform.Find("Dialog/Next Button").GetComponent<Button>().interactable = (this._pages.Count > 1);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00030908 File Offset: 0x0002EB08
	private int GetPinupPageCount()
	{
		for (int i = 3; i >= 0; i--)
		{
			for (int j = 0; j < 8; j++)
			{
				if (this.IsPinupUnlocked(i, j))
				{
					return i + 1;
				}
			}
		}
		return 0;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0003094C File Offset: 0x0002EB4C
	public void InitPage0()
	{
		if (this._pages == null || this._pages.Count == 0)
		{
			this.InitPage(0);
		}
		else
		{
			this.InitPage(this._pages[0]);
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00030994 File Offset: 0x0002EB94
	public void NextPage()
	{
		this.SwapImages();
		base.transform.Find("Dialog/Slider Mask/Images 1").transform.localPosition = new Vector3(892f, (float)this.yoffset, 0f);
		this.startingPosition = 892f;
		this.currentTime = 1f;
		if (this._currentPage == this._pages.Count - 1)
		{
			this._currentPage = 0;
		}
		else
		{
			this._currentPage++;
		}
		this.InitPage(this._pages[this._currentPage]);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00030A38 File Offset: 0x0002EC38
	public void PrevPage()
	{
		this.SwapImages();
		base.transform.Find("Dialog/Slider Mask/Images 1").transform.localPosition = new Vector3(-877f, (float)this.yoffset, 0f);
		this.startingPosition = -877f;
		this.currentTime = 1f;
		if (this._currentPage == 0)
		{
			this._currentPage = this._pages.Count - 1;
		}
		else
		{
			this._currentPage--;
		}
		this.InitPage(this._pages[this._currentPage]);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00030ADC File Offset: 0x0002ECDC
	public void InitDatePopup(Girl girl, int dateIndex)
	{
		Balance.GirlName girlName = (!(girl == null)) ? girl.GirlName : Balance.GirlName.Unknown;
		if (girl != null)
		{
			GameState.CurrentState.StartCoroutine(this.GetDateSpriteAsync(girlName, dateIndex));
		}
		else
		{
			this.GirlOverlay.gameObject.SetActive(false);
		}
		if (this.ReminisceButton != null)
		{
			this.ReminisceButton.gameObject.SetActive(false);
		}
		this._currentImage = 0;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00030B60 File Offset: 0x0002ED60
	private IEnumerator GetDateSpriteAsync(Balance.GirlName girlName, int dateIndex)
	{
		Girl girl = Girl.FindGirl(girlName);
		if (girl == null)
		{
			yield break;
		}
		Girl.LoadImageAsyncRequest dateRequest = new Girl.LoadImageAsyncRequest(girl, Girl.SpriteType.Moonlight + dateIndex);
		yield return dateRequest.GetSpriteAsync();
		if (dateRequest.Sprite == null)
		{
			yield break;
		}
		this._bundlesToUnload.Add(dateRequest.Bundle);
		this.GirlOverlay.sprite = dateRequest.Sprite;
		float scaling = 260f / this.GirlOverlay.sprite.rect.size.y;
		this.GirlOverlay.rectTransform.sizeDelta = scaling * this.GirlOverlay.sprite.rect.size;
		if (girl.DateOffsets.Length > dateIndex)
		{
			this.GirlOverlay.rectTransform.localPosition = new Vector3((float)girl.DateOffsets[dateIndex], 0f, 0f);
		}
		else
		{
			this.GirlOverlay.rectTransform.localPosition = new Vector3(0f, 0f, 0f);
		}
		this.GirlOverlay.gameObject.SetActive(true);
		bool darkPortal = Girl.FindGirl(Balance.GirlName.QPiddy).Love != 9 && GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Karma);
		DateModel dateModel = Universe.Dates[(short)(dateIndex + 1)];
		Sprite dateSprite = (!darkPortal) ? dateModel.Sprite1 : dateModel.Sprite2;
		this.AlbumPopup.InitAlbum(girlName, dateSprite, 0, true, false, 0);
		yield break;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00030B98 File Offset: 0x0002ED98
	public void InitPopup(Image image, int imageIndex)
	{
		if (image.sprite == this.UnknownPicture)
		{
			return;
		}
		int num = imageIndex - 4;
		Balance.GirlName girl = (Balance.GirlName)this._pages[this._currentPage];
		Girl girl2 = Girl.FindGirl(girl);
		if (this._pages[this._currentPage] >= this._girlCount)
		{
			int num2 = this._pages[this._currentPage] - this._girlCount;
			bool flag = imageIndex == 0 && num2 == 0;
			Kiss albumPopup = this.AlbumPopup;
			bool award = flag;
			albumPopup.InitAlbum(girl, image.sprite, imageIndex, false, award, num2 * 8 + imageIndex);
			this.GirlOverlay.gameObject.SetActive(false);
			if (this.ReminisceButton != null)
			{
				this.ReminisceButton.gameObject.SetActive(false);
			}
		}
		else if (num >= 0)
		{
			this.InitDatePopup(girl2, num);
		}
		else
		{
			this.AlbumPopup.InitAlbum(girl, image.sprite, imageIndex, false, false, 0);
			this.GirlOverlay.gameObject.SetActive(false);
			this._currentImage = imageIndex;
			if (this.ReminisceButton != null)
			{
				this.ReminisceButton.gameObject.SetActive((imageIndex % 4 == 3 && GameState.NSFW) || imageIndex % 4 == 0);
			}
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00030CF4 File Offset: 0x0002EEF4
	public void OpenFinale()
	{
		this.AlbumPopup.gameObject.SetActive(false);
		Balance.GirlName girl = Utilities.GirlFromString(Universe.Girls[(short)(this._pages[this._currentPage] + 1)].Name);
		Girl girl2 = Girl.FindGirl(girl);
		if (girl2 != null && GameState.NSFW && this._currentImage % 4 == 3)
		{
			GameState.GetCutSceneProvider().Initialize(new FinalInteraction(girl2, girl2.Data.SexyDataList));
		}
		else if (this._currentImage % 4 == 0)
		{
			GameState.CurrentState.transform.Find("Girls").GetComponent<Girls>().LaunchIntro(girl2, false);
		}
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00030DB4 File Offset: 0x0002EFB4
	private void SetImage(int firstPhoto, int i, Sprite sprite)
	{
		this._girlSprites[i] = sprite;
		Image component = this.ActiveImages().GetChild(i).GetComponent<Image>();
		bool flag = !Album.AlbumPictures.Contains(firstPhoto + i);
		component.sprite = ((!flag) ? sprite : this.UnknownPicture);
		component.GetComponent<Button>().transition = ((!flag) ? Selectable.Transition.ColorTint : Selectable.Transition.None);
		component.GetComponent<ButtonScale>().enabled = !flag;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00030E2C File Offset: 0x0002F02C
	private void SetDateImage(int firstPhoto, int i)
	{
		Image component = this.ActiveImages().GetChild(i).GetComponent<Image>();
		int item = 4096 + (i - 4) + this._pages[this._currentPage] * 32;
		bool flag = !Album.AlbumPictures.Contains(item);
		bool flag2 = Girl.FindGirl(Balance.GirlName.QPiddy).Love != 9 && GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Karma);
		DateModel dateModel = Universe.Dates[(short)(i - 3)];
		component.sprite = ((!flag) ? ((!flag2) ? dateModel.Sprite2 : dateModel.Sprite1) : this.UnknownPicture);
		component.GetComponent<Button>().transition = ((!flag) ? Selectable.Transition.ColorTint : Selectable.Transition.None);
		component.GetComponent<ButtonScale>().enabled = !flag;
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x00030F04 File Offset: 0x0002F104
	public bool IsLoadingBundle
	{
		get
		{
			return this._isLoadingBundle;
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00030F0C File Offset: 0x0002F10C
	private IEnumerator LoadAlbumAssets(int firstPhoto, Balance.GirlName girl)
	{
		this._isLoadingBundle = true;
		if (this._bundlesToUnload.Count > 0)
		{
			this.UnloadAssets();
		}
		if (this._currentBundle != null)
		{
			this._bundlesToUnload.Add(this._currentBundle);
		}
		if (this._currentBundleNsfw != null)
		{
			this._bundlesToUnload.Add(this._currentBundleNsfw);
		}
		for (int i = 0; i < this._girlSprites.Length; i++)
		{
			this._girlSprites[i] = null;
		}
		this._currentGirl = girl;
		if (GameState.NSFW)
		{
			AssetBundleAsync nsfwRequest = GameState.AssetManager.GetBundleAsync(string.Format("{0}/{0}_pinups_nsfw", girl.ToLowerFriendlyString()), false);
			yield return nsfwRequest;
			if (this._currentGirl != girl)
			{
				this._isLoadingBundle = false;
				yield break;
			}
			this._currentBundleNsfw = nsfwRequest.AssetBundle;
			if (this._currentBundleNsfw != null)
			{
				yield return this.ProcessAssets(firstPhoto, nsfwRequest.AssetBundle, girl);
			}
		}
		if (this._currentGirl != girl)
		{
			this._isLoadingBundle = false;
			yield break;
		}
		AssetBundleAsync sfwRequest = GameState.AssetManager.GetBundleAsync(string.Format("{0}/{0}_pinups", girl.ToLowerFriendlyString()), false);
		yield return sfwRequest;
		if (this._currentGirl != girl)
		{
			this._isLoadingBundle = false;
			yield break;
		}
		this._currentBundle = sfwRequest.AssetBundle;
		if (this._currentBundle != null)
		{
			yield return this.ProcessAssets(firstPhoto, sfwRequest.AssetBundle, girl);
		}
		this._isLoadingBundle = false;
		yield break;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00030F44 File Offset: 0x0002F144
	private IEnumerator ProcessAssets(int firstPhoto, AssetBundle bundle, Balance.GirlName girl)
	{
		if (this.introImageNames.Count == 0)
		{
			foreach (string s in Album.introImageName)
			{
				this.introImageNames.Add(s.ToLowerInvariant());
			}
		}
		string[] names = bundle.GetAllAssetNames();
		this.SetDateImage(firstPhoto, 4);
		this.SetDateImage(firstPhoto, 5);
		this.SetDateImage(firstPhoto, 6);
		this.SetDateImage(firstPhoto, 7);
		for (int i = 0; i < names.Length; i++)
		{
			string name = names[i];
			string lower = name.ToLowerInvariant();
			lower = lower.Substring(0, names[i].Length - 4);
			lower = lower.Substring(lower.LastIndexOf('/') + 1);
			if (lower.StartsWith("date") || lower.StartsWith("event"))
			{
				if (!this.introImageNames.Contains(lower) || !(this._girlSprites[0] != null))
				{
					if (!lower.StartsWith("eventcgs01_") || !(this._girlSprites[1] != null))
					{
						if (!lower.StartsWith("eventcgs02_") || !(this._girlSprites[2] != null))
						{
							if (!lower.StartsWith("eventcgfinal_") || lower.Contains("1") || lower.Contains("2") || !(this._girlSprites[3] != null))
							{
								if (!lower.StartsWith("eventcgs00_") || this.introImageNames.Contains(lower))
								{
									Utilities.AsyncSpriteAssetRequest request = new Utilities.AsyncSpriteAssetRequest(bundle, name);
									yield return request.GetSpriteAsync();
									if (this._bundlesToUnload.Contains(bundle) || girl != this._currentGirl)
									{
										Debug.Log("Coroutine was still running, but bundle was swapped.");
										yield break;
									}
									Sprite sprite = request.Sprite;
									if (sprite == null)
									{
										Debug.LogWarning(string.Format("{0} was not loaded successfully", name));
										yield break;
									}
									if (this.introImageNames.Contains(lower))
									{
										this.SetImage(firstPhoto, 0, sprite);
									}
									else if (lower.StartsWith("eventcgs01_"))
									{
										this.SetImage(firstPhoto, 1, sprite);
									}
									else if (lower.StartsWith("eventcgs02_"))
									{
										this.SetImage(firstPhoto, 2, sprite);
									}
									else if (lower.StartsWith("eventcgfinal_") && !lower.Contains("1") && !lower.Contains("2"))
									{
										this.SetImage(firstPhoto, 3, sprite);
									}
								}
							}
						}
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x040005F2 RID: 1522
	public static readonly int CacheSize = 6;

	// Token: 0x040005F3 RID: 1523
	private static List<int> AlbumPictures = new List<int>();

	// Token: 0x040005F4 RID: 1524
	public static Dictionary<int, Sprite> AlbumSprites = new Dictionary<int, Sprite>();

	// Token: 0x040005F5 RID: 1525
	private int _girlCount;

	// Token: 0x040005F6 RID: 1526
	private int _currentPage;

	// Token: 0x040005F7 RID: 1527
	private int _currentImage;

	// Token: 0x040005F8 RID: 1528
	private List<int> _pages = new List<int>();

	// Token: 0x040005F9 RID: 1529
	private long[] _albumCache = new long[Album.CacheSize];

	// Token: 0x040005FA RID: 1530
	private int xoffset;

	// Token: 0x040005FB RID: 1531
	private int yoffset;

	// Token: 0x040005FC RID: 1532
	public Sprite UnknownPicture;

	// Token: 0x040005FD RID: 1533
	public Button ReminisceButton;

	// Token: 0x040005FE RID: 1534
	public Text NameText;

	// Token: 0x040005FF RID: 1535
	public Kiss AlbumPopup;

	// Token: 0x04000600 RID: 1536
	public Image GirlOverlay;

	// Token: 0x04000601 RID: 1537
	private string[] sfwPinupNames = new string[]
	{
		"summergift",
		"easterelle",
		"SUMMERgroup",
		"SCHOOLgroup",
		"eventCGAYANOevent01",
		"nutakuanniCG",
		"HALLOWEENgroup",
		"BonchovyPinup",
		"ChristmasPinup",
		"SteamvalentineBG",
		"PinupBed_Fumi",
		"PinupBed_Nina",
		"PinupBed_Alpha",
		"PinupBed_Bearverly",
		"PinupBed_Bear",
		"PinupBed_Pamu",
		"PinupBed_Bonnibel",
		"PinupBed_Ayano",
		"YUKATAgroup",
		"Snowballpinup",
		"cakepinup",
		string.Empty,
		"XmasGirlsPinup",
		"anniversary",
		"PeanutStudy",
		"peanutBEDthumbnail",
		"SuzuPinup4CC",
		"mio6thCrushCrush",
		"SoyandLake",
		"mioAnimated"
	};

	// Token: 0x04000602 RID: 1538
	private string[] nsfwPinupNames = new string[]
	{
		"summergift_nsfw",
		"easterelle_nsfw",
		"SUMMERgroupNSFW",
		"SCHOOLgroupNSFW",
		"eventCGAYANOevent01_nsfw",
		"nutakuanniCG",
		"HALLOWEENgroup",
		"BonchovyPinupnsfw",
		"ChristmasPinup",
		"SteamvalentineBG",
		"PinupBed_Fumi",
		"PinupBed_Nina",
		"PinupBed_Alpha",
		"PinupBed_Bearverly",
		"PinupBed_Bear",
		"PinupBed_Pamu",
		"PinupBed_Bonnibel",
		"PinupBed_Ayano",
		string.Empty,
		"SnowballpinupNSFW",
		"cakepinupNSFW",
		"YUKATAgroup",
		"XmasGirlsPinup",
		"anniversaryNSFW",
		string.Empty,
		"peanutBEDthumbnailNSFW",
		"SuzuPinup4CCNSFW",
		"mio6thCrushCrushNSFW",
		"SoyandLakeNSFW",
		"mioAnimatedNSFW"
	};

	// Token: 0x04000603 RID: 1539
	private float startingPosition;

	// Token: 0x04000604 RID: 1540
	private float currentTime;

	// Token: 0x04000605 RID: 1541
	private Sprite[] _girlSprites = new Sprite[8];

	// Token: 0x04000606 RID: 1542
	private List<AssetBundle> _bundlesToUnload = new List<AssetBundle>();

	// Token: 0x04000607 RID: 1543
	private AssetBundle _currentBundle;

	// Token: 0x04000608 RID: 1544
	private AssetBundle _currentBundleNsfw;

	// Token: 0x04000609 RID: 1545
	private Balance.GirlName _currentGirl;

	// Token: 0x0400060A RID: 1546
	private int _loadingPage;

	// Token: 0x0400060B RID: 1547
	private bool _isLoadingBundle;

	// Token: 0x0400060C RID: 1548
	private static readonly string[] introImageName = new string[]
	{
		"eventCGs00_alpha",
		"eventCGs00_ayano",
		"eventCGs00_bearverly",
		"eventCGs00_bonchovy1",
		"eventCGs00_bonnibel",
		"eventCGs00_cassie",
		"eventCGs00_catara2",
		"eventCGs00_charlotte1",
		"eventCGs00_claudia",
		"eventCGs00_darkone",
		"eventCGs00_elle",
		"eventCGs00_eva",
		"eventCGs00_fumi",
		"eventCGs00_iro",
		"eventCGs00_jelle1",
		"eventCGs00_juliet1",
		"eventCGs00_wendy",
		"eventCGs00_karma",
		"eventCGs00_luna",
		"eventCGs00_mio",
		"eventCGs00_nina",
		"eventCGs00_nutaku",
		"eventCGs00_odango1",
		"eventCGs00_pamu",
		"eventCGs00_peanut",
		"eventCGs00_qpiddy",
		"eventCGs00_quill",
		"eventCGs00_quillzone1",
		"eventCGs00_rosa",
		"eventCGs00_roxxy1",
		"eventCGs00_shibuki1",
		"eventCGs00_sirina1",
		"eventCGs00_spectrum1",
		"eventCGs00_sutra",
		"eventCGs00_tessa2",
		"eventCGs00_vellatrix",
		"eventcgs00_Darya",
		"eventCGs00_ruri",
		"eventCGs00_generica1",
		"eventCGs00_suzu1",
		"eventCGs00_lustat1",
		"eventCGs00_sawyer",
		"eventCGs00_explora",
		"eventCGs00_esper",
		"eventCGs00_renee1",
		"eventCGs00_mallory2",
		"eventCGs00_lake2"
	};

	// Token: 0x0400060D RID: 1549
	private List<string> introImageNames = new List<string>();
}
