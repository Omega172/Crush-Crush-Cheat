using System;

namespace Spine
{
	// Token: 0x020001AB RID: 427
	public class Event
	{
		// Token: 0x06000CF7 RID: 3319 RVA: 0x0005E01C File Offset: 0x0005C21C
		public Event(float time, EventData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			this.time = time;
			this.data = data;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0005E054 File Offset: 0x0005C254
		public EventData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0005E05C File Offset: 0x0005C25C
		public float Time
		{
			get
			{
				return this.time;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0005E064 File Offset: 0x0005C264
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x0005E06C File Offset: 0x0005C26C
		public int Int
		{
			get
			{
				return this.intValue;
			}
			set
			{
				this.intValue = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0005E078 File Offset: 0x0005C278
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0005E080 File Offset: 0x0005C280
		public float Float
		{
			get
			{
				return this.floatValue;
			}
			set
			{
				this.floatValue = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0005E08C File Offset: 0x0005C28C
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0005E094 File Offset: 0x0005C294
		public string String
		{
			get
			{
				return this.stringValue;
			}
			set
			{
				this.stringValue = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0005E0A0 File Offset: 0x0005C2A0
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x0005E0A8 File Offset: 0x0005C2A8
		public float Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				this.volume = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0005E0B4 File Offset: 0x0005C2B4
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x0005E0BC File Offset: 0x0005C2BC
		public float Balance
		{
			get
			{
				return this.balance;
			}
			set
			{
				this.balance = value;
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0005E0C8 File Offset: 0x0005C2C8
		public override string ToString()
		{
			return this.data.Name;
		}

		// Token: 0x04000B8F RID: 2959
		internal readonly EventData data;

		// Token: 0x04000B90 RID: 2960
		internal readonly float time;

		// Token: 0x04000B91 RID: 2961
		internal int intValue;

		// Token: 0x04000B92 RID: 2962
		internal float floatValue;

		// Token: 0x04000B93 RID: 2963
		internal string stringValue;

		// Token: 0x04000B94 RID: 2964
		internal float volume;

		// Token: 0x04000B95 RID: 2965
		internal float balance;
	}
}
