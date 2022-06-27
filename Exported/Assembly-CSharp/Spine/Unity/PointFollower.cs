using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E3 RID: 483
	[AddComponentMenu("Spine/Point Follower")]
	[ExecuteInEditMode]
	public class PointFollower : MonoBehaviour, IHasSkeletonRenderer, IHasSkeletonComponent
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0006F2B8 File Offset: 0x0006D4B8
		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return this.skeletonRenderer;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0006F2C0 File Offset: 0x0006D4C0
		public ISkeletonComponent SkeletonComponent
		{
			get
			{
				return this.skeletonRenderer;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0006F2C8 File Offset: 0x0006D4C8
		public bool IsValid
		{
			get
			{
				return this.valid;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0006F2D0 File Offset: 0x0006D4D0
		public void Initialize()
		{
			this.valid = (this.skeletonRenderer != null && this.skeletonRenderer.valid);
			if (!this.valid)
			{
				return;
			}
			this.UpdateReferences();
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0006F30C File Offset: 0x0006D50C
		private void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
		{
			this.Initialize();
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0006F314 File Offset: 0x0006D514
		private void UpdateReferences()
		{
			this.skeletonTransform = this.skeletonRenderer.transform;
			this.skeletonRenderer.OnRebuild -= this.HandleRebuildRenderer;
			this.skeletonRenderer.OnRebuild += this.HandleRebuildRenderer;
			this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
			this.bone = null;
			this.point = null;
			if (!string.IsNullOrEmpty(this.pointAttachmentName))
			{
				Skeleton skeleton = this.skeletonRenderer.Skeleton;
				int num = skeleton.FindSlotIndex(this.slotName);
				if (num >= 0)
				{
					Slot slot = skeleton.slots.Items[num];
					this.bone = slot.bone;
					this.point = (skeleton.GetAttachment(num, this.pointAttachmentName) as PointAttachment);
				}
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0006F3EC File Offset: 0x0006D5EC
		private void OnDestroy()
		{
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRebuildRenderer;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0006F424 File Offset: 0x0006D624
		public void LateUpdate()
		{
			if (this.point == null)
			{
				if (string.IsNullOrEmpty(this.pointAttachmentName))
				{
					return;
				}
				this.UpdateReferences();
				if (this.point == null)
				{
					return;
				}
			}
			Vector2 vector;
			this.point.ComputeWorldPosition(this.bone, out vector.x, out vector.y);
			float num = this.point.ComputeWorldRotation(this.bone);
			Transform transform = base.transform;
			if (this.skeletonTransformIsParent)
			{
				transform.localPosition = new Vector3(vector.x, vector.y, (!this.followSkeletonZPosition) ? transform.localPosition.z : 0f);
				if (this.followRotation)
				{
					float f = num * 0.5f * 0.017453292f;
					transform.localRotation = new Quaternion
					{
						z = Mathf.Sin(f),
						w = Mathf.Cos(f)
					};
				}
			}
			else
			{
				Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(vector.x, vector.y, 0f));
				if (!this.followSkeletonZPosition)
				{
					position.z = transform.position.z;
				}
				Transform parent = transform.parent;
				if (parent != null)
				{
					Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
					if (localToWorldMatrix.m00 * localToWorldMatrix.m11 - localToWorldMatrix.m01 * localToWorldMatrix.m10 < 0f)
					{
						num = -num;
					}
				}
				if (this.followRotation)
				{
					Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
					transform.position = position;
					transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num);
				}
				else
				{
					transform.position = position;
				}
			}
			if (this.followSkeletonFlip)
			{
				Vector3 localScale = transform.localScale;
				localScale.y = Mathf.Abs(localScale.y) * Mathf.Sign(this.bone.skeleton.ScaleX * this.bone.skeleton.ScaleY);
				transform.localScale = localScale;
			}
		}

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000CE5 RID: 3301
		[SpineSlot("", "skeletonRenderer", false, true, false)]
		public string slotName;

		// Token: 0x04000CE6 RID: 3302
		[SpineAttachment(true, false, false, "slotName", "skeletonRenderer", "", true, true)]
		public string pointAttachmentName;

		// Token: 0x04000CE7 RID: 3303
		public bool followRotation = true;

		// Token: 0x04000CE8 RID: 3304
		public bool followSkeletonFlip = true;

		// Token: 0x04000CE9 RID: 3305
		public bool followSkeletonZPosition;

		// Token: 0x04000CEA RID: 3306
		private Transform skeletonTransform;

		// Token: 0x04000CEB RID: 3307
		private bool skeletonTransformIsParent;

		// Token: 0x04000CEC RID: 3308
		private PointAttachment point;

		// Token: 0x04000CED RID: 3309
		private Bone bone;

		// Token: 0x04000CEE RID: 3310
		private bool valid;
	}
}
