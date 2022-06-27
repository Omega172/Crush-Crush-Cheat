using System;

namespace BlayFapShared
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class FinishPurchaseRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400023D RID: 573
		public string OrderID;
	}
}
