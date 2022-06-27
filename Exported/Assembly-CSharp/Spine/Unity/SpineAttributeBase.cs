using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000219 RID: 537
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public abstract class SpineAttributeBase : PropertyAttribute
	{
		// Token: 0x04000E20 RID: 3616
		public string dataField = string.Empty;

		// Token: 0x04000E21 RID: 3617
		public string startsWith = string.Empty;

		// Token: 0x04000E22 RID: 3618
		public bool includeNone = true;

		// Token: 0x04000E23 RID: 3619
		public bool fallbackToTextField;
	}
}
