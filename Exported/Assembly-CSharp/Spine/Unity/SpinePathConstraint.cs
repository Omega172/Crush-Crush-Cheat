using System;

namespace Spine.Unity
{
	// Token: 0x02000220 RID: 544
	public class SpinePathConstraint : SpineAttributeBase
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x00079F2C File Offset: 0x0007812C
		public SpinePathConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}
	}
}
