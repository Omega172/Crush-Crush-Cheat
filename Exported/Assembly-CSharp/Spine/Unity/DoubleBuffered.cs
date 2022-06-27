using System;

namespace Spine.Unity
{
	// Token: 0x0200020E RID: 526
	public class DoubleBuffered<T> where T : new()
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x00076448 File Offset: 0x00074648
		public T GetCurrent()
		{
			return (!this.usingA) ? this.b : this.a;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00076468 File Offset: 0x00074668
		public T GetNext()
		{
			this.usingA = !this.usingA;
			return (!this.usingA) ? this.b : this.a;
		}

		// Token: 0x04000DE0 RID: 3552
		private readonly T a = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);

		// Token: 0x04000DE1 RID: 3553
		private readonly T b = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);

		// Token: 0x04000DE2 RID: 3554
		private bool usingA;
	}
}
