using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200010D RID: 269
public class FinalInteraction : GenericIntroduction
{
	// Token: 0x06000673 RID: 1651 RVA: 0x00036870 File Offset: 0x00034A70
	public FinalInteraction(Girl girl, List<GirlModel.IntroData> introDataList = null) : base(girl, introDataList)
	{
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0003687C File Offset: 0x00034A7C
	protected override void Init(Girl girl, List<GirlModel.IntroData> introDataList)
	{
		string arg = girl.GirlName.ToString().ToLowerInvariant();
		string[] array = new string[]
		{
			string.Format("eventCGFINAL_{0}1", arg),
			string.Format("eventCGFINAL_{0}2", arg),
			string.Format("eventCGFINAL_{0}", arg)
		};
		if (introDataList.Count == 2)
		{
			introDataList.Add(new GirlModel.IntroData
			{
				Data = array[1],
				Playable = false
			});
		}
		if (introDataList.Count != 3)
		{
			throw new Exception("Universe data did not contain the expected data.");
		}
		introDataList[0].Data = array[0];
		introDataList[1].Data = array[2];
		base.Init(girl, introDataList);
		this._backgroundColor = new Color(0.79607844f, 0.36078432f, 0.49803922f);
		Album.Add(girl.Data, 3);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0003695C File Offset: 0x00034B5C
	public override IEnumerator Initialize(IntroProvider provider, Sprite albumImage)
	{
		if (this._provider == null)
		{
			this._provider = provider;
		}
		provider.Portrait.GetComponent<Button>().onClick.RemoveAllListeners();
		provider.Portrait.GetComponent<Button>().onClick.AddListener(new UnityAction(this.DoInteraction));
		return base.Initialize(provider, albumImage);
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x000369C0 File Offset: 0x00034BC0
	public override void Update(IntroProvider provider)
	{
		if (this._provider.skipping)
		{
			return;
		}
		if (this._interactionTimer > 0f && this._state == 1)
		{
			this._interactionTimer -= Time.deltaTime;
			if (this._interactionTimer > 0.5f)
			{
				this._provider.Portrait.transform.localPosition = new Vector3(5f * Mathf.Sin(20f * this._interactionTimer), 5f * Mathf.Cos(35f * this._interactionTimer), 0f);
			}
			else if (provider.Portrait.transform.localPosition.x != 0f || provider.Portrait.transform.localPosition.y != 0f)
			{
				provider.Portrait.transform.localPosition = new Vector3(0f, 0f);
			}
			if (this._interactionTimer <= 0f)
			{
				provider.Portrait.sprite = provider.Sprites[0];
			}
		}
		base.Update(provider);
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00036AFC File Offset: 0x00034CFC
	public void DoInteraction()
	{
		if (this._provider.Sprites == null || this._provider.Sprites.Length <= 2)
		{
			string data = string.Format("Null: {0} Length: {1} {2}", this._provider.Sprites == null, (this._provider.Sprites != null) ? this._provider.Sprites.Length.ToString() : "null", this.Girl.GirlName.ToFriendlyString());
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, data);
			return;
		}
		if (!this._provider.skipping && this._provider.Portrait.sprite == this._provider.Sprites[0])
		{
			this._interactionTimer = 1f;
			this._provider.Portrait.sprite = this._provider.Sprites[2];
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00036BF0 File Offset: 0x00034DF0
	protected override void Skip(IntroProvider provider)
	{
		if (Girls.CurrentGirl != null && Girls.CurrentGirl.GirlName == Balance.GirlName.DarkOne)
		{
			GameState.GetGirlScreen().SetGirl(Girl.FindGirl(Balance.GirlName.Sutra));
		}
		base.Skip(provider);
	}

	// Token: 0x04000677 RID: 1655
	private float _interactionTimer;

	// Token: 0x04000678 RID: 1656
	private IntroProvider _provider;
}
