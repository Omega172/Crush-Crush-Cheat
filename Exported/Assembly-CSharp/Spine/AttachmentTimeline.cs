using System;

namespace Spine
{
	// Token: 0x0200017B RID: 379
	public class AttachmentTimeline : Timeline, ISlotTimeline
	{
		// Token: 0x06000AD2 RID: 2770 RVA: 0x00056A30 File Offset: 0x00054C30
		public AttachmentTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.attachmentNames = new string[frameCount];
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00056A50 File Offset: 0x00054C50
		public int PropertyId
		{
			get
			{
				return 67108864 + this.slotIndex;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00056A60 File Offset: 0x00054C60
		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00056A88 File Offset: 0x00054C88
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x00056A6C File Offset: 0x00054C6C
		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.slotIndex = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00056A90 File Offset: 0x00054C90
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x00056A98 File Offset: 0x00054C98
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

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00056AA4 File Offset: 0x00054CA4
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x00056AAC File Offset: 0x00054CAC
		public string[] AttachmentNames
		{
			get
			{
				return this.attachmentNames;
			}
			set
			{
				this.attachmentNames = value;
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00056AB8 File Offset: 0x00054CB8
		public void SetFrame(int frameIndex, float time, string attachmentName)
		{
			this.frames[frameIndex] = time;
			this.attachmentNames[frameIndex] = attachmentName;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00056ACC File Offset: 0x00054CCC
		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			if (!slot.bone.active)
			{
				return;
			}
			if (direction == MixDirection.Out)
			{
				if (blend == MixBlend.Setup)
				{
					this.SetAttachment(skeleton, slot, slot.data.attachmentName);
				}
				return;
			}
			float[] array = this.frames;
			if (time < array[0])
			{
				if (blend == MixBlend.Setup || blend == MixBlend.First)
				{
					this.SetAttachment(skeleton, slot, slot.data.attachmentName);
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
			this.SetAttachment(skeleton, slot, this.attachmentNames[num]);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00056B88 File Offset: 0x00054D88
		private void SetAttachment(Skeleton skeleton, Slot slot, string attachmentName)
		{
			slot.Attachment = ((attachmentName != null) ? skeleton.GetAttachment(this.slotIndex, attachmentName) : null);
		}

		// Token: 0x04000A47 RID: 2631
		internal int slotIndex;

		// Token: 0x04000A48 RID: 2632
		internal float[] frames;

		// Token: 0x04000A49 RID: 2633
		internal string[] attachmentNames;
	}
}
