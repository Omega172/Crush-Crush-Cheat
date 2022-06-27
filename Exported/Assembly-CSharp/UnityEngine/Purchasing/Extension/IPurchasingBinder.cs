using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000C4 RID: 196
	public interface IPurchasingBinder
	{
		// Token: 0x06000442 RID: 1090
		void RegisterConfiguration<T>(T instance) where T : IStoreConfiguration;

		// Token: 0x06000443 RID: 1091
		void RegisterExtension<T>(T instance) where T : IStoreExtension;

		// Token: 0x06000444 RID: 1092
		void RegisterStore(string name, IStore a);

		// Token: 0x06000445 RID: 1093
		void SetCatalogProvider(ICatalogProvider provider);

		// Token: 0x06000446 RID: 1094
		void SetCatalogProviderFunction(Action<Action<HashSet<ProductDefinition>>> func);
	}
}
