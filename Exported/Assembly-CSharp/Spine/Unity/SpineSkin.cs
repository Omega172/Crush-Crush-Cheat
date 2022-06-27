using System;

namespace Spine.Unity
{
	// Token: 0x02000221 RID: 545
	public class SpineSkin : SpineAttributeBase
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x00079F54 File Offset: 0x00078154
		public SpineSkin(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false, bool defaultAsEmptyString = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
			this.defaultAsEmptyString = defaultAsEmptyString;
		}

		// Token: 0x04000E26 RID: 3622
		public bool defaultAsEmptyString;
	}
}
