using System;
using System.Collections.Generic;

namespace BlayFapShared
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public class ConsumeUserVirtualCurrencyResponse : BlayFapResponse
	{
		// Token: 0x04000215 RID: 533
		public Dictionary<CurrencyType, int> ConsumedCurrency;
	}
}
