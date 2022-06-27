using System;

namespace Spine
{
	// Token: 0x02000179 RID: 377
	public class ColorTimeline : CurveTimeline, ISlotTimeline
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x000561C8 File Offset: 0x000543C8
		public ColorTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 5];
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x000561E0 File Offset: 0x000543E0
		public override int PropertyId
		{
			get
			{
				return 83886080 + this.slotIndex;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0005620C File Offset: 0x0005440C
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x000561F0 File Offset: 0x000543F0
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00056214 File Offset: 0x00054414
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x0005621C File Offset: 0x0005441C
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

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00056228 File Offset: 0x00054428
		public void SetFrame(int frameIndex, float time, float r, float g, float b, float a)
		{
			frameIndex *= 5;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = r;
			this.frames[frameIndex + 2] = g;
			this.frames[frameIndex + 3] = b;
			this.frames[frameIndex + 4] = a;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00056268 File Offset: 0x00054468
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
				if (alpha == 1f)
				{
					slot.r = num2;
					slot.g = num3;
					slot.b = num4;
					slot.a = num5;
					slot.ClampColor();
				}
				else
				{
					float r;
					float g;
					float b;
					float a;
					if (blend == MixBlend.Setup)
					{
						r = slot.data.r;
						g = slot.data.g;
						b = slot.data.b;
						a = slot.data.a;
					}
					else
					{
						r = slot.r;
						g = slot.g;
						b = slot.b;
						a = slot.a;
					}
					slot.r = r + (num2 - r) * alpha;
					slot.g = g + (num3 - g) * alpha;
					slot.b = b + (num4 - b) * alpha;
					slot.a = a + (num5 - a) * alpha;
					slot.ClampColor();
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
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			slot.r += (data.r - slot.r) * alpha;
			slot.g += (data.g - slot.g) * alpha;
			slot.b += (data.b - slot.b) * alpha;
			slot.a += (data.a - slot.a) * alpha;
			slot.ClampColor();
		}

		// Token: 0x04000A29 RID: 2601
		public const int ENTRIES = 5;

		// Token: 0x04000A2A RID: 2602
		protected const int PREV_TIME = -5;

		// Token: 0x04000A2B RID: 2603
		protected const int PREV_R = -4;

		// Token: 0x04000A2C RID: 2604
		protected const int PREV_G = -3;

		// Token: 0x04000A2D RID: 2605
		protected const int PREV_B = -2;

		// Token: 0x04000A2E RID: 2606
		protected const int PREV_A = -1;

		// Token: 0x04000A2F RID: 2607
		protected const int R = 1;

		// Token: 0x04000A30 RID: 2608
		protected const int G = 2;

		// Token: 0x04000A31 RID: 2609
		protected const int B = 3;

		// Token: 0x04000A32 RID: 2610
		protected const int A = 4;

		// Token: 0x04000A33 RID: 2611
		internal int slotIndex;

		// Token: 0x04000A34 RID: 2612
		internal float[] frames;
	}
}
