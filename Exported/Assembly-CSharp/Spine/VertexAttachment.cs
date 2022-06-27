using System;

namespace Spine
{
	// Token: 0x020001A0 RID: 416
	public abstract class VertexAttachment : Attachment
	{
		// Token: 0x06000C56 RID: 3158 RVA: 0x0005C3C4 File Offset: 0x0005A5C4
		public VertexAttachment(string name) : base(name)
		{
			this.deformAttachment = this;
			object obj = VertexAttachment.nextIdLock;
			lock (obj)
			{
				this.id = (VertexAttachment.nextID++ & 65535) << 11;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0005C444 File Offset: 0x0005A644
		public int Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0005C44C File Offset: 0x0005A64C
		// (set) Token: 0x06000C5A RID: 3162 RVA: 0x0005C454 File Offset: 0x0005A654
		public int[] Bones
		{
			get
			{
				return this.bones;
			}
			set
			{
				this.bones = value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0005C460 File Offset: 0x0005A660
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x0005C468 File Offset: 0x0005A668
		public float[] Vertices
		{
			get
			{
				return this.vertices;
			}
			set
			{
				this.vertices = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0005C474 File Offset: 0x0005A674
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x0005C47C File Offset: 0x0005A67C
		public int WorldVerticesLength
		{
			get
			{
				return this.worldVerticesLength;
			}
			set
			{
				this.worldVerticesLength = value;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0005C488 File Offset: 0x0005A688
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x0005C490 File Offset: 0x0005A690
		public VertexAttachment DeformAttachment
		{
			get
			{
				return this.deformAttachment;
			}
			set
			{
				this.deformAttachment = value;
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0005C49C File Offset: 0x0005A69C
		public void ComputeWorldVertices(Slot slot, float[] worldVertices)
		{
			this.ComputeWorldVertices(slot, 0, this.worldVerticesLength, worldVertices, 0, 2);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0005C4B0 File Offset: 0x0005A6B0
		public void ComputeWorldVertices(Slot slot, int start, int count, float[] worldVertices, int offset, int stride = 2)
		{
			count = offset + (count >> 1) * stride;
			Skeleton skeleton = slot.bone.skeleton;
			ExposedList<float> deform = slot.deform;
			float[] items = this.vertices;
			int[] array = this.bones;
			if (array == null)
			{
				if (deform.Count > 0)
				{
					items = deform.Items;
				}
				Bone bone = slot.bone;
				float worldX = bone.worldX;
				float worldY = bone.worldY;
				float a = bone.a;
				float b = bone.b;
				float c = bone.c;
				float d = bone.d;
				int num = start;
				for (int i = offset; i < count; i += stride)
				{
					float num2 = items[num];
					float num3 = items[num + 1];
					worldVertices[i] = num2 * a + num3 * b + worldX;
					worldVertices[i + 1] = num2 * c + num3 * d + worldY;
					num += 2;
				}
				return;
			}
			int j = 0;
			int num4 = 0;
			for (int k = 0; k < start; k += 2)
			{
				int num5 = array[j];
				j += num5 + 1;
				num4 += num5;
			}
			Bone[] items2 = skeleton.bones.Items;
			if (deform.Count == 0)
			{
				int l = offset;
				int num6 = num4 * 3;
				while (l < count)
				{
					float num7 = 0f;
					float num8 = 0f;
					int num9 = array[j++];
					num9 += j;
					while (j < num9)
					{
						Bone bone2 = items2[array[j]];
						float num10 = items[num6];
						float num11 = items[num6 + 1];
						float num12 = items[num6 + 2];
						num7 += (num10 * bone2.a + num11 * bone2.b + bone2.worldX) * num12;
						num8 += (num10 * bone2.c + num11 * bone2.d + bone2.worldY) * num12;
						j++;
						num6 += 3;
					}
					worldVertices[l] = num7;
					worldVertices[l + 1] = num8;
					l += stride;
				}
			}
			else
			{
				float[] items3 = deform.Items;
				int m = offset;
				int num13 = num4 * 3;
				int num14 = num4 << 1;
				while (m < count)
				{
					float num15 = 0f;
					float num16 = 0f;
					int num17 = array[j++];
					num17 += j;
					while (j < num17)
					{
						Bone bone3 = items2[array[j]];
						float num18 = items[num13] + items3[num14];
						float num19 = items[num13 + 1] + items3[num14 + 1];
						float num20 = items[num13 + 2];
						num15 += (num18 * bone3.a + num19 * bone3.b + bone3.worldX) * num20;
						num16 += (num18 * bone3.c + num19 * bone3.d + bone3.worldY) * num20;
						j++;
						num13 += 3;
						num14 += 2;
					}
					worldVertices[m] = num15;
					worldVertices[m + 1] = num16;
					m += stride;
				}
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0005C7AC File Offset: 0x0005A9AC
		internal void CopyTo(VertexAttachment attachment)
		{
			if (this.bones != null)
			{
				attachment.bones = new int[this.bones.Length];
				Array.Copy(this.bones, 0, attachment.bones, 0, this.bones.Length);
			}
			else
			{
				attachment.bones = null;
			}
			if (this.vertices != null)
			{
				attachment.vertices = new float[this.vertices.Length];
				Array.Copy(this.vertices, 0, attachment.vertices, 0, this.vertices.Length);
			}
			else
			{
				attachment.vertices = null;
			}
			attachment.worldVerticesLength = this.worldVerticesLength;
			attachment.deformAttachment = this.deformAttachment;
		}

		// Token: 0x04000B47 RID: 2887
		private static int nextID = 0;

		// Token: 0x04000B48 RID: 2888
		private static readonly object nextIdLock = new object();

		// Token: 0x04000B49 RID: 2889
		internal readonly int id;

		// Token: 0x04000B4A RID: 2890
		internal int[] bones;

		// Token: 0x04000B4B RID: 2891
		internal float[] vertices;

		// Token: 0x04000B4C RID: 2892
		internal int worldVerticesLength;

		// Token: 0x04000B4D RID: 2893
		internal VertexAttachment deformAttachment;
	}
}
