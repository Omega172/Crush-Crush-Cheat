using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000215 RID: 533
	public static class SpineMesh
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x00079A20 File Offset: 0x00077C20
		public static Mesh NewSkeletonMesh()
		{
			Mesh mesh = new Mesh();
			mesh.MarkDynamic();
			mesh.name = "Skeleton Mesh";
			mesh.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
			return mesh;
		}

		// Token: 0x04000E10 RID: 3600
		internal const HideFlags MeshHideflags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
	}
}
