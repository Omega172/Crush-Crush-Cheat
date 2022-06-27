using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AssetBundles;
using BlayFap;
using KittyCrush;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000107 RID: 263
public class Cellphone : MonoBehaviour
{
	// Token: 0x0600061F RID: 1567 RVA: 0x00030FD4 File Offset: 0x0002F1D4
	private void Start()
	{
		this.contentPanel = base.transform.Find("Phone UI/Messenger/Messages/Content");
		this.contactsPanel = base.transform.Find("Phone UI/Contacts/List/Content");
		for (int i = this.contentPanel.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.contentPanel.GetChild(i).gameObject);
		}
		this.replyTime = base.transform.Find("Phone UI/Messenger/Status Bar/Reply Timer/Text").GetComponent<Text>();
		this.replyProgress = base.transform.Find("Phone UI/Messenger/Status Bar/Reply Timer/Fill").GetComponent<Image>();
		this.replyPinupIcon = base.transform.Find("Phone UI/Messenger/Status Bar/Image").GetComponent<Image>();
		this.pinupPosition = base.transform.Find("Phone UI/Messenger/Status Bar/Progress/Text").GetComponent<Text>();
		this.pinupProgress = base.transform.Find("Phone UI/Messenger/Status Bar/Progress/Fill").GetComponent<Image>();
		this.pinupProgressContainer = base.transform.Find("Phone UI/Messenger/Status Bar/Progress").gameObject;
		this.purchaseButton = base.transform.Find("Phone UI/Messenger/Status Bar/Buy").GetComponent<Button>();
		this.replyProgressContainer = base.transform.Find("Phone UI/Messenger/Status Bar/Reply Timer").gameObject;
		this.DisableReplies();
		this.messenger = base.transform.Find("Phone UI/Messenger").gameObject;
		this.contacts = base.transform.Find("Phone UI/Contacts").gameObject;
		this.fullImage = base.transform.Find("Phone UI/Full Image").gameObject;
		this.purchasePopup = base.transform.Find("Phone UI/Messenger/Purchase").gameObject;
		this.gallery = base.transform.Find("Phone UI/Messenger/Gallery");
		this.galleryIcon = base.transform.Find("Phone UI/Messenger/Top/Gallery Icon").gameObject;
		this.messenger.SetActive(false);
		this.contacts.SetActive(true);
		this.fullImage.SetActive(false);
		this.purchasePopup.SetActive(false);
		this.gallery.gameObject.SetActive(false);
		this.galleryIcon.SetActive(true);
		GameState.UniverseReady += new ReactiveProperty<bool>.Changed(this.OnReady);
		this.fullscreenImage = base.transform.Find("Phone UI/Full Image").GetComponent<Image>();
		base.transform.Find("Phone UI/Messenger/Top/Arrow").GetComponent<Button>().onClick.AddListener(new UnityAction(this.BackArrow));
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0003126C File Offset: 0x0002F46C
	private void OnReady(bool ready)
	{
		if (ready)
		{
			this.Init();
			this.OnDestroy();
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00031280 File Offset: 0x0002F480
	private void OnDestroy()
	{
		if (!this.disposed)
		{
			GameState.UniverseReady -= new ReactiveProperty<bool>.Changed(this.OnReady);
			this.disposed = true;
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x000312B0 File Offset: 0x0002F4B0
	private void OnEnable()
	{
		GameState.Voiceover.SetCellPhoneActive(true);
		this.UpdateIcons();
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x000312C4 File Offset: 0x0002F4C4
	private void OnDisable()
	{
		if (this.messenger == null)
		{
			return;
		}
		this.DisposeGirl();
		this.messenger.SetActive(false);
		this.contacts.SetActive(true);
		this.fullImage.SetActive(false);
		this.purchasePopup.SetActive(false);
		base.transform.Find("Phone UI/Name").gameObject.SetActive(false);
		this.gallery.gameObject.SetActive(false);
		this.galleryIcon.SetActive(true);
		base.transform.Find("Phone UI/Messenger/Vote").gameObject.SetActive(false);
		base.transform.Find("Phone UI/Messenger/Vote Results").gameObject.SetActive(false);
		base.transform.Find("Phone UI/Messenger/Reset Fling").gameObject.SetActive(false);
		GameState.Voiceover.SetCellPhoneActive(false);
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000313B0 File Offset: 0x0002F5B0
	private static Cellphone.NotificationType HasNotification(short id, out int seconds)
	{
		seconds = -4;
		PhoneModel phoneModel = Universe.CellphoneGirls[id];
		string @string = phoneModel.PathPref.GetString(string.Empty);
		int num = 1;
		if (!string.IsNullOrEmpty(@string) && @string.Contains("`"))
		{
			if (@string.Contains("`"))
			{
				for (int i = 0; i < @string.Length; i++)
				{
					if (@string[i] == '`')
					{
						num++;
					}
				}
				if (num > phoneModel.MessageCount)
				{
					return Cellphone.NotificationType.None;
				}
				seconds = int.Parse(@string.Substring(@string.LastIndexOf('`') + 1));
			}
			else
			{
				seconds = int.Parse(@string);
			}
		}
		long @long = phoneModel.DatePref.GetLong(0L);
		if (@long == 0L || @long == 9223372036854775807L || seconds == -4)
		{
			if (string.IsNullOrEmpty(phoneModel.PathPref.GetString(string.Empty)))
			{
				return (!Cellphone.IsUnlocked(id)) ? Cellphone.NotificationType.None : Cellphone.NotificationType.NewMessage;
			}
			return (num - 1 >= phoneModel.Messages.Count || !Cellphone.HasConversation(id, phoneModel.Messages[num - 1].ConversationId)) ? Cellphone.NotificationType.None : Cellphone.NotificationType.NewMessage;
		}
		else
		{
			double totalSeconds = (DateTime.UtcNow - DateTime.FromBinary(@long)).TotalSeconds;
			if (seconds == 0)
			{
				return Cellphone.NotificationType.PendingReply;
			}
			if (totalSeconds >= (double)seconds)
			{
				return Cellphone.NotificationType.NewMessage;
			}
			return Cellphone.NotificationType.None;
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00031530 File Offset: 0x0002F730
	private static bool HasPinup(short id)
	{
		PhoneModel phoneModel = Universe.CellphoneGirls[id];
		int num = 1;
		PhoneModel.PhoneMessage phoneMessage = phoneModel.FirstMessage;
		while (phoneMessage != null)
		{
			if (phoneMessage is PhoneModel.PhoneImage)
			{
				break;
			}
			phoneMessage = phoneMessage.NextMessage;
			num++;
		}
		string @string = phoneModel.PathPref.GetString(string.Empty);
		int num2 = 1;
		for (int i = 0; i < @string.Length; i++)
		{
			if (@string[i] == '`')
			{
				num2++;
			}
		}
		return num2 > num;
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x000315C4 File Offset: 0x0002F7C4
	public static bool IsVisible(short id)
	{
		PhoneModel phoneModel = Universe.CellphoneGirls[id];
		if (phoneModel.DatePref.GetLong(0L) != 0L)
		{
			return true;
		}
		switch (id)
		{
		case 23:
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.Cassie);
			return girl != null && girl.Love >= 9;
		}
		case 24:
		{
			Girl girl2 = Girl.FindGirl(Balance.GirlName.Mio);
			return girl2 != null && girl2.Love >= 9;
		}
		case 25:
		{
			Girl girl3 = Girl.FindGirl(Balance.GirlName.Quill);
			return girl3 != null && girl3.Love >= 9;
		}
		case 26:
		{
			Girl girl4 = Girl.FindGirl(Balance.GirlName.Elle);
			return girl4 != null && girl4.Love >= 9;
		}
		case 27:
		{
			Girl girl5 = Girl.FindGirl(Balance.GirlName.Iro);
			return girl5 != null && girl5.Love >= 9;
		}
		case 28:
		{
			Girl girl6 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			return girl6 != null && girl6.Love >= 9;
		}
		case 29:
		{
			Girl girl7 = Girl.FindGirl(Balance.GirlName.Fumi);
			return girl7 != null && girl7.Love >= 9;
		}
		case 30:
		{
			Girl girl8 = Girl.FindGirl(Balance.GirlName.Bearverly);
			return girl8 != null && girl8.Love >= 9;
		}
		case 31:
		{
			Girl girl9 = Girl.FindGirl(Balance.GirlName.Nina);
			return girl9 != null && girl9.Love >= 9;
		}
		case 32:
		{
			Girl girl10 = Girl.FindGirl(Balance.GirlName.Alpha);
			return girl10 != null && girl10.Love >= 9;
		}
		default:
			return false;
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x000317AC File Offset: 0x0002F9AC
	public static bool IsUnlocked(short id)
	{
		PhoneModel phoneModel = Universe.CellphoneGirls[id];
		if (phoneModel.DatePref.GetLong(0L) != 0L)
		{
			return true;
		}
		switch (id)
		{
		case 1:
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.Elle);
			return !(girl == null) && girl.Love >= 2;
		}
		case 2:
		{
			Girl girl2 = Girl.FindGirl(Balance.GirlName.Iro);
			return !(girl2 == null) && girl2.Love >= 2;
		}
		case 3:
		{
			Girl girl3 = Girl.FindGirl(Balance.GirlName.Ayano);
			return !(girl3 == null) && girl3.Love >= 2;
		}
		case 4:
		{
			Girl girl4 = Girl.FindGirl(Balance.GirlName.Mio);
			return !(girl4 == null) && girl4.Love >= 9;
		}
		case 5:
		{
			Girl girl5 = Girl.FindGirl(Balance.GirlName.Iro);
			return !(girl5 == null) && girl5.Love >= 6;
		}
		case 6:
		{
			int num = Skills.SkillLevel[2];
			return num >= 25;
		}
		case 7:
		{
			Job2 job = null;
			foreach (Job2 job2 in Job2.ActiveJobs)
			{
				if (job2.JobType == Requirement.JobType.Computers)
				{
					job = job2;
				}
			}
			return !(job == null) && job.Level >= 2;
		}
		case 8:
		{
			string @string = Universe.CellphoneGirls[1].PathPref.GetString(string.Empty);
			string[] array = @string.Split(new char[]
			{
				'`'
			});
			return array.Length >= 42;
		}
		case 9:
			return GameState.CurrentState.TimeMultiplier.Value >= 25f;
		case 10:
		{
			Girl girl6 = Girl.FindGirl(Balance.GirlName.Pamu);
			return girl6 != null && (girl6.Love > 0 || girl6.Hearts > 0L);
		}
		case 11:
		{
			string string2 = Universe.CellphoneGirls[10].PathPref.GetString(string.Empty);
			string[] array2 = string2.Split(new char[]
			{
				'`'
			});
			return array2.Length >= 179;
		}
		case 12:
		{
			Girl girl7 = Girl.FindGirl(Balance.GirlName.Nina);
			return girl7 != null && girl7.Love >= 2;
		}
		case 13:
		{
			int num2 = Skills.SkillLevel[9];
			return num2 >= 25;
		}
		case 14:
		{
			int num3 = Skills.SkillLevel[4];
			return num3 >= 25;
		}
		case 15:
		{
			Girl girl8 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			return girl8 != null && girl8.Love >= 9;
		}
		case 16:
		{
			Girl girl9 = Girl.FindGirl(Balance.GirlName.Bearverly);
			return girl9 != null && girl9.Love >= 6;
		}
		case 17:
		{
			int num4 = Skills.SkillLevel[0];
			return num4 >= 25;
		}
		case 18:
		{
			string string3 = Universe.CellphoneGirls[11].PathPref.GetString(string.Empty);
			string[] array3 = string3.Split(new char[]
			{
				'`'
			});
			return array3.Length >= 145;
		}
		case 19:
		{
			Girl girl10 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			return girl10 != null && girl10.Love >= 5;
		}
		case 20:
		{
			Girl girl11 = Girl.FindGirl(Balance.GirlName.Fumi);
			return girl11 != null && girl11.Love >= 5;
		}
		case 21:
		{
			Girl girl12 = Girl.FindGirl(Balance.GirlName.QPiddy);
			return girl12 != null && girl12.Love >= 2;
		}
		case 22:
			return !GameState.GetAchievements().ActiveAchievements.Contains(GameState.GetAchievements().GetAchievementFromIndex(473));
		case 23:
		{
			Girl girl13 = Girl.FindGirl(Balance.GirlName.Cassie);
			return girl13 != null && girl13.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Cassie) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 24:
		{
			Girl girl14 = Girl.FindGirl(Balance.GirlName.Mio);
			return girl14 != null && girl14.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Mio) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 25:
		{
			Girl girl15 = Girl.FindGirl(Balance.GirlName.Quill);
			return girl15 != null && girl15.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Quill) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 26:
		{
			Girl girl16 = Girl.FindGirl(Balance.GirlName.Elle);
			return girl16 != null && girl16.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Elle) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 27:
		{
			Girl girl17 = Girl.FindGirl(Balance.GirlName.Iro);
			return girl17 != null && girl17.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Iro) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 28:
		{
			Girl girl18 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			return girl18 != null && girl18.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bonnibel) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 29:
		{
			Girl girl19 = Girl.FindGirl(Balance.GirlName.Fumi);
			return girl19 != null && girl19.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Fumi) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 30:
		{
			Girl girl20 = Girl.FindGirl(Balance.GirlName.Bearverly);
			return girl20 != null && girl20.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bearverly) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 31:
		{
			Girl girl21 = Girl.FindGirl(Balance.GirlName.Nina);
			return girl21 != null && girl21.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Nina) != (Playfab.PhoneFlingPurchases)0L;
		}
		case 32:
		{
			Girl girl22 = Girl.FindGirl(Balance.GirlName.Alpha);
			return girl22 != null && girl22.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Alpha) != (Playfab.PhoneFlingPurchases)0L;
		}
		default:
			return false;
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00031E30 File Offset: 0x00030030
	public static bool HasConversation(short id, int conversationId)
	{
		PhoneModel phoneModel = Universe.CellphoneGirls[id];
		switch (id)
		{
		case 1:
			if (conversationId == 1)
			{
				string @string = Universe.CellphoneGirls[8].PathPref.GetString(string.Empty);
				string[] array = @string.Split(new char[]
				{
					'`'
				});
				return array.Length >= 27;
			}
			return conversationId == 2;
		case 2:
			if (conversationId == 1)
			{
				Girl girl = Girl.FindGirl(Balance.GirlName.Iro);
				return girl != null && girl.Love >= 7;
			}
			return conversationId == 2;
		case 3:
			if (conversationId == 1)
			{
				Girl girl2 = Girl.FindGirl(Balance.GirlName.Ayano);
				return girl2 != null && girl2.Love >= 6;
			}
			return false;
		case 4:
			if (conversationId == 1)
			{
				Girl girl3 = Girl.FindGirl(Balance.GirlName.Quill);
				return girl3 != null && girl3.Love >= 9;
			}
			if (conversationId == 2)
			{
				Girl girl4 = Girl.FindGirl(Balance.GirlName.Elle);
				return girl4 != null && girl4.Love >= 9;
			}
			if (conversationId == 3)
			{
				Girl girl5 = Girl.FindGirl(Balance.GirlName.Iro);
				return girl5 != null && girl5.Love >= 9;
			}
			return false;
		case 5:
			if (conversationId == 1)
			{
				Job2 job = null;
				foreach (Job2 job2 in Job2.ActiveJobs)
				{
					if (job2.JobType == Requirement.JobType.Lifeguard)
					{
						job = job2;
					}
				}
				return !(job == null) && job.Level >= 5;
			}
			if (conversationId == 2)
			{
				int num = Skills.SkillLevel[5];
				return num >= 35;
			}
			if (conversationId == 3)
			{
				int num2 = Skills.SkillLevel[11];
				return num2 >= 45;
			}
			return false;
		case 6:
			if (conversationId == 1)
			{
				Job2 job3 = null;
				foreach (Job2 job4 in Job2.ActiveJobs)
				{
					if (job4.JobType == Requirement.JobType.Movies)
					{
						job3 = job4;
					}
				}
				return !(job3 == null) && job3.Level >= 1;
			}
			if (conversationId == 2)
			{
				int num3 = Skills.SkillLevel[6];
				return num3 >= 55;
			}
			if (conversationId == 3)
			{
				int num4 = Skills.SkillLevel[1];
				return num4 >= 60;
			}
			return false;
		case 7:
			if (conversationId == 1)
			{
				Girl girl6 = Girl.FindGirl(Balance.GirlName.Bearverly);
				return girl6 != null && girl6.Love >= 2;
			}
			return false;
		case 8:
			if (conversationId == 1)
			{
				int num5 = Skills.SkillLevel[6];
				return num5 >= 62;
			}
			return false;
		case 9:
			return conversationId == 1 && GameState.CurrentState.TimeMultiplier.Value >= 1024f;
		case 10:
			if (conversationId == 1)
			{
				string string2 = Universe.CellphoneGirls[11].PathPref.GetString(string.Empty);
				string[] array2 = string2.Split(new char[]
				{
					'`'
				});
				return array2.Length >= 363;
			}
			if (conversationId == 2)
			{
				Job2 job5 = null;
				foreach (Job2 job6 in Job2.ActiveJobs)
				{
					if (job6.JobType == Requirement.JobType.Sports)
					{
						job5 = job6;
					}
				}
				return !(job5 == null) && job5.Level >= 5;
			}
			return false;
		case 11:
			if (conversationId == 1)
			{
				int num6 = Skills.SkillLevel[4];
				return num6 >= 25;
			}
			if (conversationId == 2)
			{
				int num7 = Skills.SkillLevel[4];
				return num7 >= 30;
			}
			if (conversationId == 3)
			{
				int num8 = Skills.SkillLevel[4];
				return num8 >= 40;
			}
			if (conversationId == 4)
			{
				int num9 = Skills.SkillLevel[4];
				return num9 >= 45;
			}
			return false;
		case 12:
			if (conversationId == 1)
			{
				Girl girl7 = Girl.FindGirl(Balance.GirlName.Nina);
				return girl7 != null && girl7.Love >= 9;
			}
			if (conversationId == 2)
			{
				int num10 = Skills.SkillLevel[6];
				return num10 >= 70;
			}
			if (conversationId == 3)
			{
				int num11 = Skills.SkillLevel[4];
				return num11 >= 73;
			}
			return false;
		case 13:
			if (conversationId == 1)
			{
				int num12 = Skills.SkillLevel[9];
				return num12 >= 40;
			}
			if (conversationId == 2)
			{
				Job2 job7 = null;
				foreach (Job2 job8 in Job2.ActiveJobs)
				{
					if (job8.JobType == Requirement.JobType.Art)
					{
						job7 = job8;
					}
				}
				return !(job7 == null) && job7.Level >= 8;
			}
			if (conversationId == 3)
			{
				int num13 = Skills.SkillLevel[9];
				return num13 >= 65;
			}
			if (conversationId == 4)
			{
				int num14 = Skills.SkillLevel[1];
				return num14 >= 70;
			}
			return false;
		case 14:
			if (conversationId == 1)
			{
				int num15 = Skills.SkillLevel[0];
				return num15 >= 42;
			}
			if (conversationId == 2)
			{
				int num16 = Skills.SkillLevel[7];
				int num17 = Skills.SkillLevel[11];
				return num16 >= 48 && num17 >= 48;
			}
			if (conversationId == 3)
			{
				int num18 = Skills.SkillLevel[1];
				return num18 >= 55;
			}
			return false;
		case 15:
			if (conversationId == 1)
			{
				int num19 = Skills.SkillLevel[1];
				return num19 >= 46;
			}
			if (conversationId == 2)
			{
				string string3 = Universe.CellphoneGirls[13].PathPref.GetString(string.Empty);
				string[] array3 = string3.Split(new char[]
				{
					'`'
				});
				return array3.Length >= 388;
			}
			if (conversationId == 3)
			{
				int num20 = Skills.SkillLevel[11];
				return num20 >= 64;
			}
			return false;
		case 16:
			if (conversationId == 1)
			{
				Girl girl8 = Girl.FindGirl(Balance.GirlName.Bearverly);
				return girl8 != null && girl8.Love >= 9;
			}
			if (conversationId == 2)
			{
				int num21 = Skills.SkillLevel[8];
				return num21 >= 43;
			}
			return false;
		case 19:
			if (conversationId == 1)
			{
				Job2 job9 = null;
				foreach (Job2 job10 in Job2.ActiveJobs)
				{
					if (job10.JobType == Requirement.JobType.Zoo)
					{
						job9 = job10;
					}
				}
				return !(job9 == null) && job9.Level >= 6;
			}
			if (conversationId == 2)
			{
				Job2 job11 = null;
				foreach (Job2 job12 in Job2.ActiveJobs)
				{
					if (job12.JobType == Requirement.JobType.Zoo)
					{
						job11 = job12;
					}
				}
				return !(job11 == null) && job11.Level >= 8;
			}
			if (conversationId == 3)
			{
				Job2 job13 = null;
				foreach (Job2 job14 in Job2.ActiveJobs)
				{
					if (job14.JobType == Requirement.JobType.Legal)
					{
						job13 = job14;
					}
				}
				return !(job13 == null) && job13.Level >= 8;
			}
			return false;
		}
		return false;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00032748 File Offset: 0x00030948
	private void Init()
	{
		GameState.AssetManager.GetBundle("cellphone/common", false, delegate(AssetBundle bundle)
		{
			if (bundle != null)
			{
				this.commonBundle = bundle;
				this.AttachClosures();
			}
		});
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00032768 File Offset: 0x00030968
	private Sprite GetContactIcon(PhoneModel girl)
	{
		if (this.commonBundle == null)
		{
			return null;
		}
		Sprite sprite;
		if (this.contactIcons.TryGetValue(girl.Id, out sprite))
		{
			if (girl.Id == 9 && this.HoneyIsOlder() && sprite.name != "Icon_ honey1")
			{
				sprite = this.commonBundle.LoadAsset<Sprite>("Icon_" + girl.AssetBundle + "1");
				this.contactIcons[girl.Id] = sprite;
			}
			else if (girl.Id == 9 && !this.HoneyIsOlder() && sprite.name != "Icon_honey")
			{
				sprite = this.commonBundle.LoadAsset<Sprite>("Icon_" + girl.AssetBundle);
				this.contactIcons[girl.Id] = sprite;
			}
			return sprite;
		}
		if (girl.Id == 9 && this.HoneyIsOlder())
		{
			sprite = this.commonBundle.LoadAsset<Sprite>("Icon_" + girl.AssetBundle + "1");
		}
		else
		{
			sprite = this.commonBundle.LoadAsset<Sprite>("Icon_" + girl.AssetBundle);
		}
		this.contactIcons[girl.Id] = sprite;
		return sprite;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x000328D0 File Offset: 0x00030AD0
	private void UpdateIcons()
	{
		if (this.contactsPanel == null)
		{
			return;
		}
		int num = 0;
		while (num < this.availableFlings.Count && num < this.contactsPanel.childCount)
		{
			Transform child = this.contactsPanel.GetChild(num);
			short num2 = this.availableFlings[num];
			PhoneModel girl = Universe.CellphoneGirls[num2];
			Sprite sprite = (!Cellphone.HasPinup(num2)) ? this.UnknownGirl : this.GetContactIcon(girl);
			child.transform.Find("Icon").GetComponent<Image>().sprite = sprite;
			if (this.CurrentGirl != null && num2 == this.CurrentGirl.Id)
			{
				base.transform.Find("Phone UI/Messenger/Top/Contact Icon").GetComponent<Image>().sprite = sprite;
			}
			bool flag = Cellphone.IsUnlocked(num2);
			child.GetComponent<Button>().interactable = flag;
			if (!flag && this.NeedsPurchasing(num2))
			{
				child.transform.Find("Name").GetComponent<Text>().text = this.GetNameOrRequirement(child.transform, true, num2);
				child.transform.Find("Purchase").gameObject.SetActive(true);
			}
			else
			{
				child.transform.Find("Name").GetComponent<Text>().text = this.GetNameOrRequirement(child.transform, flag, num2);
			}
			num++;
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00032A50 File Offset: 0x00030C50
	public List<NotificationData> GetPhoneFlingNotifications()
	{
		if (this.availableFlings.Count == 0)
		{
			foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
			{
				this.availableFlings.Add(keyValuePair.Key);
			}
			this.availableFlings.Sort();
		}
		List<NotificationData> list = new List<NotificationData>();
		foreach (KeyValuePair<short, PhoneModel> keyValuePair2 in Universe.CellphoneGirls)
		{
			short key = keyValuePair2.Key;
			if (Cellphone.IsUnlocked(key))
			{
				int num;
				if (Cellphone.HasNotification(key, out num) == Cellphone.NotificationType.None && num != -4 && num >= 7200)
				{
					PhoneModel phoneModel = Universe.CellphoneGirls[key];
					int num2 = phoneModel.PathPref.GetString(string.Empty).Split(new char[]
					{
						'`'
					}).Length - 1;
					if (phoneModel.Messages.Count > num2)
					{
						PhoneModel.PhoneMessage phoneMessage = phoneModel.Messages[num2];
						string name = phoneModel.Name;
						string text = (!Cellphone.HasPinup(key)) ? "icon_locked" : ("icon_" + phoneModel.AssetBundle.ToLowerInvariant());
						string text2 = (!(phoneMessage is PhoneModel.PhoneImage)) ? phoneModel.Messages[num2].GetTranslatedMessage(phoneModel) : (phoneModel.Name + " has sent you an image.");
						List<NotificationData> list2 = list;
						string largeIcon = text;
						list2.Add(new NotificationData(MobileNotificationType.PhoneFlingNewText, name, text2, DateTime.Now.AddSeconds((double)num), text2, "icon_small_default", largeIcon, (int)key));
					}
				}
			}
		}
		return list;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00032C74 File Offset: 0x00030E74
	public bool NeedsPurchasing(short id)
	{
		if (id == 23)
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.Cassie);
			return girl != null && girl.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Cassie) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 24)
		{
			Girl girl2 = Girl.FindGirl(Balance.GirlName.Mio);
			return girl2 != null && girl2.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Mio) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 25)
		{
			Girl girl3 = Girl.FindGirl(Balance.GirlName.Quill);
			return girl3 != null && girl3.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Quill) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 26)
		{
			Girl girl4 = Girl.FindGirl(Balance.GirlName.Elle);
			return girl4 != null && girl4.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Elle) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 27)
		{
			Girl girl5 = Girl.FindGirl(Balance.GirlName.Iro);
			return girl5 != null && girl5.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Iro) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 28)
		{
			Girl girl6 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			return girl6 != null && girl6.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bonnibel) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 29)
		{
			Girl girl7 = Girl.FindGirl(Balance.GirlName.Fumi);
			return girl7 != null && girl7.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Fumi) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 30)
		{
			Girl girl8 = Girl.FindGirl(Balance.GirlName.Bearverly);
			return girl8 != null && girl8.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Bearverly) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 31)
		{
			Girl girl9 = Girl.FindGirl(Balance.GirlName.Nina);
			return girl9 != null && girl9.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Nina) == (Playfab.PhoneFlingPurchases)0L;
		}
		if (id == 32)
		{
			Girl girl10 = Girl.FindGirl(Balance.GirlName.Alpha);
			return girl10 != null && girl10.Love >= 9 && (Playfab.FlingPurchases & Playfab.PhoneFlingPurchases.Alpha) == (Playfab.PhoneFlingPurchases)0L;
		}
		return false;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00032ED8 File Offset: 0x000310D8
	public void UpdateNotifications()
	{
		if (this.availableFlings.Count == 0)
		{
			foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
			{
				this.availableFlings.Add(keyValuePair.Key);
			}
			this.availableFlings.Sort();
		}
		Cellphone.NotificationType notificationType = Cellphone.NotificationType.None;
		for (int i = 0; i < this.availableFlings.Count; i++)
		{
			short num = this.availableFlings[i];
			if (this.CurrentGirl == null || this.CurrentGirl.Id != num)
			{
				int num2;
				Cellphone.NotificationType notificationType2 = Cellphone.HasNotification(num, out num2);
				if (this.contactsPanel != null && i < this.contactsPanel.childCount)
				{
					Transform child = this.contactsPanel.GetChild(i);
					bool flag = Cellphone.IsUnlocked(num);
					child.transform.Find("Name").GetComponent<Text>().text = this.GetNameOrRequirement(child, flag | this.NeedsPurchasing(num), num);
					child.transform.Find("Notification").gameObject.SetActive(notificationType2 == Cellphone.NotificationType.NewMessage);
					child.transform.Find("Reply").gameObject.SetActive(notificationType2 == Cellphone.NotificationType.PendingReply);
					child.GetComponent<Button>().interactable = flag;
				}
				notificationType |= notificationType2;
			}
		}
		GameState.CurrentState.transform.Find("Top UI/Cellphone").GetComponent<Image>().sprite = ((notificationType == Cellphone.NotificationType.None) ? this.CellphoneSprite : this.CellphoneNotificationSprite);
		GameState.CurrentState.transform.Find("Top UI/Cellphone").GetComponent<Wiggle>().AlwaysOn = (notificationType != Cellphone.NotificationType.None);
		if ((this.currentNotification & Cellphone.NotificationType.NewMessage) == Cellphone.NotificationType.None && (notificationType & Cellphone.NotificationType.NewMessage) != Cellphone.NotificationType.None)
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneVibrate);
		}
		this.currentNotification = notificationType;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00033100 File Offset: 0x00031300
	private string GetNameOrRequirement(Transform prefab, bool unlocked, short id)
	{
		if (!unlocked)
		{
			return Translations.GetCellphoneRequirement(id);
		}
		long @long = Universe.CellphoneGirls[id].DatePref.GetLong(0L);
		string[] array = Universe.CellphoneGirls[id].PathPref.GetString(string.Empty).Split(new char[]
		{
			'`'
		});
		if (array.Length > 1)
		{
			int num = int.Parse(array[array.Length - 1]);
			int num2 = array.Length - 1;
			if ((@long == 0L || @long == 9223372036854775807L || num == -4) && num2 < Universe.CellphoneGirls[id].MessageCount && !Cellphone.HasConversation(id, Universe.CellphoneGirls[id].Messages[num2].ConversationId))
			{
				return Translations.GetCellphoneConvoRequirement(id, (short)Universe.CellphoneGirls[id].Messages[num2].ConversationId);
			}
		}
		return Translations.GetCellphoneName(Universe.CellphoneGirls[id]);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00033208 File Offset: 0x00031408
	private bool HoneyIsOlder()
	{
		string @string = Universe.CellphoneGirls[9].PathPref.GetString(string.Empty);
		string[] array = @string.Split(new char[]
		{
			'`'
		});
		return array.Length > 257;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00033250 File Offset: 0x00031450
	private void AttachClosures()
	{
		this.contactButtons = new Button[Universe.CellphoneGirls.Count];
		this.availableFlings.Clear();
		foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
		{
			this.availableFlings.Add(keyValuePair.Key);
		}
		this.availableFlings.Sort();
		for (int i = 0; i < this.availableFlings.Count; i++)
		{
			GameObject prefab = (GameObject)UnityEngine.Object.Instantiate(this.ContactPrefab, this.contactsPanel);
			prefab.transform.localScale = Vector3.one;
			prefab.transform.localPosition = Vector3.zero;
			short id = this.availableFlings[i];
			PhoneModel phoneModel = Universe.CellphoneGirls[id];
			bool flag = Cellphone.IsUnlocked(id);
			prefab.transform.Find("Name").GetComponent<Text>().text = this.GetNameOrRequirement(prefab.transform, flag | this.NeedsPurchasing(id), id);
			if (!flag && this.NeedsPurchasing(id))
			{
				prefab.transform.Find("Purchase").gameObject.SetActive(true);
			}
			Sprite sprite;
			if (id == 9 && this.HoneyIsOlder())
			{
				sprite = this.commonBundle.LoadAsset<Sprite>("Icon_" + phoneModel.AssetBundle + "1");
			}
			else
			{
				sprite = ((!Cellphone.HasPinup(id)) ? this.UnknownGirl : this.commonBundle.LoadAsset<Sprite>("Icon_" + phoneModel.AssetBundle));
			}
			prefab.transform.Find("Icon").GetComponent<Image>().sprite = sprite;
			int num;
			Cellphone.NotificationType notificationType = Cellphone.HasNotification(id, out num);
			prefab.transform.Find("Notification").gameObject.SetActive(notificationType == Cellphone.NotificationType.NewMessage);
			prefab.transform.Find("Reply").gameObject.SetActive(notificationType == Cellphone.NotificationType.PendingReply);
			if (id >= 23)
			{
				prefab.transform.Find("Purchase").GetComponent<Button>().onClick.AddListener(delegate()
				{
					Utilities.ConfirmPurchase(10, delegate
					{
						if (id == 23)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Cassie;
						}
						else if (id == 24)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Mio;
						}
						else if (id == 25)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Quill;
						}
						else if (id == 26)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Elle;
						}
						else if (id == 27)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Iro;
						}
						else if (id == 28)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Bonnibel;
						}
						else if (id == 29)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Fumi;
						}
						else if (id == 30)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Bearverly;
						}
						else if (id == 31)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Nina;
						}
						else if (id == 32)
						{
							Playfab.FlingPurchases |= Playfab.PhoneFlingPurchases.Alpha;
						}
						global::PlayerPrefs.SetLong("PlayfabFlingPurchases", (long)Playfab.FlingPurchases);
						this.UpdateIcons();
						prefab.transform.Find("Purchase").gameObject.SetActive(false);
						if (Girls.CurrentGirl != null)
						{
							GameState.GetGirlScreen().UpdateRequirements(Girls.CurrentGirl);
						}
					});
				});
			}
			prefab.GetComponent<Button>().onClick.AddListener(delegate()
			{
				foreach (Button button in this.contactButtons)
				{
					button.interactable = false;
				}
				this.StopAllCoroutines();
				this.DisableReplies();
				for (int k = this.contentPanel.childCount - 1; k >= 0; k--)
				{
					UnityEngine.Object.Destroy(this.contentPanel.GetChild(k).gameObject);
				}
				this.transform.Find("Phone UI/Messenger/Top/Contact Icon").GetComponent<Image>().sprite = prefab.transform.Find("Icon").GetComponent<Image>().sprite;
				this.CurrentGirl = Universe.CellphoneGirls[id];
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Select);
				this.InitGallery();
				this.LoadState();
			});
			prefab.GetComponent<Button>().interactable = flag;
			this.contactButtons[i] = prefab.GetComponent<Button>();
		}
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00033594 File Offset: 0x00031794
	private void EnableMessenger()
	{
		this.messenger.SetActive(true);
		this.contacts.SetActive(false);
		int num = 0;
		while (num < this.availableFlings.Count && num < this.contactButtons.Length)
		{
			bool flag = Cellphone.IsUnlocked(this.availableFlings[num]);
			this.contactButtons[num].interactable = flag;
			if (flag)
			{
				Utilities.SendPFUnlockedEvent(Universe.CellphoneGirls[this.availableFlings[num]]);
			}
			num++;
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00033624 File Offset: 0x00031824
	private void InitGallery()
	{
		if (this.CurrentGirl == null)
		{
			return;
		}
		Transform transform = this.gallery.transform.Find("Content");
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < this.CurrentGirl.SpriteCount; j++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.GalleryPrefab, transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x000336CC File Offset: 0x000318CC
	private void LoadState()
	{
		long num = this.CurrentGirl.DatePref.GetLong(0L);
		if (num == 0L)
		{
			this.choices.Clear();
			this.YieldReply(this.CurrentGirl.FirstMessage);
			this.EnableMessenger();
		}
		else
		{
			if (num == 9223372036854775807L)
			{
				num = DateTime.UtcNow.ToBinary();
				this.CurrentGirl.DatePref.SetLong(num);
				GameState.CurrentState.QueueSave();
			}
			base.StartCoroutine(this.ReplayConversation(DateTime.FromBinary(num), this.CurrentGirl.PathPref.GetString(string.Empty)));
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0003377C File Offset: 0x0003197C
	private IEnumerator ReplayConversation(DateTime lastOpened, string status)
	{
		GameState.Voiceover.AddPlaybackBlocker(base.gameObject);
		string[] split = status.Split(new char[]
		{
			'`'
		});
		PhoneModel.PhoneMessage currentMessage = this.CurrentGirl.FirstMessage;
		this.choices.Clear();
		for (int i = 0; i < split.Length; i++)
		{
			int value;
			if (!int.TryParse(split[i], out value))
			{
				break;
			}
			if (value == -4)
			{
				if (Cellphone.HasConversation(this.CurrentGirl.Id, (int)this.message.Id))
				{
					this.AddMessage(currentMessage, true);
				}
				else
				{
					this.currentTime = -4.0;
				}
			}
			else if (value == -3)
			{
				this.ShowIncomingCall();
			}
			else
			{
				if (value > 0)
				{
					if (lastOpened > DateTime.UtcNow)
					{
						lastOpened = DateTime.UtcNow;
						this.CurrentGirl.DatePref.SetLong(DateTime.UtcNow.ToBinary());
						GameState.CurrentState.QueueSave();
					}
					float seconds = (float)(DateTime.UtcNow - lastOpened).TotalSeconds;
					if (seconds < 0f)
					{
						seconds = 0f;
					}
					this.message = currentMessage;
					if ((float)value > this.message.Time)
					{
						value = (int)this.message.Time;
					}
					if (value > 0)
					{
						this.currentTime = (double)Mathf.Max(0.01f, (float)value - seconds);
						if (this.currentTime > (double)this.message.Time)
						{
							this.currentTime = (double)this.message.Time;
						}
					}
					else
					{
						this.currentTime = (double)value;
					}
					break;
				}
				if (currentMessage is PhoneModel.PhoneResponse)
				{
					if (value == -1)
					{
						if (((PhoneModel.PhoneResponse)currentMessage).Response2.HasMessage)
						{
							this.AddMessage(((PhoneModel.PhoneResponse)currentMessage).Response1, false);
						}
					}
					else
					{
						if (value != -2)
						{
							this.message = currentMessage;
							this.currentTime = 0.0;
							break;
						}
						this.AddMessage(((PhoneModel.PhoneResponse)currentMessage).Response2, false);
					}
					this.choices.Add(value);
				}
				else if (currentMessage is PhoneModel.PhoneImage)
				{
					yield return this.AddImage((PhoneModel.PhoneImage)currentMessage, false);
				}
				else
				{
					this.AddMessage(currentMessage, true);
				}
				if (currentMessage.NextMessage == null)
				{
					this.replyProgressContainer.gameObject.SetActive(false);
					this.purchaseButton.gameObject.SetActive(false);
					this.message = currentMessage;
					this.currentTime = -1.0;
					this.AddResetPrefab();
					this.AddVotePrefab();
					break;
				}
				currentMessage = currentMessage.NextMessage;
				this.message = currentMessage;
			}
		}
		if (this.message == null)
		{
			this.YieldReply(this.CurrentGirl.FirstMessage);
		}
		else if (this.currentTime <= 0.0 && this.message.Time != -3f && this.message.NextMessage != null)
		{
			this.YieldReply(this.message);
			this.replyProgressContainer.gameObject.SetActive(false);
			this.purchaseButton.gameObject.SetActive(false);
			this.StoreState(this.message.NewConversation && !Cellphone.HasConversation(this.CurrentGirl.Id, this.message.ConversationId));
			GameState.CurrentState.QueueSave();
		}
		if (this.currentTime < 0.0 && this.message.NextMessage != null && this.message.Time != -3f)
		{
			this.currentTime = 0.0;
		}
		this.FindRelativePositionInPinups();
		if (this.nextPinupPosition == -1 || (this.message.NewConversation && !Cellphone.HasConversation(this.CurrentGirl.Id, this.message.ConversationId)))
		{
			this.pinupProgressContainer.SetActive(false);
			this.replyPinupIcon.gameObject.SetActive(false);
		}
		else
		{
			this.pinupProgressContainer.SetActive(true);
			this.replyPinupIcon.gameObject.SetActive(true);
			int top = this.replyPosition - this.lastPinupPosition - 1;
			if (top < 0)
			{
				top = 0;
			}
			int bottom = this.nextPinupPosition - this.lastPinupPosition;
			if (bottom <= 0)
			{
				bottom = 1;
			}
			this.pinupPosition.text = string.Format("{0}/{1}", top.ToString(), bottom.ToString());
			this.pinupProgress.fillAmount = (float)top / (float)bottom;
		}
		yield return new WaitForEndOfFrame();
		this.EnableMessenger();
		GameState.Voiceover.RemovePlaybackBlocker(base.gameObject, false);
		yield break;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x000337B4 File Offset: 0x000319B4
	private void AddVotePrefab()
	{
		if (this.CurrentGirl.Id < 4 || this.CurrentGirl.Id == 7 || this.CurrentGirl.Id == 10 || this.CurrentGirl.Id > 19 || this.CurrentGirl.Id == 16)
		{
			return;
		}
		if (!BlayFapClient.LoggedIn)
		{
			return;
		}
		if (!Playfab.Promotion.Contains("flingvote."))
		{
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.VotePrefab, this.contentPanel);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.Find("Vote Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			base.transform.Find("Phone UI/Messenger/Vote").GetComponent<Voting>().Init(this.CurrentGirl);
		});
		if (gameObject.transform.Find("Results Button") != null)
		{
			gameObject.transform.Find("Results Button").GetComponent<Button>().onClick.AddListener(delegate()
			{
				base.transform.Find("Phone UI/Messenger/Vote").GetComponent<Voting>().ShowResults();
			});
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x000338E4 File Offset: 0x00031AE4
	private void AddResetPrefab()
	{
		if (this.CurrentGirl.Id == 1)
		{
			return;
		}
		if (this.CurrentGirl.Id == 2 || this.CurrentGirl.Id == 7 || this.CurrentGirl.Id == 3 || this.CurrentGirl.Id == 10 || this.CurrentGirl.Id == 16 || this.CurrentGirl.Id == 11)
		{
			this.AddLTEPrefab();
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ResetPrefab, this.contentPanel);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.Find("Reset Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			Transform transform = base.transform.Find("Phone UI/Messenger/Reset Fling");
			transform.Find("Dialog/Text").GetComponent<Text>().text = string.Format("Would you like to reset {0} so that you can play through the phone fling from the beginning?", Translations.GetCellphoneName(this.CurrentGirl));
			transform.gameObject.SetActive(true);
		});
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x000339D8 File Offset: 0x00031BD8
	private void AddLTEPrefab()
	{
		int eventID = (this.CurrentGirl.Id != 11) ? ((this.CurrentGirl.Id != 16) ? ((this.CurrentGirl.Id != 10) ? ((this.CurrentGirl.Id != 3) ? ((this.CurrentGirl.Id != 2) ? 116 : 113) : 119) : 140) : 161) : 179;
		if (this.CurrentGirl.Id == 2)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID))
			{
				GameState.GetGirlScreen().UnlockGirls();
				GameState.GetAchievements().Rebuild();
				return;
			}
		}
		else if (this.CurrentGirl.Id == 7)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID))
			{
				GameState.GetGirlScreen().UnlockGirls();
				GameState.GetAchievements().Rebuild();
				return;
			}
		}
		else if (this.CurrentGirl.Id == 3)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Generica) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID))
			{
				GameState.GetGirlScreen().UnlockGirls();
				GameState.GetAchievements().Rebuild();
				return;
			}
		}
		else if (this.CurrentGirl.Id == 10)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID))
			{
				GameState.GetGirlScreen().UnlockGirls();
				GameState.GetAchievements().Rebuild();
				return;
			}
		}
		else if (this.CurrentGirl.Id == 16)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Renee) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID))
			{
				GameState.GetGirlScreen().UnlockGirls();
				GameState.GetAchievements().Rebuild();
				return;
			}
		}
		else if (this.CurrentGirl.Id == 11 && ((Playfab.AwardedItems & Playfab.PlayfabItems.Lake) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(eventID)))
		{
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			return;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.LTEPrefab, this.contentPanel);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		TaskManager taskManager = GameState.CurrentState.TaskSystem;
		if ((taskManager != null && taskManager.CurrentEvent != null && (int)taskManager.CurrentEvent.EventID < eventID) || taskManager.CurrentEvent == null)
		{
			gameObject.transform.Find("Button").GetComponent<Button>().interactable = false;
			gameObject.transform.Find("Button/Text").GetComponent<Text>().text = "Coming Soon";
		}
		else if (taskManager != null && taskManager.CurrentEvent != null && (int)taskManager.CurrentEvent.EventID <= eventID + 2)
		{
			gameObject.transform.Find("Button/Text").GetComponent<Text>().text = "Open LTE";
		}
		else
		{
			gameObject.transform.Find("Button/Text").GetComponent<Text>().text = "Open Store";
		}
		gameObject.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			if (taskManager != null && taskManager.CurrentEvent != null && (int)taskManager.CurrentEvent.EventID <= eventID + 2)
			{
				GameState.CurrentState.GetComponent<PanelScript>().ClickLTE();
			}
			else
			{
				GameState.CurrentState.GetComponent<PanelScript>().ClickBundles();
				string text = this.CurrentGirl.AssetBundle.ToLowerInvariant();
				text = text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);
				GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().ScrollToGirl(text, false, false, string.Empty);
			}
			this.gameObject.SetActive(false);
		});
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00033DE0 File Offset: 0x00031FE0
	public void ResetCurrentGirl()
	{
		this.choices.Clear();
		this.StoreState(false);
		base.transform.Find("Phone UI/Messenger/Reset Fling").gameObject.SetActive(false);
		base.transform.Find("Phone UI/Messenger/Top/Contact Icon").GetComponent<Image>().sprite = this.UnknownGirl;
		PhoneModel currentGirl = this.CurrentGirl;
		this.DisposeGirl();
		this.CurrentGirl = currentGirl;
		this.InitGallery();
		base.StartCoroutine(this.ReplayConversation(DateTime.UtcNow, string.Empty));
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00033E6C File Offset: 0x0003206C
	private void StoreState(bool endOfFling = false)
	{
		if (this.CurrentGirl == null)
		{
			return;
		}
		this.CurrentGirl.DatePref.SetLong(DateTime.UtcNow.ToBinary());
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.choices.Count; i++)
		{
			stringBuilder.Append(string.Format("{0}`", this.choices[i].ToString()));
		}
		if (!endOfFling)
		{
			stringBuilder.Append(((int)Math.Ceiling(Math.Max(0.0, this.currentTime))).ToString());
		}
		else
		{
			stringBuilder.Append(-4.ToString());
		}
		this.CurrentGirl.PathPref.SetString(stringBuilder.ToString());
		GameState.CurrentState.QueueSave();
		GameState.CurrentState.CheckCellphoneUnlock();
		if (this.CurrentGirl.Id == 1)
		{
			GameState.GetGirlScreen().UnlockGirls();
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00033F78 File Offset: 0x00032178
	public static void TimeSkip(int seconds)
	{
		TimeSpan t = TimeSpan.FromSeconds((double)seconds);
		foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
		{
			long @long = keyValuePair.Value.DatePref.GetLong(0L);
			if (@long != 0L && @long != 9223372036854775807L)
			{
				DateTime d = DateTime.FromBinary(@long);
				d -= t;
				keyValuePair.Value.DatePref.SetLong(d.ToBinary());
			}
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00034038 File Offset: 0x00032238
	public void BackArrow()
	{
		if (this.fullscreenImage.gameObject.activeSelf)
		{
			this.fullscreenImage.gameObject.SetActive(false);
			base.transform.Find("Phone UI/Name").gameObject.SetActive(false);
			this.galleryIcon.SetActive(!this.gallery.gameObject.activeSelf);
		}
		else if (this.gallery.gameObject.activeSelf)
		{
			this.gallery.gameObject.SetActive(false);
			this.galleryIcon.SetActive(true);
		}
		else
		{
			GameState.Voiceover.Stop();
			this.messenger.SetActive(false);
			this.contacts.SetActive(true);
			this.DisposeGirl();
			this.UpdateIcons();
			this.UpdateNotifications();
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00034114 File Offset: 0x00032314
	private void DisposeGirl()
	{
		this.disabledTransforms.Clear();
		this.CurrentGirl = null;
		this.currentBundle = (this.currentBundleNsfw = null);
		this.message = null;
		this.currentTime = -1.0;
		this.imageIndex = 0;
		if (this.contentPanel != null && this.contentPanel.childCount > 0)
		{
			for (int i = this.contentPanel.childCount - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.contentPanel.GetChild(i).gameObject);
			}
		}
		base.transform.Find("Phone UI/Messenger/Incoming Call").gameObject.SetActive(false);
		base.transform.Find("Phone UI/Messenger/Call").gameObject.SetActive(false);
		this.inCall = false;
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x000341F4 File Offset: 0x000323F4
	private void AddDivider(PhoneModel.PhoneMessage message)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ConversationDivider, this.contentPanel);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00034238 File Offset: 0x00032438
	private void AddMessage(PhoneModel.PhoneMessage message, bool fromGirl)
	{
		if (fromGirl && this.contentPanel.childCount > 0)
		{
			Transform child = this.contentPanel.GetChild(this.contentPanel.childCount - 1);
			if (child.name.Contains("Pending"))
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (message.NewConversation)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ConversationDivider, this.contentPanel);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
		}
		GameObject original = (!fromGirl) ? this.ReplyMessage : this.GirlMessage;
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(original, this.contentPanel);
		this.disabledTransforms.Add(gameObject2.GetComponent<RectTransform>());
		if (fromGirl)
		{
			this.choices.Add(0);
		}
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localPosition = Vector3.zero;
		Text component = gameObject2.transform.Find("Background/Text").GetComponent<Text>();
		if (fromGirl && message.HasAlternateMessage)
		{
			int num = 0;
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != 0)
				{
					num = this.choices[i];
					break;
				}
			}
			if (num == -2)
			{
				component.text = message.GetTranslatedAlternateMessage(this.CurrentGirl);
			}
			else
			{
				component.text = message.GetTranslatedMessage(this.CurrentGirl);
			}
		}
		else
		{
			component.text = message.GetTranslatedMessage(this.CurrentGirl);
		}
		if (this.textGen == null)
		{
			this.textGen = new TextGenerator();
		}
		Text component2 = gameObject2.transform.Find("Background/Text").GetComponent<Text>();
		TextGenerationSettings generationSettings = component2.GetGenerationSettings(new Vector2(199f, 400f));
		generationSettings.scaleFactor = 1f;
		float num2 = this.textGen.GetPreferredHeight(component.text, generationSettings) / 17f;
		gameObject2.GetComponent<LayoutElement>().preferredHeight = 28f + num2 * 17f;
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0003448C File Offset: 0x0003268C
	private void AddPendingMessage()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.PendingMessage, this.contentPanel);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x000344D0 File Offset: 0x000326D0
	private IEnumerator LoadAssetBundle()
	{
		if (this.currentBundle == null || !this.currentBundle.name.EndsWith(this.CurrentGirl.AssetBundle))
		{
			if (GameState.NSFW)
			{
				AssetBundleAsync nsfwBundleRequest = GameState.AssetManager.GetBundleAsync("cellphone/" + this.CurrentGirl.AssetBundle + "_nsfw", false);
				yield return nsfwBundleRequest;
				if (nsfwBundleRequest == null)
				{
					Debug.LogError("Failed to load bundle cellphone/" + this.CurrentGirl.AssetBundle + "_nsfw");
				}
				else
				{
					this.currentBundleNsfw = nsfwBundleRequest.AssetBundle;
				}
			}
			AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync("cellphone/" + this.CurrentGirl.AssetBundle, false);
			yield return bundleRequest;
			if (bundleRequest == null)
			{
				Debug.LogError("Failed to load bundle cellphone/" + this.CurrentGirl.AssetBundle);
			}
			else
			{
				this.currentBundle = bundleRequest.AssetBundle;
			}
			if (this.CurrentGirl.Id == 16)
			{
				yield return GameState.Voiceover.LoadSpecialBundle(Balance.GirlName.ReneePF, true);
			}
			if (this.CurrentGirl.Id == 13)
			{
				yield return GameState.Voiceover.LoadSpecialBundle(Balance.GirlName.NovaPF, true);
			}
		}
		yield break;
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x000344EC File Offset: 0x000326EC
	private IEnumerator AddImage(PhoneModel.PhoneImage image, bool storeState = true)
	{
		yield return this.LoadAssetBundle();
		Sprite sprite = null;
		string spriteName = image.SpriteName1;
		if (!string.IsNullOrEmpty(image.SpriteName2))
		{
			int lastChoice = 0;
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != 0)
				{
					lastChoice = this.choices[i];
					break;
				}
			}
			if (lastChoice == -2)
			{
				spriteName = image.SpriteName2;
			}
		}
		if (GameState.NSFW && this.currentBundleNsfw != null)
		{
			Sprite nsfwSprite = this.currentBundleNsfw.LoadAsset<Sprite>(spriteName + "nsfw");
			if (nsfwSprite != null)
			{
				sprite = nsfwSprite;
			}
		}
		if (sprite == null && this.currentBundle != null)
		{
			sprite = this.currentBundle.LoadAsset<Sprite>(spriteName);
		}
		if (sprite == null)
		{
			Debug.LogError("Could not load sprite " + spriteName);
			yield break;
		}
		this.message = image;
		GameObject prefab = (GameObject)UnityEngine.Object.Instantiate((!image.FromPlayer) ? this.ImageMessage : this.PlayerImageMessage, this.contentPanel);
		this.choices.Add(0);
		prefab.transform.localScale = Vector3.one;
		prefab.transform.localPosition = Vector3.zero;
		prefab.transform.Find("Background/Image").GetComponent<Image>().sprite = sprite;
		if (image.InGallery)
		{
			if (this.gallery.transform.Find("Content") == null || this.gallery.transform.Find("Content").childCount <= this.imageIndex || this.gallery.transform.Find("Content").GetChild(this.imageIndex) == null)
			{
				yield break;
			}
			Image galleryImage = this.gallery.transform.Find("Content").GetChild(this.imageIndex).GetComponent<Image>();
			galleryImage.sprite = sprite;
			galleryImage.GetComponent<Button>().onClick.RemoveAllListeners();
			galleryImage.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.fullscreenImage.sprite = sprite;
				this.fullscreenImage.gameObject.SetActive(true);
			});
			this.imageIndex++;
		}
		Button button = prefab.transform.Find("Background/Image").GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(delegate()
		{
			this.fullscreenImage.sprite = sprite;
			this.fullscreenImage.gameObject.SetActive(true);
			this.galleryIcon.SetActive(false);
		});
		if (storeState)
		{
			this.YieldReply(image.NextMessage);
			this.StoreState(false);
			this.UpdateIcons();
		}
		yield break;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00034524 File Offset: 0x00032724
	private void AddReplies(PhoneModel.PhoneResponse response)
	{
		if (!response.Response2.HasMessage)
		{
			base.transform.Find("Phone UI/Messenger/Reply/Reply 1").gameObject.SetActive(false);
			base.transform.Find("Phone UI/Messenger/Reply/Reply 2").gameObject.SetActive(false);
			base.transform.Find("Phone UI/Messenger/Reply/Action").gameObject.SetActive(true);
			base.transform.Find("Phone UI/Messenger/Reply/Divider/Text").gameObject.SetActive(false);
			base.transform.Find("Phone UI/Messenger/Reply/Action/Text").GetComponent<Text>().text = response.Response1.GetTranslatedMessage(this.CurrentGirl);
			base.transform.Find("Phone UI/Messenger/Reply/Action").GetComponent<Button>().onClick.RemoveAllListeners();
			base.transform.Find("Phone UI/Messenger/Reply/Action").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.choices.Add(-1);
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneBubble);
				this.DisableReplies();
				this.YieldReply(response.NextMessage);
				this.StoreState(false);
			});
		}
		else
		{
			base.transform.Find("Phone UI/Messenger/Reply/Reply 1").gameObject.SetActive(true);
			base.transform.Find("Phone UI/Messenger/Reply/Reply 2").gameObject.SetActive(true);
			base.transform.Find("Phone UI/Messenger/Reply/Action").gameObject.SetActive(false);
			base.transform.Find("Phone UI/Messenger/Reply/Divider/Text").gameObject.SetActive(true);
			base.transform.Find("Phone UI/Messenger/Reply/Reply 1/Text").GetComponent<Text>().text = response.Response1.GetTranslatedMessage(this.CurrentGirl);
			base.transform.Find("Phone UI/Messenger/Reply/Reply 2/Text").GetComponent<Text>().text = response.Response2.GetTranslatedMessage(this.CurrentGirl);
			base.transform.Find("Phone UI/Messenger/Reply/Reply 1").GetComponent<Button>().onClick.RemoveAllListeners();
			base.transform.Find("Phone UI/Messenger/Reply/Reply 1").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.AddMessage(response.Response1, false);
				this.choices.Add(-1);
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneBubble);
				this.DisableReplies();
				this.YieldReply(response.NextMessage);
				this.StoreState(false);
			});
			base.transform.Find("Phone UI/Messenger/Reply/Reply 2").GetComponent<Button>().onClick.RemoveAllListeners();
			base.transform.Find("Phone UI/Messenger/Reply/Reply 2").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.AddMessage(response.Response2, false);
				this.choices.Add(-2);
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneBubble);
				this.DisableReplies();
				this.YieldReply(response.NextMessage);
				this.StoreState(false);
			});
		}
		base.transform.Find("Phone UI/Messenger/Messages").GetComponent<RectTransform>().offsetMin = new Vector2(0f, 153f);
		base.transform.Find("Phone UI/Messenger/Reply").gameObject.SetActive(true);
		base.transform.Find("Phone UI/Messenger/Divider").gameObject.SetActive(false);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00034804 File Offset: 0x00032A04
	private void YieldReply(PhoneModel.PhoneMessage currentMessage)
	{
		if (currentMessage == null)
		{
			return;
		}
		Button component = base.transform.Find("Phone UI/Messenger/Divider/Highlight").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.gameObject.SetActive(false);
		if (currentMessage is PhoneModel.PhoneResponse)
		{
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(delegate()
			{
				this.AddReplies((PhoneModel.PhoneResponse)currentMessage);
			});
			component.gameObject.SetActive(true);
			this.message = currentMessage;
		}
		else
		{
			this.currentTime = (double)currentMessage.Time;
			this.message = currentMessage;
			if (this.currentTime > 0.0)
			{
				this.replyProgressContainer.SetActive(true);
				this.purchaseButton.gameObject.SetActive(true);
			}
			else if (this.currentTime == -3.0)
			{
				this.ShowIncomingCall();
			}
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0003491C File Offset: 0x00032B1C
	public void ShowIncomingCall()
	{
		base.transform.Find("Phone UI/Messenger/Incoming Call/Name").GetComponent<Text>().text = Translations.GetCellphoneName(this.CurrentGirl);
		base.transform.Find("Phone UI/Messenger/Incoming Call/Icon").GetComponent<Image>().sprite = this.GetContactIcon(this.CurrentGirl);
		base.transform.Find("Phone UI/Messenger/Incoming Call").gameObject.SetActive(true);
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneRing);
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x000349A0 File Offset: 0x00032BA0
	public void AcceptCall()
	{
		if (this.currentTime == -3.0 || this.message.Time == -3f)
		{
			this.callTime = 0f;
			this.currentTime = 2.0;
		}
		base.transform.Find("Phone UI/Messenger/Incoming Call").gameObject.SetActive(false);
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00034A0C File Offset: 0x00032C0C
	private void YieldMessage(PhoneModel.PhoneMessage currentMessage)
	{
		this.AddMessage(currentMessage, true);
		this.YieldReply(currentMessage.NextMessage);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00034A24 File Offset: 0x00032C24
	private void FindRelativePositionInPinups()
	{
		int num = 1;
		PhoneModel.PhoneMessage phoneMessage = this.CurrentGirl.FirstMessage;
		this.nextPinupPosition = (this.replyPosition = -1);
		this.lastPinupPosition = 0;
		while (phoneMessage != null)
		{
			if (phoneMessage.NewConversation)
			{
				this.replyPosition = -1;
				this.lastPinupPosition = num - 1;
			}
			if (phoneMessage is PhoneModel.PhoneResponse)
			{
				if (phoneMessage == this.message)
				{
					this.replyPosition = num - 1;
				}
			}
			else if (phoneMessage is PhoneModel.PhoneImage)
			{
				if (phoneMessage == this.message)
				{
					this.replyPosition = num;
				}
				if (this.replyPosition == -1)
				{
					this.lastPinupPosition = num;
				}
				else if (this.nextPinupPosition == -1)
				{
					this.nextPinupPosition = num;
				}
				if (this.replyPosition != -1 && this.nextPinupPosition != -1)
				{
					break;
				}
				num++;
			}
			else
			{
				if (phoneMessage == this.message)
				{
					this.replyPosition = num;
				}
				num++;
			}
			phoneMessage = phoneMessage.NextMessage;
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00034B30 File Offset: 0x00032D30
	private void DisableReplies()
	{
		base.transform.Find("Phone UI/Messenger/Messages").GetComponent<RectTransform>().offsetMin = new Vector2(0f, 45f);
		base.transform.Find("Phone UI/Messenger/Reply").gameObject.SetActive(false);
		base.transform.Find("Phone UI/Messenger/Divider").gameObject.SetActive(true);
		base.transform.Find("Phone UI/Messenger/Divider/Highlight").gameObject.SetActive(false);
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00034BB8 File Offset: 0x00032DB8
	public void Purchase()
	{
		int diamonds = Mathf.Min(50, 5 * Mathf.CeilToInt((float)this.currentTime / 21600f));
		PhoneModel.PhoneMessage purchaseMessage = this.message;
		this.ConfirmCellphonePurchase(diamonds, delegate
		{
			if (purchaseMessage == this.message)
			{
				this.currentTime = 0.0;
				Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, string.Format("Cell:{0}:{1}", this.CurrentGirl.Id.ToString(), purchaseMessage.Id.ToString()));
			}
			else
			{
				GameState.Diamonds.Value += diamonds;
			}
		});
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00034C18 File Offset: 0x00032E18
	private void ConfirmCellphonePurchase(int diamonds, Action OnSuccess)
	{
		if (diamonds < 0)
		{
			return;
		}
		this.purchasePopup.transform.Find("Dialog/Buy Button/Cost").GetComponent<Text>().text = diamonds.ToString();
		Button component = this.purchasePopup.transform.Find("Dialog/Buy Button").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate()
		{
			Utilities.ConfirmPurchase(diamonds, delegate
			{
				OnSuccess();
				this.purchasePopup.gameObject.SetActive(false);
			});
		});
		this.purchasePopup.SetActive(true);
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00034CC0 File Offset: 0x00032EC0
	public void Fullscreen()
	{
		if (!Cellphone.HasPinup(this.CurrentGirl.Id))
		{
			return;
		}
		base.StartCoroutine(this.LoadFullscreen());
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00034CE8 File Offset: 0x00032EE8
	private IEnumerator LoadFullscreen()
	{
		yield return this.LoadAssetBundle();
		string correctCase = this.CurrentGirl.AssetBundle[0].ToString().ToUpperInvariant() + this.CurrentGirl.AssetBundle.Substring(1);
		if (this.CurrentGirl.Id == 9 && this.HoneyIsOlder())
		{
			correctCase += "1";
		}
		Sprite sprite = this.currentBundle.LoadAsset<Sprite>(correctCase);
		this.fullscreenImage.sprite = sprite;
		this.fullscreenImage.gameObject.SetActive(true);
		base.transform.Find("Phone UI/Name/Text").GetComponent<Text>().text = Translations.GetCellphoneName(this.CurrentGirl);
		base.transform.Find("Phone UI/Name").gameObject.SetActive(true);
		this.galleryIcon.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00034D04 File Offset: 0x00032F04
	private void CheckDisabledMessages()
	{
		try
		{
			RectTransform component = this.contentPanel.GetComponent<RectTransform>();
			float num = component.localPosition.y + component.sizeDelta.y;
			for (int i = 0; i < this.invisibleText.Count; i++)
			{
				Color color = this.invisibleText[i].color;
				this.invisibleText[i].color = new Color(color.r, color.g, color.b, 1f);
			}
			this.invisibleText.Clear();
			for (int j = 0; j < this.disabledTransforms.Count; j++)
			{
				RectTransform rectTransform = this.disabledTransforms[j];
				if (rectTransform == null || false)
				{
					if (rectTransform != null)
					{
						rectTransform.transform.Find("Background/Text").gameObject.SetActive(true);
					}
					this.disabledTransforms.RemoveAt(j--);
				}
				else if (rectTransform.anchoredPosition.y != 0f)
				{
					float num2 = rectTransform.anchoredPosition.y + num;
					float num3 = rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y + num;
					if (num2 > -925f && num3 < 650f)
					{
						rectTransform.transform.Find("Background/Text").gameObject.SetActive(true);
						Text component2 = rectTransform.transform.Find("Background/Text").GetComponent<Text>();
						Color color2 = component2.color;
						component2.color = new Color(color2.r, color2.g, color2.b, 0f);
						this.invisibleText.Add(component2);
						this.disabledTransforms.RemoveAt(j--);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Cellphone.CheckDisabledMessages " + ex.Message + " " + ((this.CurrentGirl != null) ? this.CurrentGirl.Name : "null"));
		}
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00034F80 File Offset: 0x00033180
	private void Update()
	{
		if (this.message != null && this.messenger.activeSelf)
		{
			if (this.message.NewConversation && !Cellphone.HasConversation(this.CurrentGirl.Id, this.message.ConversationId))
			{
				if (this.CurrentGirl.DatePref.GetLong(0L) != 9223372036854775807L)
				{
					this.CurrentGirl.DatePref.SetLong(long.MaxValue);
					GameState.CurrentState.QueueSave();
				}
				this.replyProgressContainer.SetActive(false);
				this.purchaseButton.gameObject.SetActive(false);
				this.purchasePopup.gameObject.SetActive(false);
				this.AddVotePrefab();
				this.message = null;
				return;
			}
			if (this.message.IsPhoneCall(this.CurrentGirl) && this.message.NextMessage != null)
			{
				if (!this.inCall)
				{
					base.transform.Find("Phone UI/Messenger/Call").gameObject.SetActive(true);
					base.transform.Find("Phone UI/Messenger/Call/Name").GetComponent<Text>().text = Translations.GetCellphoneName(this.CurrentGirl);
					base.transform.Find("Phone UI/Messenger/Call/Counter").GetComponent<Text>().text = "0:00";
					this.callTime = 0f;
				}
				else
				{
					this.callTime += Time.deltaTime;
					string text = string.Format("{0}:{1}", ((int)(this.callTime / 60f)).ToString(), ((int)this.callTime % 60).ToString("D2"));
					base.transform.Find("Phone UI/Messenger/Call/Counter").GetComponent<Text>().text = text;
				}
			}
			else if (this.inCall && this.message.NextMessage != null)
			{
				base.transform.Find("Phone UI/Messenger/Call").gameObject.SetActive(false);
			}
			this.inCall = this.message.IsPhoneCall(this.CurrentGirl);
			if (this.currentTime >= 0.0 && !(this.message is PhoneModel.PhoneResponse))
			{
				this.currentTime = Math.Max(0.0, this.currentTime - (double)Time.deltaTime);
				if (this.currentTime > 0.0 && this.currentTime < 4.0 && !this.pendingMessage && !(this.message is PhoneModel.PhoneImage) && !this.message.IsPhoneCall(this.CurrentGirl))
				{
					this.AddPendingMessage();
					this.pendingMessage = true;
				}
				if (this.currentTime == 0.0)
				{
					this.pendingMessage = false;
					this.replyProgressContainer.SetActive(false);
					this.purchaseButton.gameObject.SetActive(false);
					this.purchasePopup.gameObject.SetActive(false);
					this.FindRelativePositionInPinups();
					bool flag = this.message.NextMessage == null;
					if (this.message is PhoneModel.PhoneImage)
					{
						this.currentTime = -1.0;
						base.StartCoroutine(this.AddImage((PhoneModel.PhoneImage)this.message, true));
						GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneBubble);
					}
					else
					{
						this.YieldMessage(this.message);
						if (!this.message.IsPhoneCall(this.CurrentGirl))
						{
							GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.PhoneBubble);
						}
						this.StoreState(flag || (this.message.NewConversation && !Cellphone.HasConversation(this.CurrentGirl.Id, this.message.ConversationId)));
					}
					if (flag)
					{
						this.AddResetPrefab();
						this.AddVotePrefab();
						this.message = null;
						if (this.inCall)
						{
							base.transform.Find("Phone UI/Messenger/Call").gameObject.SetActive(false);
						}
						this.inCall = false;
					}
					this.replyProgress.fillAmount = 1f;
					this.replyTime.text = string.Empty;
					if (this.nextPinupPosition == -1)
					{
						this.pinupProgressContainer.SetActive(false);
						this.replyPinupIcon.gameObject.SetActive(false);
					}
					else
					{
						this.pinupProgressContainer.SetActive(true);
						this.replyPinupIcon.gameObject.SetActive(true);
						int num = this.replyPosition - this.lastPinupPosition;
						int num2 = this.nextPinupPosition - this.lastPinupPosition;
						if (num2 <= 0)
						{
							num2 = 1;
						}
						this.pinupPosition.text = string.Format("{0}/{1}", num.ToString(), num2.ToString());
						this.pinupProgress.fillAmount = (float)num / (float)num2;
					}
				}
				else if (this.currentTime > 0.0)
				{
					float fillAmount = (float)(1.0 - this.currentTime / (double)this.message.Time);
					this.replyProgress.fillAmount = fillAmount;
					this.replyTime.text = "Next message:\n" + Utilities.CreateCountdown(TimeSpan.FromSeconds(this.currentTime));
					if (!this.replyProgressContainer.gameObject.activeSelf)
					{
						this.replyProgressContainer.gameObject.SetActive(true);
						this.purchaseButton.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00035524 File Offset: 0x00033724
	private void OnApplicationPause(bool pause)
	{
		if (this.CurrentGirl != null && this.message != null)
		{
			if (!pause && this.currentTime > 0.0)
			{
				long num = this.CurrentGirl.DatePref.GetLong(0L);
				if (num == 9223372036854775807L)
				{
					num = DateTime.UtcNow.ToBinary();
				}
				TimeSpan timeSpan = DateTime.UtcNow - DateTime.FromBinary(this.CurrentGirl.DatePref.GetLong(0L));
				this.currentTime = Math.Max(0.01, this.currentTime - timeSpan.TotalSeconds);
			}
			else
			{
				this.StoreState(false);
			}
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x000355E4 File Offset: 0x000337E4
	public void Debug_SkipMessage()
	{
		if (this.currentTime > 0.0)
		{
			this.currentTime = 0.0;
		}
	}

	// Token: 0x04000613 RID: 1555
	private const int Eof = -4;

	// Token: 0x04000614 RID: 1556
	private int state;

	// Token: 0x04000615 RID: 1557
	public GameObject GirlMessage;

	// Token: 0x04000616 RID: 1558
	public GameObject ReplyMessage;

	// Token: 0x04000617 RID: 1559
	public GameObject PendingMessage;

	// Token: 0x04000618 RID: 1560
	public GameObject ImageMessage;

	// Token: 0x04000619 RID: 1561
	public GameObject PlayerImageMessage;

	// Token: 0x0400061A RID: 1562
	public GameObject ConversationDivider;

	// Token: 0x0400061B RID: 1563
	public GameObject VotePrefab;

	// Token: 0x0400061C RID: 1564
	public GameObject ResetPrefab;

	// Token: 0x0400061D RID: 1565
	public GameObject LTEPrefab;

	// Token: 0x0400061E RID: 1566
	private Transform contentPanel;

	// Token: 0x0400061F RID: 1567
	private Transform contactsPanel;

	// Token: 0x04000620 RID: 1568
	public GameObject ContactPrefab;

	// Token: 0x04000621 RID: 1569
	private Text replyTime;

	// Token: 0x04000622 RID: 1570
	private Text pinupPosition;

	// Token: 0x04000623 RID: 1571
	private Image replyProgress;

	// Token: 0x04000624 RID: 1572
	private Image pinupProgress;

	// Token: 0x04000625 RID: 1573
	private Image replyPinupIcon;

	// Token: 0x04000626 RID: 1574
	private GameObject pinupProgressContainer;

	// Token: 0x04000627 RID: 1575
	private GameObject replyProgressContainer;

	// Token: 0x04000628 RID: 1576
	private Button purchaseButton;

	// Token: 0x04000629 RID: 1577
	private GameObject messenger;

	// Token: 0x0400062A RID: 1578
	private GameObject contacts;

	// Token: 0x0400062B RID: 1579
	private GameObject fullImage;

	// Token: 0x0400062C RID: 1580
	private GameObject purchasePopup;

	// Token: 0x0400062D RID: 1581
	private GameObject galleryIcon;

	// Token: 0x0400062E RID: 1582
	private bool disposed;

	// Token: 0x0400062F RID: 1583
	private AssetBundle commonBundle;

	// Token: 0x04000630 RID: 1584
	public Sprite UnknownGirl;

	// Token: 0x04000631 RID: 1585
	private Button[] contactButtons;

	// Token: 0x04000632 RID: 1586
	private Dictionary<short, Sprite> contactIcons = new Dictionary<short, Sprite>();

	// Token: 0x04000633 RID: 1587
	private Cellphone.NotificationType currentNotification;

	// Token: 0x04000634 RID: 1588
	public Sprite CellphoneNotificationSprite;

	// Token: 0x04000635 RID: 1589
	public Sprite CellphoneSprite;

	// Token: 0x04000636 RID: 1590
	private List<short> availableFlings = new List<short>();

	// Token: 0x04000637 RID: 1591
	private Transform gallery;

	// Token: 0x04000638 RID: 1592
	public GameObject GalleryPrefab;

	// Token: 0x04000639 RID: 1593
	private List<int> choices = new List<int>();

	// Token: 0x0400063A RID: 1594
	private Image fullscreenImage;

	// Token: 0x0400063B RID: 1595
	private PhoneModel.PhoneMessage message;

	// Token: 0x0400063C RID: 1596
	private PhoneModel CurrentGirl;

	// Token: 0x0400063D RID: 1597
	private PhoneModel CurrentGirlCached;

	// Token: 0x0400063E RID: 1598
	private TextGenerator textGen;

	// Token: 0x0400063F RID: 1599
	private AssetBundle currentBundle;

	// Token: 0x04000640 RID: 1600
	private AssetBundle currentBundleNsfw;

	// Token: 0x04000641 RID: 1601
	private int imageIndex;

	// Token: 0x04000642 RID: 1602
	private double currentTime;

	// Token: 0x04000643 RID: 1603
	private int replyPosition;

	// Token: 0x04000644 RID: 1604
	private int nextPinupPosition;

	// Token: 0x04000645 RID: 1605
	private int lastPinupPosition;

	// Token: 0x04000646 RID: 1606
	private bool pendingMessage;

	// Token: 0x04000647 RID: 1607
	private List<RectTransform> disabledTransforms = new List<RectTransform>();

	// Token: 0x04000648 RID: 1608
	private List<Text> invisibleText = new List<Text>();

	// Token: 0x04000649 RID: 1609
	private bool inCall;

	// Token: 0x0400064A RID: 1610
	private float callTime;

	// Token: 0x02000108 RID: 264
	[Flags]
	public enum NotificationType
	{
		// Token: 0x0400064C RID: 1612
		None = 0,
		// Token: 0x0400064D RID: 1613
		NewMessage = 1,
		// Token: 0x0400064E RID: 1614
		PendingReply = 2
	}
}
