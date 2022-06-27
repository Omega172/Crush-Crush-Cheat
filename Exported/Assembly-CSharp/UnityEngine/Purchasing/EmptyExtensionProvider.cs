using System;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B3 RID: 179
	public class EmptyExtensionProvider : IExtensionProvider
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x00020C74 File Offset: 0x0001EE74
		public T GetExtension<T>() where T : IStoreExtension
		{
			throw new NotImplementedException();
		}
	}
}
