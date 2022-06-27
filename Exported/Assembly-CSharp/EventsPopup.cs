using System;
using System.Collections;
using AssetBundles;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
public class EventsPopup : MonoBehaviour
{
	// Token: 0x0600066F RID: 1647 RVA: 0x000367F0 File Offset: 0x000349F0
	public void InitEventPopup(string jsonDataUrl)
	{
		this._jsonDataUrl = jsonDataUrl;
		GameState.CurrentState.StartCoroutine(this.ShowEventsBundleAsync());
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0003680C File Offset: 0x00034A0C
	public IEnumerator ShowEventsBundleAsync()
	{
		yield return this.GetBundleEventData();
		if (this._bundleEventData == null)
		{
			yield break;
		}
		if (this._bundleEventData.AssetBundles == null || this._bundleEventData.AssetBundles.Length == 0)
		{
			yield break;
		}
		for (int i = 0; i < this._bundleEventData.AssetBundles.Length; i++)
		{
			this._isEventValid = EventDataResolver.IsEventValid(this._bundleEventData.Requirements[i]);
			if (this._isEventValid)
			{
				yield return this.HandlePopup(this._bundleEventData.AssetBundles[i]);
			}
		}
		yield break;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00036828 File Offset: 0x00034A28
	private IEnumerator GetBundleEventData()
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(this._jsonDataUrl))
		{
			yield return webRequest.Send();
			if (webRequest.isError)
			{
				Debug.LogWarning("Error: " + webRequest.error);
			}
			else
			{
				string json = webRequest.downloadHandler.text;
				this._bundleEventData = SimpleJson.DeserializeObject<BundleEventData>(json, null);
			}
		}
		yield break;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00036844 File Offset: 0x00034A44
	private IEnumerator HandlePopup(string bundleWithHash)
	{
		AssetBundle loadedBundle = null;
		if (loadedBundle == null)
		{
			AssetBundleAsync eventsBundle = GameState.AssetManager.GetBundleAsync(bundleWithHash.ToLowerInvariant(), true);
			yield return eventsBundle;
			loadedBundle = eventsBundle.AssetBundle;
		}
		if (loadedBundle == null)
		{
			yield break;
		}
		string assetName = bundleWithHash;
		if (assetName.Contains("/"))
		{
			assetName = assetName.Substring(assetName.LastIndexOf('/') + 1);
		}
		AssetBundleRequest assetRequest = loadedBundle.LoadAssetAsync<TextAsset>(assetName + ".json");
		yield return assetRequest;
		if (assetRequest.asset == null)
		{
			yield break;
		}
		TextAsset txtAsset = assetRequest.asset as TextAsset;
		assetRequest = loadedBundle.LoadAssetAsync<GameObject>(assetName);
		yield return assetRequest;
		if (assetRequest.asset == null)
		{
			yield break;
		}
		GameObject go = UnityEngine.Object.Instantiate<GameObject>(assetRequest.asset as GameObject);
		CanvasGroup cg = go.AddComponent<CanvasGroup>();
		cg.alpha = 0f;
		cg.blocksRaycasts = false;
		go.transform.SetParent(base.transform.Find("Popups/"), false);
		go.SetActive(true);
		this._eventData = SimpleJson.DeserializeObject<PopupEventData>(txtAsset.text, null);
		yield return this._eventData.Deserialize(go.transform);
		Text[] textObjects = go.GetComponentsInChildren<Text>();
		Font font = GameObject.Find("Canvas").GetComponentInChildren<Text>().font;
		foreach (Text obj in textObjects)
		{
			obj.font = font;
		}
		cg.blocksRaycasts = true;
		cg.alpha = 1f;
		yield return null;
		UnityEngine.Object.Destroy(cg);
		while (go.activeSelf)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(go);
		if (loadedBundle != null)
		{
			loadedBundle.Unload(true);
		}
		yield break;
	}

	// Token: 0x04000673 RID: 1651
	private string _jsonDataUrl = "wrongDataStub";

	// Token: 0x04000674 RID: 1652
	private bool _isEventValid;

	// Token: 0x04000675 RID: 1653
	private BundleEventData _bundleEventData;

	// Token: 0x04000676 RID: 1654
	private PopupEventData _eventData;
}
