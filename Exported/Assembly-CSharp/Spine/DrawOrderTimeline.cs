using System;

namespace Spine
{
	// Token: 0x0200017E RID: 382
	public class DrawOrderTimeline : Timeline
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x000573CC File Offset: 0x000555CC
		public DrawOrderTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.drawOrders = new int[frameCount][];
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000573EC File Offset: 0x000555EC
		public int PropertyId
		{
			get
			{
				return 134217728;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x000573F4 File Offset: 0x000555F4
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00057400 File Offset: 0x00055600
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x00057408 File Offset: 0x00055608
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

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00057414 File Offset: 0x00055614
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x0005741C File Offset: 0x0005561C
		public int[][] DrawOrders
		{
			get
			{
				return this.drawOrders;
			}
			set
			{
				this.drawOrders = value;
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00057428 File Offset: 0x00055628
		public void SetFrame(int frameIndex, float time, int[] drawOrder)
		{
			this.frames[frameIndex] = time;
			this.drawOrders[frameIndex] = drawOrder;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0005743C File Offset: 0x0005563C
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			ExposedList<Slot> slots = skeleton.slots;
			if (direction == MixDirection.Out)
			{
				if (blend == MixBlend.Setup)
				{
					Array.Copy(slots.Items, 0, drawOrder.Items, 0, slots.Count);
				}
				return;
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				if (blend == MixBlend.Setup || blend == MixBlend.First)
				{
					Array.Copy(slots.Items, 0, drawOrder.Items, 0, slots.Count);
				}
				return;
			}
			int num;
			if (time >= array[array.Length - 1])
			{
				num = array.Length - 1;
			}
			else
			{
				num = Animation.BinarySearch(array, time) - 1;
			}
			int[] array2 = this.drawOrders[num];
			if (array2 == null)
			{
				Array.Copy(slots.Items, 0, drawOrder.Items, 0, slots.Count);
			}
			else
			{
				Slot[] items = drawOrder.Items;
				Slot[] items2 = slots.Items;
				int i = 0;
				int num2 = array2.Length;
				while (i < num2)
				{
					items[i] = items2[array2[i]];
					i++;
				}
			}
		}

		// Token: 0x04000A50 RID: 2640
		internal float[] frames;

		// Token: 0x04000A51 RID: 2641
		private int[][] drawOrders;
	}
}
