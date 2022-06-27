using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FA RID: 506
	public class ActivateBasedOnFlipDirection : MonoBehaviour
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x00074440 File Offset: 0x00072640
		private void Start()
		{
			this.jointsNormalX = this.activeOnNormalX.GetComponentsInChildren<HingeJoint2D>();
			this.jointsFlippedX = this.activeOnFlippedX.GetComponentsInChildren<HingeJoint2D>();
			ISkeletonComponent skeletonComponent2;
			if (this.skeletonRenderer != null)
			{
				ISkeletonComponent skeletonComponent = this.skeletonRenderer;
				skeletonComponent2 = skeletonComponent;
			}
			else
			{
				skeletonComponent2 = this.skeletonGraphic;
			}
			this.skeletonComponent = skeletonComponent2;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0007449C File Offset: 0x0007269C
		private void FixedUpdate()
		{
			bool flag = this.skeletonComponent.Skeleton.ScaleX < 0f;
			if (flag != this.wasFlippedXBefore)
			{
				this.HandleFlip(flag);
			}
			this.wasFlippedXBefore = flag;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000744DC File Offset: 0x000726DC
		private void HandleFlip(bool isFlippedX)
		{
			GameObject gameObject = (!isFlippedX) ? this.activeOnNormalX : this.activeOnFlippedX;
			GameObject gameObject2 = (!isFlippedX) ? this.activeOnFlippedX : this.activeOnNormalX;
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			this.ResetJointPositions((!isFlippedX) ? this.jointsNormalX : this.jointsFlippedX);
			this.ResetJointPositions((!isFlippedX) ? this.jointsFlippedX : this.jointsNormalX);
			this.CompensateMovementAfterFlipX(gameObject.transform, gameObject2.transform);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00074574 File Offset: 0x00072774
		private void ResetJointPositions(HingeJoint2D[] joints)
		{
			foreach (HingeJoint2D hingeJoint2D in joints)
			{
				Transform transform = hingeJoint2D.connectedBody.transform;
				hingeJoint2D.transform.position = transform.TransformPoint(hingeJoint2D.connectedAnchor);
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x000745C4 File Offset: 0x000727C4
		private void CompensateMovementAfterFlipX(Transform toActivate, Transform toDeactivate)
		{
			Transform child = toDeactivate.GetChild(0);
			Transform child2 = toActivate.GetChild(0);
			toActivate.position += child.position - child2.position;
		}

		// Token: 0x04000D98 RID: 3480
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000D99 RID: 3481
		public SkeletonGraphic skeletonGraphic;

		// Token: 0x04000D9A RID: 3482
		public GameObject activeOnNormalX;

		// Token: 0x04000D9B RID: 3483
		public GameObject activeOnFlippedX;

		// Token: 0x04000D9C RID: 3484
		private HingeJoint2D[] jointsNormalX;

		// Token: 0x04000D9D RID: 3485
		private HingeJoint2D[] jointsFlippedX;

		// Token: 0x04000D9E RID: 3486
		private ISkeletonComponent skeletonComponent;

		// Token: 0x04000D9F RID: 3487
		private bool wasFlippedXBefore;
	}
}
