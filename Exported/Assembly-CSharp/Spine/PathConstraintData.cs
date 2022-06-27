using System;

namespace Spine
{
	// Token: 0x020001BB RID: 443
	public class PathConstraintData : ConstraintData
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x00061F0C File Offset: 0x0006010C
		public PathConstraintData(string name) : base(name)
		{
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00061F20 File Offset: 0x00060120
		public ExposedList<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x00061F28 File Offset: 0x00060128
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x00061F30 File Offset: 0x00060130
		public SlotData Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00061F3C File Offset: 0x0006013C
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x00061F44 File Offset: 0x00060144
		public PositionMode PositionMode
		{
			get
			{
				return this.positionMode;
			}
			set
			{
				this.positionMode = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00061F50 File Offset: 0x00060150
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x00061F58 File Offset: 0x00060158
		public SpacingMode SpacingMode
		{
			get
			{
				return this.spacingMode;
			}
			set
			{
				this.spacingMode = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00061F64 File Offset: 0x00060164
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x00061F6C File Offset: 0x0006016C
		public RotateMode RotateMode
		{
			get
			{
				return this.rotateMode;
			}
			set
			{
				this.rotateMode = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00061F78 File Offset: 0x00060178
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00061F80 File Offset: 0x00060180
		public float OffsetRotation
		{
			get
			{
				return this.offsetRotation;
			}
			set
			{
				this.offsetRotation = value;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00061F8C File Offset: 0x0006018C
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x00061F94 File Offset: 0x00060194
		public float Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00061FA0 File Offset: 0x000601A0
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00061FA8 File Offset: 0x000601A8
		public float Spacing
		{
			get
			{
				return this.spacing;
			}
			set
			{
				this.spacing = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00061FB4 File Offset: 0x000601B4
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00061FBC File Offset: 0x000601BC
		public float RotateMix
		{
			get
			{
				return this.rotateMix;
			}
			set
			{
				this.rotateMix = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00061FC8 File Offset: 0x000601C8
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00061FD0 File Offset: 0x000601D0
		public float TranslateMix
		{
			get
			{
				return this.translateMix;
			}
			set
			{
				this.translateMix = value;
			}
		}

		// Token: 0x04000BE7 RID: 3047
		internal ExposedList<BoneData> bones = new ExposedList<BoneData>();

		// Token: 0x04000BE8 RID: 3048
		internal SlotData target;

		// Token: 0x04000BE9 RID: 3049
		internal PositionMode positionMode;

		// Token: 0x04000BEA RID: 3050
		internal SpacingMode spacingMode;

		// Token: 0x04000BEB RID: 3051
		internal RotateMode rotateMode;

		// Token: 0x04000BEC RID: 3052
		internal float offsetRotation;

		// Token: 0x04000BED RID: 3053
		internal float position;

		// Token: 0x04000BEE RID: 3054
		internal float spacing;

		// Token: 0x04000BEF RID: 3055
		internal float rotateMix;

		// Token: 0x04000BF0 RID: 3056
		internal float translateMix;
	}
}
