using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000B RID: 11
public class Orbs : MonoBehaviour
{
	// Token: 0x06000029 RID: 41 RVA: 0x000037A8 File Offset: 0x000019A8
	private void Start()
	{
		this._orbs = new Image[base.transform.childCount];
		for (int i = 0; i < this._orbs.Length; i++)
		{
			this._orbs[i] = base.transform.GetChild(i).GetComponent<Image>();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003800 File Offset: 0x00001A00
	private void Update()
	{
		this._currentTime1 += Time.deltaTime * 0.7f;
		this._currentTime1 %= 6.2831855f;
		this._currentTime2 += Time.deltaTime;
		this._currentTime2 %= 6.2831855f;
		this._currentTime3 += Time.deltaTime * 1.3f;
		this._currentTime3 %= 6.2831855f;
		this._orbs[0].rectTransform.localPosition = new Vector3(114f, 127f + 20f * Mathf.Sin(this._currentTime1));
		this._orbs[1].rectTransform.localPosition = new Vector3(-101f, 124f + 15f * Mathf.Cos(this._currentTime2));
		this._orbs[2].rectTransform.localScale = new Vector3(1f + 0.2f * Mathf.Cos(this._currentTime1), 1f + 0.2f * Mathf.Cos(this._currentTime1), 1f);
		this._orbs[2].rectTransform.localPosition = new Vector3(-91f, 7f + 10f * Mathf.Sin(this._currentTime3));
		this._orbs[3].rectTransform.localScale = new Vector3(1f + 0.15f * Mathf.Sin(this._currentTime2), 1f + 0.15f * Mathf.Sin(this._currentTime2), 1f);
		this._orbs[3].rectTransform.localPosition = new Vector3(95f, -101f + 15f * Mathf.Cos(this._currentTime1));
		this._orbs[4].rectTransform.localScale = new Vector3(1f + 0.1f * Mathf.Sin(this._currentTime3), 1f + 0.1f * Mathf.Sin(this._currentTime3), 1f);
		this._orbs[4].rectTransform.localPosition = new Vector3(76f, 102f + 10f * Mathf.Sin(this._currentTime2));
	}

	// Token: 0x04000014 RID: 20
	private Image[] _orbs;

	// Token: 0x04000015 RID: 21
	private float _currentTime1;

	// Token: 0x04000016 RID: 22
	private float _currentTime2;

	// Token: 0x04000017 RID: 23
	private float _currentTime3;
}
