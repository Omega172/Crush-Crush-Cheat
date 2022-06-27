using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000116 RID: 278
public class OfflineProgress : MonoBehaviour
{
	// Token: 0x060006DA RID: 1754 RVA: 0x0003BB98 File Offset: 0x00039D98
	private void OnEnable()
	{
		GameState.Voiceover.AddPlaybackBlocker(base.gameObject);
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0003BBAC File Offset: 0x00039DAC
	private void OnDisable()
	{
		GameState.Voiceover.RemovePlaybackBlocker(base.gameObject, true);
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0003BBC0 File Offset: 0x00039DC0
	public void Init(float wallTime, float gameTime, double offlineMoney, long offlineSkills, double offlineHearts, OfflineProgress.ProgressReason reason)
	{
		if (this._moneyText == null)
		{
			this._titleText = base.transform.Find("Dialog/Title").GetComponent<Text>();
			this._moneyText = base.transform.Find("Dialog/Money Text").GetComponent<Text>();
			this._skillsText = base.transform.Find("Dialog/Skills Text").GetComponent<Text>();
			this._heartsText = base.transform.Find("Dialog/Hearts Text").GetComponent<Text>();
		}
		this._moneyText.text = Utilities.ToPrefixedNumber(offlineMoney, false, false);
		this._skillsText.text = Utilities.ToPrefixedNumber((double)offlineSkills, false, false);
		this._heartsText.text = Utilities.ToPrefixedNumber(offlineHearts, false, false);
		switch (reason)
		{
		case OfflineProgress.ProgressReason.None:
		case OfflineProgress.ProgressReason.Offline:
			this._titleText.text = Translations.GetTranslation("everything_else_56_0", "Offline Earnings");
			break;
		case OfflineProgress.ProgressReason.KongregateAd:
			this._titleText.text = "Thanks For Watching That Ad!\nBonus 30 Minutes";
			break;
		case OfflineProgress.ProgressReason.NutakuRegistration:
			this._titleText.text = "Thanks For Registering!\nBonus 30 Minutes";
			break;
		case OfflineProgress.ProgressReason.Timelord:
			this._titleText.text = "Time Lord!\nSkipped 1 Week";
			break;
		case OfflineProgress.ProgressReason.Timeskip:
			this._titleText.text = "Time Skip Earnings";
			break;
		case OfflineProgress.ProgressReason.KongregateAdIncreaseMoney:
			this._titleText.text = "Thanks For Watching That Ad!\nYou Doubled Your Money:";
			break;
		case OfflineProgress.ProgressReason.KongregateAdPhoneFlingSkip:
			this._titleText.text = "Thanks For Watching That Ad!";
			break;
		}
		base.transform.Find("Dialog/Earnings Report").GetComponent<Text>().text = Translations.GetTranslation("everything_else_57_0", "Heads up! You can bank up to 7 Days worth of offline progress! Don't leave the ladies hanging!");
		base.transform.Find("Dialog/Close Button/Button Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_58_0", "Thanks!");
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0003BDB8 File Offset: 0x00039FB8
	public void OnClose()
	{
		base.gameObject.SetActive(false);
		if (!string.IsNullOrEmpty(Playfab.Promotion))
		{
			GameState.CurrentState.LaunchPromotion();
		}
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0003BDE0 File Offset: 0x00039FE0
	private void OnAdAvailable(bool available)
	{
		if (base.gameObject == null || base.transform == null)
		{
			return;
		}
		if (base.transform.Find("Dialog/Buttons/Watch Ad") != null)
		{
			base.transform.Find("Dialog/Buttons/Watch Ad").GetComponent<Button>().interactable = available;
		}
	}

	// Token: 0x040006C4 RID: 1732
	private Text _moneyText;

	// Token: 0x040006C5 RID: 1733
	private Text _skillsText;

	// Token: 0x040006C6 RID: 1734
	private Text _heartsText;

	// Token: 0x040006C7 RID: 1735
	private Text _titleText;

	// Token: 0x040006C8 RID: 1736
	private bool _adSubscribed;

	// Token: 0x02000117 RID: 279
	public enum ProgressReason
	{
		// Token: 0x040006CA RID: 1738
		None,
		// Token: 0x040006CB RID: 1739
		KongregateAd,
		// Token: 0x040006CC RID: 1740
		NutakuRegistration,
		// Token: 0x040006CD RID: 1741
		Timelord,
		// Token: 0x040006CE RID: 1742
		Offline,
		// Token: 0x040006CF RID: 1743
		Purchased,
		// Token: 0x040006D0 RID: 1744
		DoubledProgress,
		// Token: 0x040006D1 RID: 1745
		Timeskip,
		// Token: 0x040006D2 RID: 1746
		KongregateAdIncreaseMoney,
		// Token: 0x040006D3 RID: 1747
		KongregateAdPhoneFlingSkip
	}
}
