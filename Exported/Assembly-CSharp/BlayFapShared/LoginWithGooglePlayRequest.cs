using System;

namespace BlayFapShared
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class LoginWithGooglePlayRequest
	{
		// Token: 0x040001E8 RID: 488
		public string Token;

		// Token: 0x040001E9 RID: 489
		public bool CreateAccount;

		// Token: 0x040001EA RID: 490
		public bool ReactivateAccount;

		// Token: 0x040001EB RID: 491
		public string VersionIdentifier;
	}
}
