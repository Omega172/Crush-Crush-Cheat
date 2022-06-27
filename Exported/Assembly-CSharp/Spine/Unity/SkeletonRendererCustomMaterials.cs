using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001F7 RID: 503
	[ExecuteInEditMode]
	public class SkeletonRendererCustomMaterials : MonoBehaviour
	{
		// Token: 0x0600108D RID: 4237 RVA: 0x00074074 File Offset: 0x00072274
		private void SetCustomSlotMaterials()
		{
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customSlotMaterials.Count; i++)
			{
				SkeletonRendererCustomMaterials.SlotMaterialOverride slotMaterialOverride = this.customSlotMaterials[i];
				if (!slotMaterialOverride.overrideDisabled && !string.IsNullOrEmpty(slotMaterialOverride.slotName))
				{
					Slot key = this.skeletonRenderer.skeleton.FindSlot(slotMaterialOverride.slotName);
					this.skeletonRenderer.CustomSlotMaterials[key] = slotMaterialOverride.material;
				}
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00074118 File Offset: 0x00072318
		private void RemoveCustomSlotMaterials()
		{
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customSlotMaterials.Count; i++)
			{
				SkeletonRendererCustomMaterials.SlotMaterialOverride slotMaterialOverride = this.customSlotMaterials[i];
				if (!string.IsNullOrEmpty(slotMaterialOverride.slotName))
				{
					Slot key = this.skeletonRenderer.skeleton.FindSlot(slotMaterialOverride.slotName);
					Material x;
					if (this.skeletonRenderer.CustomSlotMaterials.TryGetValue(key, out x))
					{
						if (!(x != slotMaterialOverride.material))
						{
							this.skeletonRenderer.CustomSlotMaterials.Remove(key);
						}
					}
				}
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x000741E0 File Offset: 0x000723E0
		private void SetCustomMaterialOverrides()
		{
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonRendererCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				if (!atlasMaterialOverride.overrideDisabled)
				{
					this.skeletonRenderer.CustomMaterialOverride[atlasMaterialOverride.originalMaterial] = atlasMaterialOverride.replacementMaterial;
				}
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00074264 File Offset: 0x00072464
		private void RemoveCustomMaterialOverrides()
		{
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonRendererCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				Material x;
				if (this.skeletonRenderer.CustomMaterialOverride.TryGetValue(atlasMaterialOverride.originalMaterial, out x))
				{
					if (!(x != atlasMaterialOverride.replacementMaterial))
					{
						this.skeletonRenderer.CustomMaterialOverride.Remove(atlasMaterialOverride.originalMaterial);
					}
				}
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00074308 File Offset: 0x00072508
		private void OnEnable()
		{
			if (this.skeletonRenderer == null)
			{
				this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			}
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			this.skeletonRenderer.Initialize(false);
			this.SetCustomMaterialOverrides();
			this.SetCustomSlotMaterials();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00074368 File Offset: 0x00072568
		private void OnDisable()
		{
			if (this.skeletonRenderer == null)
			{
				Debug.LogError("skeletonRenderer == null");
				return;
			}
			this.RemoveCustomMaterialOverrides();
			this.RemoveCustomSlotMaterials();
		}

		// Token: 0x04000D8F RID: 3471
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000D90 RID: 3472
		[SerializeField]
		protected List<SkeletonRendererCustomMaterials.SlotMaterialOverride> customSlotMaterials = new List<SkeletonRendererCustomMaterials.SlotMaterialOverride>();

		// Token: 0x04000D91 RID: 3473
		[SerializeField]
		protected List<SkeletonRendererCustomMaterials.AtlasMaterialOverride> customMaterialOverrides = new List<SkeletonRendererCustomMaterials.AtlasMaterialOverride>();

		// Token: 0x020001F8 RID: 504
		[Serializable]
		public struct SlotMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.SlotMaterialOverride>
		{
			// Token: 0x06001093 RID: 4243 RVA: 0x000743A0 File Offset: 0x000725A0
			public bool Equals(SkeletonRendererCustomMaterials.SlotMaterialOverride other)
			{
				return this.overrideDisabled == other.overrideDisabled && this.slotName == other.slotName && this.material == other.material;
			}

			// Token: 0x04000D92 RID: 3474
			public bool overrideDisabled;

			// Token: 0x04000D93 RID: 3475
			[SpineSlot("", "", false, true, false)]
			public string slotName;

			// Token: 0x04000D94 RID: 3476
			public Material material;
		}

		// Token: 0x020001F9 RID: 505
		[Serializable]
		public struct AtlasMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.AtlasMaterialOverride>
		{
			// Token: 0x06001094 RID: 4244 RVA: 0x000743EC File Offset: 0x000725EC
			public bool Equals(SkeletonRendererCustomMaterials.AtlasMaterialOverride other)
			{
				return this.overrideDisabled == other.overrideDisabled && this.originalMaterial == other.originalMaterial && this.replacementMaterial == other.replacementMaterial;
			}

			// Token: 0x04000D95 RID: 3477
			public bool overrideDisabled;

			// Token: 0x04000D96 RID: 3478
			public Material originalMaterial;

			// Token: 0x04000D97 RID: 3479
			public Material replacementMaterial;
		}
	}
}
