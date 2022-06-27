using System;

namespace Spine.Unity
{
	// Token: 0x0200021C RID: 540
	public class SpineAnimation : SpineAttributeBase
	{
		// Token: 0x0600112C RID: 4396 RVA: 0x00079E84 File Offset: 0x00078084
		public SpineAnimation(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}
	}
}
