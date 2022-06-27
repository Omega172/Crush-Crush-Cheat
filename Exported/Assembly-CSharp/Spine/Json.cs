using System;
using System.IO;
using SharpJson;

namespace Spine
{
	// Token: 0x020001B2 RID: 434
	public static class Json
	{
		// Token: 0x06000D8A RID: 3466 RVA: 0x00060004 File Offset: 0x0005E204
		public static object Deserialize(TextReader text)
		{
			return new JsonDecoder
			{
				parseNumbersAsFloat = true
			}.Decode(text.ReadToEnd());
		}
	}
}
