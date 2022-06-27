using System;

namespace Spine
{
	// Token: 0x02000181 RID: 385
	public class PathConstraintPositionTimeline : CurveTimeline
	{
		// Token: 0x06000B0C RID: 2828 RVA: 0x00057DA4 File Offset: 0x00055FA4
		public PathConstraintPositionTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 2];
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00057DBC File Offset: 0x00055FBC
		public override int PropertyId
		{
			get
			{
				return 184549376 + this.pathConstraintIndex;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00057DE8 File Offset: 0x00055FE8
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x00057DCC File Offset: 0x00055FCC
		public int PathConstraintIndex
		{
			get
			{
				return this.pathConstraintIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.pathConstraintIndex = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00057DF0 File Offset: 0x00055FF0
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00057DF8 File Offset: 0x00055FF8
		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00057E04 File Offset: 0x00056004
		public void SetFrame(int frameIndex, float time, float position)
		{
			frameIndex *= 2;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = position;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00057E20 File Offset: 0x00056020
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			PathConstraint pathConstraint = skeleton.pathConstraints.Items[this.pathConstraintIndex];
			if (!pathConstraint.active)
			{
				return;
			}
			float[] array = this.frames;
			if (time >= array[0])
			{
				float num;
				if (time >= array[array.Length - 2])
				{
					num = array[array.Length + -1];
				}
				else
				{
					int num2 = Animation.BinarySearch(array, time, 2);
					num = array[num2 + -1];
					float num3 = array[num2];
					float curvePercent = base.GetCurvePercent(num2 / 2 - 1, 1f - (time - num3) / (array[num2 + -2] - num3));
					num += (array[num2 + 1] - num) * curvePercent;
				}
				if (blend == MixBlend.Setup)
				{
					pathConstraint.position = pathConstraint.data.position + (num - pathConstraint.data.position) * alpha;
				}
				else
				{
					pathConstraint.position += (num - pathConstraint.position) * alpha;
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				pathConstraint.position = pathConstraint.data.position;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			pathConstraint.position += (pathConstraint.data.position - pathConstraint.position) * alpha;
		}

		// Token: 0x04000A6C RID: 2668
		public const int ENTRIES = 2;

		// Token: 0x04000A6D RID: 2669
		protected const int PREV_TIME = -2;

		// Token: 0x04000A6E RID: 2670
		protected const int PREV_VALUE = -1;

		// Token: 0x04000A6F RID: 2671
		protected const int VALUE = 1;

		// Token: 0x04000A70 RID: 2672
		internal int pathConstraintIndex;

		// Token: 0x04000A71 RID: 2673
		internal float[] frames;
	}
}
