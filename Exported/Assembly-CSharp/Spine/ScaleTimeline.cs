using System;

namespace Spine
{
	// Token: 0x02000177 RID: 375
	public class ScaleTimeline : TranslateTimeline, IBoneTimeline
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x00055B48 File Offset: 0x00053D48
		public ScaleTimeline(int frameCount) : base(frameCount)
		{
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00055B54 File Offset: 0x00053D54
		public override int PropertyId
		{
			get
			{
				return 33554432 + this.boneIndex;
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00055B64 File Offset: 0x00053D64
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Bone bone = skeleton.bones.Items[this.boneIndex];
			if (!bone.active)
			{
				return;
			}
			float[] frames = this.frames;
			if (time >= frames[0])
			{
				float num;
				float num2;
				if (time >= frames[frames.Length - 3])
				{
					num = frames[frames.Length + -2] * bone.data.scaleX;
					num2 = frames[frames.Length + -1] * bone.data.scaleY;
				}
				else
				{
					int num3 = Animation.BinarySearch(frames, time, 3);
					num = frames[num3 + -2];
					num2 = frames[num3 + -1];
					float num4 = frames[num3];
					float curvePercent = base.GetCurvePercent(num3 / 3 - 1, 1f - (time - num4) / (frames[num3 + -3] - num4));
					num = (num + (frames[num3 + 1] - num) * curvePercent) * bone.data.scaleX;
					num2 = (num2 + (frames[num3 + 2] - num2) * curvePercent) * bone.data.scaleY;
				}
				if (alpha == 1f)
				{
					if (blend == MixBlend.Add)
					{
						bone.scaleX += num - bone.data.scaleX;
						bone.scaleY += num2 - bone.data.scaleY;
					}
					else
					{
						bone.scaleX = num;
						bone.scaleY = num2;
					}
				}
				else if (direction == MixDirection.Out)
				{
					switch (blend)
					{
					case MixBlend.Setup:
					{
						float num5 = bone.data.scaleX;
						float num6 = bone.data.scaleY;
						bone.scaleX = num5 + (Math.Abs(num) * (float)Math.Sign(num5) - num5) * alpha;
						bone.scaleY = num6 + (Math.Abs(num2) * (float)Math.Sign(num6) - num6) * alpha;
						break;
					}
					case MixBlend.First:
					case MixBlend.Replace:
					{
						float num5 = bone.scaleX;
						float num6 = bone.scaleY;
						bone.scaleX = num5 + (Math.Abs(num) * (float)Math.Sign(num5) - num5) * alpha;
						bone.scaleY = num6 + (Math.Abs(num2) * (float)Math.Sign(num6) - num6) * alpha;
						break;
					}
					case MixBlend.Add:
					{
						float num5 = bone.scaleX;
						float num6 = bone.scaleY;
						bone.scaleX = num5 + (Math.Abs(num) * (float)Math.Sign(num5) - bone.data.scaleX) * alpha;
						bone.scaleY = num6 + (Math.Abs(num2) * (float)Math.Sign(num6) - bone.data.scaleY) * alpha;
						break;
					}
					}
				}
				else
				{
					switch (blend)
					{
					case MixBlend.Setup:
					{
						float num5 = Math.Abs(bone.data.scaleX) * (float)Math.Sign(num);
						float num6 = Math.Abs(bone.data.scaleY) * (float)Math.Sign(num2);
						bone.scaleX = num5 + (num - num5) * alpha;
						bone.scaleY = num6 + (num2 - num6) * alpha;
						break;
					}
					case MixBlend.First:
					case MixBlend.Replace:
					{
						float num5 = Math.Abs(bone.scaleX) * (float)Math.Sign(num);
						float num6 = Math.Abs(bone.scaleY) * (float)Math.Sign(num2);
						bone.scaleX = num5 + (num - num5) * alpha;
						bone.scaleY = num6 + (num2 - num6) * alpha;
						break;
					}
					case MixBlend.Add:
					{
						float num5 = (float)Math.Sign(num);
						float num6 = (float)Math.Sign(num2);
						bone.scaleX = Math.Abs(bone.scaleX) * num5 + (num - Math.Abs(bone.data.scaleX) * num5) * alpha;
						bone.scaleY = Math.Abs(bone.scaleY) * num6 + (num2 - Math.Abs(bone.data.scaleY) * num6) * alpha;
						break;
					}
					}
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				bone.scaleX = bone.data.scaleX;
				bone.scaleY = bone.data.scaleY;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			bone.scaleX += (bone.data.scaleX - bone.scaleX) * alpha;
			bone.scaleY += (bone.data.scaleY - bone.scaleY) * alpha;
		}
	}
}
