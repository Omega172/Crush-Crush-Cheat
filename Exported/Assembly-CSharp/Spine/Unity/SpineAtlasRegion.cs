using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000224 RID: 548
	public class SpineAtlasRegion : PropertyAttribute
	{
		// Token: 0x06001137 RID: 4407 RVA: 0x0007A108 File Offset: 0x00078308
		public SpineAtlasRegion(string atlasAssetField = "")
		{
			this.atlasAssetField = atlasAssetField;
		}

		// Token: 0x04000E2F RID: 3631
		public string atlasAssetField;
	}
}
