using System;
using System.Collections.ObjectModel;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000BB RID: 187
	public interface IStore
	{
		// Token: 0x0600042A RID: 1066
		void FinishTransaction(ProductDefinition product, string transactionId);

		// Token: 0x0600042B RID: 1067
		void Initialize(IStoreCallback callback);

		// Token: 0x0600042C RID: 1068
		void Purchase(ProductDefinition product, string developerPayload);

		// Token: 0x0600042D RID: 1069
		void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products);
	}
}
