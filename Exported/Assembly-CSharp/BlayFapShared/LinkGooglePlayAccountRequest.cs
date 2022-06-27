using System;

namespace BlayFapShared
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	public class LinkGooglePlayAccountRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		public LinkGooglePlayAccountRequest(string token, bool force = false)
		{
			this.Token = token;
			this.Force = force;
		}

		// Token: 0x04000264 RID: 612
		public string Token;

		// Token: 0x04000265 RID: 613
		public bool Force;
	}
}
