using System;

namespace BlayFapShared
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class LoginWithGameCenterRequest
	{
		// Token: 0x040001F0 RID: 496
		public string PlayerId;

		// Token: 0x040001F1 RID: 497
		public bool CreateAccount;

		// Token: 0x040001F2 RID: 498
		public bool ReactivateAccount;

		// Token: 0x040001F3 RID: 499
		public string VersionIdentifier;
	}
}
