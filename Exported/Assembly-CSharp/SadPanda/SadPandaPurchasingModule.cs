using System;
using UnityEngine;
using UnityEngine.Purchasing.Extension;

namespace SadPanda
{
	// Token: 0x020000C6 RID: 198
	public class SadPandaPurchasingModule : AbstractPurchasingModule
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x00020E94 File Offset: 0x0001F094
		public static SadPandaPurchasingModule Instance()
		{
			Debug.Log("Sad Panda Purchasing Module v1.0");
			return new SadPandaPurchasingModule();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00020EA8 File Offset: 0x0001F0A8
		public override void Configure()
		{
			if (this.store == null)
			{
				this.store = new SteamStore();
				this.appStore = "Steam";
			}
			if (this.store == null)
			{
				this.store = new FakeStore();
				this.appStore = "fake";
			}
			base.RegisterStore(this.appStore, this.store);
		}

		// Token: 0x0400046C RID: 1132
		private IStore store;

		// Token: 0x0400046D RID: 1133
		public string appStore;
	}
}
