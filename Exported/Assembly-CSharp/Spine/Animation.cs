using System;
using System.Collections.Generic;

namespace Spine
{
	// Token: 0x0200016D RID: 365
	public class Animation
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x000550E0 File Offset: 0x000532E0
		public Animation(string name, ExposedList<Timeline> timelines, float duration)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			if (timelines == null)
			{
				throw new ArgumentNullException("timelines", "timelines cannot be null.");
			}
			int[] array = new int[timelines.Count];
			for (int i = 0; i < timelines.Count; i++)
			{
				array[i] = timelines.Items[i].PropertyId;
			}
			this.timelineIds = new HashSet<int>(array);
			this.name = name;
			this.timelines = timelines;
			this.duration = duration;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00055174 File Offset: 0x00053374
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x0005517C File Offset: 0x0005337C
		public ExposedList<Timeline> Timelines
		{
			get
			{
				return this.timelines;
			}
			set
			{
				this.timelines = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00055188 File Offset: 0x00053388
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00055190 File Offset: 0x00053390
		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0005519C File Offset: 0x0005339C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000551A4 File Offset: 0x000533A4
		public bool HasTimeline(int id)
		{
			return this.timelineIds.Contains(id);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000551B4 File Offset: 0x000533B4
		public void Apply(Skeleton skeleton, float lastTime, float time, bool loop, ExposedList<Event> events, float alpha, MixBlend blend, MixDirection direction)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			if (loop && this.duration != 0f)
			{
				time %= this.duration;
				if (lastTime > 0f)
				{
					lastTime %= this.duration;
				}
			}
			ExposedList<Timeline> exposedList = this.timelines;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				exposedList.Items[i].Apply(skeleton, lastTime, time, events, alpha, blend, direction);
				i++;
			}
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00055248 File Offset: 0x00053448
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00055250 File Offset: 0x00053450
		internal static int BinarySearch(float[] values, float target, int step)
		{
			int num = 0;
			int num2 = values.Length / step - 2;
			if (num2 == 0)
			{
				return step;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[(num3 + 1) * step] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return (num + 1) * step;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000552A4 File Offset: 0x000534A4
		internal static int BinarySearch(float[] values, float target)
		{
			int num = 0;
			int num2 = values.Length - 2;
			if (num2 == 0)
			{
				return 1;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[num3 + 1] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return num + 1;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000552F0 File Offset: 0x000534F0
		internal static int LinearSearch(float[] values, float target, int step)
		{
			int i = 0;
			int num = values.Length - step;
			while (i <= num)
			{
				if (values[i] > target)
				{
					return i;
				}
				i += step;
			}
			return -1;
		}

		// Token: 0x040009FA RID: 2554
		internal string name;

		// Token: 0x040009FB RID: 2555
		internal ExposedList<Timeline> timelines;

		// Token: 0x040009FC RID: 2556
		internal HashSet<int> timelineIds;

		// Token: 0x040009FD RID: 2557
		internal float duration;
	}
}
