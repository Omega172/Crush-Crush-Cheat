using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public static class EventDataResolver
{
	// Token: 0x060004CE RID: 1230 RVA: 0x00026798 File Offset: 0x00024998
	public static bool IsEventValid(RequirementData requirementData)
	{
		if (requirementData == null)
		{
			Debug.LogWarning("Event data is null!");
			return false;
		}
		if (requirementData.hideEventRequirement == HideEventRequirement.None)
		{
			return true;
		}
		bool flag = true;
		int num = 0;
		while (num < 32 && flag)
		{
			HideEventRequirement hideEventRequirement = requirementData.hideEventRequirement & (HideEventRequirement)(1 << num);
			if (hideEventRequirement != HideEventRequirement.None)
			{
				HideEventRequirement hideEventRequirement2 = hideEventRequirement;
				switch (hideEventRequirement2)
				{
				case HideEventRequirement.HasItem:
					flag = ((Playfab.AwardedItems & (Playfab.PlayfabItems)requirementData.GetItemID()) == (Playfab.PlayfabItems)0L);
					break;
				default:
					if (hideEventRequirement2 == HideEventRequirement.MinAllowedVersion)
					{
						flag = requirementData.IsVersionValid(HideEventRequirement.MinAllowedVersion);
					}
					break;
				case HideEventRequirement.MaxAllowedVersion:
					flag = requirementData.IsVersionValid(HideEventRequirement.MaxAllowedVersion);
					break;
				}
			}
			num++;
		}
		return flag;
	}
}
