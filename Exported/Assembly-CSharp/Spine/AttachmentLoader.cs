using System;

namespace Spine
{
	// Token: 0x02000198 RID: 408
	public interface AttachmentLoader
	{
		// Token: 0x06000BD3 RID: 3027
		RegionAttachment NewRegionAttachment(Skin skin, string name, string path);

		// Token: 0x06000BD4 RID: 3028
		MeshAttachment NewMeshAttachment(Skin skin, string name, string path);

		// Token: 0x06000BD5 RID: 3029
		BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name);

		// Token: 0x06000BD6 RID: 3030
		PathAttachment NewPathAttachment(Skin skin, string name);

		// Token: 0x06000BD7 RID: 3031
		PointAttachment NewPointAttachment(Skin skin, string name);

		// Token: 0x06000BD8 RID: 3032
		ClippingAttachment NewClippingAttachment(Skin skin, string name);
	}
}
