using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001FE RID: 510
	[ExecuteInEditMode]
	[RequireComponent(typeof(ISkeletonAnimation))]
	public sealed class SkeletonUtility : MonoBehaviour
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060010A7 RID: 4263 RVA: 0x000748B4 File Offset: 0x00072AB4
		// (remove) Token: 0x060010A8 RID: 4264 RVA: 0x000748D0 File Offset: 0x00072AD0
		public event SkeletonUtility.SkeletonUtilityDelegate OnReset;

		// Token: 0x060010A9 RID: 4265 RVA: 0x000748EC File Offset: 0x00072AEC
		public static PolygonCollider2D AddBoundingBoxGameObject(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
		{
			Skin skin = (!string.IsNullOrEmpty(skinName)) ? skeleton.data.FindSkin(skinName) : skeleton.data.defaultSkin;
			if (skin == null)
			{
				Debug.LogError("Skin " + skinName + " not found!");
				return null;
			}
			Attachment attachment = skin.GetAttachment(skeleton.FindSlotIndex(slotName), attachmentName);
			if (attachment == null)
			{
				Debug.LogFormat("Attachment in slot '{0}' named '{1}' not found in skin '{2}'.", new object[]
				{
					slotName,
					attachmentName,
					skin.name
				});
				return null;
			}
			BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
			if (boundingBoxAttachment != null)
			{
				Slot slot = skeleton.FindSlot(slotName);
				return SkeletonUtility.AddBoundingBoxGameObject(boundingBoxAttachment.Name, boundingBoxAttachment, slot, parent, isTrigger);
			}
			Debug.LogFormat("Attachment '{0}' was not a Bounding Box.", new object[]
			{
				attachmentName
			});
			return null;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000749B4 File Offset: 0x00072BB4
		public static PolygonCollider2D AddBoundingBoxGameObject(string name, BoundingBoxAttachment box, Slot slot, Transform parent, bool isTrigger = true)
		{
			GameObject gameObject = new GameObject("[BoundingBox]" + ((!string.IsNullOrEmpty(name)) ? name : box.Name));
			Transform transform = gameObject.transform;
			transform.parent = parent;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			return SkeletonUtility.AddBoundingBoxAsComponent(box, slot, gameObject, isTrigger);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00074A24 File Offset: 0x00072C24
		public static PolygonCollider2D AddBoundingBoxAsComponent(BoundingBoxAttachment box, Slot slot, GameObject gameObject, bool isTrigger = true)
		{
			if (box == null)
			{
				return null;
			}
			PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
			polygonCollider2D.isTrigger = isTrigger;
			SkeletonUtility.SetColliderPointsLocal(polygonCollider2D, slot, box);
			return polygonCollider2D;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00074A50 File Offset: 0x00072C50
		public static void SetColliderPointsLocal(PolygonCollider2D collider, Slot slot, BoundingBoxAttachment box)
		{
			if (box == null)
			{
				return;
			}
			if (box.IsWeighted())
			{
				Debug.LogWarning("UnityEngine.PolygonCollider2D does not support weighted or animated points. Collider points will not be animated and may have incorrect orientation. If you want to use it as a collider, please remove weights and animations from the bounding box in Spine editor.");
			}
			Vector2[] localVertices = box.GetLocalVertices(slot, null);
			collider.SetPath(0, localVertices);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00074A8C File Offset: 0x00072C8C
		public static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
		{
			float[] vertices = boundingBox.Vertices;
			int num = vertices.Length;
			Bounds result = default(Bounds);
			result.center = new Vector3(vertices[0], vertices[1], 0f);
			for (int i = 2; i < num; i += 2)
			{
				result.Encapsulate(new Vector3(vertices[i], vertices[i + 1], 0f));
			}
			Vector3 size = result.size;
			size.z = depth;
			result.size = size;
			return result;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00074B0C File Offset: 0x00072D0C
		public static Rigidbody2D AddBoneRigidbody2D(GameObject gameObject, bool isKinematic = true, float gravityScale = 0f)
		{
			Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
			if (rigidbody2D == null)
			{
				rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
				rigidbody2D.isKinematic = isKinematic;
				rigidbody2D.gravityScale = gravityScale;
			}
			return rigidbody2D;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00074B44 File Offset: 0x00072D44
		private void Update()
		{
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			if (skeleton != null && this.boneRoot != null)
			{
				if (this.flipBy180DegreeRotation)
				{
					this.boneRoot.localScale = new Vector3(Mathf.Abs(skeleton.ScaleX), Mathf.Abs(skeleton.ScaleY), 1f);
					this.boneRoot.eulerAngles = new Vector3((float)((skeleton.ScaleY <= 0f) ? 180 : 0), (float)((skeleton.ScaleX <= 0f) ? 180 : 0), 0f);
				}
				else
				{
					this.boneRoot.localScale = new Vector3(skeleton.ScaleX, skeleton.ScaleY, 1f);
				}
			}
			if (this.canvas != null)
			{
				this.positionScale = this.canvas.referencePixelsPerUnit;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00074C40 File Offset: 0x00072E40
		public ISkeletonComponent SkeletonComponent
		{
			get
			{
				if (this.skeletonComponent == null)
				{
					ISkeletonComponent skeletonComponent;
					if (this.skeletonRenderer != null)
					{
						ISkeletonComponent component = this.skeletonRenderer.GetComponent<ISkeletonComponent>();
						skeletonComponent = component;
					}
					else if (this.skeletonGraphic != null)
					{
						ISkeletonComponent component = this.skeletonGraphic.GetComponent<ISkeletonComponent>();
						skeletonComponent = component;
					}
					else
					{
						skeletonComponent = base.GetComponent<ISkeletonComponent>();
					}
					this.skeletonComponent = skeletonComponent;
				}
				return this.skeletonComponent;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00074CB0 File Offset: 0x00072EB0
		public Skeleton Skeleton
		{
			get
			{
				if (this.SkeletonComponent == null)
				{
					return null;
				}
				return this.skeletonComponent.Skeleton;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x00074CCC File Offset: 0x00072ECC
		public bool IsValid
		{
			get
			{
				return (this.skeletonRenderer != null && this.skeletonRenderer.valid) || (this.skeletonGraphic != null && this.skeletonGraphic.IsValid);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00074D1C File Offset: 0x00072F1C
		public float PositionScale
		{
			get
			{
				return this.positionScale;
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00074D24 File Offset: 0x00072F24
		public void ResubscribeEvents()
		{
			this.OnDisable();
			this.OnEnable();
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00074D34 File Offset: 0x00072F34
		private void OnEnable()
		{
			if (this.skeletonRenderer == null)
			{
				this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			}
			if (this.skeletonGraphic == null)
			{
				this.skeletonGraphic = base.GetComponent<SkeletonGraphic>();
			}
			if (this.skeletonAnimation == null)
			{
				ISkeletonAnimation skeletonAnimation;
				if (this.skeletonRenderer != null)
				{
					ISkeletonAnimation component = this.skeletonRenderer.GetComponent<ISkeletonAnimation>();
					skeletonAnimation = component;
				}
				else if (this.skeletonGraphic != null)
				{
					ISkeletonAnimation component = this.skeletonGraphic.GetComponent<ISkeletonAnimation>();
					skeletonAnimation = component;
				}
				else
				{
					skeletonAnimation = base.GetComponent<ISkeletonAnimation>();
				}
				this.skeletonAnimation = skeletonAnimation;
			}
			if (this.skeletonComponent == null)
			{
				ISkeletonComponent skeletonComponent;
				if (this.skeletonRenderer != null)
				{
					ISkeletonComponent component2 = this.skeletonRenderer.GetComponent<ISkeletonComponent>();
					skeletonComponent = component2;
				}
				else if (this.skeletonGraphic != null)
				{
					ISkeletonComponent component2 = this.skeletonGraphic.GetComponent<ISkeletonComponent>();
					skeletonComponent = component2;
				}
				else
				{
					skeletonComponent = base.GetComponent<ISkeletonComponent>();
				}
				this.skeletonComponent = skeletonComponent;
			}
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRendererReset;
				this.skeletonRenderer.OnRebuild += this.HandleRendererReset;
			}
			else if (this.skeletonGraphic != null)
			{
				this.skeletonGraphic.OnRebuild -= this.HandleRendererReset;
				this.skeletonGraphic.OnRebuild += this.HandleRendererReset;
				this.canvas = this.skeletonGraphic.canvas;
				if (this.canvas == null)
				{
					this.canvas = this.skeletonGraphic.GetComponentInParent<Canvas>();
				}
				if (this.canvas == null)
				{
					this.positionScale = 100f;
				}
			}
			if (this.skeletonAnimation != null)
			{
				this.skeletonAnimation.UpdateLocal -= this.UpdateLocal;
				this.skeletonAnimation.UpdateLocal += this.UpdateLocal;
			}
			this.CollectBones();
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00074F48 File Offset: 0x00073148
		private void Start()
		{
			this.CollectBones();
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00074F50 File Offset: 0x00073150
		private void OnDisable()
		{
			if (this.skeletonRenderer != null)
			{
				this.skeletonRenderer.OnRebuild -= this.HandleRendererReset;
			}
			if (this.skeletonGraphic != null)
			{
				this.skeletonGraphic.OnRebuild -= this.HandleRendererReset;
			}
			if (this.skeletonAnimation != null)
			{
				this.skeletonAnimation.UpdateLocal -= this.UpdateLocal;
				this.skeletonAnimation.UpdateWorld -= this.UpdateWorld;
				this.skeletonAnimation.UpdateComplete -= this.UpdateComplete;
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00075000 File Offset: 0x00073200
		private void HandleRendererReset(SkeletonRenderer r)
		{
			if (this.OnReset != null)
			{
				this.OnReset();
			}
			this.CollectBones();
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00075020 File Offset: 0x00073220
		private void HandleRendererReset(SkeletonGraphic g)
		{
			if (this.OnReset != null)
			{
				this.OnReset();
			}
			this.CollectBones();
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00075040 File Offset: 0x00073240
		public void RegisterBone(SkeletonUtilityBone bone)
		{
			if (this.boneComponents.Contains(bone))
			{
				return;
			}
			this.boneComponents.Add(bone);
			this.needToReprocessBones = true;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00075068 File Offset: 0x00073268
		public void UnregisterBone(SkeletonUtilityBone bone)
		{
			this.boneComponents.Remove(bone);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00075078 File Offset: 0x00073278
		public void RegisterConstraint(SkeletonUtilityConstraint constraint)
		{
			if (this.constraintComponents.Contains(constraint))
			{
				return;
			}
			this.constraintComponents.Add(constraint);
			this.needToReprocessBones = true;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000750A0 File Offset: 0x000732A0
		public void UnregisterConstraint(SkeletonUtilityConstraint constraint)
		{
			this.constraintComponents.Remove(constraint);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x000750B0 File Offset: 0x000732B0
		public void CollectBones()
		{
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			if (skeleton == null)
			{
				return;
			}
			if (this.boneRoot != null)
			{
				List<object> list = new List<object>();
				ExposedList<IkConstraint> ikConstraints = skeleton.IkConstraints;
				int i = 0;
				int count = ikConstraints.Count;
				while (i < count)
				{
					list.Add(ikConstraints.Items[i].target);
					i++;
				}
				ExposedList<TransformConstraint> transformConstraints = skeleton.TransformConstraints;
				int j = 0;
				int count2 = transformConstraints.Count;
				while (j < count2)
				{
					list.Add(transformConstraints.Items[j].target);
					j++;
				}
				List<SkeletonUtilityBone> list2 = this.boneComponents;
				int k = 0;
				int count3 = list2.Count;
				while (k < count3)
				{
					SkeletonUtilityBone skeletonUtilityBone = list2[k];
					if (skeletonUtilityBone.bone != null)
					{
						goto IL_E5;
					}
					skeletonUtilityBone.DoUpdate(SkeletonUtilityBone.UpdatePhase.Local);
					if (skeletonUtilityBone.bone != null)
					{
						goto IL_E5;
					}
					IL_116:
					k++;
					continue;
					IL_E5:
					this.hasOverrideBones |= (skeletonUtilityBone.mode == SkeletonUtilityBone.Mode.Override);
					this.hasConstraints |= list.Contains(skeletonUtilityBone.bone);
					goto IL_116;
				}
				this.hasConstraints |= (this.constraintComponents.Count > 0);
				if (this.skeletonAnimation != null)
				{
					this.skeletonAnimation.UpdateWorld -= this.UpdateWorld;
					this.skeletonAnimation.UpdateComplete -= this.UpdateComplete;
					if (this.hasOverrideBones || this.hasConstraints)
					{
						this.skeletonAnimation.UpdateWorld += this.UpdateWorld;
					}
					if (this.hasConstraints)
					{
						this.skeletonAnimation.UpdateComplete += this.UpdateComplete;
					}
				}
				this.needToReprocessBones = false;
			}
			else
			{
				this.boneComponents.Clear();
				this.constraintComponents.Clear();
			}
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000752A8 File Offset: 0x000734A8
		private void UpdateLocal(ISkeletonAnimation anim)
		{
			if (this.needToReprocessBones)
			{
				this.CollectBones();
			}
			List<SkeletonUtilityBone> list = this.boneComponents;
			if (list == null)
			{
				return;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].transformLerpComplete = false;
				i++;
			}
			this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Local);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00075304 File Offset: 0x00073504
		private void UpdateWorld(ISkeletonAnimation anim)
		{
			this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.World);
			int i = 0;
			int count = this.constraintComponents.Count;
			while (i < count)
			{
				this.constraintComponents[i].DoUpdate();
				i++;
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00075348 File Offset: 0x00073548
		private void UpdateComplete(ISkeletonAnimation anim)
		{
			this.UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Complete);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00075354 File Offset: 0x00073554
		private void UpdateAllBones(SkeletonUtilityBone.UpdatePhase phase)
		{
			if (this.boneRoot == null)
			{
				this.CollectBones();
			}
			List<SkeletonUtilityBone> list = this.boneComponents;
			if (list == null)
			{
				return;
			}
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].DoUpdate(phase);
				i++;
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x000753AC File Offset: 0x000735AC
		public Transform GetBoneRoot()
		{
			if (this.boneRoot != null)
			{
				return this.boneRoot;
			}
			GameObject gameObject = new GameObject("SkeletonUtility-SkeletonRoot");
			if (this.skeletonGraphic != null)
			{
				gameObject.AddComponent<RectTransform>();
			}
			this.boneRoot = gameObject.transform;
			this.boneRoot.SetParent(base.transform);
			this.boneRoot.localPosition = Vector3.zero;
			this.boneRoot.localRotation = Quaternion.identity;
			this.boneRoot.localScale = Vector3.one;
			return this.boneRoot;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00075448 File Offset: 0x00073648
		public GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			this.GetBoneRoot();
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			GameObject result = this.SpawnBone(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
			this.CollectBones();
			return result;
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00075488 File Offset: 0x00073688
		public GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			this.GetBoneRoot();
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			GameObject result = this.SpawnBoneRecursively(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
			this.CollectBones();
			return result;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000754C8 File Offset: 0x000736C8
		public GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			GameObject gameObject = this.SpawnBone(bone, parent, mode, pos, rot, sca);
			ExposedList<Bone> children = bone.Children;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				Bone bone2 = children.Items[i];
				this.SpawnBoneRecursively(bone2, gameObject.transform, mode, pos, rot, sca);
				i++;
			}
			return gameObject;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00075528 File Offset: 0x00073728
		public GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			GameObject gameObject = new GameObject(bone.Data.Name);
			if (this.skeletonGraphic != null)
			{
				gameObject.AddComponent<RectTransform>();
			}
			Transform transform = gameObject.transform;
			transform.SetParent(parent);
			SkeletonUtilityBone skeletonUtilityBone = gameObject.AddComponent<SkeletonUtilityBone>();
			skeletonUtilityBone.hierarchy = this;
			skeletonUtilityBone.position = pos;
			skeletonUtilityBone.rotation = rot;
			skeletonUtilityBone.scale = sca;
			skeletonUtilityBone.mode = mode;
			skeletonUtilityBone.zPosition = true;
			skeletonUtilityBone.Reset();
			skeletonUtilityBone.bone = bone;
			skeletonUtilityBone.boneName = bone.Data.Name;
			skeletonUtilityBone.valid = true;
			if (mode == SkeletonUtilityBone.Mode.Override)
			{
				if (rot)
				{
					transform.localRotation = Quaternion.Euler(0f, 0f, skeletonUtilityBone.bone.AppliedRotation);
				}
				if (pos)
				{
					transform.localPosition = new Vector3(skeletonUtilityBone.bone.X * this.positionScale, skeletonUtilityBone.bone.Y * this.positionScale, 0f);
				}
				transform.localScale = new Vector3(skeletonUtilityBone.bone.scaleX, skeletonUtilityBone.bone.scaleY, 0f);
			}
			return gameObject;
		}

		// Token: 0x04000DA8 RID: 3496
		public Transform boneRoot;

		// Token: 0x04000DA9 RID: 3497
		public bool flipBy180DegreeRotation;

		// Token: 0x04000DAA RID: 3498
		[HideInInspector]
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04000DAB RID: 3499
		[HideInInspector]
		public SkeletonGraphic skeletonGraphic;

		// Token: 0x04000DAC RID: 3500
		private Canvas canvas;

		// Token: 0x04000DAD RID: 3501
		[NonSerialized]
		public ISkeletonAnimation skeletonAnimation;

		// Token: 0x04000DAE RID: 3502
		private ISkeletonComponent skeletonComponent;

		// Token: 0x04000DAF RID: 3503
		[NonSerialized]
		public List<SkeletonUtilityBone> boneComponents = new List<SkeletonUtilityBone>();

		// Token: 0x04000DB0 RID: 3504
		[NonSerialized]
		public List<SkeletonUtilityConstraint> constraintComponents = new List<SkeletonUtilityConstraint>();

		// Token: 0x04000DB1 RID: 3505
		private float positionScale = 1f;

		// Token: 0x04000DB2 RID: 3506
		private bool hasOverrideBones;

		// Token: 0x04000DB3 RID: 3507
		private bool hasConstraints;

		// Token: 0x04000DB4 RID: 3508
		private bool needToReprocessBones;

		// Token: 0x02000242 RID: 578
		// (Invoke) Token: 0x0600120C RID: 4620
		public delegate void SkeletonUtilityDelegate();
	}
}
