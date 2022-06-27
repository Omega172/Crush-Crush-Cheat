using System;

// Token: 0x02000097 RID: 151
public interface IUpdateable
{
	// Token: 0x06000304 RID: 772
	GameState.UpdateType PerformUpdate(float dt);

	// Token: 0x06000305 RID: 773
	void SaveCurrentTime();
}
