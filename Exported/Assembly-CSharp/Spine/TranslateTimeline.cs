using System;

namespace Spine
{
	// Token: 0x02000176 RID: 374
	public class TranslateTimeline : CurveTimeline, IBoneTimeline
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x000558BC File Offset: 0x00053ABC
		public TranslateTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 3];
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x000558D4 File Offset: 0x00053AD4
		public override int PropertyId
		{
			get
			{
				return 16777216 + this.boneIndex;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00055900 File Offset: 0x00053B00
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x000558E4 File Offset: 0x00053AE4
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

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00055908 File Offset: 0x00053B08
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00055910 File Offset: 0x00053B10
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

		// Token: 0x06000ABB RID: 2747 RVA: 0x0005591C File Offset: 0x00053B1C
		public void SetFrame(int frameIndex, float time, float x, float y)
		{
			frameIndex *= 3;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = x;
			this.frames[frameIndex + 2] = y;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00055944 File Offset: 0x00053B44
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (!bone.active)
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
				switch (blend)
				{
				case MixBlend.Setup:
					bone.x = bone.data.x + num * alpha;
					bone.y = bone.data.y + num2 * alpha;
					break;
				case MixBlend.First:
				case MixBlend.Replace:
					bone.x += (bone.data.x + num - bone.x) * alpha;
					bone.y += (bone.data.y + num2 - bone.y) * alpha;
					break;
				case MixBlend.Add:
					bone.x += num * alpha;
					bone.y += num2 * alpha;
					break;
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				bone.x = bone.data.x;
				bone.y = bone.data.y;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			bone.x += (bone.data.x - bone.x) * alpha;
			bone.y += (bone.data.y - bone.y) * alpha;
		}

		// Token: 0x04000A21 RID: 2593
		public const int ENTRIES = 3;

		// Token: 0x04000A22 RID: 2594
		protected const int PREV_TIME = -3;

		// Token: 0x04000A23 RID: 2595
		protected const int PREV_X = -2;

		// Token: 0x04000A24 RID: 2596
		protected const int PREV_Y = -1;

		// Token: 0x04000A25 RID: 2597
		protected const int X = 1;

		// Token: 0x04000A26 RID: 2598
		protected const int Y = 2;

		// Token: 0x04000A27 RID: 2599
		internal int boneIndex;

		// Token: 0x04000A28 RID: 2600
		internal float[] frames;
	}
}
