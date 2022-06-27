using System;
using System.Collections;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200001E RID: 30
	public class AssetBundleManifestAsync : IEnumerator
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00007D04 File Offset: 0x00005F04
		public AssetBundleManifestAsync(string bundleName, Action<string, Action<AssetBundle>> callToAction)
		{
			this.IsDone = false;
			callToAction(bundleName, new Action<AssetBundle>(this.OnAssetBundleManifestComplete));
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00007D34 File Offset: 0x00005F34
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00007D3C File Offset: 0x00005F3C
		public bool Success { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00007D48 File Offset: 0x00005F48
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00007D50 File Offset: 0x00005F50
		public bool IsDone { get; private set; }

		// Token: 0x060000DB RID: 219 RVA: 0x00007D5C File Offset: 0x00005F5C
		private void OnAssetBundleManifestComplete(AssetBundle bundle)
		{
			this.Success = (bundle != null);
			this.IsDone = true;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007D74 File Offset: 0x00005F74
		public bool MoveNext()
		{
			return !this.IsDone;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00007D80 File Offset: 0x00005F80
		public void Reset()
		{
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00007D84 File Offset: 0x00005F84
		public object Current
		{
			get
			{
				return null;
			}
		}
	}
}
