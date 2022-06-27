using System;
using System.Collections.Generic;
using System.Text;
using BlayFap;
using BlayFapShared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200011C RID: 284
public class Voting : MonoBehaviour
{
	// Token: 0x060006ED RID: 1773 RVA: 0x0003C314 File Offset: 0x0003A514
	private void Start()
	{
		this._voteButton = base.transform.Find("Dialog/Buy Button").GetComponent<Button>();
		this._voteCounter = base.transform.Find("Dialog/Counter").GetComponent<Text>();
		this._voteButton.onClick.AddListener(new UnityAction(this.OnVoteButtonClicked));
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0003C374 File Offset: 0x0003A574
	public void Init(PhoneModel girl)
	{
		if (this._voteCounter == null)
		{
			this._voteCounter = base.transform.Find("Dialog/Counter").GetComponent<Text>();
		}
		if (this._voteButton == null)
		{
			this._voteButton = base.transform.Find("Dialog/Buy Button").GetComponent<Button>();
		}
		this._voteButton.interactable = false;
		this._girl = girl;
		if (Math.Abs((DateTime.UtcNow - this._lastCache).TotalMinutes) > 10.0 || this._lastPollResponse == null)
		{
			GetPollRequest request = new GetPollRequest
			{
				Id = 9U
			};
			BlayFapClient.Instance.GetPoll(request, delegate(GetPollResponse response)
			{
				this.OnVoteResponse(response);
				this._lastPollResponse = response;
				this._lastCache = DateTime.UtcNow;
			});
		}
		else
		{
			this.OnVoteResponse(this._lastPollResponse);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0003C468 File Offset: 0x0003A668
	private void OnVoteResponse(GetPollResponse response)
	{
		if (response.VoteStatus >= 5)
		{
			this._voteCounter.text = "You have voted 5/5 times.";
			this._voteButton.interactable = false;
		}
		else
		{
			this._voteCounter.text = string.Format("You have voted {0}/5 times.", response.VoteStatus.ToString());
			this._voteButton.interactable = true;
		}
		this._voteButton.transform.Find("Free").gameObject.SetActive(response.VoteStatus == 0);
		this._voteButton.transform.Find("Cost").gameObject.SetActive(response.VoteStatus != 0);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0003C524 File Offset: 0x0003A724
	private void OnVoteError(BlayFapResponse response)
	{
		this._voteCounter.text = "There was an error voting.";
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0003C538 File Offset: 0x0003A738
	private void OnVoteButtonClicked()
	{
		if (this._girl == null)
		{
			return;
		}
		if (this._lastPollResponse.VoteStatus == 0)
		{
			this.VoteForGirl(this._girl, false);
		}
		else
		{
			Utilities.ConfirmPurchase(5, delegate
			{
				this.VoteForGirl(this._girl, true);
			});
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0003C588 File Offset: 0x0003A788
	private uint MapGirlToQuestionId(PhoneModel girl)
	{
		if (girl.Id < 7)
		{
			return (uint)(girl.Id - 3);
		}
		if (girl.Id < 10)
		{
			return (uint)(girl.Id - 4);
		}
		if (girl.Id < 16)
		{
			return (uint)(girl.Id - 5);
		}
		return (uint)(girl.Id - 6);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0003C5E0 File Offset: 0x0003A7E0
	private void VoteForGirl(PhoneModel girl, bool purchased = false)
	{
		this._voteButton.interactable = false;
		VoteForPollRequest request = new VoteForPollRequest
		{
			Id = 9U,
			QuestionId = this.MapGirlToQuestionId(girl),
			UseDiamonds = purchased
		};
		BlayFapClient.Instance.VoteForPoll(request, delegate(BlayFapResponse response)
		{
			if (response.Error == null)
			{
				GetPollResponse lastPollResponse = this._lastPollResponse;
				lastPollResponse.VoteStatus += 1;
				this._lastPollResponse.Responses[(int)((UIntPtr)(request.QuestionId - 1U))] += 1UL;
				this.OnVoteResponse(this._lastPollResponse);
			}
			else
			{
				this.OnVoteError(response);
			}
		});
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0003C64C File Offset: 0x0003A84C
	public void ShowResults()
	{
		if (this._resultsTransform == null)
		{
			this._resultsTransform = GameState.CurrentState.transform.Find("Popups/Cellphone/Phone UI/Messenger/Vote Results");
		}
		if (Math.Abs((DateTime.UtcNow - this._lastCache).TotalMinutes) > 10.0 || this._lastPollResponse == null)
		{
			GetPollRequest request = new GetPollRequest
			{
				Id = 9U
			};
			BlayFapClient.Instance.GetPoll(request, delegate(GetPollResponse response)
			{
				this.ShowResults(response);
				this._lastPollResponse = response;
				this._lastCache = DateTime.UtcNow;
			});
		}
		else
		{
			this.ShowResults(this._lastPollResponse);
		}
		this._resultsTransform.gameObject.SetActive(true);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0003C704 File Offset: 0x0003A904
	private void ShowResults(GetPollResponse response)
	{
		List<Voting.SortableResult> list = new List<Voting.SortableResult>();
		for (int i = 0; i < response.Questions.Length; i++)
		{
			list.Add(new Voting.SortableResult(response.Questions[i], response.Responses[i]));
		}
		list.Sort();
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < list.Count; j++)
		{
			Voting.SortableResult sortableResult = list[j];
			if (j == 0)
			{
				stringBuilder.Append("1st: ");
			}
			else if (j == 1)
			{
				stringBuilder.Append("2nd: ");
			}
			else if (j == 2)
			{
				stringBuilder.Append("3rd: ");
			}
			stringBuilder.AppendLine(sortableResult.ToString());
		}
		this._resultsTransform.Find("Dialog/Results").GetComponent<Text>().text = stringBuilder.ToString();
	}

	// Token: 0x040006F5 RID: 1781
	private DateTime _lastCache;

	// Token: 0x040006F6 RID: 1782
	private Text _voteCounter;

	// Token: 0x040006F7 RID: 1783
	private Button _voteButton;

	// Token: 0x040006F8 RID: 1784
	private GetPollResponse _lastPollResponse;

	// Token: 0x040006F9 RID: 1785
	private PhoneModel _girl;

	// Token: 0x040006FA RID: 1786
	private Transform _resultsTransform;

	// Token: 0x0200011D RID: 285
	private class SortableResult : IComparable<Voting.SortableResult>
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0003C830 File Offset: 0x0003AA30
		public SortableResult(string name, ulong count)
		{
			name = name.Replace("MissD", "Miss D");
			this.Name = name;
			this.Count = count;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0003C864 File Offset: 0x0003AA64
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0003C86C File Offset: 0x0003AA6C
		public string Name { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0003C878 File Offset: 0x0003AA78
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0003C880 File Offset: 0x0003AA80
		public ulong Count { get; private set; }

		// Token: 0x060006FE RID: 1790 RVA: 0x0003C88C File Offset: 0x0003AA8C
		public int CompareTo(Voting.SortableResult other)
		{
			return -this.Count.CompareTo(other.Count);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0003C8B0 File Offset: 0x0003AAB0
		public override string ToString()
		{
			return this.Name;
		}
	}
}
