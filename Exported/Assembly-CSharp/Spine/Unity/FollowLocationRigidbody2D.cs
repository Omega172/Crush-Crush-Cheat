using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FC RID: 508
	[RequireComponent(typeof(Rigidbody2D))]
	public class FollowLocationRigidbody2D : MonoBehaviour
	{
		// Token: 0x0600109F RID: 4255 RVA: 0x00074660 File Offset: 0x00072860
		private void Awake()
		{
			this.ownRigidbody = base.GetComponent<Rigidbody2D>();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00074670 File Offset: 0x00072870
		private void FixedUpdate()
		{
			if (this.followFlippedX)
			{
				this.ownRigidbody.rotation = (-this.reference.rotation.eulerAngles.z + 270f) % 360f - 90f;
			}
			else
			{
				this.ownRigidbody.rotation = this.reference.rotation.eulerAngles.z;
			}
			this.ownRigidbody.position = this.reference.position;
		}

		// Token: 0x04000DA2 RID: 3490
		public Transform reference;

		// Token: 0x04000DA3 RID: 3491
		public bool followFlippedX;

		// Token: 0x04000DA4 RID: 3492
		private Rigidbody2D ownRigidbody;
	}
}
