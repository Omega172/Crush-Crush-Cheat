using System;
using System.Collections.Generic;
using SadPanda;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B7 RID: 183
	public class SteamStoreController : IStoreController
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x00020C7C File Offset: 0x0001EE7C
		public SteamStoreController(SteamStore store)
		{
			this._store = store;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00020C8C File Offset: 0x0001EE8C
		public ProductCollection products
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00020C94 File Offset: 0x0001EE94
		public void ConfirmPendingPurchase(Product product)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00020C9C File Offset: 0x0001EE9C
		public void FetchAdditionalProducts(HashSet<ProductDefinition> products, Action successCallback, Action<InitializationFailureReason> failCallback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00020CA4 File Offset: 0x0001EEA4
		public void InitiatePurchase(Product product, string payload)
		{
			this._store.Purchase(product.definition, payload);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00020CB8 File Offset: 0x0001EEB8
		public void InitiatePurchase(string productId, string payload)
		{
			foreach (Product product in this.products.all)
			{
				if (product.definition.id == productId)
				{
					this.InitiatePurchase(product, payload);
				}
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00020D08 File Offset: 0x0001EF08
		public void InitiatePurchase(Product product)
		{
			this._store.Purchase(product.definition, string.Empty);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00020D20 File Offset: 0x0001EF20
		public void InitiatePurchase(string productId)
		{
			foreach (Product product in this.products.all)
			{
				if (product.definition.id == productId)
				{
					this.InitiatePurchase(product);
				}
			}
		}

		// Token: 0x0400045A RID: 1114
		private SteamStore _store;
	}
}
