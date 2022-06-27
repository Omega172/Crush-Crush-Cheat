using System;

namespace BlayFapShared
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class SubmitSupportTicketRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x04000259 RID: 601
		public string Subject;

		// Token: 0x0400025A RID: 602
		public string Message;
	}
}
