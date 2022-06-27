using System;

namespace AssetBundles
{
	// Token: 0x0200002E RID: 46
	public interface ICommandHandler<in T>
	{
		// Token: 0x06000121 RID: 289
		void Handle(T cmd, AssetBundleManager manager);
	}
}
