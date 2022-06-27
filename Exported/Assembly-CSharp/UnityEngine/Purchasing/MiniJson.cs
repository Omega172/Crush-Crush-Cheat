using System;
using SimpleJSON;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000A9 RID: 169
	public class MiniJson
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x00020BC8 File Offset: 0x0001EDC8
		public static object JsonDecode(string json)
		{
			return SimpleJson.DeserializeObject(json);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00020BD0 File Offset: 0x0001EDD0
		public static string JsonEncode(object json)
		{
			return SimpleJson.SerializeObject(json, null);
		}
	}
}
