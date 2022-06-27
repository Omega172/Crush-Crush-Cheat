using System;

namespace Spine
{
	// Token: 0x0200017A RID: 378
	public class TwoColorTimeline : CurveTimeline, ISlotTimeline
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x00056524 File Offset: 0x00054724
		public TwoColorTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 8];
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0005653C File Offset: 0x0005473C
		public override int PropertyId
		{
			get
			{
				return 234881024 + this.slotIndex;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00056568 File Offset: 0x00054768
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0005654C File Offset: 0x0005474C
		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.slotIndex = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00056570 File Offset: 0x00054770
		public float[] Frames
		{
			get
			{
				return this.frames;
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00056578 File Offset: 0x00054778
		public void SetFrame(int frameIndex, float time, float r, float g, float b, float a, float r2, float g2, float b2)
		{
			frameIndex *= 8;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = r;
			this.frames[frameIndex + 2] = g;
			this.frames[frameIndex + 3] = b;
			this.frames[frameIndex + 4] = a;
			this.frames[frameIndex + 5] = r2;
			this.frames[frameIndex + 6] = g2;
			this.frames[frameIndex + 7] = b2;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000565E8 File Offset: 0x000547E8
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			if (!slot.bone.active)
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
				float num6;
				float num7;
				float num8;
				if (time >= array[array.Length - 8])
				{
					int num = array.Length;
					num2 = array[num + -7];
					num3 = array[num + -6];
					num4 = array[num + -5];
					num5 = array[num + -4];
					num6 = array[num + -3];
					num7 = array[num + -2];
					num8 = array[num + -1];
				}
				else
				{
					int num9 = Animation.BinarySearch(array, time, 8);
					num2 = array[num9 + -7];
					num3 = array[num9 + -6];
					num4 = array[num9 + -5];
					num5 = array[num9 + -4];
					num6 = array[num9 + -3];
					num7 = array[num9 + -2];
					num8 = array[num9 + -1];
					float num10 = array[num9];
					float curvePercent = base.GetCurvePercent(num9 / 8 - 1, 1f - (time - num10) / (array[num9 + -8] - num10));
					num2 += (array[num9 + 1] - num2) * curvePercent;
					num3 += (array[num9 + 2] - num3) * curvePercent;
					num4 += (array[num9 + 3] - num4) * curvePercent;
					num5 += (array[num9 + 4] - num5) * curvePercent;
					num6 += (array[num9 + 5] - num6) * curvePercent;
					num7 += (array[num9 + 6] - num7) * curvePercent;
					num8 += (array[num9 + 7] - num8) * curvePercent;
				}
				if (alpha == 1f)
				{
					slot.r = num2;
					slot.g = num3;
					slot.b = num4;
					slot.a = num5;
					slot.ClampColor();
					slot.r2 = num6;
					slot.g2 = num7;
					slot.b2 = num8;
					slot.ClampSecondColor();
				}
				else
				{
					float r;
					float g;
					float b;
					float a;
					float r2;
					float g2;
					float b2;
					if (blend == MixBlend.Setup)
					{
						r = slot.data.r;
						g = slot.data.g;
						b = slot.data.b;
						a = slot.data.a;
						r2 = slot.data.r2;
						g2 = slot.data.g2;
						b2 = slot.data.b2;
					}
					else
					{
						r = slot.r;
						g = slot.g;
						b = slot.b;
						a = slot.a;
						r2 = slot.r2;
						g2 = slot.g2;
						b2 = slot.b2;
					}
					slot.r = r + (num2 - r) * alpha;
					slot.g = g + (num3 - g) * alpha;
					slot.b = b + (num4 - b) * alpha;
					slot.a = a + (num5 - a) * alpha;
					slot.ClampColor();
					slot.r2 = r2 + (num6 - r2) * alpha;
					slot.g2 = g2 + (num7 - g2) * alpha;
					slot.b2 = b2 + (num8 - b2) * alpha;
					slot.ClampSecondColor();
				}
				return;
			}
			SlotData data = slot.data;
			if (blend == MixBlend.Setup)
			{
				slot.r = data.r;
				slot.g = data.g;
				slot.b = data.b;
				slot.a = data.a;
				slot.ClampColor();
				slot.r2 = data.r2;
				slot.g2 = data.g2;
				slot.b2 = data.b2;
				slot.ClampSecondColor();
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			slot.r += (slot.r - data.r) * alpha;
			slot.g += (slot.g - data.g) * alpha;
			slot.b += (slot.b - data.b) * alpha;
			slot.a += (slot.a - data.a) * alpha;
			slot.ClampColor();
			slot.r2 += (slot.r2 - data.r2) * alpha;
			slot.g2 += (slot.g2 - data.g2) * alpha;
			slot.b2 += (slot.b2 - data.b2) * alpha;
			slot.ClampSecondColor();
		}

		// Token: 0x04000A35 RID: 2613
		public const int ENTRIES = 8;

		// Token: 0x04000A36 RID: 2614
		protected const int PREV_TIME = -8;

		// Token: 0x04000A37 RID: 2615
		protected const int PREV_R = -7;

		// Token: 0x04000A38 RID: 2616
		protected const int PREV_G = -6;

		// Token: 0x04000A39 RID: 2617
		protected const int PREV_B = -5;

		// Token: 0x04000A3A RID: 2618
		protected const int PREV_A = -4;

		// Token: 0x04000A3B RID: 2619
		protected const int PREV_R2 = -3;

		// Token: 0x04000A3C RID: 2620
		protected const int PREV_G2 = -2;

		// Token: 0x04000A3D RID: 2621
		protected const int PREV_B2 = -1;

		// Token: 0x04000A3E RID: 2622
		protected const int R = 1;

		// Token: 0x04000A3F RID: 2623
		protected const int G = 2;

		// Token: 0x04000A40 RID: 2624
		protected const int B = 3;

		// Token: 0x04000A41 RID: 2625
		protected const int A = 4;

		// Token: 0x04000A42 RID: 2626
		protected const int R2 = 5;

		// Token: 0x04000A43 RID: 2627
		protected const int G2 = 6;

		// Token: 0x04000A44 RID: 2628
		protected const int B2 = 7;

		// Token: 0x04000A45 RID: 2629
		internal int slotIndex;

		// Token: 0x04000A46 RID: 2630
		internal float[] frames;
	}
}
