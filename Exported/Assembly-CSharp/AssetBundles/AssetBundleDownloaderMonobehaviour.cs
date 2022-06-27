using System;
using System.Collections;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000022 RID: 34
	public class AssetBundleDownloaderMonobehaviour : MonoBehaviour
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00008078 File Offset: 0x00006278
		public static AssetBundleDownloaderMonobehaviour Instance
		{
			get
			{
				if (AssetBundleDownloaderMonobehaviour.instance == null)
				{
					AssetBundleDownloaderMonobehaviour.instance = AssetBundleDownloaderMonobehaviour.CreateInstance();
				}
				return AssetBundleDownloaderMonobehaviour.instance;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000809C File Offset: 0x0000629C
		private static AssetBundleDownloaderMonobehaviour CreateInstance()
		{
			GameObject gameObject = new GameObject("AssetBundleDownloaderMonobehaviour");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<AssetBundleDownloaderMonobehaviour>();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000080C0 File Offset: 0x000062C0
		private void Awake()
		{
			if (AssetBundleDownloaderMonobehaviour.instance != null)
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
				return;
			}
			Application.runInBackground = true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000080F0 File Offset: 0x000062F0
		private void OnDestroy()
		{
			if (AssetBundleDownloaderMonobehaviour.instance == this)
			{
				AssetBundleDownloaderMonobehaviour.instance = null;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00008108 File Offset: 0x00006308
		public void HandleCoroutine(IEnumerator coroutine)
		{
			base.StartCoroutine(coroutine);
		}

		// Token: 0x040000C7 RID: 199
		private static AssetBundleDownloaderMonobehaviour instance;
	}
}
