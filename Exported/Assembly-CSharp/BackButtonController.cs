using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class BackButtonController : MonoBehaviour
{
	// Token: 0x0600053C RID: 1340 RVA: 0x0002C29C File Offset: 0x0002A49C
	public void PushToStack(BackButtonView _viewGO)
	{
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0002C2A0 File Offset: 0x0002A4A0
	public void Pop(bool fromBackButton = false)
	{
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0002C2A4 File Offset: 0x0002A4A4
	public void LockView()
	{
		this._isViewLocked = true;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
	public void UnlockView()
	{
		this._isViewLocked = false;
	}

	// Token: 0x04000548 RID: 1352
	private Stack<BackButtonView> _viewStack = new Stack<BackButtonView>();

	// Token: 0x04000549 RID: 1353
	private GameObject _quitGamePopup;

	// Token: 0x0400054A RID: 1354
	[Header("Debug")]
	[SerializeField]
	private bool _isViewLocked;
}
