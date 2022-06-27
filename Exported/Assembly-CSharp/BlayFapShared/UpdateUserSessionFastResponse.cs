using System;

namespace BlayFapShared
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class UpdateUserSessionFastResponse : BlayFapResponse
	{
		// Token: 0x04000208 RID: 520
		public byte SequenceID;

		// Token: 0x04000209 RID: 521
		public ulong SessionID;
	}
}
