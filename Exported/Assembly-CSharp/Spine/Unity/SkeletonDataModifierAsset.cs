using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001DA RID: 474
	public abstract class SkeletonDataModifierAsset : ScriptableObject
	{
		// Token: 0x06000F57 RID: 3927
		public abstract void Apply(SkeletonData skeletonData);
	}
}
