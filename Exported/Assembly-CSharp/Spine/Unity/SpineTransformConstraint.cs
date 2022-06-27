using System;

namespace Spine.Unity
{
	// Token: 0x0200021F RID: 543
	public class SpineTransformConstraint : SpineAttributeBase
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x00079F04 File Offset: 0x00078104
		public SpineTransformConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}
	}
}
