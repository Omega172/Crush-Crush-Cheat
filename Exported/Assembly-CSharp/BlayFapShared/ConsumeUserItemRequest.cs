using System;

namespace BlayFapShared
{
	// Token: 0x02000054 RID: 84
	[Serializable]
	public class ConsumeUserItemRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400021C RID: 540
		public string ItemId;

		// Token: 0x0400021D RID: 541
		public int Quantity;
	}
}
