using System;

namespace BlayFapShared
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	public class InventoryItem
	{
		// Token: 0x04000216 RID: 534
		public string ItemId;

		// Token: 0x04000217 RID: 535
		public int Quantity;

		// Token: 0x04000218 RID: 536
		public bool Consumable;

		// Token: 0x04000219 RID: 537
		public string CustomData;

		// Token: 0x0400021A RID: 538
		public uint Price;
	}
}
