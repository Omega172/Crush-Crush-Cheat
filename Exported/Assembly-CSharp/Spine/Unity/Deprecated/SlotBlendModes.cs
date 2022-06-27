using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Deprecated
{
	// Token: 0x02000203 RID: 515
	[DisallowMultipleComponent]
	[Obsolete("The spine-unity 3.7 runtime introduced SkeletonDataModifierAssets BlendModeMaterials which replaced SlotBlendModes. Will be removed in spine-unity 3.9.", false)]
	public class SlotBlendModes : MonoBehaviour
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x00075E40 File Offset: 0x00074040
		internal static Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount> MaterialTable
		{
			get
			{
				if (SlotBlendModes.materialTable == null)
				{
					SlotBlendModes.materialTable = new Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount>();
				}
				return SlotBlendModes.materialTable;
			}
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00075E5C File Offset: 0x0007405C
		internal static Material GetOrAddMaterialFor(Material materialSource, Texture2D texture)
		{
			if (materialSource == null || texture == null)
			{
				return null;
			}
			Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount> dictionary = SlotBlendModes.MaterialTable;
			SlotBlendModes.MaterialTexturePair key = new SlotBlendModes.MaterialTexturePair
			{
				material = materialSource,
				texture2D = texture
			};
			SlotBlendModes.MaterialWithRefcount materialWithRefcount;
			if (!dictionary.TryGetValue(key, out materialWithRefcount))
			{
				materialWithRefcount = new SlotBlendModes.MaterialWithRefcount(new Material(materialSource));
				Material materialClone = materialWithRefcount.materialClone;
				materialClone.name = "(Clone)" + texture.name + "-" + materialSource.name;
				materialClone.mainTexture = texture;
				dictionary[key] = materialWithRefcount;
			}
			else
			{
				materialWithRefcount.refcount++;
			}
			return materialWithRefcount.materialClone;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00075F14 File Offset: 0x00074114
		internal static SlotBlendModes.MaterialWithRefcount GetExistingMaterialFor(Material materialSource, Texture2D texture)
		{
			if (materialSource == null || texture == null)
			{
				return null;
			}
			Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount> dictionary = SlotBlendModes.MaterialTable;
			SlotBlendModes.MaterialTexturePair key = new SlotBlendModes.MaterialTexturePair
			{
				material = materialSource,
				texture2D = texture
			};
			SlotBlendModes.MaterialWithRefcount result;
			if (!dictionary.TryGetValue(key, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00075F70 File Offset: 0x00074170
		internal static void RemoveMaterialFromTable(Material materialSource, Texture2D texture)
		{
			Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount> dictionary = SlotBlendModes.MaterialTable;
			SlotBlendModes.MaterialTexturePair key = new SlotBlendModes.MaterialTexturePair
			{
				material = materialSource,
				texture2D = texture
			};
			dictionary.Remove(key);
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x00075FA8 File Offset: 0x000741A8
		// (set) Token: 0x060010DB RID: 4315 RVA: 0x00075FB0 File Offset: 0x000741B0
		public bool Applied { get; private set; }

		// Token: 0x060010DC RID: 4316 RVA: 0x00075FBC File Offset: 0x000741BC
		private void Start()
		{
			if (!this.Applied)
			{
				this.Apply();
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00075FD0 File Offset: 0x000741D0
		private void OnDestroy()
		{
			if (this.Applied)
			{
				this.Remove();
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00075FE4 File Offset: 0x000741E4
		public void Apply()
		{
			this.GetTexture();
			if (this.texture == null)
			{
				return;
			}
			SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
			if (component == null)
			{
				return;
			}
			Dictionary<Slot, Material> customSlotMaterials = component.CustomSlotMaterials;
			int num = 0;
			foreach (Slot slot in component.Skeleton.Slots)
			{
				BlendMode blendMode = slot.data.blendMode;
				if (blendMode != BlendMode.Multiply)
				{
					if (blendMode == BlendMode.Screen)
					{
						if (this.screenMaterialSource != null)
						{
							customSlotMaterials[slot] = SlotBlendModes.GetOrAddMaterialFor(this.screenMaterialSource, this.texture);
							num++;
						}
					}
				}
				else if (this.multiplyMaterialSource != null)
				{
					customSlotMaterials[slot] = SlotBlendModes.GetOrAddMaterialFor(this.multiplyMaterialSource, this.texture);
					num++;
				}
			}
			this.slotsWithCustomMaterial = new SlotBlendModes.SlotMaterialTextureTuple[num];
			int num2 = 0;
			foreach (Slot slot2 in component.Skeleton.Slots)
			{
				BlendMode blendMode = slot2.data.blendMode;
				if (blendMode != BlendMode.Multiply)
				{
					if (blendMode == BlendMode.Screen)
					{
						if (this.screenMaterialSource != null)
						{
							this.slotsWithCustomMaterial[num2++] = new SlotBlendModes.SlotMaterialTextureTuple(slot2, this.screenMaterialSource, this.texture);
						}
					}
				}
				else if (this.multiplyMaterialSource != null)
				{
					this.slotsWithCustomMaterial[num2++] = new SlotBlendModes.SlotMaterialTextureTuple(slot2, this.multiplyMaterialSource, this.texture);
				}
			}
			this.Applied = true;
			component.LateUpdate();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0007621C File Offset: 0x0007441C
		public void Remove()
		{
			this.GetTexture();
			if (this.texture == null)
			{
				return;
			}
			SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
			if (component == null)
			{
				return;
			}
			Dictionary<Slot, Material> customSlotMaterials = component.CustomSlotMaterials;
			foreach (SlotBlendModes.SlotMaterialTextureTuple slotMaterialTextureTuple in this.slotsWithCustomMaterial)
			{
				Slot slot = slotMaterialTextureTuple.slot;
				Material material = slotMaterialTextureTuple.material;
				Texture2D texture2D = slotMaterialTextureTuple.texture2D;
				SlotBlendModes.MaterialWithRefcount existingMaterialFor = SlotBlendModes.GetExistingMaterialFor(material, texture2D);
				if (--existingMaterialFor.refcount == 0)
				{
					SlotBlendModes.RemoveMaterialFromTable(material, texture2D);
				}
				Material objA;
				if (customSlotMaterials.TryGetValue(slot, out objA))
				{
					Material objB = (existingMaterialFor != null) ? existingMaterialFor.materialClone : null;
					if (object.ReferenceEquals(objA, objB))
					{
						customSlotMaterials.Remove(slot);
					}
				}
			}
			this.slotsWithCustomMaterial = null;
			this.Applied = false;
			if (component.valid)
			{
				component.LateUpdate();
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0007632C File Offset: 0x0007452C
		public void GetTexture()
		{
			if (this.texture == null)
			{
				SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
				if (component == null)
				{
					return;
				}
				SkeletonDataAsset skeletonDataAsset = component.skeletonDataAsset;
				if (skeletonDataAsset == null)
				{
					return;
				}
				AtlasAssetBase atlasAssetBase = skeletonDataAsset.atlasAssets[0];
				if (atlasAssetBase == null)
				{
					return;
				}
				Material primaryMaterial = atlasAssetBase.PrimaryMaterial;
				if (primaryMaterial == null)
				{
					return;
				}
				this.texture = (primaryMaterial.mainTexture as Texture2D);
			}
		}

		// Token: 0x04000DCE RID: 3534
		private static Dictionary<SlotBlendModes.MaterialTexturePair, SlotBlendModes.MaterialWithRefcount> materialTable;

		// Token: 0x04000DCF RID: 3535
		public Material multiplyMaterialSource;

		// Token: 0x04000DD0 RID: 3536
		public Material screenMaterialSource;

		// Token: 0x04000DD1 RID: 3537
		private Texture2D texture;

		// Token: 0x04000DD2 RID: 3538
		private SlotBlendModes.SlotMaterialTextureTuple[] slotsWithCustomMaterial = new SlotBlendModes.SlotMaterialTextureTuple[0];

		// Token: 0x02000204 RID: 516
		public struct MaterialTexturePair
		{
			// Token: 0x04000DD4 RID: 3540
			public Texture2D texture2D;

			// Token: 0x04000DD5 RID: 3541
			public Material material;
		}

		// Token: 0x02000205 RID: 517
		internal class MaterialWithRefcount
		{
			// Token: 0x060010E1 RID: 4321 RVA: 0x000763B0 File Offset: 0x000745B0
			public MaterialWithRefcount(Material mat)
			{
				this.materialClone = mat;
			}

			// Token: 0x04000DD6 RID: 3542
			public Material materialClone;

			// Token: 0x04000DD7 RID: 3543
			public int refcount = 1;
		}

		// Token: 0x02000206 RID: 518
		internal struct SlotMaterialTextureTuple
		{
			// Token: 0x060010E2 RID: 4322 RVA: 0x000763C8 File Offset: 0x000745C8
			public SlotMaterialTextureTuple(Slot slot, Material material, Texture2D texture)
			{
				this.slot = slot;
				this.material = material;
				this.texture2D = texture;
			}

			// Token: 0x04000DD8 RID: 3544
			public Slot slot;

			// Token: 0x04000DD9 RID: 3545
			public Texture2D texture2D;

			// Token: 0x04000DDA RID: 3546
			public Material material;
		}
	}
}
