using System;

namespace Spine
{
	// Token: 0x0200019F RID: 415
	public class RegionAttachment : Attachment, IHasRendererObject
	{
		// Token: 0x06000C29 RID: 3113 RVA: 0x0005BDAC File Offset: 0x00059FAC
		public RegionAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0005BE1C File Offset: 0x0005A01C
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0005BE24 File Offset: 0x0005A024
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0005BE30 File Offset: 0x0005A030
		// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0005BE38 File Offset: 0x0005A038
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0005BE44 File Offset: 0x0005A044
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0005BE4C File Offset: 0x0005A04C
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

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0005BE58 File Offset: 0x0005A058
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0005BE60 File Offset: 0x0005A060
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0005BE6C File Offset: 0x0005A06C
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0005BE74 File Offset: 0x0005A074
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0005BE80 File Offset: 0x0005A080
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x0005BE88 File Offset: 0x0005A088
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0005BE94 File Offset: 0x0005A094
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x0005BE9C File Offset: 0x0005A09C
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

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0005BEA8 File Offset: 0x0005A0A8
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x0005BEB0 File Offset: 0x0005A0B0
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0005BEBC File Offset: 0x0005A0BC
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0005BEC4 File Offset: 0x0005A0C4
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0005BED0 File Offset: 0x0005A0D0
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0005BED8 File Offset: 0x0005A0D8
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

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0005BEE4 File Offset: 0x0005A0E4
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0005BEEC File Offset: 0x0005A0EC
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0005BEF8 File Offset: 0x0005A0F8
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0005BF00 File Offset: 0x0005A100
		public string Path { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0005BF0C File Offset: 0x0005A10C
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0005BF14 File Offset: 0x0005A114
		public object RendererObject { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0005BF20 File Offset: 0x0005A120
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0005BF28 File Offset: 0x0005A128
		public float RegionOffsetX
		{
			get
			{
				return this.regionOffsetX;
			}
			set
			{
				this.regionOffsetX = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0005BF34 File Offset: 0x0005A134
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0005BF3C File Offset: 0x0005A13C
		public float RegionOffsetY
		{
			get
			{
				return this.regionOffsetY;
			}
			set
			{
				this.regionOffsetY = value;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0005BF48 File Offset: 0x0005A148
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0005BF50 File Offset: 0x0005A150
		public float RegionWidth
		{
			get
			{
				return this.regionWidth;
			}
			set
			{
				this.regionWidth = value;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0005BF5C File Offset: 0x0005A15C
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0005BF64 File Offset: 0x0005A164
		public float RegionHeight
		{
			get
			{
				return this.regionHeight;
			}
			set
			{
				this.regionHeight = value;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0005BF70 File Offset: 0x0005A170
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0005BF78 File Offset: 0x0005A178
		public float RegionOriginalWidth
		{
			get
			{
				return this.regionOriginalWidth;
			}
			set
			{
				this.regionOriginalWidth = value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0005BF84 File Offset: 0x0005A184
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0005BF8C File Offset: 0x0005A18C
		public float RegionOriginalHeight
		{
			get
			{
				return this.regionOriginalHeight;
			}
			set
			{
				this.regionOriginalHeight = value;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0005BF98 File Offset: 0x0005A198
		public float[] Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x0005BFA0 File Offset: 0x0005A1A0
		public float[] UVs
		{
			get
			{
				return this.uvs;
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0005BFA8 File Offset: 0x0005A1A8
		public void UpdateOffset()
		{
			float num = this.width;
			float num2 = this.height;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			float num5 = -num3;
			float num6 = -num4;
			if (this.regionOriginalWidth != 0f)
			{
				num5 += this.regionOffsetX / this.regionOriginalWidth * num;
				num6 += this.regionOffsetY / this.regionOriginalHeight * num2;
				num3 -= (this.regionOriginalWidth - this.regionOffsetX - this.regionWidth) / this.regionOriginalWidth * num;
				num4 -= (this.regionOriginalHeight - this.regionOffsetY - this.regionHeight) / this.regionOriginalHeight * num2;
			}
			float num7 = this.scaleX;
			float num8 = this.scaleY;
			num5 *= num7;
			num6 *= num8;
			num3 *= num7;
			num4 *= num8;
			float degrees = this.rotation;
			float num9 = MathUtils.CosDeg(degrees);
			float num10 = MathUtils.SinDeg(degrees);
			float num11 = this.x;
			float num12 = this.y;
			float num13 = num5 * num9 + num11;
			float num14 = num5 * num10;
			float num15 = num6 * num9 + num12;
			float num16 = num6 * num10;
			float num17 = num3 * num9 + num11;
			float num18 = num3 * num10;
			float num19 = num4 * num9 + num12;
			float num20 = num4 * num10;
			float[] array = this.offset;
			array[0] = num13 - num16;
			array[1] = num15 + num14;
			array[2] = num13 - num20;
			array[3] = num19 + num14;
			array[4] = num17 - num20;
			array[5] = num19 + num18;
			array[6] = num17 - num16;
			array[7] = num15 + num18;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0005C138 File Offset: 0x0005A338
		public void SetUVs(float u, float v, float u2, float v2, bool rotate)
		{
			float[] array = this.uvs;
			if (rotate)
			{
				array[4] = u;
				array[5] = v2;
				array[6] = u;
				array[7] = v;
				array[0] = u2;
				array[1] = v;
				array[2] = u2;
				array[3] = v2;
			}
			else
			{
				array[2] = u;
				array[3] = v2;
				array[4] = u;
				array[5] = v;
				array[6] = u2;
				array[7] = v;
				array[0] = u2;
				array[1] = v2;
			}
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0005C19C File Offset: 0x0005A39C
		public void ComputeWorldVertices(Bone bone, float[] worldVertices, int offset, int stride = 2)
		{
			float[] array = this.offset;
			float worldX = bone.worldX;
			float worldY = bone.worldY;
			float num = bone.a;
			float num2 = bone.b;
			float c = bone.c;
			float d = bone.d;
			float num3 = array[6];
			float num4 = array[7];
			worldVertices[offset] = num3 * num + num4 * num2 + worldX;
			worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
			offset += stride;
			num3 = array[0];
			num4 = array[1];
			worldVertices[offset] = num3 * num + num4 * num2 + worldX;
			worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
			offset += stride;
			num3 = array[2];
			num4 = array[3];
			worldVertices[offset] = num3 * num + num4 * num2 + worldX;
			worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
			offset += stride;
			num3 = array[4];
			num4 = array[5];
			worldVertices[offset] = num3 * num + num4 * num2 + worldX;
			worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0005C29C File Offset: 0x0005A49C
		public override Attachment Copy()
		{
			RegionAttachment regionAttachment = new RegionAttachment(base.Name);
			regionAttachment.RendererObject = this.RendererObject;
			regionAttachment.regionOffsetX = this.regionOffsetX;
			regionAttachment.regionOffsetY = this.regionOffsetY;
			regionAttachment.regionWidth = this.regionWidth;
			regionAttachment.regionHeight = this.regionHeight;
			regionAttachment.regionOriginalWidth = this.regionOriginalWidth;
			regionAttachment.regionOriginalHeight = this.regionOriginalHeight;
			regionAttachment.Path = this.Path;
			regionAttachment.x = this.x;
			regionAttachment.y = this.y;
			regionAttachment.scaleX = this.scaleX;
			regionAttachment.scaleY = this.scaleY;
			regionAttachment.rotation = this.rotation;
			regionAttachment.width = this.width;
			regionAttachment.height = this.height;
			Array.Copy(this.uvs, 0, regionAttachment.uvs, 0, 8);
			Array.Copy(this.offset, 0, regionAttachment.offset, 0, 8);
			regionAttachment.r = this.r;
			regionAttachment.g = this.g;
			regionAttachment.b = this.b;
			regionAttachment.a = this.a;
			return regionAttachment;
		}

		// Token: 0x04000B2A RID: 2858
		public const int BLX = 0;

		// Token: 0x04000B2B RID: 2859
		public const int BLY = 1;

		// Token: 0x04000B2C RID: 2860
		public const int ULX = 2;

		// Token: 0x04000B2D RID: 2861
		public const int ULY = 3;

		// Token: 0x04000B2E RID: 2862
		public const int URX = 4;

		// Token: 0x04000B2F RID: 2863
		public const int URY = 5;

		// Token: 0x04000B30 RID: 2864
		public const int BRX = 6;

		// Token: 0x04000B31 RID: 2865
		public const int BRY = 7;

		// Token: 0x04000B32 RID: 2866
		internal float x;

		// Token: 0x04000B33 RID: 2867
		internal float y;

		// Token: 0x04000B34 RID: 2868
		internal float rotation;

		// Token: 0x04000B35 RID: 2869
		internal float scaleX = 1f;

		// Token: 0x04000B36 RID: 2870
		internal float scaleY = 1f;

		// Token: 0x04000B37 RID: 2871
		internal float width;

		// Token: 0x04000B38 RID: 2872
		internal float height;

		// Token: 0x04000B39 RID: 2873
		internal float regionOffsetX;

		// Token: 0x04000B3A RID: 2874
		internal float regionOffsetY;

		// Token: 0x04000B3B RID: 2875
		internal float regionWidth;

		// Token: 0x04000B3C RID: 2876
		internal float regionHeight;

		// Token: 0x04000B3D RID: 2877
		internal float regionOriginalWidth;

		// Token: 0x04000B3E RID: 2878
		internal float regionOriginalHeight;

		// Token: 0x04000B3F RID: 2879
		internal float[] offset = new float[8];

		// Token: 0x04000B40 RID: 2880
		internal float[] uvs = new float[8];

		// Token: 0x04000B41 RID: 2881
		internal float r = 1f;

		// Token: 0x04000B42 RID: 2882
		internal float g = 1f;

		// Token: 0x04000B43 RID: 2883
		internal float b = 1f;

		// Token: 0x04000B44 RID: 2884
		internal float a = 1f;
	}
}
