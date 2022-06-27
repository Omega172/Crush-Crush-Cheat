using System;

namespace SimpleJSON
{
	// Token: 0x02000087 RID: 135
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class JsonProperty : Attribute
	{
		// Token: 0x0400029A RID: 666
		public string PropertyName;

		// Token: 0x0400029B RID: 667
		public NullValueHandling NullValueHandling;
	}
}
