using System;

namespace Spine
{
	// Token: 0x02000193 RID: 403
	public class AtlasRegion
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x0005B024 File Offset: 0x00059224
		public AtlasRegion Clone()
		{
			return base.MemberwiseClone() as AtlasRegion;
		}

		// Token: 0x04000AEC RID: 2796
		public AtlasPage page;

		// Token: 0x04000AED RID: 2797
		public string name;

		// Token: 0x04000AEE RID: 2798
		public int x;

		// Token: 0x04000AEF RID: 2799
		public int y;

		// Token: 0x04000AF0 RID: 2800
		public int width;

		// Token: 0x04000AF1 RID: 2801
		public int height;

		// Token: 0x04000AF2 RID: 2802
		public float u;

		// Token: 0x04000AF3 RID: 2803
		public float v;

		// Token: 0x04000AF4 RID: 2804
		public float u2;

		// Token: 0x04000AF5 RID: 2805
		public float v2;

		// Token: 0x04000AF6 RID: 2806
		public float offsetX;

		// Token: 0x04000AF7 RID: 2807
		public float offsetY;

		// Token: 0x04000AF8 RID: 2808
		public int originalWidth;

		// Token: 0x04000AF9 RID: 2809
		public int originalHeight;

		// Token: 0x04000AFA RID: 2810
		public int index;

		// Token: 0x04000AFB RID: 2811
		public bool rotate;

		// Token: 0x04000AFC RID: 2812
		public int degrees;

		// Token: 0x04000AFD RID: 2813
		public int[] splits;

		// Token: 0x04000AFE RID: 2814
		public int[] pads;
	}
}
