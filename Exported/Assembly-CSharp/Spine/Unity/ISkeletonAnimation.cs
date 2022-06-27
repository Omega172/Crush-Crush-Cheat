using System;

namespace Spine.Unity
{
	// Token: 0x02000208 RID: 520
	public interface ISkeletonAnimation
	{
		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060010E3 RID: 4323
		// (remove) Token: 0x060010E4 RID: 4324
		event UpdateBonesDelegate UpdateLocal;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060010E5 RID: 4325
		// (remove) Token: 0x060010E6 RID: 4326
		event UpdateBonesDelegate UpdateWorld;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060010E7 RID: 4327
		// (remove) Token: 0x060010E8 RID: 4328
		event UpdateBonesDelegate UpdateComplete;

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060010E9 RID: 4329
		Skeleton Skeleton { get; }
	}
}
