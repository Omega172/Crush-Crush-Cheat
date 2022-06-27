using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001D5 RID: 469
	[CreateAssetMenu(fileName = "New SkeletonDataAsset", menuName = "Spine/SkeletonData Asset")]
	public class SkeletonDataAsset : ScriptableObject
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0006D0FC File Offset: 0x0006B2FC
		public bool IsLoaded
		{
			get
			{
				return this.skeletonData != null;
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0006D10C File Offset: 0x0006B30C
		private void Reset()
		{
			this.Clear();
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0006D114 File Offset: 0x0006B314
		public static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, AtlasAssetBase atlasAsset, bool initialize, float scale = 0.01f)
		{
			return SkeletonDataAsset.CreateRuntimeInstance(skeletonDataFile, new AtlasAssetBase[]
			{
				atlasAsset
			}, initialize, scale);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0006D128 File Offset: 0x0006B328
		public static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, AtlasAssetBase[] atlasAssets, bool initialize, float scale = 0.01f)
		{
			SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
			skeletonDataAsset.Clear();
			skeletonDataAsset.skeletonJSON = skeletonDataFile;
			skeletonDataAsset.atlasAssets = atlasAssets;
			skeletonDataAsset.scale = scale;
			if (initialize)
			{
				skeletonDataAsset.GetSkeletonData(true);
			}
			return skeletonDataAsset;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0006D168 File Offset: 0x0006B368
		public void Clear()
		{
			this.skeletonData = null;
			this.stateData = null;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0006D178 File Offset: 0x0006B378
		public AnimationStateData GetAnimationStateData()
		{
			if (this.stateData != null)
			{
				return this.stateData;
			}
			this.GetSkeletonData(false);
			return this.stateData;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0006D1A8 File Offset: 0x0006B3A8
		public SkeletonData GetSkeletonData(bool quiet)
		{
			if (this.skeletonJSON == null)
			{
				if (!quiet)
				{
					Debug.LogError("Skeleton JSON file not set for SkeletonData asset: " + base.name, this);
				}
				this.Clear();
				return null;
			}
			if (this.skeletonData != null)
			{
				return this.skeletonData;
			}
			Atlas[] atlasArray = this.GetAtlasArray();
			AttachmentLoader attachmentLoader2;
			if (atlasArray.Length == 0)
			{
				AttachmentLoader attachmentLoader = new RegionlessAttachmentLoader();
				attachmentLoader2 = attachmentLoader;
			}
			else
			{
				attachmentLoader2 = new AtlasAttachmentLoader(atlasArray);
			}
			AttachmentLoader attachmentLoader3 = attachmentLoader2;
			float num = this.scale;
			bool flag = this.skeletonJSON.name.ToLower().Contains(".skel");
			SkeletonData skeletonData = null;
			try
			{
				if (flag)
				{
					skeletonData = SkeletonDataAsset.ReadSkeletonData(this.skeletonJSON.bytes, attachmentLoader3, num);
				}
				else
				{
					skeletonData = SkeletonDataAsset.ReadSkeletonData(this.skeletonJSON.text, attachmentLoader3, num);
				}
			}
			catch (Exception ex)
			{
				if (!quiet)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Error reading skeleton JSON file for SkeletonData asset: ",
						base.name,
						"\n",
						ex.Message,
						"\n",
						ex.StackTrace
					}), this);
				}
			}
			if (skeletonData == null)
			{
				return null;
			}
			if (this.skeletonDataModifiers != null)
			{
				foreach (SkeletonDataModifierAsset skeletonDataModifierAsset in this.skeletonDataModifiers)
				{
					if (skeletonDataModifierAsset != null)
					{
						skeletonDataModifierAsset.Apply(skeletonData);
					}
				}
			}
			this.InitializeWithData(skeletonData);
			return this.skeletonData;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0006D378 File Offset: 0x0006B578
		internal void InitializeWithData(SkeletonData sd)
		{
			this.skeletonData = sd;
			this.stateData = new AnimationStateData(this.skeletonData);
			this.FillStateData();
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0006D398 File Offset: 0x0006B598
		public void FillStateData()
		{
			if (this.stateData != null)
			{
				this.stateData.defaultMix = this.defaultMix;
				int i = 0;
				int num = this.fromAnimation.Length;
				while (i < num)
				{
					if (this.fromAnimation[i].Length != 0 && this.toAnimation[i].Length != 0)
					{
						this.stateData.SetMix(this.fromAnimation[i], this.toAnimation[i], this.duration[i]);
					}
					i++;
				}
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0006D428 File Offset: 0x0006B628
		internal Atlas[] GetAtlasArray()
		{
			List<Atlas> list = new List<Atlas>(this.atlasAssets.Length);
			for (int i = 0; i < this.atlasAssets.Length; i++)
			{
				AtlasAssetBase atlasAssetBase = this.atlasAssets[i];
				if (!(atlasAssetBase == null))
				{
					Atlas atlas = atlasAssetBase.GetAtlas();
					if (atlas != null)
					{
						list.Add(atlas);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0006D498 File Offset: 0x0006B698
		internal static SkeletonData ReadSkeletonData(byte[] bytes, AttachmentLoader attachmentLoader, float scale)
		{
			SkeletonData result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				SkeletonBinary skeletonBinary = new SkeletonBinary(attachmentLoader)
				{
					Scale = scale
				};
				result = skeletonBinary.ReadSkeletonData(memoryStream);
			}
			return result;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0006D4F8 File Offset: 0x0006B6F8
		internal static SkeletonData ReadSkeletonData(string text, AttachmentLoader attachmentLoader, float scale)
		{
			StringReader reader = new StringReader(text);
			SkeletonJson skeletonJson = new SkeletonJson(attachmentLoader)
			{
				Scale = scale
			};
			return skeletonJson.ReadSkeletonData(reader);
		}

		// Token: 0x04000C9D RID: 3229
		public AtlasAssetBase[] atlasAssets = new AtlasAssetBase[0];

		// Token: 0x04000C9E RID: 3230
		public float scale = 0.01f;

		// Token: 0x04000C9F RID: 3231
		public TextAsset skeletonJSON;

		// Token: 0x04000CA0 RID: 3232
		[Tooltip("Use SkeletonDataModifierAssets to apply changes to the SkeletonData after being loaded, such as apply blend mode Materials to Attachments under slots with special blend modes.")]
		public List<SkeletonDataModifierAsset> skeletonDataModifiers = new List<SkeletonDataModifierAsset>();

		// Token: 0x04000CA1 RID: 3233
		[SpineAnimation("", "", false, false)]
		public string[] fromAnimation = new string[0];

		// Token: 0x04000CA2 RID: 3234
		[SpineAnimation("", "", false, false)]
		public string[] toAnimation = new string[0];

		// Token: 0x04000CA3 RID: 3235
		public float[] duration = new float[0];

		// Token: 0x04000CA4 RID: 3236
		public float defaultMix;

		// Token: 0x04000CA5 RID: 3237
		public RuntimeAnimatorController controller;

		// Token: 0x04000CA6 RID: 3238
		private SkeletonData skeletonData;

		// Token: 0x04000CA7 RID: 3239
		private AnimationStateData stateData;
	}
}
