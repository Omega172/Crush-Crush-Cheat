using System;
using System.Collections;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000232 RID: 562
	public class WaitForSpineEvent : IEnumerator
	{
		// Token: 0x060011BC RID: 4540 RVA: 0x0007D8EC File Offset: 0x0007BAEC
		public WaitForSpineEvent(AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0007D900 File Offset: 0x0007BB00
		public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			this.Subscribe(skeletonAnimation.state, eventDataReference, unsubscribeAfterFiring);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0007D918 File Offset: 0x0007BB18
		public WaitForSpineEvent(AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
		{
			this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0007D92C File Offset: 0x0007BB2C
		public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, string eventName, bool unsubscribeAfterFiring = true)
		{
			this.SubscribeByName(skeletonAnimation.state, eventName, unsubscribeAfterFiring);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0007D944 File Offset: 0x0007BB44
		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0007D95C File Offset: 0x0007BB5C
		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x0007D968 File Offset: 0x0007BB68
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0007D96C File Offset: 0x0007BB6C
		private void Subscribe(AnimationState state, EventData eventDataReference, bool unsubscribe)
		{
			if (state == null)
			{
				Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			if (eventDataReference == null)
			{
				Debug.LogWarning("eventDataReference argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			this.m_AnimationState = state;
			this.m_TargetEvent = eventDataReference;
			state.Event += this.HandleAnimationStateEvent;
			this.m_unsubscribeAfterFiring = unsubscribe;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0007D9D0 File Offset: 0x0007BBD0
		private void SubscribeByName(AnimationState state, string eventName, bool unsubscribe)
		{
			if (state == null)
			{
				Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			if (string.IsNullOrEmpty(eventName))
			{
				Debug.LogWarning("eventName argument was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
				return;
			}
			this.m_AnimationState = state;
			this.m_EventName = eventName;
			state.Event += this.HandleAnimationStateEventByName;
			this.m_unsubscribeAfterFiring = unsubscribe;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0007DA3C File Offset: 0x0007BC3C
		private void HandleAnimationStateEventByName(TrackEntry trackEntry, Event e)
		{
			this.m_WasFired |= (e.Data.Name == this.m_EventName);
			if (this.m_WasFired && this.m_unsubscribeAfterFiring)
			{
				this.m_AnimationState.Event -= this.HandleAnimationStateEventByName;
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0007DA9C File Offset: 0x0007BC9C
		private void HandleAnimationStateEvent(TrackEntry trackEntry, Event e)
		{
			this.m_WasFired |= (e.Data == this.m_TargetEvent);
			if (this.m_WasFired && this.m_unsubscribeAfterFiring)
			{
				this.m_AnimationState.Event -= this.HandleAnimationStateEvent;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0007DAF4 File Offset: 0x0007BCF4
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x0007DAFC File Offset: 0x0007BCFC
		public bool WillUnsubscribeAfterFiring
		{
			get
			{
				return this.m_unsubscribeAfterFiring;
			}
			set
			{
				this.m_unsubscribeAfterFiring = value;
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0007DB08 File Offset: 0x0007BD08
		public WaitForSpineEvent NowWaitFor(AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
		{
			((IEnumerator)this).Reset();
			this.Clear(state);
			this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
			return this;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0007DB2C File Offset: 0x0007BD2C
		public WaitForSpineEvent NowWaitFor(AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
		{
			((IEnumerator)this).Reset();
			this.Clear(state);
			this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
			return this;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0007DB50 File Offset: 0x0007BD50
		private void Clear(AnimationState state)
		{
			state.Event -= this.HandleAnimationStateEvent;
			state.Event -= this.HandleAnimationStateEventByName;
		}

		// Token: 0x04000E47 RID: 3655
		private EventData m_TargetEvent;

		// Token: 0x04000E48 RID: 3656
		private string m_EventName;

		// Token: 0x04000E49 RID: 3657
		private AnimationState m_AnimationState;

		// Token: 0x04000E4A RID: 3658
		private bool m_WasFired;

		// Token: 0x04000E4B RID: 3659
		private bool m_unsubscribeAfterFiring;
	}
}
