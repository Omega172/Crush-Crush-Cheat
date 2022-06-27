using System;
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000A8 RID: 168
	public class StandardPurchasingModule : AbstractPurchasingModule, IAndroidStoreSelection, IStoreConfiguration
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00020B94 File Offset: 0x0001ED94
		public AndroidStore androidStore
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00020B9C File Offset: 0x0001ED9C
		public AppStore appStore
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00020BA4 File Offset: 0x0001EDA4
		public static StandardPurchasingModule Instance()
		{
			return StandardPurchasingModule._instance;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00020BAC File Offset: 0x0001EDAC
		public static StandardPurchasingModule Instance(AndroidStore androidStore)
		{
			return StandardPurchasingModule._instance;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00020BB4 File Offset: 0x0001EDB4
		public static StandardPurchasingModule Instance(AppStore androidStore)
		{
			return StandardPurchasingModule._instance;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00020BBC File Offset: 0x0001EDBC
		public override void Configure()
		{
		}

		// Token: 0x0400041F RID: 1055
		private static StandardPurchasingModule _instance = new StandardPurchasingModule();
	}
}
