using System;

namespace BlayFapShared
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class LoginWithIosRequest
	{
		// Token: 0x040001EC RID: 492
		public string DeviceId;

		// Token: 0x040001ED RID: 493
		public bool CreateAccount;

		// Token: 0x040001EE RID: 494
		public bool ReactivateAccount;

		// Token: 0x040001EF RID: 495
		public string VersionIdentifier;
	}
}
