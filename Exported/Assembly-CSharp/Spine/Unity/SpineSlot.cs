using System;

namespace Spine.Unity
{
	// Token: 0x0200021B RID: 539
	public class SpineSlot : SpineAttributeBase
	{
		// Token: 0x0600112B RID: 4395 RVA: 0x00079E54 File Offset: 0x00078054
		public SpineSlot(string startsWith = "", string dataField = "", bool containsBoundingBoxes = false, bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.containsBoundingBoxes = containsBoundingBoxes;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}

		// Token: 0x04000E24 RID: 3620
		public bool containsBoundingBoxes;
	}
}
