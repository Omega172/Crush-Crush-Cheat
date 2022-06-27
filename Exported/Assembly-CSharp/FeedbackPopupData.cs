using System;

// Token: 0x02000098 RID: 152
public class FeedbackPopupData
{
	// Token: 0x06000307 RID: 775 RVA: 0x00016510 File Offset: 0x00014710
	public FeedbackPopupData SetText(string title, string content)
	{
		this.Title = title;
		this.Content = content;
		return this;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00016524 File Offset: 0x00014724
	public FeedbackPopupData Center(bool centerPopup = true)
	{
		this.CenterPopup = centerPopup;
		return this;
	}

	// Token: 0x0400034F RID: 847
	public string Title;

	// Token: 0x04000350 RID: 848
	public string Content;

	// Token: 0x04000351 RID: 849
	public bool EnablePopup = true;

	// Token: 0x04000352 RID: 850
	public bool CenterPopup;

	// Token: 0x04000353 RID: 851
	public Action YesButtonAction;

	// Token: 0x04000354 RID: 852
	public Action NoButtonAction;
}
