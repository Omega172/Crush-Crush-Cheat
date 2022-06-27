using System;
using System.IO;
using BlayFap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200012A RID: 298
public class TitleScreen : MonoBehaviour
{
	// Token: 0x060007C0 RID: 1984 RVA: 0x00045F60 File Offset: 0x00044160
	private void Start()
	{
		TitleScreen.timer = 0f;
		this.CheckPrereg();
		Playfab.InventoryObjects = (Requirement.OutfitType)global::PlayerPrefs.GetInt("PlayfabInventory");
		base.transform.Find("Crush Crush Title/NSFW").gameObject.SetActive(GameState.NSFW);
		base.transform.Find("Crush Crush Title/SFW").gameObject.SetActive(!GameState.NSFW);
		if (GameObject.Find("Main Camera").transform.Find("Audio Source") != null)
		{
			bool flag = global::PlayerPrefs.GetInt("SettingsSoundMute", 0) == 1;
			try
			{
				if (!flag)
				{
					float @float = global::PlayerPrefs.GetFloat("SettingsEffects", 1f);
					GameObject.Find("Main Camera").transform.Find("Audio Source").GetComponent<AudioSource>().volume = Mathf.Min(0.5f, @float);
					GameObject.Find("Main Camera").transform.Find("Audio Source").GetComponent<AudioSource>().Play();
				}
			}
			catch (Exception)
			{
			}
		}
		for (int i = 0; i < this.Sparkles.Length; i++)
		{
			this.Sparkles[i].SetActive(UnityEngine.Random.value > 0.5f);
		}
		Playfab.InventoryObjects = (Requirement.OutfitType)global::PlayerPrefs.GetInt("PlayfabInventory");
		Playfab.AwardedItems = (Playfab.PlayfabItems)global::PlayerPrefs.GetLong("PlayfabAwardedItems", 0L);
		Playfab.FlingPurchases = (Playfab.PhoneFlingPurchases)global::PlayerPrefs.GetLong("PlayfabFlingPurchases", 0L);
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x000460F4 File Offset: 0x000442F4
	private void CheckPrereg()
	{
		if (!global::PlayerPrefs.HasKey("Prereg") && !global::PlayerPrefs.HasKey("GameStateDate"))
		{
			string path = Application.dataPath + "/../SavesDir/crushcrush.sav";
			try
			{
				string text = SteamManager.Instance.ReadFromCloudSave();
				if (string.IsNullOrEmpty(text))
				{
					text = File.ReadAllText(path);
				}
				if (!string.IsNullOrEmpty(text))
				{
					global::PlayerPrefs.Import(global::PlayerPrefs.Import(text), false);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(string.Format("There was an error trying to load .sav file at title screen: {0}", ex.Message));
				Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "TitleScreen.checkPrereg: " + ex.Message);
			}
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000461B8 File Offset: 0x000443B8
	private void Update()
	{
		this.currentTime += Time.deltaTime;
		if ((double)this.currentTime > 0.1)
		{
			for (int i = 0; i < this.Sparkles.Length; i++)
			{
				if (UnityEngine.Random.value > 0.92f)
				{
					this.Sparkles[i].SetActive(!this.Sparkles[i].activeSelf);
				}
			}
			this.currentTime = 0f;
		}
		TitleScreen.timer += Time.deltaTime;
		TitleScreen.NetworkTimeout += Time.deltaTime;
		if (TitleScreen.timer > 3f)
		{
			this.SadPandaLogo.SetActive(false);
		}
		GameObject gameObject = GameObject.Find("Steam");
		if (gameObject == null)
		{
			gameObject = new GameObject("Steam");
		}
		if (gameObject.GetComponent<SteamManager>() == null)
		{
			SteamManager.InitializeSteam(new Action<string>(this.OnSteamError));
		}
		if (TitleScreen.timer > 5f && BlayFapClient.LoggedIn)
		{
			SceneManager.LoadScene("CrushCrushProto");
		}
		else if (TitleScreen.timer > 5f)
		{
			base.transform.Find("Logging In").gameObject.SetActive(true);
		}
		if (TitleScreen.timer > 10f && this.errorDialog == null)
		{
			this.OnSteamError("Timed out while trying to sign in.");
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00046338 File Offset: 0x00044538
	public void LoadSceneAnyways()
	{
		SceneManager.LoadScene("CrushCrushProto");
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00046344 File Offset: 0x00044544
	private void OnSteamError(string error)
	{
		this.errorDialog = base.transform.Find("Error");
		this.errorDialog.Find("Dialog/Error").GetComponent<Text>().text = error;
		this.errorDialog.gameObject.SetActive(true);
	}

	// Token: 0x0400083D RID: 2109
	private const float networkMaximumWait = 10f;

	// Token: 0x0400083E RID: 2110
	private const string MainScene = "CrushCrushProto";

	// Token: 0x0400083F RID: 2111
	public GameObject SadPandaLogo;

	// Token: 0x04000840 RID: 2112
	public GameObject CrushCrushTitle;

	// Token: 0x04000841 RID: 2113
	public GameObject[] Sparkles;

	// Token: 0x04000842 RID: 2114
	public static float NetworkTimeout;

	// Token: 0x04000843 RID: 2115
	private static float timer;

	// Token: 0x04000844 RID: 2116
	public GameObject LoginFailedDialog;

	// Token: 0x04000845 RID: 2117
	private float currentTime;

	// Token: 0x04000846 RID: 2118
	private Transform errorDialog;
}
