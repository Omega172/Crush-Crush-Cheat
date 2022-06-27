using System;

namespace Spine
{
	// Token: 0x02000192 RID: 402
	public class AtlasPage
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x0005B00C File Offset: 0x0005920C
		public AtlasPage Clone()
		{
			return base.MemberwiseClone() as AtlasPage;
		}

		// Token: 0x04000AE3 RID: 2787
		public string name;

		// Token: 0x04000AE4 RID: 2788
		public Format format;

		// Token: 0x04000AE5 RID: 2789
		public TextureFilter minFilter;

		// Token: 0x04000AE6 RID: 2790
		public TextureFilter magFilter;

		// Token: 0x04000AE7 RID: 2791
		public TextureWrap uWrap;

		// Token: 0x04000AE8 RID: 2792
		public TextureWrap vWrap;

		// Token: 0x04000AE9 RID: 2793
		public object rendererObject;

		// Token: 0x04000AEA RID: 2794
		public int width;

		// Token: 0x04000AEB RID: 2795
		public int height;
	}
}
