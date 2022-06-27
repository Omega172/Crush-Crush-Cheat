using System;

namespace Spine
{
	// Token: 0x020001AA RID: 426
	public abstract class ConstraintData
	{
		// Token: 0x06000CF0 RID: 3312 RVA: 0x0005DFBC File Offset: 0x0005C1BC
		public ConstraintData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0005DFE4 File Offset: 0x0005C1E4
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0005DFEC File Offset: 0x0005C1EC
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x0005DFF4 File Offset: 0x0005C1F4
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0005E000 File Offset: 0x0005C200
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x0005E008 File Offset: 0x0005C208
		public bool SkinRequired
		{
			get
			{
				return this.skinRequired;
			}
			set
			{
				this.skinRequired = value;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0005E014 File Offset: 0x0005C214
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000B8C RID: 2956
		internal readonly string name;

		// Token: 0x04000B8D RID: 2957
		internal int order;

		// Token: 0x04000B8E RID: 2958
		internal bool skinRequired;
	}
}
