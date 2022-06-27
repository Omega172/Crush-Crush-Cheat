using System;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000BE RID: 190
	public class PurchaseFailureDescription
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00020DD4 File Offset: 0x0001EFD4
		public PurchaseFailureDescription(string productId, PurchaseFailureReason reason, string message)
		{
		}

		// Token: 0x04000466 RID: 1126
		public string productId;

		// Token: 0x04000467 RID: 1127
		public PurchaseFailureReason reason;

		// Token: 0x04000468 RID: 1128
		public string message;
	}
}
