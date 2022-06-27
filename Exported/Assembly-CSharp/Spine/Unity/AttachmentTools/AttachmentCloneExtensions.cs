using System;
using UnityEngine;

namespace Spine.Unity.AttachmentTools
{
	// Token: 0x02000227 RID: 551
	public static class AttachmentCloneExtensions
	{
		// Token: 0x0600115C RID: 4444 RVA: 0x0007B5E0 File Offset: 0x000797E0
		public static Attachment GetCopy(this Attachment o, bool cloneMeshesAsLinked)
		{
			MeshAttachment meshAttachment = o as MeshAttachment;
			if (meshAttachment != null && cloneMeshesAsLinked)
			{
				return meshAttachment.NewLinkedMesh();
			}
			return o.Copy();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0007B610 File Offset: 0x00079810
		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, string newLinkedMeshName, AtlasRegion region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			MeshAttachment meshAttachment = o.NewLinkedMesh();
			meshAttachment.SetRegion2(region, false);
			return meshAttachment;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0007B640 File Offset: 0x00079840
		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Shader shader, Material materialPropertySource = null)
		{
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			return o.GetLinkedMesh(sprite.name, sprite.ToAtlasRegion(false));
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0007B688 File Offset: 0x00079888
		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Material materialPropertySource)
		{
			return o.GetLinkedMesh(sprite, materialPropertySource.shader, materialPropertySource);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0007B698 File Offset: 0x00079898
		public static Attachment GetRemappedClone(this Attachment o, Sprite sprite, Material sourceMaterial, bool premultiplyAlpha = true, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false)
		{
			AtlasRegion atlasRegion = (!premultiplyAlpha) ? sprite.ToAtlasRegion(new Material(sourceMaterial)
			{
				mainTexture = sprite.texture
			}) : sprite.ToAtlasRegionPMAClone(sourceMaterial, TextureFormat.RGBA32, false);
			return o.GetRemappedClone(atlasRegion, cloneMeshAsLinked, useOriginalRegionSize, 1f / sprite.pixelsPerUnit);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0007B6EC File Offset: 0x000798EC
		public static Attachment GetRemappedClone(this Attachment o, AtlasRegion atlasRegion, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false, float scale = 0.01f)
		{
			RegionAttachment regionAttachment = o as RegionAttachment;
			if (regionAttachment != null)
			{
				RegionAttachment regionAttachment2 = (RegionAttachment)regionAttachment.Copy();
				regionAttachment2.SetRegion1(atlasRegion, false);
				if (!useOriginalRegionSize)
				{
					regionAttachment2.width = (float)atlasRegion.width * scale;
					regionAttachment2.height = (float)atlasRegion.height * scale;
				}
				regionAttachment2.UpdateOffset();
				return regionAttachment2;
			}
			MeshAttachment meshAttachment = o as MeshAttachment;
			if (meshAttachment != null)
			{
				MeshAttachment meshAttachment2 = (!cloneMeshAsLinked) ? ((MeshAttachment)meshAttachment.Copy()) : meshAttachment.NewLinkedMesh();
				meshAttachment2.SetRegion2(atlasRegion, true);
				return meshAttachment2;
			}
			return o.GetCopy(true);
		}
	}
}
