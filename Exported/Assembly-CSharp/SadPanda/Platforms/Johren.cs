using System;
using UnityEngine;

namespace SadPanda.Platforms
{
	// Token: 0x020000F7 RID: 247
	public class Johren : MonoBehaviour
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0002DD44 File Offset: 0x0002BF44
		public static bool Connected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0002DD48 File Offset: 0x0002BF48
		public static string GameAuthToken
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0002DD50 File Offset: 0x0002BF50
		public static string UserID
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0002DD58 File Offset: 0x0002BF58
		public static void ShowSignIn()
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0002DD5C File Offset: 0x0002BF5C
		public static void Submit(string statisticName, int value, bool submitQueue = true)
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002DD60 File Offset: 0x0002BF60
		public static void SubmitQueue()
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002DD64 File Offset: 0x0002BF64
		public static void StartPurchase(Store2.BlayfapItem item, int spriteIndex)
		{
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0002DD68 File Offset: 0x0002BF68
		public static Johren.JohrenUserGrade UserGrade
		{
			get
			{
				return Johren.JohrenUserGrade.Player;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0002DD6C File Offset: 0x0002BF6C
		public static bool Sandbox
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000584 RID: 1412
		public Sprite AdsAvailableSprite;

		// Token: 0x04000585 RID: 1413
		public Sprite AdsUnavailableSprite;

		// Token: 0x020000F8 RID: 248
		public enum JohrenUserGrade
		{
			// Token: 0x04000587 RID: 1415
			Guest = 1,
			// Token: 0x04000588 RID: 1416
			Player = 0,
			// Token: 0x04000589 RID: 1417
			Staff = 2
		}
	}
}
