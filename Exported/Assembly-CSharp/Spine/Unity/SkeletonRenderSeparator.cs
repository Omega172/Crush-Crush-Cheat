using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Spine.Unity
{
	// Token: 0x020001F0 RID: 496
	[HelpURL("http://esotericsoftware.com/spine-unity-skeletonrenderseparator")]
	[ExecuteInEditMode]
	public class SkeletonRenderSeparator : MonoBehaviour
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00072A3C File Offset: 0x00070C3C
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00072A44 File Offset: 0x00070C44
		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return this.skeletonRenderer;
			}
			set
			{
				if (this.skeletonRenderer != null)
				{
					this.skeletonRenderer.GenerateMeshOverride -= this.HandleRender;
				}
				this.skeletonRenderer = value;
				if (value == null)
				{
					base.enabled = false;
				}
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00072A94 File Offset: 0x00070C94
		public static SkeletonRenderSeparator AddToSkeletonRenderer(SkeletonRenderer skeletonRenderer, int sortingLayerID = 0, int extraPartsRenderers = 0, int sortingOrderIncrement = 5, int baseSortingOrder = 0, bool addMinimumPartsRenderers = true)
		{
			if (skeletonRenderer == null)
			{
				Debug.Log("Tried to add SkeletonRenderSeparator to a null SkeletonRenderer reference.");
				return null;
			}
			SkeletonRenderSeparator skeletonRenderSeparator = skeletonRenderer.gameObject.AddComponent<SkeletonRenderSeparator>();
			skeletonRenderSeparator.skeletonRenderer = skeletonRenderer;
			skeletonRenderer.Initialize(false);
			int num = extraPartsRenderers;
			if (addMinimumPartsRenderers)
			{
				num = extraPartsRenderers + skeletonRenderer.separatorSlots.Count + 1;
			}
			Transform transform = skeletonRenderer.transform;
			List<SkeletonPartsRenderer> list = skeletonRenderSeparator.partsRenderers;
			for (int i = 0; i < num; i++)
			{
				SkeletonPartsRenderer skeletonPartsRenderer = SkeletonPartsRenderer.NewPartsRendererGameObject(transform, i.ToString(), 0);
				MeshRenderer meshRenderer = skeletonPartsRenderer.MeshRenderer;
				meshRenderer.sortingLayerID = sortingLayerID;
				meshRenderer.sortingOrder = baseSortingOrder + i * sortingOrderIncrement;
				list.Add(skeletonPartsRenderer);
			}
			skeletonRenderSeparator.OnEnable();
			return skeletonRenderSeparator;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00072B50 File Offset: 0x00070D50
		public SkeletonPartsRenderer AddPartsRenderer(int sortingOrderIncrement = 5, string name = null)
		{
			int sortingLayerID = 0;
			int sortingOrder = 0;
			if (this.partsRenderers.Count > 0)
			{
				SkeletonPartsRenderer skeletonPartsRenderer = this.partsRenderers[this.partsRenderers.Count - 1];
				MeshRenderer meshRenderer = skeletonPartsRenderer.MeshRenderer;
				sortingLayerID = meshRenderer.sortingLayerID;
				sortingOrder = meshRenderer.sortingOrder + sortingOrderIncrement;
			}
			if (string.IsNullOrEmpty(name))
			{
				name = this.partsRenderers.Count.ToString();
			}
			SkeletonPartsRenderer skeletonPartsRenderer2 = SkeletonPartsRenderer.NewPartsRendererGameObject(this.skeletonRenderer.transform, name, 0);
			this.partsRenderers.Add(skeletonPartsRenderer2);
			MeshRenderer meshRenderer2 = skeletonPartsRenderer2.MeshRenderer;
			meshRenderer2.sortingLayerID = sortingLayerID;
			meshRenderer2.sortingOrder = sortingOrder;
			return skeletonPartsRenderer2;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00072C00 File Offset: 0x00070E00
		public void OnEnable()
		{
			if (this.skeletonRenderer == null)
			{
				return;
			}
			if (this.copiedBlock == null)
			{
				this.copiedBlock = new MaterialPropertyBlock();
			}
			this.mainMeshRenderer = this.skeletonRenderer.GetComponent<MeshRenderer>();
			this.skeletonRenderer.GenerateMeshOverride -= this.HandleRender;
			this.skeletonRenderer.GenerateMeshOverride += this.HandleRender;
			if (this.copyMeshRendererFlags)
			{
				LightProbeUsage lightProbeUsage = this.mainMeshRenderer.lightProbeUsage;
				bool receiveShadows = this.mainMeshRenderer.receiveShadows;
				ReflectionProbeUsage reflectionProbeUsage = this.mainMeshRenderer.reflectionProbeUsage;
				ShadowCastingMode shadowCastingMode = this.mainMeshRenderer.shadowCastingMode;
				Transform probeAnchor = this.mainMeshRenderer.probeAnchor;
				for (int i = 0; i < this.partsRenderers.Count; i++)
				{
					SkeletonPartsRenderer skeletonPartsRenderer = this.partsRenderers[i];
					if (!(skeletonPartsRenderer == null))
					{
						MeshRenderer meshRenderer = skeletonPartsRenderer.MeshRenderer;
						meshRenderer.lightProbeUsage = lightProbeUsage;
						meshRenderer.receiveShadows = receiveShadows;
						meshRenderer.reflectionProbeUsage = reflectionProbeUsage;
						meshRenderer.shadowCastingMode = shadowCastingMode;
						meshRenderer.probeAnchor = probeAnchor;
					}
				}
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00072D30 File Offset: 0x00070F30
		public void OnDisable()
		{
			if (this.skeletonRenderer == null)
			{
				return;
			}
			this.skeletonRenderer.GenerateMeshOverride -= this.HandleRender;
			this.skeletonRenderer.LateUpdate();
			foreach (SkeletonPartsRenderer skeletonPartsRenderer in this.partsRenderers)
			{
				if (skeletonPartsRenderer != null)
				{
					skeletonPartsRenderer.ClearMesh();
				}
			}
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00072DD8 File Offset: 0x00070FD8
		private void HandleRender(SkeletonRendererInstruction instruction)
		{
			int count = this.partsRenderers.Count;
			if (count <= 0)
			{
				return;
			}
			if (this.copyPropertyBlock)
			{
				this.mainMeshRenderer.GetPropertyBlock(this.copiedBlock);
			}
			MeshGenerator.Settings settings = new MeshGenerator.Settings
			{
				addNormals = this.skeletonRenderer.addNormals,
				calculateTangents = this.skeletonRenderer.calculateTangents,
				immutableTriangles = false,
				pmaVertexColors = this.skeletonRenderer.pmaVertexColors,
				tintBlack = this.skeletonRenderer.tintBlack,
				useClipping = true,
				zSpacing = this.skeletonRenderer.zSpacing
			};
			ExposedList<SubmeshInstruction> submeshInstructions = instruction.submeshInstructions;
			SubmeshInstruction[] items = submeshInstructions.Items;
			int num = submeshInstructions.Count - 1;
			int i = 0;
			SkeletonPartsRenderer skeletonPartsRenderer = this.partsRenderers[i];
			int j = 0;
			int startSubmesh = 0;
			while (j <= num)
			{
				if (!(skeletonPartsRenderer == null))
				{
					if (items[j].forceSeparate || j == num)
					{
						MeshGenerator meshGenerator = skeletonPartsRenderer.MeshGenerator;
						meshGenerator.settings = settings;
						if (this.copyPropertyBlock)
						{
							skeletonPartsRenderer.SetPropertyBlock(this.copiedBlock);
						}
						skeletonPartsRenderer.RenderParts(instruction.submeshInstructions, startSubmesh, j + 1);
						startSubmesh = j + 1;
						i++;
						if (i >= count)
						{
							break;
						}
						skeletonPartsRenderer = this.partsRenderers[i];
					}
				}
				j++;
			}
			while (i < count)
			{
				skeletonPartsRenderer = this.partsRenderers[i];
				if (skeletonPartsRenderer != null)
				{
					this.partsRenderers[i].ClearMesh();
				}
				i++;
			}
		}

		// Token: 0x04000D53 RID: 3411
		public const int DefaultSortingOrderIncrement = 5;

		// Token: 0x04000D54 RID: 3412
		[SerializeField]
		protected SkeletonRenderer skeletonRenderer;

		// Token: 0x04000D55 RID: 3413
		private MeshRenderer mainMeshRenderer;

		// Token: 0x04000D56 RID: 3414
		public bool copyPropertyBlock = true;

		// Token: 0x04000D57 RID: 3415
		[Tooltip("Copies MeshRenderer flags into each parts renderer")]
		public bool copyMeshRendererFlags = true;

		// Token: 0x04000D58 RID: 3416
		public List<SkeletonPartsRenderer> partsRenderers = new List<SkeletonPartsRenderer>();

		// Token: 0x04000D59 RID: 3417
		private MaterialPropertyBlock copiedBlock;
	}
}
