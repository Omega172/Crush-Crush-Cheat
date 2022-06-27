using System;

namespace Spine
{
	// Token: 0x020001B0 RID: 432
	public class IkConstraint : IUpdatable
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x0005F180 File Offset: 0x0005D380
		public IkConstraint(IkConstraintData data, Skeleton skeleton)
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
			this.mix = data.mix;
			this.softness = data.softness;
			this.bendDirection = data.bendDirection;
			this.compress = data.compress;
			this.stretch = data.stretch;
			this.bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				this.bones.Add(skeleton.FindBone(boneData.name));
			}
			this.target = skeleton.FindBone(data.target.name);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0005F2B0 File Offset: 0x0005D4B0
		public IkConstraint(IkConstraint constraint, Skeleton skeleton)
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
			this.mix = constraint.mix;
			this.softness = constraint.softness;
			this.bendDirection = constraint.bendDirection;
			this.compress = constraint.compress;
			this.stretch = constraint.stretch;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0005F3F0 File Offset: 0x0005D5F0
		public void Apply()
		{
			this.Update();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0005F3F8 File Offset: 0x0005D5F8
		public void Update()
		{
			Bone bone = this.target;
			ExposedList<Bone> exposedList = this.bones;
			int count = exposedList.Count;
			if (count != 1)
			{
				if (count == 2)
				{
					IkConstraint.Apply(exposedList.Items[0], exposedList.Items[1], bone.worldX, bone.worldY, this.bendDirection, this.stretch, this.softness, this.mix);
				}
			}
			else
			{
				IkConstraint.Apply(exposedList.Items[0], bone.worldX, bone.worldY, this.compress, this.stretch, this.data.uniform, this.mix);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x0005F4A8 File Offset: 0x0005D6A8
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		// (set) Token: 0x06000D6A RID: 3434 RVA: 0x0005F4B8 File Offset: 0x0005D6B8
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0005F4C4 File Offset: 0x0005D6C4
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x0005F4CC File Offset: 0x0005D6CC
		public float Mix
		{
			get
			{
				return this.mix;
			}
			set
			{
				this.mix = value;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0005F4D8 File Offset: 0x0005D6D8
		// (set) Token: 0x06000D6E RID: 3438 RVA: 0x0005F4E0 File Offset: 0x0005D6E0
		public float Softness
		{
			get
			{
				return this.softness;
			}
			set
			{
				this.softness = value;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0005F4EC File Offset: 0x0005D6EC
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x0005F4F4 File Offset: 0x0005D6F4
		public int BendDirection
		{
			get
			{
				return this.bendDirection;
			}
			set
			{
				this.bendDirection = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0005F500 File Offset: 0x0005D700
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x0005F508 File Offset: 0x0005D708
		public bool Compress
		{
			get
			{
				return this.compress;
			}
			set
			{
				this.compress = value;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0005F514 File Offset: 0x0005D714
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x0005F51C File Offset: 0x0005D71C
		public bool Stretch
		{
			get
			{
				return this.stretch;
			}
			set
			{
				this.stretch = value;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0005F528 File Offset: 0x0005D728
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0005F530 File Offset: 0x0005D730
		public IkConstraintData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0005F538 File Offset: 0x0005D738
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0005F548 File Offset: 0x0005D748
		public static void Apply(Bone bone, float targetX, float targetY, bool compress, bool stretch, bool uniform, float alpha)
		{
			if (!bone.appliedValid)
			{
				bone.UpdateAppliedTransform();
			}
			Bone parent = bone.parent;
			float a = parent.a;
			float num = parent.b;
			float c = parent.c;
			float num2 = parent.d;
			float num3 = -bone.ashearX - bone.arotation;
			TransformMode transformMode = bone.data.transformMode;
			float num4;
			float num5;
			if (transformMode != TransformMode.NoRotationOrReflection)
			{
				if (transformMode == TransformMode.OnlyTranslation)
				{
					num4 = targetX - bone.worldX;
					num5 = targetY - bone.worldY;
					goto IL_151;
				}
			}
			else
			{
				float num6 = Math.Abs(a * num2 - num * c) / (a * a + c * c);
				float num7 = a / bone.skeleton.ScaleX;
				float num8 = c / bone.skeleton.ScaleY;
				num = -num8 * num6 * bone.skeleton.ScaleX;
				num2 = num7 * num6 * bone.skeleton.ScaleY;
				num3 += (float)Math.Atan2((double)c, (double)a) * 57.295776f;
			}
			float num9 = targetX - parent.worldX;
			float num10 = targetY - parent.worldY;
			float num11 = a * num2 - num * c;
			num4 = (num9 * num2 - num10 * num) / num11 - bone.ax;
			num5 = (num10 * a - num9 * c) / num11 - bone.ay;
			IL_151:
			num3 += (float)Math.Atan2((double)num5, (double)num4) * 57.295776f;
			if (bone.ascaleX < 0f)
			{
				num3 += 180f;
			}
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			else if (num3 < -180f)
			{
				num3 += 360f;
			}
			float num12 = bone.ascaleX;
			float num13 = bone.ascaleY;
			if (compress || stretch)
			{
				transformMode = bone.data.transformMode;
				if (transformMode != TransformMode.NoScale)
				{
					if (transformMode == TransformMode.NoScaleOrReflection)
					{
						num4 = targetX - bone.worldX;
						num5 = targetY - bone.worldY;
					}
				}
				else
				{
					num4 = targetX - bone.worldX;
					num5 = targetY - bone.worldY;
				}
				float num14 = bone.data.length * num12;
				float num15 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
				if ((compress && num15 < num14) || (stretch && num15 > num14 && num14 > 0.0001f))
				{
					float num16 = (num15 / num14 - 1f) * alpha + 1f;
					num12 *= num16;
					if (uniform)
					{
						num13 *= num16;
					}
				}
			}
			bone.UpdateWorldTransform(bone.ax, bone.ay, bone.arotation + num3 * alpha, num12, num13, bone.ashearX, bone.ashearY);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0005F824 File Offset: 0x0005DA24
		public static void Apply(Bone parent, Bone child, float targetX, float targetY, int bendDir, bool stretch, float softness, float alpha)
		{
			if (alpha == 0f)
			{
				child.UpdateWorldTransform();
				return;
			}
			if (!parent.appliedValid)
			{
				parent.UpdateAppliedTransform();
			}
			if (!child.appliedValid)
			{
				child.UpdateAppliedTransform();
			}
			float ax = parent.ax;
			float ay = parent.ay;
			float num = parent.ascaleX;
			float num2 = num;
			float num3 = parent.ascaleY;
			float num4 = child.ascaleX;
			int num5;
			int num6;
			if (num < 0f)
			{
				num = -num;
				num5 = 180;
				num6 = -1;
			}
			else
			{
				num5 = 0;
				num6 = 1;
			}
			if (num3 < 0f)
			{
				num3 = -num3;
				num6 = -num6;
			}
			int num7;
			if (num4 < 0f)
			{
				num4 = -num4;
				num7 = 180;
			}
			else
			{
				num7 = 0;
			}
			float ax2 = child.ax;
			float num8 = parent.a;
			float num9 = parent.b;
			float num10 = parent.c;
			float num11 = parent.d;
			bool flag = Math.Abs(num - num3) <= 0.0001f;
			float num12;
			float num13;
			float num14;
			if (!flag)
			{
				num12 = 0f;
				num13 = num8 * ax2 + parent.worldX;
				num14 = num10 * ax2 + parent.worldY;
			}
			else
			{
				num12 = child.ay;
				num13 = num8 * ax2 + num9 * num12 + parent.worldX;
				num14 = num10 * ax2 + num11 * num12 + parent.worldY;
			}
			Bone parent2 = parent.parent;
			num8 = parent2.a;
			num9 = parent2.b;
			num10 = parent2.c;
			num11 = parent2.d;
			float num15 = 1f / (num8 * num11 - num9 * num10);
			float num16 = num13 - parent2.worldX;
			float num17 = num14 - parent2.worldY;
			float num18 = (num16 * num11 - num17 * num9) * num15 - ax;
			float num19 = (num17 * num8 - num16 * num10) * num15 - ay;
			float num20 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
			float num21 = child.data.length * num4;
			if (num20 < 0.0001f)
			{
				IkConstraint.Apply(parent, targetX, targetY, false, stretch, false, alpha);
				child.UpdateWorldTransform(ax2, num12, 0f, child.ascaleX, child.ascaleY, child.ashearX, child.ashearY);
				return;
			}
			num16 = targetX - parent2.worldX;
			num17 = targetY - parent2.worldY;
			float num22 = (num16 * num11 - num17 * num9) * num15 - ax;
			float num23 = (num17 * num8 - num16 * num10) * num15 - ay;
			float num24 = num22 * num22 + num23 * num23;
			if (softness != 0f)
			{
				softness *= num * (num4 + 1f) / 2f;
				float num25 = (float)Math.Sqrt((double)num24);
				float num26 = num25 - num20 - num21 * num + softness;
				if (num26 > 0f)
				{
					float num27 = Math.Min(1f, num26 / (softness * 2f)) - 1f;
					num27 = (num26 - softness * (1f - num27 * num27)) / num25;
					num22 -= num27 * num22;
					num23 -= num27 * num23;
					num24 = num22 * num22 + num23 * num23;
				}
			}
			float num29;
			float num30;
			if (flag)
			{
				num21 *= num;
				float num28 = (num24 - num20 * num20 - num21 * num21) / (2f * num20 * num21);
				if (num28 < -1f)
				{
					num28 = -1f;
				}
				else if (num28 > 1f)
				{
					num28 = 1f;
					if (stretch)
					{
						num2 *= ((float)Math.Sqrt((double)num24) / (num20 + num21) - 1f) * alpha + 1f;
					}
				}
				num29 = (float)Math.Acos((double)num28) * (float)bendDir;
				num8 = num20 + num21 * num28;
				num9 = num21 * (float)Math.Sin((double)num29);
				num30 = (float)Math.Atan2((double)(num23 * num8 - num22 * num9), (double)(num22 * num8 + num23 * num9));
			}
			else
			{
				num8 = num * num21;
				num9 = num3 * num21;
				float num31 = num8 * num8;
				float num32 = num9 * num9;
				float num33 = (float)Math.Atan2((double)num23, (double)num22);
				num10 = num32 * num20 * num20 + num31 * num24 - num31 * num32;
				float num34 = -2f * num32 * num20;
				float num35 = num32 - num31;
				num11 = num34 * num34 - 4f * num35 * num10;
				if (num11 >= 0f)
				{
					float num36 = (float)Math.Sqrt((double)num11);
					if (num34 < 0f)
					{
						num36 = -num36;
					}
					num36 = -(num34 + num36) / 2f;
					float num37 = num36 / num35;
					float num38 = num10 / num36;
					float num39 = (Math.Abs(num37) >= Math.Abs(num38)) ? num38 : num37;
					if (num39 * num39 <= num24)
					{
						num17 = (float)Math.Sqrt((double)(num24 - num39 * num39)) * (float)bendDir;
						num30 = num33 - (float)Math.Atan2((double)num17, (double)num39);
						num29 = (float)Math.Atan2((double)(num17 / num3), (double)((num39 - num20) / num));
						goto IL_609;
					}
				}
				float num40 = 3.1415927f;
				float num41 = num20 - num8;
				float num42 = num41 * num41;
				float num43 = 0f;
				float num44 = 0f;
				float num45 = num20 + num8;
				float num46 = num45 * num45;
				float num47 = 0f;
				num10 = -num8 * num20 / (num31 - num32);
				if (num10 >= -1f && num10 <= 1f)
				{
					num10 = (float)Math.Acos((double)num10);
					num16 = num8 * (float)Math.Cos((double)num10) + num20;
					num17 = num9 * (float)Math.Sin((double)num10);
					num11 = num16 * num16 + num17 * num17;
					if (num11 < num42)
					{
						num40 = num10;
						num42 = num11;
						num41 = num16;
						num43 = num17;
					}
					if (num11 > num46)
					{
						num44 = num10;
						num46 = num11;
						num45 = num16;
						num47 = num17;
					}
				}
				if (num24 <= (num42 + num46) / 2f)
				{
					num30 = num33 - (float)Math.Atan2((double)(num43 * (float)bendDir), (double)num41);
					num29 = num40 * (float)bendDir;
				}
				else
				{
					num30 = num33 - (float)Math.Atan2((double)(num47 * (float)bendDir), (double)num45);
					num29 = num44 * (float)bendDir;
				}
			}
			IL_609:
			float num48 = (float)Math.Atan2((double)num12, (double)ax2) * (float)num6;
			float arotation = parent.arotation;
			num30 = (num30 - num48) * 57.295776f + (float)num5 - arotation;
			if (num30 > 180f)
			{
				num30 -= 360f;
			}
			else if (num30 < -180f)
			{
				num30 += 360f;
			}
			parent.UpdateWorldTransform(ax, ay, arotation + num30 * alpha, num2, parent.ascaleY, 0f, 0f);
			arotation = child.arotation;
			num29 = ((num29 + num48) * 57.295776f - child.ashearX) * (float)num6 + (float)num7 - arotation;
			if (num29 > 180f)
			{
				num29 -= 360f;
			}
			else if (num29 < -180f)
			{
				num29 += 360f;
			}
			child.UpdateWorldTransform(ax2, num12, arotation + num29 * alpha, child.ascaleX, child.ascaleY, child.ashearX, child.ashearY);
		}

		// Token: 0x04000BA6 RID: 2982
		internal IkConstraintData data;

		// Token: 0x04000BA7 RID: 2983
		internal ExposedList<Bone> bones = new ExposedList<Bone>();

		// Token: 0x04000BA8 RID: 2984
		internal Bone target;

		// Token: 0x04000BA9 RID: 2985
		internal int bendDirection;

		// Token: 0x04000BAA RID: 2986
		internal bool compress;

		// Token: 0x04000BAB RID: 2987
		internal bool stretch;

		// Token: 0x04000BAC RID: 2988
		internal float mix = 1f;

		// Token: 0x04000BAD RID: 2989
		internal float softness;

		// Token: 0x04000BAE RID: 2990
		internal bool active;
	}
}
