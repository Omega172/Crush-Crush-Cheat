using System;

namespace Spine
{
	// Token: 0x020001A3 RID: 419
	public class BoneData
	{
		// Token: 0x06000C9D RID: 3229 RVA: 0x0005D64C File Offset: 0x0005B84C
		public BoneData(int index, string name, BoneData parent)
		{
			if (index < 0)
			{
				throw new ArgumentException("index must be >= 0", "index");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name", "name cannot be null.");
			}
			this.index = index;
			this.name = name;
			this.parent = parent;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0005D6B8 File Offset: 0x0005B8B8
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0005D6C0 File Offset: 0x0005B8C0
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005D6C8 File Offset: 0x0005B8C8
		public BoneData Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0005D6D0 File Offset: 0x0005B8D0
		// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x0005D6D8 File Offset: 0x0005B8D8
		public float Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0005D6E4 File Offset: 0x0005B8E4
		// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x0005D6EC File Offset: 0x0005B8EC
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0005D6F8 File Offset: 0x0005B8F8
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x0005D700 File Offset: 0x0005B900
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0005D70C File Offset: 0x0005B90C
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x0005D714 File Offset: 0x0005B914
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0005D720 File Offset: 0x0005B920
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x0005D728 File Offset: 0x0005B928
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0005D734 File Offset: 0x0005B934
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x0005D73C File Offset: 0x0005B93C
		public float ScaleY
		{
			get
			{
				return this.scaleY;
			}
			set
			{
				this.scaleY = value;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0005D748 File Offset: 0x0005B948
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0005D750 File Offset: 0x0005B950
		public float ShearX
		{
			get
			{
				return this.shearX;
			}
			set
			{
				this.shearX = value;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0005D75C File Offset: 0x0005B95C
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0005D764 File Offset: 0x0005B964
		public float ShearY
		{
			get
			{
				return this.shearY;
			}
			set
			{
				this.shearY = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0005D770 File Offset: 0x0005B970
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x0005D778 File Offset: 0x0005B978
		public TransformMode TransformMode
		{
			get
			{
				return this.transformMode;
			}
			set
			{
				this.transformMode = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0005D784 File Offset: 0x0005B984
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0005D78C File Offset: 0x0005B98C
		public bool SkinRequired
		{
			get
			{
				return this.skinRequired;
			}
			set
			{
				this.skinRequired = value;
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0005D798 File Offset: 0x0005B998
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000B6F RID: 2927
		internal int index;

		// Token: 0x04000B70 RID: 2928
		internal string name;

		// Token: 0x04000B71 RID: 2929
		internal BoneData parent;

		// Token: 0x04000B72 RID: 2930
		internal float length;

		// Token: 0x04000B73 RID: 2931
		internal float x;

		// Token: 0x04000B74 RID: 2932
		internal float y;

		// Token: 0x04000B75 RID: 2933
		internal float rotation;

		// Token: 0x04000B76 RID: 2934
		internal float scaleX = 1f;

		// Token: 0x04000B77 RID: 2935
		internal float scaleY = 1f;

		// Token: 0x04000B78 RID: 2936
		internal float shearX;

		// Token: 0x04000B79 RID: 2937
		internal float shearY;

		// Token: 0x04000B7A RID: 2938
		internal TransformMode transformMode;

		// Token: 0x04000B7B RID: 2939
		internal bool skinRequired;
	}
}
