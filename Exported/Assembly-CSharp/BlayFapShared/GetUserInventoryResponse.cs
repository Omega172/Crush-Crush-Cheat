using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	public class GetUserInventoryResponse : BlayFapResponse
	{
		// Token: 0x0400021B RID: 539
		public List<InventoryItem> Items;
	}
}
