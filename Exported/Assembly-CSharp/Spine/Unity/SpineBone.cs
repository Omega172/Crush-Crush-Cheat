using System;

namespace Spine.Unity
{
	// Token: 0x0200021A RID: 538
	public class SpineBone : SpineAttributeBase
	{
		// Token: 0x06001128 RID: 4392 RVA: 0x00079DF0 File Offset: 0x00077FF0
		public SpineBone(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00079E18 File Offset: 0x00078018
		public static Bone GetBone(string boneName, SkeletonRenderer renderer)
		{
			return (renderer.skeleton != null) ? renderer.skeleton.FindBone(boneName) : null;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00079E38 File Offset: 0x00078038
		public static BoneData GetBoneData(string boneName, SkeletonDataAsset skeletonDataAsset)
		{
			SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
			return skeletonData.FindBone(boneName);
		}
	}
}
