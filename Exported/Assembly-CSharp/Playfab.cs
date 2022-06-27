using System;

// Token: 0x02000102 RID: 258
public static class Playfab
{
	// Token: 0x060005FB RID: 1531 RVA: 0x0002F82C File Offset: 0x0002DA2C
	public static void ExportToSlot(string slot, bool notify)
	{
	}

	// Token: 0x040005B4 RID: 1460
	public const string NoEventLabel = "No Events";

	// Token: 0x040005B5 RID: 1461
	public static Playfab.PlayfabItems AwardedItems = (Playfab.PlayfabItems)0L;

	// Token: 0x040005B6 RID: 1462
	public static Playfab.PhoneFlingPurchases FlingPurchases = (Playfab.PhoneFlingPurchases)0L;

	// Token: 0x040005B7 RID: 1463
	public static Requirement.OutfitType InventoryObjects = Requirement.OutfitType.None;

	// Token: 0x040005B8 RID: 1464
	public static string Promotion = string.Empty;

	// Token: 0x040005B9 RID: 1465
	public static string JsonDataUrl = string.Empty;

	// Token: 0x040005BA RID: 1466
	public static int EventTokenRefund;

	// Token: 0x040005BB RID: 1467
	public static int BlayFapEventTokenRefund;

	// Token: 0x040005BC RID: 1468
	public static bool ResetAwardedItems = false;

	// Token: 0x02000103 RID: 259
	[Flags]
	public enum PhoneFlingPurchases : long
	{
		// Token: 0x040005BE RID: 1470
		Cassie = 1L,
		// Token: 0x040005BF RID: 1471
		Mio = 2L,
		// Token: 0x040005C0 RID: 1472
		Quill = 4L,
		// Token: 0x040005C1 RID: 1473
		Elle = 8L,
		// Token: 0x040005C2 RID: 1474
		Iro = 16L,
		// Token: 0x040005C3 RID: 1475
		Bonnibel = 32L,
		// Token: 0x040005C4 RID: 1476
		Fumi = 64L,
		// Token: 0x040005C5 RID: 1477
		Bearverly = 128L,
		// Token: 0x040005C6 RID: 1478
		Nina = 256L,
		// Token: 0x040005C7 RID: 1479
		Alpha = 512L
	}

	// Token: 0x02000104 RID: 260
	[Flags]
	public enum PlayfabItems : long
	{
		// Token: 0x040005C9 RID: 1481
		NutakuUserOutreach = 1L,
		// Token: 0x040005CA RID: 1482
		Easter2017 = 2L,
		// Token: 0x040005CB RID: 1483
		Summer2017 = 4L,
		// Token: 0x040005CC RID: 1484
		StarterPack = 8L,
		// Token: 0x040005CD RID: 1485
		July2017 = 16L,
		// Token: 0x040005CE RID: 1486
		BackToSchool2017 = 32L,
		// Token: 0x040005CF RID: 1487
		Ayano2017 = 64L,
		// Token: 0x040005D0 RID: 1488
		Darya = 128L,
		// Token: 0x040005D1 RID: 1489
		JelleQuillzone = 256L,
		// Token: 0x040005D2 RID: 1490
		BonchovySpectrum = 512L,
		// Token: 0x040005D3 RID: 1491
		NSFW = 1024L,
		// Token: 0x040005D4 RID: 1492
		Winter2018 = 2048L,
		// Token: 0x040005D5 RID: 1493
		Charlotte = 4096L,
		// Token: 0x040005D6 RID: 1494
		Nutaku2019 = 8192L,
		// Token: 0x040005D7 RID: 1495
		Odango = 16384L,
		// Token: 0x040005D8 RID: 1496
		Shibuki = 32768L,
		// Token: 0x040005D9 RID: 1497
		Sirina = 65536L,
		// Token: 0x040005DA RID: 1498
		Vellatrix = 131072L,
		// Token: 0x040005DB RID: 1499
		Roxxy = 262144L,
		// Token: 0x040005DC RID: 1500
		Tessa = 524288L,
		// Token: 0x040005DD RID: 1501
		Catara = 1048576L,
		// Token: 0x040005DE RID: 1502
		Claudia = 2097152L,
		// Token: 0x040005DF RID: 1503
		Juliet = 4194304L,
		// Token: 0x040005E0 RID: 1504
		Rosa = 8388608L,
		// Token: 0x040005E1 RID: 1505
		Wendy = 16777216L,
		// Token: 0x040005E2 RID: 1506
		Ruri = 33554432L,
		// Token: 0x040005E3 RID: 1507
		Generica = 67108864L,
		// Token: 0x040005E4 RID: 1508
		Suzu = 134217728L,
		// Token: 0x040005E5 RID: 1509
		FullVoices = 268435456L,
		// Token: 0x040005E6 RID: 1510
		Lustat = 536870912L,
		// Token: 0x040005E7 RID: 1511
		Winter2020 = 1073741824L,
		// Token: 0x040005E8 RID: 1512
		Anniversary2021 = 2147483648L,
		// Token: 0x040005E9 RID: 1513
		Sawyer = 4294967296L,
		// Token: 0x040005EA RID: 1514
		Explora = 8589934592L,
		// Token: 0x040005EB RID: 1515
		Esper = 17179869184L,
		// Token: 0x040005EC RID: 1516
		MioPlush = 34359738368L,
		// Token: 0x040005ED RID: 1517
		QuillPlush = 68719476736L,
		// Token: 0x040005EE RID: 1518
		Renee = 137438953472L,
		// Token: 0x040005EF RID: 1519
		Mallory = 274877906944L,
		// Token: 0x040005F0 RID: 1520
		Anniversary2022 = 549755813888L,
		// Token: 0x040005F1 RID: 1521
		Lake = 1099511627776L
	}
}
