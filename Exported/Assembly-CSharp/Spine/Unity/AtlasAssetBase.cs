using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001D2 RID: 466
	public abstract class AtlasAssetBase : ScriptableObject
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000F34 RID: 3892
		public abstract Material PrimaryMaterial { get; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000F35 RID: 3893
		public abstract IEnumerable<Material> Materials { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000F36 RID: 3894
		public abstract int MaterialCount { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000F37 RID: 3895
		public abstract bool IsLoaded { get; }

		// Token: 0x06000F38 RID: 3896
		public abstract void Clear();

		// Token: 0x06000F39 RID: 3897
		public abstract Atlas GetAtlas();
	}
}
