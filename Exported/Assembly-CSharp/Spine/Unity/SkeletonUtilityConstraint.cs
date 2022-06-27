using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000202 RID: 514
	[RequireComponent(typeof(SkeletonUtilityBone))]
	[ExecuteInEditMode]
	public abstract class SkeletonUtilityConstraint : MonoBehaviour
	{
		// Token: 0x060010D2 RID: 4306 RVA: 0x00075DE4 File Offset: 0x00073FE4
		protected virtual void OnEnable()
		{
			this.bone = base.GetComponent<SkeletonUtilityBone>();
			this.hierarchy = base.transform.GetComponentInParent<SkeletonUtility>();
			this.hierarchy.RegisterConstraint(this);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00075E1C File Offset: 0x0007401C
		protected virtual void OnDisable()
		{
			this.hierarchy.UnregisterConstraint(this);
		}

		// Token: 0x060010D4 RID: 4308
		public abstract void DoUpdate();

		// Token: 0x04000DCC RID: 3532
		protected SkeletonUtilityBone bone;

		// Token: 0x04000DCD RID: 3533
		protected SkeletonUtility hierarchy;
	}
}
