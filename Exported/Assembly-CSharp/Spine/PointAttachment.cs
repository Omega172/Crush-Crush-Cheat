using System;

namespace Spine
{
	// Token: 0x0200019E RID: 414
	public class PointAttachment : Attachment
	{
		// Token: 0x06000C1F RID: 3103 RVA: 0x0005BCB4 File Offset: 0x00059EB4
		public PointAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0005BCC0 File Offset: 0x00059EC0
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0005BCC8 File Offset: 0x00059EC8
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0005BCD4 File Offset: 0x00059ED4
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0005BCDC File Offset: 0x00059EDC
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

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0005BCE8 File Offset: 0x00059EE8
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0005BCF0 File Offset: 0x00059EF0
		public float Rotation
		{
			get
			{
				return this.rotation;
			}
			set
			{
				this.rotation = value;
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0005BCFC File Offset: 0x00059EFC
		public void ComputeWorldPosition(Bone bone, out float ox, out float oy)
		{
			bone.LocalToWorld(this.x, this.y, out ox, out oy);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0005BD14 File Offset: 0x00059F14
		public float ComputeWorldRotation(Bone bone)
		{
			float num = MathUtils.CosDeg(this.rotation);
			float num2 = MathUtils.SinDeg(this.rotation);
			float num3 = num * bone.a + num2 * bone.b;
			float num4 = num * bone.c + num2 * bone.d;
			return MathUtils.Atan2(num4, num3) * 57.295776f;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0005BD6C File Offset: 0x00059F6C
		public override Attachment Copy()
		{
			return new PointAttachment(base.Name)
			{
				x = this.x,
				y = this.y,
				rotation = this.rotation
			};
		}

		// Token: 0x04000B27 RID: 2855
		internal float x;

		// Token: 0x04000B28 RID: 2856
		internal float y;

		// Token: 0x04000B29 RID: 2857
		internal float rotation;
	}
}
