using System;
using Spine.Unity.AnimationTools;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E5 RID: 485
	public class SkeletonRootMotion : SkeletonRootMotionBase
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0006F7E8 File Offset: 0x0006D9E8
		protected override float AdditionalScale
		{
			get
			{
				return (!this.canvas) ? 1f : this.canvas.referencePixelsPerUnit;
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0006F810 File Offset: 0x0006DA10
		protected override void Reset()
		{
			base.Reset();
			this.animationTrackFlags = -1;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0006F820 File Offset: 0x0006DA20
		protected override void Start()
		{
			base.Start();
			IAnimationStateComponent animationStateComponent = this.skeletonComponent as IAnimationStateComponent;
			this.animationState = ((animationStateComponent == null) ? null : animationStateComponent.AnimationState);
			if (base.GetComponent<CanvasRenderer>() != null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0006F874 File Offset: 0x0006DA74
		protected override Vector2 CalculateAnimationsMovementDelta()
		{
			Vector2 vector = Vector2.zero;
			int count = this.animationState.Tracks.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.animationTrackFlags == -1 || (this.animationTrackFlags & 1 << i) != 0)
				{
					TrackEntry trackEntry = this.animationState.GetCurrent(i);
					TrackEntry next = null;
					while (trackEntry != null)
					{
						Animation animation = trackEntry.Animation;
						TranslateTimeline translateTimeline = animation.FindTranslateTimelineForBone(this.rootMotionBoneIndex);
						if (translateTimeline != null)
						{
							Vector2 trackMovementDelta = this.GetTrackMovementDelta(trackEntry, translateTimeline, animation, next);
							vector += trackMovementDelta;
						}
						next = trackEntry;
						trackEntry = trackEntry.mixingFrom;
					}
				}
			}
			return vector;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0006F92C File Offset: 0x0006DB2C
		private Vector2 GetTrackMovementDelta(TrackEntry track, TranslateTimeline timeline, Animation animation, TrackEntry next)
		{
			float animationLast = track.animationLast;
			float animationTime = track.AnimationTime;
			Vector2 timelineMovementDelta = base.GetTimelineMovementDelta(animationLast, animationTime, timeline, animation);
			this.ApplyMixAlphaToDelta(ref timelineMovementDelta, next, track);
			return timelineMovementDelta;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0006F960 File Offset: 0x0006DB60
		private void ApplyMixAlphaToDelta(ref Vector2 currentDelta, TrackEntry next, TrackEntry track)
		{
			if (next != null)
			{
				float num;
				if (next.mixDuration == 0f)
				{
					num = 1f;
				}
				else
				{
					num = next.mixTime / next.mixDuration;
					if (num > 1f)
					{
						num = 1f;
					}
				}
				float d = track.alpha * next.interruptAlpha * (1f - num);
				currentDelta *= d;
			}
			else
			{
				float num;
				if (track.mixDuration == 0f)
				{
					num = 1f;
				}
				else
				{
					num = track.alpha * (track.mixTime / track.mixDuration);
					if (num > 1f)
					{
						num = 1f;
					}
				}
				currentDelta *= num;
			}
		}

		// Token: 0x04000CF3 RID: 3315
		private const int DefaultAnimationTrackFlags = -1;

		// Token: 0x04000CF4 RID: 3316
		public int animationTrackFlags = -1;

		// Token: 0x04000CF5 RID: 3317
		private AnimationState animationState;

		// Token: 0x04000CF6 RID: 3318
		private Canvas canvas;
	}
}
