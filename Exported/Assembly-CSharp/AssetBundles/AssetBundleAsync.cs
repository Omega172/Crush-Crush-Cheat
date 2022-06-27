using System;
using System.Collections;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200001D RID: 29
	public class AssetBundleAsync : IEnumerator
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00007C60 File Offset: 0x00005E60
		public AssetBundleAsync(string bundleName, bool allowDownload, Action<string, bool, Action<AssetBundle>> callToAction)
		{
			this.IsDone = false;
			callToAction(bundleName, allowDownload, new Action<AssetBundle>(this.OnAssetBundleComplete));
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007C90 File Offset: 0x00005E90
		public AssetBundleAsync()
		{
			this.IsDone = true;
			this.Failed = true;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00007CA8 File Offset: 0x00005EA8
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00007CB0 File Offset: 0x00005EB0
		public bool IsDone { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00007CBC File Offset: 0x00005EBC
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00007CC4 File Offset: 0x00005EC4
		public bool Failed { get; private set; }

		// Token: 0x060000D2 RID: 210 RVA: 0x00007CD0 File Offset: 0x00005ED0
		private void OnAssetBundleComplete(AssetBundle bundle)
		{
			this.AssetBundle = bundle;
			this.Failed = (bundle == null);
			this.IsDone = true;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007CF0 File Offset: 0x00005EF0
		public bool MoveNext()
		{
			return !this.IsDone;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007CFC File Offset: 0x00005EFC
		public void Reset()
		{
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00007D00 File Offset: 0x00005F00
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040000AD RID: 173
		public AssetBundle AssetBundle;
	}
}
