using System;

namespace Spine
{
	// Token: 0x02000183 RID: 387
	public class PathConstraintMixTimeline : CurveTimeline
	{
		// Token: 0x06000B17 RID: 2839 RVA: 0x00058094 File Offset: 0x00056294
		public PathConstraintMixTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 3];
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000580AC File Offset: 0x000562AC
		public override int PropertyId
		{
			get
			{
				return 218103808 + this.pathConstraintIndex;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x000580D8 File Offset: 0x000562D8
		// (set) Token: 0x06000B19 RID: 2841 RVA: 0x000580BC File Offset: 0x000562BC
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x000580E0 File Offset: 0x000562E0
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x000580E8 File Offset: 0x000562E8
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

		// Token: 0x06000B1D RID: 2845 RVA: 0x000580F4 File Offset: 0x000562F4
		public void SetFrame(int frameIndex, float time, float rotateMix, float translateMix)
		{
			frameIndex *= 3;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = rotateMix;
			this.frames[frameIndex + 2] = translateMix;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0005811C File Offset: 0x0005631C
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
				float num2;
				if (time >= array[array.Length - 3])
				{
					num = array[array.Length + -2];
					num2 = array[array.Length + -1];
				}
				else
				{
					int num3 = Animation.BinarySearch(array, time, 3);
					num = array[num3 + -2];
					num2 = array[num3 + -1];
					float num4 = array[num3];
					float curvePercent = base.GetCurvePercent(num3 / 3 - 1, 1f - (time - num4) / (array[num3 + -3] - num4));
					num += (array[num3 + 1] - num) * curvePercent;
					num2 += (array[num3 + 2] - num2) * curvePercent;
				}
				if (blend == MixBlend.Setup)
				{
					pathConstraint.rotateMix = pathConstraint.data.rotateMix + (num - pathConstraint.data.rotateMix) * alpha;
					pathConstraint.translateMix = pathConstraint.data.translateMix + (num2 - pathConstraint.data.translateMix) * alpha;
				}
				else
				{
					pathConstraint.rotateMix += (num - pathConstraint.rotateMix) * alpha;
					pathConstraint.translateMix += (num2 - pathConstraint.translateMix) * alpha;
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				pathConstraint.rotateMix = pathConstraint.data.rotateMix;
				pathConstraint.translateMix = pathConstraint.data.translateMix;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			pathConstraint.rotateMix += (pathConstraint.data.rotateMix - pathConstraint.rotateMix) * alpha;
			pathConstraint.translateMix += (pathConstraint.data.translateMix - pathConstraint.translateMix) * alpha;
		}

		// Token: 0x04000A72 RID: 2674
		public const int ENTRIES = 3;

		// Token: 0x04000A73 RID: 2675
		private const int PREV_TIME = -3;

		// Token: 0x04000A74 RID: 2676
		private const int PREV_ROTATE = -2;

		// Token: 0x04000A75 RID: 2677
		private const int PREV_TRANSLATE = -1;

		// Token: 0x04000A76 RID: 2678
		private const int ROTATE = 1;

		// Token: 0x04000A77 RID: 2679
		private const int TRANSLATE = 2;

		// Token: 0x04000A78 RID: 2680
		internal int pathConstraintIndex;

		// Token: 0x04000A79 RID: 2681
		internal float[] frames;
	}
}
