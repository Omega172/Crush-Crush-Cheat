using System;

namespace Spine.Unity
{
	// Token: 0x0200021E RID: 542
	public class SpineIkConstraint : SpineAttributeBase
	{
		// Token: 0x0600112E RID: 4398 RVA: 0x00079EDC File Offset: 0x000780DC
		public SpineIkConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}
	}
}
