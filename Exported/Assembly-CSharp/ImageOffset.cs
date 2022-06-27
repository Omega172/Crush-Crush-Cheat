using System;

// Token: 0x02000158 RID: 344
public struct ImageOffset
{
	// Token: 0x06000996 RID: 2454 RVA: 0x0005030C File Offset: 0x0004E50C
	public ImageOffset(int leftOffset, int width)
	{
		this.LeftOffset = leftOffset;
		this.Width = width;
	}

	// Token: 0x04000986 RID: 2438
	public int LeftOffset;

	// Token: 0x04000987 RID: 2439
	public int Width;
}
