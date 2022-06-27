using System;

namespace BlayFapShared
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public class GetUserInfoRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x04000223 RID: 547
		public UserInfoFlags Flags;
	}
}
