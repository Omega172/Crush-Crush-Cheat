using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000C0 RID: 192
	public class ProductCollection
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x00020DE4 File Offset: 0x0001EFE4
		public Product WithID(string id)
		{
			foreach (Product product in this.all)
			{
				if (product.definition.id == id)
				{
					return product;
				}
			}
			return null;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00020E2C File Offset: 0x0001F02C
		public Product WithStoreSpecificID(string id)
		{
			foreach (Product product in this.all)
			{
				if (product.definition.id == id)
				{
					return product;
				}
			}
			return null;
		}

		// Token: 0x04000469 RID: 1129
		public HashSet<Product> set;

		// Token: 0x0400046A RID: 1130
		public Product[] all;
	}
}
