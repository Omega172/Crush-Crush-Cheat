using System;

namespace Spine
{
	// Token: 0x020001B1 RID: 433
	public class IkConstraintData : ConstraintData
	{
		// Token: 0x06000D7A RID: 3450 RVA: 0x0005FF3C File Offset: 0x0005E13C
		public IkConstraintData(string name) : base(name)
		{
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0005FF70 File Offset: 0x0005E170
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0005FF78 File Offset: 0x0005E178
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0005FF80 File Offset: 0x0005E180
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0005FF8C File Offset: 0x0005E18C
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x0005FF94 File Offset: 0x0005E194
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

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0005FFA0 File Offset: 0x0005E1A0
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x0005FFA8 File Offset: 0x0005E1A8
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

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0005FFB4 File Offset: 0x0005E1B4
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x0005FFBC File Offset: 0x0005E1BC
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

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0005FFC8 File Offset: 0x0005E1C8
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x0005FFD0 File Offset: 0x0005E1D0
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0005FFDC File Offset: 0x0005E1DC
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x0005FFE4 File Offset: 0x0005E1E4
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

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x0005FFF8 File Offset: 0x0005E1F8
		public bool Uniform
		{
			get
			{
				return this.uniform;
			}
			set
			{
				this.uniform = value;
			}
		}

		// Token: 0x04000BAF RID: 2991
		internal ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04000BB0 RID: 2992
		internal BoneData target;

		// Token: 0x04000BB1 RID: 2993
		internal int bendDirection = 1;

		// Token: 0x04000BB2 RID: 2994
		internal bool compress;

		// Token: 0x04000BB3 RID: 2995
		internal bool stretch;

		// Token: 0x04000BB4 RID: 2996
		internal bool uniform;

		// Token: 0x04000BB5 RID: 2997
		internal float mix = 1f;

		// Token: 0x04000BB6 RID: 2998
		internal float softness;
	}
}
