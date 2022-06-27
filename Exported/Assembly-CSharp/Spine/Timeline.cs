using System;

namespace Spine
{
	// Token: 0x0200016E RID: 366
	public interface Timeline
	{
		// Token: 0x06000AA0 RID: 2720
		void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> events, float alpha, MixBlend blend, MixDirection direction);

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000AA1 RID: 2721
		int PropertyId { get; }
	}
}
