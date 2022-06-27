using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundles
{
	// Token: 0x02000021 RID: 33
	public class AssetBundleDownloader : ICommandHandler<AssetBundleDownloadCommand>
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00007F40 File Offset: 0x00006140
		public AssetBundleDownloader(string baseUri)
		{
			this.baseUri = baseUri;
			this.coroutineHandler = new Action<IEnumerator>(AssetBundleDownloaderMonobehaviour.Instance.HandleCoroutine);
			if (!this.baseUri.EndsWith("/"))
			{
				this.baseUri += "/";
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007FD8 File Offset: 0x000061D8
		public void Handle(AssetBundleDownloadCommand cmd, AssetBundleManager manager)
		{
			this.InternalHandle(this.Download(cmd, 0, manager));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007FEC File Offset: 0x000061EC
		private void InternalHandle(IEnumerator downloadCoroutine)
		{
			if (this.activeDownloads < 4)
			{
				this.activeDownloads++;
				this.coroutineHandler(downloadCoroutine);
			}
			else
			{
				this.downloadQueue.Enqueue(downloadCoroutine);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008028 File Offset: 0x00006228
		private IEnumerator Download(AssetBundleDownloadCommand cmd, int retryCount, AssetBundleManager manager)
		{
			string uri = this.baseUri + cmd.BundleName;
			string bundleName = this.baseUri + cmd.BundleName;
			UnityWebRequest req;
			if (cmd.Hashes == null || cmd.Hashes.Length == 0 || cmd.Hashes[0].Hash128 == AssetBundleDownloader.DEFAULT_HASH)
			{
				AssetBundleManager.Log(string.Format("Download AssetBundle [{0}].", uri), ELoggingLevel.Debug);
				req = UnityWebRequest.GetAssetBundle(uri);
			}
			else
			{
				Hash128 hash = AssetBundleDownloader.DEFAULT_HASH;
				uri = this.baseUri + cmd.BundleName + "_" + cmd.Hashes[0].Hash;
				hash = cmd.Hashes[0].Hash128;
				AssetBundleManager.Log(string.Format("Download AssetBundle [{0}] [{1}].", uri, hash.ToString()), ELoggingLevel.Debug);
				req = UnityWebRequest.GetAssetBundle(uri, hash, 0U);
			}
			if (!cmd.AllowDownload)
			{
				if (req != null)
				{
					req.Dispose();
				}
				manager.AddPendingDownload(uri, cmd, cmd.Hashes[0], false);
				this.activeDownloads--;
				yield break;
			}
			req.Send();
			while (!req.isDone)
			{
				cmd.Progress = req.downloadProgress;
				if (cmd.OnProgress != null)
				{
					cmd.OnProgress(manager);
				}
				yield return null;
			}
			cmd.Progress = 1f;
			if (cmd.OnProgress != null)
			{
				cmd.OnProgress(manager);
			}
			bool isNetworkError = req.isError;
			bool isHttpError = req.isError;
			if (isHttpError)
			{
				AssetBundleManager.Log(string.Format("Error downloading [{0}]: [{1}] [{2}]", uri, req.responseCode, req.error), ELoggingLevel.Error);
				if (retryCount < 3 && AssetBundleDownloader.RETRY_ON_ERRORS.Contains(req.responseCode))
				{
					AssetBundleManager.Log(string.Format("Retrying [{0}] in [{1}] seconds...", uri, 1f), ELoggingLevel.Warning);
					req.Dispose();
					this.activeDownloads--;
					yield return new WaitForSeconds(1f);
					this.InternalHandle(this.Download(cmd, retryCount + 1, manager));
					yield break;
				}
			}
			try
			{
				AssetBundle bundle;
				if (isNetworkError || isHttpError)
				{
					AssetBundleManager.Log(string.Format("Error downloading [{0}]: [{1}]", uri, req.error), ELoggingLevel.Error);
					bundle = null;
					manager.AddPendingDownload(uri, cmd, cmd.Hashes[0], true);
				}
				else
				{
					bundle = DownloadHandlerAssetBundle.GetContent(req);
				}
				AssetBundleManager.Log(string.Format("Downloaded: [{0}] Bundle is null: [{1}]", uri, (bundle == null).ToString()), ELoggingLevel.Debug);
				cmd.OnComplete(bundle);
			}
			finally
			{
				req.Dispose();
				this.activeDownloads--;
				if (this.downloadQueue.Count > 0)
				{
					this.InternalHandle(this.downloadQueue.Dequeue());
				}
			}
			yield break;
		}

		// Token: 0x040000BE RID: 190
		private const int MAX_RETRY_COUNT = 3;

		// Token: 0x040000BF RID: 191
		private const float RETRY_WAIT_PERIOD = 1f;

		// Token: 0x040000C0 RID: 192
		private const int MAX_SIMULTANEOUS_DOWNLOADS = 4;

		// Token: 0x040000C1 RID: 193
		private static readonly Hash128 DEFAULT_HASH = default(Hash128);

		// Token: 0x040000C2 RID: 194
		private static readonly long[] RETRY_ON_ERRORS = new long[]
		{
			503L
		};

		// Token: 0x040000C3 RID: 195
		private string baseUri;

		// Token: 0x040000C4 RID: 196
		private Action<IEnumerator> coroutineHandler;

		// Token: 0x040000C5 RID: 197
		private int activeDownloads;

		// Token: 0x040000C6 RID: 198
		private Queue<IEnumerator> downloadQueue = new Queue<IEnumerator>();
	}
}
