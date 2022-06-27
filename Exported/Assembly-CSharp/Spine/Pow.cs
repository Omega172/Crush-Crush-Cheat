using System;

namespace Spine
{
	// Token: 0x020001B8 RID: 440
	public class Pow : IInterpolation
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x00060B10 File Offset: 0x0005ED10
		public Pow(float power)
		{
			this.Power = power;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00060B20 File Offset: 0x0005ED20
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x00060B28 File Offset: 0x0005ED28
		public float Power { get; set; }

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00060B34 File Offset: 0x0005ED34
		protected override float Apply(float a)
		{
			if (a <= 0.5f)
			{
				return (float)Math.Pow((double)(a * 2f), (double)this.Power) / 2f;
			}
			return (float)Math.Pow((double)((a - 1f) * 2f), (double)this.Power) / (float)((this.Power % 2f != 0f) ? 2 : -2) + 1f;
		}
	}
}
