using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace SadPanda
{
	// Token: 0x020000C7 RID: 199
	public class FakeStore : IStore
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x00020F14 File Offset: 0x0001F114
		public void Initialize(IStoreCallback callback)
		{
			Debug.Log("Initializing FakeStore...");
			this.callback = callback;
			callback.useTransactionLog = false;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00020F30 File Offset: 0x0001F130
		public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
		{
			List<ProductDescription> list = new List<ProductDescription>(products.Count);
			foreach (ProductDefinition productDefinition in products)
			{
				ProductMetadata metadata = new ProductMetadata("1", "name:" + productDefinition.storeSpecificId, "desc:" + productDefinition.storeSpecificId, "fake", 1m);
				list.Add(new ProductDescription(productDefinition.storeSpecificId, metadata));
			}
			this.callback.OnProductsRetrieved(list);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00020FE8 File Offset: 0x0001F1E8
		public void Purchase(ProductDefinition product, string developerPayload)
		{
			if (product.storeSpecificId.Contains("fail"))
			{
				Debug.LogWarningFormat("Purposfully failing purchase of product [{0}] because it contains 'fail' in the id.", new object[]
				{
					product.storeSpecificId
				});
				this.callback.OnPurchaseFailed(new PurchaseFailureDescription(product.storeSpecificId, PurchaseFailureReason.Unknown, "Purposfully failed."));
			}
			else
			{
				this.callback.OnPurchaseSucceeded(product.storeSpecificId, string.Empty, "fake#" + (this.purchaseCount += 1));
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0002107C File Offset: 0x0001F27C
		public void FinishTransaction(ProductDefinition product, string transactionId)
		{
			Debug.LogFormat("Transaction for product [{0}] finished.", new object[]
			{
				product.storeSpecificId
			});
		}

		// Token: 0x0400046E RID: 1134
		private IStoreCallback callback;

		// Token: 0x0400046F RID: 1135
		private short purchaseCount;
	}
}
