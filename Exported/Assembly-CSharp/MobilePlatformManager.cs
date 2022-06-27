using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class MobilePlatformManager : MonoBehaviour
{
	// Token: 0x0600055E RID: 1374 RVA: 0x0002C71C File Offset: 0x0002A91C
	private void Awake()
	{
		Debug.LogWarning("Platform: " + Application.platform);
		if (Application.platform != this.targetPlatform && !this.IsEditor())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0002C768 File Offset: 0x0002A968
	private bool IsEditor()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor;
	}

	// Token: 0x04000555 RID: 1365
	[SerializeField]
	private RuntimePlatform targetPlatform = RuntimePlatform.Android;
}
