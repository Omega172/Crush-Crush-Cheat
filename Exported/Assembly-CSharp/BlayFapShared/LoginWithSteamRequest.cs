using System;

namespace BlayFapShared
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class LoginWithSteamRequest
	{
		// Token: 0x040001DF RID: 479
		public string SteamAuthToken;

		// Token: 0x040001E0 RID: 480
		public ulong SteamId;

		// Token: 0x040001E1 RID: 481
		public bool CreateAccount;

		// Token: 0x040001E2 RID: 482
		public bool ReactivateAccount;

		// Token: 0x040001E3 RID: 483
		public string VersionIdentifier;
	}
}
