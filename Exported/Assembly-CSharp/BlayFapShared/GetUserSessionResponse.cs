using System;

namespace BlayFapShared
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public class GetUserSessionResponse : BlayFapResponse
	{
		// Token: 0x0400020C RID: 524
		public string Session;

		// Token: 0x0400020D RID: 525
		public byte SequenceID;
	}
}
