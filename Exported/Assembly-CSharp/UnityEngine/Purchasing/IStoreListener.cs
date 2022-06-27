using System;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B6 RID: 182
	public interface IStoreListener
	{
		// Token: 0x06000411 RID: 1041
		void OnInitialized(IStoreController controller, IExtensionProvider extensions);

		// Token: 0x06000412 RID: 1042
		void OnInitializeFailed(InitializationFailureReason error);

		// Token: 0x06000413 RID: 1043
		void OnPurchaseFailed(Product i, PurchaseFailureReason p);

		// Token: 0x06000414 RID: 1044
		PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e);
	}
}
