using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FF RID: 511
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/SkeletonUtilityBone")]
	public class SkeletonUtilityBone : MonoBehaviour
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x00075674 File Offset: 0x00073874
		public bool IncompatibleTransformMode
		{
			get
			{
				return this.incompatibleTransformMode;
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0007567C File Offset: 0x0007387C
		public void Reset()
		{
			this.bone = null;
			this.cachedTransform = base.transform;
			this.valid = (this.hierarchy != null && this.hierarchy.IsValid);
			if (!this.valid)
			{
				return;
			}
			this.skeletonTransform = this.hierarchy.transform;
			this.hierarchy.OnReset -= this.HandleOnReset;
			this.hierarchy.OnReset += this.HandleOnReset;
			this.DoUpdate(SkeletonUtilityBone.UpdatePhase.Local);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00075714 File Offset: 0x00073914
		private void OnEnable()
		{
			if (this.hierarchy == null)
			{
				this.hierarchy = base.transform.GetComponentInParent<SkeletonUtility>();
			}
			if (this.hierarchy == null)
			{
				return;
			}
			this.hierarchy.RegisterBone(this);
			this.hierarchy.OnReset += this.HandleOnReset;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00075778 File Offset: 0x00073978
		private void HandleOnReset()
		{
			this.Reset();
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00075780 File Offset: 0x00073980
		private void OnDisable()
		{
			if (this.hierarchy != null)
			{
				this.hierarchy.OnReset -= this.HandleOnReset;
				this.hierarchy.UnregisterBone(this);
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x000757C4 File Offset: 0x000739C4
		public void DoUpdate(SkeletonUtilityBone.UpdatePhase phase)
		{
			if (!this.valid)
			{
				this.Reset();
				return;
			}
			Skeleton skeleton = this.hierarchy.Skeleton;
			if (this.bone == null)
			{
				if (string.IsNullOrEmpty(this.boneName))
				{
					return;
				}
				this.bone = skeleton.FindBone(this.boneName);
				if (this.bone == null)
				{
					Debug.LogError("Bone not found: " + this.boneName, this);
					return;
				}
			}
			if (!this.bone.Active)
			{
				return;
			}
			float positionScale = this.hierarchy.PositionScale;
			Transform transform = this.cachedTransform;
			float num = Mathf.Sign(skeleton.ScaleX * skeleton.ScaleY);
			if (this.mode == SkeletonUtilityBone.Mode.Follow)
			{
				switch (phase)
				{
				case SkeletonUtilityBone.UpdatePhase.Local:
					if (this.position)
					{
						transform.localPosition = new Vector3(this.bone.x * positionScale, this.bone.y * positionScale, 0f);
					}
					if (this.rotation)
					{
						if (this.bone.data.transformMode.InheritsRotation())
						{
							transform.localRotation = Quaternion.Euler(0f, 0f, this.bone.rotation);
						}
						else
						{
							Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
							transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + this.bone.WorldRotationX * num);
						}
					}
					if (this.scale)
					{
						transform.localScale = new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
						this.incompatibleTransformMode = SkeletonUtilityBone.BoneTransformModeIncompatible(this.bone);
					}
					break;
				case SkeletonUtilityBone.UpdatePhase.World:
				case SkeletonUtilityBone.UpdatePhase.Complete:
					if (!this.bone.appliedValid)
					{
						this.bone.UpdateAppliedTransform();
					}
					if (this.position)
					{
						transform.localPosition = new Vector3(this.bone.ax * positionScale, this.bone.ay * positionScale, 0f);
					}
					if (this.rotation)
					{
						if (this.bone.data.transformMode.InheritsRotation())
						{
							transform.localRotation = Quaternion.Euler(0f, 0f, this.bone.AppliedRotation);
						}
						else
						{
							Vector3 eulerAngles2 = this.skeletonTransform.rotation.eulerAngles;
							transform.rotation = Quaternion.Euler(eulerAngles2.x, eulerAngles2.y, eulerAngles2.z + this.bone.WorldRotationX * num);
						}
					}
					if (this.scale)
					{
						transform.localScale = new Vector3(this.bone.ascaleX, this.bone.ascaleY, 1f);
						this.incompatibleTransformMode = SkeletonUtilityBone.BoneTransformModeIncompatible(this.bone);
					}
					break;
				}
			}
			else if (this.mode == SkeletonUtilityBone.Mode.Override)
			{
				if (this.transformLerpComplete)
				{
					return;
				}
				if (this.parentReference == null)
				{
					if (this.position)
					{
						Vector3 vector = transform.localPosition / positionScale;
						this.bone.x = Mathf.Lerp(this.bone.x, vector.x, this.overrideAlpha);
						this.bone.y = Mathf.Lerp(this.bone.y, vector.y, this.overrideAlpha);
					}
					if (this.rotation)
					{
						float appliedRotation = Mathf.LerpAngle(this.bone.Rotation, transform.localRotation.eulerAngles.z, this.overrideAlpha);
						this.bone.Rotation = appliedRotation;
						this.bone.AppliedRotation = appliedRotation;
					}
					if (this.scale)
					{
						Vector3 localScale = transform.localScale;
						this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, localScale.x, this.overrideAlpha);
						this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, localScale.y, this.overrideAlpha);
					}
				}
				else
				{
					if (this.transformLerpComplete)
					{
						return;
					}
					if (this.position)
					{
						Vector3 vector2 = this.parentReference.InverseTransformPoint(transform.position) / positionScale;
						this.bone.x = Mathf.Lerp(this.bone.x, vector2.x, this.overrideAlpha);
						this.bone.y = Mathf.Lerp(this.bone.y, vector2.y, this.overrideAlpha);
					}
					if (this.rotation)
					{
						float appliedRotation2 = Mathf.LerpAngle(this.bone.Rotation, Quaternion.LookRotation(Vector3.forward, this.parentReference.InverseTransformDirection(transform.up)).eulerAngles.z, this.overrideAlpha);
						this.bone.Rotation = appliedRotation2;
						this.bone.AppliedRotation = appliedRotation2;
					}
					if (this.scale)
					{
						Vector3 localScale2 = transform.localScale;
						this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, localScale2.x, this.overrideAlpha);
						this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, localScale2.y, this.overrideAlpha);
					}
					this.incompatibleTransformMode = SkeletonUtilityBone.BoneTransformModeIncompatible(this.bone);
				}
				this.transformLerpComplete = true;
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00075D84 File Offset: 0x00073F84
		public static bool BoneTransformModeIncompatible(Bone bone)
		{
			return !bone.data.transformMode.InheritsScale();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00075D9C File Offset: 0x00073F9C
		public void AddBoundingBox(string skinName, string slotName, string attachmentName)
		{
			SkeletonUtility.AddBoneRigidbody2D(base.transform.gameObject, true, 0f);
			SkeletonUtility.AddBoundingBoxGameObject(this.bone.skeleton, skinName, slotName, attachmentName, base.transform, true);
		}

		// Token: 0x04000DB6 RID: 3510
		public string boneName;

		// Token: 0x04000DB7 RID: 3511
		public Transform parentReference;

		// Token: 0x04000DB8 RID: 3512
		public SkeletonUtilityBone.Mode mode;

		// Token: 0x04000DB9 RID: 3513
		public bool position;

		// Token: 0x04000DBA RID: 3514
		public bool rotation;

		// Token: 0x04000DBB RID: 3515
		public bool scale;

		// Token: 0x04000DBC RID: 3516
		public bool zPosition = true;

		// Token: 0x04000DBD RID: 3517
		[Range(0f, 1f)]
		public float overrideAlpha = 1f;

		// Token: 0x04000DBE RID: 3518
		public SkeletonUtility hierarchy;

		// Token: 0x04000DBF RID: 3519
		[NonSerialized]
		public Bone bone;

		// Token: 0x04000DC0 RID: 3520
		[NonSerialized]
		public bool transformLerpComplete;

		// Token: 0x04000DC1 RID: 3521
		[NonSerialized]
		public bool valid;

		// Token: 0x04000DC2 RID: 3522
		private Transform cachedTransform;

		// Token: 0x04000DC3 RID: 3523
		private Transform skeletonTransform;

		// Token: 0x04000DC4 RID: 3524
		private bool incompatibleTransformMode;

		// Token: 0x02000200 RID: 512
		public enum Mode
		{
			// Token: 0x04000DC6 RID: 3526
			Follow,
			// Token: 0x04000DC7 RID: 3527
			Override
		}

		// Token: 0x02000201 RID: 513
		public enum UpdatePhase
		{
			// Token: 0x04000DC9 RID: 3529
			Local,
			// Token: 0x04000DCA RID: 3530
			World,
			// Token: 0x04000DCB RID: 3531
			Complete
		}
	}
}
