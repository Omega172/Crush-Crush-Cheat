using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000125 RID: 293
public class Settings : MonoBehaviour
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0003D7B8 File Offset: 0x0003B9B8
	public static void LoadState()
	{
		Settings.HandleLegacyPrefs();
		Settings.ParticlesDisabled = (global::PlayerPrefs.GetInt("SettingsParticlesOff", 0) == 1);
		Settings.MusicVolume = global::PlayerPrefs.GetFloat("SettingsMusic", 0.1f);
		Settings.EffectsVolume = global::PlayerPrefs.GetFloat("SettingsEffects", 1f);
		Settings.VoiceVolume = global::PlayerPrefs.GetFloat("SettingsVoice", 1f);
		bool flag = global::PlayerPrefs.GetInt("SettingsMusicMute", 0) == 1;
		bool flag2 = global::PlayerPrefs.GetInt("SettingsSoundMute", 0) == 1;
		Settings.IntrosDisabled = (global::PlayerPrefs.GetInt("SettingsIntrosOff", 0) == 1);
		Settings.PopupsDisabled = (global::PlayerPrefs.GetInt("SettingsPopupsOff", 0) == 1);
		Settings.DisableCloudBackup = (global::PlayerPrefs.GetInt("SettingsDisableCloud", 0) == 1);
		Translations.PreferredLanguage = (Translations.Language)global::PlayerPrefs.GetInt("SettingsLanguage", 0);
		Settings.NextLanguage = Translations.PreferredLanguage;
		if (flag)
		{
			Settings.MusicVolume = 0f;
		}
		if (flag2)
		{
			Settings.EffectsVolume = 0f;
		}
		if (flag || flag2)
		{
			Settings.SaveState();
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0003D8BC File Offset: 0x0003BABC
	public static void SaveState()
	{
		global::PlayerPrefs.SetInt("SettingsParticlesOff", (!Settings.ParticlesDisabled) ? 0 : 1);
		global::PlayerPrefs.SetFloat("SettingsMusic", Settings.MusicVolume);
		global::PlayerPrefs.SetFloat("SettingsEffects", Settings.EffectsVolume);
		global::PlayerPrefs.SetFloat("SettingsVoice", Settings.VoiceVolume);
		global::PlayerPrefs.SetInt("SettingsSoundMute", 0);
		global::PlayerPrefs.SetInt("SettingsMusicMute", 0);
		global::PlayerPrefs.DeleteKey("SettingsSoundMute", false);
		global::PlayerPrefs.DeleteKey("SettingsMusicMute", false);
		global::PlayerPrefs.SetInt("SettingsIntrosOff", (!Settings.IntrosDisabled) ? 0 : 1);
		global::PlayerPrefs.SetInt("SettingsPopupsOff", (!Settings.PopupsDisabled) ? 0 : 1);
		global::PlayerPrefs.SetInt("SettingsLanguage", (int)Translations.PreferredLanguage);
		global::PlayerPrefs.SetInt("GameStateNSFW", (!GameState.NSFW) ? 0 : 1);
		global::PlayerPrefs.SetInt("SettingsDisableCloud", (!Settings.DisableCloudBackup) ? 0 : 1);
		global::PlayerPrefs.Save();
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600072B RID: 1835 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
	// (set) Token: 0x0600072C RID: 1836 RVA: 0x0003D9C8 File Offset: 0x0003BBC8
	public static bool ParticlesDisabled { get; private set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x0003D9D0 File Offset: 0x0003BBD0
	// (set) Token: 0x0600072E RID: 1838 RVA: 0x0003D9D8 File Offset: 0x0003BBD8
	public static bool IntrosDisabled { get; private set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x0003D9E0 File Offset: 0x0003BBE0
	// (set) Token: 0x06000730 RID: 1840 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
	public static bool PopupsDisabled { get; private set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x0003D9F0 File Offset: 0x0003BBF0
	// (set) Token: 0x06000732 RID: 1842 RVA: 0x0003D9F8 File Offset: 0x0003BBF8
	public static bool NotificationsDisabled { get; private set; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000733 RID: 1843 RVA: 0x0003DA00 File Offset: 0x0003BC00
	// (set) Token: 0x06000734 RID: 1844 RVA: 0x0003DA08 File Offset: 0x0003BC08
	public static bool VibrationDisabled { get; private set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000735 RID: 1845 RVA: 0x0003DA10 File Offset: 0x0003BC10
	// (set) Token: 0x06000736 RID: 1846 RVA: 0x0003DA18 File Offset: 0x0003BC18
	public static float MusicVolume { get; private set; }

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x0003DA20 File Offset: 0x0003BC20
	// (set) Token: 0x06000738 RID: 1848 RVA: 0x0003DA28 File Offset: 0x0003BC28
	public static float EffectsVolume { get; private set; }

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x0003DA30 File Offset: 0x0003BC30
	// (set) Token: 0x0600073A RID: 1850 RVA: 0x0003DA38 File Offset: 0x0003BC38
	public static float VoiceVolume { get; private set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x0003DA40 File Offset: 0x0003BC40
	// (set) Token: 0x0600073C RID: 1852 RVA: 0x0003DA48 File Offset: 0x0003BC48
	public static bool DisableCloudBackup { get; set; }

	// Token: 0x0600073D RID: 1853 RVA: 0x0003DA50 File Offset: 0x0003BC50
	private void Start()
	{
		Settings.LoadState();
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0003DA58 File Offset: 0x0003BC58
	private void OnEnable()
	{
		base.transform.Find("Dialog/Particle Checkbox").GetComponent<CheckBox>().Checked = !Settings.ParticlesDisabled;
		base.transform.Find("Dialog/Intros Checkbox").GetComponent<CheckBox>().Checked = Settings.IntrosDisabled;
		base.transform.Find("Dialog/Popup Checkbox").GetComponent<CheckBox>().Checked = Settings.PopupsDisabled;
		base.transform.Find("Dialog/Music Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.MusicVolume;
		base.transform.Find("Dialog/SFX Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.EffectsVolume;
		base.transform.Find("Dialog/Voice Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.VoiceVolume;
		base.transform.Find("Dialog/NSFW Checkbox").GetComponent<CheckBox>().Checked = GameState.NSFW;
		base.transform.Find("Dialog/Cloud Checkbox").GetComponent<CheckBox>().Checked = Settings.DisableCloudBackup;
		GameState.Voiceover.AddPlaybackBlocker(base.gameObject);
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0003DB70 File Offset: 0x0003BD70
	private void OnDisable()
	{
		GameState.Voiceover.RemovePlaybackBlocker(base.gameObject, false);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0003DB84 File Offset: 0x0003BD84
	public void Init()
	{
		Transform transform = base.transform.Find("Dialog/Language Selection/Languages");
		Text component = base.transform.Find("Dialog/Language Selection/Disclaimer").GetComponent<Text>();
		Settings.NextLanguage = Translations.PreferredLanguage;
		for (int i = 0; i < transform.childCount; i++)
		{
			LanguageSelector component2 = transform.GetChild(i).GetComponent<LanguageSelector>();
			if (component2 != null)
			{
				component2.transform.Find("Check").gameObject.SetActive(component2.Language == Settings.NextLanguage);
			}
		}
		component.text = Translations.GetDisclaimer(Settings.NextLanguage);
		base.transform.Find("Dialog/NSFW Checkbox").gameObject.SetActive(GameState.NSFWAllowed);
		base.transform.Find("Dialog/NSFW Checkbox").GetComponent<Button>().interactable = GameState.NSFWAllowed;
		base.transform.Find("NSFW Information").gameObject.SetActive(false);
		base.transform.Find("NSFW Information Dialog").gameObject.SetActive(false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0003DCAC File Offset: 0x0003BEAC
	public void OnOK()
	{
		Settings.ParticlesDisabled = !base.transform.Find("Dialog/Particle Checkbox").GetComponent<CheckBox>().Checked;
		Settings.IntrosDisabled = base.transform.Find("Dialog/Intros Checkbox").GetComponent<CheckBox>().Checked;
		Settings.PopupsDisabled = base.transform.Find("Dialog/Popup Checkbox").GetComponent<CheckBox>().Checked;
		Settings.MusicVolume = base.transform.Find("Dialog/Music Slider/Slider Bar Slider").GetComponent<global::Slider>().Value;
		Settings.EffectsVolume = base.transform.Find("Dialog/SFX Slider/Slider Bar Slider").GetComponent<global::Slider>().Value;
		Settings.VoiceVolume = base.transform.Find("Dialog/Voice Slider/Slider Bar Slider").GetComponent<global::Slider>().Value;
		if (GameState.Voiceover != null)
		{
			GameState.Voiceover.Init(true);
		}
		Settings.DisableCloudBackup = base.transform.Find("Dialog/Cloud Checkbox").GetComponent<CheckBox>().Checked;
		base.gameObject.SetActive(false);
		bool nsfw = GameState.NSFW;
		GameState.NSFW = (GameState.NSFWAllowed && base.transform.Find("Dialog/NSFW Checkbox").GetComponent<CheckBox>().Checked);
		if (nsfw != GameState.NSFW)
		{
			this.VerifyNSFWPatch();
		}
		if (Translations.PreferredLanguage != Settings.NextLanguage)
		{
			Translations.PreferredLanguage = Settings.NextLanguage;
			Translations.Init(null);
			Achievements component = GameObject.Find("Canvas").transform.Find("Achievements").GetComponent<Achievements>();
			component.UpdateAchievementText();
		}
		Settings.SaveState();
		GameState.CurrentState.GetComponent<Audio>().SetVolume();
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003DE64 File Offset: 0x0003C064
	private void VerifyNSFWPatch()
	{
		foreach (Girl girl in Girl.ActiveGirls)
		{
			if (girl.transform.Find("Unlocked").GetComponent<Button>().interactable)
			{
				girl.LoadAssets(false, false);
			}
		}
		if (Girls.CurrentGirl != null)
		{
			Girls.CurrentGirl.GetSprite(Girls.CurrentGirl.CurrentSpriteType, new Action<Sprite, Girl>(GameState.GetGirlScreen().SetSprite));
			GameState.GetGirlScreen().SetGirl(Girls.CurrentGirl);
		}
		GameState.CurrentState.transform.Find("Popups/Memory Album").gameObject.SetActive(false);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0003DF4C File Offset: 0x0003C14C
	public void OnCancel()
	{
		base.transform.Find("Dialog/Music Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.MusicVolume;
		base.transform.Find("Dialog/SFX Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.EffectsVolume;
		base.transform.Find("Dialog/Voice Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = Settings.VoiceVolume;
		GameState.CurrentState.GetComponent<Audio>().SetVolume();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0003DFD4 File Offset: 0x0003C1D4
	public void MuteMusic()
	{
		base.transform.Find("Dialog/Music Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = 0f;
		AudioSource music = GameObject.Find("Canvas").GetComponent<Audio>().Music;
		if (music.isPlaying)
		{
			music.Pause();
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0003E028 File Offset: 0x0003C228
	public void MuteSound()
	{
		base.transform.Find("Dialog/SFX Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = 0f;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0003E054 File Offset: 0x0003C254
	public void MuteVoice()
	{
		base.transform.Find("Dialog/Voice Slider/Slider Bar Slider").GetComponent<global::Slider>().Value = 0f;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0003E080 File Offset: 0x0003C280
	private static void HandleLegacyPrefs()
	{
	}

	// Token: 0x04000786 RID: 1926
	public static Translations.Language NextLanguage;
}
