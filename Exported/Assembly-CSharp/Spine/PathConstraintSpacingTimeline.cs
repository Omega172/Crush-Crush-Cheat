using System;

namespace Spine
{
	// Token: 0x02000182 RID: 386
	public class PathConstraintSpacingTimeline : PathConstraintPositionTimeline
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x00057F4C File Offset: 0x0005614C
		public PathConstraintSpacingTimeline(int frameCount) : base(frameCount)
		{
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00057F58 File Offset: 0x00056158
		public override int PropertyId
		{
			get
			{
				return 201326592 + this.pathConstraintIndex;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00057F68 File Offset: 0x00056168
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> events, float alpha, MixBlend blend, MixDirection direction)
		{
			PathConstraint pathConstraint = skeleton.pathConstraints.Items[this.pathConstraintIndex];
			if (!pathConstraint.active)
			{
				return;
			}
			float[] frames = this.frames;
			if (time >= frames[0])
			{
				float num;
				if (time >= frames[frames.Length - 2])
				{
					num = frames[frames.Length + -1];
				}
				else
				{
					int num2 = Animation.BinarySearch(frames, time, 2);
					num = frames[num2 + -1];
					float num3 = frames[num2];
					float curvePercent = base.GetCurvePercent(num2 / 2 - 1, 1f - (time - num3) / (frames[num2 + -2] - num3));
					num += (frames[num2 + 1] - num) * curvePercent;
				}
				if (blend == MixBlend.Setup)
				{
					pathConstraint.spacing = pathConstraint.data.spacing + (num - pathConstraint.data.spacing) * alpha;
				}
				else
				{
					pathConstraint.spacing += (num - pathConstraint.spacing) * alpha;
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				pathConstraint.spacing = pathConstraint.data.spacing;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			pathConstraint.spacing += (pathConstraint.data.spacing - pathConstraint.spacing) * alpha;
		}
	}
}
