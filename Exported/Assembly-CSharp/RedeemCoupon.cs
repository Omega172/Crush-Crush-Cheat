using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000119 RID: 281
public class RedeemCoupon : MonoBehaviour
{
	// Token: 0x060006E3 RID: 1763 RVA: 0x0003BF98 File Offset: 0x0003A198
	private void OnEnable()
	{
		base.transform.Find("Dialog/InputField").GetComponent<InputField>().text = string.Empty;
	}
}
