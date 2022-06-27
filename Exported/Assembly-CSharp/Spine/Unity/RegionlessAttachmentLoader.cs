using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001D4 RID: 468
	public class RegionlessAttachmentLoader : AttachmentLoader
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0006CFC8 File Offset: 0x0006B1C8
		private static AtlasRegion EmptyRegion
		{
			get
			{
				if (RegionlessAttachmentLoader.emptyRegion == null)
				{
					RegionlessAttachmentLoader.emptyRegion = new AtlasRegion
					{
						name = "Empty AtlasRegion",
						page = new AtlasPage
						{
							name = "Empty AtlasPage",
							rendererObject = new Material(Shader.Find("Spine/Special/HiddenPass"))
							{
								name = "NoRender Material"
							}
						}
					};
				}
				return RegionlessAttachmentLoader.emptyRegion;
			}
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0006D038 File Offset: 0x0006B238
		public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
		{
			return new RegionAttachment(name)
			{
				RendererObject = RegionlessAttachmentLoader.EmptyRegion
			};
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0006D05C File Offset: 0x0006B25C
		public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
		{
			return new MeshAttachment(name)
			{
				RendererObject = RegionlessAttachmentLoader.EmptyRegion
			};
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0006D080 File Offset: 0x0006B280
		public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
		{
			return new BoundingBoxAttachment(name);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0006D088 File Offset: 0x0006B288
		public PathAttachment NewPathAttachment(Skin skin, string name)
		{
			return new PathAttachment(name);
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0006D090 File Offset: 0x0006B290
		public PointAttachment NewPointAttachment(Skin skin, string name)
		{
			return new PointAttachment(name);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0006D098 File Offset: 0x0006B298
		public ClippingAttachment NewClippingAttachment(Skin skin, string name)
		{
			return new ClippingAttachment(name);
		}

		// Token: 0x04000C9C RID: 3228
		private static AtlasRegion emptyRegion;
	}
}
