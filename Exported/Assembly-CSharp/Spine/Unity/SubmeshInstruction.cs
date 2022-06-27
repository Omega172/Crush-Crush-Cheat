using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000216 RID: 534
	public struct SubmeshInstruction
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x00079A50 File Offset: 0x00077C50
		public int SlotCount
		{
			get
			{
				return this.endSlot - this.startSlot;
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00079A60 File Offset: 0x00077C60
		public override string ToString()
		{
			return string.Format("[SubmeshInstruction: slots {0} to {1}. (Material){2}. preActiveClippingSlotSource:{3}]", new object[]
			{
				this.startSlot,
				this.endSlot - 1,
				(!(this.material == null)) ? this.material.name : "<none>",
				this.preActiveClippingSlotSource
			});
		}

		// Token: 0x04000E11 RID: 3601
		public Skeleton skeleton;

		// Token: 0x04000E12 RID: 3602
		public int startSlot;

		// Token: 0x04000E13 RID: 3603
		public int endSlot;

		// Token: 0x04000E14 RID: 3604
		public Material material;

		// Token: 0x04000E15 RID: 3605
		public bool forceSeparate;

		// Token: 0x04000E16 RID: 3606
		public int preActiveClippingSlotSource;

		// Token: 0x04000E17 RID: 3607
		public int rawTriangleCount;

		// Token: 0x04000E18 RID: 3608
		public int rawVertexCount;

		// Token: 0x04000E19 RID: 3609
		public int rawFirstVertexIndex;

		// Token: 0x04000E1A RID: 3610
		public bool hasClipping;
	}
}
