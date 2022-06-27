using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class Roflcopter : MonoBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x00003FA0 File Offset: 0x000021A0
	private void Update()
	{
		this.currentTime += Time.deltaTime;
		this.currentTime1 += Time.deltaTime;
		if (this.currentTime > 0.25f)
		{
			this.whichRofl = !this.whichRofl;
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = (!this.whichRofl) ? this.rofl2 : this.rofl1;
			string str = new string(' ', this.offset);
			for (int i = 0; i < array.Length - 1; i++)
			{
				stringBuilder.AppendLine(str + array[i]);
			}
			stringBuilder.Append(str + array[array.Length - 1]);
			base.GetComponent<Text>().text = stringBuilder.ToString();
			this.currentTime -= 0.25f;
		}
		if (this.currentTime1 > 1f)
		{
			this.currentTime1 = 0f;
			this.offset += this.offsetDiff;
			if (this.offset > 10 || this.offset == 0)
			{
				this.offsetDiff *= -1;
			}
		}
	}

	// Token: 0x0400001D RID: 29
	private string[] rofl1 = new string[]
	{
		" ROFL:ROFL:LOL:",
		"         ___^___ _",
		" L    __/      [] \\   ",
		" O ===__           \\ ",
		" L      \\___ ___ ___]",
		"            I   I",
		"          ----------/"
	};

	// Token: 0x0400001E RID: 30
	private string[] rofl2 = new string[]
	{
		"          :LOL:ROFL:ROFL",
		"         ___^___ _",
		"      __/      [] \\   ",
		"LOL===__           \\ ",
		"        \\___ ___ ___]",
		"            I   I",
		"          ----------/"
	};

	// Token: 0x0400001F RID: 31
	private float currentTime;

	// Token: 0x04000020 RID: 32
	private float currentTime1;

	// Token: 0x04000021 RID: 33
	private bool whichRofl;

	// Token: 0x04000022 RID: 34
	private int offset;

	// Token: 0x04000023 RID: 35
	private int offsetDiff = 1;
}
