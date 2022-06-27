using System;

namespace Spine.Unity
{
	// Token: 0x0200020A RID: 522
	public interface ISkeletonComponent
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060010EB RID: 4331
		SkeletonDataAsset SkeletonDataAsset { get; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060010EC RID: 4332
		Skeleton Skeleton { get; }
	}
}
