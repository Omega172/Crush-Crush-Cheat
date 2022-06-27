using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000A4 RID: 164
	public class ConfigurationBuilder
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00020B04 File Offset: 0x0001ED04
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x00020B0C File Offset: 0x0001ED0C
		[Obsolete("This property has been renamed 'useCatalogProvider'", false)]
		public bool useCloudCatalog { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00020B18 File Offset: 0x0001ED18
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00020B20 File Offset: 0x0001ED20
		public bool useCatalogProvider { get; set; }

		// Token: 0x060003E7 RID: 999 RVA: 0x00020B2C File Offset: 0x0001ED2C
		public static ConfigurationBuilder Instance(IPurchasingModule first, params IPurchasingModule[] rest)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00020B34 File Offset: 0x0001ED34
		public ConfigurationBuilder AddProduct(string id, ProductType type)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00020B3C File Offset: 0x0001ED3C
		public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00020B44 File Offset: 0x0001ED44
		public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs, PayoutDefinition payout)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00020B4C File Offset: 0x0001ED4C
		public ConfigurationBuilder AddProduct(string id, ProductType type, IDs storeIDs, IEnumerable<PayoutDefinition> payouts)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00020B54 File Offset: 0x0001ED54
		public ConfigurationBuilder AddProducts(IEnumerable<ProductDefinition> products)
		{
			return ConfigurationBuilder._instance;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00020B5C File Offset: 0x0001ED5C
		public T Configure<T>() where T : IStoreConfiguration
		{
			return default(T);
		}

		// Token: 0x04000413 RID: 1043
		public HashSet<ProductDefinition> products;

		// Token: 0x04000414 RID: 1044
		private static ConfigurationBuilder _instance = new ConfigurationBuilder();
	}
}
