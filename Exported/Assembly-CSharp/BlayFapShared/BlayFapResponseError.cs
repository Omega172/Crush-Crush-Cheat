using System;

namespace BlayFapShared
{
	// Token: 0x02000082 RID: 130
	public class BlayFapResponseError
	{
		// Token: 0x060001CB RID: 459 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		public BlayFapResponseError(BlayFapResponseError.BlayFapError errorType)
		{
			this.ErrorType = errorType;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		public BlayFapResponseError()
		{
			this.ErrorType = BlayFapResponseError.BlayFapError.UnknownError;
		}

		// Token: 0x04000267 RID: 615
		public BlayFapResponseError.BlayFapError ErrorType;

		// Token: 0x02000083 RID: 131
		public enum BlayFapError
		{
			// Token: 0x04000269 RID: 617
			UnknownError = -1,
			// Token: 0x0400026A RID: 618
			MissingAuthToken,
			// Token: 0x0400026B RID: 619
			InvalidAuthToken,
			// Token: 0x0400026C RID: 620
			ExpiredSession,
			// Token: 0x0400026D RID: 621
			NotAuthenticated,
			// Token: 0x0400026E RID: 622
			Timeout,
			// Token: 0x0400026F RID: 623
			ExceededCallLimit,
			// Token: 0x04000270 RID: 624
			SteamInvalidToken,
			// Token: 0x04000271 RID: 625
			SteamApiOffline,
			// Token: 0x04000272 RID: 626
			InvalidCustomID,
			// Token: 0x04000273 RID: 627
			InvalidTitleID,
			// Token: 0x04000274 RID: 628
			UnknownPaymentProvider,
			// Token: 0x04000275 RID: 629
			UnknownUser,
			// Token: 0x04000276 RID: 630
			InvalidVirtualCurrency,
			// Token: 0x04000277 RID: 631
			InvalidInventoryItem,
			// Token: 0x04000278 RID: 632
			InvalidInventoryQuantity,
			// Token: 0x04000279 RID: 633
			InvalidItemId,
			// Token: 0x0400027A RID: 634
			InvalidReceipt,
			// Token: 0x0400027B RID: 635
			ReceiptAlreadyClaimed,
			// Token: 0x0400027C RID: 636
			ObsoleteApiCall,
			// Token: 0x0400027D RID: 637
			InvalidSequenceId,
			// Token: 0x0400027E RID: 638
			InvalidSupportEmail,
			// Token: 0x0400027F RID: 639
			InvalidCoupon,
			// Token: 0x04000280 RID: 640
			ServiceUnavailable,
			// Token: 0x04000281 RID: 641
			InMaintenance,
			// Token: 0x04000282 RID: 642
			CouponAlreadyUsed,
			// Token: 0x04000283 RID: 643
			CustomIdAlreadyInUse,
			// Token: 0x04000284 RID: 644
			InvalidLink,
			// Token: 0x04000285 RID: 645
			CannotUnlinkPrimaryAccount,
			// Token: 0x04000286 RID: 646
			LinkedAccountAlreadyClaimed,
			// Token: 0x04000287 RID: 647
			EmailAlreadyInUse,
			// Token: 0x04000288 RID: 648
			InvalidPurchase,
			// Token: 0x04000289 RID: 649
			PurchaseAlreadyRefunded,
			// Token: 0x0400028A RID: 650
			DurableItemAlreadyAwarded,
			// Token: 0x0400028B RID: 651
			UserAlreadyVoted,
			// Token: 0x0400028C RID: 652
			FacebookInvalidSignature,
			// Token: 0x0400028D RID: 653
			InvalidSessionId,
			// Token: 0x0400028E RID: 654
			DynamoDbFailure,
			// Token: 0x0400028F RID: 655
			SwitchInvalidToken,
			// Token: 0x04000290 RID: 656
			SwitchApiOffline,
			// Token: 0x04000291 RID: 657
			SwitchInvalidEnvironment,
			// Token: 0x04000292 RID: 658
			SwitchErrorCode,
			// Token: 0x04000293 RID: 659
			AlreadyRequestedDeletion,
			// Token: 0x04000294 RID: 660
			NoPendingDeletion
		}
	}
}
