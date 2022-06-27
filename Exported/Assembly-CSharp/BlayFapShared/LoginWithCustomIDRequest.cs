using System;

namespace BlayFapShared
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class LoginWithCustomIDRequest
	{
		// Token: 0x040001D6 RID: 470
		public string CustomId;

		// Token: 0x040001D7 RID: 471
		public bool CreateAccount;

		// Token: 0x040001D8 RID: 472
		public bool ReactivateAccount;

		// Token: 0x040001D9 RID: 473
		public string VersionIdentifier;
	}
}
