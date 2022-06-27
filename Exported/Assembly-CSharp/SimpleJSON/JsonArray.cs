using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJSON
{
	// Token: 0x02000088 RID: 136
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonArray : List<object>
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000DD44 File Offset: 0x0000BF44
		public JsonArray()
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000DD4C File Offset: 0x0000BF4C
		public JsonArray(int capacity) : base(capacity)
		{
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this, null) ?? string.Empty;
		}
	}
}
