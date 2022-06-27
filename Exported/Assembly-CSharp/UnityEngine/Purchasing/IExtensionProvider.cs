using System;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B5 RID: 181
	public interface IExtensionProvider
	{
		// Token: 0x06000410 RID: 1040
		T GetExtension<T>() where T : IStoreExtension;
	}
}
