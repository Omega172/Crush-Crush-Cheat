using System;

namespace BlayFapShared
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class GetPollResponse : BlayFapResponse
	{
		// Token: 0x04000251 RID: 593
		public string[] Questions;

		// Token: 0x04000252 RID: 594
		public ulong[] Responses;

		// Token: 0x04000253 RID: 595
		public string[] ImageUrls;

		// Token: 0x04000254 RID: 596
		public uint[] QuestionIds;

		// Token: 0x04000255 RID: 597
		public byte VoteStatus;
	}
}
