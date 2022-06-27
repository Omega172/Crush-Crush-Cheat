using System;

namespace Spine
{
	// Token: 0x020001C4 RID: 452
	public class Polygon
	{
		// Token: 0x06000E54 RID: 3668 RVA: 0x000664D4 File Offset: 0x000646D4
		public Polygon()
		{
			this.Vertices = new float[16];
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x000664EC File Offset: 0x000646EC
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x000664F4 File Offset: 0x000646F4
		public float[] Vertices { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00066500 File Offset: 0x00064700
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00066508 File Offset: 0x00064708
		public int Count { get; set; }
	}
}
