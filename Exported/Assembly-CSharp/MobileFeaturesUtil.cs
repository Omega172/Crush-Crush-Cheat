using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class MobileFeaturesUtil
{
	// Token: 0x0600054D RID: 1357 RVA: 0x0002C468 File Offset: 0x0002A668
	public static void StartReview(bool lateReview = false)
	{
		if (global::PlayerPrefs.GetInt("Review", 0) == 1)
		{
			return;
		}
		if (!GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Cassie) || Girl.GetLove(Balance.GirlName.Cassie) < 5)
		{
			return;
		}
		GameState.CurrentState.StartCoroutine(MobileFeaturesUtil.DoReview(lateReview));
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
	private static IEnumerator DoReview(bool lateReview)
	{
		GameObject girls = GameState.GetGirlScreen().gameObject;
		if (lateReview)
		{
			while (girls.activeInHierarchy)
			{
				yield return null;
			}
			while (!girls.activeInHierarchy)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0002C4DC File Offset: 0x0002A6DC
	public static void OpenReviewUrl()
	{
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0002C4E0 File Offset: 0x0002A6E0
	public static void CreateReviewPopup()
	{
		Transform reviewPopup = GameState.CurrentState.transform.Find("Popups/Review Popup");
		reviewPopup.gameObject.SetActive(true);
		MobileFeaturesUtil.SetReviewAsSeen();
		reviewPopup.Find("Dialog/Bottom/No Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			reviewPopup.gameObject.SetActive(false);
		});
		reviewPopup.Find("Dialog/Bottom/Yes Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			MobileFeaturesUtil.OpenReviewUrl();
			reviewPopup.gameObject.SetActive(false);
		});
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002C57C File Offset: 0x0002A77C
	public static void SetReviewAsSeen()
	{
		global::PlayerPrefs.SetInt("Review", 1);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002C594 File Offset: 0x0002A794
	public static bool IsIpad()
	{
		return false;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002C598 File Offset: 0x0002A798
	public static bool IsAspectCloseTo3by4()
	{
		float num = (float)Screen.height / (float)Screen.width;
		return num > 1.2f && num < 1.4f;
	}
}
