using System;

namespace Spine
{
	// Token: 0x02000196 RID: 406
	public abstract class Attachment
	{
		// Token: 0x06000BCC RID: 3020 RVA: 0x0005B230 File Offset: 0x00059430
		protected Attachment(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null");
			}
			this.Name = name;
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0005B258 File Offset: 0x00059458
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0005B260 File Offset: 0x00059460
		public string Name { get; private set; }

		// Token: 0x06000BCF RID: 3023 RVA: 0x0005B26C File Offset: 0x0005946C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000BD0 RID: 3024
		public abstract Attachment Copy();
	}
}
