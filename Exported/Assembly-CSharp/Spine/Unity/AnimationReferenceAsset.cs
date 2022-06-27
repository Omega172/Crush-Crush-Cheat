using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001D1 RID: 465
	[CreateAssetMenu(menuName = "Spine/Animation Reference Asset", order = 100)]
	public class AnimationReferenceAsset : ScriptableObject, IHasSkeletonDataAsset
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0006CE80 File Offset: 0x0006B080
		public SkeletonDataAsset SkeletonDataAsset
		{
			get
			{
				return this.skeletonDataAsset;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0006CE88 File Offset: 0x0006B088
		public Animation Animation
		{
			get
			{
				if (this.animation == null)
				{
					this.Initialize();
				}
				return this.animation;
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0006CEA4 File Offset: 0x0006B0A4
		public void Initialize()
		{
			if (this.skeletonDataAsset == null)
			{
				return;
			}
			this.animation = this.skeletonDataAsset.GetSkeletonData(true).FindAnimation(this.animationName);
			if (this.animation == null)
			{
				Debug.LogWarningFormat("Animation '{0}' not found in SkeletonData : {1}.", new object[]
				{
					this.animationName,
					this.skeletonDataAsset.name
				});
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0006CF14 File Offset: 0x0006B114
		public static implicit operator Animation(AnimationReferenceAsset asset)
		{
			return asset.Animation;
		}

		// Token: 0x04000C94 RID: 3220
		private const bool QuietSkeletonData = true;

		// Token: 0x04000C95 RID: 3221
		[SerializeField]
		protected SkeletonDataAsset skeletonDataAsset;

		// Token: 0x04000C96 RID: 3222
		[SpineAnimation("", "", true, false)]
		[SerializeField]
		protected string animationName;

		// Token: 0x04000C97 RID: 3223
		private Animation animation;
	}
}
