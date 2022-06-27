using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000BF RID: 191
	public interface IStoreCallback
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000436 RID: 1078
		ProductCollection products { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000437 RID: 1079
		// (set) Token: 0x06000438 RID: 1080
		bool useTransactionLog { get; set; }

		// Token: 0x06000439 RID: 1081
		void OnProductsRetrieved(List<ProductDescription> products);

		// Token: 0x0600043A RID: 1082
		void OnPurchaseFailed(PurchaseFailureDescription desc);

		// Token: 0x0600043B RID: 1083
		void OnPurchaseSucceeded(string storeSpecificId, string receipt, string transactionIdentifier);

		// Token: 0x0600043C RID: 1084
		void OnSetupFailed(InitializationFailureReason reason);
	}
}
