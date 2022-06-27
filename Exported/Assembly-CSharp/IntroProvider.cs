using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000113 RID: 275
public class IntroProvider : MonoBehaviour
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x00038790 File Offset: 0x00036990
	private void Start()
	{
		if (this.loading == null)
		{
			this.loading = base.transform.Find("Loading");
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x000387BC File Offset: 0x000369BC
	private void Update()
	{
		if (this.loading.gameObject.activeSelf)
		{
			this.loading.eulerAngles = new Vector3(0f, 0f, this.loading.eulerAngles.z - Time.deltaTime * 180f);
		}
		if (this.skipping)
		{
			if (this.fade == 0f)
			{
				this.TextBox.gameObject.SetActive(false);
				this.NextButton.gameObject.SetActive(false);
				this.SkipButton.gameObject.SetActive(false);
				if (this.introduction != null && this.introduction.Girl.Love != 9 && GameState.GetGirlScreen().gameObject.activeInHierarchy && !GameState.CurrentState.transform.Find("Popups/Memory Album").gameObject.activeInHierarchy)
				{
					GameState.GetGirlScreen().SetGirl(this.introduction.Girl);
					GameState.GetGirlScreen().StoreState();
				}
			}
			if (this.fade < 1f)
			{
				this.Background.color = new Color(0.1882353f, 0.1882353f, 0.1882353f, 1f - this.fade);
				if (this.Portrait.color.a != 0f)
				{
					this.Portrait.color = new Color(1f, 1f, 1f, 1f - this.fade);
				}
				if (this.TextBoxBackground.color.a != 0f)
				{
					this.TextBoxBackground.color = new Color(1f, 1f, 1f, 1f - this.fade);
				}
				this.fade += Time.deltaTime;
			}
			else
			{
				if (this.introduction != null)
				{
					this.introduction.Destroy(this);
					foreach (AssetBundle bundle in this.loadedBundles)
					{
						GameState.AssetManager.UnloadBundle(bundle, true);
					}
					this.loadedBundles.Clear();
				}
				base.gameObject.SetActive(false);
				GC.Collect();
			}
		}
		if (this.introduction == null || this.skipping)
		{
			return;
		}
		if ((double)this.fade < 0.5)
		{
			this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, this.fade * 2f);
			this.loading.GetComponent<Image>().color = new Color(1f, 1f, 1f, this.fade * 2f);
			this.fade += Time.deltaTime;
		}
		else if (!this.Portrait.gameObject.activeSelf && this.Background.color.a != 1f)
		{
			this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, 1f);
			this.loading.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			base.transform.Find("Loading Text").gameObject.SetActive(true);
		}
		else if (this.fade < 1f && this.Portrait.gameObject.activeSelf)
		{
			if (this.Background.color.a != 1f)
			{
				this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, 1f);
			}
			this.TextBoxBackground.color = new Color(1f, 1f, 1f, this.fade * 2f - 1f);
			this.Portrait.color = new Color(1f, 1f, 1f, this.fade * 2f - 1f);
			this.fade += Time.deltaTime;
		}
		else if (this.fade < 1.5f && this.Portrait.gameObject.activeSelf)
		{
			float num = Mathf.Min(1f, (this.fade - 1f) * 2f);
			float r = 0.18f * num + 0.87058824f * (1f - num);
			float g = 0.18f * num + 0.8627451f * (1f - num);
			float b = 0.18f * num + 0.89411765f * (1f - num);
			this.TextBox.color = new Color(r, g, b, 1f);
			this.fade += Time.deltaTime;
		}
		else if (this.fade >= 1.5f && this.fade < 100f && this.Portrait.gameObject.activeSelf)
		{
			if (this.Background.color.a != 1f)
			{
				this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, 1f);
			}
			if (this.TextBoxBackground.color.a != 1f)
			{
				this.TextBoxBackground.color = new Color(1f, 1f, 1f, 1f);
			}
			if (this.TextBox.color.a != 1f)
			{
				this.TextBox.color = new Color(0.18f, 0.18f, 0.18f, 1f);
			}
			if (this.Portrait.color.a != 1f)
			{
				this.Portrait.color = new Color(1f, 1f, 1f, 1f);
			}
			if (!this.NextButton.gameObject.activeSelf)
			{
				for (int i = 0; i < base.transform.childCount; i++)
				{
					Transform child = base.transform.GetChild(i);
					if (!(child.name == "Skip Button"))
					{
						if (!child.name.StartsWith("Loading"))
						{
							if (!child.name.StartsWith("Options"))
							{
								if (child != this.Mask.transform && child != this.Crush.transform)
								{
									child.gameObject.SetActive(true);
								}
							}
						}
					}
				}
				this.SkipButton.gameObject.SetActive(this.introduction.Girl.GirlName != Balance.GirlName.Cassie || IntroScreen.TutorialState > IntroScreen.State.GirlsActive);
				if (this.introduction is EndingCutScene)
				{
					this.SkipButton.gameObject.SetActive(false);
				}
			}
			this.introduction.Update(this);
			this.fade = 110f;
		}
		else
		{
			this.introduction.Update(this);
		}
		if (!this.Portrait.gameObject.activeSelf)
		{
			if (this.loadTimeout == 0f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
			}
			else if (this.loadTimeout < 1f && (double)this.loadTimeout > 0.5 && this.loadTimeout + Time.deltaTime < 1f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, (this.loadTimeout - 0.5f) * 2f);
			}
			else if (this.loadTimeout < 1f && this.loadTimeout + Time.deltaTime >= 1f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().color = Color.white;
			}
			else if (this.loadTimeout < 3f && this.loadTimeout + Time.deltaTime >= 3f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Taking longer than usual...";
			}
			else if (this.loadTimeout < 8f && this.loadTimeout + Time.deltaTime >= 8f && !(this.introduction is EndingCutScene))
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "You can skip if you'd like...";
				this.SkipButton.gameObject.SetActive(true);
			}
			else if (this.loadTimeout < 15f && this.loadTimeout + Time.deltaTime >= 15f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Pretty sure something bad happened...";
			}
			else if (this.loadTimeout < 25f && this.loadTimeout + Time.deltaTime >= 25f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Do you have slow internet?";
			}
			else if (this.loadTimeout < 35f && this.loadTimeout + Time.deltaTime >= 35f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Maybe our server is broken :(";
			}
			else if (this.loadTimeout < 45f && this.loadTimeout + Time.deltaTime >= 45f && !(this.introduction is EndingCutScene))
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Try skipping with the button above";
			}
			else if (this.loadTimeout < 60f && this.loadTimeout + Time.deltaTime >= 60f)
			{
				base.transform.Find("Loading Text").GetComponent<Text>().text = "Sorry about that...";
			}
			this.loadTimeout += Time.deltaTime;
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x000393D0 File Offset: 0x000375D0
	public Introduction GetIntroduction()
	{
		return this.introduction;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x000393D8 File Offset: 0x000375D8
	public void Initialize(Introduction intro)
	{
		base.gameObject.SetActive(true);
		this.skipping = false;
		this.fade = 0f;
		this.loadTimeout = 0f;
		this.introduction = intro;
		if (intro == null)
		{
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
		this.TextBox.gameObject.SetActive(true);
		this.TextBoxBackground.gameObject.SetActive(true);
		this.Background.gameObject.SetActive(true);
		this.Portrait.rectTransform.localPosition = new Vector3(0f, 0f);
		this.Portrait.rectTransform.sizeDelta = new Vector2(776f, 360f);
		if (this.Portrait.GetComponent<Button>() != null)
		{
			this.Portrait.GetComponent<Button>().interactable = true;
		}
		this.Portrait.gameObject.SetActive(false);
		this.TextBox.color = new Color(1f, 1f, 1f, 0f);
		this.TextBoxBackground.color = new Color(1f, 1f, 1f, 0f);
		this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, 0f);
		this.Portrait.color = new Color(1f, 1f, 1f, 0f);
		this.Crush.color = new Color(1f, 1f, 1f, 1f);
		if (this.loading == null)
		{
			this.loading = base.transform.Find("Loading");
		}
		this.loading.gameObject.SetActive(true);
		base.transform.Find("Loading Text").gameObject.SetActive(false);
		this.loading.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
		base.StartCoroutine(this.LoadAssets());
		if (intro is EndingCutScene)
		{
			this.SkipButton.transform.localPosition = new Vector3(0f, 1000f, 0f);
		}
		else
		{
			this.SkipButton.transform.localPosition = new Vector3(0f, 271f, 0f);
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000396B8 File Offset: 0x000378B8
	public void InitializeWithoutReset(Introduction intro)
	{
		base.gameObject.SetActive(true);
		this.skipping = false;
		this.fade = 0.5f;
		this.loadTimeout = 0f;
		this.introduction = intro;
		if (intro == null)
		{
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (!(base.transform.GetChild(i).name == "Mask"))
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		this.TextBox.gameObject.SetActive(true);
		this.TextBoxBackground.gameObject.SetActive(true);
		this.Background.gameObject.SetActive(true);
		this.SkipButton.gameObject.SetActive(false);
		this.Portrait.rectTransform.localPosition = new Vector3(0f, 0f);
		this.Portrait.rectTransform.sizeDelta = new Vector2(776f, 360f);
		this.Portrait.gameObject.SetActive(false);
		this.TextBox.color = new Color(1f, 1f, 1f, 0f);
		this.TextBoxBackground.color = new Color(1f, 1f, 1f, 0f);
		this.Background.color = new Color(this.introduction.BackgroundColor.r, this.introduction.BackgroundColor.g, this.introduction.BackgroundColor.b, 1f);
		this.Portrait.color = new Color(1f, 1f, 1f, 0f);
		this.Crush.color = new Color(1f, 1f, 1f, 1f);
		if (this.loading == null)
		{
			this.loading = base.transform.Find("Loading");
		}
		this.loading.gameObject.SetActive(true);
		base.transform.Find("Loading Text").gameObject.SetActive(true);
		this.loading.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.StartCoroutine(this.LoadAssets());
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00039950 File Offset: 0x00037B50
	public IEnumerator LoadIntroSpritesAsync(string assetBundle, string[] names)
	{
		if (this.loadedBundles.Count > 0)
		{
			foreach (AssetBundle bundle in this.loadedBundles)
			{
				GameState.AssetManager.UnloadBundle(bundle, true);
			}
			this.loadedBundles.Clear();
		}
		assetBundle = assetBundle.Replace("_mobile", string.Empty);
		this.Sprites = new Sprite[names.Length];
		if (names.Length > 0 && names[0] == "eventCGs00_monsters")
		{
			AssetBundleAsync introBundleRequest = GameState.AssetManager.GetBundleAsync("universe/intro", false);
			yield return introBundleRequest;
			if (introBundleRequest.AssetBundle != null)
			{
				this.loadedBundles.Add(introBundleRequest.AssetBundle);
			}
		}
		if (GameState.NSFW && !assetBundle.Contains("nsfw"))
		{
			AssetBundleAsync nsfwBundleRequest = GameState.AssetManager.GetBundleAsync(assetBundle + "_nsfw", false);
			yield return nsfwBundleRequest;
			if (nsfwBundleRequest.AssetBundle != null)
			{
				this.loadedBundles.Add(nsfwBundleRequest.AssetBundle);
			}
		}
		AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync(assetBundle, false);
		yield return bundleRequest;
		if (bundleRequest.AssetBundle == null)
		{
			yield break;
		}
		this.loadedBundles.Add(bundleRequest.AssetBundle);
		foreach (AssetBundle bundle2 in this.loadedBundles)
		{
			for (int i = 0; i < names.Length; i++)
			{
				if (!string.IsNullOrEmpty(names[i]))
				{
					if (!(this.Sprites[i] != null))
					{
						AssetBundleRequest assetRequest = bundle2.LoadAssetAsync<Sprite>(names[i]);
						yield return assetRequest;
						if (!(assetRequest.asset == null))
						{
							this.Sprites[i] = (assetRequest.asset as Sprite);
						}
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00039988 File Offset: 0x00037B88
	private IEnumerator LoadAssets()
	{
		base.transform.Find("Loading Text").GetComponent<Text>().text = "Loading...";
		this.loading.gameObject.SetActive(true);
		if (this.introduction.Girl.GirlName == Balance.GirlName.Karma || this.introduction.Girl.GirlName == Balance.GirlName.Sutra)
		{
			foreach (KeyValuePair<short, GirlModel> g in Universe.Girls)
			{
				if (g.Value.Name.ToLowerInvariant() == "karma" || g.Value.Name.ToLowerInvariant() == "sutra")
				{
					Album.Add(g.Value, 0);
				}
			}
		}
		else
		{
			Album.Add(this.introduction.Girl.Data, 0);
		}
		yield return this.introduction.Initialize(this, null);
		this.loading.gameObject.SetActive(false);
		base.transform.Find("Loading Text").gameObject.SetActive(false);
		if (this.Mask != null)
		{
			this.Mask.gameObject.SetActive(true);
		}
		this.Portrait.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x000399A4 File Offset: 0x00037BA4
	public void OnClick()
	{
		if (this.introduction != null)
		{
			this.introduction.OnClick(this);
		}
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x000399C0 File Offset: 0x00037BC0
	public void Skip()
	{
		base.StopAllCoroutines();
		this.loading.gameObject.SetActive(false);
		base.transform.Find("Loading Text").gameObject.SetActive(false);
		this.skipping = true;
		if (this.introduction is EndingCutScene)
		{
			EndingCutScene endingCutScene = (EndingCutScene)this.introduction;
			if (endingCutScene.GetCutSceneType() == EndingCutScene.CutSceneType.SacrificeYourself)
			{
				GameState.CurrentState.transform.Find("Popups/Credits").GetComponent<CreditsMobile>().Init(true);
			}
			else if (endingCutScene.GetCutSceneType() == EndingCutScene.CutSceneType.SacrificeQpiddy)
			{
				GameState.CurrentState.transform.Find("Popups/Credits").GetComponent<CreditsMobile>().Init(false);
			}
		}
		this.fade = 0f;
		this.TextBox.gameObject.SetActive(true);
		if (this.introduction != null)
		{
			this.introduction.Girl.SetIcon();
		}
		if (this.introduction is GenericIntroduction)
		{
			((GenericIntroduction)this.introduction).OnSceneDone();
		}
		this.Portrait.GetComponent<Button>().onClick.RemoveAllListeners();
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00039AF4 File Offset: 0x00037CF4
	public void LaunchOutro()
	{
		GameState.GetGirlScreen().pendingOutro = false;
		Girl girl = Girl.FindGirl(Balance.GirlName.QPiddy);
		EndingCutScene endingCutScene = new EndingCutScene(girl, ((QPiddyModel)girl.Data).OutroIntro, EndingCutScene.CutSceneType.Outro);
		endingCutScene.SetOnIntroductionDoneCallback(new Action(this.HandleOutroPreSacrifice));
		this.Initialize(endingCutScene);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00039B48 File Offset: 0x00037D48
	public void HandleOutroPreSacrifice()
	{
		Transform outro = GameState.CurrentState.transform.Find("Popups/Final Outro");
		if (outro != null)
		{
			outro.gameObject.SetActive(true);
			Transform prestigePopup = outro.Find("Prestige Popup");
			Button component = prestigePopup.Find("Dialog/OK Button").GetComponent<Button>();
			QPiddyModel qPiddy = Girl.FindGirl(Balance.GirlName.QPiddy).Data as QPiddyModel;
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(delegate()
			{
				outro.gameObject.SetActive(false);
			});
			component.onClick.AddListener(delegate()
			{
				this.InitializeWithoutReset(new EndingCutScene(Girl.FindGirl(Balance.GirlName.QPiddy), qPiddy.SacrificeYourselfIntro, EndingCutScene.CutSceneType.SacrificeYourself));
			});
			outro.Find("Left Button").GetComponent<Button>().onClick.RemoveAllListeners();
			outro.Find("Left Button").GetComponent<Button>().onClick.AddListener(delegate()
			{
				prestigePopup.gameObject.SetActive(true);
			});
			outro.Find("Right Button").GetComponent<Button>().onClick.RemoveAllListeners();
			outro.Find("Right Button").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.InitializeWithoutReset(new EndingCutScene(Girl.FindGirl(Balance.GirlName.QPiddy), qPiddy.SacrificeQPiddyIntro, EndingCutScene.CutSceneType.SacrificeQpiddy));
			});
			outro.Find("Right Button").GetComponent<Button>().onClick.AddListener(delegate()
			{
				outro.gameObject.SetActive(false);
			});
		}
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00039CF0 File Offset: 0x00037EF0
	public void CheckShowLocalizationDisclaimer()
	{
		if (Settings.NextLanguage != Translations.Language.English)
		{
			string disclaimer = Translations.GetDisclaimer(Settings.NextLanguage);
			Transform transform = GameState.CurrentState.transform.Find("Popups/Disclaimer Popup");
			transform.gameObject.SetActive(true);
			transform.transform.Find("Dialog/Disclaimer").GetComponent<Text>().text = disclaimer;
		}
		else
		{
			this.UpdateTranslation();
		}
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00039D5C File Offset: 0x00037F5C
	public void UpdateTranslation()
	{
	}

	// Token: 0x040006A2 RID: 1698
	public static readonly string IntroBundle = "intro_art";

	// Token: 0x040006A3 RID: 1699
	public static readonly int Shake = 20;

	// Token: 0x040006A4 RID: 1700
	public static readonly int YOffset;

	// Token: 0x040006A5 RID: 1701
	public Image Background;

	// Token: 0x040006A6 RID: 1702
	public Image Crush;

	// Token: 0x040006A7 RID: 1703
	public Image Portrait;

	// Token: 0x040006A8 RID: 1704
	public Image Mask;

	// Token: 0x040006A9 RID: 1705
	public Text NextButtonText;

	// Token: 0x040006AA RID: 1706
	public Button NextButton;

	// Token: 0x040006AB RID: 1707
	public Text TextBox;

	// Token: 0x040006AC RID: 1708
	public Image TextBoxBackground;

	// Token: 0x040006AD RID: 1709
	public Button SkipButton;

	// Token: 0x040006AE RID: 1710
	public GameObject LanguageBar;

	// Token: 0x040006AF RID: 1711
	internal Sprite[] Sprites;

	// Token: 0x040006B0 RID: 1712
	private float fade;

	// Token: 0x040006B1 RID: 1713
	public bool skipping;

	// Token: 0x040006B2 RID: 1714
	private Transform loading;

	// Token: 0x040006B3 RID: 1715
	private float loadTimeout;

	// Token: 0x040006B4 RID: 1716
	private Introduction introduction;

	// Token: 0x040006B5 RID: 1717
	private List<AssetBundle> loadedBundles = new List<AssetBundle>();
}
