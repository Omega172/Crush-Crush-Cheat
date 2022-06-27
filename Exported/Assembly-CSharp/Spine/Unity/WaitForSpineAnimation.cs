using System;
using System.Collections;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x0200022E RID: 558
	public class WaitForSpineAnimation : IEnumerator
	{
		// Token: 0x060011B1 RID: 4529 RVA: 0x0007D7A4 File Offset: 0x0007B9A4
		public WaitForSpineAnimation(TrackEntry trackEntry, WaitForSpineAnimation.AnimationEventTypes eventsToWaitFor)
		{
			this.SafeSubscribe(trackEntry, eventsToWaitFor);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0007D7B4 File Offset: 0x0007B9B4
		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0007D7CC File Offset: 0x0007B9CC
		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0007D7D8 File Offset: 0x0007B9D8
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0007D7DC File Offset: 0x0007B9DC
		public WaitForSpineAnimation NowWaitFor(TrackEntry trackEntry, WaitForSpineAnimation.AnimationEventTypes eventsToWaitFor)
		{
			this.SafeSubscribe(trackEntry, eventsToWaitFor);
			return this;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0007D7E8 File Offset: 0x0007B9E8
		protected void SafeSubscribe(TrackEntry trackEntry, WaitForSpineAnimation.AnimationEventTypes eventsToWaitFor)
		{
			if (trackEntry == null)
			{
				Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
			}
			else
			{
				if ((eventsToWaitFor & WaitForSpineAnimation.AnimationEventTypes.Start) != (WaitForSpineAnimation.AnimationEventTypes)0)
				{
					trackEntry.Start += this.HandleComplete;
				}
				if ((eventsToWaitFor & WaitForSpineAnimation.AnimationEventTypes.Interrupt) != (WaitForSpineAnimation.AnimationEventTypes)0)
				{
					trackEntry.Interrupt += this.HandleComplete;
				}
				if ((eventsToWaitFor & WaitForSpineAnimation.AnimationEventTypes.End) != (WaitForSpineAnimation.AnimationEventTypes)0)
				{
					trackEntry.End += this.HandleComplete;
				}
				if ((eventsToWaitFor & WaitForSpineAnimation.AnimationEventTypes.Dispose) != (WaitForSpineAnimation.AnimationEventTypes)0)
				{
					trackEntry.Dispose += this.HandleComplete;
				}
				if ((eventsToWaitFor & WaitForSpineAnimation.AnimationEventTypes.Complete) != (WaitForSpineAnimation.AnimationEventTypes)0)
				{
					trackEntry.Complete += this.HandleComplete;
				}
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0007D894 File Offset: 0x0007BA94
		private void HandleComplete(TrackEntry trackEntry)
		{
			this.m_WasFired = true;
		}

		// Token: 0x04000E40 RID: 3648
		private bool m_WasFired;

		// Token: 0x0200022F RID: 559
		[Flags]
		public enum AnimationEventTypes
		{
			// Token: 0x04000E42 RID: 3650
			Start = 1,
			// Token: 0x04000E43 RID: 3651
			Interrupt = 2,
			// Token: 0x04000E44 RID: 3652
			End = 4,
			// Token: 0x04000E45 RID: 3653
			Dispose = 8,
			// Token: 0x04000E46 RID: 3654
			Complete = 16
		}
	}
}
