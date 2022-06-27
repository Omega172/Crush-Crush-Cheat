using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000217 RID: 535
	[CreateAssetMenu(menuName = "Spine/SkeletonData Modifiers/Blend Mode Materials", order = 200)]
	public class BlendModeMaterialsAsset : SkeletonDataModifierAsset
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x00079AE4 File Offset: 0x00077CE4
		public override void Apply(SkeletonData skeletonData)
		{
			BlendModeMaterialsAsset.ApplyMaterials(skeletonData, this.multiplyMaterialTemplate, this.screenMaterialTemplate, this.additiveMaterialTemplate, this.applyAdditiveMaterial);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00079B04 File Offset: 0x00077D04
		public static void ApplyMaterials(SkeletonData skeletonData, Material multiplyTemplate, Material screenTemplate, Material additiveTemplate, bool includeAdditiveSlots)
		{
			if (skeletonData == null)
			{
				throw new ArgumentNullException("skeletonData");
			}
			using (BlendModeMaterialsAsset.AtlasMaterialCache atlasMaterialCache = new BlendModeMaterialsAsset.AtlasMaterialCache())
			{
				List<Skin.SkinEntry> list = new List<Skin.SkinEntry>();
				SlotData[] items = skeletonData.Slots.Items;
				int i = 0;
				int count = skeletonData.Slots.Count;
				while (i < count)
				{
					SlotData slotData = items[i];
					if (slotData.blendMode != BlendMode.Normal)
					{
						if (includeAdditiveSlots || slotData.blendMode != BlendMode.Additive)
						{
							list.Clear();
							foreach (Skin skin in skeletonData.Skins)
							{
								skin.GetAttachments(i, list);
							}
							Material material = null;
							switch (slotData.blendMode)
							{
							case BlendMode.Additive:
								material = additiveTemplate;
								break;
							case BlendMode.Multiply:
								material = multiplyTemplate;
								break;
							case BlendMode.Screen:
								material = screenTemplate;
								break;
							}
							if (!(material == null))
							{
								foreach (Skin.SkinEntry skinEntry in list)
								{
									IHasRendererObject hasRendererObject = skinEntry.Attachment as IHasRendererObject;
									if (hasRendererObject != null)
									{
										hasRendererObject.RendererObject = atlasMaterialCache.CloneAtlasRegionWithMaterial((AtlasRegion)hasRendererObject.RendererObject, material);
									}
								}
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x04000E1B RID: 3611
		public Material multiplyMaterialTemplate;

		// Token: 0x04000E1C RID: 3612
		public Material screenMaterialTemplate;

		// Token: 0x04000E1D RID: 3613
		public Material additiveMaterialTemplate;

		// Token: 0x04000E1E RID: 3614
		public bool applyAdditiveMaterial = true;

		// Token: 0x02000218 RID: 536
		private class AtlasMaterialCache : IDisposable
		{
			// Token: 0x06001124 RID: 4388 RVA: 0x00079CF8 File Offset: 0x00077EF8
			public AtlasRegion CloneAtlasRegionWithMaterial(AtlasRegion originalRegion, Material materialTemplate)
			{
				AtlasRegion atlasRegion = originalRegion.Clone();
				atlasRegion.page = this.GetAtlasPageWithMaterial(originalRegion.page, materialTemplate);
				return atlasRegion;
			}

			// Token: 0x06001125 RID: 4389 RVA: 0x00079D20 File Offset: 0x00077F20
			private AtlasPage GetAtlasPageWithMaterial(AtlasPage originalPage, Material materialTemplate)
			{
				if (originalPage == null)
				{
					throw new ArgumentNullException("originalPage");
				}
				AtlasPage atlasPage = null;
				KeyValuePair<AtlasPage, Material> key = new KeyValuePair<AtlasPage, Material>(originalPage, materialTemplate);
				this.cache.TryGetValue(key, out atlasPage);
				if (atlasPage == null)
				{
					atlasPage = originalPage.Clone();
					Material material = originalPage.rendererObject as Material;
					atlasPage.rendererObject = new Material(materialTemplate)
					{
						name = material.name + " " + materialTemplate.name,
						mainTexture = material.mainTexture
					};
					this.cache.Add(key, atlasPage);
				}
				return atlasPage;
			}

			// Token: 0x06001126 RID: 4390 RVA: 0x00079DB8 File Offset: 0x00077FB8
			public void Dispose()
			{
				this.cache.Clear();
			}

			// Token: 0x04000E1F RID: 3615
			private readonly Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage> cache = new Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage>();
		}
	}
}
