using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000024 RID: 36
	public class AssetBundleManager : IDisposable
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00008150 File Offset: 0x00006350
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00008158 File Offset: 0x00006358
		public bool Initialized { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00008164 File Offset: 0x00006364
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000816C File Offset: 0x0000636C
		public AssetBundleManifest Manifest { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00008178 File Offset: 0x00006378
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008180 File Offset: 0x00006380
		public AssetBundleManager.PrimaryManifestType PrimaryManifest { get; private set; }

		// Token: 0x060000F8 RID: 248 RVA: 0x0000818C File Offset: 0x0000638C
		public AssetBundleManager SetBaseUri(string uri)
		{
			return this.SetBaseUri(new string[]
			{
				uri
			});
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000081A0 File Offset: 0x000063A0
		public AssetBundleManager SetBaseUri(string[] uris)
		{
			if (this.baseUri == null || this.baseUri.Length == 0)
			{
				AssetBundleManager.Log(string.Format("Setting base uri to [{0}].", string.Join(",", uris)), ELoggingLevel.Warning);
			}
			else
			{
				AssetBundleManager.Log(string.Format("Overriding base uri from [{0}] to [{1}].", string.Join(",", this.baseUri), string.Join(",", uris)), ELoggingLevel.Warning);
			}
			this.baseUri = new string[uris.Length];
			for (int i = 0; i < uris.Length; i++)
			{
				StringBuilder stringBuilder = new StringBuilder(uris[i]);
				if (!uris[i].EndsWith("/"))
				{
					stringBuilder.Append("/");
				}
				stringBuilder.Append(Utility.GetPlatformName()).Append("/");
				this.baseUri[i] = stringBuilder.ToString();
			}
			return this;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008280 File Offset: 0x00006480
		public AssetBundleManager UseSimulatedUri()
		{
			this.SetBaseUri(new string[]
			{
				string.Format("file://{0}/../AssetBundles/", Application.dataPath)
			});
			return this;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000082B0 File Offset: 0x000064B0
		public AssetBundleManager UseStreamingAssetsFolder()
		{
			this.SetBaseUri(new string[]
			{
				Application.streamingAssetsPath
			});
			return this;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000082C8 File Offset: 0x000064C8
		public AssetBundleManager SetPrioritizationStrategy(AssetBundleManager.PrioritizationStrategy strategy)
		{
			this.defaultPrioritizationStrategy = strategy;
			return this;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000082D4 File Offset: 0x000064D4
		public AssetBundleManager.AssetHash[] GetHashNames(string bundleName)
		{
			AssetBundleManager.AssetBundleObject[] array = null;
			if (this._manifest.TryGetValue(bundleName, out array))
			{
				AssetBundleManager.AssetBundleObject[] array2 = array;
				AssetBundleManager.AssetHash[] array3 = new AssetBundleManager.AssetHash[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					AssetBundleManager.AssetBundleObject assetBundleObject = array2[i];
					array3[i] = new AssetBundleManager.AssetHash(bundleName, assetBundleObject.Name, assetBundleObject.Size.ToString(), array2[i].BuiltIn);
				}
				return array3;
			}
			return new AssetBundleManager.AssetHash[]
			{
				new AssetBundleManager.AssetHash(bundleName, string.Empty, "0", false)
			};
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008360 File Offset: 0x00006560
		public void Initialize(Action<bool> onComplete)
		{
			if (this.baseUri.Length == 0)
			{
				AssetBundleManager.Log("You need to set the base uri before you can initialize.", ELoggingLevel.Error);
				return;
			}
			string path = Utility.GetPlatformName().ToLowerInvariant() + "_manifest";
			TextAsset textAsset = Resources.Load<TextAsset>(path);
			this._manifest = SimpleJson.DeserializeObject<Dictionary<string, AssetBundleManager.AssetBundleObject[]>>(textAsset.text, null);
			this.Initialized = true;
			this.handler = (this._bundleDownloader = new AssetBundleDownloader(this.baseUri[0]));
			this.handler = new AssetBundleCacheDecorator(this.handler, this.baseUri[0]);
			this.handler = new StreamingAssetsBundleDownloadDecorator(this.handler, this.defaultPrioritizationStrategy);
			onComplete(this._manifest != null);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000841C File Offset: 0x0000661C
		public AssetBundleManifestAsync InitializeAsync()
		{
			if (this.baseUri == null || this.baseUri.Length == 0)
			{
				AssetBundleManager.Log("You need to set the base uri before you can initialize.", ELoggingLevel.Error);
				return null;
			}
			return new AssetBundleManifestAsync(Utility.GetPlatformName(), delegate(string bundleName, Action<AssetBundle> onAsyncComplete)
			{
				this.Initialize(delegate(bool didComplete)
				{
					onAsyncComplete(null);
				});
			});
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000845C File Offset: 0x0000665C
		public void GetBundle(string bundleName, bool downloadWithoutPrompt, Action<AssetBundle> onComplete)
		{
			if (!this.Initialized)
			{
				Debug.LogError("AssetBundleManager must be initialized before you can get a bundle.");
				if (onComplete != null)
				{
					onComplete(null);
				}
				return;
			}
			AssetBundleManager.DownloadSettings downloadSettings = AssetBundleManager.DownloadSettings.UseCacheIfAvailable;
			downloadSettings |= AssetBundleManager.DownloadSettings.AllowDownload;
			this.GetBundle(bundleName, onComplete, downloadSettings);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000849C File Offset: 0x0000669C
		public void GetBundle(string bundleName, Action<AssetBundle> onComplete, AssetBundleManager.DownloadSettings downloadSettings)
		{
			if (!this.Initialized)
			{
				AssetBundleManager.Log("AssetBundleManager must be initialized before you can get a bundle.", ELoggingLevel.Error);
				if (onComplete != null)
				{
					onComplete(null);
				}
				return;
			}
			AssetBundleManager.AssetHash[] array2;
			if (this._manifest.ContainsKey(bundleName))
			{
				AssetBundleManager.AssetBundleObject[] array = this._manifest[bundleName];
				array2 = new AssetBundleManager.AssetHash[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					AssetBundleManager.AssetBundleObject assetBundleObject = array[i];
					array2[i] = new AssetBundleManager.AssetHash(bundleName, assetBundleObject.Name, assetBundleObject.Size.ToString(), assetBundleObject.BuiltIn);
				}
			}
			else
			{
				if (bundleName.EndsWith("nsfw"))
				{
					if (onComplete != null)
					{
						onComplete(null);
					}
					return;
				}
				try
				{
					string[] array3 = bundleName.Split(new char[]
					{
						'_'
					});
					bundleName = bundleName.Substring(0, bundleName.LastIndexOf("_"));
					array2 = new AssetBundleManager.AssetHash[]
					{
						new AssetBundleManager.AssetHash(bundleName, array3[array3.Length - 1], "0", false)
					};
					AssetBundleManager.Log("Found hash for the bundle" + bundleName, ELoggingLevel.Warning);
				}
				catch (Exception)
				{
					AssetBundleManager.Log("No hash existed for the bundle " + bundleName + ".", ELoggingLevel.Error);
					if (onComplete != null)
					{
						onComplete(null);
					}
					return;
				}
			}
			AssetBundleManager.AssetBundleContainer assetBundleContainer;
			if (this.activeBundles.TryGetValue(bundleName, out assetBundleContainer))
			{
				if (!(assetBundleContainer.AssetBundle == null))
				{
					assetBundleContainer.References++;
					if (onComplete != null)
					{
						onComplete(assetBundleContainer.AssetBundle);
					}
					return;
				}
				this.activeBundles.Remove(bundleName);
			}
			AssetBundleManager.DownloadInProgressContainer downloadInProgressContainer;
			if (this.downloadsInProgress.TryGetValue(bundleName, out downloadInProgressContainer))
			{
				downloadInProgressContainer.References++;
				if (onComplete != null)
				{
					AssetBundleManager.DownloadInProgressContainer downloadInProgressContainer2 = downloadInProgressContainer;
					downloadInProgressContainer2.OnComplete = (Action<AssetBundle>)Delegate.Combine(downloadInProgressContainer2.OnComplete, onComplete);
				}
				if (this.OnDownloadPending != null)
				{
					this.OnDownloadPending(this);
				}
				return;
			}
			this.downloadsInProgress.Add(bundleName, new AssetBundleManager.DownloadInProgressContainer(onComplete));
			bool flag = (downloadSettings & AssetBundleManager.DownloadSettings.UseCacheIfAvailable) == AssetBundleManager.DownloadSettings.UseCacheIfAvailable;
			bool allowDownload = (downloadSettings & AssetBundleManager.DownloadSettings.AllowDownload) == AssetBundleManager.DownloadSettings.AllowDownload;
			AssetBundleDownloadCommand assetBundleDownloadCommand = new AssetBundleDownloadCommand();
			assetBundleDownloadCommand.BundleName = bundleName;
			AssetBundleDownloadCommand assetBundleDownloadCommand2 = assetBundleDownloadCommand;
			AssetBundleManager.AssetHash[] hashes;
			if (flag)
			{
				hashes = array2;
			}
			else
			{
				(hashes = new AssetBundleManager.AssetHash[1])[0] = new AssetBundleManager.AssetHash(string.Empty, default(Hash128).ToString(), "0", false);
			}
			assetBundleDownloadCommand2.Hashes = hashes;
			assetBundleDownloadCommand.OnComplete = delegate(AssetBundle bundle)
			{
				this.OnDownloadComplete(bundleName, bundle);
			};
			assetBundleDownloadCommand.AllowDownload = allowDownload;
			assetBundleDownloadCommand.BuiltIn = (array2.Length == 1 && array2[0].BuiltIn);
			AssetBundleDownloadCommand cmd = assetBundleDownloadCommand;
			this.handler.Handle(cmd, this);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000087DC File Offset: 0x000069DC
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000087E4 File Offset: 0x000069E4
		public Action<AssetBundleManager> OnDownloadPending { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000087F0 File Offset: 0x000069F0
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000087F8 File Offset: 0x000069F8
		public Action<AssetBundleManager> OnDownloadProgress { get; set; }

		// Token: 0x06000106 RID: 262 RVA: 0x00008804 File Offset: 0x00006A04
		public void AddPendingDownload(string uri, AssetBundleDownloadCommand cmd, AssetBundleManager.AssetHash hash, bool addSilently = false)
		{
			if (!this.PendingDownloads.ContainsKey(uri))
			{
				cmd.Size = hash.Size;
				this.PendingDownloads.Add(uri, cmd);
				AssetBundleManager.Log(string.Format("Adding {0} to the list of pending downloads. {1} total assets are pending.", uri, this.PendingDownloads.Count), ELoggingLevel.Debug);
			}
			if (this.OnDownloadPending != null && !addSilently)
			{
				this.OnDownloadPending(this);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000887C File Offset: 0x00006A7C
		public void ProcessPendingDownloads()
		{
			if (this._bundleDownloader == null)
			{
				AssetBundleManager.Log("Initialize must be called before downloading assets.", ELoggingLevel.Error);
				return;
			}
			Dictionary<string, AssetBundleDownloadCommand> downloadQueue = this.PendingDownloads;
			this.PendingDownloads = new Dictionary<string, AssetBundleDownloadCommand>();
			foreach (KeyValuePair<string, AssetBundleDownloadCommand> keyValuePair in downloadQueue)
			{
				AssetBundleManager.Log("Downloading " + keyValuePair.Key, ELoggingLevel.Debug);
				keyValuePair.Value.AllowDownload = true;
				Action<AssetBundle> oldComplete = keyValuePair.Value.OnComplete;
				keyValuePair.Value.OnProgress = this.OnDownloadProgress;
				keyValuePair.Value.OnComplete = delegate(AssetBundle bundle)
				{
					this.OnDownloadProgress(this);
					if (oldComplete != null)
					{
						oldComplete(bundle);
					}
					foreach (KeyValuePair<string, AssetBundleDownloadCommand> keyValuePair2 in downloadQueue)
					{
						if (keyValuePair2.Value.Progress != 1f)
						{
						}
					}
				};
				this._bundleDownloader.Handle(keyValuePair.Value, this);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000089A0 File Offset: 0x00006BA0
		public AssetBundleAsync GetBundleAsync(string bundleName, bool downloadWithoutPrompt)
		{
			if (!this.Initialized)
			{
				AssetBundleManager.Log("AssetBundleManager must be initialized before you can get a bundle.", ELoggingLevel.Error);
				return new AssetBundleAsync();
			}
			return new AssetBundleAsync(bundleName, downloadWithoutPrompt, new Action<string, bool, Action<AssetBundle>>(this.GetBundle));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000089DC File Offset: 0x00006BDC
		public void Dispose()
		{
			foreach (AssetBundleManager.AssetBundleContainer assetBundleContainer in this.activeBundles.Values)
			{
				if (assetBundleContainer.AssetBundle != null)
				{
					assetBundleContainer.AssetBundle.Unload(true);
				}
			}
			this.activeBundles.Clear();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008A68 File Offset: 0x00006C68
		public void UnloadBundle(AssetBundle bundle)
		{
			if (bundle == null)
			{
				return;
			}
			this.UnloadBundle(bundle.name, false, false);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008A88 File Offset: 0x00006C88
		public void UnloadBundle(AssetBundle bundle, bool unloadAllLoadedObjects)
		{
			if (bundle == null)
			{
				return;
			}
			this.UnloadBundle(bundle.name, unloadAllLoadedObjects, false);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public void UnloadBundle(string bundleName, bool unloadAllLoadedObjects, bool force)
		{
			if (string.IsNullOrEmpty(bundleName))
			{
				return;
			}
			AssetBundleManager.AssetBundleContainer assetBundleContainer;
			if (!this.activeBundles.TryGetValue(bundleName, out assetBundleContainer))
			{
				return;
			}
			if (force || --assetBundleContainer.References <= 0)
			{
				if (assetBundleContainer.AssetBundle != null)
				{
					assetBundleContainer.AssetBundle.Unload(unloadAllLoadedObjects);
				}
				this.activeBundles.Remove(bundleName);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008B1C File Offset: 0x00006D1C
		private void OnDownloadComplete(string bundleName, AssetBundle bundle)
		{
			AssetBundleManager.DownloadInProgressContainer downloadInProgressContainer = this.downloadsInProgress[bundleName];
			if (bundle != null)
			{
				this.downloadsInProgress.Remove(bundleName);
				try
				{
					this.activeBundles.Add(bundleName, new AssetBundleManager.AssetBundleContainer
					{
						AssetBundle = bundle,
						References = downloadInProgressContainer.References
					});
				}
				catch (ArgumentException)
				{
					AssetBundleManager.Log("Attempted to activate a bundle that was already active.  Not sure how this happened, attempting to fail gracefully.", ELoggingLevel.Warning);
					this.activeBundles[bundleName].References++;
				}
			}
			if (downloadInProgressContainer.OnComplete != null)
			{
				downloadInProgressContainer.OnComplete(bundle);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008BD8 File Offset: 0x00006DD8
		public int GetBundleSize(string bundleName)
		{
			if (this._manifest == null || !this._manifest.ContainsKey(bundleName))
			{
				return -1;
			}
			return this._manifest[bundleName][0].Size;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008C0C File Offset: 0x00006E0C
		public bool IsCached(string bundleName)
		{
			if (this._manifest == null || !this._manifest.ContainsKey(bundleName))
			{
				return false;
			}
			AssetBundleManager.AssetBundleObject[] array = this._manifest[bundleName];
			AssetBundleManager.AssetHash[] array2 = new AssetBundleManager.AssetHash[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				AssetBundleManager.AssetBundleObject assetBundleObject = array[i];
				if (assetBundleObject.BuiltIn)
				{
					return true;
				}
				array2[i] = new AssetBundleManager.AssetHash(bundleName, assetBundleObject.Name, assetBundleObject.Size.ToString(), assetBundleObject.BuiltIn);
			}
			return AssetBundleCacheDecorator.GetCachedHash(this.baseUri[0], bundleName, array2) != null;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008CAC File Offset: 0x00006EAC
		public static void Log(string message, ELoggingLevel loggingLevel = ELoggingLevel.Debug)
		{
			if (AssetBundleManager.LoggingLevel > loggingLevel)
			{
				return;
			}
			message = "[ABM] " + message;
			if (loggingLevel != ELoggingLevel.Warning)
			{
				if (loggingLevel != ELoggingLevel.Error)
				{
					Debug.Log(message);
				}
				else
				{
					Debug.LogError(message);
				}
			}
			else
			{
				Debug.LogWarning(message);
			}
		}

		// Token: 0x040000CF RID: 207
		private const string MANIFEST_DOWNLOAD_IN_PROGRESS_KEY = "__manifest__";

		// Token: 0x040000D0 RID: 208
		private const string MANIFEST_PLAYERPREFS_KEY = "__abm_manifest_version__";

		// Token: 0x040000D1 RID: 209
		private static readonly ELoggingLevel LoggingLevel = ELoggingLevel.Trace;

		// Token: 0x040000D2 RID: 210
		private string[] baseUri;

		// Token: 0x040000D3 RID: 211
		private AssetBundleManager.PrioritizationStrategy defaultPrioritizationStrategy;

		// Token: 0x040000D4 RID: 212
		private ICommandHandler<AssetBundleDownloadCommand> handler;

		// Token: 0x040000D5 RID: 213
		private IDictionary<string, AssetBundleManager.AssetBundleContainer> activeBundles = new Dictionary<string, AssetBundleManager.AssetBundleContainer>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040000D6 RID: 214
		private IDictionary<string, AssetBundleManager.DownloadInProgressContainer> downloadsInProgress = new Dictionary<string, AssetBundleManager.DownloadInProgressContainer>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040000D7 RID: 215
		private Dictionary<string, AssetBundleManager.AssetBundleObject[]> _manifest;

		// Token: 0x040000D8 RID: 216
		public Dictionary<string, AssetBundleDownloadCommand> PendingDownloads = new Dictionary<string, AssetBundleDownloadCommand>();

		// Token: 0x040000D9 RID: 217
		private AssetBundleDownloader _bundleDownloader;

		// Token: 0x02000025 RID: 37
		[Flags]
		public enum DownloadSettings
		{
			// Token: 0x040000E0 RID: 224
			DoNotUseCache = 0,
			// Token: 0x040000E1 RID: 225
			UseCacheIfAvailable = 1,
			// Token: 0x040000E2 RID: 226
			AllowDownload = 2
		}

		// Token: 0x02000026 RID: 38
		public enum PrioritizationStrategy
		{
			// Token: 0x040000E4 RID: 228
			PrioritizeRemote,
			// Token: 0x040000E5 RID: 229
			PrioritizeStreamingAssets
		}

		// Token: 0x02000027 RID: 39
		public enum PrimaryManifestType
		{
			// Token: 0x040000E7 RID: 231
			None,
			// Token: 0x040000E8 RID: 232
			Remote,
			// Token: 0x040000E9 RID: 233
			RemoteCached,
			// Token: 0x040000EA RID: 234
			StreamingAssets
		}

		// Token: 0x02000028 RID: 40
		public class AssetHash
		{
			// Token: 0x06000112 RID: 274 RVA: 0x00008D3C File Offset: 0x00006F3C
			public AssetHash(string parent, string hash, string size, bool builtIn)
			{
				this.Parent = parent;
				this.Hash = hash;
				this.Size = long.Parse(size);
				this.Hash128 = Hash128.Parse(hash);
				this.BuiltIn = builtIn;
			}

			// Token: 0x040000EB RID: 235
			public string Parent;

			// Token: 0x040000EC RID: 236
			public string Hash;

			// Token: 0x040000ED RID: 237
			public Hash128 Hash128;

			// Token: 0x040000EE RID: 238
			public long Size;

			// Token: 0x040000EF RID: 239
			public bool BuiltIn;
		}

		// Token: 0x02000029 RID: 41
		public class AssetBundleObject
		{
			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000114 RID: 276 RVA: 0x00008D88 File Offset: 0x00006F88
			// (set) Token: 0x06000115 RID: 277 RVA: 0x00008D90 File Offset: 0x00006F90
			public string Name { get; set; }

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000116 RID: 278 RVA: 0x00008D9C File Offset: 0x00006F9C
			// (set) Token: 0x06000117 RID: 279 RVA: 0x00008DA4 File Offset: 0x00006FA4
			public int Size { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000118 RID: 280 RVA: 0x00008DB0 File Offset: 0x00006FB0
			// (set) Token: 0x06000119 RID: 281 RVA: 0x00008DB8 File Offset: 0x00006FB8
			public bool BuiltIn { get; set; }
		}

		// Token: 0x0200002A RID: 42
		internal class AssetBundleContainer
		{
			// Token: 0x040000F3 RID: 243
			public AssetBundle AssetBundle;

			// Token: 0x040000F4 RID: 244
			public int References = 1;
		}

		// Token: 0x0200002B RID: 43
		internal class DownloadInProgressContainer
		{
			// Token: 0x0600011B RID: 283 RVA: 0x00008DD4 File Offset: 0x00006FD4
			public DownloadInProgressContainer(Action<AssetBundle> onComplete)
			{
				this.References = 1;
				this.OnComplete = onComplete;
			}

			// Token: 0x040000F5 RID: 245
			public int References;

			// Token: 0x040000F6 RID: 246
			public Action<AssetBundle> OnComplete;
		}
	}
}
