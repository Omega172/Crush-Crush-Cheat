using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
[Serializable]
public class RequirementData
{
	// Token: 0x060004DE RID: 1246 RVA: 0x000269C8 File Offset: 0x00024BC8
	public int GetVersion()
	{
		return GameState.CurrentVersion;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x000269D0 File Offset: 0x00024BD0
	public bool IsVersionValid(HideEventRequirement hideEventRequirement)
	{
		int version = this.GetVersion();
		if (version < 0)
		{
			return false;
		}
		if (hideEventRequirement != HideEventRequirement.MaxAllowedVersion)
		{
			return hideEventRequirement == HideEventRequirement.MinAllowedVersion && this.GetMinAllowedVersion() <= version;
		}
		return this.GetMaxAllowedVersion() >= version;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00026A20 File Offset: 0x00024C20
	private int GetID(int index)
	{
		if (this.ids == null || this.ids.Length == 0 || index >= this.ids.Length)
		{
			return -1;
		}
		return this.ids[index];
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00026A60 File Offset: 0x00024C60
	public int GetCompletedLTEID()
	{
		return this.GetID(1);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00026A6C File Offset: 0x00024C6C
	public long GetItemID()
	{
		return this.itemId;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00026A74 File Offset: 0x00024C74
	public int GetMaxAllowedVersion()
	{
		return this.GetID(2);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00026A80 File Offset: 0x00024C80
	public int GetMinAllowedVersion()
	{
		return this.GetID(0);
	}

	// Token: 0x040004F7 RID: 1271
	[EnumFlag]
	public HideEventRequirement hideEventRequirement;

	// Token: 0x040004F8 RID: 1272
	[HideInInspector]
	public int[] ids;

	// Token: 0x040004F9 RID: 1273
	[HideInInspector]
	public long itemId;
}
