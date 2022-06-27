using System;

namespace BlayFapShared
{
	// Token: 0x02000047 RID: 71
	[Serializable]
	public class UpdateUserSessionRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x04000203 RID: 515
		public string Session;

		// Token: 0x04000204 RID: 516
		public byte SequenceID;

		// Token: 0x04000205 RID: 517
		public ulong SessionID;
	}
}
