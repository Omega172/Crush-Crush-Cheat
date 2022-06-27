using System;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B1 RID: 177
	public class PayoutDefinition
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x00020C18 File Offset: 0x0001EE18
		public PayoutDefinition()
		{
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00020C20 File Offset: 0x0001EE20
		public PayoutDefinition(string subtype, double quantity)
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00020C28 File Offset: 0x0001EE28
		public PayoutDefinition(string typeString, string subtype, double quantity)
		{
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00020C30 File Offset: 0x0001EE30
		public PayoutDefinition(string subtype, double quantity, string data)
		{
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00020C38 File Offset: 0x0001EE38
		public PayoutDefinition(PayoutType type, string subtype, double quantity)
		{
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00020C40 File Offset: 0x0001EE40
		public PayoutDefinition(string typeString, string subtype, double quantity, string data)
		{
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00020C48 File Offset: 0x0001EE48
		public PayoutDefinition(PayoutType type, string subtype, double quantity, string data)
		{
		}

		// Token: 0x0400044D RID: 1101
		public const int MaxSubtypeLength = 64;

		// Token: 0x0400044E RID: 1102
		public const int MaxDataLength = 1024;

		// Token: 0x0400044F RID: 1103
		public PayoutType type;

		// Token: 0x04000450 RID: 1104
		public string typeString;

		// Token: 0x04000451 RID: 1105
		public string subtype;

		// Token: 0x04000452 RID: 1106
		public double quantity;

		// Token: 0x04000453 RID: 1107
		public string data;
	}
}
