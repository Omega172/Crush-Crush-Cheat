using System;
using UnityEngine;

namespace SadPanda.Platforms
{
	// Token: 0x020000FE RID: 254
	public class Nutaku : MonoBehaviour
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0002DFAC File Offset: 0x0002C1AC
		public static bool Connected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0002DFB0 File Offset: 0x0002C1B0
		public static string GameAuthToken
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
		public static string UserID
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0002DFC0 File Offset: 0x0002C1C0
		public static void ShowSignIn()
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		public static void Submit(string statisticName, int value, bool submitQueue = true)
		{
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0002DFC8 File Offset: 0x0002C1C8
		public static void SubmitQueue()
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0002DFCC File Offset: 0x0002C1CC
		public static void StartPurchase(Store2.BlayfapItem item, int spriteIndex)
		{
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0002DFD0 File Offset: 0x0002C1D0
		public static Nutaku.NutakuUserGrade UserGrade
		{
			get
			{
				return Nutaku.NutakuUserGrade.Player;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0002DFD4 File Offset: 0x0002C1D4
		public static bool Sandbox
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040005A1 RID: 1441
		public Sprite AdsAvailableSprite;

		// Token: 0x040005A2 RID: 1442
		public Sprite AdsUnavailableSprite;

		// Token: 0x020000FF RID: 255
		public enum NutakuUserGrade
		{
			// Token: 0x040005A4 RID: 1444
			Guest = 1,
			// Token: 0x040005A5 RID: 1445
			Player = 0,
			// Token: 0x040005A6 RID: 1446
			Staff = 2
		}
	}
}
