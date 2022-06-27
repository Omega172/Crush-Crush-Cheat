using System;
using System.Collections.Generic;

namespace UnityEngine.Purchasing
{
	// Token: 0x020000B0 RID: 176
	public class ProductDefinition
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x00020BDC File Offset: 0x0001EDDC
		public ProductDefinition(string id, ProductType type)
		{
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00020BE4 File Offset: 0x0001EDE4
		public ProductDefinition(string id, string storeSpecificId, ProductType type)
		{
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00020BEC File Offset: 0x0001EDEC
		public ProductDefinition(string id, string storeSpecificId, ProductType type, bool enabled)
		{
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00020BF4 File Offset: 0x0001EDF4
		public ProductDefinition(string id, string storeSpecificId, ProductType type, bool enabled, PayoutDefinition payout)
		{
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00020BFC File Offset: 0x0001EDFC
		public ProductDefinition(string id, string storeSpecificId, ProductType type, bool enabled, IEnumerable<PayoutDefinition> payouts)
		{
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00020C04 File Offset: 0x0001EE04
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00020C10 File Offset: 0x0001EE10
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000447 RID: 1095
		public string id;

		// Token: 0x04000448 RID: 1096
		public string storeSpecificId;

		// Token: 0x04000449 RID: 1097
		public ProductType type;

		// Token: 0x0400044A RID: 1098
		public bool enabled;

		// Token: 0x0400044B RID: 1099
		public IEnumerable<PayoutDefinition> payouts;

		// Token: 0x0400044C RID: 1100
		public PayoutDefinition payout;
	}
}
