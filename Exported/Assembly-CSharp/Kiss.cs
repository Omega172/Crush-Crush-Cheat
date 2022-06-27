using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000114 RID: 276
public class Kiss : MonoBehaviour
{
	// Token: 0x060006B6 RID: 1718 RVA: 0x00039D80 File Offset: 0x00037F80
	public void SetOnIntroductionDoneCallback(Action onDone)
	{
		this.OnDone = onDone;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00039D8C File Offset: 0x00037F8C
	public IEnumerator InitAsync(Girl girl, Girl.SpriteType type)
	{
		Girl.LoadImageAsyncRequest spriteRequest = new Girl.LoadImageAsyncRequest(girl, type);
		yield return spriteRequest.GetSpriteAsync();
		if (spriteRequest == null)
		{
			yield break;
		}
		this.loadedBundle = spriteRequest.Bundle;
		this.Init(spriteRequest.Sprite);
		yield break;
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00039DC4 File Offset: 0x00037FC4
	public void Init(Sprite kiss)
	{
		base.transform.Find("Dialog/UI Container/Remember Button").gameObject.SetActive(false);
		if (this.KissOverlay != null)
		{
			this.KissOverlay.gameObject.SetActive(false);
		}
		this.InitAlbum(Balance.GirlName.Cassie, kiss, 0, false, false, 0);
		if (kiss.name.Contains("explora"))
		{
			this._currentTime = 0f;
			this.KissImage.GetComponent<PixelationEffect>().SetWidth(560f);
			this.KissImage.GetComponent<PixelationEffect>().SetEffect(0f);
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00039E64 File Offset: 0x00038064
	public void Update()
	{
		if (this._currentTime < 5f)
		{
			this._currentTime += Time.deltaTime;
			if (this._currentTime >= 5f)
			{
				this.KissImage.GetComponent<PixelationEffect>().SetEffect(1f);
			}
			else
			{
				this.KissImage.GetComponent<PixelationEffect>().SetEffect(Mathf.Round(10f * this._currentTime) / 50f);
			}
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00039EE4 File Offset: 0x000380E4
	private void OnDisable()
	{
		this._currentTime = 5f;
		if (this.OnDone != null)
		{
			this.OnDone();
			this.OnDone = null;
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00039F1C File Offset: 0x0003811C
	private void InitPeanutPhonePinup(AssetBundle bundle, AssetBundle nsfwBundle, string anchor, string animationName, Action onSuccess)
	{
		if (bundle == null)
		{
			return;
		}
		RectTransform component = this.KissImage.transform.Find("Animated").GetComponent<RectTransform>();
		SkeletonGraphic component2 = component.GetComponent<SkeletonGraphic>();
		TextAsset textAsset = null;
		TextAsset textAsset2 = null;
		Texture texture = null;
		Sprite sprite = null;
		if (nsfwBundle != null)
		{
			string[] allAssetNames = nsfwBundle.GetAllAssetNames();
			Debug.Log("Found nsfw! " + allAssetNames[0]);
			for (int i = 0; i < allAssetNames.Length; i++)
			{
				if (allAssetNames[i].EndsWith("json"))
				{
					textAsset = nsfwBundle.LoadAsset<TextAsset>(allAssetNames[i]);
				}
				else if (allAssetNames[i].EndsWith("txt"))
				{
					textAsset2 = nsfwBundle.LoadAsset<TextAsset>(allAssetNames[i]);
				}
				else if (allAssetNames[i].EndsWith("png"))
				{
					if (allAssetNames[i].Contains("bkg") || allAssetNames[i].ToLowerInvariant().Contains("bg"))
					{
						sprite = nsfwBundle.LoadAsset<Sprite>(allAssetNames[i]);
					}
					else if (texture == null)
					{
						texture = nsfwBundle.LoadAsset<Texture>(allAssetNames[i]);
					}
				}
			}
		}
		string[] allAssetNames2 = bundle.GetAllAssetNames();
		for (int j = 0; j < allAssetNames2.Length; j++)
		{
			if (allAssetNames2[j].EndsWith("json"))
			{
				if (textAsset == null)
				{
					textAsset = bundle.LoadAsset<TextAsset>(allAssetNames2[j]);
				}
			}
			else if (allAssetNames2[j].EndsWith("txt"))
			{
				if (textAsset2 == null)
				{
					textAsset2 = bundle.LoadAsset<TextAsset>(allAssetNames2[j]);
				}
			}
			else if (allAssetNames2[j].EndsWith("png"))
			{
				if (allAssetNames2[j].Contains("bkg") || allAssetNames2[j].ToLowerInvariant().Contains("bg"))
				{
					sprite = bundle.LoadAsset<Sprite>(allAssetNames2[j]);
				}
				else if (texture == null)
				{
					texture = bundle.LoadAsset<Texture>(allAssetNames2[j]);
				}
			}
		}
		if (textAsset == null || textAsset2 == null || texture == null)
		{
			Debug.LogError("Could not load needed assets for animated girl pinup");
			return;
		}
		if (component2.skeletonDataAsset != null && component2.skeletonDataAsset.skeletonJSON.Equals(textAsset) && texture.name == component2.material.GetTexture("_MainTex").name)
		{
			component.gameObject.SetActive(true);
			if (sprite != null)
			{
				this.KissImage.sprite = sprite;
			}
			if (onSuccess != null)
			{
				onSuccess();
			}
			return;
		}
		SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
		skeletonDataAsset.skeletonJSON = textAsset;
		skeletonDataAsset.scale = 0.01f;
		SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
		if (this._albumSpineMaterial == null)
		{
			this._albumSpineMaterial = new Material(component2.material);
			component2.material = this._albumSpineMaterial;
		}
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
		component2.startingAnimation = animationName;
		component2.startingLoop = true;
		component2.Initialize(true);
		RectTransform component3 = component.Find(anchor).GetComponent<RectTransform>();
		component.anchoredPosition = component3.anchoredPosition;
		component.pivot = component3.pivot;
		component.localScale = component3.localScale;
		component.anchorMin = component3.anchorMin;
		component.anchorMax = component3.anchorMax;
		component.sizeDelta = component3.sizeDelta;
		component.localRotation = component3.localRotation;
		component.gameObject.SetActive(true);
		if (sprite != null)
		{
			this.KissImage.sprite = sprite;
		}
		if (onSuccess != null)
		{
			onSuccess();
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0003A348 File Offset: 0x00038548
	public void InitAlbum(Balance.GirlName girl, Sprite kiss, int imageType = 0, bool date = false, bool award = false, int image = 0)
	{
		this.KissImage.GetComponent<PixelationEffect>().SetEffect(1f);
		if (image != 24 && this.KissImage.transform.Find("Animated").GetComponent<PeanutLofi>() != null)
		{
			UnityEngine.Object.Destroy(this.KissImage.transform.Find("Animated").GetComponent<PeanutLofi>());
		}
		this.KissImage.transform.Find("Animated").gameObject.SetActive(false);
		this.KissImage.sprite = kiss;
		base.gameObject.SetActive(true);
		this.DestroyParticles();
		base.transform.Find("Dialog/Heart System").gameObject.SetActive(true);
		RectTransform component = base.transform.Find("Dialog/Border").GetComponent<RectTransform>();
		if (date)
		{
			base.transform.Find("Dialog").transform.localPosition = new Vector3(0f, 112f, 0f);
			if (this.KissUI != null)
			{
				this.KissUI.rectTransform.sizeDelta = new Vector2(776f, 270f);
			}
			component.offsetMax = new Vector2(3f, 7f);
			component.offsetMin = new Vector2(-3f, -10f);
		}
		else if (award)
		{
			base.transform.Find("Dialog").transform.localPosition = new Vector3(0f, 293f, 0f);
			if (this.KissUI != null)
			{
				this.KissUI.rectTransform.sizeDelta = new Vector2(784f, 586f);
			}
			component.offsetMax = new Vector2(3f, 7f);
			component.offsetMin = new Vector2(-3f, -10f);
		}
		else if (image >= 24 && image <= 31)
		{
			if (image == 24)
			{
				GameState.AssetManager.GetBundle("peanut/peanut_lofi_pinup", false, delegate(AssetBundle bundle)
				{
					this.InitPeanutPhonePinup(bundle, null, "PeanutStudying", "SFW", delegate
					{
						if (this.KissImage.transform.Find("Animated").GetComponent<PeanutLofi>() == null)
						{
							this.KissImage.transform.Find("Animated").gameObject.AddComponent<PeanutLofi>();
						}
					});
				});
			}
			else if (image == 25)
			{
				if (GameState.NSFW)
				{
					GameState.AssetManager.GetBundle("peanut/peanut_phone_pinup_nsfw", false, delegate(AssetBundle nsfwBundle)
					{
						GameState.AssetManager.GetBundle("peanut/peanut_phone_pinup", false, delegate(AssetBundle bundle)
						{
							this.InitPeanutPhonePinup(bundle, nsfwBundle, "PeanutPhone", "NSFW", null);
						});
					});
				}
				else
				{
					GameState.AssetManager.GetBundle("peanut/peanut_phone_pinup", false, delegate(AssetBundle bundle)
					{
						this.InitPeanutPhonePinup(bundle, null, "PeanutPhone", "SFW", null);
					});
				}
			}
			else if (image == 27)
			{
				if (GameState.NSFW)
				{
					GameState.AssetManager.GetBundle("mio/mio_6_anniversary_nsfw", false, delegate(AssetBundle nsfwBundle)
					{
						GameState.AssetManager.GetBundle("mio/mio_6_anniversary", false, delegate(AssetBundle bundle)
						{
							this.InitPeanutPhonePinup(bundle, nsfwBundle, "Mio6Anniversary", "MioCG03 NSFW", null);
						});
					});
				}
				else
				{
					GameState.AssetManager.GetBundle("mio/mio_6_anniversary", false, delegate(AssetBundle bundle)
					{
						this.InitPeanutPhonePinup(bundle, null, "Mio6Anniversary", "MioCG03 SFW", null);
					});
				}
			}
			else if (image == 26)
			{
				if (GameState.NSFW)
				{
					GameState.AssetManager.GetBundle("suzu/suzu_animated_pinup_nsfw", false, delegate(AssetBundle nsfwBundle)
					{
						GameState.AssetManager.GetBundle("suzu/suzu_animated_pinup", false, delegate(AssetBundle bundle)
						{
							this.InitPeanutPhonePinup(bundle, nsfwBundle, "Suzu", "NSFW", null);
						});
					});
				}
				else
				{
					GameState.AssetManager.GetBundle("suzu/suzu_animated_pinup", false, delegate(AssetBundle bundle)
					{
						this.InitPeanutPhonePinup(bundle, null, "Suzu", "SFW", null);
					});
				}
			}
			else if (image == 28)
			{
				if (GameState.NSFW)
				{
					GameState.AssetManager.GetBundle("sawyer/sawyer_lake_animated_pinup_nsfw", false, delegate(AssetBundle nsfwBundle)
					{
						GameState.AssetManager.GetBundle("sawyer/sawyer_lake_animated_pinup", false, delegate(AssetBundle bundle)
						{
							this.InitPeanutPhonePinup(bundle, nsfwBundle, "SawyerLake", "NSFW", null);
						});
					});
				}
				else
				{
					GameState.AssetManager.GetBundle("sawyer/sawyer_lake_animated_pinup", false, delegate(AssetBundle bundle)
					{
						this.InitPeanutPhonePinup(bundle, null, "SawyerLake", "SFW", null);
					});
				}
			}
			else if (image == 29)
			{
				if (GameState.NSFW)
				{
					GameState.AssetManager.GetBundle("mio/mio_animated_nsfw", false, delegate(AssetBundle nsfwBundle)
					{
						GameState.AssetManager.GetBundle("mio/mio_animated", false, delegate(AssetBundle bundle)
						{
							this.InitPeanutPhonePinup(bundle, nsfwBundle, "MioAnimated", "NSFW", null);
						});
					});
				}
				else
				{
					GameState.AssetManager.GetBundle("mio/mio_animated", false, delegate(AssetBundle bundle)
					{
						this.InitPeanutPhonePinup(bundle, null, "MioAnimated", "SFW", null);
					});
				}
			}
			base.transform.Find("Dialog").transform.localPosition = new Vector3(0f, 213f, 0f);
			if (this.KissUI != null && (image == 24 || image == 25))
			{
				this.KissUI.rectTransform.sizeDelta = new Vector2(788f, 433f);
			}
			else if (this.KissUI != null)
			{
				this.KissUI.rectTransform.sizeDelta = new Vector2(788f, 370f);
			}
			component.offsetMax = new Vector2(0f, 3f);
			component.offsetMin = new Vector2(0f, -5f);
		}
		else
		{
			base.transform.Find("Dialog").transform.localPosition = new Vector3(0f, 187f, 0f);
			if (this.KissUI != null)
			{
				this.KissUI.rectTransform.sizeDelta = new Vector2(788f, 370f);
			}
			component.offsetMax = new Vector2(0f, 3f);
			component.offsetMin = new Vector2(0f, -5f);
		}
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0003A88C File Offset: 0x00038A8C
	public void Close()
	{
		if (this.loadedBundle != null)
		{
			GameState.AssetManager.UnloadBundle(this.loadedBundle, true);
		}
		base.gameObject.SetActive(false);
		this.DestroyParticles();
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0003A8D0 File Offset: 0x00038AD0
	public void SpawnParticles()
	{
		if (Settings.ParticlesDisabled)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleSystem);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		gameObject.GetComponent<ParticleSystem>().Play();
		this.particles.Add(gameObject.GetComponent<ParticleSystem>());
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Hearts);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0003A94C File Offset: 0x00038B4C
	public void DestroyParticles()
	{
		foreach (ParticleSystem particleSystem in this.particles)
		{
			UnityEngine.Object.Destroy(particleSystem.gameObject);
		}
		this.particles.Clear();
	}

	// Token: 0x040006B6 RID: 1718
	public Image KissImage;

	// Token: 0x040006B7 RID: 1719
	public Image KissBackground;

	// Token: 0x040006B8 RID: 1720
	public Image KissUI;

	// Token: 0x040006B9 RID: 1721
	public Image KissOverlay;

	// Token: 0x040006BA RID: 1722
	private AssetBundle loadedBundle;

	// Token: 0x040006BB RID: 1723
	private Action OnDone;

	// Token: 0x040006BC RID: 1724
	private float _currentTime = 5f;

	// Token: 0x040006BD RID: 1725
	private Material _albumSpineMaterial;

	// Token: 0x040006BE RID: 1726
	public GameObject ParticleSystem;

	// Token: 0x040006BF RID: 1727
	private List<ParticleSystem> particles = new List<ParticleSystem>();
}
