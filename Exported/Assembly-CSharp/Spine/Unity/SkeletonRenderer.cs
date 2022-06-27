using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Spine.Unity
{
	// Token: 0x020001F2 RID: 498
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	[HelpURL("http://esotericsoftware.com/spine-unity-rendering")]
	public class SkeletonRenderer : MonoBehaviour, IHasSkeletonDataAsset, ISkeletonComponent
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600105E RID: 4190 RVA: 0x0007304C File Offset: 0x0007124C
		// (remove) Token: 0x0600105F RID: 4191 RVA: 0x00073068 File Offset: 0x00071268
		private event SkeletonRenderer.InstructionDelegate generateMeshOverride;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001060 RID: 4192 RVA: 0x00073084 File Offset: 0x00071284
		// (remove) Token: 0x06001061 RID: 4193 RVA: 0x000730D4 File Offset: 0x000712D4
		public event SkeletonRenderer.InstructionDelegate GenerateMeshOverride
		{
			add
			{
				this.generateMeshOverride = (SkeletonRenderer.InstructionDelegate)Delegate.Combine(this.generateMeshOverride, value);
				if (this.disableRenderingOnOverride && this.generateMeshOverride != null)
				{
					this.Initialize(false);
					this.meshRenderer.enabled = false;
				}
			}
			remove
			{
				this.generateMeshOverride = (SkeletonRenderer.InstructionDelegate)Delegate.Remove(this.generateMeshOverride, value);
				if (this.disableRenderingOnOverride && this.generateMeshOverride == null)
				{
					this.Initialize(false);
					this.meshRenderer.enabled = true;
				}
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001062 RID: 4194 RVA: 0x00073124 File Offset: 0x00071324
		// (remove) Token: 0x06001063 RID: 4195 RVA: 0x00073140 File Offset: 0x00071340
		public event MeshGeneratorDelegate OnPostProcessVertices;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001064 RID: 4196 RVA: 0x0007315C File Offset: 0x0007135C
		// (remove) Token: 0x06001065 RID: 4197 RVA: 0x00073178 File Offset: 0x00071378
		public event SkeletonRenderer.SkeletonRendererDelegate OnRebuild;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001066 RID: 4198 RVA: 0x00073194 File Offset: 0x00071394
		// (remove) Token: 0x06001067 RID: 4199 RVA: 0x000731B0 File Offset: 0x000713B0
		public event SkeletonRenderer.SkeletonRendererDelegate OnMeshAndMaterialsUpdated;

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x000731CC File Offset: 0x000713CC
		// (set) Token: 0x06001069 RID: 4201 RVA: 0x000731D4 File Offset: 0x000713D4
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

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x000731E0 File Offset: 0x000713E0
		public Dictionary<Material, Material> CustomMaterialOverride
		{
			get
			{
				return this.customMaterialOverride;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x000731E8 File Offset: 0x000713E8
		public Dictionary<Slot, Material> CustomSlotMaterials
		{
			get
			{
				return this.customSlotMaterials;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x000731F0 File Offset: 0x000713F0
		public Skeleton Skeleton
		{
			get
			{
				this.Initialize(false);
				return this.skeleton;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x00073200 File Offset: 0x00071400
		public SkeletonDataAsset SkeletonDataAsset
		{
			get
			{
				return this.skeletonDataAsset;
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00073208 File Offset: 0x00071408
		public static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			return SkeletonRenderer.AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0007321C File Offset: 0x0007141C
		public static T AddSpineComponent<T>(GameObject gameObject, SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			T t = gameObject.AddComponent<T>();
			if (skeletonDataAsset != null)
			{
				t.skeletonDataAsset = skeletonDataAsset;
				t.Initialize(false);
			}
			return t;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00073258 File Offset: 0x00071458
		public void SetMeshSettings(MeshGenerator.Settings settings)
		{
			this.calculateTangents = settings.calculateTangents;
			this.immutableTriangles = settings.immutableTriangles;
			this.pmaVertexColors = settings.pmaVertexColors;
			this.tintBlack = settings.tintBlack;
			this.useClipping = settings.useClipping;
			this.zSpacing = settings.zSpacing;
			this.meshGenerator.settings = settings;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000732C0 File Offset: 0x000714C0
		public virtual void Awake()
		{
			this.Initialize(false);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x000732CC File Offset: 0x000714CC
		private void OnDisable()
		{
			if (this.clearStateOnDisable && this.valid)
			{
				this.ClearState();
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000732EC File Offset: 0x000714EC
		private void OnDestroy()
		{
			this.rendererBuffers.Dispose();
			this.valid = false;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00073300 File Offset: 0x00071500
		public virtual void ClearState()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				component.sharedMesh = null;
			}
			this.currentInstructions.Clear();
			if (this.skeleton != null)
			{
				this.skeleton.SetToSetupPose();
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00073348 File Offset: 0x00071548
		public void EnsureMeshGeneratorCapacity(int minimumVertexCount)
		{
			this.meshGenerator.EnsureVertexCapacity(minimumVertexCount, false, false, false);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0007335C File Offset: 0x0007155C
		public virtual void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			this.currentInstructions.Clear();
			this.rendererBuffers.Clear();
			this.meshGenerator.Begin();
			this.skeleton = null;
			this.valid = false;
			if (this.skeletonDataAsset == null)
			{
				return;
			}
			SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
			if (skeletonData == null)
			{
				return;
			}
			this.valid = true;
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.rendererBuffers.Initialize();
			this.skeleton = new Skeleton(skeletonData)
			{
				ScaleX = (float)((!this.initialFlipX) ? 1 : -1),
				ScaleY = (float)((!this.initialFlipY) ? 1 : -1)
			};
			if (!string.IsNullOrEmpty(this.initialSkinName) && !string.Equals(this.initialSkinName, "default", StringComparison.Ordinal))
			{
				this.skeleton.SetSkin(this.initialSkinName);
			}
			this.separatorSlots.Clear();
			for (int i = 0; i < this.separatorSlotNames.Length; i++)
			{
				this.separatorSlots.Add(this.skeleton.FindSlot(this.separatorSlotNames[i]));
			}
			this.LateUpdate();
			if (this.OnRebuild != null)
			{
				this.OnRebuild(this);
			}
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000734D0 File Offset: 0x000716D0
		public virtual void LateUpdate()
		{
			if (!this.valid)
			{
				return;
			}
			if (this.updateMode <= UpdateMode.EverythingExceptMesh)
			{
				return;
			}
			bool flag = this.generateMeshOverride != null;
			if (!this.meshRenderer.enabled && !flag)
			{
				return;
			}
			SkeletonRendererInstruction skeletonRendererInstruction = this.currentInstructions;
			ExposedList<SubmeshInstruction> submeshInstructions = skeletonRendererInstruction.submeshInstructions;
			MeshRendererBuffers.SmartMesh nextMesh = this.rendererBuffers.GetNextMesh();
			bool flag2;
			if (this.singleSubmesh)
			{
				MeshGenerator.GenerateSingleSubmeshInstruction(skeletonRendererInstruction, this.skeleton, this.skeletonDataAsset.atlasAssets[0].PrimaryMaterial);
				if (this.customMaterialOverride.Count > 0)
				{
					MeshGenerator.TryReplaceMaterials(submeshInstructions, this.customMaterialOverride);
				}
				this.meshGenerator.settings = new MeshGenerator.Settings
				{
					pmaVertexColors = this.pmaVertexColors,
					zSpacing = this.zSpacing,
					useClipping = this.useClipping,
					tintBlack = this.tintBlack,
					calculateTangents = this.calculateTangents,
					addNormals = this.addNormals
				};
				this.meshGenerator.Begin();
				flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
				if (skeletonRendererInstruction.hasActiveClipping)
				{
					this.meshGenerator.AddSubmesh(submeshInstructions.Items[0], flag2);
				}
				else
				{
					this.meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
				}
			}
			else
			{
				MeshGenerator.GenerateSkeletonRendererInstruction(skeletonRendererInstruction, this.skeleton, this.customSlotMaterials, this.separatorSlots, flag, this.immutableTriangles);
				if (this.customMaterialOverride.Count > 0)
				{
					MeshGenerator.TryReplaceMaterials(submeshInstructions, this.customMaterialOverride);
				}
				if (flag)
				{
					this.generateMeshOverride(skeletonRendererInstruction);
					if (this.disableRenderingOnOverride)
					{
						return;
					}
				}
				flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
				this.meshGenerator.settings = new MeshGenerator.Settings
				{
					pmaVertexColors = this.pmaVertexColors,
					zSpacing = this.zSpacing,
					useClipping = this.useClipping,
					tintBlack = this.tintBlack,
					calculateTangents = this.calculateTangents,
					addNormals = this.addNormals
				};
				this.meshGenerator.Begin();
				if (skeletonRendererInstruction.hasActiveClipping)
				{
					this.meshGenerator.BuildMesh(skeletonRendererInstruction, flag2);
				}
				else
				{
					this.meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
				}
			}
			if (this.OnPostProcessVertices != null)
			{
				this.OnPostProcessVertices(this.meshGenerator.Buffers);
			}
			Mesh mesh = nextMesh.mesh;
			this.meshGenerator.FillVertexData(mesh);
			this.rendererBuffers.UpdateSharedMaterials(submeshInstructions);
			bool flag3 = this.rendererBuffers.MaterialsChangedInLastUpdate();
			if (flag2)
			{
				this.meshGenerator.FillTriangles(mesh);
				this.meshRenderer.sharedMaterials = this.rendererBuffers.GetUpdatedSharedMaterialsArray();
			}
			else if (flag3)
			{
				this.meshRenderer.sharedMaterials = this.rendererBuffers.GetUpdatedSharedMaterialsArray();
			}
			if (flag3 && this.maskMaterials.AnyMaterialCreated)
			{
				this.maskMaterials = new SkeletonRenderer.SpriteMaskInteractionMaterials();
			}
			this.meshGenerator.FillLateVertexData(mesh);
			this.meshFilter.sharedMesh = mesh;
			nextMesh.instructionUsed.Set(skeletonRendererInstruction);
			if (this.meshRenderer != null)
			{
				this.AssignSpriteMaskMaterials();
			}
			if (this.OnMeshAndMaterialsUpdated != null)
			{
				this.OnMeshAndMaterialsUpdated(this);
			}
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0007384C File Offset: 0x00071A4C
		public void OnBecameVisible()
		{
			this.updateMode = UpdateMode.FullUpdate;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00073858 File Offset: 0x00071A58
		public void OnBecameInvisible()
		{
			this.updateMode = this.updateWhenInvisible;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00073868 File Offset: 0x00071A68
		public void FindAndApplySeparatorSlots(string startsWith, bool clearExistingSeparators = true, bool updateStringArray = false)
		{
			if (string.IsNullOrEmpty(startsWith))
			{
				return;
			}
			this.FindAndApplySeparatorSlots((string slotName) => slotName.StartsWith(startsWith), clearExistingSeparators, updateStringArray);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000738A8 File Offset: 0x00071AA8
		public void FindAndApplySeparatorSlots(Func<string, bool> slotNamePredicate, bool clearExistingSeparators = true, bool updateStringArray = false)
		{
			if (slotNamePredicate == null)
			{
				return;
			}
			if (!this.valid)
			{
				return;
			}
			if (clearExistingSeparators)
			{
				this.separatorSlots.Clear();
			}
			ExposedList<Slot> slots = this.skeleton.slots;
			foreach (Slot slot in slots)
			{
				if (slotNamePredicate(slot.data.name))
				{
					this.separatorSlots.Add(slot);
				}
			}
			if (updateStringArray)
			{
				List<string> list = new List<string>();
				foreach (Slot slot2 in this.skeleton.slots)
				{
					string name = slot2.data.name;
					if (slotNamePredicate(name))
					{
						list.Add(name);
					}
				}
				if (!clearExistingSeparators)
				{
					string[] array = this.separatorSlotNames;
					foreach (string item in array)
					{
						list.Add(item);
					}
				}
				this.separatorSlotNames = list.ToArray();
			}
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00073A20 File Offset: 0x00071C20
		public void ReapplySeparatorSlotNames()
		{
			if (!this.valid)
			{
				return;
			}
			this.separatorSlots.Clear();
			int i = 0;
			int num = this.separatorSlotNames.Length;
			while (i < num)
			{
				Slot slot = this.skeleton.FindSlot(this.separatorSlotNames[i]);
				if (slot != null)
				{
					this.separatorSlots.Add(slot);
				}
				i++;
			}
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00073A88 File Offset: 0x00071C88
		private void AssignSpriteMaskMaterials()
		{
			if (Application.isPlaying && this.maskInteraction != SpriteMaskInteraction.None && this.maskMaterials.materialsMaskDisabled.Length == 0)
			{
				this.maskMaterials.materialsMaskDisabled = this.meshRenderer.sharedMaterials;
			}
			if (this.maskMaterials.materialsMaskDisabled.Length > 0 && this.maskMaterials.materialsMaskDisabled[0] != null && this.maskInteraction == SpriteMaskInteraction.None)
			{
				this.meshRenderer.materials = this.maskMaterials.materialsMaskDisabled;
			}
			else if (this.maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
			{
				if ((this.maskMaterials.materialsInsideMask.Length == 0 || this.maskMaterials.materialsInsideMask[0] == null) && !this.InitSpriteMaskMaterialsInsideMask())
				{
					return;
				}
				this.meshRenderer.materials = this.maskMaterials.materialsInsideMask;
			}
			else if (this.maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
			{
				if ((this.maskMaterials.materialsOutsideMask.Length == 0 || this.maskMaterials.materialsOutsideMask[0] == null) && !this.InitSpriteMaskMaterialsOutsideMask())
				{
					return;
				}
				this.meshRenderer.materials = this.maskMaterials.materialsOutsideMask;
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00073BD8 File Offset: 0x00071DD8
		private bool InitSpriteMaskMaterialsInsideMask()
		{
			return this.InitSpriteMaskMaterialsForMaskType(CompareFunction.LessEqual, ref this.maskMaterials.materialsInsideMask);
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00073BEC File Offset: 0x00071DEC
		private bool InitSpriteMaskMaterialsOutsideMask()
		{
			return this.InitSpriteMaskMaterialsForMaskType(CompareFunction.Greater, ref this.maskMaterials.materialsOutsideMask);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00073C00 File Offset: 0x00071E00
		private bool InitSpriteMaskMaterialsForMaskType(CompareFunction maskFunction, ref Material[] materialsToFill)
		{
			Material[] materialsMaskDisabled = this.maskMaterials.materialsMaskDisabled;
			materialsToFill = new Material[materialsMaskDisabled.Length];
			for (int i = 0; i < materialsMaskDisabled.Length; i++)
			{
				Material material = new Material(materialsMaskDisabled[i]);
				material.SetFloat(SkeletonRenderer.STENCIL_COMP_PARAM_ID, (float)maskFunction);
				materialsToFill[i] = material;
			}
			return true;
		}

		// Token: 0x04000D5E RID: 3422
		public const CompareFunction STENCIL_COMP_MASKINTERACTION_NONE = CompareFunction.Always;

		// Token: 0x04000D5F RID: 3423
		public const CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE = CompareFunction.LessEqual;

		// Token: 0x04000D60 RID: 3424
		public const CompareFunction STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE = CompareFunction.Greater;

		// Token: 0x04000D61 RID: 3425
		public SkeletonDataAsset skeletonDataAsset;

		// Token: 0x04000D62 RID: 3426
		[SpineSkin("", "", true, false, true)]
		public string initialSkinName;

		// Token: 0x04000D63 RID: 3427
		public bool initialFlipX;

		// Token: 0x04000D64 RID: 3428
		public bool initialFlipY;

		// Token: 0x04000D65 RID: 3429
		[SerializeField]
		protected UpdateMode updateMode = UpdateMode.FullUpdate;

		// Token: 0x04000D66 RID: 3430
		public UpdateMode updateWhenInvisible = UpdateMode.FullUpdate;

		// Token: 0x04000D67 RID: 3431
		[FormerlySerializedAs("submeshSeparators")]
		[SerializeField]
		[SpineSlot("", "", false, true, false)]
		protected string[] separatorSlotNames = new string[0];

		// Token: 0x04000D68 RID: 3432
		[NonSerialized]
		public readonly List<Slot> separatorSlots = new List<Slot>();

		// Token: 0x04000D69 RID: 3433
		[Range(-0.1f, 0f)]
		public float zSpacing;

		// Token: 0x04000D6A RID: 3434
		public bool useClipping = true;

		// Token: 0x04000D6B RID: 3435
		public bool immutableTriangles;

		// Token: 0x04000D6C RID: 3436
		public bool pmaVertexColors = true;

		// Token: 0x04000D6D RID: 3437
		public bool clearStateOnDisable;

		// Token: 0x04000D6E RID: 3438
		public bool tintBlack;

		// Token: 0x04000D6F RID: 3439
		public bool singleSubmesh;

		// Token: 0x04000D70 RID: 3440
		[FormerlySerializedAs("calculateNormals")]
		public bool addNormals;

		// Token: 0x04000D71 RID: 3441
		public bool calculateTangents;

		// Token: 0x04000D72 RID: 3442
		public SpriteMaskInteraction maskInteraction;

		// Token: 0x04000D73 RID: 3443
		public SkeletonRenderer.SpriteMaskInteractionMaterials maskMaterials = new SkeletonRenderer.SpriteMaskInteractionMaterials();

		// Token: 0x04000D74 RID: 3444
		public static readonly int STENCIL_COMP_PARAM_ID = Shader.PropertyToID("_StencilComp");

		// Token: 0x04000D75 RID: 3445
		public bool disableRenderingOnOverride = true;

		// Token: 0x04000D76 RID: 3446
		[NonSerialized]
		private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();

		// Token: 0x04000D77 RID: 3447
		[NonSerialized]
		private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();

		// Token: 0x04000D78 RID: 3448
		[NonSerialized]
		private readonly SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

		// Token: 0x04000D79 RID: 3449
		private readonly MeshGenerator meshGenerator = new MeshGenerator();

		// Token: 0x04000D7A RID: 3450
		[NonSerialized]
		private readonly MeshRendererBuffers rendererBuffers = new MeshRendererBuffers();

		// Token: 0x04000D7B RID: 3451
		private MeshRenderer meshRenderer;

		// Token: 0x04000D7C RID: 3452
		private MeshFilter meshFilter;

		// Token: 0x04000D7D RID: 3453
		[NonSerialized]
		public bool valid;

		// Token: 0x04000D7E RID: 3454
		[NonSerialized]
		public Skeleton skeleton;

		// Token: 0x020001F3 RID: 499
		[Serializable]
		public class SpriteMaskInteractionMaterials
		{
			// Token: 0x1700030D RID: 781
			// (get) Token: 0x06001082 RID: 4226 RVA: 0x00073C8C File Offset: 0x00071E8C
			public bool AnyMaterialCreated
			{
				get
				{
					return this.materialsMaskDisabled.Length > 0 || this.materialsInsideMask.Length > 0 || this.materialsOutsideMask.Length > 0;
				}
			}

			// Token: 0x04000D83 RID: 3459
			public Material[] materialsMaskDisabled = new Material[0];

			// Token: 0x04000D84 RID: 3460
			public Material[] materialsInsideMask = new Material[0];

			// Token: 0x04000D85 RID: 3461
			public Material[] materialsOutsideMask = new Material[0];
		}

		// Token: 0x0200023E RID: 574
		// (Invoke) Token: 0x060011FC RID: 4604
		public delegate void InstructionDelegate(SkeletonRendererInstruction instruction);

		// Token: 0x0200023F RID: 575
		// (Invoke) Token: 0x06001200 RID: 4608
		public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);
	}
}
