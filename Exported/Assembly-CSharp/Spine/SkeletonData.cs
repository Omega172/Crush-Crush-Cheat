using System;

namespace Spine
{
	// Token: 0x020001C6 RID: 454
	public class SkeletonData
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00066F40 File Offset: 0x00065140
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x00066F48 File Offset: 0x00065148
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00066F54 File Offset: 0x00065154
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00066F5C File Offset: 0x0006515C
		public ExposedList<SlotData> Slots
		{
			get
			{
				return this.slots;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00066F64 File Offset: 0x00065164
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x00066F6C File Offset: 0x0006516C
		public ExposedList<Skin> Skins
		{
			get
			{
				return this.skins;
			}
			set
			{
				this.skins = value;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x00066F78 File Offset: 0x00065178
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x00066F80 File Offset: 0x00065180
		public Skin DefaultSkin
		{
			get
			{
				return this.defaultSkin;
			}
			set
			{
				this.defaultSkin = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00066F8C File Offset: 0x0006518C
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x00066F94 File Offset: 0x00065194
		public ExposedList<EventData> Events
		{
			get
			{
				return this.events;
			}
			set
			{
				this.events = value;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00066FA0 File Offset: 0x000651A0
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x00066FA8 File Offset: 0x000651A8
		public ExposedList<Animation> Animations
		{
			get
			{
				return this.animations;
			}
			set
			{
				this.animations = value;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00066FB4 File Offset: 0x000651B4
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x00066FBC File Offset: 0x000651BC
		public ExposedList<IkConstraintData> IkConstraints
		{
			get
			{
				return this.ikConstraints;
			}
			set
			{
				this.ikConstraints = value;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00066FC8 File Offset: 0x000651C8
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x00066FD0 File Offset: 0x000651D0
		public ExposedList<TransformConstraintData> TransformConstraints
		{
			get
			{
				return this.transformConstraints;
			}
			set
			{
				this.transformConstraints = value;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00066FDC File Offset: 0x000651DC
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x00066FE4 File Offset: 0x000651E4
		public ExposedList<PathConstraintData> PathConstraints
		{
			get
			{
				return this.pathConstraints;
			}
			set
			{
				this.pathConstraints = value;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00066FF0 File Offset: 0x000651F0
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x00066FF8 File Offset: 0x000651F8
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00067004 File Offset: 0x00065204
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0006700C File Offset: 0x0006520C
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00067018 File Offset: 0x00065218
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x00067020 File Offset: 0x00065220
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0006702C File Offset: 0x0006522C
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x00067034 File Offset: 0x00065234
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x00067040 File Offset: 0x00065240
		// (set) Token: 0x06000E80 RID: 3712 RVA: 0x00067048 File Offset: 0x00065248
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00067054 File Offset: 0x00065254
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x0006705C File Offset: 0x0006525C
		public string Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00067068 File Offset: 0x00065268
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x00067070 File Offset: 0x00065270
		public string ImagesPath
		{
			get
			{
				return this.imagesPath;
			}
			set
			{
				this.imagesPath = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0006707C File Offset: 0x0006527C
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x00067084 File Offset: 0x00065284
		public string AudioPath
		{
			get
			{
				return this.audioPath;
			}
			set
			{
				this.audioPath = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x00067090 File Offset: 0x00065290
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x00067098 File Offset: 0x00065298
		public float Fps
		{
			get
			{
				return this.fps;
			}
			set
			{
				this.fps = value;
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x000670A4 File Offset: 0x000652A4
		public BoneData FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName", "boneName cannot be null.");
			}
			ExposedList<BoneData> exposedList = this.bones;
			BoneData[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				BoneData boneData = items[i];
				if (boneData.name == boneName)
				{
					return boneData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0006710C File Offset: 0x0006530C
		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName", "boneName cannot be null.");
			}
			ExposedList<BoneData> exposedList = this.bones;
			BoneData[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (items[i].name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0006716C File Offset: 0x0006536C
		public SlotData FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName", "slotName cannot be null.");
			}
			ExposedList<SlotData> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				SlotData slotData = exposedList.Items[i];
				if (slotData.name == slotName)
				{
					return slotData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000671CC File Offset: 0x000653CC
		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName", "slotName cannot be null.");
			}
			ExposedList<SlotData> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].name == slotName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0006722C File Offset: 0x0006542C
		public Skin FindSkin(string skinName)
		{
			if (skinName == null)
			{
				throw new ArgumentNullException("skinName", "skinName cannot be null.");
			}
			foreach (Skin skin in this.skins)
			{
				if (skin.name == skinName)
				{
					return skin;
				}
			}
			return null;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x000672BC File Offset: 0x000654BC
		public EventData FindEvent(string eventDataName)
		{
			if (eventDataName == null)
			{
				throw new ArgumentNullException("eventDataName", "eventDataName cannot be null.");
			}
			foreach (EventData eventData in this.events)
			{
				if (eventData.name == eventDataName)
				{
					return eventData;
				}
			}
			return null;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0006734C File Offset: 0x0006554C
		public Animation FindAnimation(string animationName)
		{
			if (animationName == null)
			{
				throw new ArgumentNullException("animationName", "animationName cannot be null.");
			}
			ExposedList<Animation> exposedList = this.animations;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Animation animation = exposedList.Items[i];
				if (animation.name == animationName)
				{
					return animation;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000673AC File Offset: 0x000655AC
		public IkConstraintData FindIkConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<IkConstraintData> exposedList = this.ikConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				IkConstraintData ikConstraintData = exposedList.Items[i];
				if (ikConstraintData.name == constraintName)
				{
					return ikConstraintData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0006740C File Offset: 0x0006560C
		public TransformConstraintData FindTransformConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<TransformConstraintData> exposedList = this.transformConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				TransformConstraintData transformConstraintData = exposedList.Items[i];
				if (transformConstraintData.name == constraintName)
				{
					return transformConstraintData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0006746C File Offset: 0x0006566C
		public PathConstraintData FindPathConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<PathConstraintData> exposedList = this.pathConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				PathConstraintData pathConstraintData = exposedList.Items[i];
				if (pathConstraintData.name.Equals(constraintName))
				{
					return pathConstraintData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000674CC File Offset: 0x000656CC
		public int FindPathConstraintIndex(string pathConstraintName)
		{
			if (pathConstraintName == null)
			{
				throw new ArgumentNullException("pathConstraintName", "pathConstraintName cannot be null.");
			}
			ExposedList<PathConstraintData> exposedList = this.pathConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (exposedList.Items[i].name.Equals(pathConstraintName))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0006752C File Offset: 0x0006572C
		public override string ToString()
		{
			return this.name ?? base.ToString();
		}

		// Token: 0x04000C38 RID: 3128
		internal string name;

		// Token: 0x04000C39 RID: 3129
		internal ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04000C3A RID: 3130
		internal ExposedList<SlotData> slots = new ExposedList<SlotData>();

		// Token: 0x04000C3B RID: 3131
		internal ExposedList<Skin> skins = new ExposedList<Skin>();

		// Token: 0x04000C3C RID: 3132
		internal Skin defaultSkin;

		// Token: 0x04000C3D RID: 3133
		internal ExposedList<EventData> events = new ExposedList<EventData>();

		// Token: 0x04000C3E RID: 3134
		internal ExposedList<Animation> animations = new ExposedList<Animation>();

		// Token: 0x04000C3F RID: 3135
		internal ExposedList<IkConstraintData> ikConstraints = new ExposedList<IkConstraintData>();

		// Token: 0x04000C40 RID: 3136
		internal ExposedList<TransformConstraintData> transformConstraints = new ExposedList<TransformConstraintData>();

		// Token: 0x04000C41 RID: 3137
		internal ExposedList<PathConstraintData> pathConstraints = new ExposedList<PathConstraintData>();

		// Token: 0x04000C42 RID: 3138
		internal float x;

		// Token: 0x04000C43 RID: 3139
		internal float y;

		// Token: 0x04000C44 RID: 3140
		internal float width;

		// Token: 0x04000C45 RID: 3141
		internal float height;

		// Token: 0x04000C46 RID: 3142
		internal string version;

		// Token: 0x04000C47 RID: 3143
		internal string hash;

		// Token: 0x04000C48 RID: 3144
		internal float fps;

		// Token: 0x04000C49 RID: 3145
		internal string imagesPath;

		// Token: 0x04000C4A RID: 3146
		internal string audioPath;
	}
}
