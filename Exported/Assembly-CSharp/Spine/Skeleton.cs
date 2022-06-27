using System;

namespace Spine
{
	// Token: 0x020001BF RID: 447
	public class Skeleton
	{
		// Token: 0x06000DE3 RID: 3555 RVA: 0x00061FDC File Offset: 0x000601DC
		public Skeleton(SkeletonData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			this.data = data;
			this.bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				Bone item;
				if (boneData.parent == null)
				{
					item = new Bone(boneData, this, null);
				}
				else
				{
					Bone bone = this.bones.Items[boneData.parent.index];
					item = new Bone(boneData, this, bone);
					bone.children.Add(item);
				}
				this.bones.Add(item);
			}
			this.slots = new ExposedList<Slot>(data.slots.Count);
			this.drawOrder = new ExposedList<Slot>(data.slots.Count);
			foreach (SlotData slotData in data.slots)
			{
				Bone bone2 = this.bones.Items[slotData.boneData.index];
				Slot item2 = new Slot(slotData, bone2);
				this.slots.Add(item2);
				this.drawOrder.Add(item2);
			}
			this.ikConstraints = new ExposedList<IkConstraint>(data.ikConstraints.Count);
			foreach (IkConstraintData ikConstraintData in data.ikConstraints)
			{
				this.ikConstraints.Add(new IkConstraint(ikConstraintData, this));
			}
			this.transformConstraints = new ExposedList<TransformConstraint>(data.transformConstraints.Count);
			foreach (TransformConstraintData transformConstraintData in data.transformConstraints)
			{
				this.transformConstraints.Add(new TransformConstraint(transformConstraintData, this));
			}
			this.pathConstraints = new ExposedList<PathConstraint>(data.pathConstraints.Count);
			foreach (PathConstraintData pathConstraintData in data.pathConstraints)
			{
				this.pathConstraints.Add(new PathConstraint(pathConstraintData, this));
			}
			this.UpdateCache();
			this.UpdateWorldTransform();
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x00062354 File Offset: 0x00060554
		public SkeletonData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0006235C File Offset: 0x0006055C
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x00062364 File Offset: 0x00060564
		public ExposedList<IUpdatable> UpdateCacheList
		{
			get
			{
				return this.updateCache;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0006236C File Offset: 0x0006056C
		public ExposedList<Slot> Slots
		{
			get
			{
				return this.slots;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00062374 File Offset: 0x00060574
		public ExposedList<Slot> DrawOrder
		{
			get
			{
				return this.drawOrder;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0006237C File Offset: 0x0006057C
		public ExposedList<IkConstraint> IkConstraints
		{
			get
			{
				return this.ikConstraints;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00062384 File Offset: 0x00060584
		public ExposedList<PathConstraint> PathConstraints
		{
			get
			{
				return this.pathConstraints;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0006238C File Offset: 0x0006058C
		public ExposedList<TransformConstraint> TransformConstraints
		{
			get
			{
				return this.transformConstraints;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00062394 File Offset: 0x00060594
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0006239C File Offset: 0x0006059C
		public Skin Skin
		{
			get
			{
				return this.skin;
			}
			set
			{
				this.SetSkin(value);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x000623A8 File Offset: 0x000605A8
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x000623B0 File Offset: 0x000605B0
		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x000623BC File Offset: 0x000605BC
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x000623C4 File Offset: 0x000605C4
		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x000623D0 File Offset: 0x000605D0
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x000623D8 File Offset: 0x000605D8
		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x000623E4 File Offset: 0x000605E4
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x000623EC File Offset: 0x000605EC
		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x000623F8 File Offset: 0x000605F8
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x00062400 File Offset: 0x00060600
		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0006240C File Offset: 0x0006060C
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x00062414 File Offset: 0x00060614
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x00062420 File Offset: 0x00060620
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x00062428 File Offset: 0x00060628
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x00062434 File Offset: 0x00060634
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0006243C File Offset: 0x0006063C
		public float ScaleX
		{
			get
			{
				return this.scaleX;
			}
			set
			{
				this.scaleX = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x00062448 File Offset: 0x00060648
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x00062464 File Offset: 0x00060664
		public float ScaleY
		{
			get
			{
				return this.scaleY * (float)((!Bone.yDown) ? 1 : -1);
			}
			set
			{
				this.scaleY = value;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x00062470 File Offset: 0x00060670
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x00062480 File Offset: 0x00060680
		[Obsolete("Use ScaleX instead. FlipX is when ScaleX is negative.")]
		public bool FlipX
		{
			get
			{
				return this.scaleX < 0f;
			}
			set
			{
				this.scaleX = ((!value) ? 1f : -1f);
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x000624A0 File Offset: 0x000606A0
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x000624B0 File Offset: 0x000606B0
		[Obsolete("Use ScaleY instead. FlipY is when ScaleY is negative.")]
		public bool FlipY
		{
			get
			{
				return this.scaleY < 0f;
			}
			set
			{
				this.scaleY = ((!value) ? 1f : -1f);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000624D0 File Offset: 0x000606D0
		public Bone RootBone
		{
			get
			{
				return (this.bones.Count != 0) ? this.bones.Items[0] : null;
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000624F8 File Offset: 0x000606F8
		public void UpdateCache()
		{
			ExposedList<IUpdatable> exposedList = this.updateCache;
			exposedList.Clear(true);
			this.updateCacheReset.Clear(true);
			int num = this.bones.Items.Length;
			ExposedList<Bone> exposedList2 = this.bones;
			for (int i = 0; i < num; i++)
			{
				Bone bone = exposedList2.Items[i];
				bone.sorted = bone.data.skinRequired;
				bone.active = !bone.sorted;
			}
			if (this.skin != null)
			{
				object[] items = this.skin.bones.Items;
				int j = 0;
				int count = this.skin.bones.Count;
				while (j < count)
				{
					Bone bone2 = exposedList2.Items[((BoneData)items[j]).index];
					do
					{
						bone2.sorted = false;
						bone2.active = true;
						bone2 = bone2.parent;
					}
					while (bone2 != null);
					j++;
				}
			}
			int count2 = this.ikConstraints.Count;
			int count3 = this.transformConstraints.Count;
			int count4 = this.pathConstraints.Count;
			ExposedList<IkConstraint> exposedList3 = this.ikConstraints;
			ExposedList<TransformConstraint> exposedList4 = this.transformConstraints;
			ExposedList<PathConstraint> exposedList5 = this.pathConstraints;
			int num2 = count2 + count3 + count4;
			int k = 0;
			IL_20D:
			while (k < num2)
			{
				for (int l = 0; l < count2; l++)
				{
					IkConstraint ikConstraint = exposedList3.Items[l];
					if (ikConstraint.data.order == k)
					{
						this.SortIkConstraint(ikConstraint);
						IL_207:
						k++;
						goto IL_20D;
					}
				}
				for (int m = 0; m < count3; m++)
				{
					TransformConstraint transformConstraint = exposedList4.Items[m];
					if (transformConstraint.data.order == k)
					{
						this.SortTransformConstraint(transformConstraint);
						goto IL_207;
					}
				}
				for (int n = 0; n < count4; n++)
				{
					PathConstraint pathConstraint = exposedList5.Items[n];
					if (pathConstraint.data.order == k)
					{
						this.SortPathConstraint(pathConstraint);
						break;
					}
				}
				goto IL_207;
			}
			for (int num3 = 0; num3 < num; num3++)
			{
				this.SortBone(exposedList2.Items[num3]);
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00062740 File Offset: 0x00060940
		private void SortIkConstraint(IkConstraint constraint)
		{
			constraint.active = (constraint.target.active && (!constraint.data.skinRequired || (this.skin != null && this.skin.constraints.Contains(constraint.data))));
			if (!constraint.active)
			{
				return;
			}
			Bone target = constraint.target;
			this.SortBone(target);
			ExposedList<Bone> exposedList = constraint.bones;
			Bone bone = exposedList.Items[0];
			this.SortBone(bone);
			if (exposedList.Count > 1)
			{
				Bone item = exposedList.Items[exposedList.Count - 1];
				if (!this.updateCache.Contains(item))
				{
					this.updateCacheReset.Add(item);
				}
			}
			this.updateCache.Add(constraint);
			Skeleton.SortReset(bone.children);
			exposedList.Items[exposedList.Count - 1].sorted = true;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00062834 File Offset: 0x00060A34
		private void SortPathConstraint(PathConstraint constraint)
		{
			constraint.active = (constraint.target.bone.active && (!constraint.data.skinRequired || (this.skin != null && this.skin.constraints.Contains(constraint.data))));
			if (!constraint.active)
			{
				return;
			}
			Slot target = constraint.target;
			int index = target.data.index;
			Bone bone = target.bone;
			if (this.skin != null)
			{
				this.SortPathConstraintAttachment(this.skin, index, bone);
			}
			if (this.data.defaultSkin != null && this.data.defaultSkin != this.skin)
			{
				this.SortPathConstraintAttachment(this.data.defaultSkin, index, bone);
			}
			Attachment attachment = target.attachment;
			if (attachment is PathAttachment)
			{
				this.SortPathConstraintAttachment(attachment, bone);
			}
			ExposedList<Bone> exposedList = constraint.bones;
			int count = exposedList.Count;
			for (int i = 0; i < count; i++)
			{
				this.SortBone(exposedList.Items[i]);
			}
			this.updateCache.Add(constraint);
			for (int j = 0; j < count; j++)
			{
				Skeleton.SortReset(exposedList.Items[j].children);
			}
			for (int k = 0; k < count; k++)
			{
				exposedList.Items[k].sorted = true;
			}
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000629C0 File Offset: 0x00060BC0
		private void SortTransformConstraint(TransformConstraint constraint)
		{
			constraint.active = (constraint.target.active && (!constraint.data.skinRequired || (this.skin != null && this.skin.constraints.Contains(constraint.data))));
			if (!constraint.active)
			{
				return;
			}
			this.SortBone(constraint.target);
			ExposedList<Bone> exposedList = constraint.bones;
			int count = exposedList.Count;
			if (constraint.data.local)
			{
				for (int i = 0; i < count; i++)
				{
					Bone bone = exposedList.Items[i];
					this.SortBone(bone.parent);
					if (!this.updateCache.Contains(bone))
					{
						this.updateCacheReset.Add(bone);
					}
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					this.SortBone(exposedList.Items[j]);
				}
			}
			this.updateCache.Add(constraint);
			for (int k = 0; k < count; k++)
			{
				Skeleton.SortReset(exposedList.Items[k].children);
			}
			for (int l = 0; l < count; l++)
			{
				exposedList.Items[l].sorted = true;
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00062B1C File Offset: 0x00060D1C
		private void SortPathConstraintAttachment(Skin skin, int slotIndex, Bone slotBone)
		{
			foreach (Skin.SkinEntry skinEntry in skin.Attachments.Keys)
			{
				Skin.SkinEntry skinEntry2 = skinEntry;
				if (skinEntry2.SlotIndex == slotIndex)
				{
					this.SortPathConstraintAttachment(skinEntry2.Attachment, slotBone);
				}
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00062B9C File Offset: 0x00060D9C
		private void SortPathConstraintAttachment(Attachment attachment, Bone slotBone)
		{
			if (!(attachment is PathAttachment))
			{
				return;
			}
			int[] array = ((PathAttachment)attachment).bones;
			if (array == null)
			{
				this.SortBone(slotBone);
			}
			else
			{
				ExposedList<Bone> exposedList = this.bones;
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					int num2 = array[i++];
					num2 += i;
					while (i < num2)
					{
						this.SortBone(exposedList.Items[array[i++]]);
					}
				}
			}
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00062C1C File Offset: 0x00060E1C
		private void SortBone(Bone bone)
		{
			if (bone.sorted)
			{
				return;
			}
			Bone parent = bone.parent;
			if (parent != null)
			{
				this.SortBone(parent);
			}
			bone.sorted = true;
			this.updateCache.Add(bone);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00062C5C File Offset: 0x00060E5C
		private static void SortReset(ExposedList<Bone> bones)
		{
			Bone[] items = bones.Items;
			int i = 0;
			int count = bones.Count;
			while (i < count)
			{
				Bone bone = items[i];
				if (bone.active)
				{
					if (bone.sorted)
					{
						Skeleton.SortReset(bone.children);
					}
					bone.sorted = false;
				}
				i++;
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00062CBC File Offset: 0x00060EBC
		public void UpdateWorldTransform()
		{
			ExposedList<Bone> exposedList = this.updateCacheReset;
			Bone[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone = items[i];
				bone.ax = bone.x;
				bone.ay = bone.y;
				bone.arotation = bone.rotation;
				bone.ascaleX = bone.scaleX;
				bone.ascaleY = bone.scaleY;
				bone.ashearX = bone.shearX;
				bone.ashearY = bone.shearY;
				bone.appliedValid = true;
				i++;
			}
			IUpdatable[] items2 = this.updateCache.Items;
			int j = 0;
			int count2 = this.updateCache.Count;
			while (j < count2)
			{
				items2[j].Update();
				j++;
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00062D9C File Offset: 0x00060F9C
		public void UpdateWorldTransform(Bone parent)
		{
			ExposedList<Bone> exposedList = this.updateCacheReset;
			Bone[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone = items[i];
				bone.ax = bone.x;
				bone.ay = bone.y;
				bone.arotation = bone.rotation;
				bone.ascaleX = bone.scaleX;
				bone.ascaleY = bone.scaleY;
				bone.ashearX = bone.shearX;
				bone.ashearY = bone.shearY;
				bone.appliedValid = true;
				i++;
			}
			Bone rootBone = this.RootBone;
			float num = parent.a;
			float num2 = parent.b;
			float c = parent.c;
			float d = parent.d;
			rootBone.worldX = num * this.x + num2 * this.y + parent.worldX;
			rootBone.worldY = c * this.x + d * this.y + parent.worldY;
			float degrees = rootBone.rotation + 90f + rootBone.shearY;
			float num3 = MathUtils.CosDeg(rootBone.rotation + rootBone.shearX) * rootBone.scaleX;
			float num4 = MathUtils.CosDeg(degrees) * rootBone.scaleY;
			float num5 = MathUtils.SinDeg(rootBone.rotation + rootBone.shearX) * rootBone.scaleX;
			float num6 = MathUtils.SinDeg(degrees) * rootBone.scaleY;
			rootBone.a = (num * num3 + num2 * num5) * this.scaleX;
			rootBone.b = (num * num4 + num2 * num6) * this.scaleX;
			rootBone.c = (c * num3 + d * num5) * this.scaleY;
			rootBone.d = (c * num4 + d * num6) * this.scaleY;
			ExposedList<IUpdatable> exposedList2 = this.updateCache;
			IUpdatable[] items2 = exposedList2.Items;
			int j = 0;
			int count2 = exposedList2.Count;
			while (j < count2)
			{
				IUpdatable updatable = items2[j];
				if (updatable != rootBone)
				{
					updatable.Update();
				}
				j++;
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00062FCC File Offset: 0x000611CC
		public void SetToSetupPose()
		{
			this.SetBonesToSetupPose();
			this.SetSlotsToSetupPose();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00062FDC File Offset: 0x000611DC
		public void SetBonesToSetupPose()
		{
			Bone[] items = this.bones.Items;
			int i = 0;
			int count = this.bones.Count;
			while (i < count)
			{
				items[i].SetToSetupPose();
				i++;
			}
			IkConstraint[] items2 = this.ikConstraints.Items;
			int j = 0;
			int count2 = this.ikConstraints.Count;
			while (j < count2)
			{
				IkConstraint ikConstraint = items2[j];
				ikConstraint.mix = ikConstraint.data.mix;
				ikConstraint.softness = ikConstraint.data.softness;
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.compress = ikConstraint.data.compress;
				ikConstraint.stretch = ikConstraint.data.stretch;
				j++;
			}
			TransformConstraint[] items3 = this.transformConstraints.Items;
			int k = 0;
			int count3 = this.transformConstraints.Count;
			while (k < count3)
			{
				TransformConstraint transformConstraint = items3[k];
				TransformConstraintData transformConstraintData = transformConstraint.data;
				transformConstraint.rotateMix = transformConstraintData.rotateMix;
				transformConstraint.translateMix = transformConstraintData.translateMix;
				transformConstraint.scaleMix = transformConstraintData.scaleMix;
				transformConstraint.shearMix = transformConstraintData.shearMix;
				k++;
			}
			PathConstraint[] items4 = this.pathConstraints.Items;
			int l = 0;
			int count4 = this.pathConstraints.Count;
			while (l < count4)
			{
				PathConstraint pathConstraint = items4[l];
				PathConstraintData pathConstraintData = pathConstraint.data;
				pathConstraint.position = pathConstraintData.position;
				pathConstraint.spacing = pathConstraintData.spacing;
				pathConstraint.rotateMix = pathConstraintData.rotateMix;
				pathConstraint.translateMix = pathConstraintData.translateMix;
				l++;
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000631A4 File Offset: 0x000613A4
		public void SetSlotsToSetupPose()
		{
			ExposedList<Slot> exposedList = this.slots;
			Slot[] items = exposedList.Items;
			this.drawOrder.Clear(true);
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				this.drawOrder.Add(items[i]);
				i++;
			}
			int j = 0;
			int count2 = exposedList.Count;
			while (j < count2)
			{
				items[j].SetToSetupPose();
				j++;
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0006321C File Offset: 0x0006141C
		public Bone FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName", "boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = this.bones;
			Bone[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Bone bone = items[i];
				if (bone.data.name == boneName)
				{
					return bone;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00063288 File Offset: 0x00061488
		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName", "boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = this.bones;
			Bone[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (items[i].data.name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000632F0 File Offset: 0x000614F0
		public Slot FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName", "slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			Slot[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Slot slot = items[i];
				if (slot.data.name == slotName)
				{
					return slot;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0006335C File Offset: 0x0006155C
		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName", "slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			Slot[] items = exposedList.Items;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				if (items[i].data.name.Equals(slotName))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000633C4 File Offset: 0x000615C4
		public void SetSkin(string skinName)
		{
			Skin skin = this.data.FindSkin(skinName);
			if (skin == null)
			{
				throw new ArgumentException("Skin not found: " + skinName, "skinName");
			}
			this.SetSkin(skin);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00063404 File Offset: 0x00061604
		public void SetSkin(Skin newSkin)
		{
			if (newSkin == this.skin)
			{
				return;
			}
			if (newSkin != null)
			{
				if (this.skin != null)
				{
					newSkin.AttachAll(this, this.skin);
				}
				else
				{
					ExposedList<Slot> exposedList = this.slots;
					int i = 0;
					int count = exposedList.Count;
					while (i < count)
					{
						Slot slot = exposedList.Items[i];
						string attachmentName = slot.data.attachmentName;
						if (attachmentName != null)
						{
							Attachment attachment = newSkin.GetAttachment(i, attachmentName);
							if (attachment != null)
							{
								slot.Attachment = attachment;
							}
						}
						i++;
					}
				}
			}
			this.skin = newSkin;
			this.UpdateCache();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000634A8 File Offset: 0x000616A8
		public Attachment GetAttachment(string slotName, string attachmentName)
		{
			return this.GetAttachment(this.data.FindSlotIndex(slotName), attachmentName);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000634C0 File Offset: 0x000616C0
		public Attachment GetAttachment(int slotIndex, string attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentNullException("attachmentName", "attachmentName cannot be null.");
			}
			if (this.skin != null)
			{
				Attachment attachment = this.skin.GetAttachment(slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			return (this.data.defaultSkin == null) ? null : this.data.defaultSkin.GetAttachment(slotIndex, attachmentName);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0006352C File Offset: 0x0006172C
		public void SetAttachment(string slotName, string attachmentName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName", "slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = this.slots;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				Slot slot = exposedList.Items[i];
				if (slot.data.name == slotName)
				{
					Attachment attachment = null;
					if (attachmentName != null)
					{
						attachment = this.GetAttachment(i, attachmentName);
						if (attachment == null)
						{
							throw new Exception("Attachment not found: " + attachmentName + ", for slot: " + slotName);
						}
					}
					slot.Attachment = attachment;
					return;
				}
				i++;
			}
			throw new Exception("Slot not found: " + slotName);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000635D8 File Offset: 0x000617D8
		public IkConstraint FindIkConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<IkConstraint> exposedList = this.ikConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				IkConstraint ikConstraint = exposedList.Items[i];
				if (ikConstraint.data.name == constraintName)
				{
					return ikConstraint;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00063640 File Offset: 0x00061840
		public TransformConstraint FindTransformConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<TransformConstraint> exposedList = this.transformConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				TransformConstraint transformConstraint = exposedList.Items[i];
				if (transformConstraint.data.Name == constraintName)
				{
					return transformConstraint;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000636A8 File Offset: 0x000618A8
		public PathConstraint FindPathConstraint(string constraintName)
		{
			if (constraintName == null)
			{
				throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
			}
			ExposedList<PathConstraint> exposedList = this.pathConstraints;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				PathConstraint pathConstraint = exposedList.Items[i];
				if (pathConstraint.data.Name.Equals(constraintName))
				{
					return pathConstraint;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00063710 File Offset: 0x00061910
		public void Update(float delta)
		{
			this.time += delta;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00063720 File Offset: 0x00061920
		public void GetBounds(out float x, out float y, out float width, out float height, ref float[] vertexBuffer)
		{
			float[] array = vertexBuffer;
			array = (array ?? new float[8]);
			Slot[] items = this.drawOrder.Items;
			float num = 2.1474836E+09f;
			float num2 = 2.1474836E+09f;
			float num3 = -2.1474836E+09f;
			float num4 = -2.1474836E+09f;
			int i = 0;
			int num5 = items.Length;
			while (i < num5)
			{
				Slot slot = items[i];
				if (slot.bone.active)
				{
					int num6 = 0;
					float[] array2 = null;
					Attachment attachment = slot.attachment;
					RegionAttachment regionAttachment = attachment as RegionAttachment;
					if (regionAttachment != null)
					{
						num6 = 8;
						array2 = array;
						if (array2.Length < 8)
						{
							array = (array2 = new float[8]);
						}
						regionAttachment.ComputeWorldVertices(slot.bone, array, 0, 2);
					}
					else
					{
						MeshAttachment meshAttachment = attachment as MeshAttachment;
						if (meshAttachment != null)
						{
							MeshAttachment meshAttachment2 = meshAttachment;
							num6 = meshAttachment2.WorldVerticesLength;
							array2 = array;
							if (array2.Length < num6)
							{
								array = (array2 = new float[num6]);
							}
							meshAttachment2.ComputeWorldVertices(slot, 0, num6, array, 0, 2);
						}
					}
					if (array2 != null)
					{
						for (int j = 0; j < num6; j += 2)
						{
							float val = array2[j];
							float val2 = array2[j + 1];
							num = Math.Min(num, val);
							num2 = Math.Min(num2, val2);
							num3 = Math.Max(num3, val);
							num4 = Math.Max(num4, val2);
						}
					}
				}
				i++;
			}
			x = num;
			y = num2;
			width = num3 - num;
			height = num4 - num2;
			vertexBuffer = array;
		}

		// Token: 0x04000BFC RID: 3068
		internal SkeletonData data;

		// Token: 0x04000BFD RID: 3069
		internal ExposedList<Bone> bones;

		// Token: 0x04000BFE RID: 3070
		internal ExposedList<Slot> slots;

		// Token: 0x04000BFF RID: 3071
		internal ExposedList<Slot> drawOrder;

		// Token: 0x04000C00 RID: 3072
		internal ExposedList<IkConstraint> ikConstraints;

		// Token: 0x04000C01 RID: 3073
		internal ExposedList<TransformConstraint> transformConstraints;

		// Token: 0x04000C02 RID: 3074
		internal ExposedList<PathConstraint> pathConstraints;

		// Token: 0x04000C03 RID: 3075
		internal ExposedList<IUpdatable> updateCache = new ExposedList<IUpdatable>();

		// Token: 0x04000C04 RID: 3076
		internal ExposedList<Bone> updateCacheReset = new ExposedList<Bone>();

		// Token: 0x04000C05 RID: 3077
		internal Skin skin;

		// Token: 0x04000C06 RID: 3078
		internal float r = 1f;

		// Token: 0x04000C07 RID: 3079
		internal float g = 1f;

		// Token: 0x04000C08 RID: 3080
		internal float b = 1f;

		// Token: 0x04000C09 RID: 3081
		internal float a = 1f;

		// Token: 0x04000C0A RID: 3082
		internal float time;

		// Token: 0x04000C0B RID: 3083
		private float scaleX = 1f;

		// Token: 0x04000C0C RID: 3084
		private float scaleY = 1f;

		// Token: 0x04000C0D RID: 3085
		internal float x;

		// Token: 0x04000C0E RID: 3086
		internal float y;
	}
}
