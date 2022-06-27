using System;
using UnityEngine;

namespace SadPanda.Platforms
{
	// Token: 0x020000F9 RID: 249
	public class Kongregate : MonoBehaviour
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x0002DDA8 File Offset: 0x0002BFA8
		private static void Initialize()
		{
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0002DDAC File Offset: 0x0002BFAC
		private static void RegisterCallbacks()
		{
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
		public static bool IsGuest()
		{
			return true;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0002DDB4 File Offset: 0x0002BFB4
		public static string GetUsername()
		{
			return "Guest";
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0002DDBC File Offset: 0x0002BFBC
		public static int GetUserId()
		{
			return 1;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
		public static void SignIn()
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002DDC4 File Offset: 0x0002BFC4
		public static void GetCatalogItems()
		{
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
		public static void GetUserItems()
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0002DDCC File Offset: 0x0002BFCC
		public static void PurchaseItem(string itemId)
		{
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002DDD0 File Offset: 0x0002BFD0
		public static void ConsumeItem(string itemInstanceId)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		public static void ShowAd()
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002DDD8 File Offset: 0x0002BFD8
		public static bool IsKongregate()
		{
			return false;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002DDDC File Offset: 0x0002BFDC
		public static void SubmitStat(string rawStatName, long value)
		{
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0002DDE0 File Offset: 0x0002BFE0
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0002DDE8 File Offset: 0x0002BFE8
		public static Kongregate instance { get; private set; }

		// Token: 0x060005C1 RID: 1473 RVA: 0x0002DDF0 File Offset: 0x0002BFF0
		private void Start()
		{
			if (Kongregate.instance != null)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			Kongregate.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (Kongregate.IsKongregate())
			{
				Kongregate.Initialize();
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0002DE34 File Offset: 0x0002C034
		private void OnKongregateApiInitialized()
		{
			Kongregate.RegisterCallbacks();
			Kongregate.Initialized.Value = true;
			Kongregate.LoggedIn.Value = (Kongregate.IsGuest() ? 1 : 2);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0002DE64 File Offset: 0x0002C064
		private void OnLogin()
		{
			Kongregate.LoggedIn.Value = (Kongregate.IsGuest() ? 1 : 2);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0002DE84 File Offset: 0x0002C084
		private void OnPurchase(object success)
		{
			if (Kongregate.PurchaseCompleted != null)
			{
				Kongregate.PurchaseCompleted(JsonUtility.FromJson<Kongregate.PurchaseResult>(success.ToString()));
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0002DEA8 File Offset: 0x0002C0A8
		private void OnCatalogItems(object result)
		{
			if (Kongregate.CatalogReady != null)
			{
				Kongregate.GetCatalogItemResult getCatalogItemResult = JsonUtility.FromJson<Kongregate.GetCatalogItemResult>(result.ToString());
				Kongregate.CatalogReady(getCatalogItemResult.success, getCatalogItemResult.data);
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0002DEE4 File Offset: 0x0002C0E4
		private void OnUserItems(object result)
		{
			string text = result.ToString().Trim();
			Debug.LogFormat("Got items: [{0}]", new object[]
			{
				text
			});
			if (text.Length > 0)
			{
				if (text.Contains(","))
				{
					foreach (string itemInstanceId in text.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries))
					{
						Kongregate.ConsumeItem(itemInstanceId);
					}
				}
				else
				{
					Kongregate.ConsumeItem(text);
				}
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0002DF6C File Offset: 0x0002C16C
		private void OnAdsAvailable()
		{
			Kongregate.AdsAvailable.Value = true;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002DF7C File Offset: 0x0002C17C
		private void OnAdsUnavailable()
		{
			Kongregate.AdsAvailable.Value = false;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0002DF8C File Offset: 0x0002C18C
		private void OnAdCompleted(string message)
		{
			if (Kongregate.AdCompleted != null)
			{
				Kongregate.AdCompleted();
			}
		}

		// Token: 0x0400058A RID: 1418
		public static string GameAuthToken;

		// Token: 0x0400058B RID: 1419
		public static Action<bool, Kongregate.CatalogItem[]> CatalogReady;

		// Token: 0x0400058C RID: 1420
		public static ReactiveProperty<bool> Initialized = new ReactiveProperty<bool>(false);

		// Token: 0x0400058D RID: 1421
		public static ReactiveProperty<byte> LoggedIn = new ReactiveProperty<byte>(0);

		// Token: 0x0400058E RID: 1422
		public static ReactiveProperty<bool> AdsAvailable = new ReactiveProperty<bool>();

		// Token: 0x0400058F RID: 1423
		public static Action AdCompleted;

		// Token: 0x04000590 RID: 1424
		public static Action<Kongregate.PurchaseResult> PurchaseCompleted;

		// Token: 0x020000FA RID: 250
		public enum KongApiState : byte
		{
			// Token: 0x04000593 RID: 1427
			Unknown,
			// Token: 0x04000594 RID: 1428
			Guest,
			// Token: 0x04000595 RID: 1429
			LoggedIn
		}

		// Token: 0x020000FB RID: 251
		[Serializable]
		public struct CatalogItem
		{
			// Token: 0x04000596 RID: 1430
			public int id;

			// Token: 0x04000597 RID: 1431
			public string identifier;

			// Token: 0x04000598 RID: 1432
			public string name;

			// Token: 0x04000599 RID: 1433
			public string description;

			// Token: 0x0400059A RID: 1434
			public int price;

			// Token: 0x0400059B RID: 1435
			public string[] tags;

			// Token: 0x0400059C RID: 1436
			public string image_url;
		}

		// Token: 0x020000FC RID: 252
		[Serializable]
		public struct GetCatalogItemResult
		{
			// Token: 0x0400059D RID: 1437
			public bool success;

			// Token: 0x0400059E RID: 1438
			public Kongregate.CatalogItem[] data;
		}

		// Token: 0x020000FD RID: 253
		[Serializable]
		public struct PurchaseResult
		{
			// Token: 0x0400059F RID: 1439
			public bool success;

			// Token: 0x040005A0 RID: 1440
			public string item;
		}
	}
}
