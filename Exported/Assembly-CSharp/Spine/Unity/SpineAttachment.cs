using System;

namespace Spine.Unity
{
	// Token: 0x02000222 RID: 546
	public class SpineAttachment : SpineAttributeBase
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x00079F84 File Offset: 0x00078184
		public SpineAttachment(bool currentSkinOnly = true, bool returnAttachmentPath = false, bool placeholdersOnly = false, string slotField = "", string dataField = "", string skinField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			this.currentSkinOnly = currentSkinOnly;
			this.returnAttachmentPath = returnAttachmentPath;
			this.placeholdersOnly = placeholdersOnly;
			this.slotField = slotField;
			this.dataField = dataField;
			this.skinField = skinField;
			this.includeNone = includeNone;
			this.fallbackToTextField = fallbackToTextField;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00079FEC File Offset: 0x000781EC
		public static SpineAttachment.Hierarchy GetHierarchy(string fullPath)
		{
			return new SpineAttachment.Hierarchy(fullPath);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00079FF4 File Offset: 0x000781F4
		public static Attachment GetAttachment(string attachmentPath, SkeletonData skeletonData)
		{
			SpineAttachment.Hierarchy hierarchy = SpineAttachment.GetHierarchy(attachmentPath);
			return (!string.IsNullOrEmpty(hierarchy.name)) ? skeletonData.FindSkin(hierarchy.skin).GetAttachment(skeletonData.FindSlotIndex(hierarchy.slot), hierarchy.name) : null;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0007A048 File Offset: 0x00078248
		public static Attachment GetAttachment(string attachmentPath, SkeletonDataAsset skeletonDataAsset)
		{
			return SpineAttachment.GetAttachment(attachmentPath, skeletonDataAsset.GetSkeletonData(true));
		}

		// Token: 0x04000E27 RID: 3623
		public bool returnAttachmentPath;

		// Token: 0x04000E28 RID: 3624
		public bool currentSkinOnly;

		// Token: 0x04000E29 RID: 3625
		public bool placeholdersOnly;

		// Token: 0x04000E2A RID: 3626
		public string skinField = string.Empty;

		// Token: 0x04000E2B RID: 3627
		public string slotField = string.Empty;

		// Token: 0x02000223 RID: 547
		public struct Hierarchy
		{
			// Token: 0x06001136 RID: 4406 RVA: 0x0007A058 File Offset: 0x00078258
			public Hierarchy(string fullPath)
			{
				string[] array = fullPath.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 0)
				{
					this.skin = string.Empty;
					this.slot = string.Empty;
					this.name = string.Empty;
					return;
				}
				if (array.Length < 2)
				{
					throw new Exception("Cannot generate Attachment Hierarchy from string! Not enough components! [" + fullPath + "]");
				}
				this.skin = array[0];
				this.slot = array[1];
				this.name = string.Empty;
				for (int i = 2; i < array.Length; i++)
				{
					this.name += array[i];
				}
			}

			// Token: 0x04000E2C RID: 3628
			public string skin;

			// Token: 0x04000E2D RID: 3629
			public string slot;

			// Token: 0x04000E2E RID: 3630
			public string name;
		}
	}
}
