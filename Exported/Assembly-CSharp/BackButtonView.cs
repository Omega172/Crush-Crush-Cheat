using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000EA RID: 234
public class BackButtonView : MonoBehaviour
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000541 RID: 1345 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
	// (set) Token: 0x06000542 RID: 1346 RVA: 0x0002C2DC File Offset: 0x0002A4DC
	public bool CanBeDisabled
	{
		get
		{
			return this._canBeDisabled;
		}
		set
		{
			this._canBeDisabled = value;
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0002C2E8 File Offset: 0x0002A4E8
	private bool HasCallbackImplemented(UnityEvent unityEvent)
	{
		if (unityEvent == null)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
		{
			if (unityEvent.GetPersistentTarget(i) != null)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002C330 File Offset: 0x0002A530
	public void SetCanAddToStack(bool canAddToStack)
	{
		this._canAddToBackButtonStack = canAddToStack;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0002C33C File Offset: 0x0002A53C
	public bool CheckDoBackButtonOverride()
	{
		if (this._backButtonOverride == null)
		{
			return false;
		}
		bool flag = this.HasCallbackImplemented(this._backButtonOverride);
		if (flag)
		{
			this._backButtonOverride.Invoke();
		}
		return flag;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0002C378 File Offset: 0x0002A578
	public void DoOnPop(bool fromBackButton)
	{
		this._wasDisabledFromStack = fromBackButton;
		bool flag = false;
		if (fromBackButton || !this._onPopOnlyFromBackButton)
		{
			flag = (this._doOnPopAction != null || this.HasCallbackImplemented(this._onPopAction));
			if (this.HasCallbackImplemented(this._onPopAction))
			{
				this._onPopAction.Invoke();
			}
			if (this._doOnPopAction != null)
			{
				this._doOnPopAction();
			}
		}
		if (!flag && this._canBeDisabled)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0002C40C File Offset: 0x0002A60C
	public void SetDoOnPopAction(Action callback)
	{
		this._doOnPopAction = callback;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002C418 File Offset: 0x0002A618
	public void RemoveDoOnPopAction()
	{
		this._doOnPopAction = null;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002C424 File Offset: 0x0002A624
	public void SetLocksView(bool locksView)
	{
		this._locksView = locksView;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002C430 File Offset: 0x0002A630
	public void OnGotContext()
	{
		if (this._locksView)
		{
			UnityEngine.Object.FindObjectOfType<BackButtonController>().LockView();
		}
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0002C448 File Offset: 0x0002A648
	public void OnLostContext()
	{
		if (this._locksView)
		{
			UnityEngine.Object.FindObjectOfType<BackButtonController>().UnlockView();
		}
	}

	// Token: 0x0400054B RID: 1355
	[SerializeField]
	[Header("Optional")]
	private bool _locksView;

	// Token: 0x0400054C RID: 1356
	[SerializeField]
	private bool _onPopOnlyFromBackButton;

	// Token: 0x0400054D RID: 1357
	[SerializeField]
	private UnityEvent _onPopAction;

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private UnityEvent _backButtonOverride;

	// Token: 0x0400054F RID: 1359
	[SerializeField]
	private UnityEvent<bool> _useBackButtonValidator;

	// Token: 0x04000550 RID: 1360
	private Action _doOnPopAction;

	// Token: 0x04000551 RID: 1361
	private bool _canAddToBackButtonStack = true;

	// Token: 0x04000552 RID: 1362
	private bool _wasDisabledFromStack;

	// Token: 0x04000553 RID: 1363
	private bool _canBeDisabled = true;
}
