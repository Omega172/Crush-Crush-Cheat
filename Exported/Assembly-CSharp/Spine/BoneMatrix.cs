using System;

namespace Spine
{
	// Token: 0x0200022A RID: 554
	public struct BoneMatrix
	{
		// Token: 0x06001193 RID: 4499 RVA: 0x0007C444 File Offset: 0x0007A644
		public BoneMatrix(BoneData boneData)
		{
			float degrees = boneData.rotation + 90f + boneData.shearY;
			float degrees2 = boneData.rotation + boneData.shearX;
			this.a = MathUtils.CosDeg(degrees2) * boneData.scaleX;
			this.c = MathUtils.SinDeg(degrees2) * boneData.scaleX;
			this.b = MathUtils.CosDeg(degrees) * boneData.scaleY;
			this.d = MathUtils.SinDeg(degrees) * boneData.scaleY;
			this.x = boneData.x;
			this.y = boneData.y;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0007C4D8 File Offset: 0x0007A6D8
		public BoneMatrix(Bone bone)
		{
			float degrees = bone.rotation + 90f + bone.shearY;
			float degrees2 = bone.rotation + bone.shearX;
			this.a = MathUtils.CosDeg(degrees2) * bone.scaleX;
			this.c = MathUtils.SinDeg(degrees2) * bone.scaleX;
			this.b = MathUtils.CosDeg(degrees) * bone.scaleY;
			this.d = MathUtils.SinDeg(degrees) * bone.scaleY;
			this.x = bone.x;
			this.y = bone.y;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0007C56C File Offset: 0x0007A76C
		public static BoneMatrix CalculateSetupWorld(BoneData boneData)
		{
			if (boneData == null)
			{
				return default(BoneMatrix);
			}
			if (boneData.parent == null)
			{
				return BoneMatrix.GetInheritedInternal(boneData, default(BoneMatrix));
			}
			BoneMatrix parentMatrix = BoneMatrix.CalculateSetupWorld(boneData.parent);
			return BoneMatrix.GetInheritedInternal(boneData, parentMatrix);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0007C5B8 File Offset: 0x0007A7B8
		private static BoneMatrix GetInheritedInternal(BoneData boneData, BoneMatrix parentMatrix)
		{
			if (boneData.parent == null)
			{
				return new BoneMatrix(boneData);
			}
			float num = parentMatrix.a;
			float num2 = parentMatrix.b;
			float num3 = parentMatrix.c;
			float num4 = parentMatrix.d;
			BoneMatrix result = default(BoneMatrix);
			result.x = num * boneData.x + num2 * boneData.y + parentMatrix.x;
			result.y = num3 * boneData.x + num4 * boneData.y + parentMatrix.y;
			switch (boneData.transformMode)
			{
			case TransformMode.Normal:
			{
				float degrees = boneData.rotation + 90f + boneData.shearY;
				float num5 = MathUtils.CosDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
				float num6 = MathUtils.CosDeg(degrees) * boneData.scaleY;
				float num7 = MathUtils.SinDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
				float num8 = MathUtils.SinDeg(degrees) * boneData.scaleY;
				result.a = num * num5 + num2 * num7;
				result.b = num * num6 + num2 * num8;
				result.c = num3 * num5 + num4 * num7;
				result.d = num3 * num6 + num4 * num8;
				break;
			}
			case TransformMode.NoRotationOrReflection:
			{
				float num9 = num * num + num3 * num3;
				float num10;
				if (num9 > 0.0001f)
				{
					num9 = Math.Abs(num * num4 - num2 * num3) / num9;
					num2 = num3 * num9;
					num4 = num * num9;
					num10 = MathUtils.Atan2(num3, num) * 57.295776f;
				}
				else
				{
					num = 0f;
					num3 = 0f;
					num10 = 90f - MathUtils.Atan2(num4, num2) * 57.295776f;
				}
				float degrees2 = boneData.rotation + boneData.shearX - num10;
				float degrees3 = boneData.rotation + boneData.shearY - num10 + 90f;
				float num11 = MathUtils.CosDeg(degrees2) * boneData.scaleX;
				float num12 = MathUtils.CosDeg(degrees3) * boneData.scaleY;
				float num13 = MathUtils.SinDeg(degrees2) * boneData.scaleX;
				float num14 = MathUtils.SinDeg(degrees3) * boneData.scaleY;
				result.a = num * num11 - num2 * num13;
				result.b = num * num12 - num2 * num14;
				result.c = num3 * num11 + num4 * num13;
				result.d = num3 * num12 + num4 * num14;
				break;
			}
			case TransformMode.NoScale:
			case TransformMode.NoScaleOrReflection:
			{
				float num15 = MathUtils.CosDeg(boneData.rotation);
				float num16 = MathUtils.SinDeg(boneData.rotation);
				float num17 = num * num15 + num2 * num16;
				float num18 = num3 * num15 + num4 * num16;
				float num19 = (float)Math.Sqrt((double)(num17 * num17 + num18 * num18));
				if (num19 > 1E-05f)
				{
					num19 = 1f / num19;
				}
				num17 *= num19;
				num18 *= num19;
				num19 = (float)Math.Sqrt((double)(num17 * num17 + num18 * num18));
				float radians = 1.5707964f + MathUtils.Atan2(num18, num17);
				float num20 = MathUtils.Cos(radians) * num19;
				float num21 = MathUtils.Sin(radians) * num19;
				float num22 = MathUtils.CosDeg(boneData.shearX) * boneData.scaleX;
				float num23 = MathUtils.CosDeg(90f + boneData.shearY) * boneData.scaleY;
				float num24 = MathUtils.SinDeg(boneData.shearX) * boneData.scaleX;
				float num25 = MathUtils.SinDeg(90f + boneData.shearY) * boneData.scaleY;
				if (boneData.transformMode != TransformMode.NoScaleOrReflection && num * num4 - num2 * num3 < 0f)
				{
					num20 = -num20;
					num21 = -num21;
				}
				result.a = num17 * num22 + num20 * num24;
				result.b = num17 * num23 + num20 * num25;
				result.c = num18 * num22 + num21 * num24;
				result.d = num18 * num23 + num21 * num25;
				break;
			}
			case TransformMode.OnlyTranslation:
			{
				float degrees4 = boneData.rotation + 90f + boneData.shearY;
				result.a = MathUtils.CosDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
				result.b = MathUtils.CosDeg(degrees4) * boneData.scaleY;
				result.c = MathUtils.SinDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
				result.d = MathUtils.SinDeg(degrees4) * boneData.scaleY;
				break;
			}
			}
			return result;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0007CA4C File Offset: 0x0007AC4C
		public BoneMatrix TransformMatrix(BoneMatrix local)
		{
			return new BoneMatrix
			{
				a = this.a * local.a + this.b * local.c,
				b = this.a * local.b + this.b * local.d,
				c = this.c * local.a + this.d * local.c,
				d = this.c * local.b + this.d * local.d,
				x = this.a * local.x + this.b * local.y + this.x,
				y = this.c * local.x + this.d * local.y + this.y
			};
		}

		// Token: 0x04000E3A RID: 3642
		public float a;

		// Token: 0x04000E3B RID: 3643
		public float b;

		// Token: 0x04000E3C RID: 3644
		public float c;

		// Token: 0x04000E3D RID: 3645
		public float d;

		// Token: 0x04000E3E RID: 3646
		public float x;

		// Token: 0x04000E3F RID: 3647
		public float y;
	}
}
