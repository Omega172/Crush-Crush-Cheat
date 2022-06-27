using System;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000AF RID: 175
	public enum PurchaseFailureReason
	{
		// Token: 0x0400043F RID: 1087
		PurchasingUnavailable,
		// Token: 0x04000440 RID: 1088
		ExistingPurchasePending,
		// Token: 0x04000441 RID: 1089
		ProductUnavailable,
		// Token: 0x04000442 RID: 1090
		SignatureInvalid,
		// Token: 0x04000443 RID: 1091
		UserCancelled,
		// Token: 0x04000444 RID: 1092
		PaymentDeclined,
		// Token: 0x04000445 RID: 1093
		DuplicateTransaction,
		// Token: 0x04000446 RID: 1094
		Unknown
	}
}
