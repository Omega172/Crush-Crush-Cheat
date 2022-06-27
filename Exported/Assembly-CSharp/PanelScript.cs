using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000F5 RID: 245
public class PanelScript : MonoBehaviour
{
	// Token: 0x0600058B RID: 1419 RVA: 0x0002D404 File Offset: 0x0002B604
	public void LoadStartupScreen()
	{
		this.JobPanel.SetActive(false);
		if (global::PlayerPrefs.GetInt("Tutorial", 0) == 0)
		{
			this.GirlPanel.SetActive(false);
		}
		this.HobbyPanel.SetActive(false);
		this.SkillPanel.SetActive(false);
		this.AchievementPanel.SetActive(false);
		this.MorePanel.SetActive(false);
		this.StorePanel.SetActive(false);
		this.EventSystem.SetSelectedGameObject(null);
		if (global::PlayerPrefs.GetInt("Tutorial", 0) != 0)
		{
			this.GirlPanel.SetActive(true);
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0002D4A0 File Offset: 0x0002B6A0
	private void HideAll(bool hideTopBar = false)
	{
		if (this.JobPanel.gameObject.activeInHierarchy)
		{
			GameState.HideJobsNotification();
		}
		this.JobPanel.SetActive(false);
		this.GirlPanel.SetActive(false);
		this.HobbyPanel.SetActive(false);
		this.SkillPanel.SetActive(false);
		this.AchievementPanel.SetActive(false);
		this.MorePanel.SetActive(false);
		this.StorePanel.SetActive(false);
		this.EventSystem.SetSelectedGameObject(null);
		GameState.CurrentState.LaunchPromotion();
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0002D534 File Offset: 0x0002B734
	private void SetBackground(int index)
	{
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002D538 File Offset: 0x0002B738
	public void ClickJob()
	{
		if (IntroScreen.TutorialState == IntroScreen.State.IntroduceJobs)
		{
			GameState.GetIntroScreen().JobIntro();
		}
		if (this.JobPanel.gameObject.activeInHierarchy)
		{
			return;
		}
		GameState.HideJobsNotification();
		this.HideAll(false);
		this.JobPanel.SetActive(true);
		this.SetBackground(1);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0002D590 File Offset: 0x0002B790
	public void ClickStore()
	{
		GameState.HideStoreNotification();
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.SetBackground(5);
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0002D5B4 File Offset: 0x0002B7B4
	public void ClickDiamonds()
	{
		if (IntroScreen.TutorialState < IntroScreen.State.AllActive)
		{
			return;
		}
		GameState.HideStoreNotification();
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(0);
		if (GameState.CurrentState.transform.Find("Popups/Summer Status") != null)
		{
			GameState.CurrentState.transform.Find("Popups/Summer Status").gameObject.SetActive(false);
		}
		GameState.GetCellphone().gameObject.SetActive(false);
		this.SetBackground(5);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0002D64C File Offset: 0x0002B84C
	public void ClickTimeSkips()
	{
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(5);
		if (GameState.CurrentState.transform.Find("Popups/Summer Status") != null)
		{
			GameState.CurrentState.transform.Find("Popups/Summer Status").gameObject.SetActive(false);
		}
		GameState.GetCellphone().gameObject.SetActive(false);
		this.SetBackground(5);
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002D6D4 File Offset: 0x0002B8D4
	public void ClickTimeBlocks()
	{
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(4);
		if (GameState.CurrentState.transform.Find("Popups/Summer Status") != null)
		{
			GameState.CurrentState.transform.Find("Popups/Summer Status").gameObject.SetActive(false);
		}
		GameState.GetTaskSystem().gameObject.SetActive(false);
		GameState.GetCellphone().gameObject.SetActive(false);
		this.SetBackground(5);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0002D76C File Offset: 0x0002B96C
	public void ClickSpeedBoosts()
	{
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(4);
		if (GameState.CurrentState.transform.Find("Popups/Summer Status") != null)
		{
			GameState.CurrentState.transform.Find("Popups/Summer Status").gameObject.SetActive(false);
		}
		GameState.GetTaskSystem().gameObject.SetActive(false);
		GameState.GetCellphone().gameObject.SetActive(false);
		this.SetBackground(5);
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0002D804 File Offset: 0x0002BA04
	public void ClickBundles()
	{
		GameState.HideStoreNotification();
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(1);
		this.StorePanel.GetComponent<Store2>().SelectTabOnLoad = 1;
		this.SetBackground(5);
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0002D854 File Offset: 0x0002BA54
	public void ClickOutfits()
	{
		GameState.HideStoreNotification();
		this.HideAll(false);
		this.StorePanel.SetActive(true);
		this.StorePanel.GetComponent<Store2>().SelectTab(2);
		this.StorePanel.GetComponent<Store2>().SelectTabOnLoad = 2;
		this.SetBackground(5);
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0002D8A4 File Offset: 0x0002BAA4
	public void ClickLTEBundles()
	{
		GameState.HideStoreNotification();
		this.ClickBundles();
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0002D8B4 File Offset: 0x0002BAB4
	public void ClickPhoneBundles()
	{
		GameState.HideStoreNotification();
		this.ClickBundles();
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0002D8C4 File Offset: 0x0002BAC4
	public void ClickValueBundles()
	{
		GameState.HideStoreNotification();
		this.ClickBundles();
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0002D8D4 File Offset: 0x0002BAD4
	public void ClickGirl()
	{
		if (this.GirlPanel.activeInHierarchy)
		{
			if (this.MorePanel.activeInHierarchy)
			{
				this.MorePanel.SetActive(false);
			}
			this.GirlPanel.GetComponent<Girls>().HideInteractions();
			this.SetBackground(0);
			return;
		}
		if (IntroScreen.TutorialState == IntroScreen.State.JobsActive)
		{
			return;
		}
		if (IntroScreen.TutorialState == IntroScreen.State.WaitFor250)
		{
			GameState.GetIntroScreen().CassieFinished();
		}
		GameState.HideGirlNotification();
		this.HideAll(false);
		this.GirlPanel.SetActive(true);
		this.GirlPanel.GetComponent<Girls>().SetGirl();
		this.GirlPanel.GetComponent<Girls>().HideInteractions();
		if (IntroScreen.TutorialState == IntroScreen.State.GirlsActive)
		{
			Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("achievements_406_0", "You met your first girl!"));
			Girls.UnlockedGirlCount = 1;
			Girl girl = Girl.FindGirl(Balance.GirlName.Cassie);
			if (girl != null)
			{
				girl.transform.Find("Unlock System").GetComponent<ParticleSystem>().Play();
			}
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
			if (girl != null && girl.transform.Find("Notification") != null)
			{
				girl.transform.Find("Notification").gameObject.SetActive(true);
			}
		}
		this.SetBackground(0);
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002DA2C File Offset: 0x0002BC2C
	public void ClickHobby()
	{
		GameState.HideHobbyNotification();
		this.HideAll(false);
		this.HobbyPanel.SetActive(true);
		this.SetBackground(2);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0002DA50 File Offset: 0x0002BC50
	public void ClickSkill()
	{
		this.HideAll(false);
		this.SkillPanel.SetActive(true);
		this.SkillPanel.GetComponent<Skills>().UpdateStats();
		this.SetBackground(3);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0002DA88 File Offset: 0x0002BC88
	public void ClickAchievement()
	{
		this.HideAll(false);
		this.AchievementPanel.SetActive(true);
		this.SetBackground(3);
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0002DAA4 File Offset: 0x0002BCA4
	public void ClickMore()
	{
		this.HideAll(false);
		this.MorePanel.SetActive(true);
		this.MorePanel.transform.Find("Buttons/Memory Album Button").gameObject.SetActive(true);
		this.MorePanel.transform.Find("Select Save Slot").gameObject.SetActive(false);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0002DB04 File Offset: 0x0002BD04
	public void ClickReset()
	{
		this.HideAll(false);
		this.ResetPanel.SetActive(true);
		this.SetBackground(4);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0002DB20 File Offset: 0x0002BD20
	public void ClickLTE()
	{
		GameState.GetTaskSystem().OpenLTEPanel();
		this.SetBackground(7);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002DB34 File Offset: 0x0002BD34
	public void ClickTexts()
	{
		if (this.CellphonePanel.activeSelf)
		{
			return;
		}
		this.HideAll(true);
		this.CellphonePanel.SetActive(true);
		this.SetBackground(6);
	}

	// Token: 0x04000572 RID: 1394
	public GameObject JobPanel;

	// Token: 0x04000573 RID: 1395
	public GameObject GirlPanel;

	// Token: 0x04000574 RID: 1396
	public GameObject HobbyPanel;

	// Token: 0x04000575 RID: 1397
	public GameObject SkillPanel;

	// Token: 0x04000576 RID: 1398
	public GameObject AchievementPanel;

	// Token: 0x04000577 RID: 1399
	public GameObject MorePanel;

	// Token: 0x04000578 RID: 1400
	public GameObject StorePanel;

	// Token: 0x04000579 RID: 1401
	public GameObject ResetPanel;

	// Token: 0x0400057A RID: 1402
	public GameObject CellphonePanel;

	// Token: 0x0400057B RID: 1403
	public Color BottomColor;

	// Token: 0x0400057C RID: 1404
	public Color BottomSelectedColor;

	// Token: 0x0400057D RID: 1405
	public EventSystem EventSystem;
}
