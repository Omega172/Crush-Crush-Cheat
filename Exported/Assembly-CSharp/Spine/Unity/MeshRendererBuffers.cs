using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000212 RID: 530
	public class MeshRendererBuffers : IDisposable
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x000793CC File Offset: 0x000775CC
		public void Initialize()
		{
			if (this.doubleBufferedMesh != null)
			{
				this.doubleBufferedMesh.GetNext().Clear();
				this.doubleBufferedMesh.GetNext().Clear();
				this.submeshMaterials.Clear(true);
			}
			else
			{
				this.doubleBufferedMesh = new DoubleBuffered<MeshRendererBuffers.SmartMesh>();
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00079420 File Offset: 0x00077620
		public Material[] GetUpdatedSharedMaterialsArray()
		{
			if (this.submeshMaterials.Count == this.sharedMaterials.Length)
			{
				this.submeshMaterials.CopyTo(this.sharedMaterials);
			}
			else
			{
				this.sharedMaterials = this.submeshMaterials.ToArray();
			}
			return this.sharedMaterials;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00079474 File Offset: 0x00077674
		public bool MaterialsChangedInLastUpdate()
		{
			int count = this.submeshMaterials.Count;
			Material[] array = this.sharedMaterials;
			if (count != array.Length)
			{
				return true;
			}
			Material[] items = this.submeshMaterials.Items;
			for (int i = 0; i < count; i++)
			{
				if (!object.ReferenceEquals(items[i], array[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x000794D0 File Offset: 0x000776D0
		public void UpdateSharedMaterials(ExposedList<SubmeshInstruction> instructions)
		{
			int count = instructions.Count;
			if (count > this.submeshMaterials.Items.Length)
			{
				Array.Resize<Material>(ref this.submeshMaterials.Items, count);
			}
			this.submeshMaterials.Count = count;
			Material[] items = this.submeshMaterials.Items;
			SubmeshInstruction[] items2 = instructions.Items;
			for (int i = 0; i < count; i++)
			{
				items[i] = items2[i].material;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00079548 File Offset: 0x00077748
		public MeshRendererBuffers.SmartMesh GetNextMesh()
		{
			return this.doubleBufferedMesh.GetNext();
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00079558 File Offset: 0x00077758
		public void Clear()
		{
			this.sharedMaterials = new Material[0];
			this.submeshMaterials.Clear(true);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00079574 File Offset: 0x00077774
		public void Dispose()
		{
			if (this.doubleBufferedMesh == null)
			{
				return;
			}
			this.doubleBufferedMesh.GetNext().Dispose();
			this.doubleBufferedMesh.GetNext().Dispose();
			this.doubleBufferedMesh = null;
		}

		// Token: 0x04000E06 RID: 3590
		private DoubleBuffered<MeshRendererBuffers.SmartMesh> doubleBufferedMesh;

		// Token: 0x04000E07 RID: 3591
		internal readonly ExposedList<Material> submeshMaterials = new ExposedList<Material>();

		// Token: 0x04000E08 RID: 3592
		internal Material[] sharedMaterials = new Material[0];

		// Token: 0x02000213 RID: 531
		public class SmartMesh : IDisposable
		{
			// Token: 0x06001115 RID: 4373 RVA: 0x000795D4 File Offset: 0x000777D4
			public void Clear()
			{
				this.mesh.Clear();
				this.instructionUsed.Clear();
			}

			// Token: 0x06001116 RID: 4374 RVA: 0x000795EC File Offset: 0x000777EC
			public void Dispose()
			{
				if (this.mesh != null)
				{
					UnityEngine.Object.Destroy(this.mesh);
				}
				this.mesh = null;
			}

			// Token: 0x04000E09 RID: 3593
			public Mesh mesh = SpineMesh.NewSkeletonMesh();

			// Token: 0x04000E0A RID: 3594
			public SkeletonRendererInstruction instructionUsed = new SkeletonRendererInstruction();
		}
	}
}
