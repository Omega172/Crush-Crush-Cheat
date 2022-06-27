using System;

namespace Spine
{
	// Token: 0x02000178 RID: 376
	public class ShearTimeline : TranslateTimeline, IBoneTimeline
	{
		// Token: 0x06000AC0 RID: 2752 RVA: 0x00055FA8 File Offset: 0x000541A8
		public ShearTimeline(int frameCount) : base(frameCount)
		{
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00055FB4 File Offset: 0x000541B4
		public override int PropertyId
		{
			get
			{
				return 50331648 + this.boneIndex;
			}
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00055FC4 File Offset: 0x000541C4
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
					num = frames[frames.Length + -2];
					num2 = frames[frames.Length + -1];
				}
				else
				{
					int num3 = Animation.BinarySearch(frames, time, 3);
					num = frames[num3 + -2];
					num2 = frames[num3 + -1];
					float num4 = frames[num3];
					float curvePercent = base.GetCurvePercent(num3 / 3 - 1, 1f - (time - num4) / (frames[num3 + -3] - num4));
					num += (frames[num3 + 1] - num) * curvePercent;
					num2 += (frames[num3 + 2] - num2) * curvePercent;
				}
				switch (blend)
				{
				case MixBlend.Setup:
					bone.shearX = bone.data.shearX + num * alpha;
					bone.shearY = bone.data.shearY + num2 * alpha;
					break;
				case MixBlend.First:
				case MixBlend.Replace:
					bone.shearX += (bone.data.shearX + num - bone.shearX) * alpha;
					bone.shearY += (bone.data.shearY + num2 - bone.shearY) * alpha;
					break;
				case MixBlend.Add:
					bone.shearX += num * alpha;
					bone.shearY += num2 * alpha;
					break;
				}
				return;
			}
			if (blend == MixBlend.Setup)
			{
				bone.shearX = bone.data.shearX;
				bone.shearY = bone.data.shearY;
				return;
			}
			if (blend != MixBlend.First)
			{
				return;
			}
			bone.shearX += (bone.data.shearX - bone.shearX) * alpha;
			bone.shearY += (bone.data.shearY - bone.shearY) * alpha;
		}
	}
}
