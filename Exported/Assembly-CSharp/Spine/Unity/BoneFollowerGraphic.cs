using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E1 RID: 481
	[DisallowMultipleComponent]
	[AddComponentMenu("Spine/UI/BoneFollowerGraphic")]
	[ExecuteInEditMode]
	public class BoneFollowerGraphic : MonoBehaviour
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0006E7C0 File Offset: 0x0006C9C0
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0006E7C8 File Offset: 0x0006C9C8
		public SkeletonGraphic SkeletonGraphic
		{
			get
			{
				return this.skeletonGraphic;
			}
			set
			{
				this.skeletonGraphic = value;
				this.Initialize();
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0006E7D8 File Offset: 0x0006C9D8
		public bool SetBone(string name)
		{
			this.bone = this.skeletonGraphic.Skeleton.FindBone(name);
			if (this.bone == null)
			{
				Debug.LogError("Bone not found: " + name, this);
				return false;
			}
			this.boneName = name;
			return true;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0006E824 File Offset: 0x0006CA24
		public void Awake()
		{
			if (this.initializeOnAwake)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0006E838 File Offset: 0x0006CA38
		public void Initialize()
		{
			this.bone = null;
			this.valid = (this.skeletonGraphic != null && this.skeletonGraphic.IsValid);
			if (!this.valid)
			{
				return;
			}
			this.skeletonTransform = this.skeletonGraphic.transform;
			this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
			if (!string.IsNullOrEmpty(this.boneName))
			{
				this.bone = this.skeletonGraphic.Skeleton.FindBone(this.boneName);
			}
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0006E8D8 File Offset: 0x0006CAD8
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
				this.bone = this.skeletonGraphic.Skeleton.FindBone(this.boneName);
				if (!this.SetBone(this.boneName))
				{
					return;
				}
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Canvas canvas = this.skeletonGraphic.canvas;
			if (canvas == null)
			{
				canvas = this.skeletonGraphic.GetComponentInParent<Canvas>();
			}
			float num = (!(canvas != null)) ? 100f : canvas.referencePixelsPerUnit;
			if (this.skeletonTransformIsParent)
			{
				rectTransform.localPosition = new Vector3((!this.followXYPosition) ? rectTransform.localPosition.x : (this.bone.worldX * num), (!this.followXYPosition) ? rectTransform.localPosition.y : (this.bone.worldY * num), (!this.followZPosition) ? rectTransform.localPosition.z : 0f);
				if (this.followBoneRotation)
				{
					rectTransform.localRotation = this.bone.GetQuaternion();
				}
			}
			else
			{
				Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(this.bone.worldX * num, this.bone.worldY * num, 0f));
				if (!this.followZPosition)
				{
					position.z = rectTransform.position.z;
				}
				if (!this.followXYPosition)
				{
					position.x = rectTransform.position.x;
					position.y = rectTransform.position.y;
				}
				float num2 = this.bone.WorldRotationX;
				Transform parent = rectTransform.parent;
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
					rectTransform.position = position;
					rectTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num2);
				}
				else
				{
					rectTransform.position = position;
				}
			}
			Vector3 localScale = (!this.followLocalScale) ? new Vector3(1f, 1f, 1f) : new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
			if (this.followSkeletonFlip)
			{
				localScale.y *= Mathf.Sign(this.bone.skeleton.ScaleX * this.bone.skeleton.ScaleY);
			}
			rectTransform.localScale = localScale;
		}

		// Token: 0x04000CCD RID: 3277
		public SkeletonGraphic skeletonGraphic;

		// Token: 0x04000CCE RID: 3278
		public bool initializeOnAwake = true;

		// Token: 0x04000CCF RID: 3279
		[SerializeField]
		[SpineBone("", "skeletonGraphic", true, false)]
		public string boneName;

		// Token: 0x04000CD0 RID: 3280
		public bool followBoneRotation = true;

		// Token: 0x04000CD1 RID: 3281
		[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
		public bool followSkeletonFlip = true;

		// Token: 0x04000CD2 RID: 3282
		[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
		public bool followLocalScale;

		// Token: 0x04000CD3 RID: 3283
		public bool followXYPosition = true;

		// Token: 0x04000CD4 RID: 3284
		public bool followZPosition = true;

		// Token: 0x04000CD5 RID: 3285
		[NonSerialized]
		public Bone bone;

		// Token: 0x04000CD6 RID: 3286
		private Transform skeletonTransform;

		// Token: 0x04000CD7 RID: 3287
		private bool skeletonTransformIsParent;

		// Token: 0x04000CD8 RID: 3288
		[NonSerialized]
		public bool valid;
	}
}
