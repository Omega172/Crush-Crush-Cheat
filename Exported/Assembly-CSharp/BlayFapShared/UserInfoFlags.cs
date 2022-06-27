using System;

namespace BlayFapShared
{
	// Token: 0x02000056 RID: 86
	[Flags]
	public enum UserInfoFlags : byte
	{
		// Token: 0x0400021F RID: 543
		Legacy = 0,
		// Token: 0x04000220 RID: 544
		Purchases = 1,
		// Token: 0x04000221 RID: 545
		LinkedAccounts = 2,
		// Token: 0x04000222 RID: 546
		All = 255
	}
}
