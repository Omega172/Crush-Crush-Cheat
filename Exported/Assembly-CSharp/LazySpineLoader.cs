using System;
using System.Collections;
using AssetBundles;
using Spine.Unity;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class LazySpineLoader : MonoBehaviour
{
	// Token: 0x06000537 RID: 1335 RVA: 0x0002BFA8 File Offset: 0x0002A1A8
	private void OnEnable()
	{
		this._spinner = base.transform.Find("Spinner");
		SkeletonGraphic component = base.transform.Find("Spine").GetComponent<SkeletonGraphic>();
		if (this._material == null || this._material.mainTexture == null)
		{
			this._spinner.gameObject.SetActive(true);
			component.gameObject.SetActive(false);
			base.StartCoroutine(this.InitSpineAsync());
		}
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0002C034 File Offset: 0x0002A234
	private IEnumerator InitSpineAsync()
	{
		AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync(this.AssetBundle, true);
		yield return bundleRequest;
		if (bundleRequest.AssetBundle == null)
		{
			yield break;
		}
		this.InitSpine(bundleRequest.AssetBundle, this.AnimationName, null);
		GameState.AssetManager.UnloadBundle(bundleRequest.AssetBundle);
		yield break;
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0002C050 File Offset: 0x0002A250
	private void InitSpine(AssetBundle bundle, string animationName, Action onSuccess)
	{
		if (bundle == null)
		{
			return;
		}
		SkeletonGraphic component = base.transform.Find("Spine").GetComponent<SkeletonGraphic>();
		TextAsset textAsset = null;
		TextAsset textAsset2 = null;
		Texture texture = null;
		string[] allAssetNames = bundle.GetAllAssetNames();
		for (int i = 0; i < allAssetNames.Length; i++)
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
		if (textAsset == null || textAsset2 == null || texture == null)
		{
			Debug.LogError("Could not load needed assets for animated girl pinup");
			return;
		}
		SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
		skeletonDataAsset.skeletonJSON = textAsset;
		skeletonDataAsset.scale = 0.01f;
		SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
		this._material = new Material(component.material);
		component.material = this._material;
		component.material.SetTexture("_MainTex", texture);
		spineAtlasAsset.atlasFile = textAsset2;
		spineAtlasAsset.materials = new Material[]
		{
			component.material
		};
		skeletonDataAsset.atlasAssets = new SpineAtlasAsset[]
		{
			spineAtlasAsset
		};
		component.skeletonDataAsset = skeletonDataAsset;
		component.startingAnimation = animationName;
		component.startingLoop = true;
		component.Initialize(true);
		this._spinner.gameObject.SetActive(false);
		component.gameObject.SetActive(true);
		if (onSuccess != null)
		{
			onSuccess();
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0002C22C File Offset: 0x0002A42C
	public void Update()
	{
		if (this._spinner.gameObject.activeSelf)
		{
			this._spinner.eulerAngles = new Vector3(0f, 0f, this._spinner.eulerAngles.z - Time.deltaTime * 180f);
		}
	}

	// Token: 0x04000544 RID: 1348
	public string AssetBundle;

	// Token: 0x04000545 RID: 1349
	public string AnimationName;

	// Token: 0x04000546 RID: 1350
	private Transform _spinner;

	// Token: 0x04000547 RID: 1351
	private Material _material;
}
