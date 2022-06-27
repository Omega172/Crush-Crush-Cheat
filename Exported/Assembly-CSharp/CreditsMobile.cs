using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000109 RID: 265
public class CreditsMobile : MonoBehaviour
{
	// Token: 0x06000657 RID: 1623 RVA: 0x00035728 File Offset: 0x00033928
	private void OnEnable()
	{
		this._currentTime = 0f;
		this._fadeTime = 0f;
		this._isReady = false;
		this._portraitTemplate = base.transform.Find("Content/Sprite Template").gameObject;
		this._groupTemplate = base.transform.Find("Content/Group Template").gameObject;
		this._portraitTemplate.SetActive(false);
		this._groupTemplate.SetActive(false);
		this._contentTransform = base.transform.Find("Content");
		if (this._content != null)
		{
			this._content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		}
		if (this._thanksText != null)
		{
			this._thanksText.color = Color.white;
		}
		GameState.Voiceover.AddPlaybackBlocker(base.gameObject);
		GameState.CurrentState.StartCoroutine(this.CreateCredits());
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00035828 File Offset: 0x00033A28
	public void Init(bool prestigeOnSkip)
	{
		this._prestigeOnSkip = prestigeOnSkip;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00035840 File Offset: 0x00033A40
	private void OnDisable()
	{
		this._portraitTemplate.SetActive(true);
		this._groupTemplate.SetActive(true);
		GameState.Voiceover.RemovePlaybackBlocker(base.gameObject, true);
		this.DisposeCredits();
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0003587C File Offset: 0x00033A7C
	private void DisposeCredits()
	{
		if (this._creditsObjects.Count == 0)
		{
			return;
		}
		for (int i = this._creditsObjects.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this._creditsObjects[i].gameObject);
		}
		this._creditsObjects.Clear();
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x000358DC File Offset: 0x00033ADC
	private Sprite GetSpriteFromName(string spriteName)
	{
		for (int i = 0; i < this.NameToSprite.Length; i++)
		{
			if (spriteName == this.NameToSprite[i].Key)
			{
				return this.NameToSprite[i].Sprite;
			}
		}
		return null;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0003592C File Offset: 0x00033B2C
	private IEnumerator CreateCredits()
	{
		List<CreditsModel> credits = Universe.Credits;
		List<RectTransform> groupList = new List<RectTransform>();
		this.DisposeCredits();
		foreach (CreditsModel credit in credits)
		{
			GameObject go = this.CreateCreditsObject(credit);
			go.name = credit.Category;
			go.SetActive(true);
			if (!credit.IsPortraitType)
			{
				groupList.Add(go.GetComponent<RectTransform>());
			}
			this._creditsObjects.Add(go.transform);
		}
		yield return null;
		this.UpdateGroupItemsSize(groupList);
		yield return null;
		foreach (Transform item in this._creditsObjects)
		{
			item.SetParent(this._contentTransform, false);
		}
		RectTransform spacingRect = new GameObject("Spacing").AddComponent<RectTransform>();
		spacingRect.sizeDelta = new Vector2(1f, this.Spacing);
		spacingRect.SetParent(this._contentTransform, false);
		this._creditsObjects.Add(spacingRect);
		base.transform.Find("Content/Thanks Container").SetAsLastSibling();
		yield return null;
		this._isReady = true;
		yield break;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00035948 File Offset: 0x00033B48
	private GameObject CreateCreditsObject(CreditsModel credit)
	{
		GameObject gameObject;
		if (credit.IsPortraitType)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this._portraitTemplate);
			gameObject.GetComponentInChildren<Image>().sprite = this.GetSpriteFromName(credit.Sprite);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this._groupTemplate);
		}
		this.FormatTextComponent(gameObject.GetComponentInChildren<Text>(), credit);
		return gameObject;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000359A4 File Offset: 0x00033BA4
	private void FormatTextComponent(Text textComponent, CreditsModel credit)
	{
		string text = string.Empty;
		List<string> names = credit.Names;
		for (int i = 0; i < names.Count; i++)
		{
			text += ((i != names.Count - 1) ? (names[i] + Environment.NewLine) : names[i]);
		}
		string text2 = string.Format(this.TextWithSize, this.TitleSize.ToString(), credit.Category);
		if (!credit.IsPortraitType)
		{
			text2 += Environment.NewLine;
		}
		text2 = text2 + Environment.NewLine + text;
		textComponent.fontSize = this.TextSize;
		textComponent.text = text2;
		if (this.textGen == null)
		{
			this.textGen = new TextGenerator();
		}
		TextGenerationSettings generationSettings = textComponent.GetGenerationSettings(new Vector2(600f, 400f));
		generationSettings.scaleFactor = 1f;
		float num = this.textGen.GetPreferredHeight(textComponent.text, generationSettings) + 10f;
		textComponent.rectTransform.sizeDelta = new Vector2(textComponent.rectTransform.sizeDelta.x, num);
		if (!credit.IsPortraitType)
		{
			textComponent.transform.parent.GetComponent<LayoutElement>().preferredHeight = num;
		}
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00035AF8 File Offset: 0x00033CF8
	private void UpdateGroupItemsSize(List<RectTransform> groupList)
	{
		foreach (RectTransform rectTransform in groupList)
		{
			RectTransform component = rectTransform.transform.Find("Text").GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, component.sizeDelta.y);
		}
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00035B90 File Offset: 0x00033D90
	private void FixedUpdate()
	{
		if (!this._isReady)
		{
			return;
		}
		if (this._content == null)
		{
			this._content = base.transform.Find("Content").GetComponent<RectTransform>();
		}
		if (this._thanksText == null)
		{
			this._thanksText = base.transform.Find("Content/Thanks Container/Text").GetComponent<Text>();
			this._thanksYOffset = this._thanksText.rectTransform.sizeDelta.y / 2f;
		}
		if (this._thanksText.transform.position.y < this._thanksYOffset)
		{
			if (BlayFapIntegration.IsTestDevice)
			{
				this._multiplier = ((!Input.anyKey) ? 1 : this._maxMultiplier);
			}
			this._currentTime += Time.fixedDeltaTime * (float)this._multiplier;
			this._content.anchoredPosition = new Vector2(0f, this._currentTime * this._speed);
		}
		else
		{
			this._fadeTime += Time.fixedDeltaTime;
			float num = Mathf.Max(0f, Mathf.Min(1f, this._fadeTime - 3f));
			if (num >= 1f)
			{
				this.Skip();
				Achievements.ForceAchievement(474);
			}
			else if (num > 0f)
			{
				this._thanksText.color = new Color(1f, 1f, 1f, 1f - num);
			}
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00035D34 File Offset: 0x00033F34
	public void Skip()
	{
		base.gameObject.SetActive(false);
		if (this._prestigeOnSkip)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Unlock, "SacrificeSelf");
			Achievements.ForceAchievement(472);
			Girl.FindGirl(Balance.GirlName.QPiddy).Love = 9;
			Achievements.TriggerLoveAchievement(Balance.GirlName.QPiddy);
			GameState.CurrentState.Prestige();
		}
	}

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private CreditsMobile.CreditsSprite[] NameToSprite;

	// Token: 0x04000650 RID: 1616
	[SerializeField]
	private int TitleSize = 36;

	// Token: 0x04000651 RID: 1617
	[SerializeField]
	private int TextSize = 24;

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private float Spacing = 500f;

	// Token: 0x04000653 RID: 1619
	private float _currentTime;

	// Token: 0x04000654 RID: 1620
	private float _fadeTime;

	// Token: 0x04000655 RID: 1621
	private RectTransform _content;

	// Token: 0x04000656 RID: 1622
	private Text _thanksText;

	// Token: 0x04000657 RID: 1623
	private GameObject _portraitTemplate;

	// Token: 0x04000658 RID: 1624
	private GameObject _groupTemplate;

	// Token: 0x04000659 RID: 1625
	private Transform _contentTransform;

	// Token: 0x0400065A RID: 1626
	private float _speed = 100f;

	// Token: 0x0400065B RID: 1627
	private int _multiplier = 1;

	// Token: 0x0400065C RID: 1628
	private float _thanksYOffset;

	// Token: 0x0400065D RID: 1629
	private readonly int _maxMultiplier = 8;

	// Token: 0x0400065E RID: 1630
	private bool _isReady;

	// Token: 0x0400065F RID: 1631
	private bool _prestigeOnSkip;

	// Token: 0x04000660 RID: 1632
	private readonly string TextWithSize = "<size={0}>{1}</size>";

	// Token: 0x04000661 RID: 1633
	private List<Transform> _creditsObjects = new List<Transform>();

	// Token: 0x04000662 RID: 1634
	private TextGenerator textGen;

	// Token: 0x0200010A RID: 266
	[Serializable]
	public class CreditsSprite
	{
		// Token: 0x04000663 RID: 1635
		public string Key;

		// Token: 0x04000664 RID: 1636
		public Sprite Sprite;
	}
}
