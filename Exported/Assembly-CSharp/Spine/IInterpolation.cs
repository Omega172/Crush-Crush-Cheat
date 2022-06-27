using System;

namespace Spine
{
	// Token: 0x020001B7 RID: 439
	public abstract class IInterpolation
	{
		// Token: 0x06000DB2 RID: 3506
		protected abstract float Apply(float a);

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00060B00 File Offset: 0x0005ED00
		public float Apply(float start, float end, float a)
		{
			return start + (end - start) * this.Apply(a);
		}

		// Token: 0x04000BD2 RID: 3026
		public static IInterpolation Pow2 = new Pow(2f);

		// Token: 0x04000BD3 RID: 3027
		public static IInterpolation Pow2Out = new PowOut(2f);
	}
}
