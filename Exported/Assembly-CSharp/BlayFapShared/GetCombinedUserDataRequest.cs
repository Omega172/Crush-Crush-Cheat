using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x02000043 RID: 67
	public class GetCombinedUserDataRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x040001FE RID: 510
		public List<string> Keys;

		// Token: 0x040001FF RID: 511
		public CombinedUserDataFlags Flags;
	}
}
