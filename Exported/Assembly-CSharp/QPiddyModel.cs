using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class QPiddyModel : GirlModel
{
	// Token: 0x0600094D RID: 2381 RVA: 0x0004EC38 File Offset: 0x0004CE38
	public QPiddyModel(AssetBundle gamedata, List<string[]> states) : base(gamedata, states)
	{
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600094E RID: 2382 RVA: 0x0004EC64 File Offset: 0x0004CE64
	public List<GirlModel.IntroData> OutroIntro
	{
		get
		{
			return this._outroIntro;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x0600094F RID: 2383 RVA: 0x0004EC6C File Offset: 0x0004CE6C
	public List<GirlModel.IntroData> SacrificeYourselfIntro
	{
		get
		{
			return this._sacrificeYourselfIntro;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x0004EC74 File Offset: 0x0004CE74
	public List<GirlModel.IntroData> SacrificeQPiddyIntro
	{
		get
		{
			return this._sacrificeQPiddyIntro;
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0004EC7C File Offset: 0x0004CE7C
	protected override void AddIntroToList(GirlModel.IntroData introData, GirlModel.TextType textType)
	{
		if (textType == GirlModel.TextType.Outro)
		{
			this._outroIntro.Add(introData);
		}
		else if (textType == GirlModel.TextType.SacrificeQPiddy || textType == GirlModel.TextType.SacrificeQPiddyCrush)
		{
			this._sacrificeQPiddyIntro.Add(introData);
		}
		else if (textType == GirlModel.TextType.SacrificeYourself || textType == GirlModel.TextType.SacrificeYourselfCrush)
		{
			this._sacrificeYourselfIntro.Add(introData);
		}
		else
		{
			base.AddIntroToList(introData, textType);
		}
	}

	// Token: 0x04000960 RID: 2400
	private List<GirlModel.IntroData> _outroIntro = new List<GirlModel.IntroData>();

	// Token: 0x04000961 RID: 2401
	private List<GirlModel.IntroData> _sacrificeYourselfIntro = new List<GirlModel.IntroData>();

	// Token: 0x04000962 RID: 2402
	private List<GirlModel.IntroData> _sacrificeQPiddyIntro = new List<GirlModel.IntroData>();
}
