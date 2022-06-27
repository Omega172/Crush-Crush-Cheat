using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000136 RID: 310
public class LanguageSelector : MonoBehaviour
{
	// Token: 0x0600080B RID: 2059 RVA: 0x0004B2A4 File Offset: 0x000494A4
	private void Start()
	{
		Button component = base.GetComponent<Button>();
		component.onClick.AddListener(new UnityAction(this.Check));
		base.transform.Find("Check").gameObject.SetActive(Translations.PreferredLanguage == this.Language);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0004B2F8 File Offset: 0x000494F8
	private void Update()
	{
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0004B2FC File Offset: 0x000494FC
	public void Check()
	{
		for (int i = 0; i < base.transform.parent.childCount; i++)
		{
			Transform child = base.transform.parent.GetChild(i);
			if (child.Find("Check") != null)
			{
				if (child == base.transform)
				{
					child.Find("Check").gameObject.SetActive(true);
				}
				else
				{
					child.Find("Check").gameObject.SetActive(false);
				}
			}
		}
		Settings.NextLanguage = this.Language;
		base.transform.parent.parent.Find("Disclaimer").GetComponent<Text>().text = Translations.GetDisclaimer(this.Language);
	}

	// Token: 0x04000882 RID: 2178
	public Translations.Language Language;
}
