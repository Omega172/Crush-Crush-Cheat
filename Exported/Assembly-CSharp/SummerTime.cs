using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class SummerTime : MonoBehaviour
{
	// Token: 0x06000047 RID: 71 RVA: 0x00004788 File Offset: 0x00002988
	private bool IsActive(int i)
	{
		return (global::PlayerPrefs.GetInt("SummerItems2017", 0) & 1 << i) != 1 << i;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000047A8 File Offset: 0x000029A8
	private void SetActive(int i, bool value)
	{
		if (!this.IsActive(i))
		{
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000047BC File Offset: 0x000029BC
	private string CreateCountdown(TimeSpan end)
	{
		int num = 10 + ((end.Days <= 9) ? 0 : 1);
		if (this.countdown == null || this.countdown.Length != num)
		{
			this.countdown = new char[num];
		}
		int num2 = 0;
		if (end.Days >= 10)
		{
			this.countdown[num2++] = (char)(48 + end.Days / 10);
		}
		this.countdown[num2++] = (char)(48 + end.Days % 10);
		this.countdown[num2++] = ':';
		if (end.Hours >= 10)
		{
			this.countdown[num2++] = (char)(48 + end.Hours / 10);
		}
		else
		{
			this.countdown[num2++] = '0';
		}
		this.countdown[num2++] = (char)(48 + end.Hours % 10);
		this.countdown[num2++] = ':';
		if (end.Minutes >= 10)
		{
			this.countdown[num2++] = (char)(48 + end.Minutes / 10);
		}
		else
		{
			this.countdown[num2++] = '0';
		}
		this.countdown[num2++] = (char)(48 + end.Minutes % 10);
		this.countdown[num2++] = ':';
		if (end.Seconds >= 10)
		{
			this.countdown[num2++] = (char)(48 + end.Seconds / 10);
		}
		else
		{
			this.countdown[num2++] = '0';
		}
		this.countdown[num2++] = (char)(48 + end.Seconds % 10);
		return new string(this.countdown);
	}

	// Token: 0x04000036 RID: 54
	private char[] countdown;
}
