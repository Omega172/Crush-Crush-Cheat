using System;

namespace Spine
{
	// Token: 0x0200019B RID: 411
	public class ClippingAttachment : VertexAttachment
	{
		// Token: 0x06000BDB RID: 3035 RVA: 0x0005B2A4 File Offset: 0x000594A4
		public ClippingAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0005B2B0 File Offset: 0x000594B0
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0005B2B8 File Offset: 0x000594B8
		public SlotData EndSlot
		{
			get
			{
				return this.endSlot;
			}
			set
			{
				this.endSlot = value;
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0005B2C4 File Offset: 0x000594C4
		public override Attachment Copy()
		{
			ClippingAttachment clippingAttachment = new ClippingAttachment(base.Name);
			base.CopyTo(clippingAttachment);
			clippingAttachment.endSlot = this.endSlot;
			return clippingAttachment;
		}

		// Token: 0x04000B09 RID: 2825
		internal SlotData endSlot;
	}
}
