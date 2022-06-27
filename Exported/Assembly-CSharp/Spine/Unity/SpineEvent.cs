using System;

namespace Spine.Unity
{
	// Token: 0x0200021D RID: 541
	public class SpineEvent : SpineAttributeBase
	{
		// Token: 0x0600112D RID: 4397 RVA: 0x00079EAC File Offset: 0x000780AC
		public SpineEvent(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false, bool audioOnly = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
			this.audioOnly = audioOnly;
		}

		// Token: 0x04000E25 RID: 3621
		public bool audioOnly;
	}
}
