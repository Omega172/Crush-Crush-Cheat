using System;
using System.Collections.Generic;
using Spine.Collections;

namespace Spine.Unity.AttachmentTools
{
	// Token: 0x0200022C RID: 556
	public static class SkinUtilities
	{
		// Token: 0x060011A4 RID: 4516 RVA: 0x0007D070 File Offset: 0x0007B270
		public static Skin UnshareSkin(this Skeleton skeleton, bool includeDefaultSkin, bool unshareAttachments, AnimationState state = null)
		{
			Skin clonedSkin = skeleton.GetClonedSkin("cloned skin", includeDefaultSkin, unshareAttachments, true);
			skeleton.SetSkin(clonedSkin);
			if (state != null)
			{
				skeleton.SetToSetupPose();
				state.Apply(skeleton);
			}
			return clonedSkin;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0007D0A8 File Offset: 0x0007B2A8
		public static Skin GetClonedSkin(this Skeleton skeleton, string newSkinName, bool includeDefaultSkin = false, bool cloneAttachments = false, bool cloneMeshesAsLinked = true)
		{
			Skin skin = new Skin(newSkinName);
			Skin defaultSkin = skeleton.data.DefaultSkin;
			Skin skin2 = skeleton.skin;
			if (includeDefaultSkin)
			{
				defaultSkin.CopyTo(skin, true, cloneAttachments, cloneMeshesAsLinked);
			}
			if (skin2 != null)
			{
				skin2.CopyTo(skin, true, cloneAttachments, cloneMeshesAsLinked);
			}
			return skin;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0007D0F4 File Offset: 0x0007B2F4
		public static Skin GetClone(this Skin original)
		{
			Skin skin = new Skin(original.name + " clone");
			OrderedDictionary<Skin.SkinEntry, Attachment> attachments = skin.Attachments;
			ExposedList<BoneData> bones = skin.Bones;
			ExposedList<ConstraintData> constraints = skin.Constraints;
			foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair in original.Attachments)
			{
				attachments[keyValuePair.Key] = keyValuePair.Value;
			}
			bones.AddRange(original.bones);
			constraints.AddRange(original.constraints);
			return skin;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0007D1B0 File Offset: 0x0007B3B0
		public static void SetAttachment(this Skin skin, string slotName, string keyName, Attachment attachment, Skeleton skeleton)
		{
			int num = skeleton.FindSlotIndex(slotName);
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			if (num == -1)
			{
				throw new ArgumentException(string.Format("Slot '{0}' does not exist in skeleton.", slotName), "slotName");
			}
			skin.SetAttachment(num, keyName, attachment);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0007D204 File Offset: 0x0007B404
		public static void AddAttachments(this Skin skin, Skin otherSkin)
		{
			if (otherSkin == null)
			{
				return;
			}
			otherSkin.CopyTo(skin, true, false, true);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0007D218 File Offset: 0x0007B418
		public static Attachment GetAttachment(this Skin skin, string slotName, string keyName, Skeleton skeleton)
		{
			int num = skeleton.FindSlotIndex(slotName);
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			if (num == -1)
			{
				throw new ArgumentException(string.Format("Slot '{0}' does not exist in skeleton.", slotName), "slotName");
			}
			return skin.GetAttachment(num, keyName);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0007D268 File Offset: 0x0007B468
		public static void SetAttachment(this Skin skin, int slotIndex, string keyName, Attachment attachment)
		{
			skin.SetAttachment(slotIndex, keyName, attachment);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0007D274 File Offset: 0x0007B474
		public static void RemoveAttachment(this Skin skin, string slotName, string keyName, SkeletonData skeletonData)
		{
			int num = skeletonData.FindSlotIndex(slotName);
			if (skeletonData == null)
			{
				throw new ArgumentNullException("skeletonData", "skeletonData cannot be null.");
			}
			if (num == -1)
			{
				throw new ArgumentException(string.Format("Slot '{0}' does not exist in skeleton.", slotName), "slotName");
			}
			skin.RemoveAttachment(num, keyName);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0007D2C4 File Offset: 0x0007B4C4
		public static void Clear(this Skin skin)
		{
			skin.Attachments.Clear();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0007D2D4 File Offset: 0x0007B4D4
		public static void Append(this Skin destination, Skin source)
		{
			source.CopyTo(destination, true, false, true);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0007D2E0 File Offset: 0x0007B4E0
		public static void CopyTo(this Skin source, Skin destination, bool overwrite, bool cloneAttachments, bool cloneMeshesAsLinked = true)
		{
			OrderedDictionary<Skin.SkinEntry, Attachment> attachments = source.Attachments;
			OrderedDictionary<Skin.SkinEntry, Attachment> attachments2 = destination.Attachments;
			ExposedList<BoneData> bones = destination.Bones;
			ExposedList<ConstraintData> constraints = destination.Constraints;
			if (cloneAttachments)
			{
				if (overwrite)
				{
					foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair in attachments)
					{
						Attachment copy = keyValuePair.Value.GetCopy(cloneMeshesAsLinked);
						attachments2[new Skin.SkinEntry(keyValuePair.Key.SlotIndex, keyValuePair.Key.Name, copy)] = copy;
					}
				}
				else
				{
					foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair2 in attachments)
					{
						if (!attachments2.ContainsKey(keyValuePair2.Key))
						{
							Attachment copy2 = keyValuePair2.Value.GetCopy(cloneMeshesAsLinked);
							attachments2.Add(new Skin.SkinEntry(keyValuePair2.Key.SlotIndex, keyValuePair2.Key.Name, copy2), copy2);
						}
					}
				}
			}
			else if (overwrite)
			{
				foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair3 in attachments)
				{
					attachments2[keyValuePair3.Key] = keyValuePair3.Value;
				}
			}
			else
			{
				foreach (KeyValuePair<Skin.SkinEntry, Attachment> keyValuePair4 in attachments)
				{
					if (!attachments2.ContainsKey(keyValuePair4.Key))
					{
						attachments2.Add(keyValuePair4.Key, keyValuePair4.Value);
					}
				}
			}
			foreach (BoneData item in source.bones)
			{
				if (!bones.Contains(item))
				{
					bones.Add(item);
				}
			}
			foreach (ConstraintData item2 in source.constraints)
			{
				if (!constraints.Contains(item2))
				{
					constraints.Add(item2);
				}
			}
		}
	}
}
