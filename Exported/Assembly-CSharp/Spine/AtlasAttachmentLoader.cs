using System;

namespace Spine
{
	// Token: 0x02000195 RID: 405
	public class AtlasAttachmentLoader : AttachmentLoader
	{
		// Token: 0x06000BC4 RID: 3012 RVA: 0x0005B034 File Offset: 0x00059234
		public AtlasAttachmentLoader(params Atlas[] atlasArray)
		{
			if (atlasArray == null)
			{
				throw new ArgumentNullException("atlas array cannot be null.");
			}
			this.atlasArray = atlasArray;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0005B054 File Offset: 0x00059254
		public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
		{
			AtlasRegion atlasRegion = this.FindRegion(path);
			if (atlasRegion == null)
			{
				throw new ArgumentException(string.Format("Region not found in atlas: {0} (region attachment: {1})", path, name));
			}
			RegionAttachment regionAttachment = new RegionAttachment(name);
			regionAttachment.RendererObject = atlasRegion;
			regionAttachment.SetUVs(atlasRegion.u, atlasRegion.v, atlasRegion.u2, atlasRegion.v2, atlasRegion.rotate);
			regionAttachment.regionOffsetX = atlasRegion.offsetX;
			regionAttachment.regionOffsetY = atlasRegion.offsetY;
			regionAttachment.regionWidth = (float)atlasRegion.width;
			regionAttachment.regionHeight = (float)atlasRegion.height;
			regionAttachment.regionOriginalWidth = (float)atlasRegion.originalWidth;
			regionAttachment.regionOriginalHeight = (float)atlasRegion.originalHeight;
			return regionAttachment;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0005B100 File Offset: 0x00059300
		public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
		{
			AtlasRegion atlasRegion = this.FindRegion(path);
			if (atlasRegion == null)
			{
				throw new ArgumentException(string.Format("Region not found in atlas: {0} (region attachment: {1})", path, name));
			}
			return new MeshAttachment(name)
			{
				RendererObject = atlasRegion,
				RegionU = atlasRegion.u,
				RegionV = atlasRegion.v,
				RegionU2 = atlasRegion.u2,
				RegionV2 = atlasRegion.v2,
				RegionRotate = atlasRegion.rotate,
				RegionDegrees = atlasRegion.degrees,
				regionOffsetX = atlasRegion.offsetX,
				regionOffsetY = atlasRegion.offsetY,
				regionWidth = (float)atlasRegion.width,
				regionHeight = (float)atlasRegion.height,
				regionOriginalWidth = (float)atlasRegion.originalWidth,
				regionOriginalHeight = (float)atlasRegion.originalHeight
			};
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0005B1D0 File Offset: 0x000593D0
		public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
		{
			return new BoundingBoxAttachment(name);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0005B1D8 File Offset: 0x000593D8
		public PathAttachment NewPathAttachment(Skin skin, string name)
		{
			return new PathAttachment(name);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0005B1E0 File Offset: 0x000593E0
		public PointAttachment NewPointAttachment(Skin skin, string name)
		{
			return new PointAttachment(name);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0005B1E8 File Offset: 0x000593E8
		public ClippingAttachment NewClippingAttachment(Skin skin, string name)
		{
			return new ClippingAttachment(name);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0005B1F0 File Offset: 0x000593F0
		public AtlasRegion FindRegion(string name)
		{
			for (int i = 0; i < this.atlasArray.Length; i++)
			{
				AtlasRegion atlasRegion = this.atlasArray[i].FindRegion(name);
				if (atlasRegion != null)
				{
					return atlasRegion;
				}
			}
			return null;
		}

		// Token: 0x04000AFF RID: 2815
		private Atlas[] atlasArray;
	}
}
