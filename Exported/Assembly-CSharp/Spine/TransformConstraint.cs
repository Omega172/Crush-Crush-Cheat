using System;

namespace Spine
{
	// Token: 0x020001CE RID: 462
	public class TransformConstraint : IUpdatable
	{
		// Token: 0x06000EF6 RID: 3830 RVA: 0x0006B64C File Offset: 0x0006984C
		public TransformConstraint(TransformConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			this.data = data;
			this.rotateMix = data.rotateMix;
			this.translateMix = data.translateMix;
			this.scaleMix = data.scaleMix;
			this.shearMix = data.shearMix;
			this.bones = new ExposedList<Bone>();
			foreach (BoneData boneData in data.bones)
			{
				this.bones.Add(skeleton.FindBone(boneData.name));
			}
			this.target = skeleton.FindBone(data.target.name);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0006B750 File Offset: 0x00069950
		public TransformConstraint(TransformConstraint constraint, Skeleton skeleton)
		{
			if (constraint == null)
			{
				throw new ArgumentNullException("constraint cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			this.data = constraint.data;
			this.bones = new ExposedList<Bone>(constraint.Bones.Count);
			foreach (Bone bone in constraint.Bones)
			{
				this.bones.Add(skeleton.Bones.Items[bone.data.index]);
			}
			this.target = skeleton.Bones.Items[constraint.target.data.index];
			this.rotateMix = constraint.rotateMix;
			this.translateMix = constraint.translateMix;
			this.scaleMix = constraint.scaleMix;
			this.shearMix = constraint.shearMix;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0006B870 File Offset: 0x00069A70
		public void Apply()
		{
			this.Update();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0006B878 File Offset: 0x00069A78
		public void Update()
		{
			if (this.data.local)
			{
				if (this.data.relative)
				{
					this.ApplyRelativeLocal();
				}
				else
				{
					this.ApplyAbsoluteLocal();
				}
			}
			else if (this.data.relative)
			{
				this.ApplyRelativeWorld();
			}
			else
			{
				this.ApplyAbsoluteWorld();
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0006B8DC File Offset: 0x00069ADC
		private void ApplyAbsoluteWorld()
		{
			float num = this.rotateMix;
			float num2 = this.translateMix;
			float num3 = this.scaleMix;
			float num4 = this.shearMix;
			Bone bone = this.target;
			float a = bone.a;
			float b = bone.b;
			float c = bone.c;
			float d = bone.d;
			float num5 = (a * d - b * c <= 0f) ? -0.017453292f : 0.017453292f;
			float num6 = this.data.offsetRotation * num5;
			float num7 = this.data.offsetShearY * num5;
			ExposedList<Bone> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone2 = exposedList.Items[i];
				bool flag = false;
				if (num != 0f)
				{
					float a2 = bone2.a;
					float b2 = bone2.b;
					float c2 = bone2.c;
					float d2 = bone2.d;
					float num8 = MathUtils.Atan2(c, a) - MathUtils.Atan2(c2, a2) + num6;
					if (num8 > 3.1415927f)
					{
						num8 -= 6.2831855f;
					}
					else if (num8 < -3.1415927f)
					{
						num8 += 6.2831855f;
					}
					num8 *= num;
					float num9 = MathUtils.Cos(num8);
					float num10 = MathUtils.Sin(num8);
					bone2.a = num9 * a2 - num10 * c2;
					bone2.b = num9 * b2 - num10 * d2;
					bone2.c = num10 * a2 + num9 * c2;
					bone2.d = num10 * b2 + num9 * d2;
					flag = true;
				}
				if (num2 != 0f)
				{
					float num11;
					float num12;
					bone.LocalToWorld(this.data.offsetX, this.data.offsetY, out num11, out num12);
					bone2.worldX += (num11 - bone2.worldX) * num2;
					bone2.worldY += (num12 - bone2.worldY) * num2;
					flag = true;
				}
				if (num3 > 0f)
				{
					float num13 = (float)Math.Sqrt((double)(bone2.a * bone2.a + bone2.c * bone2.c));
					if (num13 != 0f)
					{
						num13 = (num13 + ((float)Math.Sqrt((double)(a * a + c * c)) - num13 + this.data.offsetScaleX) * num3) / num13;
					}
					bone2.a *= num13;
					bone2.c *= num13;
					num13 = (float)Math.Sqrt((double)(bone2.b * bone2.b + bone2.d * bone2.d));
					if (num13 != 0f)
					{
						num13 = (num13 + ((float)Math.Sqrt((double)(b * b + d * d)) - num13 + this.data.offsetScaleY) * num3) / num13;
					}
					bone2.b *= num13;
					bone2.d *= num13;
					flag = true;
				}
				if (num4 > 0f)
				{
					float b3 = bone2.b;
					float d3 = bone2.d;
					float num14 = MathUtils.Atan2(d3, b3);
					float num15 = MathUtils.Atan2(d, b) - MathUtils.Atan2(c, a) - (num14 - MathUtils.Atan2(bone2.c, bone2.a));
					if (num15 > 3.1415927f)
					{
						num15 -= 6.2831855f;
					}
					else if (num15 < -3.1415927f)
					{
						num15 += 6.2831855f;
					}
					num15 = num14 + (num15 + num7) * num4;
					float num16 = (float)Math.Sqrt((double)(b3 * b3 + d3 * d3));
					bone2.b = MathUtils.Cos(num15) * num16;
					bone2.d = MathUtils.Sin(num15) * num16;
					flag = true;
				}
				if (flag)
				{
					bone2.appliedValid = false;
				}
				i++;
			}
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0006BCD4 File Offset: 0x00069ED4
		private void ApplyRelativeWorld()
		{
			float num = this.rotateMix;
			float num2 = this.translateMix;
			float num3 = this.scaleMix;
			float num4 = this.shearMix;
			Bone bone = this.target;
			float a = bone.a;
			float b = bone.b;
			float c = bone.c;
			float d = bone.d;
			float num5 = (a * d - b * c <= 0f) ? -0.017453292f : 0.017453292f;
			float num6 = this.data.offsetRotation * num5;
			float num7 = this.data.offsetShearY * num5;
			ExposedList<Bone> exposedList = this.bones;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone2 = exposedList.Items[i];
				bool flag = false;
				if (num != 0f)
				{
					float a2 = bone2.a;
					float b2 = bone2.b;
					float c2 = bone2.c;
					float d2 = bone2.d;
					float num8 = MathUtils.Atan2(c, a) + num6;
					if (num8 > 3.1415927f)
					{
						num8 -= 6.2831855f;
					}
					else if (num8 < -3.1415927f)
					{
						num8 += 6.2831855f;
					}
					num8 *= num;
					float num9 = MathUtils.Cos(num8);
					float num10 = MathUtils.Sin(num8);
					bone2.a = num9 * a2 - num10 * c2;
					bone2.b = num9 * b2 - num10 * d2;
					bone2.c = num10 * a2 + num9 * c2;
					bone2.d = num10 * b2 + num9 * d2;
					flag = true;
				}
				if (num2 != 0f)
				{
					float num11;
					float num12;
					bone.LocalToWorld(this.data.offsetX, this.data.offsetY, out num11, out num12);
					bone2.worldX += num11 * num2;
					bone2.worldY += num12 * num2;
					flag = true;
				}
				if (num3 > 0f)
				{
					float num13 = ((float)Math.Sqrt((double)(a * a + c * c)) - 1f + this.data.offsetScaleX) * num3 + 1f;
					bone2.a *= num13;
					bone2.c *= num13;
					num13 = ((float)Math.Sqrt((double)(b * b + d * d)) - 1f + this.data.offsetScaleY) * num3 + 1f;
					bone2.b *= num13;
					bone2.d *= num13;
					flag = true;
				}
				if (num4 > 0f)
				{
					float num14 = MathUtils.Atan2(d, b) - MathUtils.Atan2(c, a);
					if (num14 > 3.1415927f)
					{
						num14 -= 6.2831855f;
					}
					else if (num14 < -3.1415927f)
					{
						num14 += 6.2831855f;
					}
					float b3 = bone2.b;
					float d3 = bone2.d;
					num14 = MathUtils.Atan2(d3, b3) + (num14 - 1.5707964f + num7) * num4;
					float num15 = (float)Math.Sqrt((double)(b3 * b3 + d3 * d3));
					bone2.b = MathUtils.Cos(num14) * num15;
					bone2.d = MathUtils.Sin(num14) * num15;
					flag = true;
				}
				if (flag)
				{
					bone2.appliedValid = false;
				}
				i++;
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0006C038 File Offset: 0x0006A238
		private void ApplyAbsoluteLocal()
		{
			float num = this.rotateMix;
			float num2 = this.translateMix;
			float num3 = this.scaleMix;
			float num4 = this.shearMix;
			Bone bone = this.target;
			if (!bone.appliedValid)
			{
				bone.UpdateAppliedTransform();
			}
			Bone[] items = this.bones.Items;
			int i = 0;
			int count = this.bones.Count;
			while (i < count)
			{
				Bone bone2 = items[i];
				if (!bone2.appliedValid)
				{
					bone2.UpdateAppliedTransform();
				}
				float num5 = bone2.arotation;
				if (num != 0f)
				{
					float num6 = bone.arotation - num5 + this.data.offsetRotation;
					num6 -= (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360);
					num5 += num6 * num;
				}
				float num7 = bone2.ax;
				float num8 = bone2.ay;
				if (num2 != 0f)
				{
					num7 += (bone.ax - num7 + this.data.offsetX) * num2;
					num8 += (bone.ay - num8 + this.data.offsetY) * num2;
				}
				float num9 = bone2.ascaleX;
				float num10 = bone2.ascaleY;
				if (num3 != 0f)
				{
					if (num9 != 0f)
					{
						num9 = (num9 + (bone.ascaleX - num9 + this.data.offsetScaleX) * num3) / num9;
					}
					if (num10 != 0f)
					{
						num10 = (num10 + (bone.ascaleY - num10 + this.data.offsetScaleY) * num3) / num10;
					}
				}
				float num11 = bone2.ashearY;
				if (num4 != 0f)
				{
					float num12 = bone.ashearY - num11 + this.data.offsetShearY;
					num12 -= (float)((16384 - (int)(16384.499999999996 - (double)(num12 / 360f))) * 360);
					num11 += num12 * num4;
				}
				bone2.UpdateWorldTransform(num7, num8, num5, num9, num10, bone2.ashearX, num11);
				i++;
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0006C264 File Offset: 0x0006A464
		private void ApplyRelativeLocal()
		{
			float num = this.rotateMix;
			float num2 = this.translateMix;
			float num3 = this.scaleMix;
			float num4 = this.shearMix;
			Bone bone = this.target;
			if (!bone.appliedValid)
			{
				bone.UpdateAppliedTransform();
			}
			Bone[] items = this.bones.Items;
			int i = 0;
			int count = this.bones.Count;
			while (i < count)
			{
				Bone bone2 = items[i];
				if (!bone2.appliedValid)
				{
					bone2.UpdateAppliedTransform();
				}
				float num5 = bone2.arotation;
				if (num != 0f)
				{
					num5 += (bone.arotation + this.data.offsetRotation) * num;
				}
				float num6 = bone2.ax;
				float num7 = bone2.ay;
				if (num2 != 0f)
				{
					num6 += (bone.ax + this.data.offsetX) * num2;
					num7 += (bone.ay + this.data.offsetY) * num2;
				}
				float num8 = bone2.ascaleX;
				float num9 = bone2.ascaleY;
				if (num3 != 0f)
				{
					num8 *= (bone.ascaleX - 1f + this.data.offsetScaleX) * num3 + 1f;
					num9 *= (bone.ascaleY - 1f + this.data.offsetScaleY) * num3 + 1f;
				}
				float num10 = bone2.ashearY;
				if (num4 != 0f)
				{
					num10 += (bone.ashearY + this.data.offsetShearY) * num4;
				}
				bone2.UpdateWorldTransform(num6, num7, num5, num8, num9, bone2.ashearX, num10);
				i++;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0006C424 File Offset: 0x0006A624
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0006C42C File Offset: 0x0006A62C
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0006C434 File Offset: 0x0006A634
		public Bone Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0006C440 File Offset: 0x0006A640
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0006C448 File Offset: 0x0006A648
		public float RotateMix
		{
			get
			{
				return this.rotateMix;
			}
			set
			{
				this.rotateMix = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0006C454 File Offset: 0x0006A654
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0006C45C File Offset: 0x0006A65C
		public float TranslateMix
		{
			get
			{
				return this.translateMix;
			}
			set
			{
				this.translateMix = value;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0006C468 File Offset: 0x0006A668
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0006C470 File Offset: 0x0006A670
		public float ScaleMix
		{
			get
			{
				return this.scaleMix;
			}
			set
			{
				this.scaleMix = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0006C47C File Offset: 0x0006A67C
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0006C484 File Offset: 0x0006A684
		public float ShearMix
		{
			get
			{
				return this.shearMix;
			}
			set
			{
				this.shearMix = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0006C490 File Offset: 0x0006A690
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0006C498 File Offset: 0x0006A698
		public TransformConstraintData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0006C4A0 File Offset: 0x0006A6A0
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x04000C77 RID: 3191
		internal TransformConstraintData data;

		// Token: 0x04000C78 RID: 3192
		internal ExposedList<Bone> bones;

		// Token: 0x04000C79 RID: 3193
		internal Bone target;

		// Token: 0x04000C7A RID: 3194
		internal float rotateMix;

		// Token: 0x04000C7B RID: 3195
		internal float translateMix;

		// Token: 0x04000C7C RID: 3196
		internal float scaleMix;

		// Token: 0x04000C7D RID: 3197
		internal float shearMix;

		// Token: 0x04000C7E RID: 3198
		internal bool active;
	}
}
