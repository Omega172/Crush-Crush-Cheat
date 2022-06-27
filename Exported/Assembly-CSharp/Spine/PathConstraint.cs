using System;

namespace Spine
{
	// Token: 0x020001BA RID: 442
	public class PathConstraint : IUpdatable
	{
		// Token: 0x06000DBA RID: 3514 RVA: 0x00060C00 File Offset: 0x0005EE00
		public PathConstraint(PathConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			this.data = data;
			this.bones = new ExposedList<Bone>(data.Bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				this.bones.Add(skeleton.FindBone(boneData.name));
			}
			this.target = skeleton.FindSlot(data.target.name);
			this.position = data.position;
			this.spacing = data.spacing;
			this.rotateMix = data.rotateMix;
			this.translateMix = data.translateMix;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00060D50 File Offset: 0x0005EF50
		public PathConstraint(PathConstraint constraint, Skeleton skeleton)
		{
			if (constraint == null)
			{
				throw new ArgumentNullException("constraint cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			this.data = constraint.data;
			this.bones = new ExposedList<Bone>(constraint.Bones.Count);
			foreach (Bone bone in constraint.Bones)
			{
				this.bones.Add(skeleton.Bones.Items[bone.data.index]);
			}
			this.target = skeleton.slots.Items[constraint.target.data.index];
			this.position = constraint.position;
			this.spacing = constraint.spacing;
			this.rotateMix = constraint.rotateMix;
			this.translateMix = constraint.translateMix;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00060EB4 File Offset: 0x0005F0B4
		public void Apply()
		{
			this.Update();
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00060EBC File Offset: 0x0005F0BC
		public void Update()
		{
			PathAttachment pathAttachment = this.target.Attachment as PathAttachment;
			if (pathAttachment == null)
			{
				return;
			}
			float num = this.rotateMix;
			float num2 = this.translateMix;
			bool flag = num2 > 0f;
			bool flag2 = num > 0f;
			if (!flag && !flag2)
			{
				return;
			}
			PathConstraintData pathConstraintData = this.data;
			bool flag3 = pathConstraintData.spacingMode == SpacingMode.Percent;
			RotateMode rotateMode = pathConstraintData.rotateMode;
			bool flag4 = rotateMode == RotateMode.Tangent;
			bool flag5 = rotateMode == RotateMode.ChainScale;
			int count = this.bones.Count;
			int num3 = (!flag4) ? (count + 1) : count;
			Bone[] items = this.bones.Items;
			ExposedList<float> exposedList = this.spaces.Resize(num3);
			ExposedList<float> exposedList2 = null;
			float num4 = this.spacing;
			if (flag5 || !flag3)
			{
				if (flag5)
				{
					exposedList2 = this.lengths.Resize(count);
				}
				bool flag6 = pathConstraintData.spacingMode == SpacingMode.Length;
				int i = 0;
				int num5 = num3 - 1;
				while (i < num5)
				{
					Bone bone = items[i];
					float length = bone.data.length;
					if (length < 1E-05f)
					{
						if (flag5)
						{
							exposedList2.Items[i] = 0f;
						}
						exposedList.Items[++i] = 0f;
					}
					else if (flag3)
					{
						if (flag5)
						{
							float num6 = length * bone.a;
							float num7 = length * bone.c;
							float num8 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7));
							exposedList2.Items[i] = num8;
						}
						exposedList.Items[++i] = num4;
					}
					else
					{
						float num9 = length * bone.a;
						float num10 = length * bone.c;
						float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
						if (flag5)
						{
							exposedList2.Items[i] = num11;
						}
						exposedList.Items[++i] = ((!flag6) ? num4 : (length + num4)) * num11 / length;
					}
				}
			}
			else
			{
				for (int j = 1; j < num3; j++)
				{
					exposedList.Items[j] = num4;
				}
			}
			float[] array = this.ComputeWorldPositions(pathAttachment, num3, flag4, pathConstraintData.positionMode == PositionMode.Percent, flag3);
			float num12 = array[0];
			float num13 = array[1];
			float num14 = pathConstraintData.offsetRotation;
			bool flag7;
			if (num14 == 0f)
			{
				flag7 = (rotateMode == RotateMode.Chain);
			}
			else
			{
				flag7 = false;
				Bone bone2 = this.target.bone;
				num14 *= ((bone2.a * bone2.d - bone2.b * bone2.c <= 0f) ? -0.017453292f : 0.017453292f);
			}
			int k = 0;
			int num15 = 3;
			while (k < count)
			{
				Bone bone3 = items[k];
				bone3.worldX += (num12 - bone3.worldX) * num2;
				bone3.worldY += (num13 - bone3.worldY) * num2;
				float num16 = array[num15];
				float num17 = array[num15 + 1];
				float num18 = num16 - num12;
				float num19 = num17 - num13;
				if (flag5)
				{
					float num20 = exposedList2.Items[k];
					if (num20 >= 1E-05f)
					{
						float num21 = ((float)Math.Sqrt((double)(num18 * num18 + num19 * num19)) / num20 - 1f) * num + 1f;
						bone3.a *= num21;
						bone3.c *= num21;
					}
				}
				num12 = num16;
				num13 = num17;
				if (flag2)
				{
					float a = bone3.a;
					float b = bone3.b;
					float c = bone3.c;
					float d = bone3.d;
					float num22;
					if (flag4)
					{
						num22 = array[num15 - 1];
					}
					else if (exposedList.Items[k + 1] < 1E-05f)
					{
						num22 = array[num15 + 2];
					}
					else
					{
						num22 = MathUtils.Atan2(num19, num18);
					}
					num22 -= MathUtils.Atan2(c, a);
					float num23;
					float num24;
					if (flag7)
					{
						num23 = MathUtils.Cos(num22);
						num24 = MathUtils.Sin(num22);
						float length2 = bone3.data.length;
						num12 += (length2 * (num23 * a - num24 * c) - num18) * num;
						num13 += (length2 * (num24 * a + num23 * c) - num19) * num;
					}
					else
					{
						num22 += num14;
					}
					if (num22 > 3.1415927f)
					{
						num22 -= 6.2831855f;
					}
					else if (num22 < -3.1415927f)
					{
						num22 += 6.2831855f;
					}
					num22 *= num;
					num23 = MathUtils.Cos(num22);
					num24 = MathUtils.Sin(num22);
					bone3.a = num23 * a - num24 * c;
					bone3.b = num23 * b - num24 * d;
					bone3.c = num24 * a + num23 * c;
					bone3.d = num24 * b + num23 * d;
				}
				bone3.appliedValid = false;
				k++;
				num15 += 3;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000613F8 File Offset: 0x0005F5F8
		private float[] ComputeWorldPositions(PathAttachment path, int spacesCount, bool tangents, bool percentPosition, bool percentSpacing)
		{
			Slot slot = this.target;
			float num = this.position;
			float[] items = this.spaces.Items;
			float[] items2 = this.positions.Resize(spacesCount * 3 + 2).Items;
			bool closed = path.Closed;
			int num2 = path.WorldVerticesLength;
			int num3 = num2 / 6;
			int num4 = -1;
			float num5;
			float[] items3;
			if (!path.ConstantSpeed)
			{
				float[] array = path.Lengths;
				num3 -= ((!closed) ? 2 : 1);
				num5 = array[num3];
				if (percentPosition)
				{
					num *= num5;
				}
				if (percentSpacing)
				{
					for (int i = 1; i < spacesCount; i++)
					{
						items[i] *= num5;
					}
				}
				items3 = this.world.Resize(8).Items;
				int j = 0;
				int num6 = 0;
				int num7 = 0;
				while (j < spacesCount)
				{
					float num8 = items[j];
					num += num8;
					float num9 = num;
					if (closed)
					{
						num9 %= num5;
						if (num9 < 0f)
						{
							num9 += num5;
						}
						num7 = 0;
						goto IL_17F;
					}
					if (num9 < 0f)
					{
						if (num4 != -2)
						{
							num4 = -2;
							path.ComputeWorldVertices(slot, 2, 4, items3, 0, 2);
						}
						PathConstraint.AddBeforePosition(num9, items3, 0, items2, num6);
					}
					else
					{
						if (num9 <= num5)
						{
							goto IL_17F;
						}
						if (num4 != -3)
						{
							num4 = -3;
							path.ComputeWorldVertices(slot, num2 - 6, 4, items3, 0, 2);
						}
						PathConstraint.AddAfterPosition(num9 - num5, items3, 0, items2, num6);
					}
					IL_26A:
					j++;
					num6 += 3;
					continue;
					IL_17F:
					float num10;
					for (;;)
					{
						num10 = array[num7];
						if (num9 <= num10)
						{
							break;
						}
						num7++;
					}
					if (num7 == 0)
					{
						num9 /= num10;
					}
					else
					{
						float num11 = array[num7 - 1];
						num9 = (num9 - num11) / (num10 - num11);
					}
					if (num7 != num4)
					{
						num4 = num7;
						if (closed && num7 == num3)
						{
							path.ComputeWorldVertices(slot, num2 - 4, 4, items3, 0, 2);
							path.ComputeWorldVertices(slot, 0, 4, items3, 4, 2);
						}
						else
						{
							path.ComputeWorldVertices(slot, num7 * 6 + 2, 8, items3, 0, 2);
						}
					}
					PathConstraint.AddCurvePosition(num9, items3[0], items3[1], items3[2], items3[3], items3[4], items3[5], items3[6], items3[7], items2, num6, tangents || (j > 0 && num8 < 1E-05f));
					goto IL_26A;
				}
				return items2;
			}
			if (closed)
			{
				num2 += 2;
				items3 = this.world.Resize(num2).Items;
				path.ComputeWorldVertices(slot, 2, num2 - 4, items3, 0, 2);
				path.ComputeWorldVertices(slot, 0, 2, items3, num2 - 4, 2);
				items3[num2 - 2] = items3[0];
				items3[num2 - 1] = items3[1];
			}
			else
			{
				num3--;
				num2 -= 4;
				items3 = this.world.Resize(num2).Items;
				path.ComputeWorldVertices(slot, 2, num2, items3, 0, 2);
			}
			float[] items4 = this.curves.Resize(num3).Items;
			num5 = 0f;
			float num12 = items3[0];
			float num13 = items3[1];
			float num14 = 0f;
			float num15 = 0f;
			float num16 = 0f;
			float num17 = 0f;
			float num18 = 0f;
			float num19 = 0f;
			int k = 0;
			int num20 = 2;
			while (k < num3)
			{
				num14 = items3[num20];
				num15 = items3[num20 + 1];
				num16 = items3[num20 + 2];
				num17 = items3[num20 + 3];
				num18 = items3[num20 + 4];
				num19 = items3[num20 + 5];
				float num21 = (num12 - num14 * 2f + num16) * 0.1875f;
				float num22 = (num13 - num15 * 2f + num17) * 0.1875f;
				float num23 = ((num14 - num16) * 3f - num12 + num18) * 0.09375f;
				float num24 = ((num15 - num17) * 3f - num13 + num19) * 0.09375f;
				float num25 = num21 * 2f + num23;
				float num26 = num22 * 2f + num24;
				float num27 = (num14 - num12) * 0.75f + num21 + num23 * 0.16666667f;
				float num28 = (num15 - num13) * 0.75f + num22 + num24 * 0.16666667f;
				num5 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
				num27 += num25;
				num28 += num26;
				num25 += num23;
				num26 += num24;
				num5 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
				num27 += num25;
				num28 += num26;
				num5 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
				num27 += num25 + num23;
				num28 += num26 + num24;
				num5 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
				items4[k] = num5;
				num12 = num18;
				num13 = num19;
				k++;
				num20 += 6;
			}
			if (percentPosition)
			{
				num *= num5;
			}
			else
			{
				num *= num5 / path.lengths[num3 - 1];
			}
			if (percentSpacing)
			{
				for (int l = 1; l < spacesCount; l++)
				{
					items[l] *= num5;
				}
			}
			float[] array2 = this.segments;
			float num29 = 0f;
			int m = 0;
			int num30 = 0;
			int num31 = 0;
			int num32 = 0;
			while (m < spacesCount)
			{
				float num33 = items[m];
				num += num33;
				float num34 = num;
				if (closed)
				{
					num34 %= num5;
					if (num34 < 0f)
					{
						num34 += num5;
					}
					num31 = 0;
					goto IL_5E5;
				}
				if (num34 < 0f)
				{
					PathConstraint.AddBeforePosition(num34, items3, 0, items2, num30);
				}
				else
				{
					if (num34 <= num5)
					{
						goto IL_5E5;
					}
					PathConstraint.AddAfterPosition(num34 - num5, items3, num2 - 4, items2, num30);
				}
				IL_8A0:
				m++;
				num30 += 3;
				continue;
				IL_5E5:
				float num35;
				for (;;)
				{
					num35 = items4[num31];
					if (num34 <= num35)
					{
						break;
					}
					num31++;
				}
				if (num31 == 0)
				{
					num34 /= num35;
				}
				else
				{
					float num36 = items4[num31 - 1];
					num34 = (num34 - num36) / (num35 - num36);
				}
				if (num31 != num4)
				{
					num4 = num31;
					int n = num31 * 6;
					num12 = items3[n];
					num13 = items3[n + 1];
					num14 = items3[n + 2];
					num15 = items3[n + 3];
					num16 = items3[n + 4];
					num17 = items3[n + 5];
					num18 = items3[n + 6];
					num19 = items3[n + 7];
					float num21 = (num12 - num14 * 2f + num16) * 0.03f;
					float num22 = (num13 - num15 * 2f + num17) * 0.03f;
					float num23 = ((num14 - num16) * 3f - num12 + num18) * 0.006f;
					float num24 = ((num15 - num17) * 3f - num13 + num19) * 0.006f;
					float num25 = num21 * 2f + num23;
					float num26 = num22 * 2f + num24;
					float num27 = (num14 - num12) * 0.3f + num21 + num23 * 0.16666667f;
					float num28 = (num15 - num13) * 0.3f + num22 + num24 * 0.16666667f;
					num29 = (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
					array2[0] = num29;
					for (n = 1; n < 8; n++)
					{
						num27 += num25;
						num28 += num26;
						num25 += num23;
						num26 += num24;
						num29 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
						array2[n] = num29;
					}
					num27 += num25;
					num28 += num26;
					num29 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
					array2[8] = num29;
					num27 += num25 + num23;
					num28 += num26 + num24;
					num29 += (float)Math.Sqrt((double)(num27 * num27 + num28 * num28));
					array2[9] = num29;
					num32 = 0;
				}
				num34 *= num29;
				float num37;
				for (;;)
				{
					num37 = array2[num32];
					if (num34 <= num37)
					{
						break;
					}
					num32++;
				}
				if (num32 == 0)
				{
					num34 /= num37;
				}
				else
				{
					float num38 = array2[num32 - 1];
					num34 = (float)num32 + (num34 - num38) / (num37 - num38);
				}
				PathConstraint.AddCurvePosition(num34 * 0.1f, num12, num13, num14, num15, num16, num17, num18, num19, items2, num30, tangents || (m > 0 && num33 < 1E-05f));
				goto IL_8A0;
			}
			return items2;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00061CBC File Offset: 0x0005FEBC
		private static void AddBeforePosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i];
			float num2 = temp[i + 1];
			float x = temp[i + 2] - num;
			float y = temp[i + 3] - num2;
			float num3 = MathUtils.Atan2(y, x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00061D14 File Offset: 0x0005FF14
		private static void AddAfterPosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i + 2];
			float num2 = temp[i + 3];
			float x = num - temp[i];
			float y = num2 - temp[i + 1];
			float num3 = MathUtils.Atan2(y, x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00061D6C File Offset: 0x0005FF6C
		private static void AddCurvePosition(float p, float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x2, float y2, float[] output, int o, bool tangents)
		{
			if (p < 1E-05f || float.IsNaN(p))
			{
				output[o] = x1;
				output[o + 1] = y1;
				output[o + 2] = (float)Math.Atan2((double)(cy1 - y1), (double)(cx1 - x1));
				return;
			}
			float num = p * p;
			float num2 = num * p;
			float num3 = 1f - p;
			float num4 = num3 * num3;
			float num5 = num4 * num3;
			float num6 = num3 * p;
			float num7 = num6 * 3f;
			float num8 = num3 * num7;
			float num9 = num7 * p;
			float num10 = x1 * num5 + cx1 * num8 + cx2 * num9 + x2 * num2;
			float num11 = y1 * num5 + cy1 * num8 + cy2 * num9 + y2 * num2;
			output[o] = num10;
			output[o + 1] = num11;
			if (tangents)
			{
				if (p < 0.001f)
				{
					output[o + 2] = (float)Math.Atan2((double)(cy1 - y1), (double)(cx1 - x1));
				}
				else
				{
					output[o + 2] = (float)Math.Atan2((double)(num11 - (y1 * num4 + cy1 * num6 * 2f + cy2 * num)), (double)(num10 - (x1 * num4 + cx1 * num6 * 2f + cx2 * num)));
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00061E90 File Offset: 0x00060090
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x00061E98 File Offset: 0x00060098
		public float Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00061EA4 File Offset: 0x000600A4
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x00061EAC File Offset: 0x000600AC
		public float Spacing
		{
			get
			{
				return this.spacing;
			}
			set
			{
				this.spacing = value;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00061EB8 File Offset: 0x000600B8
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x00061EC0 File Offset: 0x000600C0
		public float RotateMix
		{
			get
			{
				return this.rotateMix;
			}
			set
			{
				this.rotateMix = value;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00061ECC File Offset: 0x000600CC
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00061ED4 File Offset: 0x000600D4
		public float TranslateMix
		{
			get
			{
				return this.translateMix;
			}
			set
			{
				this.translateMix = value;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00061EE0 File Offset: 0x000600E0
		public ExposedList<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00061EE8 File Offset: 0x000600E8
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x00061EF0 File Offset: 0x000600F0
		public Slot Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00061EFC File Offset: 0x000600FC
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00061F04 File Offset: 0x00060104
		public PathConstraintData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04000BD5 RID: 3029
		private const int NONE = -1;

		// Token: 0x04000BD6 RID: 3030
		private const int BEFORE = -2;

		// Token: 0x04000BD7 RID: 3031
		private const int AFTER = -3;

		// Token: 0x04000BD8 RID: 3032
		private const float Epsilon = 1E-05f;

		// Token: 0x04000BD9 RID: 3033
		internal PathConstraintData data;

		// Token: 0x04000BDA RID: 3034
		internal ExposedList<Bone> bones;

		// Token: 0x04000BDB RID: 3035
		internal Slot target;

		// Token: 0x04000BDC RID: 3036
		internal float position;

		// Token: 0x04000BDD RID: 3037
		internal float spacing;

		// Token: 0x04000BDE RID: 3038
		internal float rotateMix;

		// Token: 0x04000BDF RID: 3039
		internal float translateMix;

		// Token: 0x04000BE0 RID: 3040
		internal bool active;

		// Token: 0x04000BE1 RID: 3041
		internal ExposedList<float> spaces = new ExposedList<float>();

		// Token: 0x04000BE2 RID: 3042
		internal ExposedList<float> positions = new ExposedList<float>();

		// Token: 0x04000BE3 RID: 3043
		internal ExposedList<float> world = new ExposedList<float>();

		// Token: 0x04000BE4 RID: 3044
		internal ExposedList<float> curves = new ExposedList<float>();

		// Token: 0x04000BE5 RID: 3045
		internal ExposedList<float> lengths = new ExposedList<float>();

		// Token: 0x04000BE6 RID: 3046
		internal float[] segments = new float[10];
	}
}
