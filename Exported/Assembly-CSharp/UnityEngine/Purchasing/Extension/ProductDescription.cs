using System;

namespace UnityEngine.Purchasing.Extension
{
	// Token: 0x020000BD RID: 189
	public class ProductDescription
	{
		// Token: 0x06000430 RID: 1072 RVA: 0x00020DA8 File Offset: 0x0001EFA8
		public ProductDescription(string id, ProductMetadata metadata)
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00020DB0 File Offset: 0x0001EFB0
		public ProductDescription(string id, ProductMetadata metadata, string receipt, string transactionId)
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00020DB8 File Offset: 0x0001EFB8
		public ProductDescription(string id, ProductMetadata metadata, string receipt, string transactionId, ProductType type)
		{
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00020DC0 File Offset: 0x0001EFC0
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00020DC8 File Offset: 0x0001EFC8
		public string transactionId { get; set; }

		// Token: 0x04000461 RID: 1121
		public ProductType type;

		// Token: 0x04000462 RID: 1122
		public string storeSpecificId;

		// Token: 0x04000463 RID: 1123
		public ProductMetadata metadata;

		// Token: 0x04000464 RID: 1124
		public string receipt;
	}
}
