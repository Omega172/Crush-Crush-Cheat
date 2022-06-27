using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B2 RID: 178
	public class Product
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00020C58 File Offset: 0x0001EE58
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00020C64 File Offset: 0x0001EE64
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000454 RID: 1108
		public ProductDefinition definition;

		// Token: 0x04000455 RID: 1109
		public ProductMetadata metadata;

		// Token: 0x04000456 RID: 1110
		public bool availableToPurchase;

		// Token: 0x04000457 RID: 1111
		public string transactionID;

		// Token: 0x04000458 RID: 1112
		public bool hasReceipt;

		// Token: 0x04000459 RID: 1113
		public string receipt;
	}
}
