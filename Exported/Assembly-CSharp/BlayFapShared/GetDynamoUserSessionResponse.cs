using System;

namespace BlayFapShared
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class GetDynamoUserSessionResponse : BlayFapResponse
	{
		// Token: 0x0400020A RID: 522
		public string Session;

		// Token: 0x0400020B RID: 523
		public ulong SessionID;
	}
}
