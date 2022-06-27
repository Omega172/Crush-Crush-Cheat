using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BlayFap;
using BlayFapShared;
using SadPanda.Platforms;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace SadPanda
{
	// Token: 0x020000C8 RID: 200
	public class KongregateStore : IStore
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x000210AC File Offset: 0x0001F2AC
		public void Initialize(IStoreCallback callback)
		{
			Debug.Log("Initializing KongregateStore");
			this.callback = callback;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000210C0 File Offset: 0x0001F2C0
		public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
		{
			this.requestedProducts.Clear();
			for (int i = 0; i < products.Count; i++)
			{
				this.requestedProducts.Add(products[i].storeSpecificId);
			}
			Kongregate.Initialized += new ReactiveProperty<bool>.Changed(this.OnKongregateInitialized);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00021124 File Offset: 0x0001F324
		private void OnKongregateInitialized(bool isInitialized)
		{
			if (!isInitialized)
			{
				return;
			}
			Kongregate.Initialized -= new ReactiveProperty<bool>.Changed(this.OnKongregateInitialized);
			Kongregate.CatalogReady = (Action<bool, Kongregate.CatalogItem[]>)Delegate.Combine(Kongregate.CatalogReady, new Action<bool, Kongregate.CatalogItem[]>(this.OnCatalogReady));
			Kongregate.GetCatalogItems();
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00021178 File Offset: 0x0001F378
		private void OnCatalogReady(bool success, Kongregate.CatalogItem[] items)
		{
			Kongregate.CatalogReady = (Action<bool, Kongregate.CatalogItem[]>)Delegate.Remove(Kongregate.CatalogReady, new Action<bool, Kongregate.CatalogItem[]>(this.OnCatalogReady));
			if (!success)
			{
				this.callback.OnSetupFailed(InitializationFailureReason.PurchasingUnavailable);
			}
			else if (items.Length == 0)
			{
				this.callback.OnSetupFailed(InitializationFailureReason.NoProductsAvailable);
			}
			else
			{
				List<ProductDescription> list = new List<ProductDescription>(items.Length);
				foreach (Kongregate.CatalogItem catalogItem in items)
				{
					if (this.requestedProducts.Contains(catalogItem.identifier))
					{
						ProductMetadata metadata = new ProductMetadata(catalogItem.price.ToString(), catalogItem.name, catalogItem.description, "Kreds", catalogItem.price);
						list.Add(new ProductDescription(catalogItem.identifier, metadata));
					}
				}
				this.callback.OnProductsRetrieved(list);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0002126C File Offset: 0x0001F46C
		public void Purchase(ProductDefinition product, string developerPayload)
		{
			this.pendingProduct = product;
			Product product2 = this.callback.products.WithID(product.id);
			StartPurchaseRequest request = new StartPurchaseRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				Description = product2.metadata.localizedDescription,
				ItemID = product.storeSpecificId
			};
			BlayFapClient.Instance.StartPurchase(request, delegate(StartPurchaseResponse startResponse)
			{
				if (startResponse.Error != null)
				{
					Debug.LogError(string.Format("Kongregate Purchase failure: [{0}]", startResponse.Error.ErrorType.ToString()));
					if (startResponse.Error.ErrorType == BlayFapResponseError.BlayFapError.NotAuthenticated || startResponse.Error.ErrorType == BlayFapResponseError.BlayFapError.InvalidAuthToken)
					{
						Kongregate.SignIn();
					}
					this.callback.OnPurchaseFailed(new PurchaseFailureDescription(product.storeSpecificId, PurchaseFailureReason.Unknown, startResponse.Error.ToString()));
					return;
				}
				KongregateStore.OnPurchase onPurchase = new KongregateStore.OnPurchase(this, startResponse.OrderID);
				Kongregate.PurchaseCompleted = (Action<Kongregate.PurchaseResult>)Delegate.Combine(Kongregate.PurchaseCompleted, onPurchase.Callback);
				Kongregate.PurchaseItem(product.storeSpecificId);
			});
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00021304 File Offset: 0x0001F504
		private void OnPurchaseComplete(Kongregate.PurchaseResult result, KongregateStore.OnPurchase onPurchase)
		{
			Kongregate.PurchaseCompleted = (Action<Kongregate.PurchaseResult>)Delegate.Remove(Kongregate.PurchaseCompleted, onPurchase.Callback);
			string productId = this.pendingProduct.storeSpecificId;
			this.pendingProduct = null;
			if (result.success)
			{
				FinishPurchaseRequest request = new FinishPurchaseRequest
				{
					BlayFapId = BlayFapClient.BlayFapId,
					OrderID = onPurchase.OrderId
				};
				BlayFapClient.Instance.FinishPurchase(request, delegate(FinishPurchaseResponse _)
				{
					this.callback.OnPurchaseSucceeded(productId, string.Empty, onPurchase.OrderId);
				});
			}
			else
			{
				this.callback.OnPurchaseFailed(new PurchaseFailureDescription(productId, PurchaseFailureReason.Unknown, string.Empty));
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000213C4 File Offset: 0x0001F5C4
		public void FinishTransaction(ProductDefinition product, string transactionId)
		{
		}

		// Token: 0x04000470 RID: 1136
		private IStoreCallback callback;

		// Token: 0x04000471 RID: 1137
		private HashSet<string> requestedProducts = new HashSet<string>();

		// Token: 0x04000472 RID: 1138
		private ProductDefinition pendingProduct;

		// Token: 0x020000C9 RID: 201
		private class OnPurchase
		{
			// Token: 0x0600045D RID: 1117 RVA: 0x000213C8 File Offset: 0x0001F5C8
			public OnPurchase(KongregateStore store, string orderId)
			{
				KongregateStore.OnPurchase <>f__this = this;
				this.OrderId = orderId;
				this.Callback = delegate(Kongregate.PurchaseResult result)
				{
					store.OnPurchaseComplete(result, <>f__this);
				};
			}

			// Token: 0x04000473 RID: 1139
			public string OrderId;

			// Token: 0x04000474 RID: 1140
			public Action<Kongregate.PurchaseResult> Callback;
		}
	}
}
