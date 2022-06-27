using System;

namespace BlayFapShared
{
	// Token: 0x0200006A RID: 106
	[Serializable]
	public class ValidateGooglePlayReceiptRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400023F RID: 575
		public string Receipt;

		// Token: 0x04000240 RID: 576
		public string Signature;
	}
}
