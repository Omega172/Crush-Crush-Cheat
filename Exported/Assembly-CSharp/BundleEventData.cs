using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000D1 RID: 209
[Serializable]
public class BundleEventData
{
	// Token: 0x060004B9 RID: 1209 RVA: 0x000264BC File Offset: 0x000246BC
	public void UpdateAssetBundle(string assetBundleName, RequirementData requirement)
	{
		assetBundleName = "universe/" + assetBundleName;
		this.AddToArray<string>(ref this.AssetBundles, assetBundleName, true);
		this.AddToArray<RequirementData>(ref this.Requirements, requirement, false);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x000264E8 File Offset: 0x000246E8
	private void AddToArray<T>(ref T[] array, T value, bool checkUniqueValue = true)
	{
		List<T> list = (array != null) ? array.ToList<T>() : new List<T>();
		if (checkUniqueValue && list.Contains(value))
		{
			return;
		}
		list.Add(value);
		array = list.ToArray();
	}

	// Token: 0x040004D8 RID: 1240
	public string[] AssetBundles;

	// Token: 0x040004D9 RID: 1241
	public RequirementData[] Requirements;
}
