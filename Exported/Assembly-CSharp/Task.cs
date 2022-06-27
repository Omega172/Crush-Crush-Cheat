using System;

// Token: 0x020000E4 RID: 228
public class Task
{
	// Token: 0x060004F9 RID: 1273 RVA: 0x0002724C File Offset: 0x0002544C
	public Task(TaskData taskData)
	{
		this.TaskData = taskData;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0002725C File Offset: 0x0002545C
	public void StoreState(string taskPrefix)
	{
		PlayerPrefs.SetLong(taskPrefix + "Start", this.DateStart);
		PlayerPrefs.SetInt(taskPrefix + "Complete", (!this.Complete) ? 0 : 1);
		PlayerPrefs.SetInt(taskPrefix + "Claimed", (!this.Claimed) ? 0 : 1);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000272C4 File Offset: 0x000254C4
	public void LoadState(string taskPrefix)
	{
		this.DateStart = PlayerPrefs.GetLong(taskPrefix + "Start", this.DateStart);
		this.Complete = (PlayerPrefs.GetInt(taskPrefix + "Complete", 0) == 1);
		this.Claimed = (PlayerPrefs.GetInt(taskPrefix + "Claimed", 0) == 1);
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00027324 File Offset: 0x00025524
	public override string ToString()
	{
		return string.Format("Task {0} is {1}.", this.TaskData.Name, (!this.Complete) ? "Incomplete" : "Complete");
	}

	// Token: 0x04000513 RID: 1299
	public TaskData TaskData;

	// Token: 0x04000514 RID: 1300
	public bool Complete;

	// Token: 0x04000515 RID: 1301
	public long DateStart;

	// Token: 0x04000516 RID: 1302
	public bool Claimed;
}
