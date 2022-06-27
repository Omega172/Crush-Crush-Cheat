using System;

namespace BlayFapShared
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public class GetUserInfoResponse : BlayFapResponse
	{
		// Token: 0x04000224 RID: 548
		public DateTime Created;

		// Token: 0x04000225 RID: 549
		public DateTime LastLogin;

		// Token: 0x04000226 RID: 550
		public string[] DeviceIds;

		// Token: 0x04000227 RID: 551
		public uint TotalPurchases;

		// Token: 0x04000228 RID: 552
		public string PlayFabId;
	}
}
