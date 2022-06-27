using System;
using BlayFap;
using BlayFapShared;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000012 RID: 18
public class StarterPack : MonoBehaviour
{
	// Token: 0x06000041 RID: 65 RVA: 0x000043A4 File Offset: 0x000025A4
	private void Start()
	{
		BlayFapClient instance = BlayFapClient.Instance;
		instance.OnBlayFapLogin += delegate(LoginResponse result)
		{
			if (result == null)
			{
				return;
			}
			if (result.Error == null && BlayFapClient.BlayFapId != 0UL)
			{
				if (BlayFapIntegration.IsTestDevice)
				{
					StarterPack.EndTime = DateTime.UtcNow + TimeSpan.FromDays(3.0);
				}
				else if (BlayFapClient.CreationDate != default(DateTime))
				{
					StarterPack.EndTime = BlayFapClient.CreationDate + StarterPack.LimitedTime;
				}
			}
		};
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000043E4 File Offset: 0x000025E4
	private void Update()
	{
		if (!GameState.UniverseReady.Value)
		{
			return;
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.StarterPack) != (Playfab.PlayfabItems)0L)
		{
			GameState.CurrentState.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Starter Pack").gameObject.SetActive(false);
			base.gameObject.SetActive(false);
			return;
		}
		bool activeSelf = GameState.GetGirlScreen().gameObject.activeSelf;
		if (activeSelf != this._isActive)
		{
			this._isActive = activeSelf;
			base.transform.Find("Button").gameObject.SetActive(this._isActive);
		}
		if (Utilities.TimeRequested)
		{
			return;
		}
		this.tick += Time.deltaTime;
		if (this.tick < 1f)
		{
			return;
		}
		this.tick -= 1f;
		Utilities.CheckCachedServerTime(delegate
		{
			if (StarterPack.EndTime.Year == 2000)
			{
				return;
			}
			TimeSpan end = StarterPack.EndTime - DateTime.UtcNow + Utilities.TimeOffset;
			this.totalSeconds = Mathf.FloorToInt((float)end.TotalSeconds);
			if (this.totalSeconds < 0)
			{
				GameState.CurrentState.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Starter Pack").gameObject.SetActive(false);
				base.gameObject.SetActive(false);
				return;
			}
			if (this.firstRun)
			{
				GameState.CurrentState.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Starter Pack").gameObject.SetActive(true);
				base.transform.Find("Button").gameObject.SetActive(true);
				this.firstRun = false;
			}
			string text = Utilities.CreateCountdown(end);
			base.transform.Find("Button/Text").GetComponent<Text>().text = text;
			GameState.CurrentState.transform.Find("Popups/Starter Pack/Dialog/Expiration").GetComponent<Text>().text = string.Format("Offer Expires in: {0}", text);
			GameState.CurrentState.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Starter Pack/Expiration").GetComponent<Text>().text = string.Format("Offer Expires in: {0}", text);
		});
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000044D0 File Offset: 0x000026D0
	public void Purchase(bool skipMultiplierCheck = false)
	{
		if (!skipMultiplierCheck && GameState.PurchasedMultiplier >= 8192)
		{
			GameState.CurrentState.transform.Find("Popups/Max Speed Boost").gameObject.SetActive(true);
			return;
		}
		GameState.CurrentState.transform.Find("Popups/Max Speed Boost").gameObject.SetActive(false);
		Store2.BlayfapItem storeItem = null;
		if (Store2.BundleItems == null)
		{
			return;
		}
		foreach (Store2.BlayfapItem blayfapItem in Store2.BundleItems)
		{
			if (blayfapItem.Id.EndsWith(".starterpack"))
			{
				storeItem = blayfapItem;
			}
		}
		GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().PurchaseDiamonds(storeItem);
		GameState.CurrentState.transform.Find("Popups/Starter Pack").gameObject.SetActive(false);
	}

	// Token: 0x0400002E RID: 46
	private int totalSeconds;

	// Token: 0x0400002F RID: 47
	public static DateTime EndTime = new DateTime(2000, 7, 7, 4, 0, 0, DateTimeKind.Utc);

	// Token: 0x04000030 RID: 48
	public static TimeSpan LimitedTime = new TimeSpan(3, 0, 0, 0);

	// Token: 0x04000031 RID: 49
	private DateTime lastCache = DateTime.UtcNow;

	// Token: 0x04000032 RID: 50
	private float tick;

	// Token: 0x04000033 RID: 51
	private bool firstRun = true;

	// Token: 0x04000034 RID: 52
	private bool _isActive = true;
}
