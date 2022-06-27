using System;

namespace BlayFapShared
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public class LinkAccountRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400025F RID: 607
		public string CustomID;

		// Token: 0x04000260 RID: 608
		public bool Force;
	}
}
