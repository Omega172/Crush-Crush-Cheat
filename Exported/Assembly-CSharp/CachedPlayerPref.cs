using System;

// Token: 0x02000161 RID: 353
public class CachedPlayerPref
{
	// Token: 0x06000A1A RID: 2586 RVA: 0x00053964 File Offset: 0x00051B64
	public CachedPlayerPref(string name)
	{
		this.name = name;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00053974 File Offset: 0x00051B74
	public void SetFloat(float value)
	{
		PlayerPrefs.SetFloat(this.name, value);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00053984 File Offset: 0x00051B84
	public void SetString(string value)
	{
		PlayerPrefs.SetString(this.name, value);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x00053994 File Offset: 0x00051B94
	public void SetLong(long value)
	{
		PlayerPrefs.SetLong(this.name, value);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x000539A4 File Offset: 0x00051BA4
	public void SetInt(int value)
	{
		PlayerPrefs.SetInt(this.name, value);
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x000539B4 File Offset: 0x00051BB4
	public string GetString(string defaultValue)
	{
		return PlayerPrefs.GetString(this.name, defaultValue);
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x000539C4 File Offset: 0x00051BC4
	public long GetLong(long defaultValue)
	{
		return PlayerPrefs.GetLong(this.name, defaultValue);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x000539D4 File Offset: 0x00051BD4
	public void DeleteKey()
	{
		PlayerPrefs.DeleteKey(this.name, false);
	}

	// Token: 0x040009CA RID: 2506
	private string name;
}
