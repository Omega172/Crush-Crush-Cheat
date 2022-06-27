using System;

namespace Spine
{
	// Token: 0x020001A2 RID: 418
	public class Bone : IUpdatable
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x0005C85C File Offset: 0x0005AA5C
		public Bone(BoneData data, Skeleton skeleton, Bone parent)
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
			this.skeleton = skeleton;
			this.parent = parent;
			this.SetToSetupPose();
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0005C8C4 File Offset: 0x0005AAC4
		public BoneData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0005C8CC File Offset: 0x0005AACC
		public Skeleton Skeleton
		{
			get
			{
				return this.skeleton;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0005C8D4 File Offset: 0x0005AAD4
		public Bone Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0005C8DC File Offset: 0x0005AADC
		public ExposedList<Bone> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0005C8E4 File Offset: 0x0005AAE4
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0005C8EC File Offset: 0x0005AAEC
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x0005C8F4 File Offset: 0x0005AAF4
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0005C900 File Offset: 0x0005AB00
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x0005C908 File Offset: 0x0005AB08
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0005C914 File Offset: 0x0005AB14
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x0005C91C File Offset: 0x0005AB1C
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0005C928 File Offset: 0x0005AB28
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x0005C930 File Offset: 0x0005AB30
		public float ScaleX
		{
			get
			{
				return this.scaleX;
			}
			set
			{
				this.scaleX = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x0005C93C File Offset: 0x0005AB3C
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x0005C944 File Offset: 0x0005AB44
		public float ScaleY
		{
			get
			{
				return this.scaleY;
			}
			set
			{
				this.scaleY = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0005C950 File Offset: 0x0005AB50
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0005C958 File Offset: 0x0005AB58
		public float ShearX
		{
			get
			{
				return this.shearX;
			}
			set
			{
				this.shearX = value;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0005C964 File Offset: 0x0005AB64
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0005C96C File Offset: 0x0005AB6C
		public float ShearY
		{
			get
			{
				return this.shearY;
			}
			set
			{
				this.shearY = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0005C978 File Offset: 0x0005AB78
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x0005C980 File Offset: 0x0005AB80
		public float AppliedRotation
		{
			get
			{
				return this.arotation;
			}
			set
			{
				this.arotation = value;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0005C98C File Offset: 0x0005AB8C
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x0005C994 File Offset: 0x0005AB94
		public float AX
		{
			get
			{
				return this.ax;
			}
			set
			{
				this.ax = value;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0005C9A0 File Offset: 0x0005ABA0
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0005C9A8 File Offset: 0x0005ABA8
		public float AY
		{
			get
			{
				return this.ay;
			}
			set
			{
				this.ay = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0005C9B4 File Offset: 0x0005ABB4
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x0005C9BC File Offset: 0x0005ABBC
		public float AScaleX
		{
			get
			{
				return this.ascaleX;
			}
			set
			{
				this.ascaleX = value;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0005C9C8 File Offset: 0x0005ABC8
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x0005C9D0 File Offset: 0x0005ABD0
		public float AScaleY
		{
			get
			{
				return this.ascaleY;
			}
			set
			{
				this.ascaleY = value;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0005C9DC File Offset: 0x0005ABDC
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x0005C9E4 File Offset: 0x0005ABE4
		public float AShearX
		{
			get
			{
				return this.ashearX;
			}
			set
			{
				this.ashearX = value;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0005C9F0 File Offset: 0x0005ABF0
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0005C9F8 File Offset: 0x0005ABF8
		public float AShearY
		{
			get
			{
				return this.ashearY;
			}
			set
			{
				this.ashearY = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0005CA04 File Offset: 0x0005AC04
		public float A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		public float B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0005CA14 File Offset: 0x0005AC14
		public float C
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0005CA1C File Offset: 0x0005AC1C
		public float D
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0005CA24 File Offset: 0x0005AC24
		public float WorldX
		{
			get
			{
				return this.worldX;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0005CA2C File Offset: 0x0005AC2C
		public float WorldY
		{
			get
			{
				return this.worldY;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0005CA34 File Offset: 0x0005AC34
		public float WorldRotationX
		{
			get
			{
				return MathUtils.Atan2(this.c, this.a) * 57.295776f;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0005CA50 File Offset: 0x0005AC50
		public float WorldRotationY
		{
			get
			{
				return MathUtils.Atan2(this.d, this.b) * 57.295776f;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0005CA6C File Offset: 0x0005AC6C
		public float WorldScaleX
		{
			get
			{
				return (float)Math.Sqrt((double)(this.a * this.a + this.c * this.c));
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0005CA9C File Offset: 0x0005AC9C
		public float WorldScaleY
		{
			get
			{
				return (float)Math.Sqrt((double)(this.b * this.b + this.d * this.d));
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0005CACC File Offset: 0x0005ACCC
		public void Update()
		{
			this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0005CB0C File Offset: 0x0005AD0C
		public void UpdateWorldTransform()
		{
			this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0005CB4C File Offset: 0x0005AD4C
		public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY)
		{
			this.ax = x;
			this.ay = y;
			this.arotation = rotation;
			this.ascaleX = scaleX;
			this.ascaleY = scaleY;
			this.ashearX = shearX;
			this.ashearY = shearY;
			this.appliedValid = true;
			Skeleton skeleton = this.skeleton;
			Bone bone = this.parent;
			if (bone == null)
			{
				float degrees = rotation + 90f + shearY;
				float num = skeleton.ScaleX;
				float num2 = skeleton.ScaleY;
				this.a = MathUtils.CosDeg(rotation + shearX) * scaleX * num;
				this.b = MathUtils.CosDeg(degrees) * scaleY * num;
				this.c = MathUtils.SinDeg(rotation + shearX) * scaleX * num2;
				this.d = MathUtils.SinDeg(degrees) * scaleY * num2;
				this.worldX = x * num + skeleton.x;
				this.worldY = y * num2 + skeleton.y;
				return;
			}
			float num3 = bone.a;
			float num4 = bone.b;
			float num5 = bone.c;
			float num6 = bone.d;
			this.worldX = num3 * x + num4 * y + bone.worldX;
			this.worldY = num5 * x + num6 * y + bone.worldY;
			switch (this.data.transformMode)
			{
			case TransformMode.Normal:
			{
				float degrees2 = rotation + 90f + shearY;
				float num7 = MathUtils.CosDeg(rotation + shearX) * scaleX;
				float num8 = MathUtils.CosDeg(degrees2) * scaleY;
				float num9 = MathUtils.SinDeg(rotation + shearX) * scaleX;
				float num10 = MathUtils.SinDeg(degrees2) * scaleY;
				this.a = num3 * num7 + num4 * num9;
				this.b = num3 * num8 + num4 * num10;
				this.c = num5 * num7 + num6 * num9;
				this.d = num5 * num8 + num6 * num10;
				return;
			}
			case TransformMode.NoRotationOrReflection:
			{
				float num11 = num3 * num3 + num5 * num5;
				float num12;
				if (num11 > 0.0001f)
				{
					num11 = Math.Abs(num3 * num6 - num4 * num5) / num11;
					num3 /= skeleton.ScaleX;
					num5 /= skeleton.ScaleY;
					num4 = num5 * num11;
					num6 = num3 * num11;
					num12 = MathUtils.Atan2(num5, num3) * 57.295776f;
				}
				else
				{
					num3 = 0f;
					num5 = 0f;
					num12 = 90f - MathUtils.Atan2(num6, num4) * 57.295776f;
				}
				float degrees3 = rotation + shearX - num12;
				float degrees4 = rotation + shearY - num12 + 90f;
				float num13 = MathUtils.CosDeg(degrees3) * scaleX;
				float num14 = MathUtils.CosDeg(degrees4) * scaleY;
				float num15 = MathUtils.SinDeg(degrees3) * scaleX;
				float num16 = MathUtils.SinDeg(degrees4) * scaleY;
				this.a = num3 * num13 - num4 * num15;
				this.b = num3 * num14 - num4 * num16;
				this.c = num5 * num13 + num6 * num15;
				this.d = num5 * num14 + num6 * num16;
				break;
			}
			case TransformMode.NoScale:
			case TransformMode.NoScaleOrReflection:
			{
				float num17 = MathUtils.CosDeg(rotation);
				float num18 = MathUtils.SinDeg(rotation);
				float num19 = (num3 * num17 + num4 * num18) / skeleton.ScaleX;
				float num20 = (num5 * num17 + num6 * num18) / skeleton.ScaleY;
				float num21 = (float)Math.Sqrt((double)(num19 * num19 + num20 * num20));
				if (num21 > 1E-05f)
				{
					num21 = 1f / num21;
				}
				num19 *= num21;
				num20 *= num21;
				num21 = (float)Math.Sqrt((double)(num19 * num19 + num20 * num20));
				if (this.data.transformMode == TransformMode.NoScale && num3 * num6 - num4 * num5 < 0f != (skeleton.ScaleX < 0f != skeleton.ScaleY < 0f))
				{
					num21 = -num21;
				}
				float radians = 1.5707964f + MathUtils.Atan2(num20, num19);
				float num22 = MathUtils.Cos(radians) * num21;
				float num23 = MathUtils.Sin(radians) * num21;
				float num24 = MathUtils.CosDeg(shearX) * scaleX;
				float num25 = MathUtils.CosDeg(90f + shearY) * scaleY;
				float num26 = MathUtils.SinDeg(shearX) * scaleX;
				float num27 = MathUtils.SinDeg(90f + shearY) * scaleY;
				this.a = num19 * num24 + num22 * num26;
				this.b = num19 * num25 + num22 * num27;
				this.c = num20 * num24 + num23 * num26;
				this.d = num20 * num25 + num23 * num27;
				break;
			}
			case TransformMode.OnlyTranslation:
			{
				float degrees5 = rotation + 90f + shearY;
				this.a = MathUtils.CosDeg(rotation + shearX) * scaleX;
				this.b = MathUtils.CosDeg(degrees5) * scaleY;
				this.c = MathUtils.SinDeg(rotation + shearX) * scaleX;
				this.d = MathUtils.SinDeg(degrees5) * scaleY;
				break;
			}
			}
			this.a *= skeleton.ScaleX;
			this.b *= skeleton.ScaleX;
			this.c *= skeleton.ScaleY;
			this.d *= skeleton.ScaleY;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0005D078 File Offset: 0x0005B278
		public void SetToSetupPose()
		{
			BoneData boneData = this.data;
			this.x = boneData.x;
			this.y = boneData.y;
			this.rotation = boneData.rotation;
			this.scaleX = boneData.scaleX;
			this.scaleY = boneData.scaleY;
			this.shearX = boneData.shearX;
			this.shearY = boneData.shearY;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0005D0E0 File Offset: 0x0005B2E0
		internal void UpdateAppliedTransform()
		{
			this.appliedValid = true;
			Bone bone = this.parent;
			if (bone == null)
			{
				this.ax = this.worldX;
				this.ay = this.worldY;
				this.arotation = MathUtils.Atan2(this.c, this.a) * 57.295776f;
				this.ascaleX = (float)Math.Sqrt((double)(this.a * this.a + this.c * this.c));
				this.ascaleY = (float)Math.Sqrt((double)(this.b * this.b + this.d * this.d));
				this.ashearX = 0f;
				this.ashearY = MathUtils.Atan2(this.a * this.b + this.c * this.d, this.a * this.d - this.b * this.c) * 57.295776f;
				return;
			}
			float num = bone.a;
			float num2 = bone.b;
			float num3 = bone.c;
			float num4 = bone.d;
			float num5 = 1f / (num * num4 - num2 * num3);
			float num6 = this.worldX - bone.worldX;
			float num7 = this.worldY - bone.worldY;
			this.ax = num6 * num4 * num5 - num7 * num2 * num5;
			this.ay = num7 * num * num5 - num6 * num3 * num5;
			float num8 = num5 * num4;
			float num9 = num5 * num;
			float num10 = num5 * num2;
			float num11 = num5 * num3;
			float num12 = num8 * this.a - num10 * this.c;
			float num13 = num8 * this.b - num10 * this.d;
			float num14 = num9 * this.c - num11 * this.a;
			float num15 = num9 * this.d - num11 * this.b;
			this.ashearX = 0f;
			this.ascaleX = (float)Math.Sqrt((double)(num12 * num12 + num14 * num14));
			if (this.ascaleX > 0.0001f)
			{
				float num16 = num12 * num15 - num13 * num14;
				this.ascaleY = num16 / this.ascaleX;
				this.ashearY = MathUtils.Atan2(num12 * num13 + num14 * num15, num16) * 57.295776f;
				this.arotation = MathUtils.Atan2(num14, num12) * 57.295776f;
			}
			else
			{
				this.ascaleX = 0f;
				this.ascaleY = (float)Math.Sqrt((double)(num13 * num13 + num15 * num15));
				this.ashearY = 0f;
				this.arotation = 90f - MathUtils.Atan2(num15, num13) * 57.295776f;
			}
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0005D38C File Offset: 0x0005B58C
		public void WorldToLocal(float worldX, float worldY, out float localX, out float localY)
		{
			float num = this.a;
			float num2 = this.b;
			float num3 = this.c;
			float num4 = this.d;
			float num5 = 1f / (num * num4 - num2 * num3);
			float num6 = worldX - this.worldX;
			float num7 = worldY - this.worldY;
			localX = num6 * num4 * num5 - num7 * num2 * num5;
			localY = num7 * num * num5 - num6 * num3 * num5;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005D3FC File Offset: 0x0005B5FC
		public void LocalToWorld(float localX, float localY, out float worldX, out float worldY)
		{
			worldX = localX * this.a + localY * this.b + this.worldX;
			worldY = localX * this.c + localY * this.d + this.worldY;
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0005D434 File Offset: 0x0005B634
		public float WorldToLocalRotationX
		{
			get
			{
				Bone bone = this.parent;
				if (bone == null)
				{
					return this.arotation;
				}
				float num = bone.a;
				float num2 = bone.b;
				float num3 = bone.c;
				float num4 = bone.d;
				float num5 = this.a;
				float num6 = this.c;
				return MathUtils.Atan2(num * num6 - num3 * num5, num4 * num5 - num2 * num6) * 57.295776f;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0005D4A0 File Offset: 0x0005B6A0
		public float WorldToLocalRotationY
		{
			get
			{
				Bone bone = this.parent;
				if (bone == null)
				{
					return this.arotation;
				}
				float num = bone.a;
				float num2 = bone.b;
				float num3 = bone.c;
				float num4 = bone.d;
				float num5 = this.b;
				float num6 = this.d;
				return MathUtils.Atan2(num * num6 - num3 * num5, num4 * num5 - num2 * num6) * 57.295776f;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0005D50C File Offset: 0x0005B70C
		public float WorldToLocalRotation(float worldRotation)
		{
			float num = MathUtils.SinDeg(worldRotation);
			float num2 = MathUtils.CosDeg(worldRotation);
			return MathUtils.Atan2(this.a * num - this.c * num2, this.d * num2 - this.b * num) * 57.295776f + this.rotation - this.shearX;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0005D564 File Offset: 0x0005B764
		public float LocalToWorldRotation(float localRotation)
		{
			localRotation -= this.rotation - this.shearX;
			float num = MathUtils.SinDeg(localRotation);
			float num2 = MathUtils.CosDeg(localRotation);
			return MathUtils.Atan2(num2 * this.c + num * this.d, num2 * this.a + num * this.b) * 57.295776f;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0005D5C0 File Offset: 0x0005B7C0
		public void RotateWorld(float degrees)
		{
			float num = this.a;
			float num2 = this.b;
			float num3 = this.c;
			float num4 = this.d;
			float num5 = MathUtils.CosDeg(degrees);
			float num6 = MathUtils.SinDeg(degrees);
			this.a = num5 * num - num6 * num3;
			this.b = num5 * num2 - num6 * num4;
			this.c = num6 * num + num5 * num3;
			this.d = num6 * num2 + num5 * num4;
			this.appliedValid = false;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0005D63C File Offset: 0x0005B83C
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x04000B53 RID: 2899
		public static bool yDown;

		// Token: 0x04000B54 RID: 2900
		internal BoneData data;

		// Token: 0x04000B55 RID: 2901
		internal Skeleton skeleton;

		// Token: 0x04000B56 RID: 2902
		internal Bone parent;

		// Token: 0x04000B57 RID: 2903
		internal ExposedList<Bone> children = new ExposedList<Bone>();

		// Token: 0x04000B58 RID: 2904
		internal float x;

		// Token: 0x04000B59 RID: 2905
		internal float y;

		// Token: 0x04000B5A RID: 2906
		internal float rotation;

		// Token: 0x04000B5B RID: 2907
		internal float scaleX;

		// Token: 0x04000B5C RID: 2908
		internal float scaleY;

		// Token: 0x04000B5D RID: 2909
		internal float shearX;

		// Token: 0x04000B5E RID: 2910
		internal float shearY;

		// Token: 0x04000B5F RID: 2911
		internal float ax;

		// Token: 0x04000B60 RID: 2912
		internal float ay;

		// Token: 0x04000B61 RID: 2913
		internal float arotation;

		// Token: 0x04000B62 RID: 2914
		internal float ascaleX;

		// Token: 0x04000B63 RID: 2915
		internal float ascaleY;

		// Token: 0x04000B64 RID: 2916
		internal float ashearX;

		// Token: 0x04000B65 RID: 2917
		internal float ashearY;

		// Token: 0x04000B66 RID: 2918
		internal bool appliedValid;

		// Token: 0x04000B67 RID: 2919
		internal float a;

		// Token: 0x04000B68 RID: 2920
		internal float b;

		// Token: 0x04000B69 RID: 2921
		internal float worldX;

		// Token: 0x04000B6A RID: 2922
		internal float c;

		// Token: 0x04000B6B RID: 2923
		internal float d;

		// Token: 0x04000B6C RID: 2924
		internal float worldY;

		// Token: 0x04000B6D RID: 2925
		internal bool sorted;

		// Token: 0x04000B6E RID: 2926
		internal bool active;
	}
}
