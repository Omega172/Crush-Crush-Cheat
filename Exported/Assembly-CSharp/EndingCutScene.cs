using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using Spine.Unity;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class EndingCutScene : GenericIntroduction
{
	// Token: 0x06000008 RID: 8 RVA: 0x00002708 File Offset: 0x00000908
	public EndingCutScene(Girl girl, List<GirlModel.IntroData> introDataList, EndingCutScene.CutSceneType cutSceneType) : base(girl, introDataList)
	{
		this._cutSceneType = cutSceneType;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000271C File Offset: 0x0000091C
	public EndingCutScene.CutSceneType GetCutSceneType()
	{
		return this._cutSceneType;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002724 File Offset: 0x00000924
	protected override void Init(Girl girl, List<GirlModel.IntroData> introDataList)
	{
		base.Init(girl, introDataList);
		this._backgroundColor = new Color(0.11764706f, 0.05882353f, 0.2901961f, 1f);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002750 File Offset: 0x00000950
	protected override string GetAssetBundleName(string name)
	{
		return "universe/outro";
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002758 File Offset: 0x00000958
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		if (this._cutSceneType != EndingCutScene.CutSceneType.Outro)
		{
			GameState.Voiceover.AddPlaybackBlocker(provider.gameObject);
		}
		if (this._cutSceneType == EndingCutScene.CutSceneType.Outro)
		{
			this._setAudio = false;
		}
		else if (this._cutSceneType == EndingCutScene.CutSceneType.SacrificeQpiddy)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Unlock, "SacrificeQPiddy");
			Achievements.ForceAchievement(473);
			if (Girl.FindGirl(Balance.GirlName.QPiddy) != null)
			{
				Girl.FindGirl(Balance.GirlName.QPiddy).Love = 9;
				Achievements.TriggerLoveAchievement(Balance.GirlName.QPiddy);
				Girl.FindGirl(Balance.GirlName.QPiddy).StoreState();
				GameState.CurrentState.QueueSave();
			}
			GameState.GetGirlScreen().SetGirl(Girl.FindGirl(Balance.GirlName.Cassie));
		}
		AssetBundleAsync outroRequest = GameState.AssetManager.GetBundleAsync(this.GetAssetBundleName(string.Empty), true);
		yield return outroRequest;
		if (outroRequest.AssetBundle != null)
		{
			this.LoadSpineAnimations(provider, outroRequest.AssetBundle);
			GameState.AssetManager.UnloadBundle(outroRequest.AssetBundle);
		}
		yield return base.Initialize(provider, albumImage);
		yield break;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002790 File Offset: 0x00000990
	private void LoadSpineAnimations(IntroProvider provider, AssetBundle bundle)
	{
		if (this._cutSceneType == EndingCutScene.CutSceneType.Outro)
		{
			this.InitSpineAnimation(provider, bundle, "Mask/End Scene 3", "end scene 3", delegate
			{
				provider.transform.Find("Mask/End Scene 3").gameObject.SetActive(true);
			});
		}
		if (this._cutSceneType == EndingCutScene.CutSceneType.SacrificeQpiddy || this._cutSceneType == EndingCutScene.CutSceneType.SacrificeYourself)
		{
			this.InitSpineAnimation(provider, bundle, "Mask/End Scene 1", "end scene 1", null);
		}
		if (this._cutSceneType == EndingCutScene.CutSceneType.SacrificeQpiddy)
		{
			this.InitSpineAnimation(provider, bundle, "Mask/End Scene 2", "end scene 2", delegate
			{
				provider.transform.Find("Mask/End Scene 2").gameObject.SetActive(true);
			});
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002838 File Offset: 0x00000A38
	private void InitSpineAnimation(IntroProvider provider, AssetBundle bundle, string anchor, string assetName, Action onSuccess)
	{
		if (bundle == null)
		{
			return;
		}
		RectTransform component = provider.transform.Find(anchor).GetComponent<RectTransform>();
		SkeletonGraphic component2 = component.GetComponent<SkeletonGraphic>();
		TextAsset textAsset = null;
		TextAsset textAsset2 = null;
		Texture texture = null;
		string[] allAssetNames = bundle.GetAllAssetNames();
		for (int i = 0; i < allAssetNames.Length; i++)
		{
			if (allAssetNames[i].Contains(assetName))
			{
				if (allAssetNames[i].EndsWith("json"))
				{
					if (textAsset == null)
					{
						textAsset = bundle.LoadAsset<TextAsset>(allAssetNames[i]);
					}
				}
				else if (allAssetNames[i].EndsWith("txt"))
				{
					if (textAsset2 == null)
					{
						textAsset2 = bundle.LoadAsset<TextAsset>(allAssetNames[i]);
					}
				}
				else if (allAssetNames[i].EndsWith("png") && texture == null)
				{
					texture = bundle.LoadAsset<Texture>(allAssetNames[i]);
				}
			}
		}
		if (textAsset == null || textAsset2 == null || texture == null)
		{
			Debug.LogError("Could not load needed assets for spine animation");
			return;
		}
		SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
		skeletonDataAsset.skeletonJSON = textAsset;
		skeletonDataAsset.scale = 0.01f;
		SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
		component2.material = new Material(component2.material);
		component2.material.SetTexture("_MainTex", texture);
		spineAtlasAsset.atlasFile = textAsset2;
		spineAtlasAsset.materials = new Material[]
		{
			component2.material
		};
		skeletonDataAsset.atlasAssets = new SpineAtlasAsset[]
		{
			spineAtlasAsset
		};
		component2.skeletonDataAsset = skeletonDataAsset;
		component2.startingAnimation = "animation";
		component2.startingLoop = true;
		component2.Initialize(true);
		if (onSuccess != null)
		{
			onSuccess();
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002A10 File Offset: 0x00000C10
	public override void Update(IntroProvider provider)
	{
		if (this._cutSceneType == EndingCutScene.CutSceneType.Outro)
		{
			if (this._currentTime < 1f)
			{
				GameState.CurrentState.GetComponent<Audio>().Music.volume = Settings.MusicVolume * (1f - this._currentTime);
				this._currentTime += Time.deltaTime;
			}
			else if (!this._setAudio)
			{
				GameState.CurrentState.GetComponent<Audio>().Music.clip = GameState.CurrentState.GetComponent<Audio>().HowlingWind;
				GameState.CurrentState.GetComponent<Audio>().Music.volume = ((!GameState.Voiceover.IsVoiceEnabled) ? Mathf.Max(Settings.EffectsVolume, Settings.MusicVolume) : (Settings.VoiceVolume / 3f));
				if (Settings.EffectsVolume != 0f || Settings.MusicVolume != 0f)
				{
					GameState.CurrentState.GetComponent<Audio>().Music.Play();
				}
				this._setAudio = true;
			}
		}
		base.Update(provider);
		if (this._state > 3 && this._cutSceneType != EndingCutScene.CutSceneType.Outro)
		{
			float num = Mathf.Max(0f, 1f - this._currentTime);
			float r = this.BackgroundColor.r * num + (1f - num) * 28f / 255f;
			float g = this.BackgroundColor.g * num + (1f - num) * 28f / 255f;
			float b = this.BackgroundColor.b * num + (1f - num) * 28f / 255f;
			provider.Background.color = new Color(r, g, b);
			provider.Portrait.color = new Color(1f, 1f, 1f, Mathf.Max(0f, 1f - this._currentTime));
			if (this._currentTime < 1f)
			{
				this._currentTime += Time.deltaTime;
			}
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002C30 File Offset: 0x00000E30
	public override void OnClick(IntroProvider provider)
	{
		this.VoiceoverDelay = ((this._state != 0) ? 0.2f : 1.2f);
		base.OnClick(provider);
		if (this._cutSceneType != EndingCutScene.CutSceneType.Outro && this._state == 4)
		{
			this._currentTime = 0f;
		}
		if (this._cutSceneType == EndingCutScene.CutSceneType.Outro && this._state > 1)
		{
			provider.transform.Find("Mask/End Scene 3").gameObject.SetActive(false);
		}
		if (this._cutSceneType == EndingCutScene.CutSceneType.SacrificeQpiddy && this._state > 1)
		{
			provider.transform.Find("Mask/End Scene 2").gameObject.SetActive(false);
		}
		if (this._cutSceneType != EndingCutScene.CutSceneType.Outro)
		{
			provider.transform.Find("Mask/End Scene 1").gameObject.SetActive(this._state == 2);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002D1C File Offset: 0x00000F1C
	public override void Destroy(IntroProvider provider)
	{
		if (this._cutSceneType == EndingCutScene.CutSceneType.SacrificeYourself || this._cutSceneType == EndingCutScene.CutSceneType.SacrificeQpiddy)
		{
			GameState.Voiceover.RemovePlaybackBlocker(provider.gameObject, true);
			GameState.CurrentState.GetComponent<Audio>().Music.clip = ((!(DateTime.UtcNow < new DateTime(2021, 1, 1))) ? GameState.CurrentState.GetComponent<Audio>().Music1 : GameState.CurrentState.GetComponent<Audio>().HolidayMusic);
			GameState.CurrentState.GetComponent<Audio>().Music.volume = Settings.MusicVolume;
			if (Settings.MusicVolume > 0f)
			{
				GameState.CurrentState.GetComponent<Audio>().Music.Play();
			}
			else
			{
				GameState.CurrentState.GetComponent<Audio>().Music.Stop();
			}
			provider.transform.Find("Mask/End Scene 3").gameObject.SetActive(false);
			provider.transform.Find("Mask/End Scene 2").gameObject.SetActive(false);
			provider.transform.Find("Mask/End Scene 1").gameObject.SetActive(false);
		}
		base.Destroy(provider);
	}

	// Token: 0x04000003 RID: 3
	private EndingCutScene.CutSceneType _cutSceneType;

	// Token: 0x04000004 RID: 4
	private bool _setAudio;

	// Token: 0x02000004 RID: 4
	public enum CutSceneType
	{
		// Token: 0x04000006 RID: 6
		Outro,
		// Token: 0x04000007 RID: 7
		SacrificeYourself,
		// Token: 0x04000008 RID: 8
		SacrificeQpiddy
	}
}
