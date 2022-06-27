using System;

namespace Spine
{
	// Token: 0x02000175 RID: 373
	public class RotateTimeline : CurveTimeline, IBoneTimeline
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x000555CC File Offset: 0x000537CC
		public RotateTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount << 1];
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x000555E4 File Offset: 0x000537E4
		public override int PropertyId
		{
			get
			{
				return 0 + this.boneIndex;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0005560C File Offset: 0x0005380C
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x000555F0 File Offset: 0x000537F0
		public int BoneIndex
		{
			get
			{
				return this.boneIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.boneIndex = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00055614 File Offset: 0x00053814
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0005561C File Offset: 0x0005381C
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

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00055628 File Offset: 0x00053828
		public void SetFrame(int frameIndex, float time, float degrees)
		{
			frameIndex <<= 1;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = degrees;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00055644 File Offset: 0x00053844
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (!bone.active)
			{
				return;
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				if (blend == MixBlend.Setup)
				{
					bone.rotation = bone.data.rotation;
					return;
				}
				if (blend != MixBlend.First)
				{
					return;
				}
				float num = bone.data.rotation - bone.rotation;
				bone.rotation += (num - (float)((16384 - (int)(16384.499999999996 - (double)(num / 360f))) * 360)) * alpha;
				return;
			}
			else
			{
				if (time >= array[array.Length - 2])
				{
					float num2 = array[array.Length + -1];
					switch (blend)
					{
					case MixBlend.Setup:
						bone.rotation = bone.data.rotation + num2 * alpha;
						return;
					case MixBlend.First:
					case MixBlend.Replace:
						num2 += bone.data.rotation - bone.rotation;
						num2 -= (float)((16384 - (int)(16384.499999999996 - (double)(num2 / 360f))) * 360);
						break;
					case MixBlend.Add:
						break;
					default:
						return;
					}
					bone.rotation += num2 * alpha;
					return;
				}
				int num3 = Animation.BinarySearch(array, time, 2);
				float num4 = array[num3 + -1];
				float num5 = array[num3];
				float curvePercent = base.GetCurvePercent((num3 >> 1) - 1, 1f - (time - num5) / (array[num3 + -2] - num5));
				float num6 = array[num3 + 1] - num4;
				num6 = num4 + (num6 - (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360)) * curvePercent;
				switch (blend)
				{
				case MixBlend.Setup:
					bone.rotation = bone.data.rotation + (num6 - (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360)) * alpha;
					return;
				case MixBlend.First:
				case MixBlend.Replace:
					num6 += bone.data.rotation - bone.rotation;
					break;
				case MixBlend.Add:
					break;
				default:
					return;
				}
				bone.rotation += (num6 - (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360)) * alpha;
				return;
			}
		}

		// Token: 0x04000A1B RID: 2587
		public const int ENTRIES = 2;

		// Token: 0x04000A1C RID: 2588
		internal const int PREV_TIME = -2;

		// Token: 0x04000A1D RID: 2589
		internal const int PREV_ROTATION = -1;

		// Token: 0x04000A1E RID: 2590
		internal const int ROTATION = 1;

		// Token: 0x04000A1F RID: 2591
		internal int boneIndex;

		// Token: 0x04000A20 RID: 2592
		internal float[] frames;
	}
}
