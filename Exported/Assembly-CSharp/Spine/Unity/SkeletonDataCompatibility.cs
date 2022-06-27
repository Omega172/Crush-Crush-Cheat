using System;

namespace Spine.Unity
{
	// Token: 0x020001D6 RID: 470
	public static class SkeletonDataCompatibility
	{
		// Token: 0x020001D7 RID: 471
		public enum SourceType
		{
			// Token: 0x04000CA9 RID: 3241
			Json,
			// Token: 0x04000CAA RID: 3242
			Binary
		}

		// Token: 0x020001D8 RID: 472
		[Serializable]
		public class VersionInfo
		{
			// Token: 0x04000CAB RID: 3243
			public string rawVersion;

			// Token: 0x04000CAC RID: 3244
			public int[] version;

			// Token: 0x04000CAD RID: 3245
			public SkeletonDataCompatibility.SourceType sourceType;
		}

		// Token: 0x020001D9 RID: 473
		[Serializable]
		public class CompatibilityProblemInfo
		{
			// Token: 0x06000F55 RID: 3925 RVA: 0x0006D534 File Offset: 0x0006B734
			public string DescriptionString()
			{
				string text = string.Empty;
				string arg = null;
				foreach (int[] array2 in this.compatibleVersions)
				{
					text += string.Format("{0}{1}.{2}", arg, array2[0], array2[1]);
					arg = " or ";
				}
				return string.Format("Skeleton data could not be loaded. Data version: {0}. Required version: {1}.\nPlease re-export skeleton data with Spine {1} or change runtime to version {2}.{3}.", new object[]
				{
					this.actualVersion.rawVersion,
					text,
					this.actualVersion.version[0],
					this.actualVersion.version[1]
				});
			}

			// Token: 0x04000CAE RID: 3246
			public SkeletonDataCompatibility.VersionInfo actualVersion;

			// Token: 0x04000CAF RID: 3247
			public int[][] compatibleVersions;
		}
	}
}
