using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E7 RID: 487
	[AddComponentMenu("Spine/SkeletonAnimation")]
	[ExecuteInEditMode]
	public class SkeletonAnimation : SkeletonRenderer, ISkeletonAnimation, IAnimationStateComponent
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000FC7 RID: 4039 RVA: 0x0006FFA4 File Offset: 0x0006E1A4
		// (remove) Token: 0x06000FC8 RID: 4040 RVA: 0x0006FFC0 File Offset: 0x0006E1C0
		protected event UpdateBonesDelegate _UpdateLocal;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000FC9 RID: 4041 RVA: 0x0006FFDC File Offset: 0x0006E1DC
		// (remove) Token: 0x06000FCA RID: 4042 RVA: 0x0006FFF8 File Offset: 0x0006E1F8
		protected event UpdateBonesDelegate _UpdateWorld;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000FCB RID: 4043 RVA: 0x00070014 File Offset: 0x0006E214
		// (remove) Token: 0x06000FCC RID: 4044 RVA: 0x00070030 File Offset: 0x0006E230
		protected event UpdateBonesDelegate _UpdateComplete;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000FCD RID: 4045 RVA: 0x0007004C File Offset: 0x0006E24C
		// (remove) Token: 0x06000FCE RID: 4046 RVA: 0x00070068 File Offset: 0x0006E268
		public event UpdateBonesDelegate UpdateLocal
		{
			add
			{
				this._UpdateLocal = (UpdateBonesDelegate)Delegate.Combine(this._UpdateLocal, value);
			}
			remove
			{
				this._UpdateLocal = (UpdateBonesDelegate)Delegate.Remove(this._UpdateLocal, value);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000FCF RID: 4047 RVA: 0x00070084 File Offset: 0x0006E284
		// (remove) Token: 0x06000FD0 RID: 4048 RVA: 0x000700A0 File Offset: 0x0006E2A0
		public event UpdateBonesDelegate UpdateWorld
		{
			add
			{
				this._UpdateWorld = (UpdateBonesDelegate)Delegate.Combine(this._UpdateWorld, value);
			}
			remove
			{
				this._UpdateWorld = (UpdateBonesDelegate)Delegate.Remove(this._UpdateWorld, value);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000FD1 RID: 4049 RVA: 0x000700BC File Offset: 0x0006E2BC
		// (remove) Token: 0x06000FD2 RID: 4050 RVA: 0x000700D8 File Offset: 0x0006E2D8
		public event UpdateBonesDelegate UpdateComplete
		{
			add
			{
				this._UpdateComplete = (UpdateBonesDelegate)Delegate.Combine(this._UpdateComplete, value);
			}
			remove
			{
				this._UpdateComplete = (UpdateBonesDelegate)Delegate.Remove(this._UpdateComplete, value);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x000700F4 File Offset: 0x0006E2F4
		public AnimationState AnimationState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x000700FC File Offset: 0x0006E2FC
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x00070140 File Offset: 0x0006E340
		public string AnimationName
		{
			get
			{
				if (!this.valid)
				{
					return this._animationName;
				}
				TrackEntry current = this.state.GetCurrent(0);
				return (current != null) ? current.Animation.Name : null;
			}
			set
			{
				if (this._animationName == value)
				{
					TrackEntry current = this.state.GetCurrent(0);
					if (current != null && current.loop == this.loop)
					{
						return;
					}
				}
				this._animationName = value;
				if (string.IsNullOrEmpty(value))
				{
					this.state.ClearTrack(0);
				}
				else
				{
					Animation animation = this.skeletonDataAsset.GetSkeletonData(false).FindAnimation(value);
					if (animation != null)
					{
						this.state.SetAnimation(0, animation, this.loop);
					}
				}
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000701D4 File Offset: 0x0006E3D4
		public static SkeletonAnimation AddToGameObject(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
		{
			return SkeletonRenderer.AddSpineComponent<SkeletonAnimation>(gameObject, skeletonDataAsset);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000701E0 File Offset: 0x0006E3E0
		public static SkeletonAnimation NewSkeletonAnimationGameObject(SkeletonDataAsset skeletonDataAsset)
		{
			return SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(skeletonDataAsset);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000701E8 File Offset: 0x0006E3E8
		public override void ClearState()
		{
			base.ClearState();
			if (this.state != null)
			{
				this.state.ClearTracks();
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00070208 File Offset: 0x0006E408
		public override void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			base.Initialize(overwrite);
			if (!this.valid)
			{
				return;
			}
			this.state = new AnimationState(this.skeletonDataAsset.GetAnimationStateData());
			this.wasUpdatedAfterInit = false;
			if (!string.IsNullOrEmpty(this._animationName))
			{
				Animation animation = this.skeletonDataAsset.GetSkeletonData(false).FindAnimation(this._animationName);
				if (animation != null)
				{
					this.state.SetAnimation(0, animation, this.loop);
				}
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0007029C File Offset: 0x0006E49C
		private void Update()
		{
			this.Update(Time.deltaTime);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x000702AC File Offset: 0x0006E4AC
		public void Update(float deltaTime)
		{
			if (!this.valid || this.state == null)
			{
				return;
			}
			this.wasUpdatedAfterInit = true;
			if (this.updateMode < UpdateMode.OnlyAnimationStatus)
			{
				return;
			}
			this.UpdateAnimationStatus(deltaTime);
			if (this.updateMode == UpdateMode.OnlyAnimationStatus)
			{
				return;
			}
			this.ApplyAnimation();
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00070300 File Offset: 0x0006E500
		protected void UpdateAnimationStatus(float deltaTime)
		{
			deltaTime *= this.timeScale;
			this.skeleton.Update(deltaTime);
			this.state.Update(deltaTime);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00070330 File Offset: 0x0006E530
		protected void ApplyAnimation()
		{
			this.state.Apply(this.skeleton);
			if (this._UpdateLocal != null)
			{
				this._UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this._UpdateWorld != null)
			{
				this._UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this._UpdateComplete != null)
			{
				this._UpdateComplete(this);
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000703AC File Offset: 0x0006E5AC
		public override void LateUpdate()
		{
			if (!this.wasUpdatedAfterInit)
			{
				this.Update(0f);
			}
			base.LateUpdate();
		}

		// Token: 0x04000D01 RID: 3329
		public AnimationState state;

		// Token: 0x04000D02 RID: 3330
		private bool wasUpdatedAfterInit = true;

		// Token: 0x04000D03 RID: 3331
		[SerializeField]
		[SpineAnimation("", "", true, false)]
		private string _animationName;

		// Token: 0x04000D04 RID: 3332
		public bool loop;

		// Token: 0x04000D05 RID: 3333
		public float timeScale = 1f;
	}
}
