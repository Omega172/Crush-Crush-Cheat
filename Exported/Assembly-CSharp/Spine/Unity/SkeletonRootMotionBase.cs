using System;
using System.Collections.Generic;
using Spine.Unity.AnimationTools;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E6 RID: 486
	public abstract class SkeletonRootMotionBase : MonoBehaviour
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0006FA68 File Offset: 0x0006DC68
		public bool UsesRigidbody
		{
			get
			{
				return this.rigidBody != null || this.rigidBody2D != null;
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0006FA98 File Offset: 0x0006DC98
		protected virtual void Reset()
		{
			this.FindRigidbodyComponent();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0006FAA0 File Offset: 0x0006DCA0
		protected virtual void Start()
		{
			this.skeletonComponent = base.GetComponent<ISkeletonComponent>();
			this.GatherTopLevelBones();
			this.SetRootMotionBone(this.rootMotionBoneName);
			ISkeletonAnimation skeletonAnimation = this.skeletonComponent as ISkeletonAnimation;
			if (skeletonAnimation != null)
			{
				skeletonAnimation.UpdateLocal += this.HandleUpdateLocal;
			}
		}

		// Token: 0x06000FBB RID: 4027
		protected abstract Vector2 CalculateAnimationsMovementDelta();

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0006FAF0 File Offset: 0x0006DCF0
		protected virtual float AdditionalScale
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0006FAF8 File Offset: 0x0006DCF8
		protected Vector2 GetTimelineMovementDelta(float startTime, float endTime, TranslateTimeline timeline, Animation animation)
		{
			Vector2 result;
			if (startTime > endTime)
			{
				result = timeline.Evaluate(animation.duration, null) - timeline.Evaluate(startTime, null) + (timeline.Evaluate(endTime, null) - timeline.Evaluate(0f, null));
			}
			else if (startTime != endTime)
			{
				result = timeline.Evaluate(endTime, null) - timeline.Evaluate(startTime, null);
			}
			else
			{
				result = Vector2.zero;
			}
			return result;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0006FB74 File Offset: 0x0006DD74
		private void GatherTopLevelBones()
		{
			this.topLevelBones.Clear();
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			foreach (Bone bone in skeleton.Bones)
			{
				if (bone.Parent == null)
				{
					this.topLevelBones.Add(bone);
				}
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0006FC04 File Offset: 0x0006DE04
		public void SetRootMotionBone(string name)
		{
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			int num = skeleton.FindBoneIndex(name);
			if (num >= 0)
			{
				this.rootMotionBoneIndex = num;
				this.rootMotionBone = skeleton.bones.Items[num];
			}
			else
			{
				Debug.Log("Bone named \"" + name + "\" could not be found.");
				this.rootMotionBoneIndex = 0;
				this.rootMotionBone = skeleton.RootBone;
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0006FC74 File Offset: 0x0006DE74
		private void HandleUpdateLocal(ISkeletonAnimation animatedSkeletonComponent)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			Vector2 localDelta = this.CalculateAnimationsMovementDelta();
			this.AdjustMovementDeltaToConfiguration(ref localDelta, animatedSkeletonComponent.Skeleton);
			this.ApplyRootMotion(localDelta);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0006FCAC File Offset: 0x0006DEAC
		private void AdjustMovementDeltaToConfiguration(ref Vector2 localDelta, Skeleton skeleton)
		{
			if (skeleton.ScaleX < 0f)
			{
				localDelta.x = -localDelta.x;
			}
			if (skeleton.ScaleY < 0f)
			{
				localDelta.y = -localDelta.y;
			}
			if (!this.transformPositionX)
			{
				localDelta.x = 0f;
			}
			if (!this.transformPositionY)
			{
				localDelta.y = 0f;
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0006FD20 File Offset: 0x0006DF20
		private void ApplyRootMotion(Vector2 localDelta)
		{
			localDelta *= this.AdditionalScale;
			if (this.UsesRigidbody)
			{
				this.rigidbodyDisplacement += base.transform.TransformVector(localDelta);
			}
			else
			{
				base.transform.position += base.transform.TransformVector(localDelta);
			}
			foreach (Bone bone in this.topLevelBones)
			{
				if (this.transformPositionX)
				{
					bone.x -= this.rootMotionBone.x;
				}
				if (this.transformPositionY)
				{
					bone.y -= this.rootMotionBone.y;
				}
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0006FE34 File Offset: 0x0006E034
		protected virtual void FixedUpdate()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.rigidBody2D != null)
			{
				this.rigidBody2D.MovePosition(new Vector2(base.transform.position.x, base.transform.position.y) + this.rigidbodyDisplacement);
			}
			if (this.rigidBody != null)
			{
				this.rigidBody.MovePosition(base.transform.position + new Vector3(this.rigidbodyDisplacement.x, this.rigidbodyDisplacement.y, 0f));
			}
			this.rigidbodyDisplacement = Vector2.zero;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0006FEF8 File Offset: 0x0006E0F8
		protected virtual void OnDisable()
		{
			this.rigidbodyDisplacement = Vector2.zero;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0006FF08 File Offset: 0x0006E108
		protected void FindRigidbodyComponent()
		{
			this.rigidBody2D = base.GetComponent<Rigidbody2D>();
			if (!this.rigidBody2D)
			{
				this.rigidBody = base.GetComponent<Rigidbody>();
			}
			if (!this.rigidBody2D && !this.rigidBody)
			{
				this.rigidBody2D = base.GetComponentInParent<Rigidbody2D>();
				if (!this.rigidBody2D)
				{
					this.rigidBody = base.GetComponentInParent<Rigidbody>();
				}
			}
		}

		// Token: 0x04000CF7 RID: 3319
		[SpineBone("", "", true, false)]
		[SerializeField]
		protected string rootMotionBoneName = "root";

		// Token: 0x04000CF8 RID: 3320
		public bool transformPositionX = true;

		// Token: 0x04000CF9 RID: 3321
		public bool transformPositionY = true;

		// Token: 0x04000CFA RID: 3322
		[Header("Optional")]
		public Rigidbody2D rigidBody2D;

		// Token: 0x04000CFB RID: 3323
		public Rigidbody rigidBody;

		// Token: 0x04000CFC RID: 3324
		protected ISkeletonComponent skeletonComponent;

		// Token: 0x04000CFD RID: 3325
		protected Bone rootMotionBone;

		// Token: 0x04000CFE RID: 3326
		protected int rootMotionBoneIndex;

		// Token: 0x04000CFF RID: 3327
		protected List<Bone> topLevelBones = new List<Bone>();

		// Token: 0x04000D00 RID: 3328
		protected Vector2 rigidbodyDisplacement;
	}
}
