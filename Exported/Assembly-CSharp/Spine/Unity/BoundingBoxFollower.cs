using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001E2 RID: 482
	[ExecuteInEditMode]
	public class BoundingBoxFollower : MonoBehaviour
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0006EC44 File Offset: 0x0006CE44
		public Slot Slot
		{
			get
			{
				return this.slot;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0006EC4C File Offset: 0x0006CE4C
		public BoundingBoxAttachment CurrentAttachment
		{
			get
			{
				return this.currentAttachment;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0006EC54 File Offset: 0x0006CE54
		public string CurrentAttachmentName
		{
			get
			{
				return this.currentAttachmentName;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0006EC5C File Offset: 0x0006CE5C
		public PolygonCollider2D CurrentCollider
		{
			get
			{
				return this.currentCollider;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0006EC64 File Offset: 0x0006CE64
		public bool IsTrigger
		{
			get
			{
				return this.isTrigger;
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0006EC6C File Offset: 0x0006CE6C
		private void Start()
		{
			this.Initialize(false);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0006EC78 File Offset: 0x0006CE78
		private void OnEnable()
		{
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRebuild;
				this.skeletonRenderer.OnRebuild += this.HandleRebuild;
			}
			this.Initialize(false);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0006ECCC File Offset: 0x0006CECC
		private void HandleRebuild(SkeletonRenderer sr)
		{
			this.Initialize(false);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0006ECD8 File Offset: 0x0006CED8
		public void Initialize(bool overwrite = false)
		{
			if (this.skeletonRenderer == null)
			{
				return;
			}
			this.skeletonRenderer.Initialize(false);
			if (string.IsNullOrEmpty(this.slotName))
			{
				return;
			}
			if (!overwrite && this.colliderTable.Count > 0 && this.slot != null && this.skeletonRenderer.skeleton == this.slot.Skeleton && this.slotName == this.slot.data.name)
			{
				return;
			}
			this.slot = null;
			this.currentAttachment = null;
			this.currentAttachmentName = null;
			this.currentCollider = null;
			this.colliderTable.Clear();
			this.nameTable.Clear();
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			this.slot = skeleton.FindSlot(this.slotName);
			int slotIndex = skeleton.FindSlotIndex(this.slotName);
			if (this.slot == null)
			{
				if (BoundingBoxFollower.DebugMessages)
				{
					Debug.LogWarning(string.Format("Slot '{0}' not found for BoundingBoxFollower on '{1}'. (Previous colliders were disposed.)", this.slotName, base.gameObject.name));
				}
				return;
			}
			int requiredCount = 0;
			PolygonCollider2D[] components = base.GetComponents<PolygonCollider2D>();
			if (base.gameObject.activeInHierarchy)
			{
				foreach (Skin skin in skeleton.Data.Skins)
				{
					this.AddCollidersForSkin(skin, slotIndex, components, ref requiredCount);
				}
				if (skeleton.skin != null)
				{
					this.AddCollidersForSkin(skeleton.skin, slotIndex, components, ref requiredCount);
				}
			}
			this.DisposeExcessCollidersAfter(requiredCount);
			if (BoundingBoxFollower.DebugMessages && this.colliderTable.Count == 0)
			{
				if (base.gameObject.activeInHierarchy)
				{
					Debug.LogWarning("Bounding Box Follower not valid! Slot [" + this.slotName + "] does not contain any Bounding Box Attachments!");
				}
				else
				{
					Debug.LogWarning("Bounding Box Follower tried to rebuild as a prefab.");
				}
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0006EF04 File Offset: 0x0006D104
		private void AddCollidersForSkin(Skin skin, int slotIndex, PolygonCollider2D[] previousColliders, ref int collidersCount)
		{
			if (skin == null)
			{
				return;
			}
			List<Skin.SkinEntry> list = new List<Skin.SkinEntry>();
			skin.GetAttachments(slotIndex, list);
			foreach (Skin.SkinEntry skinEntry in list)
			{
				Attachment attachment = skin.GetAttachment(slotIndex, skinEntry.Name);
				BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
				if (BoundingBoxFollower.DebugMessages && attachment != null && boundingBoxAttachment == null)
				{
					Debug.Log("BoundingBoxFollower tried to follow a slot that contains non-boundingbox attachments: " + this.slotName);
				}
				if (boundingBoxAttachment != null && !this.colliderTable.ContainsKey(boundingBoxAttachment))
				{
					PolygonCollider2D polygonCollider2D = (collidersCount >= previousColliders.Length) ? base.gameObject.AddComponent<PolygonCollider2D>() : previousColliders[collidersCount];
					collidersCount++;
					SkeletonUtility.SetColliderPointsLocal(polygonCollider2D, this.slot, boundingBoxAttachment);
					polygonCollider2D.isTrigger = this.isTrigger;
					polygonCollider2D.enabled = false;
					polygonCollider2D.hideFlags = HideFlags.NotEditable;
					polygonCollider2D.isTrigger = this.IsTrigger;
					this.colliderTable.Add(boundingBoxAttachment, polygonCollider2D);
					this.nameTable.Add(boundingBoxAttachment, skinEntry.Name);
				}
			}
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0006F058 File Offset: 0x0006D258
		private void OnDisable()
		{
			if (this.clearStateOnDisable)
			{
				this.ClearState();
			}
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRebuild;
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0006F094 File Offset: 0x0006D294
		public void ClearState()
		{
			if (this.colliderTable != null)
			{
				foreach (PolygonCollider2D polygonCollider2D in this.colliderTable.Values)
				{
					polygonCollider2D.enabled = false;
				}
			}
			this.currentAttachment = null;
			this.currentAttachmentName = null;
			this.currentCollider = null;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0006F120 File Offset: 0x0006D320
		private void DisposeExcessCollidersAfter(int requiredCount)
		{
			PolygonCollider2D[] components = base.GetComponents<PolygonCollider2D>();
			if (components.Length == 0)
			{
				return;
			}
			for (int i = requiredCount; i < components.Length; i++)
			{
				PolygonCollider2D polygonCollider2D = components[i];
				if (polygonCollider2D != null)
				{
					UnityEngine.Object.Destroy(polygonCollider2D);
				}
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0006F168 File Offset: 0x0006D368
		private void LateUpdate()
		{
			if (this.slot != null && this.slot.Attachment != this.currentAttachment)
			{
				this.MatchAttachment(this.slot.Attachment);
			}
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0006F1A8 File Offset: 0x0006D3A8
		private void MatchAttachment(Attachment attachment)
		{
			BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
			if (BoundingBoxFollower.DebugMessages && attachment != null && boundingBoxAttachment == null)
			{
				Debug.LogWarning("BoundingBoxFollower tried to match a non-boundingbox attachment. It will treat it as null.");
			}
			if (this.currentCollider != null)
			{
				this.currentCollider.enabled = false;
			}
			if (boundingBoxAttachment == null)
			{
				this.currentCollider = null;
				this.currentAttachment = null;
				this.currentAttachmentName = null;
			}
			else
			{
				PolygonCollider2D x;
				this.colliderTable.TryGetValue(boundingBoxAttachment, out x);
				if (x != null)
				{
					this.currentCollider = x;
					this.currentCollider.enabled = true;
					this.currentAttachment = boundingBoxAttachment;
					this.currentAttachmentName = this.nameTable[boundingBoxAttachment];
				}
				else
				{
					this.currentCollider = null;
					this.currentAttachment = boundingBoxAttachment;
					this.currentAttachmentName = null;
					if (BoundingBoxFollower.DebugMessages)
					{
						Debug.LogFormat("Collider for BoundingBoxAttachment named '{0}' was not initialized. It is possibly from a new skin. currentAttachmentName will be null. You may need to call BoundingBoxFollower.Initialize(overwrite: true);", new object[]
						{
							boundingBoxAttachment.Name
						});
					}
				}
			}
		}

		// Token: 0x04000CD9 RID: 3289
		internal static bool DebugMessages = true;

		// Token: 0x04000CDA RID: 3290
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000CDB RID: 3291
		[SpineSlot("", "skeletonRenderer", true, true, false)]
		public string slotName;

		// Token: 0x04000CDC RID: 3292
		public bool isTrigger;

		// Token: 0x04000CDD RID: 3293
		public bool clearStateOnDisable = true;

		// Token: 0x04000CDE RID: 3294
		private Slot slot;

		// Token: 0x04000CDF RID: 3295
		private BoundingBoxAttachment currentAttachment;

		// Token: 0x04000CE0 RID: 3296
		private string currentAttachmentName;

		// Token: 0x04000CE1 RID: 3297
		private PolygonCollider2D currentCollider;

		// Token: 0x04000CE2 RID: 3298
		public readonly Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();

		// Token: 0x04000CE3 RID: 3299
		public readonly Dictionary<BoundingBoxAttachment, string> nameTable = new Dictionary<BoundingBoxAttachment, string>();
	}
}
