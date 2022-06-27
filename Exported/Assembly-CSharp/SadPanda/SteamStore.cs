using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BlayFap;
using BlayFapShared;
using Steamworks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace SadPanda
{
	// Token: 0x020000CA RID: 202
	public class SteamStore : IStore
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x00021410 File Offset: 0x0001F610
		public void Initialize(IStoreCallback callback)
		{
			Debug.Log("Initializing SteamStore...");
			this.callback = callback;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00021424 File Offset: 0x0001F624
		public void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products)
		{
			if (!SteamManager.Initialized)
			{
				Debug.LogError("Steamworks must be initialized before initializing the store.");
				this.callback.OnSetupFailed(InitializationFailureReason.PurchasingUnavailable);
				return;
			}
			CGameID cgameID = new CGameID(SteamUtils.GetAppID());
			this.gameId = cgameID.m_GameID;
			List<ProductDescription> list = new List<ProductDescription>(products.Count);
			foreach (ProductDefinition productDefinition in products)
			{
				Store2.BlayfapItem blayfapItem = new Store2.BlayfapItem(string.Empty, 0U, null);
				foreach (Store2.BlayfapItem blayfapItem2 in Store2.DiamondItems)
				{
					if (blayfapItem2.Id == productDefinition.id)
					{
						blayfapItem = blayfapItem2;
					}
				}
				foreach (Store2.BlayfapItem blayfapItem3 in Store2.BundleItems)
				{
					if (blayfapItem3.Id == productDefinition.id)
					{
						blayfapItem = blayfapItem3;
					}
				}
				ProductMetadata metadata = new ProductMetadata(blayfapItem.Price.ToString("C2"), blayfapItem.Id, this.GetDescription(blayfapItem.Id), "USD", blayfapItem.Price);
				list.Add(new ProductDescription(productDefinition.storeSpecificId, metadata));
			}
			this.callback.OnProductsRetrieved(list);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000215AC File Offset: 0x0001F7AC
		private string GetDescription(string id)
		{
			if (id.Contains(".diamond"))
			{
				foreach (Store2.BlayfapItem blayfapItem in Store2.DiamondItems)
				{
					if (blayfapItem.Id == id)
					{
						return string.Format("Purchase {0} in-game diamond currency.", blayfapItem.Diamonds.ToString());
					}
				}
				return id;
			}
			if (id.Contains(".starterpack"))
			{
				return "Purchase Starter Pack";
			}
			if (id.Contains(".darya"))
			{
				return "Purchase Darya in-game character.";
			}
			if (id.Contains(".charlotte"))
			{
				return "Purchase Charlotte in-game character.";
			}
			if (id.Contains(".catara"))
			{
				return "Purchase Catara in-game character.";
			}
			return id;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0002166C File Offset: 0x0001F86C
		public void Purchase(ProductDefinition product, string developerPayload)
		{
			if (this.microTxnAuthorizationResponseCallback != null)
			{
				this.microTxnAuthorizationResponseCallback.Unregister();
			}
			Product product2 = this.callback.products.WithID(product.id);
			StartPurchaseRequest request = new StartPurchaseRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				Description = this.GetDescription(product.id),
				ItemID = product.storeSpecificId
			};
			Debug.LogWarning(SteamUser.GetSteamID().m_SteamID.ToString());
			BlayFapClient.Instance.StartPurchase(request, delegate(StartPurchaseResponse startResponse)
			{
				if (startResponse.Error != null)
				{
					Debug.LogErrorFormat("Steam Purchase failure: [{0}]", new object[]
					{
						startResponse.Error.ErrorType
					});
					this.callback.OnPurchaseFailed(new PurchaseFailureDescription(product.storeSpecificId, PurchaseFailureReason.Unknown, startResponse.Error.ToString()));
					return;
				}
				this.microTxnAuthorizationResponseCallback = Callback<MicroTxnAuthorizationResponse_t>.Create(delegate(MicroTxnAuthorizationResponse_t result)
				{
					if ((ulong)result.m_unAppID != this.gameId)
					{
						return;
					}
					this.microTxnAuthorizationResponseCallback.Unregister();
					this.microTxnAuthorizationResponseCallback = null;
					if (result.m_bAuthorized == 1)
					{
						FinishPurchaseRequest request2 = new FinishPurchaseRequest
						{
							BlayFapId = BlayFapClient.BlayFapId,
							OrderID = result.m_ulOrderID.ToString()
						};
						BlayFapClient.Instance.FinishPurchase(request2, delegate(FinishPurchaseResponse _)
						{
							this.callback.OnPurchaseSucceeded(product.storeSpecificId, string.Empty, result.m_ulOrderID.ToString("X"));
						});
					}
					else
					{
						this.callback.OnPurchaseFailed(new PurchaseFailureDescription(product.storeSpecificId, PurchaseFailureReason.Unknown, string.Empty));
					}
				});
			});
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0002172C File Offset: 0x0001F92C
		public void FinishTransaction(ProductDefinition product, string transactionId)
		{
		}

		// Token: 0x04000475 RID: 1141
		private IStoreCallback callback;

		// Token: 0x04000476 RID: 1142
		private Callback<MicroTxnAuthorizationResponse_t> microTxnAuthorizationResponseCallback;

		// Token: 0x04000477 RID: 1143
		private ulong gameId;
	}
}
