using System;

namespace BlayFapShared
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class LoginResponse : BlayFapResponse, IEquatable<LoginResponse>
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000D960 File Offset: 0x0000BB60
		public bool Equals(LoginResponse other)
		{
			return other.AuthToken == this.AuthToken;
		}

		// Token: 0x040001F4 RID: 500
		public ulong BlayFapId;

		// Token: 0x040001F5 RID: 501
		public bool Created;

		// Token: 0x040001F6 RID: 502
		public bool PendingDeletion;

		// Token: 0x040001F7 RID: 503
		public string AuthToken;

		// Token: 0x040001F8 RID: 504
		public DateTime CreationDate;

		// Token: 0x040001F9 RID: 505
		public DateTime AuthExpiration;
	}
}
