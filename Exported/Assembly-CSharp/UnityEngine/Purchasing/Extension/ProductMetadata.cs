using System;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000BC RID: 188
	public class ProductMetadata
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x00020D98 File Offset: 0x0001EF98
		public ProductMetadata()
		{
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00020DA0 File Offset: 0x0001EFA0
		public ProductMetadata(string priceString, string title, string description, string currencyCode, decimal localizedPrice)
		{
		}

		// Token: 0x0400045C RID: 1116
		public string localizedPriceString;

		// Token: 0x0400045D RID: 1117
		public string localizedTitle;

		// Token: 0x0400045E RID: 1118
		public string localizedDescription;

		// Token: 0x0400045F RID: 1119
		public string isoCurrencyCode;

		// Token: 0x04000460 RID: 1120
		public decimal localizedPrice;
	}
}
