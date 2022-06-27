using System;

namespace Spine
{
	// Token: 0x020001CF RID: 463
	public class TransformConstraintData : ConstraintData
	{
		// Token: 0x06000F0C RID: 3852 RVA: 0x0006C4B0 File Offset: 0x0006A6B0
		public TransformConstraintData(string name) : base(name)
		{
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0006C4C4 File Offset: 0x0006A6C4
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0006C4CC File Offset: 0x0006A6CC
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x0006C4D4 File Offset: 0x0006A6D4
		public BoneData Target
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

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0006C4E0 File Offset: 0x0006A6E0
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x0006C4E8 File Offset: 0x0006A6E8
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

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0006C4F4 File Offset: 0x0006A6F4
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x0006C4FC File Offset: 0x0006A6FC
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

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0006C508 File Offset: 0x0006A708
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x0006C510 File Offset: 0x0006A710
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x0006C51C File Offset: 0x0006A71C
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x0006C524 File Offset: 0x0006A724
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

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0006C530 File Offset: 0x0006A730
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x0006C538 File Offset: 0x0006A738
		public float OffsetRotation
		{
			get
			{
				return this.offsetRotation;
			}
			set
			{
				this.offsetRotation = value;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0006C544 File Offset: 0x0006A744
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x0006C54C File Offset: 0x0006A74C
		public float OffsetX
		{
			get
			{
				return this.offsetX;
			}
			set
			{
				this.offsetX = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0006C558 File Offset: 0x0006A758
		// (set) Token: 0x06000F1D RID: 3869 RVA: 0x0006C560 File Offset: 0x0006A760
		public float OffsetY
		{
			get
			{
				return this.offsetY;
			}
			set
			{
				this.offsetY = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0006C56C File Offset: 0x0006A76C
		// (set) Token: 0x06000F1F RID: 3871 RVA: 0x0006C574 File Offset: 0x0006A774
		public float OffsetScaleX
		{
			get
			{
				return this.offsetScaleX;
			}
			set
			{
				this.offsetScaleX = value;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0006C580 File Offset: 0x0006A780
		// (set) Token: 0x06000F21 RID: 3873 RVA: 0x0006C588 File Offset: 0x0006A788
		public float OffsetScaleY
		{
			get
			{
				return this.offsetScaleY;
			}
			set
			{
				this.offsetScaleY = value;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0006C594 File Offset: 0x0006A794
		// (set) Token: 0x06000F23 RID: 3875 RVA: 0x0006C59C File Offset: 0x0006A79C
		public float OffsetShearY
		{
			get
			{
				return this.offsetShearY;
			}
			set
			{
				this.offsetShearY = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0006C5A8 File Offset: 0x0006A7A8
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x0006C5B0 File Offset: 0x0006A7B0
		public bool Relative
		{
			get
			{
				return this.relative;
			}
			set
			{
				this.relative = value;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0006C5BC File Offset: 0x0006A7BC
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x0006C5C4 File Offset: 0x0006A7C4
		public bool Local
		{
			get
			{
				return this.local;
			}
			set
			{
				this.local = value;
			}
		}

		// Token: 0x04000C7F RID: 3199
		internal ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04000C80 RID: 3200
		internal BoneData target;

		// Token: 0x04000C81 RID: 3201
		internal float rotateMix;

		// Token: 0x04000C82 RID: 3202
		internal float translateMix;

		// Token: 0x04000C83 RID: 3203
		internal float scaleMix;

		// Token: 0x04000C84 RID: 3204
		internal float shearMix;

		// Token: 0x04000C85 RID: 3205
		internal float offsetRotation;

		// Token: 0x04000C86 RID: 3206
		internal float offsetX;

		// Token: 0x04000C87 RID: 3207
		internal float offsetY;

		// Token: 0x04000C88 RID: 3208
		internal float offsetScaleX;

		// Token: 0x04000C89 RID: 3209
		internal float offsetScaleY;

		// Token: 0x04000C8A RID: 3210
		internal float offsetShearY;

		// Token: 0x04000C8B RID: 3211
		internal bool relative;

		// Token: 0x04000C8C RID: 3212
		internal bool local;
	}
}
