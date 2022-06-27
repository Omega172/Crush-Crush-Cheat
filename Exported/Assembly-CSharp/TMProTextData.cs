using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E2 RID: 226
[Serializable]
public class TMProTextData : EventDataContainer, IDeserializeWithWait, ISerializeFromComponent
{
	// Token: 0x060004F2 RID: 1266 RVA: 0x00026EB0 File Offset: 0x000250B0
	IEnumerator IDeserializeWithWait.Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer)
	{
		Text txtMeshProUGUI = targetTransform.gameObject.AddComponent<Text>();
		txtMeshProUGUI.fontSize = this.FontSize;
		txtMeshProUGUI.resizeTextForBestFit = this.AutoSize;
		txtMeshProUGUI.resizeTextMinSize = this.MinSize;
		txtMeshProUGUI.resizeTextMaxSize = this.MaxSize;
		txtMeshProUGUI.alignment = (TextAnchor)this.AlignmentOptions;
		txtMeshProUGUI.color = new Color(this.ColorR, this.ColorG, this.ColorB, this.ColorA);
		txtMeshProUGUI.text = this.Text;
		yield return null;
		txtMeshProUGUI.rectTransform.sizeDelta = new Vector2(this.SizeDeltaX, this.SizeDeltaY);
		yield break;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00026EDC File Offset: 0x000250DC
	public void SetData(Text txtMeshProUGUI)
	{
		this.Text = txtMeshProUGUI.text;
		this.FontSize = txtMeshProUGUI.fontSize;
		this.AutoSize = txtMeshProUGUI.resizeTextForBestFit;
		this.MinSize = txtMeshProUGUI.resizeTextMinSize;
		this.MaxSize = txtMeshProUGUI.resizeTextMaxSize;
		this.AlignmentOptions = (int)txtMeshProUGUI.alignment;
		this.SizeDeltaX = txtMeshProUGUI.rectTransform.sizeDelta.x;
		this.SizeDeltaY = txtMeshProUGUI.rectTransform.sizeDelta.y;
		this.ColorR = txtMeshProUGUI.color.r;
		this.ColorG = txtMeshProUGUI.color.g;
		this.ColorB = txtMeshProUGUI.color.b;
		this.ColorA = txtMeshProUGUI.color.a;
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00026FB8 File Offset: 0x000251B8
	public override void Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer)
	{
		Debug.LogError("Should be using interface instead!");
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00026FC4 File Offset: 0x000251C4
	public void Serialize(Component component)
	{
		Text text = component as Text;
		if (text == null)
		{
			Debug.LogError("Component " + component.name + " is not of type TextMeshProUGUI");
			return;
		}
		this.SetData(text);
	}

	// Token: 0x04000503 RID: 1283
	public string Text = string.Empty;

	// Token: 0x04000504 RID: 1284
	public int FontSize = 64;

	// Token: 0x04000505 RID: 1285
	public int MinSize = 32;

	// Token: 0x04000506 RID: 1286
	public int MaxSize = 64;

	// Token: 0x04000507 RID: 1287
	public bool AutoSize;

	// Token: 0x04000508 RID: 1288
	public float ColorR;

	// Token: 0x04000509 RID: 1289
	public float ColorG;

	// Token: 0x0400050A RID: 1290
	public float ColorB;

	// Token: 0x0400050B RID: 1291
	public float ColorA;

	// Token: 0x0400050C RID: 1292
	public int AlignmentOptions;

	// Token: 0x0400050D RID: 1293
	public float SizeDeltaX;

	// Token: 0x0400050E RID: 1294
	public float SizeDeltaY;
}
