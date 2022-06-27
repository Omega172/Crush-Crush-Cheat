using System;

namespace Spine
{
	// Token: 0x0200022B RID: 555
	public static class SpineSkeletonExtensions
	{
		// Token: 0x06001198 RID: 4504 RVA: 0x0007CB48 File Offset: 0x0007AD48
		public static bool IsWeighted(this VertexAttachment va)
		{
			return va.bones != null && va.bones.Length > 0;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0007CB64 File Offset: 0x0007AD64
		public static bool IsRenderable(this Attachment a)
		{
			return a is IHasRendererObject;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0007CB70 File Offset: 0x0007AD70
		public static bool InheritsRotation(this TransformMode mode)
		{
			return ((long)mode & 1L) == 0L;
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0007CB88 File Offset: 0x0007AD88
		public static bool InheritsScale(this TransformMode mode)
		{
			return ((long)mode & 2L) == 0L;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0007CBA0 File Offset: 0x0007ADA0
		internal static void SetPropertyToSetupPose(this Skeleton skeleton, int propertyID)
		{
			int num = propertyID >> 24;
			TimelineType timelineType = (TimelineType)num;
			int num2 = propertyID - (num << 24);
			switch (timelineType)
			{
			case TimelineType.Rotate:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.rotation = bone.data.rotation;
				break;
			}
			case TimelineType.Translate:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.x = bone.data.x;
				bone.y = bone.data.y;
				break;
			}
			case TimelineType.Scale:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.scaleX = bone.data.scaleX;
				bone.scaleY = bone.data.scaleY;
				break;
			}
			case TimelineType.Shear:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.shearX = bone.data.shearX;
				bone.shearY = bone.data.shearY;
				break;
			}
			case TimelineType.Attachment:
				skeleton.SetSlotAttachmentToSetupPose(num2);
				break;
			case TimelineType.Color:
				skeleton.slots.Items[num2].SetColorToSetupPose();
				break;
			case TimelineType.Deform:
				skeleton.slots.Items[num2].Deform.Clear(true);
				break;
			case TimelineType.DrawOrder:
				skeleton.SetDrawOrderToSetupPose();
				break;
			case TimelineType.IkConstraint:
			{
				IkConstraint ikConstraint = skeleton.ikConstraints.Items[num2];
				ikConstraint.mix = ikConstraint.data.mix;
				ikConstraint.softness = ikConstraint.data.softness;
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.stretch = ikConstraint.data.stretch;
				break;
			}
			case TimelineType.TransformConstraint:
			{
				TransformConstraint transformConstraint = skeleton.transformConstraints.Items[num2];
				TransformConstraintData data = transformConstraint.data;
				transformConstraint.rotateMix = data.rotateMix;
				transformConstraint.translateMix = data.translateMix;
				transformConstraint.scaleMix = data.scaleMix;
				transformConstraint.shearMix = data.shearMix;
				break;
			}
			case TimelineType.PathConstraintPosition:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.position = pathConstraint.data.position;
				break;
			}
			case TimelineType.PathConstraintSpacing:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.spacing = pathConstraint.data.spacing;
				break;
			}
			case TimelineType.PathConstraintMix:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.rotateMix = pathConstraint.data.rotateMix;
				pathConstraint.translateMix = pathConstraint.data.translateMix;
				break;
			}
			case TimelineType.TwoColor:
				skeleton.slots.Items[num2].SetColorToSetupPose();
				break;
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0007CE68 File Offset: 0x0007B068
		public static void SetDrawOrderToSetupPose(this Skeleton skeleton)
		{
			Slot[] items = skeleton.slots.Items;
			int count = skeleton.slots.Count;
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			drawOrder.Clear(false);
			drawOrder.EnsureCapacity(count);
			drawOrder.Count = count;
			Array.Copy(items, drawOrder.Items, count);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0007CEB8 File Offset: 0x0007B0B8
		public static void SetSlotAttachmentsToSetupPose(this Skeleton skeleton)
		{
			Slot[] items = skeleton.slots.Items;
			for (int i = 0; i < skeleton.slots.Count; i++)
			{
				Slot slot = items[i];
				string attachmentName = slot.data.attachmentName;
				slot.Attachment = ((!string.IsNullOrEmpty(attachmentName)) ? skeleton.GetAttachment(i, attachmentName) : null);
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0007CF1C File Offset: 0x0007B11C
		public static void SetColorToSetupPose(this Slot slot)
		{
			slot.r = slot.data.r;
			slot.g = slot.data.g;
			slot.b = slot.data.b;
			slot.a = slot.data.a;
			slot.r2 = slot.data.r2;
			slot.g2 = slot.data.g2;
			slot.b2 = slot.data.b2;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0007CFA0 File Offset: 0x0007B1A0
		public static void SetAttachmentToSetupPose(this Slot slot)
		{
			SlotData data = slot.data;
			slot.Attachment = slot.bone.skeleton.GetAttachment(data.name, data.attachmentName);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0007CFD8 File Offset: 0x0007B1D8
		public static void SetSlotAttachmentToSetupPose(this Skeleton skeleton, int slotIndex)
		{
			Slot slot = skeleton.slots.Items[slotIndex];
			string attachmentName = slot.data.attachmentName;
			if (string.IsNullOrEmpty(attachmentName))
			{
				slot.Attachment = null;
			}
			else
			{
				Attachment attachment = skeleton.GetAttachment(slotIndex, attachmentName);
				slot.Attachment = attachment;
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0007D028 File Offset: 0x0007B228
		public static void SetKeyedItemsToSetupPose(this Animation animation, Skeleton skeleton)
		{
			animation.Apply(skeleton, 0f, 0f, false, null, 0f, MixBlend.Setup, MixDirection.Out);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0007D050 File Offset: 0x0007B250
		public static void AllowImmediateQueue(this TrackEntry trackEntry)
		{
			if (trackEntry.nextTrackLast < 0f)
			{
				trackEntry.nextTrackLast = 0f;
			}
		}
	}
}
