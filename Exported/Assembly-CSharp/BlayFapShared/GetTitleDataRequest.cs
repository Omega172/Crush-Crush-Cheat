using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public class GetTitleDataRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400022A RID: 554
		public List<string> Keys;
	}
}
