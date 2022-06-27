using System;

namespace Spine
{
	// Token: 0x020001B9 RID: 441
	public class PowOut : Pow
	{
		// Token: 0x06000DB8 RID: 3512 RVA: 0x00060BAC File Offset: 0x0005EDAC
		public PowOut(float power) : base(power)
		{
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00060BB8 File Offset: 0x0005EDB8
		protected override float Apply(float a)
		{
			return (float)Math.Pow((double)(a - 1f), (double)base.Power) * (float)((base.Power % 2f != 0f) ? 1 : -1) + 1f;
		}
	}
}
