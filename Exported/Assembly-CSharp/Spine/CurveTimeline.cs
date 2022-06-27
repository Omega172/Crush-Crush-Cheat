using System;

namespace Spine
{
	// Token: 0x02000174 RID: 372
	public abstract class CurveTimeline : Timeline
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x00055324 File Offset: 0x00053524
		public CurveTimeline(int frameCount)
		{
			if (frameCount <= 0)
			{
				throw new ArgumentOutOfRangeException("frameCount must be > 0: ");
			}
			this.curves = new float[(frameCount - 1) * 19];
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00055350 File Offset: 0x00053550
		public int FrameCount
		{
			get
			{
				return this.curves.Length / 19 + 1;
			}
		}

		// Token: 0x06000AA6 RID: 2726
		public abstract void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction);

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000AA7 RID: 2727
		public abstract int PropertyId { get; }

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00055360 File Offset: 0x00053560
		public void SetLinear(int frameIndex)
		{
			this.curves[frameIndex * 19] = 0f;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00055374 File Offset: 0x00053574
		public void SetStepped(int frameIndex)
		{
			this.curves[frameIndex * 19] = 1f;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00055388 File Offset: 0x00053588
		public float GetCurveType(int frameIndex)
		{
			int num = frameIndex * 19;
			if (num == this.curves.Length)
			{
				return 0f;
			}
			float num2 = this.curves[num];
			if (num2 == 0f)
			{
				return 0f;
			}
			if (num2 == 1f)
			{
				return 1f;
			}
			return 2f;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x000553E0 File Offset: 0x000535E0
		public void SetCurve(int frameIndex, float cx1, float cy1, float cx2, float cy2)
		{
			float num = (-cx1 * 2f + cx2) * 0.03f;
			float num2 = (-cy1 * 2f + cy2) * 0.03f;
			float num3 = ((cx1 - cx2) * 3f + 1f) * 0.006f;
			float num4 = ((cy1 - cy2) * 3f + 1f) * 0.006f;
			float num5 = num * 2f + num3;
			float num6 = num2 * 2f + num4;
			float num7 = cx1 * 0.3f + num + num3 * 0.16666667f;
			float num8 = cy1 * 0.3f + num2 + num4 * 0.16666667f;
			int i = frameIndex * 19;
			float[] array = this.curves;
			array[i++] = 2f;
			float num9 = num7;
			float num10 = num8;
			int num11 = i + 19 - 1;
			while (i < num11)
			{
				array[i] = num9;
				array[i + 1] = num10;
				num7 += num5;
				num8 += num6;
				num5 += num3;
				num6 += num4;
				num9 += num7;
				num10 += num8;
				i += 2;
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000554F8 File Offset: 0x000536F8
		public float GetCurvePercent(int frameIndex, float percent)
		{
			percent = MathUtils.Clamp(percent, 0f, 1f);
			float[] array = this.curves;
			int i = frameIndex * 19;
			float num = array[i];
			if (num == 0f)
			{
				return percent;
			}
			if (num == 1f)
			{
				return 0f;
			}
			i++;
			float num2 = 0f;
			int num3 = i;
			int num4 = i + 19 - 1;
			while (i < num4)
			{
				num2 = array[i];
				if (num2 >= percent)
				{
					if (i == num3)
					{
						return array[i + 1] * percent / num2;
					}
					float num5 = array[i - 2];
					float num6 = array[i - 1];
					return num6 + (array[i + 1] - num6) * (percent - num5) / (num2 - num5);
				}
				else
				{
					i += 2;
				}
			}
			float num7 = array[i - 1];
			return num7 + (1f - num7) * (percent - num2) / (1f - num2);
		}

		// Token: 0x04000A16 RID: 2582
		protected const float LINEAR = 0f;

		// Token: 0x04000A17 RID: 2583
		protected const float STEPPED = 1f;

		// Token: 0x04000A18 RID: 2584
		protected const float BEZIER = 2f;

		// Token: 0x04000A19 RID: 2585
		protected const int BEZIER_SIZE = 19;

		// Token: 0x04000A1A RID: 2586
		internal float[] curves;
	}
}
