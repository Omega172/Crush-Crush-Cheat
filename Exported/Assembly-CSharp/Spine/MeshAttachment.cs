using System;

namespace Spine
{
	// Token: 0x0200019C RID: 412
	public class MeshAttachment : VertexAttachment, IHasRendererObject
	{
		// Token: 0x06000BDF RID: 3039 RVA: 0x0005B2F4 File Offset: 0x000594F4
		public MeshAttachment(string name) : base(name)
		{
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0005B32C File Offset: 0x0005952C
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x0005B334 File Offset: 0x00059534
		public int HullLength
		{
			get
			{
				return this.hulllength;
			}
			set
			{
				this.hulllength = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0005B340 File Offset: 0x00059540
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0005B348 File Offset: 0x00059548
		public float[] RegionUVs
		{
			get
			{
				return this.regionUVs;
			}
			set
			{
				this.regionUVs = value;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0005B354 File Offset: 0x00059554
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0005B35C File Offset: 0x0005955C
		public float[] UVs
		{
			get
			{
				return this.uvs;
			}
			set
			{
				this.uvs = value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0005B368 File Offset: 0x00059568
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0005B370 File Offset: 0x00059570
		public int[] Triangles
		{
			get
			{
				return this.triangles;
			}
			set
			{
				this.triangles = value;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0005B37C File Offset: 0x0005957C
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0005B384 File Offset: 0x00059584
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

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0005B390 File Offset: 0x00059590
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0005B398 File Offset: 0x00059598
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

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0005B3A4 File Offset: 0x000595A4
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0005B3AC File Offset: 0x000595AC
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0005B3B8 File Offset: 0x000595B8
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0005B3C0 File Offset: 0x000595C0
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0005B3CC File Offset: 0x000595CC
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0005B3D4 File Offset: 0x000595D4
		public string Path { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0005B3E0 File Offset: 0x000595E0
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x0005B3E8 File Offset: 0x000595E8
		public object RendererObject { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0005B3F4 File Offset: 0x000595F4
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0005B3FC File Offset: 0x000595FC
		public float RegionU { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0005B408 File Offset: 0x00059608
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0005B410 File Offset: 0x00059610
		public float RegionV { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0005B41C File Offset: 0x0005961C
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0005B424 File Offset: 0x00059624
		public float RegionU2 { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0005B430 File Offset: 0x00059630
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0005B438 File Offset: 0x00059638
		public float RegionV2 { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0005B444 File Offset: 0x00059644
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0005B44C File Offset: 0x0005964C
		public bool RegionRotate { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0005B458 File Offset: 0x00059658
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0005B460 File Offset: 0x00059660
		public int RegionDegrees { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0005B46C File Offset: 0x0005966C
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0005B474 File Offset: 0x00059674
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x0005B480 File Offset: 0x00059680
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x0005B488 File Offset: 0x00059688
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0005B494 File Offset: 0x00059694
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0005B49C File Offset: 0x0005969C
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

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0005B4A8 File Offset: 0x000596A8
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0005B4B0 File Offset: 0x000596B0
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0005B4BC File Offset: 0x000596BC
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0005B4C4 File Offset: 0x000596C4
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0005B4D0 File Offset: 0x000596D0
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0005B4D8 File Offset: 0x000596D8
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

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0005B4E4 File Offset: 0x000596E4
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0005B4EC File Offset: 0x000596EC
		public MeshAttachment ParentMesh
		{
			get
			{
				return this.parentMesh;
			}
			set
			{
				this.parentMesh = value;
				if (value != null)
				{
					this.bones = value.bones;
					this.vertices = value.vertices;
					this.worldVerticesLength = value.worldVerticesLength;
					this.regionUVs = value.regionUVs;
					this.triangles = value.triangles;
					this.HullLength = value.HullLength;
					this.Edges = value.Edges;
					this.Width = value.Width;
					this.Height = value.Height;
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0005B574 File Offset: 0x00059774
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0005B57C File Offset: 0x0005977C
		public int[] Edges { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0005B588 File Offset: 0x00059788
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0005B590 File Offset: 0x00059790
		public float Width { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0005B59C File Offset: 0x0005979C
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0005B5A4 File Offset: 0x000597A4
		public float Height { get; set; }

		// Token: 0x06000C14 RID: 3092 RVA: 0x0005B5B0 File Offset: 0x000597B0
		public void UpdateUVs()
		{
			float[] array = this.regionUVs;
			if (this.uvs == null || this.uvs.Length != array.Length)
			{
				this.uvs = new float[array.Length];
			}
			float[] array2 = this.uvs;
			float num = this.RegionU;
			float num2 = this.RegionV;
			if (this.RegionDegrees == 90)
			{
				float num3 = this.regionWidth / (this.RegionV2 - this.RegionV);
				float num4 = this.regionHeight / (this.RegionU2 - this.RegionU);
				num -= (this.RegionOriginalHeight - this.RegionOffsetY - this.RegionHeight) / num4;
				num2 -= (this.RegionOriginalWidth - this.RegionOffsetX - this.RegionWidth) / num3;
				float num5 = this.RegionOriginalHeight / num4;
				float num6 = this.RegionOriginalWidth / num3;
				int i = 0;
				int num7 = array2.Length;
				while (i < num7)
				{
					array2[i] = num + array[i + 1] * num5;
					array2[i + 1] = num2 + (1f - array[i]) * num6;
					i += 2;
				}
			}
			else if (this.RegionDegrees == 180)
			{
				float num8 = this.regionWidth / (this.RegionU2 - this.RegionU);
				float num9 = this.regionHeight / (this.RegionV2 - this.RegionV);
				num -= (this.RegionOriginalWidth - this.RegionOffsetX - this.RegionWidth) / num8;
				num2 -= this.RegionOffsetY / num9;
				float num5 = this.RegionOriginalWidth / num8;
				float num6 = this.RegionOriginalHeight / num9;
				int j = 0;
				int num10 = array2.Length;
				while (j < num10)
				{
					array2[j] = num + (1f - array[j]) * num5;
					array2[j + 1] = num2 + (1f - array[j + 1]) * num6;
					j += 2;
				}
			}
			else if (this.RegionDegrees == 270)
			{
				float num11 = this.regionWidth / (this.RegionU2 - this.RegionU);
				float num12 = this.regionHeight / (this.RegionV2 - this.RegionV);
				num -= this.RegionOffsetY / num11;
				num2 -= this.RegionOffsetX / num12;
				float num5 = this.RegionOriginalHeight / num11;
				float num6 = this.RegionOriginalWidth / num12;
				int k = 0;
				int num13 = array2.Length;
				while (k < num13)
				{
					array2[k] = num + (1f - array[k + 1]) * num5;
					array2[k + 1] = num2 + array[k] * num6;
					k += 2;
				}
			}
			else
			{
				float num14 = this.regionWidth / (this.RegionU2 - this.RegionU);
				float num15 = this.regionHeight / (this.RegionV2 - this.RegionV);
				num -= this.RegionOffsetX / num14;
				num2 -= (this.RegionOriginalHeight - this.RegionOffsetY - this.RegionHeight) / num15;
				float num5 = this.RegionOriginalWidth / num14;
				float num6 = this.RegionOriginalHeight / num15;
				int l = 0;
				int num16 = array2.Length;
				while (l < num16)
				{
					array2[l] = num + array[l] * num5;
					array2[l + 1] = num2 + array[l + 1] * num6;
					l += 2;
				}
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0005B8EC File Offset: 0x00059AEC
		public override Attachment Copy()
		{
			if (this.parentMesh != null)
			{
				return this.NewLinkedMesh();
			}
			MeshAttachment meshAttachment = new MeshAttachment(base.Name);
			meshAttachment.RendererObject = this.RendererObject;
			meshAttachment.regionOffsetX = this.regionOffsetX;
			meshAttachment.regionOffsetY = this.regionOffsetY;
			meshAttachment.regionWidth = this.regionWidth;
			meshAttachment.regionHeight = this.regionHeight;
			meshAttachment.regionOriginalWidth = this.regionOriginalWidth;
			meshAttachment.regionOriginalHeight = this.regionOriginalHeight;
			meshAttachment.RegionRotate = this.RegionRotate;
			meshAttachment.RegionDegrees = this.RegionDegrees;
			meshAttachment.RegionU = this.RegionU;
			meshAttachment.RegionV = this.RegionV;
			meshAttachment.RegionU2 = this.RegionU2;
			meshAttachment.RegionV2 = this.RegionV2;
			meshAttachment.Path = this.Path;
			meshAttachment.r = this.r;
			meshAttachment.g = this.g;
			meshAttachment.b = this.b;
			meshAttachment.a = this.a;
			base.CopyTo(meshAttachment);
			meshAttachment.regionUVs = new float[this.regionUVs.Length];
			Array.Copy(this.regionUVs, 0, meshAttachment.regionUVs, 0, this.regionUVs.Length);
			meshAttachment.uvs = new float[this.uvs.Length];
			Array.Copy(this.uvs, 0, meshAttachment.uvs, 0, this.uvs.Length);
			meshAttachment.triangles = new int[this.triangles.Length];
			Array.Copy(this.triangles, 0, meshAttachment.triangles, 0, this.triangles.Length);
			meshAttachment.HullLength = this.HullLength;
			if (this.Edges != null)
			{
				meshAttachment.Edges = new int[this.Edges.Length];
				Array.Copy(this.Edges, 0, meshAttachment.Edges, 0, this.Edges.Length);
			}
			meshAttachment.Width = this.Width;
			meshAttachment.Height = this.Height;
			return meshAttachment;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0005BAE0 File Offset: 0x00059CE0
		public MeshAttachment NewLinkedMesh()
		{
			MeshAttachment meshAttachment = new MeshAttachment(base.Name);
			meshAttachment.RendererObject = this.RendererObject;
			meshAttachment.regionOffsetX = this.regionOffsetX;
			meshAttachment.regionOffsetY = this.regionOffsetY;
			meshAttachment.regionWidth = this.regionWidth;
			meshAttachment.regionHeight = this.regionHeight;
			meshAttachment.regionOriginalWidth = this.regionOriginalWidth;
			meshAttachment.regionOriginalHeight = this.regionOriginalHeight;
			meshAttachment.RegionDegrees = this.RegionDegrees;
			meshAttachment.RegionRotate = this.RegionRotate;
			meshAttachment.RegionU = this.RegionU;
			meshAttachment.RegionV = this.RegionV;
			meshAttachment.RegionU2 = this.RegionU2;
			meshAttachment.RegionV2 = this.RegionV2;
			meshAttachment.Path = this.Path;
			meshAttachment.r = this.r;
			meshAttachment.g = this.g;
			meshAttachment.b = this.b;
			meshAttachment.a = this.a;
			meshAttachment.deformAttachment = this.deformAttachment;
			meshAttachment.ParentMesh = ((this.parentMesh == null) ? this : this.parentMesh);
			meshAttachment.UpdateUVs();
			return meshAttachment;
		}

		// Token: 0x04000B0A RID: 2826
		internal float regionOffsetX;

		// Token: 0x04000B0B RID: 2827
		internal float regionOffsetY;

		// Token: 0x04000B0C RID: 2828
		internal float regionWidth;

		// Token: 0x04000B0D RID: 2829
		internal float regionHeight;

		// Token: 0x04000B0E RID: 2830
		internal float regionOriginalWidth;

		// Token: 0x04000B0F RID: 2831
		internal float regionOriginalHeight;

		// Token: 0x04000B10 RID: 2832
		private MeshAttachment parentMesh;

		// Token: 0x04000B11 RID: 2833
		internal float[] uvs;

		// Token: 0x04000B12 RID: 2834
		internal float[] regionUVs;

		// Token: 0x04000B13 RID: 2835
		internal int[] triangles;

		// Token: 0x04000B14 RID: 2836
		internal float r = 1f;

		// Token: 0x04000B15 RID: 2837
		internal float g = 1f;

		// Token: 0x04000B16 RID: 2838
		internal float b = 1f;

		// Token: 0x04000B17 RID: 2839
		internal float a = 1f;

		// Token: 0x04000B18 RID: 2840
		internal int hulllength;
	}
}
