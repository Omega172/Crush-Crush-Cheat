using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000C3 RID: 195
	public interface ICatalogProvider
	{
		// Token: 0x06000441 RID: 1089
		void FetchProducts(Action<HashSet<ProductDefinition>> callback);
	}
}
