using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FD RID: 509
	public class FollowSkeletonUtilityRootRotation : MonoBehaviour
	{
		// Token: 0x060010A2 RID: 4258 RVA: 0x00074710 File Offset: 0x00072910
		private void Start()
		{
			this.prevLocalEulerAngles = base.transform.localEulerAngles;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00074724 File Offset: 0x00072924
		private void FixedUpdate()
		{
			base.transform.rotation = this.reference.rotation;
			bool flag = Mathf.Abs(base.transform.localEulerAngles.y - this.prevLocalEulerAngles.y) > 100f;
			bool flag2 = Mathf.Abs(base.transform.localEulerAngles.x - this.prevLocalEulerAngles.x) > 100f;
			if (flag)
			{
				this.CompensatePositionToYRotation();
			}
			if (flag2)
			{
				this.CompensatePositionToXRotation();
			}
			this.prevLocalEulerAngles = base.transform.localEulerAngles;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x000747C8 File Offset: 0x000729C8
		private void CompensatePositionToYRotation()
		{
			Vector3 position = this.reference.position + (this.reference.position - base.transform.position);
			position.y = base.transform.position.y;
			base.transform.position = position;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00074828 File Offset: 0x00072A28
		private void CompensatePositionToXRotation()
		{
			Vector3 position = this.reference.position + (this.reference.position - base.transform.position);
			position.x = base.transform.position.x;
			base.transform.position = position;
		}

		// Token: 0x04000DA5 RID: 3493
		private const float FLIP_ANGLE_THRESHOLD = 100f;

		// Token: 0x04000DA6 RID: 3494
		public Transform reference;

		// Token: 0x04000DA7 RID: 3495
		private Vector3 prevLocalEulerAngles;
	}
}
