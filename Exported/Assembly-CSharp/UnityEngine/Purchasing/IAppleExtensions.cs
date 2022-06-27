using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000A3 RID: 163
	public interface IAppleExtensions : IStoreExtension
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003D5 RID: 981
		// (set) Token: 0x060003D6 RID: 982
		bool simulateAskToBuy { get; set; }

		// Token: 0x060003D7 RID: 983
		void ContinuePromotionalPurchases();

		// Token: 0x060003D8 RID: 984
		Dictionary<string, string> GetIntroductoryPriceDictionary();

		// Token: 0x060003D9 RID: 985
		Dictionary<string, string> GetProductDetails();

		// Token: 0x060003DA RID: 986
		string GetTransactionReceiptForProduct(Product product);

		// Token: 0x060003DB RID: 987
		void RefreshAppReceipt(Action<string> successCallback, Action errorCallback);

		// Token: 0x060003DC RID: 988
		void RegisterPurchaseDeferredListener(Action<Product> callback);

		// Token: 0x060003DD RID: 989
		void RestoreTransactions(Action<bool> callback);

		// Token: 0x060003DE RID: 990
		void SetApplicationUsername(string applicationUsername);

		// Token: 0x060003DF RID: 991
		void SetStorePromotionOrder(List<Product> products);

		// Token: 0x060003E0 RID: 992
		void SetStorePromotionVisibility(Product product, AppleStorePromotionVisibility visible);
	}
}
