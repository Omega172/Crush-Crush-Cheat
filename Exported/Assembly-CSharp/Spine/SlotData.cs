using System;

namespace Spine
{
	// Token: 0x020001CD RID: 461
	public class SlotData
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x0006B4CC File Offset: 0x000696CC
		public SlotData(int index, string name, BoneData boneData)
		{
			if (index < 0)
			{
				throw new ArgumentException("index must be >= 0.", "index");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			if (boneData == null)
			{
				throw new ArgumentNullException("boneData", "boneData cannot be null.");
			}
			this.index = index;
			this.name = name;
			this.boneData = boneData;
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x0006B564 File Offset: 0x00069764
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0006B56C File Offset: 0x0006976C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0006B574 File Offset: 0x00069774
		public BoneData BoneData
		{
			get
			{
				return this.boneData;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0006B57C File Offset: 0x0006977C
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0006B584 File Offset: 0x00069784
		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0006B590 File Offset: 0x00069790
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x0006B598 File Offset: 0x00069798
		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0006B5A4 File Offset: 0x000697A4
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x0006B5AC File Offset: 0x000697AC
		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0006B5B8 File Offset: 0x000697B8
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x0006B5C0 File Offset: 0x000697C0
		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0006B5CC File Offset: 0x000697CC
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x0006B5D4 File Offset: 0x000697D4
		public float R2
		{
			get
			{
				return this.r2;
			}
			set
			{
				this.r2 = value;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x0006B5E0 File Offset: 0x000697E0
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x0006B5E8 File Offset: 0x000697E8
		public float G2
		{
			get
			{
				return this.g2;
			}
			set
			{
				this.g2 = value;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x0006B5F4 File Offset: 0x000697F4
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x0006B5FC File Offset: 0x000697FC
		public float B2
		{
			get
			{
				return this.b2;
			}
			set
			{
				this.b2 = value;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0006B608 File Offset: 0x00069808
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x0006B610 File Offset: 0x00069810
		public bool HasSecondColor
		{
			get
			{
				return this.hasSecondColor;
			}
			set
			{
				this.hasSecondColor = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0006B61C File Offset: 0x0006981C
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0006B624 File Offset: 0x00069824
		public string AttachmentName
		{
			get
			{
				return this.attachmentName;
			}
			set
			{
				this.attachmentName = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0006B630 File Offset: 0x00069830
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0006B638 File Offset: 0x00069838
		public BlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
			}
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0006B644 File Offset: 0x00069844
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000C6A RID: 3178
		internal int index;

		// Token: 0x04000C6B RID: 3179
		internal string name;

		// Token: 0x04000C6C RID: 3180
		internal BoneData boneData;

		// Token: 0x04000C6D RID: 3181
		internal float r = 1f;

		// Token: 0x04000C6E RID: 3182
		internal float g = 1f;

		// Token: 0x04000C6F RID: 3183
		internal float b = 1f;

		// Token: 0x04000C70 RID: 3184
		internal float a = 1f;

		// Token: 0x04000C71 RID: 3185
		internal float r2;

		// Token: 0x04000C72 RID: 3186
		internal float g2;

		// Token: 0x04000C73 RID: 3187
		internal float b2;

		// Token: 0x04000C74 RID: 3188
		internal bool hasSecondColor;

		// Token: 0x04000C75 RID: 3189
		internal string attachmentName;

		// Token: 0x04000C76 RID: 3190
		internal BlendMode blendMode;
	}
}
