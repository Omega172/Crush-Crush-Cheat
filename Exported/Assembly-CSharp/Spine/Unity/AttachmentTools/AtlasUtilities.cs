using System;
using System.Collections.Generic;
using Spine.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Spine.Unity.AttachmentTools
{
	// Token: 0x02000225 RID: 549
	public static class AtlasUtilities
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x0007A130 File Offset: 0x00078330
		public static AtlasRegion ToAtlasRegion(this Texture2D t, Material materialPropertySource, float scale = 0.01f)
		{
			return t.ToAtlasRegion(materialPropertySource.shader, scale, materialPropertySource);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0007A140 File Offset: 0x00078340
		public static AtlasRegion ToAtlasRegion(this Texture2D t, Shader shader, float scale = 0.01f, Material materialPropertySource = null)
		{
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			material.mainTexture = t;
			AtlasPage page = material.ToSpineAtlasPage();
			float num = (float)t.width;
			float num2 = (float)t.height;
			AtlasRegion atlasRegion = new AtlasRegion();
			atlasRegion.name = t.name;
			atlasRegion.index = -1;
			atlasRegion.rotate = false;
			Vector2 zero = Vector2.zero;
			Vector2 vector = new Vector2(num, num2) * scale;
			atlasRegion.width = (int)num;
			atlasRegion.originalWidth = (int)num;
			atlasRegion.height = (int)num2;
			atlasRegion.originalHeight = (int)num2;
			atlasRegion.offsetX = num * (0.5f - AtlasUtilities.InverseLerp(zero.x, vector.x, 0f));
			atlasRegion.offsetY = num2 * (0.5f - AtlasUtilities.InverseLerp(zero.y, vector.y, 0f));
			atlasRegion.u = 0f;
			atlasRegion.v = 1f;
			atlasRegion.u2 = 1f;
			atlasRegion.v2 = 0f;
			atlasRegion.x = 0;
			atlasRegion.y = 0;
			atlasRegion.page = page;
			return atlasRegion;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0007A288 File Offset: 0x00078488
		public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
		{
			return t.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0007A29C File Offset: 0x0007849C
		public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
		{
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			Texture2D clone = t.GetClone(textureFormat, mipmaps, false, true);
			clone.name = t.name + "-pma-";
			material.name = t.name + shader.name;
			material.mainTexture = clone;
			AtlasPage page = material.ToSpineAtlasPage();
			AtlasRegion atlasRegion = clone.ToAtlasRegion(shader, 0.01f, null);
			atlasRegion.page = page;
			return atlasRegion;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0007A330 File Offset: 0x00078530
		public static AtlasPage ToSpineAtlasPage(this Material m)
		{
			AtlasPage atlasPage = new AtlasPage
			{
				rendererObject = m,
				name = m.name
			};
			Texture mainTexture = m.mainTexture;
			if (mainTexture != null)
			{
				atlasPage.width = mainTexture.width;
				atlasPage.height = mainTexture.height;
			}
			return atlasPage;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0007A384 File Offset: 0x00078584
		public static AtlasRegion ToAtlasRegion(this Sprite s, AtlasPage page)
		{
			if (page == null)
			{
				throw new ArgumentNullException("page", "page cannot be null. AtlasPage determines which texture region belongs and how it should be rendered. You can use material.ToSpineAtlasPage() to get a shareable AtlasPage from a Material, or use the sprite.ToAtlasRegion(material) overload.");
			}
			AtlasRegion atlasRegion = s.ToAtlasRegion(false);
			atlasRegion.page = page;
			return atlasRegion;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0007A3B8 File Offset: 0x000785B8
		public static AtlasRegion ToAtlasRegion(this Sprite s, Material material)
		{
			AtlasRegion atlasRegion = s.ToAtlasRegion(false);
			atlasRegion.page = material.ToSpineAtlasPage();
			return atlasRegion;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0007A3DC File Offset: 0x000785DC
		public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
		{
			return s.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0007A3F0 File Offset: 0x000785F0
		public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
		{
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			Texture2D texture2D = s.ToTexture(textureFormat, mipmaps, false, true);
			texture2D.name = s.name + "-pma-";
			material.name = texture2D.name + shader.name;
			material.mainTexture = texture2D;
			AtlasPage page = material.ToSpineAtlasPage();
			AtlasRegion atlasRegion = s.ToAtlasRegion(true);
			atlasRegion.page = page;
			return atlasRegion;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0007A47C File Offset: 0x0007867C
		internal static AtlasRegion ToAtlasRegion(this Sprite s, bool isolatedTexture = false)
		{
			AtlasRegion atlasRegion = new AtlasRegion();
			atlasRegion.name = s.name;
			atlasRegion.index = -1;
			atlasRegion.rotate = (s.packed && s.packingRotation != SpritePackingRotation.None);
			Bounds bounds = s.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			Rect rect = s.rect.SpineUnityFlipRect(s.texture.height);
			atlasRegion.width = (int)rect.width;
			atlasRegion.originalWidth = (int)rect.width;
			atlasRegion.height = (int)rect.height;
			atlasRegion.originalHeight = (int)rect.height;
			atlasRegion.offsetX = rect.width * (0.5f - AtlasUtilities.InverseLerp(vector.x, vector2.x, 0f));
			atlasRegion.offsetY = rect.height * (0.5f - AtlasUtilities.InverseLerp(vector.y, vector2.y, 0f));
			if (isolatedTexture)
			{
				atlasRegion.u = 0f;
				atlasRegion.v = 1f;
				atlasRegion.u2 = 1f;
				atlasRegion.v2 = 0f;
				atlasRegion.x = 0;
				atlasRegion.y = 0;
			}
			else
			{
				Texture2D texture = s.texture;
				Rect rect2 = AtlasUtilities.TextureRectToUVRect(s.textureRect, texture.width, texture.height);
				atlasRegion.u = rect2.xMin;
				atlasRegion.v = rect2.yMax;
				atlasRegion.u2 = rect2.xMax;
				atlasRegion.v2 = rect2.yMin;
				atlasRegion.x = (int)rect.x;
				atlasRegion.y = (int)rect.y;
			}
			return atlasRegion;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0007A644 File Offset: 0x00078844
		public static void GetRepackedAttachments(List<Attachment> sourceAttachments, List<Attachment> outputAttachments, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, string newAssetName = "Repacked Attachments", bool clearCache = false, bool useOriginalNonrenderables = true)
		{
			if (sourceAttachments == null)
			{
				throw new ArgumentNullException("sourceAttachments");
			}
			if (outputAttachments == null)
			{
				throw new ArgumentNullException("outputAttachments");
			}
			Dictionary<AtlasRegion, int> dictionary = new Dictionary<AtlasRegion, int>();
			List<int> list = new List<int>();
			List<Texture2D> list2 = new List<Texture2D>();
			List<AtlasRegion> list3 = new List<AtlasRegion>();
			outputAttachments.Clear();
			outputAttachments.AddRange(sourceAttachments);
			int num = 0;
			int i = 0;
			int count = sourceAttachments.Count;
			while (i < count)
			{
				Attachment attachment = sourceAttachments[i];
				if (AtlasUtilities.IsRenderable(attachment))
				{
					Attachment copy = attachment.GetCopy(true);
					AtlasRegion region = copy.GetRegion();
					int item;
					if (dictionary.TryGetValue(region, out item))
					{
						list.Add(item);
					}
					else
					{
						list3.Add(region);
						list2.Add(region.ToTexture(textureFormat, mipmaps, 0, false, false));
						dictionary.Add(region, num);
						list.Add(num);
						num++;
					}
					outputAttachments[i] = copy;
				}
				else
				{
					outputAttachments[i] = ((!useOriginalNonrenderables) ? attachment.GetCopy(true) : attachment);
					list.Add(-1);
				}
				i++;
			}
			Texture2D texture2D = new Texture2D(maxAtlasSize, maxAtlasSize, textureFormat, mipmaps);
			texture2D.mipMapBias = -0.5f;
			texture2D.name = newAssetName;
			if (list2.Count > 0)
			{
				Texture2D source = list2[0];
				texture2D.CopyTextureAttributesFrom(source);
			}
			Rect[] array = texture2D.PackTextures(list2.ToArray(), padding, maxAtlasSize);
			Shader shader = (!(materialPropertySource == null)) ? materialPropertySource.shader : Shader.Find("Spine/Skeleton");
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			material.name = newAssetName;
			material.mainTexture = texture2D;
			AtlasPage atlasPage = material.ToSpineAtlasPage();
			atlasPage.name = newAssetName;
			List<AtlasRegion> list4 = new List<AtlasRegion>();
			int j = 0;
			int count2 = list3.Count;
			while (j < count2)
			{
				AtlasRegion referenceRegion = list3[j];
				AtlasRegion item2 = AtlasUtilities.UVRectToAtlasRegion(array[j], referenceRegion, atlasPage);
				list4.Add(item2);
				j++;
			}
			int k = 0;
			int count3 = outputAttachments.Count;
			while (k < count3)
			{
				Attachment attachment2 = outputAttachments[k];
				if (AtlasUtilities.IsRenderable(attachment2))
				{
					attachment2.SetRegion(list4[list[k]], true);
				}
				k++;
			}
			if (clearCache)
			{
				AtlasUtilities.ClearCache();
			}
			outputTexture = texture2D;
			outputMaterial = material;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0007A8E0 File Offset: 0x00078AE0
		public static Skin GetRepackedSkin(this Skin o, string newName, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool useOriginalNonrenderables = true, bool clearCache = false, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
		{
			return o.GetRepackedSkin(newName, materialPropertySource.shader, out outputMaterial, out outputTexture, maxAtlasSize, padding, textureFormat, mipmaps, materialPropertySource, clearCache, useOriginalNonrenderables, additionalTexturePropertyIDsToCopy, additionalOutputTextures, additionalTextureFormats, additionalTextureIsLinear);
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0007A914 File Offset: 0x00078B14
		public static Skin GetRepackedSkin(this Skin o, string newName, Shader shader, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, bool clearCache = false, bool useOriginalNonrenderables = true, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
		{
			outputTexture = null;
			if (additionalTexturePropertyIDsToCopy != null && additionalTextureIsLinear == null)
			{
				additionalTextureIsLinear = new bool[additionalTexturePropertyIDsToCopy.Length];
				for (int i = 0; i < additionalTextureIsLinear.Length; i++)
				{
					additionalTextureIsLinear[i] = true;
				}
			}
			if (o == null)
			{
				throw new NullReferenceException("Skin was null");
			}
			OrderedDictionary<Skin.SkinEntry, Attachment> attachments = o.Attachments;
			Skin skin = new Skin(newName);
			skin.bones.AddRange(o.bones);
			skin.constraints.AddRange(o.constraints);
			Dictionary<AtlasRegion, int> dictionary = new Dictionary<AtlasRegion, int>();
			List<int> list = new List<int>();
			List<Attachment> list2 = new List<Attachment>();
			int num = 1 + ((additionalTexturePropertyIDsToCopy != null) ? additionalTexturePropertyIDsToCopy.Length : 0);
			additionalOutputTextures = ((additionalTexturePropertyIDsToCopy != null) ? new Texture2D[additionalTexturePropertyIDsToCopy.Length] : null);
			List<Texture2D>[] array = new List<Texture2D>[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = new List<Texture2D>();
			}
			List<AtlasRegion> list3 = new List<AtlasRegion>();
			int num2 = 0;
			foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair in attachments)
			{
				Skin.SkinEntry key = keyValuePair.Key;
				Attachment value = keyValuePair.Value;
				if (AtlasUtilities.IsRenderable(value))
				{
					Attachment copy = value.GetCopy(true);
					AtlasRegion region = copy.GetRegion();
					int item;
					if (dictionary.TryGetValue(region, out item))
					{
						list.Add(item);
					}
					else
					{
						list3.Add(region);
						for (int k = 0; k < num; k++)
						{
							Texture2D item2 = (k != 0) ? region.ToTexture((additionalTextureFormats == null || k - 1 >= additionalTextureFormats.Length) ? textureFormat : additionalTextureFormats[k - 1], mipmaps, additionalTexturePropertyIDsToCopy[k - 1], additionalTextureIsLinear[k - 1], false) : region.ToTexture(textureFormat, mipmaps, 0, false, false);
							array[k].Add(item2);
						}
						dictionary.Add(region, num2);
						list.Add(num2);
						num2++;
					}
					list2.Add(copy);
					skin.SetAttachment(key.SlotIndex, key.Name, copy);
				}
				else
				{
					skin.SetAttachment(key.SlotIndex, key.Name, (!useOriginalNonrenderables) ? value.GetCopy(true) : value);
				}
			}
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			material.name = newName;
			Rect[] array2 = null;
			for (int l = 0; l < num; l++)
			{
				Texture2D texture2D = new Texture2D(maxAtlasSize, maxAtlasSize, (l <= 0 || additionalTextureFormats == null || l - 1 >= additionalTextureFormats.Length) ? textureFormat : additionalTextureFormats[l - 1], mipmaps, l > 0 && additionalTextureIsLinear[l - 1]);
				texture2D.mipMapBias = -0.5f;
				List<Texture2D> list4 = array[l];
				if (list4.Count > 0)
				{
					Texture2D source = list4[0];
					texture2D.CopyTextureAttributesFrom(source);
				}
				texture2D.name = newName;
				Rect[] array3 = texture2D.PackTextures(list4.ToArray(), padding, maxAtlasSize);
				if (l == 0)
				{
					array2 = array3;
					material.mainTexture = texture2D;
					outputTexture = texture2D;
				}
				else
				{
					material.SetTexture(additionalTexturePropertyIDsToCopy[l - 1], texture2D);
					additionalOutputTextures[l - 1] = texture2D;
				}
			}
			AtlasPage atlasPage = material.ToSpineAtlasPage();
			atlasPage.name = newName;
			List<AtlasRegion> list5 = new List<AtlasRegion>();
			int m = 0;
			int count = list3.Count;
			while (m < count)
			{
				AtlasRegion referenceRegion = list3[m];
				AtlasRegion item3 = AtlasUtilities.UVRectToAtlasRegion(array2[m], referenceRegion, atlasPage);
				list5.Add(item3);
				m++;
			}
			int n = 0;
			int count2 = list2.Count;
			while (n < count2)
			{
				Attachment attachment = list2[n];
				if (AtlasUtilities.IsRenderable(attachment))
				{
					attachment.SetRegion(list5[list[n]], true);
				}
				n++;
			}
			if (clearCache)
			{
				AtlasUtilities.ClearCache();
			}
			outputMaterial = material;
			return skin;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0007AD7C File Offset: 0x00078F7C
		public static Sprite ToSprite(this AtlasRegion ar, float pixelsPerUnit = 100f)
		{
			return Sprite.Create(ar.GetMainTexture(), ar.GetUnityRect(), new Vector2(0.5f, 0.5f), pixelsPerUnit);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0007ADA0 File Offset: 0x00078FA0
		public static void ClearCache()
		{
			foreach (Texture2D obj in AtlasUtilities.CachedRegionTexturesList)
			{
				UnityEngine.Object.Destroy(obj);
			}
			AtlasUtilities.CachedRegionTextures.Clear();
			AtlasUtilities.CachedRegionTexturesList.Clear();
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x0007AE18 File Offset: 0x00079018
		public static Texture2D ToTexture(this AtlasRegion ar, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, int texturePropertyId = 0, bool linear = false, bool applyPMA = false)
		{
			AtlasUtilities.IntAndAtlasRegionKey key = new AtlasUtilities.IntAndAtlasRegionKey(texturePropertyId, ar);
			Texture2D texture2D;
			AtlasUtilities.CachedRegionTextures.TryGetValue(key, out texture2D);
			if (texture2D == null)
			{
				Texture2D source = (texturePropertyId != 0) ? ar.GetTexture(texturePropertyId) : ar.GetMainTexture();
				Rect unityRect = ar.GetUnityRect();
				int width = (int)unityRect.width;
				int height = (int)unityRect.height;
				texture2D = new Texture2D(width, height, textureFormat, mipmaps, linear)
				{
					name = ar.name
				};
				texture2D.CopyTextureAttributesFrom(source);
				if (applyPMA)
				{
					AtlasUtilities.CopyTextureApplyPMA(source, unityRect, texture2D);
				}
				else
				{
					AtlasUtilities.CopyTexture(source, unityRect, texture2D);
				}
				AtlasUtilities.CachedRegionTextures.Add(key, texture2D);
				AtlasUtilities.CachedRegionTexturesList.Add(texture2D);
			}
			return texture2D;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0007AED8 File Offset: 0x000790D8
		private static Texture2D ToTexture(this Sprite s, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
		{
			Texture2D texture = s.texture;
			Rect sourceRect;
			if (!s.packed || s.packingMode == SpritePackingMode.Rectangle)
			{
				sourceRect = s.textureRect;
			}
			else
			{
				sourceRect = default(Rect);
				sourceRect.xMin = Math.Min(s.uv[0].x, s.uv[1].x) * (float)texture.width;
				sourceRect.xMax = Math.Max(s.uv[0].x, s.uv[1].x) * (float)texture.width;
				sourceRect.yMin = Math.Min(s.uv[0].y, s.uv[2].y) * (float)texture.height;
				sourceRect.yMax = Math.Max(s.uv[0].y, s.uv[2].y) * (float)texture.height;
			}
			Texture2D texture2D = new Texture2D((int)sourceRect.width, (int)sourceRect.height, textureFormat, mipmaps, linear);
			texture2D.CopyTextureAttributesFrom(texture);
			if (applyPMA)
			{
				AtlasUtilities.CopyTextureApplyPMA(texture, sourceRect, texture2D);
			}
			else
			{
				AtlasUtilities.CopyTexture(texture, sourceRect, texture2D);
			}
			return texture2D;
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0007B02C File Offset: 0x0007922C
		private static Texture2D GetClone(this Texture2D t, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
		{
			Texture2D texture2D = new Texture2D(t.width, t.height, textureFormat, mipmaps, linear);
			texture2D.CopyTextureAttributesFrom(t);
			if (applyPMA)
			{
				AtlasUtilities.CopyTextureApplyPMA(t, new Rect(0f, 0f, (float)t.width, (float)t.height), texture2D);
			}
			else
			{
				AtlasUtilities.CopyTexture(t, new Rect(0f, 0f, (float)t.width, (float)t.height), texture2D);
			}
			return texture2D;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0007B0AC File Offset: 0x000792AC
		private static void CopyTexture(Texture2D source, Rect sourceRect, Texture2D destination)
		{
			if (SystemInfo.copyTextureSupport == CopyTextureSupport.None)
			{
				Color[] pixels = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
				destination.SetPixels(pixels);
				destination.Apply();
			}
			else
			{
				Graphics.CopyTexture(source, 0, 0, (int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height, destination, 0, 0, 0, 0);
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0007B12C File Offset: 0x0007932C
		private static void CopyTextureApplyPMA(Texture2D source, Rect sourceRect, Texture2D destination)
		{
			Color[] pixels = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
			int i = 0;
			int num = pixels.Length;
			while (i < num)
			{
				Color color = pixels[i];
				float a = color.a;
				color.r *= a;
				color.g *= a;
				color.b *= a;
				pixels[i] = color;
				i++;
			}
			destination.SetPixels(pixels);
			destination.Apply();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0007B1DC File Offset: 0x000793DC
		private static bool IsRenderable(Attachment a)
		{
			return a is IHasRendererObject;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0007B1E8 File Offset: 0x000793E8
		private static Rect SpineUnityFlipRect(this Rect rect, int textureHeight)
		{
			rect.y = (float)textureHeight - rect.y - rect.height;
			return rect;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0007B204 File Offset: 0x00079404
		private static Rect GetUnityRect(this AtlasRegion region)
		{
			return region.GetSpineAtlasRect(true).SpineUnityFlipRect(region.page.height);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0007B220 File Offset: 0x00079420
		private static Rect GetUnityRect(this AtlasRegion region, int textureHeight)
		{
			return region.GetSpineAtlasRect(true).SpineUnityFlipRect(textureHeight);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0007B230 File Offset: 0x00079430
		private static Rect GetSpineAtlasRect(this AtlasRegion region, bool includeRotate = true)
		{
			if (includeRotate && region.rotate)
			{
				return new Rect((float)region.x, (float)region.y, (float)region.height, (float)region.width);
			}
			return new Rect((float)region.x, (float)region.y, (float)region.width, (float)region.height);
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0007B294 File Offset: 0x00079494
		private static Rect UVRectToTextureRect(Rect uvRect, int texWidth, int texHeight)
		{
			uvRect.x *= (float)texWidth;
			uvRect.width *= (float)texWidth;
			uvRect.y *= (float)texHeight;
			uvRect.height *= (float)texHeight;
			return uvRect;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0007B2E4 File Offset: 0x000794E4
		private static Rect TextureRectToUVRect(Rect textureRect, int texWidth, int texHeight)
		{
			textureRect.x = Mathf.InverseLerp(0f, (float)texWidth, textureRect.x);
			textureRect.y = Mathf.InverseLerp(0f, (float)texHeight, textureRect.y);
			textureRect.width = Mathf.InverseLerp(0f, (float)texWidth, textureRect.width);
			textureRect.height = Mathf.InverseLerp(0f, (float)texHeight, textureRect.height);
			return textureRect;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0007B35C File Offset: 0x0007955C
		private static AtlasRegion UVRectToAtlasRegion(Rect uvRect, AtlasRegion referenceRegion, AtlasPage page)
		{
			Rect rect = AtlasUtilities.UVRectToTextureRect(uvRect, page.width, page.height);
			Rect rect2 = rect.SpineUnityFlipRect(page.height);
			int x = (int)rect2.x;
			int y = (int)rect2.y;
			int num;
			int num2;
			if (referenceRegion.rotate)
			{
				num = (int)rect2.height;
				num2 = (int)rect2.width;
			}
			else
			{
				num = (int)rect2.width;
				num2 = (int)rect2.height;
			}
			int originalWidth = Mathf.RoundToInt((float)num * ((float)referenceRegion.originalWidth / (float)referenceRegion.width));
			int originalHeight = Mathf.RoundToInt((float)num2 * ((float)referenceRegion.originalHeight / (float)referenceRegion.height));
			int num3 = Mathf.RoundToInt(referenceRegion.offsetX * ((float)num / (float)referenceRegion.width));
			int num4 = Mathf.RoundToInt(referenceRegion.offsetY * ((float)num2 / (float)referenceRegion.height));
			return new AtlasRegion
			{
				page = page,
				name = referenceRegion.name,
				u = uvRect.xMin,
				u2 = uvRect.xMax,
				v = uvRect.yMax,
				v2 = uvRect.yMin,
				index = -1,
				width = num,
				originalWidth = originalWidth,
				height = num2,
				originalHeight = originalHeight,
				offsetX = (float)num3,
				offsetY = (float)num4,
				x = x,
				y = y,
				rotate = referenceRegion.rotate
			};
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0007B4F0 File Offset: 0x000796F0
		private static Texture2D GetMainTexture(this AtlasRegion region)
		{
			Material material = region.page.rendererObject as Material;
			return material.mainTexture as Texture2D;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0007B51C File Offset: 0x0007971C
		private static Texture2D GetTexture(this AtlasRegion region, string texturePropertyName)
		{
			Material material = region.page.rendererObject as Material;
			return material.GetTexture(texturePropertyName) as Texture2D;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0007B548 File Offset: 0x00079748
		private static Texture2D GetTexture(this AtlasRegion region, int texturePropertyId)
		{
			Material material = region.page.rendererObject as Material;
			return material.GetTexture(texturePropertyId) as Texture2D;
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0007B574 File Offset: 0x00079774
		private static void CopyTextureAttributesFrom(this Texture2D destination, Texture2D source)
		{
			destination.filterMode = source.filterMode;
			destination.anisoLevel = source.anisoLevel;
			destination.wrapMode = source.wrapMode;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0007B5A8 File Offset: 0x000797A8
		private static float InverseLerp(float a, float b, float value)
		{
			return (value - a) / (b - a);
		}

		// Token: 0x04000E30 RID: 3632
		internal const TextureFormat SpineTextureFormat = TextureFormat.RGBA32;

		// Token: 0x04000E31 RID: 3633
		internal const float DefaultMipmapBias = -0.5f;

		// Token: 0x04000E32 RID: 3634
		internal const bool UseMipMaps = false;

		// Token: 0x04000E33 RID: 3635
		internal const float DefaultScale = 0.01f;

		// Token: 0x04000E34 RID: 3636
		private const int NonrenderingRegion = -1;

		// Token: 0x04000E35 RID: 3637
		private static Dictionary<AtlasUtilities.IntAndAtlasRegionKey, Texture2D> CachedRegionTextures = new Dictionary<AtlasUtilities.IntAndAtlasRegionKey, Texture2D>();

		// Token: 0x04000E36 RID: 3638
		private static List<Texture2D> CachedRegionTexturesList = new List<Texture2D>();

		// Token: 0x02000226 RID: 550
		private struct IntAndAtlasRegionKey
		{
			// Token: 0x0600115A RID: 4442 RVA: 0x0007B5B4 File Offset: 0x000797B4
			public IntAndAtlasRegionKey(int i, AtlasRegion region)
			{
				this.i = i;
				this.region = region;
			}

			// Token: 0x0600115B RID: 4443 RVA: 0x0007B5C4 File Offset: 0x000797C4
			public override int GetHashCode()
			{
				return this.i.GetHashCode() * 23 ^ this.region.GetHashCode();
			}

			// Token: 0x04000E37 RID: 3639
			private int i;

			// Token: 0x04000E38 RID: 3640
			private AtlasRegion region;
		}
	}
}
