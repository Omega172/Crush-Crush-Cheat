using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001D3 RID: 467
	[CreateAssetMenu(menuName = "Spine/EventData Reference Asset", order = 100)]
	public class EventDataReferenceAsset : ScriptableObject
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0006CF2C File Offset: 0x0006B12C
		public EventData EventData
		{
			get
			{
				if (this.eventData == null)
				{
					this.Initialize();
				}
				return this.eventData;
			}
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0006CF48 File Offset: 0x0006B148
		public void Initialize()
		{
			if (this.skeletonDataAsset == null)
			{
				return;
			}
			this.eventData = this.skeletonDataAsset.GetSkeletonData(true).FindEvent(this.eventName);
			if (this.eventData == null)
			{
				Debug.LogWarningFormat("Event Data '{0}' not found in SkeletonData : {1}.", new object[]
				{
					this.eventName,
					this.skeletonDataAsset.name
				});
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0006CFB8 File Offset: 0x0006B1B8
		public static implicit operator EventData(EventDataReferenceAsset asset)
		{
			return asset.EventData;
		}

		// Token: 0x04000C98 RID: 3224
		private const bool QuietSkeletonData = true;

		// Token: 0x04000C99 RID: 3225
		[SerializeField]
		protected SkeletonDataAsset skeletonDataAsset;

		// Token: 0x04000C9A RID: 3226
		[SerializeField]
		[SpineEvent("", "skeletonDataAsset", true, false, false)]
		protected string eventName;

		// Token: 0x04000C9B RID: 3227
		private EventData eventData;
	}
}
