using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class GetUserDataRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x040001FA RID: 506
		public List<string> Keys;
	}
}
