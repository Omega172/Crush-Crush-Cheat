using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000020 RID: 32
	public class AssetBundleDownloadCommand
	{
		// Token: 0x040000B6 RID: 182
		public string BundleName;

		// Token: 0x040000B7 RID: 183
		public AssetBundleManager.AssetHash[] Hashes;

		// Token: 0x040000B8 RID: 184
		public Action<AssetBundle> OnComplete;

		// Token: 0x040000B9 RID: 185
		public Action<AssetBundleManager> OnProgress;

		// Token: 0x040000BA RID: 186
		public bool AllowDownload;

		// Token: 0x040000BB RID: 187
		public long Size;

		// Token: 0x040000BC RID: 188
		public float Progress;

		// Token: 0x040000BD RID: 189
		public bool BuiltIn;
	}
}
