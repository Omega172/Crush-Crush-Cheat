using System;

namespace Spine
{
	// Token: 0x0200017D RID: 381
	public class EventTimeline : Timeline
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x00057290 File Offset: 0x00055490
		public EventTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.events = new Event[frameCount];
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x000572B0 File Offset: 0x000554B0
		public int PropertyId
		{
			get
			{
				return 117440512;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000572B8 File Offset: 0x000554B8
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000572C4 File Offset: 0x000554C4
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x000572CC File Offset: 0x000554CC
		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000572D8 File Offset: 0x000554D8
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x000572E0 File Offset: 0x000554E0
		public Event[] Events
		{
			get
			{
				return this.events;
			}
			set
			{
				this.events = value;
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000572EC File Offset: 0x000554EC
		public void SetFrame(int frameIndex, Event e)
		{
			this.frames[frameIndex] = e.Time;
			this.events[frameIndex] = e;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00057308 File Offset: 0x00055508
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			if (firedEvents == null)
			{
				return;
			}
			float[] array = this.frames;
			int num = array.Length;
			if (lastTime > time)
			{
				this.Apply(skeleton, lastTime, 2.1474836E+09f, firedEvents, alpha, blend, direction);
				lastTime = -1f;
			}
			else if (lastTime >= array[num - 1])
			{
				return;
			}
			if (time < array[0])
			{
				return;
			}
			int i;
			if (lastTime < array[0])
			{
				i = 0;
			}
			else
			{
				i = Animation.BinarySearch(array, lastTime);
				float num2 = array[i];
				while (i > 0)
				{
					if (array[i - 1] != num2)
					{
						break;
					}
					i--;
				}
			}
			while (i < num && time >= array[i])
			{
				firedEvents.Add(this.events[i]);
				i++;
			}
		}

		// Token: 0x04000A4E RID: 2638
		internal float[] frames;

		// Token: 0x04000A4F RID: 2639
		private Event[] events;
	}
}
