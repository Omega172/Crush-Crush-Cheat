using System;
using System.Collections.Generic;
using Spine.Collections;

namespace Spine
{
	// Token: 0x020001C9 RID: 457
	public class Skin
	{
		// Token: 0x06000EA7 RID: 3751 RVA: 0x0006A95C File Offset: 0x00068B5C
		public Skin(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0006A9B4 File Offset: 0x00068BB4
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0006A9BC File Offset: 0x00068BBC
		public OrderedDictionary<Skin.SkinEntry, Attachment> Attachments
		{
			get
			{
				return this.attachments;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0006A9C4 File Offset: 0x00068BC4
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0006A9CC File Offset: 0x00068BCC
		public ExposedList<ConstraintData> Constraints
		{
			get
			{
				return this.constraints;
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0006A9D4 File Offset: 0x00068BD4
		public void SetAttachment(int slotIndex, string name, Attachment attachment)
		{
			if (attachment == null)
			{
				throw new ArgumentNullException("attachment", "attachment cannot be null.");
			}
			if (slotIndex < 0)
			{
				throw new ArgumentNullException("slotIndex", "slotIndex must be >= 0.");
			}
			this.attachments[new Skin.SkinEntry(slotIndex, name, attachment)] = attachment;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0006AA24 File Offset: 0x00068C24
		public void AddSkin(Skin skin)
		{
			foreach (BoneData item in skin.bones)
			{
				if (!this.bones.Contains(item))
				{
					this.bones.Add(item);
				}
			}
			foreach (ConstraintData item2 in skin.constraints)
			{
				if (!this.constraints.Contains(item2))
				{
					this.constraints.Add(item2);
				}
			}
			foreach (Skin.SkinEntry skinEntry in skin.attachments.Keys)
			{
				this.SetAttachment(skinEntry.SlotIndex, skinEntry.Name, skinEntry.Attachment);
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0006AB7C File Offset: 0x00068D7C
		public void CopySkin(Skin skin)
		{
			foreach (BoneData item in skin.bones)
			{
				if (!this.bones.Contains(item))
				{
					this.bones.Add(item);
				}
			}
			foreach (ConstraintData item2 in skin.constraints)
			{
				if (!this.constraints.Contains(item2))
				{
					this.constraints.Add(item2);
				}
			}
			foreach (Skin.SkinEntry skinEntry in skin.attachments.Keys)
			{
				if (skinEntry.Attachment is MeshAttachment)
				{
					this.SetAttachment(skinEntry.SlotIndex, skinEntry.Name, (skinEntry.Attachment == null) ? null : ((MeshAttachment)skinEntry.Attachment).NewLinkedMesh());
				}
				else
				{
					this.SetAttachment(skinEntry.SlotIndex, skinEntry.Name, (skinEntry.Attachment == null) ? null : skinEntry.Attachment.Copy());
				}
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0006AD38 File Offset: 0x00068F38
		public Attachment GetAttachment(int slotIndex, string name)
		{
			Skin.SkinEntry key = new Skin.SkinEntry(slotIndex, name, null);
			Attachment attachment = null;
			bool flag = this.attachments.TryGetValue(key, out attachment);
			return (!flag) ? null : attachment;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0006AD70 File Offset: 0x00068F70
		public void RemoveAttachment(int slotIndex, string name)
		{
			if (slotIndex < 0)
			{
				throw new ArgumentOutOfRangeException("slotIndex", "slotIndex must be >= 0");
			}
			Skin.SkinEntry key = new Skin.SkinEntry(slotIndex, name, null);
			this.attachments.Remove(key);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0006ADAC File Offset: 0x00068FAC
		public ICollection<Skin.SkinEntry> GetAttachments()
		{
			return this.attachments.Keys;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0006ADBC File Offset: 0x00068FBC
		public void GetAttachments(int slotIndex, List<Skin.SkinEntry> attachments)
		{
			foreach (Skin.SkinEntry item in this.attachments.Keys)
			{
				if (item.SlotIndex == slotIndex)
				{
					attachments.Add(item);
				}
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0006AE34 File Offset: 0x00069034
		public void Clear()
		{
			this.attachments.Clear();
			this.bones.Clear(true);
			this.constraints.Clear(true);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0006AE5C File Offset: 0x0006905C
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0006AE64 File Offset: 0x00069064
		internal void AttachAll(Skeleton skeleton, Skin oldSkin)
		{
			foreach (Skin.SkinEntry skinEntry in oldSkin.attachments.Keys)
			{
				int slotIndex = skinEntry.SlotIndex;
				Slot slot = skeleton.slots.Items[slotIndex];
				if (slot.Attachment == skinEntry.Attachment)
				{
					Attachment attachment = this.GetAttachment(slotIndex, skinEntry.Name);
					if (attachment != null)
					{
						slot.Attachment = attachment;
					}
				}
			}
		}

		// Token: 0x04000C53 RID: 3155
		internal string name;

		// Token: 0x04000C54 RID: 3156
		private OrderedDictionary<Skin.SkinEntry, Attachment> attachments = new OrderedDictionary<Skin.SkinEntry, Attachment>(Skin.SkinEntryComparer.Instance);

		// Token: 0x04000C55 RID: 3157
		internal readonly ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04000C56 RID: 3158
		internal readonly ExposedList<ConstraintData> constraints = new ExposedList<ConstraintData>();

		// Token: 0x020001CA RID: 458
		public struct SkinEntry
		{
			// Token: 0x06000EB6 RID: 3766 RVA: 0x0006AF0C File Offset: 0x0006910C
			public SkinEntry(int slotIndex, string name, Attachment attachment)
			{
				this.slotIndex = slotIndex;
				this.name = name;
				this.attachment = attachment;
				this.hashCode = this.name.GetHashCode() + this.slotIndex * 37;
			}

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0006AF4C File Offset: 0x0006914C
			public int SlotIndex
			{
				get
				{
					return this.slotIndex;
				}
			}

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0006AF54 File Offset: 0x00069154
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0006AF5C File Offset: 0x0006915C
			public Attachment Attachment
			{
				get
				{
					return this.attachment;
				}
			}

			// Token: 0x04000C57 RID: 3159
			private readonly int slotIndex;

			// Token: 0x04000C58 RID: 3160
			private readonly string name;

			// Token: 0x04000C59 RID: 3161
			private readonly Attachment attachment;

			// Token: 0x04000C5A RID: 3162
			internal readonly int hashCode;
		}

		// Token: 0x020001CB RID: 459
		private class SkinEntryComparer : IEqualityComparer<Skin.SkinEntry>
		{
			// Token: 0x06000EBC RID: 3772 RVA: 0x0006AF78 File Offset: 0x00069178
			bool IEqualityComparer<Skin.SkinEntry>.Equals(Skin.SkinEntry e1, Skin.SkinEntry e2)
			{
				return e1.SlotIndex == e2.SlotIndex && string.Equals(e1.Name, e2.Name, StringComparison.Ordinal);
			}

			// Token: 0x06000EBD RID: 3773 RVA: 0x0006AFB8 File Offset: 0x000691B8
			int IEqualityComparer<Skin.SkinEntry>.GetHashCode(Skin.SkinEntry e)
			{
				return e.Name.GetHashCode() + e.SlotIndex * 37;
			}

			// Token: 0x04000C5B RID: 3163
			internal static readonly Skin.SkinEntryComparer Instance = new Skin.SkinEntryComparer();
		}
	}
}
