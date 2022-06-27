using System;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000C5 RID: 197
	public abstract class AbstractPurchasingModule : IPurchasingModule
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00020E7C File Offset: 0x0001F07C
		public void Configure(IPurchasingBinder binder)
		{
		}

		// Token: 0x06000449 RID: 1097
		public abstract void Configure();

		// Token: 0x0600044A RID: 1098 RVA: 0x00020E80 File Offset: 0x0001F080
		protected void BindConfiguration<T>(T instance) where T : IStoreConfiguration
		{
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00020E84 File Offset: 0x0001F084
		protected void BindExtension<T>(T instance) where T : IStoreExtension
		{
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00020E88 File Offset: 0x0001F088
		protected void RegisterStore(string name, IStore a)
		{
		}

		// Token: 0x0400046B RID: 1131
		protected IPurchasingBinder m_Binder;
	}
}
