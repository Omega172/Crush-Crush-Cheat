using System;
using UnityEngine;

namespace Spine.Unity.AttachmentTools
{
	// Token: 0x02000228 RID: 552
	public static class AttachmentRegionExtensions
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x0007B784 File Offset: 0x00079984
		public static AtlasRegion GetRegion(this Attachment attachment)
		{
			IHasRendererObject hasRendererObject = attachment as IHasRendererObject;
			if (hasRendererObject != null)
			{
				return hasRendererObject.RendererObject as AtlasRegion;
			}
			return null;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0007B7AC File Offset: 0x000799AC
		public static AtlasRegion GetRegion(this RegionAttachment regionAttachment)
		{
			return regionAttachment.RendererObject as AtlasRegion;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0007B7BC File Offset: 0x000799BC
		public static AtlasRegion GetRegion(this MeshAttachment meshAttachment)
		{
			return meshAttachment.RendererObject as AtlasRegion;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0007B7CC File Offset: 0x000799CC
		public static void SetRegion(this Attachment attachment, AtlasRegion region, bool updateOffset = true)
		{
			RegionAttachment regionAttachment = attachment as RegionAttachment;
			if (regionAttachment != null)
			{
				regionAttachment.SetRegion1(region, updateOffset);
			}
			MeshAttachment meshAttachment = attachment as MeshAttachment;
			if (meshAttachment != null)
			{
				meshAttachment.SetRegion2(region, updateOffset);
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0007B804 File Offset: 0x00079A04
		public static void SetRegion1(this RegionAttachment attachment, AtlasRegion region, bool updateOffset = true)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			attachment.RendererObject = region;
			attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
			attachment.regionOffsetX = region.offsetX;
			attachment.regionOffsetY = region.offsetY;
			attachment.regionWidth = (float)region.width;
			attachment.regionHeight = (float)region.height;
			attachment.regionOriginalWidth = (float)region.originalWidth;
			attachment.regionOriginalHeight = (float)region.originalHeight;
			if (updateOffset)
			{
				attachment.UpdateOffset();
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0007B8A8 File Offset: 0x00079AA8
		public static void SetRegion2(this MeshAttachment attachment, AtlasRegion region, bool updateUVs = true)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			attachment.RendererObject = region;
			attachment.RegionU = region.u;
			attachment.RegionV = region.v;
			attachment.RegionU2 = region.u2;
			attachment.RegionV2 = region.v2;
			attachment.RegionRotate = region.rotate;
			attachment.regionOffsetX = region.offsetX;
			attachment.regionOffsetY = region.offsetY;
			attachment.regionWidth = (float)region.width;
			attachment.regionHeight = (float)region.height;
			attachment.regionOriginalWidth = (float)region.originalWidth;
			attachment.regionOriginalHeight = (float)region.originalHeight;
			if (updateUVs)
			{
				attachment.UpdateUVs();
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0007B964 File Offset: 0x00079B64
		public static RegionAttachment ToRegionAttachment(this Sprite sprite, Material material, float rotation = 0f)
		{
			return sprite.ToRegionAttachment(material.ToSpineAtlasPage(), rotation);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0007B974 File Offset: 0x00079B74
		public static RegionAttachment ToRegionAttachment(this Sprite sprite, AtlasPage page, float rotation = 0f)
		{
			if (sprite == null)
			{
				throw new ArgumentNullException("sprite");
			}
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}
			AtlasRegion region = sprite.ToAtlasRegion(page);
			float scale = 1f / sprite.pixelsPerUnit;
			return region.ToRegionAttachment(sprite.name, scale, rotation);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0007B9CC File Offset: 0x00079BCC
		public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, float rotation = 0f)
		{
			if (sprite == null)
			{
				throw new ArgumentNullException("sprite");
			}
			if (shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			AtlasRegion region = sprite.ToAtlasRegionPMAClone(shader, textureFormat, mipmaps, materialPropertySource);
			float scale = 1f / sprite.pixelsPerUnit;
			return region.ToRegionAttachment(sprite.name, scale, rotation);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0007BA30 File Offset: 0x00079C30
		public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, float rotation = 0f)
		{
			return sprite.ToRegionAttachmentPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource, rotation);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0007BA50 File Offset: 0x00079C50
		public static RegionAttachment ToRegionAttachment(this AtlasRegion region, string attachmentName, float scale = 0.01f, float rotation = 0f)
		{
			if (string.IsNullOrEmpty(attachmentName))
			{
				throw new ArgumentException("attachmentName can't be null or empty.", "attachmentName");
			}
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			RegionAttachment regionAttachment = new RegionAttachment(attachmentName);
			regionAttachment.RendererObject = region;
			regionAttachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
			regionAttachment.regionOffsetX = region.offsetX;
			regionAttachment.regionOffsetY = region.offsetY;
			regionAttachment.regionWidth = (float)region.width;
			regionAttachment.regionHeight = (float)region.height;
			regionAttachment.regionOriginalWidth = (float)region.originalWidth;
			regionAttachment.regionOriginalHeight = (float)region.originalHeight;
			regionAttachment.Path = region.name;
			regionAttachment.scaleX = 1f;
			regionAttachment.scaleY = 1f;
			regionAttachment.rotation = rotation;
			regionAttachment.r = 1f;
			regionAttachment.g = 1f;
			regionAttachment.b = 1f;
			regionAttachment.a = 1f;
			regionAttachment.width = regionAttachment.regionOriginalWidth * scale;
			regionAttachment.height = regionAttachment.regionOriginalHeight * scale;
			regionAttachment.SetColor(Color.white);
			regionAttachment.UpdateOffset();
			return regionAttachment;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0007BB8C File Offset: 0x00079D8C
		public static void SetScale(this RegionAttachment regionAttachment, Vector2 scale)
		{
			regionAttachment.scaleX = scale.x;
			regionAttachment.scaleY = scale.y;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0007BBA8 File Offset: 0x00079DA8
		public static void SetScale(this RegionAttachment regionAttachment, float x, float y)
		{
			regionAttachment.scaleX = x;
			regionAttachment.scaleY = y;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0007BBB8 File Offset: 0x00079DB8
		public static void SetPositionOffset(this RegionAttachment regionAttachment, Vector2 offset)
		{
			regionAttachment.x = offset.x;
			regionAttachment.y = offset.y;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0007BBD4 File Offset: 0x00079DD4
		public static void SetPositionOffset(this RegionAttachment regionAttachment, float x, float y)
		{
			regionAttachment.x = x;
			regionAttachment.y = y;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0007BBE4 File Offset: 0x00079DE4
		public static void SetRotation(this RegionAttachment regionAttachment, float rotation)
		{
			regionAttachment.rotation = rotation;
		}
	}
}
