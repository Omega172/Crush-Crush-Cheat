using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000BA RID: 186
	public class IDs : IEnumerable, IEnumerable<KeyValuePair<string, string>>
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00020D80 File Offset: 0x0001EF80
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00020D88 File Offset: 0x0001EF88
		public void Add(string id, params string[] stores)
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00020D8C File Offset: 0x0001EF8C
		public void Add(string id, params object[] stores)
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00020D90 File Offset: 0x0001EF90
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
