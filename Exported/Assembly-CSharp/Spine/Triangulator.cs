using System;

namespace Spine
{
	// Token: 0x020001D0 RID: 464
	public class Triangulator
	{
		// Token: 0x06000F29 RID: 3881 RVA: 0x0006C640 File Offset: 0x0006A840
		public ExposedList<int> Triangulate(ExposedList<float> verticesArray)
		{
			float[] items = verticesArray.Items;
			int i = verticesArray.Count >> 1;
			ExposedList<int> exposedList = this.indicesArray;
			exposedList.Clear(true);
			int[] items2 = exposedList.Resize(i).Items;
			for (int j = 0; j < i; j++)
			{
				items2[j] = j;
			}
			ExposedList<bool> exposedList2 = this.isConcaveArray;
			bool[] items3 = exposedList2.Resize(i).Items;
			int k = 0;
			int num = i;
			while (k < num)
			{
				items3[k] = Triangulator.IsConcave(k, i, items, items2);
				k++;
			}
			ExposedList<int> exposedList3 = this.triangles;
			exposedList3.Clear(true);
			exposedList3.EnsureCapacity(Math.Max(0, i - 2) << 2);
			while (i > 3)
			{
				int num2 = i - 1;
				int num3 = 0;
				int num4 = 1;
				for (;;)
				{
					if (!items3[num3])
					{
						int num5 = items2[num2] << 1;
						int num6 = items2[num3] << 1;
						int num7 = items2[num4] << 1;
						float num8 = items[num5];
						float num9 = items[num5 + 1];
						float num10 = items[num6];
						float num11 = items[num6 + 1];
						float num12 = items[num7];
						float num13 = items[num7 + 1];
						for (int num14 = (num4 + 1) % i; num14 != num2; num14 = (num14 + 1) % i)
						{
							if (items3[num14])
							{
								int num15 = items2[num14] << 1;
								float p3x = items[num15];
								float p3y = items[num15 + 1];
								if (Triangulator.PositiveArea(num12, num13, num8, num9, p3x, p3y) && Triangulator.PositiveArea(num8, num9, num10, num11, p3x, p3y) && Triangulator.PositiveArea(num10, num11, num12, num13, p3x, p3y))
								{
									goto IL_194;
								}
							}
						}
						goto Block_8;
					}
					IL_194:
					if (num4 == 0)
					{
						break;
					}
					num2 = num3;
					num3 = num4;
					num4 = (num4 + 1) % i;
				}
				while (items3[num3])
				{
					num3--;
					if (num3 <= 0)
					{
						break;
					}
				}
				IL_1D2:
				exposedList3.Add(items2[(i + num3 - 1) % i]);
				exposedList3.Add(items2[num3]);
				exposedList3.Add(items2[(num3 + 1) % i]);
				exposedList.RemoveAt(num3);
				exposedList2.RemoveAt(num3);
				i--;
				int num16 = (i + num3 - 1) % i;
				int num17 = (num3 != i) ? num3 : 0;
				items3[num16] = Triangulator.IsConcave(num16, i, items, items2);
				items3[num17] = Triangulator.IsConcave(num17, i, items, items2);
				continue;
				Block_8:
				goto IL_1D2;
			}
			if (i == 3)
			{
				exposedList3.Add(items2[2]);
				exposedList3.Add(items2[0]);
				exposedList3.Add(items2[1]);
			}
			return exposedList3;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0006C8C8 File Offset: 0x0006AAC8
		public ExposedList<ExposedList<float>> Decompose(ExposedList<float> verticesArray, ExposedList<int> triangles)
		{
			float[] items = verticesArray.Items;
			ExposedList<ExposedList<float>> exposedList = this.convexPolygons;
			int i = 0;
			int count = exposedList.Count;
			while (i < count)
			{
				this.polygonPool.Free(exposedList.Items[i]);
				i++;
			}
			exposedList.Clear(true);
			ExposedList<ExposedList<int>> exposedList2 = this.convexPolygonsIndices;
			int j = 0;
			int count2 = exposedList2.Count;
			while (j < count2)
			{
				this.polygonIndicesPool.Free(exposedList2.Items[j]);
				j++;
			}
			exposedList2.Clear(true);
			ExposedList<int> exposedList3 = this.polygonIndicesPool.Obtain();
			exposedList3.Clear(true);
			ExposedList<float> exposedList4 = this.polygonPool.Obtain();
			exposedList4.Clear(true);
			int num = -1;
			int num2 = 0;
			int[] items2 = triangles.Items;
			int k = 0;
			int count3 = triangles.Count;
			while (k < count3)
			{
				int num3 = items2[k] << 1;
				int num4 = items2[k + 1] << 1;
				int num5 = items2[k + 2] << 1;
				float num6 = items[num3];
				float num7 = items[num3 + 1];
				float num8 = items[num4];
				float num9 = items[num4 + 1];
				float num10 = items[num5];
				float num11 = items[num5 + 1];
				bool flag = false;
				if (num == num3)
				{
					int num12 = exposedList4.Count - 4;
					float[] items3 = exposedList4.Items;
					int num13 = Triangulator.Winding(items3[num12], items3[num12 + 1], items3[num12 + 2], items3[num12 + 3], num10, num11);
					int num14 = Triangulator.Winding(num10, num11, items3[0], items3[1], items3[2], items3[3]);
					if (num13 == num2 && num14 == num2)
					{
						exposedList4.Add(num10);
						exposedList4.Add(num11);
						exposedList3.Add(num5);
						flag = true;
					}
				}
				if (!flag)
				{
					if (exposedList4.Count > 0)
					{
						exposedList.Add(exposedList4);
						exposedList2.Add(exposedList3);
					}
					else
					{
						this.polygonPool.Free(exposedList4);
						this.polygonIndicesPool.Free(exposedList3);
					}
					exposedList4 = this.polygonPool.Obtain();
					exposedList4.Clear(true);
					exposedList4.Add(num6);
					exposedList4.Add(num7);
					exposedList4.Add(num8);
					exposedList4.Add(num9);
					exposedList4.Add(num10);
					exposedList4.Add(num11);
					exposedList3 = this.polygonIndicesPool.Obtain();
					exposedList3.Clear(true);
					exposedList3.Add(num3);
					exposedList3.Add(num4);
					exposedList3.Add(num5);
					num2 = Triangulator.Winding(num6, num7, num8, num9, num10, num11);
					num = num3;
				}
				k += 3;
			}
			if (exposedList4.Count > 0)
			{
				exposedList.Add(exposedList4);
				exposedList2.Add(exposedList3);
			}
			int l = 0;
			int count4 = exposedList.Count;
			while (l < count4)
			{
				exposedList3 = exposedList2.Items[l];
				if (exposedList3.Count != 0)
				{
					int num15 = exposedList3.Items[0];
					int num16 = exposedList3.Items[exposedList3.Count - 1];
					exposedList4 = exposedList.Items[l];
					int num17 = exposedList4.Count - 4;
					float[] items4 = exposedList4.Items;
					float p1x = items4[num17];
					float p1y = items4[num17 + 1];
					float num18 = items4[num17 + 2];
					float num19 = items4[num17 + 3];
					float num20 = items4[0];
					float num21 = items4[1];
					float p3x = items4[2];
					float p3y = items4[3];
					int num22 = Triangulator.Winding(p1x, p1y, num18, num19, num20, num21);
					for (int m = 0; m < count4; m++)
					{
						if (m != l)
						{
							ExposedList<int> exposedList5 = exposedList2.Items[m];
							if (exposedList5.Count == 3)
							{
								int num23 = exposedList5.Items[0];
								int num24 = exposedList5.Items[1];
								int item = exposedList5.Items[2];
								ExposedList<float> exposedList6 = exposedList.Items[m];
								float num25 = exposedList6.Items[exposedList6.Count - 2];
								float num26 = exposedList6.Items[exposedList6.Count - 1];
								if (num23 == num15 && num24 == num16)
								{
									int num27 = Triangulator.Winding(p1x, p1y, num18, num19, num25, num26);
									int num28 = Triangulator.Winding(num25, num26, num20, num21, p3x, p3y);
									if (num27 == num22 && num28 == num22)
									{
										exposedList6.Clear(true);
										exposedList5.Clear(true);
										exposedList4.Add(num25);
										exposedList4.Add(num26);
										exposedList3.Add(item);
										p1x = num18;
										p1y = num19;
										num18 = num25;
										num19 = num26;
										m = 0;
									}
								}
							}
						}
					}
				}
				l++;
			}
			for (int n = exposedList.Count - 1; n >= 0; n--)
			{
				exposedList4 = exposedList.Items[n];
				if (exposedList4.Count == 0)
				{
					exposedList.RemoveAt(n);
					this.polygonPool.Free(exposedList4);
					exposedList3 = exposedList2.Items[n];
					exposedList2.RemoveAt(n);
					this.polygonIndicesPool.Free(exposedList3);
				}
			}
			return exposedList;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0006CDD4 File Offset: 0x0006AFD4
		private static bool IsConcave(int index, int vertexCount, float[] vertices, int[] indices)
		{
			int num = indices[(vertexCount + index - 1) % vertexCount] << 1;
			int num2 = indices[index] << 1;
			int num3 = indices[(index + 1) % vertexCount] << 1;
			return !Triangulator.PositiveArea(vertices[num], vertices[num + 1], vertices[num2], vertices[num2 + 1], vertices[num3], vertices[num3 + 1]);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0006CE20 File Offset: 0x0006B020
		private static bool PositiveArea(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
		{
			return p1x * (p3y - p2y) + p2x * (p1y - p3y) + p3x * (p2y - p1y) >= 0f;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0006CE40 File Offset: 0x0006B040
		private static int Winding(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
		{
			float num = p2x - p1x;
			float num2 = p2y - p1y;
			return (p3x * num2 - p3y * num + num * p1y - p1x * num2 < 0f) ? -1 : 1;
		}

		// Token: 0x04000C8D RID: 3213
		private readonly ExposedList<ExposedList<float>> convexPolygons = new ExposedList<ExposedList<float>>();

		// Token: 0x04000C8E RID: 3214
		private readonly ExposedList<ExposedList<int>> convexPolygonsIndices = new ExposedList<ExposedList<int>>();

		// Token: 0x04000C8F RID: 3215
		private readonly ExposedList<int> indicesArray = new ExposedList<int>();

		// Token: 0x04000C90 RID: 3216
		private readonly ExposedList<bool> isConcaveArray = new ExposedList<bool>();

		// Token: 0x04000C91 RID: 3217
		private readonly ExposedList<int> triangles = new ExposedList<int>();

		// Token: 0x04000C92 RID: 3218
		private readonly Pool<ExposedList<float>> polygonPool = new Pool<ExposedList<float>>(16, int.MaxValue);

		// Token: 0x04000C93 RID: 3219
		private readonly Pool<ExposedList<int>> polygonIndicesPool = new Pool<ExposedList<int>>(16, int.MaxValue);
	}
}
