using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000A7 RID: 167
	public interface IAndroidStoreSelection : IStoreConfiguration
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003F0 RID: 1008
		AndroidStore androidStore { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003F1 RID: 1009
		AppStore appStore { get; }
	}
}
