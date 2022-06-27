using System;

namespace BlayFapShared
{
	// Token: 0x02000072 RID: 114
	public class VoteForPollRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x0400024E RID: 590
		public uint Id;

		// Token: 0x0400024F RID: 591
		public uint QuestionId;

		// Token: 0x04000250 RID: 592
		public bool UseDiamonds;
	}
}
