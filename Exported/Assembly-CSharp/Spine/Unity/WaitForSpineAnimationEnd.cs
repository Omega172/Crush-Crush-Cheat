using System;
using System.Collections;

namespace Spine.Unity
{
	// Token: 0x02000231 RID: 561
	public class WaitForSpineAnimationEnd : WaitForSpineAnimation, IEnumerator
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0007D8D4 File Offset: 0x0007BAD4
		public WaitForSpineAnimationEnd(TrackEntry trackEntry) : base(trackEntry, WaitForSpineAnimation.AnimationEventTypes.End)
		{
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0007D8E0 File Offset: 0x0007BAE0
		public WaitForSpineAnimationEnd NowWaitFor(TrackEntry trackEntry)
		{
			base.SafeSubscribe(trackEntry, WaitForSpineAnimation.AnimationEventTypes.End);
			return this;
		}
	}
}
