using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001DD RID: 477
	[CreateAssetMenu(fileName = "New Spine SpriteAtlas Asset", menuName = "Spine/Spine SpriteAtlas Asset")]
	public class SpineSpriteAtlasAsset : AtlasAssetBase
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0006DCBC File Offset: 0x0006BEBC
		public override bool IsLoaded
		{
			get
			{
				return this.atlas != null;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0006DCCC File Offset: 0x0006BECC
		public override IEnumerable<Material> Materials
		{
			get
			{
				return this.materials;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0006DCD4 File Offset: 0x0006BED4
		public override int MaterialCount
		{
			get
			{
				return (this.materials != null) ? this.materials.Length : 0;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0006DCF0 File Offset: 0x0006BEF0
		public override Material PrimaryMaterial
		{
			get
			{
				return this.materials[0];
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0006DCFC File Offset: 0x0006BEFC
		public static SpineSpriteAtlasAsset CreateRuntimeInstance(SpriteAtlas spriteAtlasFile, Material[] materials, bool initialize)
		{
			SpineSpriteAtlasAsset spineSpriteAtlasAsset = ScriptableObject.CreateInstance<SpineSpriteAtlasAsset>();
			spineSpriteAtlasAsset.Reset();
			spineSpriteAtlasAsset.spriteAtlasFile = spriteAtlasFile;
			spineSpriteAtlasAsset.materials = materials;
			if (initialize)
			{
				spineSpriteAtlasAsset.GetAtlas();
			}
			return spineSpriteAtlasAsset;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0006DD34 File Offset: 0x0006BF34
		private void Reset()
		{
			this.Clear();
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0006DD3C File Offset: 0x0006BF3C
		public override void Clear()
		{
			this.atlas = null;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0006DD48 File Offset: 0x0006BF48
		public override Atlas GetAtlas()
		{
			if (this.spriteAtlasFile == null)
			{
				Debug.LogError("SpriteAtlas file not set for SpineSpriteAtlasAsset: " + base.name, this);
				this.Clear();
				return null;
			}
			if (this.materials == null || this.materials.Length == 0)
			{
				Debug.LogError("Materials not set for SpineSpriteAtlasAsset: " + base.name, this);
				this.Clear();
				return null;
			}
			if (this.atlas != null)
			{
				return this.atlas;
			}
			Atlas result;
			try
			{
				this.atlas = this.LoadAtlas(this.spriteAtlasFile);
				result = this.atlas;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error analyzing SpriteAtlas for SpineSpriteAtlasAsset: ",
					base.name,
					"\n",
					ex.Message,
					"\n",
					ex.StackTrace
				}), this);
				result = null;
			}
			return result;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0006DE60 File Offset: 0x0006C060
		protected void AssignRegionsFromSavedRegions(Sprite[] sprites, Atlas usedAtlas)
		{
			if (this.savedRegions == null || this.savedRegions.Length != sprites.Length)
			{
				return;
			}
			int num = 0;
			foreach (AtlasRegion atlasRegion in usedAtlas)
			{
				SpineSpriteAtlasAsset.SavedRegionInfo savedRegionInfo = this.savedRegions[num];
				AtlasPage page = atlasRegion.page;
				atlasRegion.degrees = ((savedRegionInfo.packingRotation != SpritePackingRotation.None) ? 90 : 0);
				atlasRegion.rotate = (atlasRegion.degrees != 0);
				float x = savedRegionInfo.x;
				float y = savedRegionInfo.y;
				float width = savedRegionInfo.width;
				float height = savedRegionInfo.height;
				atlasRegion.u = x / (float)page.width;
				atlasRegion.v = y / (float)page.height;
				if (atlasRegion.rotate)
				{
					atlasRegion.u2 = (x + height) / (float)page.width;
					atlasRegion.v2 = (y + width) / (float)page.height;
				}
				else
				{
					atlasRegion.u2 = (x + width) / (float)page.width;
					atlasRegion.v2 = (y + height) / (float)page.height;
				}
				atlasRegion.x = (int)x;
				atlasRegion.y = (int)y;
				atlasRegion.width = Math.Abs((int)width);
				atlasRegion.height = Math.Abs((int)height);
				float v = atlasRegion.v;
				atlasRegion.v = atlasRegion.v2;
				atlasRegion.v2 = v;
				atlasRegion.originalWidth = (int)width;
				atlasRegion.originalHeight = (int)height;
				atlasRegion.offsetX = 0f;
				atlasRegion.offsetY = 0f;
				num++;
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0006E02C File Offset: 0x0006C22C
		private Atlas LoadAtlas(SpriteAtlas spriteAtlas)
		{
			List<AtlasPage> list = new List<AtlasPage>();
			List<AtlasRegion> list2 = new List<AtlasRegion>();
			Sprite[] array = new Sprite[spriteAtlas.spriteCount];
			spriteAtlas.GetSprites(array);
			if (array.Length == 0)
			{
				return new Atlas(list, list2);
			}
			Texture2D texture2D = SpineSpriteAtlasAsset.AccessPackedTexture(array);
			Material material = this.materials[0];
			material.mainTexture = texture2D;
			AtlasPage atlasPage = new AtlasPage();
			atlasPage.name = spriteAtlas.name;
			atlasPage.width = texture2D.width;
			atlasPage.height = texture2D.height;
			atlasPage.format = Format.RGBA8888;
			atlasPage.minFilter = TextureFilter.Linear;
			atlasPage.magFilter = TextureFilter.Linear;
			atlasPage.uWrap = TextureWrap.ClampToEdge;
			atlasPage.vWrap = TextureWrap.ClampToEdge;
			atlasPage.rendererObject = material;
			list.Add(atlasPage);
			array = SpineSpriteAtlasAsset.AccessPackedSprites(spriteAtlas);
			for (int i = 0; i < array.Length; i++)
			{
				Sprite sprite = array[i];
				AtlasRegion atlasRegion = new AtlasRegion();
				atlasRegion.name = sprite.name.Replace("(Clone)", string.Empty);
				atlasRegion.page = atlasPage;
				atlasRegion.degrees = ((sprite.packingRotation != SpritePackingRotation.None) ? 90 : 0);
				atlasRegion.rotate = (atlasRegion.degrees != 0);
				atlasRegion.u2 = 1f;
				atlasRegion.v2 = 1f;
				atlasRegion.width = atlasPage.width;
				atlasRegion.height = atlasPage.height;
				atlasRegion.originalWidth = atlasPage.width;
				atlasRegion.originalHeight = atlasPage.height;
				atlasRegion.index = i;
				list2.Add(atlasRegion);
			}
			Atlas atlas = new Atlas(list, list2);
			this.AssignRegionsFromSavedRegions(array, atlas);
			return atlas;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0006E1E4 File Offset: 0x0006C3E4
		public static Texture2D AccessPackedTexture(Sprite[] sprites)
		{
			return sprites[0].texture;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0006E1F0 File Offset: 0x0006C3F0
		public static Sprite[] AccessPackedSprites(SpriteAtlas spriteAtlas)
		{
			Sprite[] array = null;
			if (array == null)
			{
				array = new Sprite[spriteAtlas.spriteCount];
				spriteAtlas.GetSprites(array);
				if (array.Length == 0)
				{
					return null;
				}
			}
			return array;
		}

		// Token: 0x04000CB4 RID: 3252
		public SpriteAtlas spriteAtlasFile;

		// Token: 0x04000CB5 RID: 3253
		public Material[] materials;

		// Token: 0x04000CB6 RID: 3254
		protected Atlas atlas;

		// Token: 0x04000CB7 RID: 3255
		public bool updateRegionsInPlayMode;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		protected SpineSpriteAtlasAsset.SavedRegionInfo[] savedRegions;

		// Token: 0x020001DE RID: 478
		[Serializable]
		protected class SavedRegionInfo
		{
			// Token: 0x04000CB9 RID: 3257
			public float x;

			// Token: 0x04000CBA RID: 3258
			public float y;

			// Token: 0x04000CBB RID: 3259
			public float width;

			// Token: 0x04000CBC RID: 3260
			public float height;

			// Token: 0x04000CBD RID: 3261
			public SpritePackingRotation packingRotation;
		}
	}
}
