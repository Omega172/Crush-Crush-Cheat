using System;

namespace Spine
{
	// Token: 0x02000180 RID: 384
	public class TransformConstraintTimeline : CurveTimeline
	{
		// Token: 0x06000B04 RID: 2820 RVA: 0x00057A50 File Offset: 0x00055C50
		public TransformConstraintTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 5];
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00057A68 File Offset: 0x00055C68
		public override int PropertyId
		{
			get
			{
				return 167772160 + this.transformConstraintIndex;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00057A94 File Offset: 0x00055C94
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x00057A78 File Offset: 0x00055C78
		public int TransformConstraintIndex
		{
			get
			{
				return this.transformConstraintIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.transformConstraintIndex = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00057A9C File Offset: 0x00055C9C
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00057AA4 File Offset: 0x00055CA4
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

		// Token: 0x06000B0A RID: 2826 RVA: 0x00057AB0 File Offset: 0x00055CB0
		public void SetFrame(int frameIndex, float time, float rotateMix, float translateMix, float scaleMix, float shearMix)
		{
			frameIndex *= 5;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = rotateMix;
			this.frames[frameIndex + 2] = translateMix;
			this.frames[frameIndex + 3] = scaleMix;
			this.frames[frameIndex + 4] = shearMix;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00057AF0 File Offset: 0x00055CF0
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			TransformConstraint transformConstraint = skeleton.transformConstraints.Items[this.transformConstraintIndex];
			if (!transformConstraint.active)
			{
				return;
			}
			float[] array = this.frames;
			if (time >= array[0])
			{
				float num2;
				float num3;
				float num4;
				float num5;
				if (time >= array[array.Length - 5])
				{
					int num = array.Length;
					num2 = array[num + -4];
					num3 = array[num + -3];
					num4 = array[num + -2];
					num5 = array[num + -1];
				}
				else
				{
					int num6 = Animation.BinarySearch(array, time, 5);
					num2 = array[num6 + -4];
					num3 = array[num6 + -3];
					num4 = array[num6 + -2];
					num5 = array[num6 + -1];
					float num7 = array[num6];
					float curvePercent = base.GetCurvePercent(num6 / 5 - 1, 1f - (time - num7) / (array[num6 + -5] - num7));
					num2 += (array[num6 + 1] - num2) * curvePercent;
					num3 += (array[num6 + 2] - num3) * curvePercent;
					num4 += (array[num6 + 3] - num4) * curvePercent;
					num5 += (array[num6 + 4] - num5) * curvePercent;
				}
				if (blend == MixBlend.Setup)
				{
					TransformConstraintData data = transformConstraint.data;
					transformConstraint.rotateMix = data.rotateMix + (num2 - data.rotateMix) * alpha;
					transformConstraint.translateMix = data.translateMix + (num3 - data.translateMix) * alpha;
					transformConstraint.scaleMix = data.scaleMix + (num4 - data.scaleMix) * alpha;
					transformConstraint.shearMix = data.shearMix + (num5 - data.shearMix) * alpha;
				}
				else
				{
					transformConstraint.rotateMix += (num2 - transformConstraint.rotateMix) * alpha;
					transformConstraint.translateMix += (num3 - transformConstraint.translateMix) * alpha;
					transformConstraint.scaleMix += (num4 - transformConstraint.scaleMix) * alpha;
					transformConstraint.shearMix += (num5 - transformConstraint.shearMix) * alpha;
				}
				return;
			}
			TransformConstraintData data2 = transformConstraint.data;
			if (blend == MixBlend.Setup)
			{
				transformConstraint.rotateMix = data2.rotateMix;
				transformConstraint.translateMix = data2.translateMix;
				transformConstraint.scaleMix = data2.scaleMix;
				transformConstraint.shearMix = data2.shearMix;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			transformConstraint.rotateMix += (data2.rotateMix - transformConstraint.rotateMix) * alpha;
			transformConstraint.translateMix += (data2.translateMix - transformConstraint.translateMix) * alpha;
			transformConstraint.scaleMix += (data2.scaleMix - transformConstraint.scaleMix) * alpha;
			transformConstraint.shearMix += (data2.shearMix - transformConstraint.shearMix) * alpha;
		}

		// Token: 0x04000A60 RID: 2656
		public const int ENTRIES = 5;

		// Token: 0x04000A61 RID: 2657
		private const int PREV_TIME = -5;

		// Token: 0x04000A62 RID: 2658
		private const int PREV_ROTATE = -4;

		// Token: 0x04000A63 RID: 2659
		private const int PREV_TRANSLATE = -3;

		// Token: 0x04000A64 RID: 2660
		private const int PREV_SCALE = -2;

		// Token: 0x04000A65 RID: 2661
		private const int PREV_SHEAR = -1;

		// Token: 0x04000A66 RID: 2662
		private const int ROTATE = 1;

		// Token: 0x04000A67 RID: 2663
		private const int TRANSLATE = 2;

		// Token: 0x04000A68 RID: 2664
		private const int SCALE = 3;

		// Token: 0x04000A69 RID: 2665
		private const int SHEAR = 4;

		// Token: 0x04000A6A RID: 2666
		internal int transformConstraintIndex;

		// Token: 0x04000A6B RID: 2667
		internal float[] frames;
	}
}
