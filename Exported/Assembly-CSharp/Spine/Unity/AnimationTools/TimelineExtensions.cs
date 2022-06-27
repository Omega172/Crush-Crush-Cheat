using System;
using UnityEngine;

namespace Spine.Unity.AnimationTools
{
	// Token: 0x0200022D RID: 557
	public static class TimelineExtensions
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x0007D600 File Offset: 0x0007B800
		public static Vector2 Evaluate(this TranslateTimeline timeline, float time, SkeletonData skeletonData = null)
		{
			float[] frames = timeline.frames;
			if (time < frames[0])
			{
				return Vector2.zero;
			}
			float num;
			float num2;
			if (time >= frames[frames.Length - 3])
			{
				num = frames[frames.Length + -2];
				num2 = frames[frames.Length + -1];
			}
			else
			{
				int num3 = Animation.BinarySearch(frames, time, 3);
				num = frames[num3 + -2];
				num2 = frames[num3 + -1];
				float num4 = frames[num3];
				float curvePercent = timeline.GetCurvePercent(num3 / 3 - 1, 1f - (time - num4) / (frames[num3 + -3] - num4));
				num += (frames[num3 + 1] - num) * curvePercent;
				num2 += (frames[num3 + 2] - num2) * curvePercent;
			}
			Vector2 vector = new Vector2(num, num2);
			if (skeletonData == null)
			{
				return vector;
			}
			BoneData boneData = skeletonData.bones.Items[timeline.boneIndex];
			return vector + new Vector2(boneData.x, boneData.y);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0007D700 File Offset: 0x0007B900
		public static TranslateTimeline FindTranslateTimelineForBone(this Animation a, int boneIndex)
		{
			foreach (Timeline timeline in a.timelines)
			{
				if (!timeline.GetType().IsSubclassOf(typeof(TranslateTimeline)))
				{
					TranslateTimeline translateTimeline = timeline as TranslateTimeline;
					if (translateTimeline != null && translateTimeline.boneIndex == boneIndex)
					{
						return translateTimeline;
					}
				}
			}
			return null;
		}
	}
}
