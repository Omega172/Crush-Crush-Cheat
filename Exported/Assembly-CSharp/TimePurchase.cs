using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011B RID: 283
public class TimePurchase : MonoBehaviour
{
	// Token: 0x060006E9 RID: 1769 RVA: 0x0003C26C File Offset: 0x0003A46C
	public void Init(int requiredTimeBlocks)
	{
		base.gameObject.SetActive(true);
		if (requiredTimeBlocks == 1)
		{
			this.Text.text = Translations.GetTranslation("everything_else_31_1", "Buy an extra Time Block:");
		}
		else
		{
			this.Text.text = string.Format(Translations.GetTranslation("everything_else_110_0", "Buy {0} extra Time Blocks:"), requiredTimeBlocks.ToString());
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0003C2D4 File Offset: 0x0003A4D4
	public void OnClickDiamonds()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickTimeBlocks();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0003C2FC File Offset: 0x0003A4FC
	public void OnClickClose()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x040006F2 RID: 1778
	public Text DiamondText;

	// Token: 0x040006F3 RID: 1779
	public Text Text;

	// Token: 0x040006F4 RID: 1780
	public Button DiamondButton;
}
