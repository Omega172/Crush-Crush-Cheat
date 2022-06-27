using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class DebugInitController : MonoBehaviour
{
	// Token: 0x06000292 RID: 658 RVA: 0x000124B4 File Offset: 0x000106B4
	public void InitDebug()
	{
		if (GameState.CurrentState.transform.Find("Popups/Debug") != null)
		{
			return;
		}
		Transform parent = GameState.CurrentState.transform.Find("Popups");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._debugPrefab, parent);
		gameObject.transform.SetAsLastSibling();
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
		gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		gameObject.name = "Debug";
	}

	// Token: 0x040002F7 RID: 759
	[SerializeField]
	private GameObject _debugPrefab;
}
