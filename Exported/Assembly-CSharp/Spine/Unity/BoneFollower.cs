using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spine.Unity
{
	// Token: 0x020001E0 RID: 480
	[AddComponentMenu("Spine/BoneFollower")]
	[ExecuteInEditMode]
	public class BoneFollower : MonoBehaviour
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0006E2AC File Offset: 0x0006C4AC
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x0006E2B4 File Offset: 0x0006C4B4
		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return this.skeletonRenderer;
			}
			set
			{
				this.skeletonRenderer = value;
				this.Initialize();
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0006E2C4 File Offset: 0x0006C4C4
		public bool SetBone(string name)
		{
			this.bone = this.skeletonRenderer.skeleton.FindBone(name);
			if (this.bone == null)
			{
				Debug.LogError("Bone not found: " + name, this);
				return false;
			}
			this.boneName = name;
			return true;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0006E304 File Offset: 0x0006C504
		public void Awake()
		{
			if (this.initializeOnAwake)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0006E318 File Offset: 0x0006C518
		public void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
		{
			this.Initialize();
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0006E320 File Offset: 0x0006C520
		public void Initialize()
		{
			this.bone = null;
			this.valid = (this.skeletonRenderer != null && this.skeletonRenderer.valid);
			if (!this.valid)
			{
				return;
			}
			this.skeletonTransform = this.skeletonRenderer.transform;
			this.skeletonRenderer.OnRebuild -= this.HandleRebuildRenderer;
			this.skeletonRenderer.OnRebuild += this.HandleRebuildRenderer;
			this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
			if (!string.IsNullOrEmpty(this.boneName))
			{
				this.bone = this.skeletonRenderer.skeleton.FindBone(this.boneName);
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0006E3EC File Offset: 0x0006C5EC
		private void OnDestroy()
		{
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRebuildRenderer;
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0006E424 File Offset: 0x0006C624
		public void LateUpdate()
		{
			if (!this.valid)
			{
				this.Initialize();
				return;
			}
			if (this.bone == null)
			{
				if (string.IsNullOrEmpty(this.boneName))
				{
					return;
				}
				this.bone = this.skeletonRenderer.skeleton.FindBone(this.boneName);
				if (!this.SetBone(this.boneName))
				{
					return;
				}
			}
			Transform transform = base.transform;
			if (this.skeletonTransformIsParent)
			{
				transform.localPosition = new Vector3((!this.followXYPosition) ? transform.localPosition.x : this.bone.worldX, (!this.followXYPosition) ? transform.localPosition.y : this.bone.worldY, (!this.followZPosition) ? transform.localPosition.z : 0f);
				if (this.followBoneRotation)
				{
					float num = Mathf.Atan2(this.bone.c, this.bone.a) * 0.5f;
					if (this.followLocalScale && this.bone.scaleX < 0f)
					{
						num += 1.5707964f;
					}
					transform.localRotation = new Quaternion
					{
						z = Mathf.Sin(num),
						w = Mathf.Cos(num)
					};
				}
			}
			else
			{
				Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(this.bone.worldX, this.bone.worldY, 0f));
				if (!this.followZPosition)
				{
					position.z = transform.position.z;
				}
				if (!this.followXYPosition)
				{
					position.x = transform.position.x;
					position.y = transform.position.y;
				}
				float num2 = this.bone.WorldRotationX;
				Transform parent = transform.parent;
				if (parent != null)
				{
					Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
					if (localToWorldMatrix.m00 * localToWorldMatrix.m11 - localToWorldMatrix.m01 * localToWorldMatrix.m10 < 0f)
					{
						num2 = -num2;
					}
				}
				if (this.followBoneRotation)
				{
					Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
					if (this.followLocalScale && this.bone.scaleX < 0f)
					{
						num2 += 180f;
					}
					transform.position = position;
					transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num2);
				}
				else
				{
					transform.position = position;
				}
			}
			Vector3 localScale = (!this.followLocalScale) ? new Vector3(1f, 1f, 1f) : new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
			if (this.followSkeletonFlip)
			{
				localScale.y *= Mathf.Sign(this.bone.skeleton.ScaleX * this.bone.skeleton.ScaleY);
			}
			transform.localScale = localScale;
		}

		// Token: 0x04000CC1 RID: 3265
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000CC2 RID: 3266
		[SpineBone("", "skeletonRenderer", true, false)]
		[SerializeField]
		public string boneName;

		// Token: 0x04000CC3 RID: 3267
		public bool followXYPosition = true;

		// Token: 0x04000CC4 RID: 3268
		public bool followZPosition = true;

		// Token: 0x04000CC5 RID: 3269
		public bool followBoneRotation = true;

		// Token: 0x04000CC6 RID: 3270
		[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
		public bool followSkeletonFlip = true;

		// Token: 0x04000CC7 RID: 3271
		[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
		public bool followLocalScale;

		// Token: 0x04000CC8 RID: 3272
		[FormerlySerializedAs("resetOnAwake")]
		public bool initializeOnAwake = true;

		// Token: 0x04000CC9 RID: 3273
		[NonSerialized]
		public bool valid;

		// Token: 0x04000CCA RID: 3274
		[NonSerialized]
		public Bone bone;

		// Token: 0x04000CCB RID: 3275
		private Transform skeletonTransform;

		// Token: 0x04000CCC RID: 3276
		private bool skeletonTransformIsParent;
	}
}
