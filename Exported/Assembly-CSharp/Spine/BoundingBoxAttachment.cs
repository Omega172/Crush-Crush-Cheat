using System;

namespace Spine
{
	// Token: 0x0200019A RID: 410
	public class BoundingBoxAttachment : VertexAttachment
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x0005B274 File Offset: 0x00059474
		public BoundingBoxAttachment(string name) : base(name)
		{
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0005B280 File Offset: 0x00059480
		public override Attachment Copy()
		{
			BoundingBoxAttachment boundingBoxAttachment = new BoundingBoxAttachment(base.Name);
			base.CopyTo(boundingBoxAttachment);
			return boundingBoxAttachment;
		}
	}
}
