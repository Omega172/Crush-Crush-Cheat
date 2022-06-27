using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001F4 RID: 500
	[ExecuteInEditMode]
	public class SkeletonGraphicCustomMaterials : MonoBehaviour
	{
		// Token: 0x06001084 RID: 4228 RVA: 0x00073CE4 File Offset: 0x00071EE4
		private void SetCustomMaterialOverrides()
		{
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonGraphicCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				if (atlasMaterialOverride.overrideEnabled)
				{
					this.skeletonGraphic.CustomMaterialOverride[atlasMaterialOverride.originalTexture] = atlasMaterialOverride.replacementMaterial;
				}
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00073D60 File Offset: 0x00071F60
		private void RemoveCustomMaterialOverrides()
		{
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonGraphicCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				Material x;
				if (this.skeletonGraphic.CustomMaterialOverride.TryGetValue(atlasMaterialOverride.originalTexture, out x))
				{
					if (!(x != atlasMaterialOverride.replacementMaterial))
					{
						this.skeletonGraphic.CustomMaterialOverride.Remove(atlasMaterialOverride.originalTexture);
					}
				}
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00073E04 File Offset: 0x00072004
		private void SetCustomTextureOverrides()
		{
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			for (int i = 0; i < this.customTextureOverrides.Count; i++)
			{
				SkeletonGraphicCustomMaterials.AtlasTextureOverride atlasTextureOverride = this.customTextureOverrides[i];
				if (atlasTextureOverride.overrideEnabled)
				{
					this.skeletonGraphic.CustomTextureOverride[atlasTextureOverride.originalTexture] = atlasTextureOverride.replacementTexture;
				}
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00073E80 File Offset: 0x00072080
		private void RemoveCustomTextureOverrides()
		{
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			for (int i = 0; i < this.customTextureOverrides.Count; i++)
			{
				SkeletonGraphicCustomMaterials.AtlasTextureOverride atlasTextureOverride = this.customTextureOverrides[i];
				Texture x;
				if (this.skeletonGraphic.CustomTextureOverride.TryGetValue(atlasTextureOverride.originalTexture, out x))
				{
					if (!(x != atlasTextureOverride.replacementTexture))
					{
						this.skeletonGraphic.CustomTextureOverride.Remove(atlasTextureOverride.originalTexture);
					}
				}
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00073F24 File Offset: 0x00072124
		private void OnEnable()
		{
			if (this.skeletonGraphic == null)
			{
				this.skeletonGraphic = base.GetComponent<SkeletonGraphic>();
			}
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			this.skeletonGraphic.Initialize(false);
			this.SetCustomMaterialOverrides();
			this.SetCustomTextureOverrides();
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00073F84 File Offset: 0x00072184
		private void OnDisable()
		{
			if (this.skeletonGraphic == null)
			{
				Debug.LogError("skeletonGraphic == null");
				return;
			}
			this.RemoveCustomMaterialOverrides();
			this.RemoveCustomTextureOverrides();
		}

		// Token: 0x04000D86 RID: 3462
		public SkeletonGraphic skeletonGraphic;

		// Token: 0x04000D87 RID: 3463
		[SerializeField]
		protected List<SkeletonGraphicCustomMaterials.AtlasMaterialOverride> customMaterialOverrides = new List<SkeletonGraphicCustomMaterials.AtlasMaterialOverride>();

		// Token: 0x04000D88 RID: 3464
		[SerializeField]
		protected List<SkeletonGraphicCustomMaterials.AtlasTextureOverride> customTextureOverrides = new List<SkeletonGraphicCustomMaterials.AtlasTextureOverride>();

		// Token: 0x020001F5 RID: 501
		[Serializable]
		public struct AtlasMaterialOverride : IEquatable<SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
		{
			// Token: 0x0600108A RID: 4234 RVA: 0x00073FBC File Offset: 0x000721BC
			public bool Equals(SkeletonGraphicCustomMaterials.AtlasMaterialOverride other)
			{
				return this.overrideEnabled == other.overrideEnabled && this.originalTexture == other.originalTexture && this.replacementMaterial == other.replacementMaterial;
			}

			// Token: 0x04000D89 RID: 3465
			public bool overrideEnabled;

			// Token: 0x04000D8A RID: 3466
			public Texture originalTexture;

			// Token: 0x04000D8B RID: 3467
			public Material replacementMaterial;
		}

		// Token: 0x020001F6 RID: 502
		[Serializable]
		public struct AtlasTextureOverride : IEquatable<SkeletonGraphicCustomMaterials.AtlasTextureOverride>
		{
			// Token: 0x0600108B RID: 4235 RVA: 0x00074008 File Offset: 0x00072208
			public bool Equals(SkeletonGraphicCustomMaterials.AtlasTextureOverride other)
			{
				return this.overrideEnabled == other.overrideEnabled && this.originalTexture == other.originalTexture && this.replacementTexture == other.replacementTexture;
			}

			// Token: 0x04000D8C RID: 3468
			public bool overrideEnabled;

			// Token: 0x04000D8D RID: 3469
			public Texture originalTexture;

			// Token: 0x04000D8E RID: 3470
			public Texture replacementTexture;
		}
	}
}
