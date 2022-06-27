using System;
using System.Collections;

namespace Spine.Unity
{
	// Token: 0x02000230 RID: 560
	public class WaitForSpineAnimationComplete : WaitForSpineAnimation, IEnumerator
	{
		// Token: 0x060011B8 RID: 4536 RVA: 0x0007D8A0 File Offset: 0x0007BAA0
		public WaitForSpineAnimationComplete(TrackEntry trackEntry, bool includeEndEvent = false) : base(trackEntry, (!includeEndEvent) ? WaitForSpineAnimation.AnimationEventTypes.Complete : (WaitForSpineAnimation.AnimationEventTypes.End | WaitForSpineAnimation.AnimationEventTypes.Complete))
		{
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0007D8B8 File Offset: 0x0007BAB8
		public WaitForSpineAnimationComplete NowWaitFor(TrackEntry trackEntry, bool includeEndEvent = false)
		{
			base.SafeSubscribe(trackEntry, (!includeEndEvent) ? WaitForSpineAnimation.AnimationEventTypes.Complete : (WaitForSpineAnimation.AnimationEventTypes.End | WaitForSpineAnimation.AnimationEventTypes.Complete));
			return this;
		}
	}
}
