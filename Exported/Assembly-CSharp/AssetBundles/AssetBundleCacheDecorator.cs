using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundles
{
	// Token: 0x0200001F RID: 31
	public class AssetBundleCacheDecorator : ICommandHandler<AssetBundleDownloadCommand>
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00007D88 File Offset: 0x00005F88
		public AssetBundleCacheDecorator(ICommandHandler<AssetBundleDownloadCommand> decorated, string baseUri)
		{
			this._baseUri = baseUri;
			this._decorated = decorated;
			this._coroutineHandler = new Action<IEnumerator>(AssetBundleDownloaderMonobehaviour.Instance.HandleCoroutine);
			if (!this._baseUri.EndsWith("/"))
			{
				this._baseUri += "/";
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007E08 File Offset: 0x00006008
		public void Handle(AssetBundleDownloadCommand cmd, AssetBundleManager manager)
		{
			this._coroutineHandler(this.InternalHandle(cmd, manager));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007E20 File Offset: 0x00006020
		public static AssetBundleManager.AssetHash GetCachedHash(string baseUri, string bundleName, AssetBundleManager.AssetHash[] hashes)
		{
			string url = baseUri + bundleName;
			foreach (AssetBundleManager.AssetHash assetHash in hashes)
			{
				url = baseUri + bundleName + "_" + assetHash.Hash;
				IEnumerable<AssetBundleManager.AssetHash> enumerable = hashes.Concat(new AssetBundleManager.AssetHash[]
				{
					new AssetBundleManager.AssetHash(bundleName, "0000e00c000ec0000000e0e00c0d0c00", "0", false)
				});
				foreach (AssetBundleManager.AssetHash assetHash2 in enumerable)
				{
					bool flag = Caching.IsVersionCached(url, assetHash2.Hash128);
					if (flag)
					{
						return assetHash2;
					}
				}
			}
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007F00 File Offset: 0x00006100
		private IEnumerator InternalHandle(AssetBundleDownloadCommand cmd, AssetBundleManager manager)
		{
			bool _foundBundle = false;
			if (cmd.Hashes == null || cmd.Hashes.Length == 0 || cmd.Hashes[0].Hash128 == AssetBundleCacheDecorator.DEFAULT_HASH)
			{
				yield break;
			}
			AssetBundleManager.Log(string.Format("Using Cache method for bundle [{0}]", cmd.BundleName), ELoggingLevel.Debug);
			AssetBundleManager.AssetHash cachedHash = AssetBundleCacheDecorator.GetCachedHash(this._baseUri, cmd.BundleName, cmd.Hashes);
			bool isCached = cachedHash != null;
			if (isCached)
			{
				string uri = this._baseUri + cmd.BundleName + "_" + cachedHash.Hash;
				using (UnityWebRequest req = UnityWebRequest.GetAssetBundle(uri, cachedHash.Hash128, 0U))
				{
					yield return req.Send();
					if (req.isError)
					{
						string errorMessage = string.Format("Network or HTTP error getting Asset Bundle. URI: {0} - Hash: {1}" + Environment.NewLine, uri, cachedHash);
						AssetBundleManager.Log(errorMessage + req.error, ELoggingLevel.Error);
					}
					else
					{
						AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(req);
						if (bundle != null)
						{
							_foundBundle = true;
							cmd.OnComplete(bundle);
						}
						else
						{
							AssetBundleManager.Log(string.Format("Error downloading Asset Bundle from Cache. URI: {0} - Hash: {1}", uri, cachedHash), ELoggingLevel.Error);
						}
					}
				}
			}
			if (!_foundBundle)
			{
				AssetBundleManager.Log(string.Format("Bundle [{0}] is not cached. Switching to fallback method.", cmd.BundleName), ELoggingLevel.Debug);
				this._decorated.Handle(cmd, manager);
			}
			yield break;
		}

		// Token: 0x040000B2 RID: 178
		private ICommandHandler<AssetBundleDownloadCommand> _decorated;

		// Token: 0x040000B3 RID: 179
		private Action<IEnumerator> _coroutineHandler;

		// Token: 0x040000B4 RID: 180
		private string _baseUri;

		// Token: 0x040000B5 RID: 181
		private static readonly Hash128 DEFAULT_HASH = default(Hash128);
	}
}
