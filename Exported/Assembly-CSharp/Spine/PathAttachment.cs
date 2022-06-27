using System;

namespace Spine
{
	// Token: 0x0200019D RID: 413
	public class PathAttachment : VertexAttachment
	{
		// Token: 0x06000C17 RID: 3095 RVA: 0x0005BC04 File Offset: 0x00059E04
		public PathAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0005BC10 File Offset: 0x00059E10
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0005BC18 File Offset: 0x00059E18
		public float[] Lengths
		{
			get
			{
				return this.lengths;
			}
			set
			{
				this.lengths = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0005BC24 File Offset: 0x00059E24
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0005BC2C File Offset: 0x00059E2C
		public bool Closed
		{
			get
			{
				return this.closed;
			}
			set
			{
				this.closed = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0005BC38 File Offset: 0x00059E38
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0005BC40 File Offset: 0x00059E40
		public bool ConstantSpeed
		{
			get
			{
				return this.constantSpeed;
			}
			set
			{
				this.constantSpeed = value;
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0005BC4C File Offset: 0x00059E4C
		public override Attachment Copy()
		{
			PathAttachment pathAttachment = new PathAttachment(base.Name);
			base.CopyTo(pathAttachment);
			pathAttachment.lengths = new float[this.lengths.Length];
			Array.Copy(this.lengths, 0, pathAttachment.lengths, 0, this.lengths.Length);
			pathAttachment.closed = this.closed;
			pathAttachment.constantSpeed = this.constantSpeed;
			return pathAttachment;
		}

		// Token: 0x04000B24 RID: 2852
		internal float[] lengths;

		// Token: 0x04000B25 RID: 2853
		internal bool closed;

		// Token: 0x04000B26 RID: 2854
		internal bool constantSpeed;
	}
}
