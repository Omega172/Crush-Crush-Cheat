using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class DevelopmentBuildOnly : MonoBehaviour
{
	// Token: 0x06000294 RID: 660 RVA: 0x000125A0 File Offset: 0x000107A0
	private void Awake()
	{
		Debug.LogWarning(string.Concat(new object[]
		{
			"Platform: ",
			Application.platform,
			"  ",
			Debug.isDebugBuild
		}));
		if (!Debug.isDebugBuild || Application.platform != RuntimePlatform.OSXEditor || Application.platform != RuntimePlatform.WindowsEditor)
		{
			if (this.shouldDestroy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private bool shouldDestroy = true;
}
