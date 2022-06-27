using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public class SetUserDataRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x04000201 RID: 513
		public Dictionary<string, string> Data;

		// Token: 0x04000202 RID: 514
		public List<string> KeysToRemove;
	}
}
