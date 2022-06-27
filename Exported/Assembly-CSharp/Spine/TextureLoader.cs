using System;

namespace Spine
{
	// Token: 0x02000194 RID: 404
	public interface TextureLoader
	{
		// Token: 0x06000BC2 RID: 3010
		void Load(AtlasPage page, string path);

		// Token: 0x06000BC3 RID: 3011
		void Unload(object texture);
	}
}
