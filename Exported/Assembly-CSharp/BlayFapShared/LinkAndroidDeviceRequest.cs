using System;

namespace BlayFapShared
{
	// Token: 0x0200007D RID: 125
	[Serializable]
	public class LinkAndroidDeviceRequest : AuthenticatedBlayFapRequest
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x0000DCC4 File Offset: 0x0000BEC4
		public LinkAndroidDeviceRequest(string deviceId, bool force = false)
		{
			this.DeviceId = deviceId;
			this.Force = force;
		}

		// Token: 0x04000262 RID: 610
		public string DeviceId;

		// Token: 0x04000263 RID: 611
		public bool Force;
	}
}
