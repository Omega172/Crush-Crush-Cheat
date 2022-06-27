using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FB RID: 507
	[RequireComponent(typeof(Rigidbody))]
	public class FollowLocationRigidbody : MonoBehaviour
	{
		// Token: 0x0600109C RID: 4252 RVA: 0x0007460C File Offset: 0x0007280C
		private void Awake()
		{
			this.ownRigidbody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0007461C File Offset: 0x0007281C
		private void FixedUpdate()
		{
			this.ownRigidbody.rotation = this.reference.rotation;
			this.ownRigidbody.position = this.reference.position;
		}

		// Token: 0x04000DA0 RID: 3488
		public Transform reference;

		// Token: 0x04000DA1 RID: 3489
		private Rigidbody ownRigidbody;
	}
}
