using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity
{
	// Token: 0x020001E8 RID: 488
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
	[AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
	[DisallowMultipleComponent]
	public class SkeletonGraphic : MaskableGraphic, ISkeletonAnimation, IHasSkeletonDataAsset, ISkeletonComponent, IAnimationStateComponent
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000FE0 RID: 4064 RVA: 0x0007046C File Offset: 0x0006E66C
		// (remove) Token: 0x06000FE1 RID: 4065 RVA: 0x00070488 File Offset: 0x0006E688
		public event SkeletonGraphic.SkeletonRendererDelegate OnRebuild;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000FE2 RID: 4066 RVA: 0x000704A4 File Offset: 0x0006E6A4
		// (remove) Token: 0x06000FE3 RID: 4067 RVA: 0x000704C0 File Offset: 0x0006E6C0
		public event SkeletonGraphic.SkeletonRendererDelegate OnMeshAndMaterialsUpdated;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000FE4 RID: 4068 RVA: 0x000704DC File Offset: 0x0006E6DC
		// (remove) Token: 0x06000FE5 RID: 4069 RVA: 0x000704F8 File Offset: 0x0006E6F8
		public event UpdateBonesDelegate UpdateLocal;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000FE6 RID: 4070 RVA: 0x00070514 File Offset: 0x0006E714
		// (remove) Token: 0x06000FE7 RID: 4071 RVA: 0x00070530 File Offset: 0x0006E730
		public event UpdateBonesDelegate UpdateWorld;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000FE8 RID: 4072 RVA: 0x0007054C File Offset: 0x0006E74C
		// (remove) Token: 0x06000FE9 RID: 4073 RVA: 0x00070568 File Offset: 0x0006E768
		public event UpdateBonesDelegate UpdateComplete;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000FEA RID: 4074 RVA: 0x00070584 File Offset: 0x0006E784
		// (remove) Token: 0x06000FEB RID: 4075 RVA: 0x000705A0 File Offset: 0x0006E7A0
		public event MeshGeneratorDelegate OnPostProcessVertices;

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x000705BC File Offset: 0x0006E7BC
		public SkeletonDataAsset SkeletonDataAsset
		{
			get
			{
				return this.skeletonDataAsset;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x000705C4 File Offset: 0x0006E7C4
		// (set) Token: 0x06000FEE RID: 4078 RVA: 0x000705CC File Offset: 0x0006E7CC
		public UpdateMode UpdateMode
		{
			get
			{
				return this.updateMode;
			}
			set
			{
				this.updateMode = value;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x000705D8 File Offset: 0x0006E7D8
		public List<Transform> SeparatorParts
		{
			get
			{
				return this.separatorParts;
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000705E0 File Offset: 0x0006E7E0
		public static SkeletonGraphic NewSkeletonGraphicGameObject(SkeletonDataAsset skeletonDataAsset, Transform parent, Material material)
		{
			SkeletonGraphic skeletonGraphic = SkeletonGraphic.AddSkeletonGraphicComponent(new GameObject("New Spine GameObject"), skeletonDataAsset, material);
			if (parent != null)
			{
				skeletonGraphic.transform.SetParent(parent, false);
			}
			return skeletonGraphic;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0007061C File Offset: 0x0006E81C
		public static SkeletonGraphic AddSkeletonGraphicComponent(GameObject gameObject, SkeletonDataAsset skeletonDataAsset, Material material)
		{
			SkeletonGraphic skeletonGraphic = gameObject.AddComponent<SkeletonGraphic>();
			if (skeletonDataAsset != null)
			{
				skeletonGraphic.material = material;
				skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
				skeletonGraphic.Initialize(false);
			}
			return skeletonGraphic;
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00070654 File Offset: 0x0006E854
		public Dictionary<Texture, Texture> CustomTextureOverride
		{
			get
			{
				return this.customTextureOverride;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0007065C File Offset: 0x0006E85C
		public Dictionary<Texture, Material> CustomMaterialOverride
		{
			get
			{
				return this.customMaterialOverride;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00070664 File Offset: 0x0006E864
		// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x0007066C File Offset: 0x0006E86C
		public Texture OverrideTexture
		{
			get
			{
				return this.overrideTexture;
			}
			set
			{
				this.overrideTexture = value;
				base.canvasRenderer.SetTexture(this.mainTexture);
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00070688 File Offset: 0x0006E888
		public override Texture mainTexture
		{
			get
			{
				if (this.overrideTexture != null)
				{
					return this.overrideTexture;
				}
				return this.baseTexture;
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x000706A8 File Offset: 0x0006E8A8
		protected override void Awake()
		{
			base.Awake();
			if (!this.IsValid)
			{
				this.Initialize(false);
				this.Rebuild(CanvasUpdate.PreRender);
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000706D4 File Offset: 0x0006E8D4
		public override void Rebuild(CanvasUpdate update)
		{
			base.Rebuild(update);
			if (base.canvasRenderer.cull)
			{
				return;
			}
			if (update == CanvasUpdate.PreRender)
			{
				this.UpdateMesh();
			}
			if (this.allowMultipleCanvasRenderers)
			{
				base.canvasRenderer.Clear();
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0007071C File Offset: 0x0006E91C
		protected override void OnDisable()
		{
			base.OnDisable();
			foreach (CanvasRenderer canvasRenderer in this.canvasRenderers)
			{
				canvasRenderer.Clear();
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00070788 File Offset: 0x0006E988
		public virtual void Update()
		{
			if (this.freeze)
			{
				return;
			}
			this.Update((!this.unscaledTime) ? Time.deltaTime : Time.unscaledDeltaTime);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x000707C4 File Offset: 0x0006E9C4
		public virtual void Update(float deltaTime)
		{
			if (!this.IsValid)
			{
				return;
			}
			this.wasUpdatedAfterInit = true;
			if (this.updateMode < UpdateMode.OnlyAnimationStatus)
			{
				return;
			}
			this.UpdateAnimationStatus(deltaTime);
			if (this.updateMode == UpdateMode.OnlyAnimationStatus)
			{
				return;
			}
			this.ApplyAnimation();
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0007080C File Offset: 0x0006EA0C
		protected void UpdateAnimationStatus(float deltaTime)
		{
			deltaTime *= this.timeScale;
			this.skeleton.Update(deltaTime);
			this.state.Update(deltaTime);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0007083C File Offset: 0x0006EA3C
		protected void ApplyAnimation()
		{
			this.state.Apply(this.skeleton);
			if (this.UpdateLocal != null)
			{
				this.UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this.UpdateWorld != null)
			{
				this.UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this.UpdateComplete != null)
			{
				this.UpdateComplete(this);
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x000708B8 File Offset: 0x0006EAB8
		public void LateUpdate()
		{
			if (!this.wasUpdatedAfterInit)
			{
				this.Update(0f);
			}
			if (this.freeze)
			{
				return;
			}
			if (this.updateMode <= UpdateMode.EverythingExceptMesh)
			{
				return;
			}
			this.UpdateMesh();
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000708F0 File Offset: 0x0006EAF0
		public void OnBecameVisible()
		{
			this.updateMode = UpdateMode.FullUpdate;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x000708FC File Offset: 0x0006EAFC
		public void OnBecameInvisible()
		{
			this.updateMode = this.updateWhenInvisible;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0007090C File Offset: 0x0006EB0C
		public void ReapplySeparatorSlotNames()
		{
			if (!this.IsValid)
			{
				return;
			}
			this.separatorSlots.Clear();
			int i = 0;
			int num = this.separatorSlotNames.Length;
			while (i < num)
			{
				string text = this.separatorSlotNames[i];
				if (!(text == string.Empty))
				{
					Slot slot = this.skeleton.FindSlot(text);
					if (slot != null)
					{
						this.separatorSlots.Add(slot);
					}
				}
				i++;
			}
			this.UpdateSeparatorPartParents();
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x00070990 File Offset: 0x0006EB90
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x00070998 File Offset: 0x0006EB98
		public Skeleton Skeleton
		{
			get
			{
				return this.skeleton;
			}
			set
			{
				this.skeleton = value;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x000709A4 File Offset: 0x0006EBA4
		public SkeletonData SkeletonData
		{
			get
			{
				return (this.skeleton != null) ? this.skeleton.data : null;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x000709C4 File Offset: 0x0006EBC4
		public bool IsValid
		{
			get
			{
				return this.skeleton != null;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x000709D4 File Offset: 0x0006EBD4
		public AnimationState AnimationState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x000709DC File Offset: 0x0006EBDC
		public MeshGenerator MeshGenerator
		{
			get
			{
				return this.meshGenerator;
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000709E4 File Offset: 0x0006EBE4
		public Mesh GetLastMesh()
		{
			return this.meshBuffers.GetCurrent().mesh;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x000709F8 File Offset: 0x0006EBF8
		public bool MatchRectTransformWithBounds()
		{
			this.UpdateMesh();
			if (!this.allowMultipleCanvasRenderers)
			{
				return this.MatchRectTransformSingleRenderer();
			}
			return this.MatchRectTransformMultipleRenderers();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00070A18 File Offset: 0x0006EC18
		protected bool MatchRectTransformSingleRenderer()
		{
			Mesh lastMesh = this.GetLastMesh();
			if (lastMesh == null)
			{
				return false;
			}
			if (lastMesh.vertexCount == 0)
			{
				base.rectTransform.sizeDelta = new Vector2(50f, 50f);
				base.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				return false;
			}
			lastMesh.RecalculateBounds();
			this.SetRectTransformBounds(lastMesh.bounds);
			return true;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00070A90 File Offset: 0x0006EC90
		protected bool MatchRectTransformMultipleRenderers()
		{
			bool flag = false;
			Bounds rectTransformBounds = default(Bounds);
			for (int i = 0; i < this.canvasRenderers.Count; i++)
			{
				CanvasRenderer canvasRenderer = this.canvasRenderers[i];
				if (canvasRenderer.gameObject.activeSelf)
				{
					Mesh mesh = this.meshes.Items[i];
					if (!(mesh == null) && mesh.vertexCount != 0)
					{
						mesh.RecalculateBounds();
						Bounds bounds = mesh.bounds;
						if (flag)
						{
							rectTransformBounds.Encapsulate(bounds);
						}
						else
						{
							flag = true;
							rectTransformBounds = bounds;
						}
					}
				}
			}
			if (!flag)
			{
				base.rectTransform.sizeDelta = new Vector2(50f, 50f);
				base.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				return false;
			}
			this.SetRectTransformBounds(rectTransformBounds);
			return true;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00070B80 File Offset: 0x0006ED80
		private void SetRectTransformBounds(Bounds combinedBounds)
		{
			Vector3 size = combinedBounds.size;
			Vector3 center = combinedBounds.center;
			Vector2 pivot = new Vector2(0.5f - center.x / size.x, 0.5f - center.y / size.y);
			base.rectTransform.sizeDelta = size;
			base.rectTransform.pivot = pivot;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00070BEC File Offset: 0x0006EDEC
		public void Clear()
		{
			this.skeleton = null;
			base.canvasRenderer.Clear();
			for (int i = 0; i < this.canvasRenderers.Count; i++)
			{
				this.canvasRenderers[i].Clear();
			}
			foreach (Mesh obj in this.meshes)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.meshes.Clear(true);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00070C9C File Offset: 0x0006EE9C
		public void TrimRenderers()
		{
			List<CanvasRenderer> list = new List<CanvasRenderer>();
			foreach (CanvasRenderer canvasRenderer in this.canvasRenderers)
			{
				if (canvasRenderer.gameObject.activeSelf)
				{
					list.Add(canvasRenderer);
				}
				else if (Application.isEditor && !Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(canvasRenderer.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(canvasRenderer.gameObject);
				}
			}
			this.canvasRenderers = list;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00070D54 File Offset: 0x0006EF54
		public void Initialize(bool overwrite)
		{
			if (this.IsValid && !overwrite)
			{
				return;
			}
			if (this.skeletonDataAsset == null)
			{
				return;
			}
			SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
			if (skeletonData == null)
			{
				return;
			}
			if (this.skeletonDataAsset.atlasAssets.Length <= 0 || this.skeletonDataAsset.atlasAssets[0].MaterialCount <= 0)
			{
				return;
			}
			this.state = new AnimationState(this.skeletonDataAsset.GetAnimationStateData());
			if (this.state == null)
			{
				this.Clear();
				return;
			}
			this.skeleton = new Skeleton(skeletonData)
			{
				ScaleX = (float)((!this.initialFlipX) ? 1 : -1),
				ScaleY = (float)((!this.initialFlipY) ? 1 : -1)
			};
			this.InitMeshBuffers();
			this.baseTexture = this.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.mainTexture;
			base.canvasRenderer.SetTexture(this.mainTexture);
			if (!string.IsNullOrEmpty(this.initialSkinName))
			{
				this.skeleton.SetSkin(this.initialSkinName);
			}
			this.separatorSlots.Clear();
			for (int i = 0; i < this.separatorSlotNames.Length; i++)
			{
				this.separatorSlots.Add(this.skeleton.FindSlot(this.separatorSlotNames[i]));
			}
			this.wasUpdatedAfterInit = false;
			if (!string.IsNullOrEmpty(this.startingAnimation))
			{
				Animation animation = this.skeletonDataAsset.GetSkeletonData(false).FindAnimation(this.startingAnimation);
				if (animation != null)
				{
					this.state.SetAnimation(0, animation, this.startingLoop);
				}
			}
			if (this.OnRebuild != null)
			{
				this.OnRebuild(this);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00070F24 File Offset: 0x0006F124
		public void UpdateMesh()
		{
			if (!this.IsValid)
			{
				return;
			}
			this.skeleton.SetColor(this.color);
			SkeletonRendererInstruction skeletonRendererInstruction = this.currentInstructions;
			if (!this.allowMultipleCanvasRenderers)
			{
				this.UpdateMeshSingleCanvasRenderer();
			}
			else
			{
				this.UpdateMeshMultipleCanvasRenderers(skeletonRendererInstruction);
			}
			if (this.OnMeshAndMaterialsUpdated != null)
			{
				this.OnMeshAndMaterialsUpdated(this);
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00070F8C File Offset: 0x0006F18C
		public bool HasMultipleSubmeshInstructions()
		{
			return this.IsValid && MeshGenerator.RequiresMultipleSubmeshesByDrawOrder(this.skeleton);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00070FA8 File Offset: 0x0006F1A8
		protected void InitMeshBuffers()
		{
			if (this.meshBuffers != null)
			{
				this.meshBuffers.GetNext().Clear();
				this.meshBuffers.GetNext().Clear();
			}
			else
			{
				this.meshBuffers = new DoubleBuffered<MeshRendererBuffers.SmartMesh>();
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00070FF0 File Offset: 0x0006F1F0
		protected void UpdateMeshSingleCanvasRenderer()
		{
			if (this.canvasRenderers.Count > 0)
			{
				this.DisableUnusedCanvasRenderers(0);
			}
			MeshRendererBuffers.SmartMesh next = this.meshBuffers.GetNext();
			MeshGenerator.GenerateSingleSubmeshInstruction(this.currentInstructions, this.skeleton, null);
			bool flag = SkeletonRendererInstruction.GeometryNotEqual(this.currentInstructions, next.instructionUsed);
			this.meshGenerator.Begin();
			if (this.currentInstructions.hasActiveClipping && this.currentInstructions.submeshInstructions.Count > 0)
			{
				this.meshGenerator.AddSubmesh(this.currentInstructions.submeshInstructions.Items[0], flag);
			}
			else
			{
				this.meshGenerator.BuildMeshWithArrays(this.currentInstructions, flag);
			}
			if (base.canvas != null)
			{
				this.meshGenerator.ScaleVertexData(base.canvas.referencePixelsPerUnit);
			}
			if (this.OnPostProcessVertices != null)
			{
				this.OnPostProcessVertices(this.meshGenerator.Buffers);
			}
			Mesh mesh = next.mesh;
			this.meshGenerator.FillVertexData(mesh);
			if (flag)
			{
				this.meshGenerator.FillTriangles(mesh);
			}
			this.meshGenerator.FillLateVertexData(mesh);
			base.canvasRenderer.SetMesh(mesh);
			next.instructionUsed.Set(this.currentInstructions);
			if (this.currentInstructions.submeshInstructions.Count > 0)
			{
				Material material = this.currentInstructions.submeshInstructions.Items[0].material;
				if (material != null && this.baseTexture != material.mainTexture)
				{
					this.baseTexture = material.mainTexture;
					if (this.overrideTexture == null)
					{
						base.canvasRenderer.SetTexture(this.mainTexture);
					}
				}
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000711CC File Offset: 0x0006F3CC
		protected void UpdateMeshMultipleCanvasRenderers(SkeletonRendererInstruction currentInstructions)
		{
			MeshGenerator.GenerateSkeletonRendererInstruction(currentInstructions, this.skeleton, null, (!this.enableSeparatorSlots) ? null : this.separatorSlots, this.enableSeparatorSlots && this.separatorSlots.Count > 0, false);
			int count = currentInstructions.submeshInstructions.Count;
			this.EnsureCanvasRendererCount(count);
			this.EnsureMeshesCount(count);
			this.EnsureSeparatorPartCount();
			Canvas canvas = base.canvas;
			float scale = (!(canvas == null)) ? canvas.referencePixelsPerUnit : 100f;
			Mesh[] items = this.meshes.Items;
			bool flag = this.customMaterialOverride.Count == 0 && this.customTextureOverride.Count == 0;
			int num = 0;
			Transform transform = (this.separatorSlots.Count != 0) ? this.separatorParts[0] : base.transform;
			if (this.updateSeparatorPartLocation)
			{
				for (int i = 0; i < this.separatorParts.Count; i++)
				{
					this.separatorParts[i].position = base.transform.position;
					this.separatorParts[i].rotation = base.transform.rotation;
				}
			}
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				SubmeshInstruction instruction = currentInstructions.submeshInstructions.Items[j];
				this.meshGenerator.Begin();
				this.meshGenerator.AddSubmesh(instruction, true);
				Mesh mesh = items[j];
				this.meshGenerator.ScaleVertexData(scale);
				if (this.OnPostProcessVertices != null)
				{
					this.OnPostProcessVertices(this.meshGenerator.Buffers);
				}
				this.meshGenerator.FillVertexData(mesh);
				this.meshGenerator.FillTriangles(mesh);
				this.meshGenerator.FillLateVertexData(mesh);
				Material material = instruction.material;
				CanvasRenderer canvasRenderer = this.canvasRenderers[j];
				canvasRenderer.gameObject.SetActive(true);
				canvasRenderer.SetMesh(mesh);
				canvasRenderer.materialCount = 1;
				if (canvasRenderer.transform.parent != transform.transform)
				{
					canvasRenderer.transform.SetParent(transform.transform, false);
					canvasRenderer.transform.localPosition = Vector3.zero;
				}
				canvasRenderer.transform.SetSiblingIndex(num2++);
				if (instruction.forceSeparate)
				{
					num2 = 0;
					transform = this.separatorParts[++num];
				}
				if (flag)
				{
					canvasRenderer.SetMaterial(this.materialForRendering, material.mainTexture);
				}
				else
				{
					Texture mainTexture = material.mainTexture;
					Material material2;
					if (!this.customMaterialOverride.TryGetValue(mainTexture, out material2))
					{
						material2 = this.material;
					}
					Texture texture;
					if (!this.customTextureOverride.TryGetValue(mainTexture, out texture))
					{
						texture = mainTexture;
					}
					canvasRenderer.SetMaterial(material2, texture);
				}
			}
			this.DisableUnusedCanvasRenderers(count);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000714E4 File Offset: 0x0006F6E4
		protected void EnsureCanvasRendererCount(int targetCount)
		{
			int count = this.canvasRenderers.Count;
			for (int i = count; i < targetCount; i++)
			{
				GameObject gameObject = new GameObject(string.Format("Renderer{0}", i), new Type[]
				{
					typeof(RectTransform)
				});
				gameObject.transform.SetParent(base.transform, false);
				gameObject.transform.localPosition = Vector3.zero;
				CanvasRenderer item = gameObject.AddComponent<CanvasRenderer>();
				this.canvasRenderers.Add(item);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00071570 File Offset: 0x0006F770
		protected void DisableUnusedCanvasRenderers(int usedCount)
		{
			for (int i = usedCount; i < this.canvasRenderers.Count; i++)
			{
				this.canvasRenderers[i].Clear();
				this.canvasRenderers[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000715C4 File Offset: 0x0006F7C4
		protected void EnsureMeshesCount(int targetCount)
		{
			int count = this.meshes.Count;
			this.meshes.EnsureCapacity(targetCount);
			Mesh[] items = this.meshes.Items;
			for (int i = count; i < targetCount; i++)
			{
				if (items[i] == null)
				{
					items[i] = new Mesh();
				}
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00071620 File Offset: 0x0006F820
		protected void EnsureSeparatorPartCount()
		{
			int num = this.separatorSlots.Count + 1;
			if (num == 1)
			{
				return;
			}
			int count = this.separatorParts.Count;
			for (int i = count; i < num; i++)
			{
				GameObject gameObject = new GameObject(string.Format("{0}[{1}]", "Part", i), new Type[]
				{
					typeof(RectTransform)
				});
				gameObject.transform.SetParent(base.transform, false);
				gameObject.transform.localPosition = Vector3.zero;
				this.separatorParts.Add(gameObject.transform);
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000716C4 File Offset: 0x0006F8C4
		protected void UpdateSeparatorPartParents()
		{
			int num = this.separatorSlots.Count + 1;
			if (num == 1)
			{
				num = 0;
				for (int i = 0; i < this.canvasRenderers.Count; i++)
				{
					CanvasRenderer canvasRenderer = this.canvasRenderers[i];
					if (canvasRenderer.transform.parent.name.Contains("Part"))
					{
						canvasRenderer.transform.SetParent(base.transform, false);
						canvasRenderer.transform.localPosition = Vector3.zero;
					}
				}
			}
			for (int j = 0; j < this.separatorParts.Count; j++)
			{
				bool active = j < num;
				this.separatorParts[j].gameObject.SetActive(active);
			}
		}

		// Token: 0x04000D09 RID: 3337
		public const string SeparatorPartGameObjectName = "Part";

		// Token: 0x04000D0A RID: 3338
		public SkeletonDataAsset skeletonDataAsset;

		// Token: 0x04000D0B RID: 3339
		[SpineSkin("", "skeletonDataAsset", true, false, true)]
		public string initialSkinName;

		// Token: 0x04000D0C RID: 3340
		public bool initialFlipX;

		// Token: 0x04000D0D RID: 3341
		public bool initialFlipY;

		// Token: 0x04000D0E RID: 3342
		[SpineAnimation("", "skeletonDataAsset", true, false)]
		public string startingAnimation;

		// Token: 0x04000D0F RID: 3343
		public bool startingLoop;

		// Token: 0x04000D10 RID: 3344
		public float timeScale = 1f;

		// Token: 0x04000D11 RID: 3345
		public bool freeze;

		// Token: 0x04000D12 RID: 3346
		[SerializeField]
		protected UpdateMode updateMode = UpdateMode.FullUpdate;

		// Token: 0x04000D13 RID: 3347
		public UpdateMode updateWhenInvisible = UpdateMode.FullUpdate;

		// Token: 0x04000D14 RID: 3348
		public bool unscaledTime;

		// Token: 0x04000D15 RID: 3349
		public bool allowMultipleCanvasRenderers;

		// Token: 0x04000D16 RID: 3350
		public List<CanvasRenderer> canvasRenderers = new List<CanvasRenderer>();

		// Token: 0x04000D17 RID: 3351
		[SpineSlot("", "", false, true, false)]
		[SerializeField]
		protected string[] separatorSlotNames = new string[0];

		// Token: 0x04000D18 RID: 3352
		[NonSerialized]
		public readonly List<Slot> separatorSlots = new List<Slot>();

		// Token: 0x04000D19 RID: 3353
		public bool enableSeparatorSlots;

		// Token: 0x04000D1A RID: 3354
		[SerializeField]
		protected List<Transform> separatorParts = new List<Transform>();

		// Token: 0x04000D1B RID: 3355
		public bool updateSeparatorPartLocation = true;

		// Token: 0x04000D1C RID: 3356
		private bool wasUpdatedAfterInit = true;

		// Token: 0x04000D1D RID: 3357
		private Texture baseTexture;

		// Token: 0x04000D1E RID: 3358
		[NonSerialized]
		private readonly Dictionary<Texture, Texture> customTextureOverride = new Dictionary<Texture, Texture>();

		// Token: 0x04000D1F RID: 3359
		[NonSerialized]
		private readonly Dictionary<Texture, Material> customMaterialOverride = new Dictionary<Texture, Material>();

		// Token: 0x04000D20 RID: 3360
		private Texture overrideTexture;

		// Token: 0x04000D21 RID: 3361
		protected Skeleton skeleton;

		// Token: 0x04000D22 RID: 3362
		protected AnimationState state;

		// Token: 0x04000D23 RID: 3363
		[SerializeField]
		protected MeshGenerator meshGenerator = new MeshGenerator();

		// Token: 0x04000D24 RID: 3364
		private DoubleBuffered<MeshRendererBuffers.SmartMesh> meshBuffers;

		// Token: 0x04000D25 RID: 3365
		private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

		// Token: 0x04000D26 RID: 3366
		private readonly ExposedList<Mesh> meshes = new ExposedList<Mesh>();

		// Token: 0x02000240 RID: 576
		// (Invoke) Token: 0x06001204 RID: 4612
		public delegate void SkeletonRendererDelegate(SkeletonGraphic skeletonGraphic);
	}
}
