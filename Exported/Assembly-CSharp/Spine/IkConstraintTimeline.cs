using System;

namespace Spine
{
	// Token: 0x0200017F RID: 383
	public class IkConstraintTimeline : CurveTimeline
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x00057544 File Offset: 0x00055744
		public IkConstraintTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 6];
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0005755C File Offset: 0x0005575C
		public override int PropertyId
		{
			get
			{
				return 150994944 + this.ikConstraintIndex;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00057588 File Offset: 0x00055788
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x0005756C File Offset: 0x0005576C
		public int IkConstraintIndex
		{
			get
			{
				return this.ikConstraintIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.ikConstraintIndex = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00057590 File Offset: 0x00055790
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00057598 File Offset: 0x00055798
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

		// Token: 0x06000B02 RID: 2818 RVA: 0x000575A4 File Offset: 0x000557A4
		public void SetFrame(int frameIndex, float time, float mix, float softness, int bendDirection, bool compress, bool stretch)
		{
			frameIndex *= 6;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = mix;
			this.frames[frameIndex + 2] = softness;
			this.frames[frameIndex + 3] = (float)bendDirection;
			this.frames[frameIndex + 4] = (float)((!compress) ? 0 : 1);
			this.frames[frameIndex + 5] = (float)((!stretch) ? 0 : 1);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00057618 File Offset: 0x00055818
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			IkConstraint ikConstraint = skeleton.ikConstraints.Items[this.ikConstraintIndex];
			if (!ikConstraint.active)
			{
				return;
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				if (blend == MixBlend.Setup)
				{
					ikConstraint.mix = ikConstraint.data.mix;
					ikConstraint.softness = ikConstraint.data.softness;
					ikConstraint.bendDirection = ikConstraint.data.bendDirection;
					ikConstraint.compress = ikConstraint.data.compress;
					ikConstraint.stretch = ikConstraint.data.stretch;
					return;
				}
				if (blend != MixBlend.First)
				{
					return;
				}
				ikConstraint.mix += (ikConstraint.data.mix - ikConstraint.mix) * alpha;
				ikConstraint.softness += (ikConstraint.data.softness - ikConstraint.softness) * alpha;
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.compress = ikConstraint.data.compress;
				ikConstraint.stretch = ikConstraint.data.stretch;
				return;
			}
			else
			{
				if (time >= array[array.Length - 6])
				{
					if (blend == MixBlend.Setup)
					{
						ikConstraint.mix = ikConstraint.data.mix + (array[array.Length + -5] - ikConstraint.data.mix) * alpha;
						ikConstraint.softness = ikConstraint.data.softness + (array[array.Length + -4] - ikConstraint.data.softness) * alpha;
						if (direction == MixDirection.Out)
						{
							ikConstraint.bendDirection = ikConstraint.data.bendDirection;
							ikConstraint.compress = ikConstraint.data.compress;
							ikConstraint.stretch = ikConstraint.data.stretch;
						}
						else
						{
							ikConstraint.bendDirection = (int)array[array.Length + -3];
							ikConstraint.compress = (array[array.Length + -2] != 0f);
							ikConstraint.stretch = (array[array.Length + -1] != 0f);
						}
					}
					else
					{
						ikConstraint.mix += (array[array.Length + -5] - ikConstraint.mix) * alpha;
						ikConstraint.softness += (array[array.Length + -4] - ikConstraint.softness) * alpha;
						if (direction == MixDirection.In)
						{
							ikConstraint.bendDirection = (int)array[array.Length + -3];
							ikConstraint.compress = (array[array.Length + -2] != 0f);
							ikConstraint.stretch = (array[array.Length + -1] != 0f);
						}
					}
					return;
				}
				int num = Animation.BinarySearch(array, time, 6);
				float num2 = array[num + -5];
				float num3 = array[num + -4];
				float num4 = array[num];
				float curvePercent = base.GetCurvePercent(num / 6 - 1, 1f - (time - num4) / (array[num + -6] - num4));
				if (blend == MixBlend.Setup)
				{
					ikConstraint.mix = ikConstraint.data.mix + (num2 + (array[num + 1] - num2) * curvePercent - ikConstraint.data.mix) * alpha;
					ikConstraint.softness = ikConstraint.data.softness + (num3 + (array[num + 2] - num3) * curvePercent - ikConstraint.data.softness) * alpha;
					if (direction == MixDirection.Out)
					{
						ikConstraint.bendDirection = ikConstraint.data.bendDirection;
						ikConstraint.compress = ikConstraint.data.compress;
						ikConstraint.stretch = ikConstraint.data.stretch;
					}
					else
					{
						ikConstraint.bendDirection = (int)array[num + -3];
						ikConstraint.compress = (array[num + -2] != 0f);
						ikConstraint.stretch = (array[num + -1] != 0f);
					}
				}
				else
				{
					ikConstraint.mix += (num2 + (array[num + 1] - num2) * curvePercent - ikConstraint.mix) * alpha;
					ikConstraint.softness += (num3 + (array[num + 2] - num3) * curvePercent - ikConstraint.softness) * alpha;
					if (direction == MixDirection.In)
					{
						ikConstraint.bendDirection = (int)array[num + -3];
						ikConstraint.compress = (array[num + -2] != 0f);
						ikConstraint.stretch = (array[num + -1] != 0f);
					}
				}
				return;
			}
		}

		// Token: 0x04000A52 RID: 2642
		public const int ENTRIES = 6;

		// Token: 0x04000A53 RID: 2643
		private const int PREV_TIME = -6;

		// Token: 0x04000A54 RID: 2644
		private const int PREV_MIX = -5;

		// Token: 0x04000A55 RID: 2645
		private const int PREV_SOFTNESS = -4;

		// Token: 0x04000A56 RID: 2646
		private const int PREV_BEND_DIRECTION = -3;

		// Token: 0x04000A57 RID: 2647
		private const int PREV_COMPRESS = -2;

		// Token: 0x04000A58 RID: 2648
		private const int PREV_STRETCH = -1;

		// Token: 0x04000A59 RID: 2649
		private const int MIX = 1;

		// Token: 0x04000A5A RID: 2650
		private const int SOFTNESS = 2;

		// Token: 0x04000A5B RID: 2651
		private const int BEND_DIRECTION = 3;

		// Token: 0x04000A5C RID: 2652
		private const int COMPRESS = 4;

		// Token: 0x04000A5D RID: 2653
		private const int STRETCH = 5;

		// Token: 0x04000A5E RID: 2654
		internal int ikConstraintIndex;

		// Token: 0x04000A5F RID: 2655
		internal float[] frames;
	}
}
