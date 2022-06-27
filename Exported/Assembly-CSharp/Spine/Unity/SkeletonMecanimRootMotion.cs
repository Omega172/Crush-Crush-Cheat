using System;
using Spine.Unity.AnimationTools;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E4 RID: 484
	public class SkeletonMecanimRootMotion : SkeletonRootMotionBase
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0006F678 File Offset: 0x0006D878
		public SkeletonMecanim SkeletonMecanim
		{
			get
			{
				return (!this.skeletonMecanim) ? (this.skeletonMecanim = base.GetComponent<SkeletonMecanim>()) : this.skeletonMecanim;
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0006F6B0 File Offset: 0x0006D8B0
		protected override void Reset()
		{
			base.Reset();
			this.mecanimLayerFlags = -1;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0006F6C0 File Offset: 0x0006D8C0
		protected override void Start()
		{
			base.Start();
			this.skeletonMecanim = base.GetComponent<SkeletonMecanim>();
			if (this.skeletonMecanim)
			{
				this.skeletonMecanim.Translator.OnClipApplied -= this.OnClipApplied;
				this.skeletonMecanim.Translator.OnClipApplied += this.OnClipApplied;
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0006F728 File Offset: 0x0006D928
		private void OnClipApplied(Animation clip, int layerIndex, float weight, float time, float lastTime, bool playsBackward)
		{
			if ((this.mecanimLayerFlags & 1 << layerIndex) == 0 || weight == 0f)
			{
				return;
			}
			TranslateTimeline translateTimeline = clip.FindTranslateTimelineForBone(this.rootMotionBoneIndex);
			if (translateTimeline != null)
			{
				if (!playsBackward)
				{
					this.movementDelta += weight * base.GetTimelineMovementDelta(lastTime, time, translateTimeline, clip);
				}
				else
				{
					this.movementDelta -= weight * base.GetTimelineMovementDelta(time, lastTime, translateTimeline, clip);
				}
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0006F7B8 File Offset: 0x0006D9B8
		protected override Vector2 CalculateAnimationsMovementDelta()
		{
			Vector2 result = this.movementDelta;
			this.movementDelta = Vector2.zero;
			return result;
		}

		// Token: 0x04000CEF RID: 3311
		private const int DefaultMecanimLayerFlags = -1;

		// Token: 0x04000CF0 RID: 3312
		public int mecanimLayerFlags = -1;

		// Token: 0x04000CF1 RID: 3313
		protected Vector2 movementDelta;

		// Token: 0x04000CF2 RID: 3314
		private SkeletonMecanim skeletonMecanim;
	}
}
