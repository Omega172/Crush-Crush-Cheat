using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001EF RID: 495
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class SkeletonPartsRenderer : MonoBehaviour
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0007277C File Offset: 0x0007097C
		public MeshGenerator MeshGenerator
		{
			get
			{
				this.LazyIntialize();
				return this.meshGenerator;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0007278C File Offset: 0x0007098C
		public MeshRenderer MeshRenderer
		{
			get
			{
				this.LazyIntialize();
				return this.meshRenderer;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0007279C File Offset: 0x0007099C
		public MeshFilter MeshFilter
		{
			get
			{
				this.LazyIntialize();
				return this.meshFilter;
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x000727AC File Offset: 0x000709AC
		private void LazyIntialize()
		{
			if (this.buffers == null)
			{
				this.buffers = new MeshRendererBuffers();
				this.buffers.Initialize();
				if (this.meshGenerator != null)
				{
					return;
				}
				this.meshGenerator = new MeshGenerator();
				this.meshFilter = base.GetComponent<MeshFilter>();
				this.meshRenderer = base.GetComponent<MeshRenderer>();
				this.currentInstructions.Clear();
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00072814 File Offset: 0x00070A14
		public void ClearMesh()
		{
			this.LazyIntialize();
			this.meshFilter.sharedMesh = null;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00072828 File Offset: 0x00070A28
		public void RenderParts(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
		{
			this.LazyIntialize();
			MeshRendererBuffers.SmartMesh nextMesh = this.buffers.GetNextMesh();
			this.currentInstructions.SetWithSubset(instructions, startSubmesh, endSubmesh);
			bool flag = SkeletonRendererInstruction.GeometryNotEqual(this.currentInstructions, nextMesh.instructionUsed);
			SubmeshInstruction[] items = this.currentInstructions.submeshInstructions.Items;
			this.meshGenerator.Begin();
			if (this.currentInstructions.hasActiveClipping)
			{
				for (int i = 0; i < this.currentInstructions.submeshInstructions.Count; i++)
				{
					this.meshGenerator.AddSubmesh(items[i], flag);
				}
			}
			else
			{
				this.meshGenerator.BuildMeshWithArrays(this.currentInstructions, flag);
			}
			this.buffers.UpdateSharedMaterials(this.currentInstructions.submeshInstructions);
			Mesh mesh = nextMesh.mesh;
			if (this.meshGenerator.VertexCount <= 0)
			{
				mesh.Clear();
			}
			else
			{
				this.meshGenerator.FillVertexData(mesh);
				if (flag)
				{
					this.meshGenerator.FillTriangles(mesh);
					this.meshRenderer.sharedMaterials = this.buffers.GetUpdatedSharedMaterialsArray();
				}
				else if (this.buffers.MaterialsChangedInLastUpdate())
				{
					this.meshRenderer.sharedMaterials = this.buffers.GetUpdatedSharedMaterialsArray();
				}
				this.meshGenerator.FillLateVertexData(mesh);
			}
			this.meshFilter.sharedMesh = mesh;
			nextMesh.instructionUsed.Set(this.currentInstructions);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000729AC File Offset: 0x00070BAC
		public void SetPropertyBlock(MaterialPropertyBlock block)
		{
			this.LazyIntialize();
			this.meshRenderer.SetPropertyBlock(block);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x000729C0 File Offset: 0x00070BC0
		public static SkeletonPartsRenderer NewPartsRendererGameObject(Transform parent, string name, int sortingOrder = 0)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			gameObject.transform.SetParent(parent, false);
			SkeletonPartsRenderer skeletonPartsRenderer = gameObject.AddComponent<SkeletonPartsRenderer>();
			skeletonPartsRenderer.MeshRenderer.sortingOrder = sortingOrder;
			return skeletonPartsRenderer;
		}

		// Token: 0x04000D4E RID: 3406
		private MeshGenerator meshGenerator;

		// Token: 0x04000D4F RID: 3407
		private MeshRenderer meshRenderer;

		// Token: 0x04000D50 RID: 3408
		private MeshFilter meshFilter;

		// Token: 0x04000D51 RID: 3409
		private MeshRendererBuffers buffers;

		// Token: 0x04000D52 RID: 3410
		private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();
	}
}
