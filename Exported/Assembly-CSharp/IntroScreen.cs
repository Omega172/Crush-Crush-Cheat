using System;
using BlayFap;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CC RID: 204
public class IntroScreen : MonoBehaviour
{
	// Token: 0x06000471 RID: 1137 RVA: 0x00021DB4 File Offset: 0x0001FFB4
	private void Update()
	{
		if (this.introState == 0)
		{
			if (this.transition < 2f)
			{
				this.transition += Time.deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, this.transition / 2f);
				this.Faery.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(44f + num * 226f, 54f + num * 326f);
				this.Faery.GetComponent<Image>().rectTransform.localPosition = new Vector2(282f + num * 48f, 51f + num * -188f);
			}
			else if (!this.Button.gameObject.activeSelf)
			{
				this.Button.gameObject.SetActive(true);
				this.Faery.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(270f, 380f);
				this.Faery.GetComponent<Image>().rectTransform.localPosition = new Vector2(330f, -137f);
			}
		}
		if (this.introState >= 2 && this.introState < 10 && this.transition < 0.5f)
		{
			this.transition = Mathf.Min(0.5f, this.transition + Time.deltaTime);
			float num2 = Utilities.Easing.CubicEaseInOut(this.transition * 2f, 0f, 1f, 1f);
			this.BottomUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -110f + 110f * num2);
		}
		if (this.introState == 10 && this.transition < 0f)
		{
			this.transition += Time.deltaTime;
		}
		else if (this.introState == 10)
		{
			this.SpeechBox.SetActive(true);
			this.Background.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
		}
		if (this.introState == 100 || this.introState == 120)
		{
			if (this.transition > 1f)
			{
				if (this.CurrentSystem == null || this.CurrentSystem == this.DiamondParticles2)
				{
					this.CurrentSystem = this.DiamondParticles1;
					this.CurrentSystem.transform.localPosition = new Vector3((float)UnityEngine.Random.Range(0, 400), (float)UnityEngine.Random.Range(-200, 200), 0f);
				}
				else
				{
					this.CurrentSystem = this.DiamondParticles2;
					this.CurrentSystem.transform.localPosition = new Vector3((float)UnityEngine.Random.Range(0, 400), (float)UnityEngine.Random.Range(-200, 200), 0f);
				}
				this.CurrentSystem.Play();
				GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Unlock00);
				this.transition = 0f;
			}
			this.transition += Time.deltaTime;
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x00022108 File Offset: 0x00020308
	public void OnClose()
	{
		base.gameObject.SetActive(false);
		this.BottomUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		GameState.Voiceover.Stop();
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0002214C File Offset: 0x0002034C
	private void SetInteractable(Button button, bool interactable)
	{
		button.interactable = interactable;
		if (button.GetComponent<Image>() != null)
		{
			button.GetComponent<Image>().color = (interactable ? Color.white : new Color(1f, 1f, 1f, 0.2f));
		}
		if (button.transform.Find("Icon") != null)
		{
			button.transform.Find("Icon").GetComponent<Image>().color = (interactable ? Color.white : new Color(1f, 1f, 1f, 0.2f));
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00022204 File Offset: 0x00020404
	private void SendKongregateAnalytic(int step, string type, bool isFinal)
	{
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00022208 File Offset: 0x00020408
	public void OnClickGirlButton()
	{
		GameState.CurrentState.GetComponent<PanelScript>().ClickGirl();
		this.CassieIntro();
		GameState.GetGirlScreen().SetGirl(Girl.FindGirl(Balance.GirlName.Cassie));
		base.transform.Find("Intro Bottom UI").gameObject.SetActive(false);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00022258 File Offset: 0x00020458
	public void NextButton()
	{
		this.introState++;
		GameState.Voiceover.Stop();
		if (this.Faery.GetComponent<Image>().sprite == this.Faery1)
		{
			this.Faery.GetComponent<Image>().sprite = this.Faery2;
		}
		else if (this.Faery.GetComponent<Image>().sprite == this.Faery2)
		{
			this.Faery.GetComponent<Image>().sprite = this.Faery3;
		}
		else
		{
			this.Faery.GetComponent<Image>().sprite = this.Faery1;
		}
		if (BlayFapIntegration.Tracking != null && BlayFapIntegration.Tracking.Contains("Tutorial"))
		{
			BlayFapClient.Instance.AddApiCount("Tutorial" + this.introState.ToString());
		}
		if (this.introState == 1)
		{
			this.SendKongregateAnalytic(10, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_2", "That girl's name was Cassie, and I'm going to help you get a date with her. You've already broken the ice (and possibly her ribs), so the next part should be easy!");
			this.Faery.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(270f, 380f);
			this.Faery.GetComponent<Image>().rectTransform.localPosition = new Vector2(330f, -137f);
		}
		else if (this.introState == 2)
		{
			this.SendKongregateAnalytic(20, "tutorial", false);
			this.transition = 0f;
			IntroProvider component = GameState.CurrentState.transform.Find("Popups/Girl Introduction").GetComponent<IntroProvider>();
			if (component.gameObject.activeInHierarchy)
			{
				component.Skip();
			}
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_4", "First things first. Click the 'GIRLS' icon below to check on any girls that you have met.");
			this.BottomUI.transform.Find("GirlsBTN/Notification").gameObject.SetActive(true);
			this.SetInteractable(this.BottomUI.transform.Find("GirlsBTN").GetComponent<Button>(), true);
			this.SetInteractable(this.BottomUI.transform.Find("JobsBTN").GetComponent<Button>(), false);
			this.ButtonText.text = Translations.GetTranslation("everything_else_28_7", "OK");
		}
		else if (this.introState == 3)
		{
			this.SendKongregateAnalytic(30, "tutorial", false);
			this.OnClose();
		}
		else if (this.introState == 11)
		{
			this.SendKongregateAnalytic(110, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_5", "Excellent! It looks like she doesn't have health insurance! Offer to pay her bills to make a good impression.");
			this.ButtonText.text = Translations.GetTranslation("everything_else_28_7", "How?");
		}
		else if (this.introState == 12)
		{
			this.SendKongregateAnalytic(120, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_6", "But first you'll have to find a job to make some money!");
			this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
			this.SetInteractable(this.BottomUI.transform.Find("GirlsBTN").GetComponent<Button>(), false);
			this.SetInteractable(this.BottomUI.transform.Find("JobsBTN").GetComponent<Button>(), true);
			this.BottomUI.transform.Find("GirlsBTN/Notification").gameObject.SetActive(false);
			this.BottomUI.transform.Find("JobsBTN/Notification").gameObject.SetActive(true);
			IntroScreen.TutorialState = IntroScreen.State.IntroduceJobs;
		}
		else if (this.introState == 13)
		{
			this.SendKongregateAnalytic(130, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_8", "Try clicking the 'JOBS' icon below to find some work!");
			this.ButtonText.text = Translations.GetTranslation("everything_else_28_7", "OK");
		}
		else if (this.introState == 14)
		{
			this.SendKongregateAnalytic(140, "tutorial", false);
			this.OnClose();
		}
		else if (this.introState == 21)
		{
			this.SendKongregateAnalytic(210, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_9", "Here you can find work, make money and advance your career!");
		}
		else if (this.introState == 22)
		{
			this.SendKongregateAnalytic(220, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_10", "You have no credentials or skills. So it looks like you're going to be flipping burgers...");
		}
		else if (this.introState == 23)
		{
			this.SendKongregateAnalytic(230, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_11", "Increase your stats later to qualify for better jobs. But for now, why not give it a try?");
			this.ButtonText.text = Translations.GetTranslation("everything_else_28_7", "OK");
		}
		else if (this.introState == 24)
		{
			this.SendKongregateAnalytic(240, "tutorial", false);
			this.OnClose();
			IntroScreen.TutorialState = IntroScreen.State.JobsActive;
		}
		else if (this.introState == 31)
		{
			this.SendKongregateAnalytic(310, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_12", "When that bar fills up, you get paid! Huzzah!");
		}
		else if (this.introState == 32)
		{
			this.SendKongregateAnalytic(320, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_13", "It's usually best to have at least one job active at all times.");
		}
		else if (this.introState == 33)
		{
			this.SendKongregateAnalytic(330, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_14", "Work hard for promotions to up your salary. If you stick with one company long enough you could even become CEO!");
		}
		else if (this.introState == 34)
		{
			this.SendKongregateAnalytic(340, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_17", "Now that you have some income, you should check in on Cassie.");
			this.ButtonText.text = Translations.GetTranslation("everything_else_28_7", "OK");
			this.SetInteractable(this.BottomUI.transform.Find("GirlsBTN").GetComponent<Button>(), true);
			this.BottomUI.transform.Find("GirlsBTN/Notification").gameObject.SetActive(true);
			this.BottomUI.transform.Find("JobsBTN/Notification").gameObject.SetActive(false);
		}
		else if (this.introState == 35)
		{
			this.SendKongregateAnalytic(350, "tutorial", false);
			this.OnClose();
			IntroScreen.TutorialState = IntroScreen.State.WaitFor250;
		}
		else if (this.introState == 41)
		{
			this.SendKongregateAnalytic(410, "tutorial", false);
			GameObject.Find("Girls").transform.Find("Girl Information/Requirements").gameObject.SetActive(true);
			GameObject.Find("Girls").transform.Find("Girl Information/Interaction Buttons").gameObject.SetActive(true);
			GameObject.Find("Girls").GetComponent<Girls>().SetGirl();
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_18", "The 'GIRLS' screen now contains information on what requirements Cassie has.");
		}
		else if (this.introState == 42)
		{
			this.SendKongregateAnalytic(420, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_19", "Meeting these requirements will allow you to reach the next stage of your relationship! They could be cash, skills or other stuff!");
		}
		else if (this.introState == 43)
		{
			this.SendKongregateAnalytic(430, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_21", "Talk to girls, buy them gifts and take them on dates to increase their 'affection' meter.");
		}
		else if (this.introState == 44)
		{
			this.SendKongregateAnalytic(440, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_22", "Remember - you only have so much free time to fit everything in. Unlock achievements to increase the number of Time Blocks you have to use.");
		}
		else if (this.introState == 45)
		{
			this.SendKongregateAnalytic(450, "tutorial", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_23", "That's it! I've told you everything I know!  The world is yours! Just please try not to hurt *every* girl that you meet, hehe!");
		}
		else if (this.introState == 46)
		{
			this.SendKongregateAnalytic(470, "tutorial", true);
			this.OnClose();
			this.SetInteractable(this.BottomUI.transform.Find("SkillsBTN").GetComponent<Button>(), true);
			this.BottomUI.transform.Find("AchievementsBTN").GetComponent<Button>().interactable = true;
			this.BottomUI.transform.Find("MoreBTN").GetComponent<Button>().interactable = true;
			this.BottomUI.transform.Find("AchievementsBTN").GetComponent<Image>().color = Color.white;
			this.BottomUI.transform.Find("MoreBTN").GetComponent<Image>().color = Color.white;
			this.SetInteractable(this.BottomUI.transform.Find("StoreBTN").GetComponent<Button>(), true);
			this.SetInteractable(this.BottomUI.transform.Find("SkillsBTN").GetComponent<Button>(), true);
			this.SetInteractable(this.BottomUI.transform.Find("StoreBTN").GetComponent<Button>(), true);
			IntroScreen.TutorialState = IntroScreen.State.AllActive;
			Girl.InitDeeplinking();
		}
		else if (this.introState == 51)
		{
			this.SendKongregateAnalytic(510, "hobby", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_26", "Try clicking on the hobbies button to pick up your first hobby!");
		}
		else if (this.introState == 52)
		{
			this.SendKongregateAnalytic(520, "hobby", true);
			this.OnClose();
			IntroScreen.TutorialState = IntroScreen.State.HobbiesActive;
			this.SetInteractable(this.BottomUI.transform.Find("HobbyBTN").GetComponent<Button>(), true);
			if (GameState.GetGirlScreen().transform.Find("Opt In Blocker") != null)
			{
				GameState.GetGirlScreen().transform.Find("Opt In Blocker").gameObject.SetActive(true);
			}
			if (GameState.CurrentState.transform.Find("Top UI/Task Blocker") != null)
			{
				GameState.CurrentState.transform.Find("Top UI/Task Blocker").gameObject.SetActive(true);
			}
			if (GameState.CurrentState.transform.Find("Top UI/Starter Pack") != null)
			{
				GameState.CurrentState.transform.Find("Top UI/Starter Pack").gameObject.SetActive(true);
			}
		}
		else if (this.introState == 61)
		{
			this.SendKongregateAnalytic(610, "affection", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_27", "Try talking, gifting or dating to build up their affection.");
		}
		else if (this.introState == 62)
		{
			this.SendKongregateAnalytic(620, "affection", false);
			if (Translations.CurrentLanguage.Value == 0 && Girls.CurrentGirl != null)
			{
				this.TextBox.text = string.Format("You can also try tapping {0} to see what happens!", Girls.CurrentGirl.GirlName.ToFriendlyString());
				this.GetIntroDialogueTranslationAndPlay("qpiddy_28", "You can also try tapping to see what happens!");
			}
			else
			{
				this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_28", "You can also try tapping to see what happens!");
			}
		}
		else if (this.introState == 63)
		{
			this.SendKongregateAnalytic(630, "affection", true);
			this.OnClose();
		}
		else if (this.introState == 71)
		{
			this.SendKongregateAnalytic(710, "timeblock", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_29", "I've grouped up your time blocks into Free Time, Jobs, Hobbies and Dates. That's WAY easier to understand. Alright - GAME ON!");
		}
		else if (this.introState == 72)
		{
			this.SendKongregateAnalytic(720, "timeblock", true);
			this.OnClose();
		}
		else if (this.introState == 81)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_30", "You can now buy a Promise Ring for your Lover, to confess your eternal affection and loyalty!");
		}
		else if (this.introState == 82)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_31", "The Promise Ring will unlock Endless Love mode - allowing you to gain unlimited levels and prestige.  Wow!");
		}
		else if (this.introState == 83)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_32", "And just so you know - if you don't want to limit your options, I would be happy to clone you for each additional Promise Ring you buy.");
		}
		else if (this.introState == 84)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_33", "That's the power of love!  And diamonds.  I'll need more diamonds for each additional ring.  Fairy magic is so random!");
		}
		else if (this.introState == 85)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_34", "So make your choice!  Good luck!");
		}
		else if (this.introState == 86)
		{
			this.OnClose();
		}
		else if (this.introState == 91)
		{
			this.SendKongregateAnalytic(910, "prestige", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_35", "To the Stats tab!");
			GameState.CurrentState.GetComponent<PanelScript>().ClickSkill();
			this.Faery.GetComponent<Image>().transform.localPosition = new Vector3(-330f, -137f, 0f);
			this.Faery.GetComponent<Image>().transform.localScale = new Vector3(-1f, 1f, 1f);
			base.transform.Find("Prestige Button").gameObject.SetActive(true);
			GameState.CurrentState.transform.Find("Popups/Memory Album").gameObject.SetActive(false);
		}
		else if (this.introState == 92)
		{
			this.SendKongregateAnalytic(920, "prestige", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_37", "Down here is the \"Reset\" button. This scary thing will reset YOUR ENTIRE GAME. Sort of. Not really. It's actually really cool.");
		}
		else if (this.introState == 93)
		{
			this.SendKongregateAnalytic(930, "prestige", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_38", "Don't worry! Once you do, it'll add this pretty boost to everything in the game! Right now that means it'll more than double your progress!");
		}
		else if (this.introState == 94)
		{
			this.SendKongregateAnalytic(940, "prestige", false);
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_39", "It's up to you to determine when's the best time. But don't be afraid! Your diamonds, time blocks and achievements won't reset. Do it! DO IT!");
		}
		else if (this.introState == 95)
		{
			this.Faery.GetComponent<Image>().transform.localPosition = new Vector3(330f, -137f, 0f);
			this.Faery.GetComponent<Image>().transform.localScale = Vector3.one;
			base.transform.Find("Prestige Button").gameObject.SetActive(false);
			this.SendKongregateAnalytic(950, "prestige", true);
			this.OnClose();
		}
		else if (this.introState == 101 || this.introState == 121)
		{
			this.OnClose();
		}
		else if (this.introState == 111)
		{
			this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_40", "Once a girl reaches Lover level, you're able to buy her special outfits and play dress up (or down!). You'll also get a special pic as a momento.");
		}
		else if (this.introState == 112)
		{
			this.OnClose();
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00023234 File Offset: 0x00021434
	public void FirstIntro()
	{
		this.introState = 0;
		this.transition = 0f;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_1", "Hey there! Believe it or not, today's your lucky day! My name's Q-Piddy, and matchmaking is my specialty!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(0, "tutorial", false);
		this.Button.gameObject.SetActive(false);
		this.Faery.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(44f, 54f);
		this.Faery.GetComponent<Image>().rectTransform.localPosition = new Vector2(282f, 51f);
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.QPArrives);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00023310 File Offset: 0x00021510
	public void CassieIntro()
	{
		this.introState = 10;
		this.transition = 0f;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_3", "Ouch, you hurt her pretty badly!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(100, "tutorial", false);
		this.SpeechBox.SetActive(false);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		base.transform.Find("Speech Box/Next Button").gameObject.SetActive(true);
		this.Background.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x000233DC File Offset: 0x000215DC
	public void JobIntro()
	{
		this.introState = 20;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_7", "This is the jobs screen!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(200, "tutorial", false);
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0002345C File Offset: 0x0002165C
	public void JobFinished()
	{
		this.introState = 30;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_15", "Awesome!  You should be able to pay off that bill in no time!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(300, "tutorial", false);
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000234DC File Offset: 0x000216DC
	public void CassieFinished()
	{
		this.introState = 40;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_16", "Cassie is still in the hospital!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(400, "tutorial", false);
		this.BottomUI.transform.Find("GirlsBTN/Notification").gameObject.SetActive(false);
		this.BottomUI.transform.Find("JobsBTN/Notification").gameObject.SetActive(false);
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0002359C File Offset: 0x0002179C
	public void HobbyStart()
	{
		this.introState = 50;
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_25", "You can now do a new hobby!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(500, "hobby", false);
		this.BottomUI.transform.Find("HobbyBTN/Notification").gameObject.SetActive(true);
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0002363C File Offset: 0x0002183C
	public void AffectionStart()
	{
		if ((global::PlayerPrefs.GetInt("Tutorial", 1) & 32) == 32)
		{
			return;
		}
		this.introState = 60;
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_44", "You've met all requirements except for affection!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		this.SendKongregateAnalytic(600, "affection", false);
		global::PlayerPrefs.SetInt("Tutorial", global::PlayerPrefs.GetInt("Tutorial", 1) | 32);
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x000236EC File Offset: 0x000218EC
	public void TimeBlockStart()
	{
		if ((global::PlayerPrefs.GetInt("Tutorial", 1) & 2) == 2)
		{
			return;
		}
		this.introState = 70;
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		this.SendKongregateAnalytic(700, "timeblock", false);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_45", "Yo! You're STACKED with time blocks! Good job! Let's try to make things a bit easier to read!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		global::PlayerPrefs.SetInt("Tutorial", global::PlayerPrefs.GetInt("Tutorial", 1) | 2);
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00023798 File Offset: 0x00021998
	public void LoverStart()
	{
		if ((global::PlayerPrefs.GetInt("Tutorial", 1) & 4) == 4)
		{
			return;
		}
		this.introState = 80;
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_46", "You have reached the \"Lover\" relationship level!  You can now decide if you want to take things to the next level!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		global::PlayerPrefs.SetInt("Tutorial", global::PlayerPrefs.GetInt("Tutorial", 1) | 4);
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00023834 File Offset: 0x00021A34
	public void PrestigeStart()
	{
		if ((global::PlayerPrefs.GetInt("Tutorial", 1) & 8) == 8)
		{
			return;
		}
		this.introState = 90;
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		this.SendKongregateAnalytic(900, "prestige", false);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_47", "Hey hot stuff! Looks like you're starting to slow down a bit. Don't worry, I've got just the thing!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		global::PlayerPrefs.SetInt("Tutorial", global::PlayerPrefs.GetInt("Tutorial", 1) | 8);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000238E0 File Offset: 0x00021AE0
	public void FinalImageStart()
	{
		if ((global::PlayerPrefs.GetInt("Tutorial", 1) & 16) == 16)
		{
			return;
		}
		this.introState = 110;
		this.SpeechBox.SetActive(true);
		this.Faery.SetActive(true);
		base.gameObject.SetActive(true);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_48", "Hey sweet cheeks! Looks like someone's fallen head over heels for ya! I knew you could do it!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_42_9", "Next");
		global::PlayerPrefs.SetInt("Tutorial", global::PlayerPrefs.GetInt("Tutorial", 1) | 16);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0002397C File Offset: 0x00021B7C
	public void DiamondPurchase()
	{
		this.introState = 100;
		this.transition = 2f;
		this.SpeechBox.SetActive(true);
		base.gameObject.SetActive(true);
		this.Faery.gameObject.SetActive(true);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_49", "Oh wow!  Thank you so much for buying those; you're INCREDIBLE!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_10_0", "Close");
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000239FC File Offset: 0x00021BFC
	public void OfferwallCompletion()
	{
		this.introState = 120;
		this.transition = 2f;
		this.SpeechBox.SetActive(true);
		base.gameObject.SetActive(true);
		this.Faery.gameObject.SetActive(true);
		this.TextBox.text = this.GetIntroDialogueTranslationAndPlay("qpiddy_50", "Oh wow!  Thank you so much for completing that; you're INCREDIBLE!");
		this.ButtonText.text = Translations.GetTranslation("everything_else_10_0", "Close");
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00023A7C File Offset: 0x00021C7C
	private void OnDisable()
	{
		GameState.Voiceover.SetCurrentBundle(Voiceover.BundleType.Girl);
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00023A8C File Offset: 0x00021C8C
	private string GetIntroDialogueTranslationAndPlay(string id, string english)
	{
		GameState.Voiceover.SetCurrentBundle(Voiceover.BundleType.Tutorial);
		GameState.Voiceover.Play(Balance.GirlName.QPiddy, Voiceover.BundleType.Tutorial, id, 0.2f);
		return Translations.GetDialogueTranslation(id, english);
	}

	// Token: 0x0400047F RID: 1151
	private const float VoiceoverDelay = 0.2f;

	// Token: 0x04000480 RID: 1152
	public Text TextBox;

	// Token: 0x04000481 RID: 1153
	public Text ButtonText;

	// Token: 0x04000482 RID: 1154
	public Button Button;

	// Token: 0x04000483 RID: 1155
	public GameObject SpeechBox;

	// Token: 0x04000484 RID: 1156
	public GameObject FirstEvents;

	// Token: 0x04000485 RID: 1157
	public GameObject BottomUI;

	// Token: 0x04000486 RID: 1158
	public GameObject Faery;

	// Token: 0x04000487 RID: 1159
	public GameObject Background;

	// Token: 0x04000488 RID: 1160
	public Sprite Faery1;

	// Token: 0x04000489 RID: 1161
	public Sprite Faery2;

	// Token: 0x0400048A RID: 1162
	public Sprite Faery3;

	// Token: 0x0400048B RID: 1163
	public ParticleSystem DiamondParticles1;

	// Token: 0x0400048C RID: 1164
	public ParticleSystem DiamondParticles2;

	// Token: 0x0400048D RID: 1165
	private ParticleSystem CurrentSystem;

	// Token: 0x0400048E RID: 1166
	public static IntroScreen.State TutorialState;

	// Token: 0x0400048F RID: 1167
	private float transition;

	// Token: 0x04000490 RID: 1168
	private int introState;

	// Token: 0x020000CD RID: 205
	public enum State
	{
		// Token: 0x04000492 RID: 1170
		GirlsActive,
		// Token: 0x04000493 RID: 1171
		IntroduceJobs,
		// Token: 0x04000494 RID: 1172
		JobsActive,
		// Token: 0x04000495 RID: 1173
		WaitFor250,
		// Token: 0x04000496 RID: 1174
		SkillsActive = 5,
		// Token: 0x04000497 RID: 1175
		AllActive,
		// Token: 0x04000498 RID: 1176
		HobbiesActive
	}
}
