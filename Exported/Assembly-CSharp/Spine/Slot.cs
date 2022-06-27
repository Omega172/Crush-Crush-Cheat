using System;

namespace Spine
{
	// Token: 0x020001CC RID: 460
	public class Slot
	{
		// Token: 0x06000EBE RID: 3774 RVA: 0x0006AFDC File Offset: 0x000691DC
		public Slot(SlotData data, Bone bone)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone", "bone cannot be null.");
			}
			this.data = data;
			this.bone = bone;
			if (data.hasSecondColor)
			{
				this.r2 = (this.g2 = (this.b2 = 0f));
			}
			this.SetToSetupPose();
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0006B064 File Offset: 0x00069264
		public Slot(Slot slot, Bone bone)
		{
			if (slot == null)
			{
				throw new ArgumentNullException("slot", "slot cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone", "bone cannot be null.");
			}
			this.data = slot.data;
			this.bone = bone;
			this.r = slot.r;
			this.g = slot.g;
			this.b = slot.b;
			this.a = slot.a;
			if (slot.hasSecondColor)
			{
				this.r2 = slot.r2;
				this.g2 = slot.g2;
				this.b2 = slot.b2;
			}
			else
			{
				this.r2 = (this.g2 = (this.b2 = 0f));
			}
			this.hasSecondColor = slot.hasSecondColor;
			this.attachment = slot.attachment;
			this.attachmentTime = slot.attachmentTime;
			this.deform.AddRange(slot.deform);
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0006B178 File Offset: 0x00069378
		public SlotData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0006B180 File Offset: 0x00069380
		public Bone Bone
		{
			get
			{
				return this.bone;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0006B188 File Offset: 0x00069388
		public Skeleton Skeleton
		{
			get
			{
				return this.bone.skeleton;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0006B198 File Offset: 0x00069398
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x0006B1A0 File Offset: 0x000693A0
		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0006B1AC File Offset: 0x000693AC
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0006B1B4 File Offset: 0x000693B4
		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0006B1C0 File Offset: 0x000693C0
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0006B1C8 File Offset: 0x000693C8
		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0006B1D4 File Offset: 0x000693D4
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0006B1DC File Offset: 0x000693DC
		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0006B1E8 File Offset: 0x000693E8
		public void ClampColor()
		{
			this.r = MathUtils.Clamp(this.r, 0f, 1f);
			this.g = MathUtils.Clamp(this.g, 0f, 1f);
			this.b = MathUtils.Clamp(this.b, 0f, 1f);
			this.a = MathUtils.Clamp(this.a, 0f, 1f);
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0006B264 File Offset: 0x00069464
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x0006B26C File Offset: 0x0006946C
		public float R2
		{
			get
			{
				return this.r2;
			}
			set
			{
				this.r2 = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0006B278 File Offset: 0x00069478
		// (set) Token: 0x06000ECF RID: 3791 RVA: 0x0006B280 File Offset: 0x00069480
		public float G2
		{
			get
			{
				return this.g2;
			}
			set
			{
				this.g2 = value;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0006B28C File Offset: 0x0006948C
		// (set) Token: 0x06000ED1 RID: 3793 RVA: 0x0006B294 File Offset: 0x00069494
		public float B2
		{
			get
			{
				return this.b2;
			}
			set
			{
				this.b2 = value;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0006B2A0 File Offset: 0x000694A0
		// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x0006B2B0 File Offset: 0x000694B0
		public bool HasSecondColor
		{
			get
			{
				return this.data.hasSecondColor;
			}
			set
			{
				this.data.hasSecondColor = value;
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0006B2C0 File Offset: 0x000694C0
		public void ClampSecondColor()
		{
			this.r2 = MathUtils.Clamp(this.r2, 0f, 1f);
			this.g2 = MathUtils.Clamp(this.g2, 0f, 1f);
			this.b2 = MathUtils.Clamp(this.b2, 0f, 1f);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0006B320 File Offset: 0x00069520
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0006B328 File Offset: 0x00069528
		public Attachment Attachment
		{
			get
			{
				return this.attachment;
			}
			set
			{
				if (this.attachment == value)
				{
					return;
				}
				this.attachment = value;
				this.attachmentTime = this.bone.skeleton.time;
				this.deform.Clear(false);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0006B36C File Offset: 0x0006956C
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0006B388 File Offset: 0x00069588
		public float AttachmentTime
		{
			get
			{
				return this.bone.skeleton.time - this.attachmentTime;
			}
			set
			{
				this.attachmentTime = this.bone.skeleton.time - value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0006B3A4 File Offset: 0x000695A4
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0006B3AC File Offset: 0x000695AC
		public ExposedList<float> Deform
		{
			get
			{
				return this.deform;
			}
			set
			{
				if (this.deform == null)
				{
					throw new ArgumentNullException("deform", "deform cannot be null.");
				}
				this.deform = value;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0006B3DC File Offset: 0x000695DC
		public void SetToSetupPose()
		{
			this.r = this.data.r;
			this.g = this.data.g;
			this.b = this.data.b;
			this.a = this.data.a;
			if (this.HasSecondColor)
			{
				this.r2 = this.data.r2;
				this.g2 = this.data.g2;
				this.b2 = this.data.b2;
			}
			if (this.data.attachmentName == null)
			{
				this.Attachment = null;
			}
			else
			{
				this.attachment = null;
				this.Attachment = this.bone.skeleton.GetAttachment(this.data.index, this.data.attachmentName);
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0006B4BC File Offset: 0x000696BC
		public override string ToString()
		{
			return this.data.name;
		}

		// Token: 0x04000C5C RID: 3164
		internal SlotData data;

		// Token: 0x04000C5D RID: 3165
		internal Bone bone;

		// Token: 0x04000C5E RID: 3166
		internal float r;

		// Token: 0x04000C5F RID: 3167
		internal float g;

		// Token: 0x04000C60 RID: 3168
		internal float b;

		// Token: 0x04000C61 RID: 3169
		internal float a;

		// Token: 0x04000C62 RID: 3170
		internal float r2;

		// Token: 0x04000C63 RID: 3171
		internal float g2;

		// Token: 0x04000C64 RID: 3172
		internal float b2;

		// Token: 0x04000C65 RID: 3173
		internal bool hasSecondColor;

		// Token: 0x04000C66 RID: 3174
		internal Attachment attachment;

		// Token: 0x04000C67 RID: 3175
		internal float attachmentTime;

		// Token: 0x04000C68 RID: 3176
		internal ExposedList<float> deform = new ExposedList<float>();

		// Token: 0x04000C69 RID: 3177
		internal int attachmentState;
	}
}
