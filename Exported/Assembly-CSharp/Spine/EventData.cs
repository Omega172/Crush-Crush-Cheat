using System;

namespace Spine
{
	// Token: 0x020001AC RID: 428
	public class EventData
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x0005E0D8 File Offset: 0x0005C2D8
		public EventData(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			this.name = name;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0005E100 File Offset: 0x0005C300
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0005E108 File Offset: 0x0005C308
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x0005E110 File Offset: 0x0005C310
		public int Int { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005E11C File Offset: 0x0005C31C
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x0005E124 File Offset: 0x0005C324
		public float Float { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0005E130 File Offset: 0x0005C330
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x0005E138 File Offset: 0x0005C338
		public string String { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0005E144 File Offset: 0x0005C344
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x0005E14C File Offset: 0x0005C34C
		public string AudioPath { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0005E158 File Offset: 0x0005C358
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0005E160 File Offset: 0x0005C360
		public float Volume { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0005E16C File Offset: 0x0005C36C
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x0005E174 File Offset: 0x0005C374
		public float Balance { get; set; }

		// Token: 0x06000D13 RID: 3347 RVA: 0x0005E180 File Offset: 0x0005C380
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000B96 RID: 2966
		internal string name;
	}
}
