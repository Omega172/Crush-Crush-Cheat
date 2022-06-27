using System;
using System.Collections;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200002F RID: 47
	public class StreamingAssetsBundleDownloadDecorator : ICommandHandler<AssetBundleDownloadCommand>
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00008EB0 File Offset: 0x000070B0
		public StreamingAssetsBundleDownloadDecorator(ICommandHandler<AssetBundleDownloadCommand> decorated, AssetBundleManager.PrioritizationStrategy strategy)
		{
			this.decorated = decorated;
			this.currentStrategy = strategy;
			this.currentPlatform = Utility.GetPlatformName();
			this.coroutineHandler = new Action<IEnumerator>(AssetBundleDownloaderMonobehaviour.Instance.HandleCoroutine);
			this.fullBundlePath = string.Format("{0}/{1}", Application.streamingAssetsPath, Utility.GetPlatformName());
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008F0C File Offset: 0x0000710C
		public void Handle(AssetBundleDownloadCommand cmd, AssetBundleManager manager)
		{
			this.coroutineHandler(this.InternalHandle(cmd, manager));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008F24 File Offset: 0x00007124
		private IEnumerator InternalHandle(AssetBundleDownloadCommand cmd, AssetBundleManager manager)
		{
			if (cmd.BundleName != this.currentPlatform && this.currentStrategy == AssetBundleManager.PrioritizationStrategy.PrioritizeStreamingAssets)
			{
				AssetBundleManager.Log(string.Format("Using StreamingAssets for bundle [{0}]", cmd.BundleName), ELoggingLevel.Debug);
				AssetBundleManager.AssetHash[] hashes = cmd.Hashes;
				foreach (AssetBundleManager.AssetHash hash in hashes)
				{
					string path = string.Format("{0}/{1}_{2}", this.fullBundlePath, cmd.BundleName, hash.Hash);
					AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
					while (!request.isDone)
					{
						yield return null;
					}
					if (request.assetBundle != null)
					{
						cmd.OnComplete(request.assetBundle);
						yield break;
					}
				}
				AssetBundleManager.Log(string.Format("StreamingAssets download failed for bundle [{0}], switching to fallback method.", cmd.BundleName), ELoggingLevel.Debug);
			}
			this.decorated.Handle(cmd, manager);
			yield break;
		}

		// Token: 0x040000F9 RID: 249
		private string fullBundlePath;

		// Token: 0x040000FA RID: 250
		private ICommandHandler<AssetBundleDownloadCommand> decorated;

		// Token: 0x040000FB RID: 251
		private AssetBundleManager.PrioritizationStrategy currentStrategy;

		// Token: 0x040000FC RID: 252
		private Action<IEnumerator> coroutineHandler;

		// Token: 0x040000FD RID: 253
		private string currentPlatform;
	}
}
