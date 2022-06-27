using System;

namespace BlayFapShared
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class ValidateFacebookReceiptRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x04000242 RID: 578
		public string DeveloperPayload;

		// Token: 0x04000243 RID: 579
		public bool IsConsumed;

		// Token: 0x04000244 RID: 580
		public string PaymentID;

		// Token: 0x04000245 RID: 581
		public string ProductID;

		// Token: 0x04000246 RID: 582
		public long PurchaseTime;

		// Token: 0x04000247 RID: 583
		public string PurchaseToken;

		// Token: 0x04000248 RID: 584
		public string SignedRequest;
	}
}
