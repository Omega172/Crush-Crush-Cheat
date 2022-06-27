using System;

namespace BlayFapShared
{
	// Token: 0x0200005D RID: 93
	public class CatalogItem
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		public bool ValidTier(float scaling = 0.01f)
		{
			try
			{
				string[] array = this.ItemID.Split(new char[]
				{
					'.'
				});
				string text = array[array.Length - 1];
				if (text[0] != 't' || !char.IsDigit(text[1]) || !char.IsDigit(text[2]))
				{
					return true;
				}
				if (text.Substring(1, 2) == this.GetTierName(scaling))
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		public string GetTierName(float scaling = 0.01f)
		{
			int num = (int)Math.Round((double)(scaling * this.Price));
			if (num <= 50)
			{
				return num.ToString("D2");
			}
			int num2 = (int)Math.Round((double)((float)(num - 50) / 5f));
			return (50 + num2).ToString("D2");
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000DB54 File Offset: 0x0000BD54
		public uint PriceFromTierName()
		{
			string text = this.ItemID.Substring(this.ItemID.LastIndexOf('.') + 1);
			if (!text.StartsWith("t"))
			{
				return 0U;
			}
			string s = text.Substring(1, text.IndexOf('_') - 1);
			uint num = 0U;
			if (!uint.TryParse(s, out num))
			{
				return 0U;
			}
			if (num <= 50U)
			{
				return num * 100U;
			}
			return (50U + (num - 50U) * 5U) * 100U;
		}

		// Token: 0x0400022B RID: 555
		public string ItemID;

		// Token: 0x0400022C RID: 556
		public uint Price;

		// Token: 0x0400022D RID: 557
		public bool Consumable;

		// Token: 0x0400022E RID: 558
		public CatalogMetadata Metadata;
	}
}
