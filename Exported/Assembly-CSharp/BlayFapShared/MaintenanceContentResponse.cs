using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class MaintenanceContentResponse
	{
		// Token: 0x0400025D RID: 605
		public Dictionary<string, Dictionary<string, string>> CachedTitleData;

		// Token: 0x0400025E RID: 606
		public Dictionary<string, CatalogItem[]> CachedCatalog;
	}
}
