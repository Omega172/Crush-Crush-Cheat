using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class EventPopupButtonProxy : MonoBehaviour
{
	// Token: 0x060004D0 RID: 1232 RVA: 0x00026854 File Offset: 0x00024A54
	public void OnButtonEvent(ButtonCallbackType callbackType)
	{
		switch (callbackType)
		{
		case ButtonCallbackType.DisableGO:
			base.gameObject.SetActive(false);
			break;
		case ButtonCallbackType.ClickBundles:
			this.ClickBundles();
			break;
		case ButtonCallbackType.ClickDiamonds:
			this.ClickDiamonds();
			break;
		case ButtonCallbackType.OpenLTEPanel:
			this.OpenLTEPanel();
			break;
		case ButtonCallbackType.OpenMobileStore:
			this.OpenMobileStore();
			break;
		case ButtonCallbackType.ClickLTEBundles:
			this.ClickLTEBundles();
			break;
		case ButtonCallbackType.ClickPhoneFlingsBundles:
			this.ClickPhoneBundles();
			break;
		case ButtonCallbackType.ClickValueBundles:
			this.ClickValueBundles();
			break;
		case ButtonCallbackType.ClaimItem:
			this.ClickClaimItem();
			break;
		case ButtonCallbackType.ShowOfferWall:
			this.ShowOfferWall();
			break;
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00026914 File Offset: 0x00024B14
	public void ClickLTEBundles()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickLTEBundles();
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00026928 File Offset: 0x00024B28
	public void ClickPhoneBundles()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickPhoneBundles();
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0002693C File Offset: 0x00024B3C
	public void ClickBundles()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickBundles();
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00026950 File Offset: 0x00024B50
	public void ClickValueBundles()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickValueBundles();
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00026964 File Offset: 0x00024B64
	public void ClickDiamonds()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickDiamonds();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00026978 File Offset: 0x00024B78
	public void OpenLTEPanel()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickLTE();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0002698C File Offset: 0x00024B8C
	public void OpenMobileStore()
	{
		string url = "https://sadpandastudios.com/";
		Application.OpenURL(url);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000269A8 File Offset: 0x00024BA8
	public void ClickClaimItem()
	{
		GameState.CurrentState.AwardAnniversary();
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x000269B4 File Offset: 0x00024BB4
	public void ShowOfferWall()
	{
	}
}
