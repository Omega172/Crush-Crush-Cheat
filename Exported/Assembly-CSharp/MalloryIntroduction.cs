using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class MalloryIntroduction : GenericIntroduction
{
	// Token: 0x06000016 RID: 22 RVA: 0x00002FC8 File Offset: 0x000011C8
	public MalloryIntroduction(Girl girl)
	{
		List<GirlModel.IntroData> list = new List<GirlModel.IntroData>(girl.Data.IntroDataList.ToArray());
		this._choice1 = list[3];
		this._choice2 = list[4];
		list.RemoveRange(3, 2);
		list.Insert(3, new GirlModel.IntroData
		{
			DisplayOptions = true
		});
		this.Init(girl, list);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00003030 File Offset: 0x00001230
	public override void Update(IntroProvider provider)
	{
		base.Update(provider);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000303C File Offset: 0x0000123C
	public override void OnClick(IntroProvider provider)
	{
		if (this._state < this._cachedIntroDataList.Count && this._cachedIntroDataList[this._state].DisplayOptions)
		{
			provider.transform.Find("Options").gameObject.SetActive(true);
			provider.transform.Find("Intro Text Box").gameObject.SetActive(false);
			provider.transform.Find("Accept Button").gameObject.SetActive(false);
			string str = "mallory_";
			provider.transform.Find("Options/Option 1/Text").GetComponent<Text>().text = Translations.GetTranslation(str + this._choice1.ID.ToString(), this._choice1.English);
			provider.transform.Find("Options/Option 2/Text").GetComponent<Text>().text = Translations.GetTranslation(str + this._choice2.ID.ToString(), this._choice2.English);
			provider.transform.Find("Options/Option 1").GetComponent<Button>().onClick.RemoveAllListeners();
			provider.transform.Find("Options/Option 1").GetComponent<Button>().onClick.AddListener(delegate()
			{
				provider.transform.Find("Options").gameObject.SetActive(false);
				provider.transform.Find("Intro Text Box").gameObject.SetActive(true);
				provider.transform.Find("Accept Button").gameObject.SetActive(true);
				this._state += 2;
				this.OnClick(provider);
			});
			provider.transform.Find("Options/Option 2").GetComponent<Button>().onClick.RemoveAllListeners();
			provider.transform.Find("Options/Option 2").GetComponent<Button>().onClick.AddListener(delegate()
			{
				provider.transform.Find("Options").gameObject.SetActive(false);
				provider.transform.Find("Intro Text Box").gameObject.SetActive(true);
				provider.transform.Find("Accept Button").gameObject.SetActive(true);
				this._state++;
				this.OnClick(provider);
			});
		}
		else
		{
			base.OnClick(provider);
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00003238 File Offset: 0x00001438
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		yield return base.Initialize(provider, albumImage);
		yield break;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003270 File Offset: 0x00001470
	public override void Destroy(IntroProvider provider)
	{
		base.Destroy(provider);
	}

	// Token: 0x0400000A RID: 10
	private GirlModel.IntroData _choice1;

	// Token: 0x0400000B RID: 11
	private GirlModel.IntroData _choice2;
}
