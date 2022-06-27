using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E3 RID: 227
public class TaskCountdown : MonoBehaviour
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x00027010 File Offset: 0x00025210
	public void Init(EventData currentEvent)
	{
		this.countdownText = base.GetComponent<CountdownBehaviour>();
		this.countdownText.EndDate = currentEvent.EndTimeUTC;
		base.gameObject.SetActive(true);
		base.GetComponent<CountdownBehaviour>().OnCompleted = new Action(this.OnCompleted);
		this.eventIcon = base.transform.Find("Icon").GetComponent<Image>();
		this.eventIcon.sprite = currentEvent.Icon;
		if (currentEvent.Icon == null)
		{
			GameState.AssetManager.GetBundle("universe/" + currentEvent.AssetBundleName, false, delegate(AssetBundle bundle)
			{
				if (bundle != null)
				{
					this.eventIcon.sprite = bundle.LoadAsset<Sprite>(currentEvent.IconName);
					GameState.AssetManager.UnloadBundle(bundle);
					this.eventIcon.gameObject.SetActive(true);
				}
			});
		}
		else
		{
			this.eventIcon.sprite = currentEvent.Icon;
		}
		this.eventIcon.gameObject.SetActive(this.eventIcon.sprite != null);
		switch (currentEvent.RewardType[0])
		{
		case TaskManager.EventRewardType.UniqueOutfit:
		case TaskManager.EventRewardType.HolidayOutfit:
		case TaskManager.EventRewardType.SchoolOutfit:
			this.countdownText.Format = "<b>Outfit Event</b>\n{0} left";
			break;
		case TaskManager.EventRewardType.ExclusivePinup:
			this.countdownText.Format = "<b>Pinup Event</b>\n{0} left";
			break;
		case TaskManager.EventRewardType.NewGirl:
		case TaskManager.EventRewardType.AllOutfits:
			if (currentEvent.RewardType.Contains(TaskManager.EventRewardType.NewGirl))
			{
				this.countdownText.Format = "<b>Character Event</b>\n{0} left";
			}
			else if (currentEvent.RewardType.Contains(TaskManager.EventRewardType.AllOutfits))
			{
				string name = Universe.Girls[currentEvent.RewardGirlsID[0]].Name;
				this.countdownText.Format = "<b>" + name + " Outfits</b>\n{0} left";
			}
			break;
		case TaskManager.EventRewardType.MonsterOutfit:
			this.countdownText.Format = "<b>Monster Event</b>\n{0} left";
			break;
		case TaskManager.EventRewardType.BikiniOutfit:
			this.countdownText.Format = "<b>Bikini Event</b>\n{0} left";
			break;
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00027238 File Offset: 0x00025438
	private void OnCompleted()
	{
		GameState.CurrentState.TaskSystem.LoadEventInternal();
	}

	// Token: 0x0400050F RID: 1295
	public Sprite[] Icons;

	// Token: 0x04000510 RID: 1296
	public Sprite[] Girls;

	// Token: 0x04000511 RID: 1297
	private Image eventIcon;

	// Token: 0x04000512 RID: 1298
	private CountdownBehaviour countdownText;
}
