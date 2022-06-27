using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class CheckBox : MonoBehaviour
{
	// Token: 0x060007FD RID: 2045 RVA: 0x0004AFB4 File Offset: 0x000491B4
	public void ToggleCheck()
	{
		this.toggled = !this.toggled;
		base.transform.Find("Check").gameObject.SetActive(this.toggled);
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060007FE RID: 2046 RVA: 0x0004AFF0 File Offset: 0x000491F0
	// (set) Token: 0x060007FF RID: 2047 RVA: 0x0004AFF8 File Offset: 0x000491F8
	public bool Checked
	{
		get
		{
			return this.toggled;
		}
		set
		{
			this.toggled = value;
			base.transform.Find("Check").gameObject.SetActive(this.toggled);
		}
	}

	// Token: 0x0400087A RID: 2170
	private bool toggled;
}
