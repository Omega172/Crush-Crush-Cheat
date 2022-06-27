using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000111 RID: 273
public class HobbyGild : MonoBehaviour
{
	// Token: 0x0600069B RID: 1691 RVA: 0x00038594 File Offset: 0x00036794
	public void Init(Hobby2 hobby)
	{
		if (GameState.Diamonds.Value < 10)
		{
			Utilities.PurchaseDiamonds(10);
			return;
		}
		if (hobby.Gilded)
		{
			return;
		}
		base.transform.Find("Dialog/Text 1").GetComponent<Text>().text = Translations.GetTranslation("everything_else_22_0", "Spend diamonds to apply a permanent x16 speed boost to this hobby?");
		base.transform.Find("Dialog/Text 2").GetComponent<Text>().text = Translations.GetTranslation("everything_else_23_0", "x16 Speed Boost");
		base.gameObject.SetActive(true);
		this.selectedJob = null;
		this.selectedHobby = hobby;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00038634 File Offset: 0x00036834
	public void Init(Job2 job)
	{
		if (GameState.Diamonds.Value < 10)
		{
			Utilities.PurchaseDiamonds(10);
			return;
		}
		if (job.Gilded)
		{
			return;
		}
		base.transform.Find("Dialog/Text 1").GetComponent<Text>().text = Translations.GetTranslation("everything_else_21_0", "Spend diamonds to apply a permanent x5 output to this job?");
		base.transform.Find("Dialog/Text 2").GetComponent<Text>().text = Translations.GetTranslation("everything_else_98_0", "x5 Money Output");
		base.gameObject.SetActive(true);
		this.selectedJob = job;
		this.selectedHobby = null;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000386D4 File Offset: 0x000368D4
	public void AcceptPurchase()
	{
		Utilities.ConfirmPurchase(10, delegate
		{
			if (this.selectedHobby != null)
			{
				this.selectedHobby.AddMultiplier();
				Utilities.SendAnalytic(Utilities.AnalyticType.Gild, this.selectedHobby.Data.Resource.Singular.English);
			}
			else
			{
				this.selectedJob.AddMultiplier();
				Utilities.SendAnalytic(Utilities.AnalyticType.Gild, this.selectedJob.JobType.ToFriendlyString());
			}
			base.gameObject.SetActive(false);
		});
	}

	// Token: 0x040006A0 RID: 1696
	private Hobby2 selectedHobby;

	// Token: 0x040006A1 RID: 1697
	private Job2 selectedJob;
}
