using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B8 RID: 184
	public interface IStoreController
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600041D RID: 1053
		ProductCollection products { get; }

		// Token: 0x0600041E RID: 1054
		void ConfirmPendingPurchase(Product product);

		// Token: 0x0600041F RID: 1055
		void FetchAdditionalProducts(HashSet<ProductDefinition> products, Action successCallback, Action<InitializationFailureReason> failCallback);

		// Token: 0x06000420 RID: 1056
		void InitiatePurchase(Product product, string payload);

		// Token: 0x06000421 RID: 1057
		void InitiatePurchase(string productId, string payload);

		// Token: 0x06000422 RID: 1058
		void InitiatePurchase(Product product);

		// Token: 0x06000423 RID: 1059
		void InitiatePurchase(string productId);
	}
}
