using System;

namespace Spine.Unity
{
	// Token: 0x02000214 RID: 532
	public class SkeletonRendererInstruction
	{
		// Token: 0x06001118 RID: 4376 RVA: 0x0007963C File Offset: 0x0007783C
		public void Clear()
		{
			this.attachments.Clear(false);
			this.rawVertexCount = -1;
			this.hasActiveClipping = false;
			this.submeshInstructions.Clear(false);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00079670 File Offset: 0x00077870
		public void Dispose()
		{
			this.attachments.Clear(true);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00079680 File Offset: 0x00077880
		public void SetWithSubset(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
		{
			int num = 0;
			ExposedList<SubmeshInstruction> exposedList = this.submeshInstructions;
			exposedList.Clear(false);
			int num2 = endSubmesh - startSubmesh;
			exposedList.Resize(num2);
			SubmeshInstruction[] items = exposedList.Items;
			SubmeshInstruction[] items2 = instructions.Items;
			for (int i = 0; i < num2; i++)
			{
				SubmeshInstruction submeshInstruction = items2[startSubmesh + i];
				items[i] = submeshInstruction;
				this.hasActiveClipping |= submeshInstruction.hasClipping;
				items[i].rawFirstVertexIndex = num;
				num += submeshInstruction.rawVertexCount;
			}
			this.rawVertexCount = num;
			int startSlot = items2[startSubmesh].startSlot;
			int endSlot = items2[endSubmesh - 1].endSlot;
			this.attachments.Clear(false);
			int num3 = endSlot - startSlot;
			this.attachments.Resize(num3);
			Attachment[] items3 = this.attachments.Items;
			Slot[] items4 = items2[0].skeleton.drawOrder.Items;
			for (int j = 0; j < num3; j++)
			{
				Slot slot = items4[startSlot + j];
				if (slot.bone.active)
				{
					items3[j] = slot.attachment;
				}
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x000797CC File Offset: 0x000779CC
		public void Set(SkeletonRendererInstruction other)
		{
			this.immutableTriangles = other.immutableTriangles;
			this.hasActiveClipping = other.hasActiveClipping;
			this.rawVertexCount = other.rawVertexCount;
			this.attachments.Clear(false);
			this.attachments.EnsureCapacity(other.attachments.Capacity);
			this.attachments.Count = other.attachments.Count;
			other.attachments.CopyTo(this.attachments.Items);
			this.submeshInstructions.Clear(false);
			this.submeshInstructions.EnsureCapacity(other.submeshInstructions.Capacity);
			this.submeshInstructions.Count = other.submeshInstructions.Count;
			other.submeshInstructions.CopyTo(this.submeshInstructions.Items);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0007989C File Offset: 0x00077A9C
		public static bool GeometryNotEqual(SkeletonRendererInstruction a, SkeletonRendererInstruction b)
		{
			if (a.hasActiveClipping || b.hasActiveClipping)
			{
				return true;
			}
			if (a.rawVertexCount != b.rawVertexCount)
			{
				return true;
			}
			if (a.immutableTriangles != b.immutableTriangles)
			{
				return true;
			}
			int count = b.attachments.Count;
			if (a.attachments.Count != count)
			{
				return true;
			}
			int count2 = a.submeshInstructions.Count;
			int count3 = b.submeshInstructions.Count;
			if (count2 != count3)
			{
				return true;
			}
			SubmeshInstruction[] items = a.submeshInstructions.Items;
			SubmeshInstruction[] items2 = b.submeshInstructions.Items;
			Attachment[] items3 = a.attachments.Items;
			Attachment[] items4 = b.attachments.Items;
			for (int i = 0; i < count; i++)
			{
				if (!object.ReferenceEquals(items3[i], items4[i]))
				{
					return true;
				}
			}
			for (int j = 0; j < count3; j++)
			{
				SubmeshInstruction submeshInstruction = items[j];
				SubmeshInstruction submeshInstruction2 = items2[j];
				if (submeshInstruction.rawVertexCount != submeshInstruction2.rawVertexCount || submeshInstruction.startSlot != submeshInstruction2.startSlot || submeshInstruction.endSlot != submeshInstruction2.endSlot || submeshInstruction.rawTriangleCount != submeshInstruction2.rawTriangleCount || submeshInstruction.rawFirstVertexIndex != submeshInstruction2.rawFirstVertexIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000E0B RID: 3595
		public readonly ExposedList<SubmeshInstruction> submeshInstructions = new ExposedList<SubmeshInstruction>();

		// Token: 0x04000E0C RID: 3596
		public bool immutableTriangles;

		// Token: 0x04000E0D RID: 3597
		public bool hasActiveClipping;

		// Token: 0x04000E0E RID: 3598
		public int rawVertexCount = -1;

		// Token: 0x04000E0F RID: 3599
		public readonly ExposedList<Attachment> attachments = new ExposedList<Attachment>();
	}
}
