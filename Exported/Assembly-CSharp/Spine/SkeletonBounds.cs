using System;

namespace Spine
{
	// Token: 0x020001C3 RID: 451
	public class SkeletonBounds
	{
		// Token: 0x06000E3B RID: 3643 RVA: 0x00065D94 File Offset: 0x00063F94
		public SkeletonBounds()
		{
			this.BoundingBoxes = new ExposedList<BoundingBoxAttachment>();
			this.Polygons = new ExposedList<Polygon>();
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00065DC0 File Offset: 0x00063FC0
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00065DC8 File Offset: 0x00063FC8
		public ExposedList<BoundingBoxAttachment> BoundingBoxes { get; private set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00065DD4 File Offset: 0x00063FD4
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00065DDC File Offset: 0x00063FDC
		public ExposedList<Polygon> Polygons { get; private set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00065DE8 File Offset: 0x00063FE8
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x00065DF0 File Offset: 0x00063FF0
		public float MinX
		{
			get
			{
				return this.minX;
			}
			set
			{
				this.minX = value;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00065DFC File Offset: 0x00063FFC
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x00065E04 File Offset: 0x00064004
		public float MinY
		{
			get
			{
				return this.minY;
			}
			set
			{
				this.minY = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00065E10 File Offset: 0x00064010
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00065E18 File Offset: 0x00064018
		public float MaxX
		{
			get
			{
				return this.maxX;
			}
			set
			{
				this.maxX = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00065E24 File Offset: 0x00064024
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00065E2C File Offset: 0x0006402C
		public float MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00065E38 File Offset: 0x00064038
		public float Width
		{
			get
			{
				return this.maxX - this.minX;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x00065E48 File Offset: 0x00064048
		public float Height
		{
			get
			{
				return this.maxY - this.minY;
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00065E58 File Offset: 0x00064058
		public void Update(Skeleton skeleton, bool updateAabb)
		{
			ExposedList<BoundingBoxAttachment> boundingBoxes = this.BoundingBoxes;
			ExposedList<Polygon> polygons = this.Polygons;
			ExposedList<Slot> slots = skeleton.slots;
			int count = slots.Count;
			boundingBoxes.Clear(true);
			int i = 0;
			int count2 = polygons.Count;
			while (i < count2)
			{
				this.polygonPool.Add(polygons.Items[i]);
				i++;
			}
			polygons.Clear(true);
			for (int j = 0; j < count; j++)
			{
				Slot slot = slots.Items[j];
				if (slot.bone.active)
				{
					BoundingBoxAttachment boundingBoxAttachment = slot.attachment as BoundingBoxAttachment;
					if (boundingBoxAttachment != null)
					{
						boundingBoxes.Add(boundingBoxAttachment);
						int count3 = this.polygonPool.Count;
						Polygon polygon;
						if (count3 > 0)
						{
							polygon = this.polygonPool.Items[count3 - 1];
							this.polygonPool.RemoveAt(count3 - 1);
						}
						else
						{
							polygon = new Polygon();
						}
						polygons.Add(polygon);
						int worldVerticesLength = boundingBoxAttachment.worldVerticesLength;
						polygon.Count = worldVerticesLength;
						if (polygon.Vertices.Length < worldVerticesLength)
						{
							polygon.Vertices = new float[worldVerticesLength];
						}
						boundingBoxAttachment.ComputeWorldVertices(slot, polygon.Vertices);
					}
				}
			}
			if (updateAabb)
			{
				this.AabbCompute();
			}
			else
			{
				this.minX = -2.1474836E+09f;
				this.minY = -2.1474836E+09f;
				this.maxX = 2.1474836E+09f;
				this.maxY = 2.1474836E+09f;
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00065FE8 File Offset: 0x000641E8
		private void AabbCompute()
		{
			float val = 2.1474836E+09f;
			float val2 = 2.1474836E+09f;
			float val3 = -2.1474836E+09f;
			float val4 = -2.1474836E+09f;
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				Polygon polygon = polygons.Items[i];
				float[] vertices = polygon.Vertices;
				int j = 0;
				int count2 = polygon.Count;
				while (j < count2)
				{
					float val5 = vertices[j];
					float val6 = vertices[j + 1];
					val = Math.Min(val, val5);
					val2 = Math.Min(val2, val6);
					val3 = Math.Max(val3, val5);
					val4 = Math.Max(val4, val6);
					j += 2;
				}
				i++;
			}
			this.minX = val;
			this.minY = val2;
			this.maxX = val3;
			this.maxY = val4;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000660BC File Offset: 0x000642BC
		public bool AabbContainsPoint(float x, float y)
		{
			return x >= this.minX && x <= this.maxX && y >= this.minY && y <= this.maxY;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000660F4 File Offset: 0x000642F4
		public bool AabbIntersectsSegment(float x1, float y1, float x2, float y2)
		{
			float num = this.minX;
			float num2 = this.minY;
			float num3 = this.maxX;
			float num4 = this.maxY;
			if ((x1 <= num && x2 <= num) || (y1 <= num2 && y2 <= num2) || (x1 >= num3 && x2 >= num3) || (y1 >= num4 && y2 >= num4))
			{
				return false;
			}
			float num5 = (y2 - y1) / (x2 - x1);
			float num6 = num5 * (num - x1) + y1;
			if (num6 > num2 && num6 < num4)
			{
				return true;
			}
			num6 = num5 * (num3 - x1) + y1;
			if (num6 > num2 && num6 < num4)
			{
				return true;
			}
			float num7 = (num2 - y1) / num5 + x1;
			if (num7 > num && num7 < num3)
			{
				return true;
			}
			num7 = (num4 - y1) / num5 + x1;
			return num7 > num && num7 < num3;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000661D4 File Offset: 0x000643D4
		public bool AabbIntersectsSkeleton(SkeletonBounds bounds)
		{
			return this.minX < bounds.maxX && this.maxX > bounds.minX && this.minY < bounds.maxY && this.maxY > bounds.minY;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00066228 File Offset: 0x00064428
		public bool ContainsPoint(Polygon polygon, float x, float y)
		{
			float[] vertices = polygon.Vertices;
			int count = polygon.Count;
			int num = count - 2;
			bool flag = false;
			for (int i = 0; i < count; i += 2)
			{
				float num2 = vertices[i + 1];
				float num3 = vertices[num + 1];
				if ((num2 < y && num3 >= y) || (num3 < y && num2 >= y))
				{
					float num4 = vertices[i];
					if (num4 + (y - num2) / (num3 - num2) * (vertices[num] - num4) < x)
					{
						flag = !flag;
					}
				}
				num = i;
			}
			return flag;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x000662B8 File Offset: 0x000644B8
		public BoundingBoxAttachment ContainsPoint(float x, float y)
		{
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				if (this.ContainsPoint(polygons.Items[i], x, y))
				{
					return this.BoundingBoxes.Items[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0006630C File Offset: 0x0006450C
		public BoundingBoxAttachment IntersectsSegment(float x1, float y1, float x2, float y2)
		{
			ExposedList<Polygon> polygons = this.Polygons;
			int i = 0;
			int count = polygons.Count;
			while (i < count)
			{
				if (this.IntersectsSegment(polygons.Items[i], x1, y1, x2, y2))
				{
					return this.BoundingBoxes.Items[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00066360 File Offset: 0x00064560
		public bool IntersectsSegment(Polygon polygon, float x1, float y1, float x2, float y2)
		{
			float[] vertices = polygon.Vertices;
			int count = polygon.Count;
			float num = x1 - x2;
			float num2 = y1 - y2;
			float num3 = x1 * y2 - y1 * x2;
			float num4 = vertices[count - 2];
			float num5 = vertices[count - 1];
			for (int i = 0; i < count; i += 2)
			{
				float num6 = vertices[i];
				float num7 = vertices[i + 1];
				float num8 = num4 * num7 - num5 * num6;
				float num9 = num4 - num6;
				float num10 = num5 - num7;
				float num11 = num * num10 - num2 * num9;
				float num12 = (num3 * num9 - num * num8) / num11;
				if (((num12 >= num4 && num12 <= num6) || (num12 >= num6 && num12 <= num4)) && ((num12 >= x1 && num12 <= x2) || (num12 >= x2 && num12 <= x1)))
				{
					float num13 = (num3 * num10 - num2 * num8) / num11;
					if (((num13 >= num5 && num13 <= num7) || (num13 >= num7 && num13 <= num5)) && ((num13 >= y1 && num13 <= y2) || (num13 >= y2 && num13 <= y1)))
					{
						return true;
					}
				}
				num4 = num6;
				num5 = num7;
			}
			return false;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000664A0 File Offset: 0x000646A0
		public Polygon GetPolygon(BoundingBoxAttachment attachment)
		{
			int num = this.BoundingBoxes.IndexOf(attachment);
			return (num != -1) ? this.Polygons.Items[num] : null;
		}

		// Token: 0x04000C26 RID: 3110
		private ExposedList<Polygon> polygonPool = new ExposedList<Polygon>();

		// Token: 0x04000C27 RID: 3111
		private float minX;

		// Token: 0x04000C28 RID: 3112
		private float minY;

		// Token: 0x04000C29 RID: 3113
		private float maxX;

		// Token: 0x04000C2A RID: 3114
		private float maxY;
	}
}
