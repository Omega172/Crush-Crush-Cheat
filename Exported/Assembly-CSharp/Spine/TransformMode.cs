using System;

namespace Spine
{
	// Token: 0x020001A4 RID: 420
	[Flags]
	public enum TransformMode
	{
		// Token: 0x04000B7D RID: 2941
		Normal = 0,
		// Token: 0x04000B7E RID: 2942
		OnlyTranslation = 7,
		// Token: 0x04000B7F RID: 2943
		NoRotationOrReflection = 1,
		// Token: 0x04000B80 RID: 2944
		NoScale = 2,
		// Token: 0x04000B81 RID: 2945
		NoScaleOrReflection = 6
	}
}
