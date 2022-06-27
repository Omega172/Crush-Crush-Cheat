using System;
using System.IO;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001DC RID: 476
	public class MaterialsTextureLoader : TextureLoader
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x0006DBA8 File Offset: 0x0006BDA8
		public MaterialsTextureLoader(SpineAtlasAsset atlasAsset)
		{
			this.atlasAsset = atlasAsset;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0006DBB8 File Offset: 0x0006BDB8
		public void Load(AtlasPage page, string path)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			Material material = null;
			foreach (Material material2 in this.atlasAsset.materials)
			{
				if (material2.mainTexture == null)
				{
					Debug.LogError("Material is missing texture: " + material2.name, material2);
					return;
				}
				if (material2.mainTexture.name == fileNameWithoutExtension)
				{
					material = material2;
					break;
				}
			}
			if (material == null)
			{
				Debug.LogError("Material with texture name \"" + fileNameWithoutExtension + "\" not found for atlas asset: " + this.atlasAsset.name, this.atlasAsset);
				return;
			}
			page.rendererObject = material;
			if (page.width == 0 || page.height == 0)
			{
				page.width = material.mainTexture.width;
				page.height = material.mainTexture.height;
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0006DCB0 File Offset: 0x0006BEB0
		public void Unload(object texture)
		{
		}

		// Token: 0x04000CB3 RID: 3251
		private SpineAtlasAsset atlasAsset;
	}
}
