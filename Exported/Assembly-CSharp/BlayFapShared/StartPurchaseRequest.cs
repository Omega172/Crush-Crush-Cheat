using System;

namespace BlayFapShared
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class StartPurchaseRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400023A RID: 570
		public string ItemID;

		// Token: 0x0400023B RID: 571
		public string Description;
	}
}
