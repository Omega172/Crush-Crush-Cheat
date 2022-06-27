using System;

namespace Spine
{
	// Token: 0x0200017C RID: 380
	public class DeformTimeline : CurveTimeline, ISlotTimeline
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x00056BAC File Offset: 0x00054DAC
		public DeformTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount];
			this.frameVertices = new float[frameCount][];
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00056BD0 File Offset: 0x00054DD0
		public override int PropertyId
		{
			get
			{
				return 805306368 + this.attachment.id + this.slotIndex;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00056C08 File Offset: 0x00054E08
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x00056BEC File Offset: 0x00054DEC
		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("index must be >= 0.");
				}
				this.slotIndex = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00056C10 File Offset: 0x00054E10
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x00056C18 File Offset: 0x00054E18
		public VertexAttachment Attachment
		{
			get
			{
				return this.attachment;
			}
			set
			{
				this.attachment = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00056C24 File Offset: 0x00054E24
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00056C2C File Offset: 0x00054E2C
		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00056C38 File Offset: 0x00054E38
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x00056C40 File Offset: 0x00054E40
		public float[][] Vertices
		{
			get
			{
				return this.frameVertices;
			}
			set
			{
				this.frameVertices = value;
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00056C4C File Offset: 0x00054E4C
		public void SetFrame(int frameIndex, float time, float[] vertices)
		{
			this.frames[frameIndex] = time;
			this.frameVertices[frameIndex] = vertices;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00056C60 File Offset: 0x00054E60
		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			if (!slot.bone.active)
			{
				return;
			}
			VertexAttachment vertexAttachment = slot.attachment as VertexAttachment;
			if (vertexAttachment == null || vertexAttachment.DeformAttachment != this.attachment)
			{
				return;
			}
			ExposedList<float> deform = slot.Deform;
			if (deform.Count == 0)
			{
				blend = MixBlend.Setup;
			}
			float[][] array = this.frameVertices;
			int num = array[0].Length;
			float[] array2 = this.frames;
			if (time < array2[0])
			{
				MixBlend mixBlend = blend;
				if (mixBlend == MixBlend.Setup)
				{
					deform.Clear(true);
					return;
				}
				if (mixBlend != MixBlend.First)
				{
					return;
				}
				if (alpha == 1f)
				{
					deform.Clear(true);
					return;
				}
				if (deform.Capacity < num)
				{
					deform.Capacity = num;
				}
				deform.Count = num;
				float[] items = deform.Items;
				if (vertexAttachment.bones == null)
				{
					float[] vertices = vertexAttachment.vertices;
					for (int i = 0; i < num; i++)
					{
						items[i] += (vertices[i] - items[i]) * alpha;
					}
				}
				else
				{
					alpha = 1f - alpha;
					for (int j = 0; j < num; j++)
					{
						items[j] *= alpha;
					}
				}
				return;
			}
			else
			{
				if (deform.Capacity < num)
				{
					deform.Capacity = num;
				}
				deform.Count = num;
				float[] items = deform.Items;
				if (time >= array2[array2.Length - 1])
				{
					float[] array3 = array[array2.Length - 1];
					if (alpha == 1f)
					{
						if (blend == MixBlend.Add)
						{
							if (vertexAttachment.bones == null)
							{
								float[] vertices2 = vertexAttachment.vertices;
								for (int k = 0; k < num; k++)
								{
									items[k] += array3[k] - vertices2[k];
								}
							}
							else
							{
								for (int l = 0; l < num; l++)
								{
									items[l] += array3[l];
								}
							}
						}
						else
						{
							Array.Copy(array3, 0, items, 0, num);
						}
					}
					else
					{
						switch (blend)
						{
						case MixBlend.Setup:
							if (vertexAttachment.bones == null)
							{
								float[] vertices3 = vertexAttachment.vertices;
								for (int m = 0; m < num; m++)
								{
									float num2 = vertices3[m];
									items[m] = num2 + (array3[m] - num2) * alpha;
								}
							}
							else
							{
								for (int n = 0; n < num; n++)
								{
									items[n] = array3[n] * alpha;
								}
							}
							break;
						case MixBlend.First:
						case MixBlend.Replace:
							for (int num3 = 0; num3 < num; num3++)
							{
								items[num3] += (array3[num3] - items[num3]) * alpha;
							}
							break;
						case MixBlend.Add:
							if (vertexAttachment.bones == null)
							{
								float[] vertices4 = vertexAttachment.vertices;
								for (int num4 = 0; num4 < num; num4++)
								{
									items[num4] += (array3[num4] - vertices4[num4]) * alpha;
								}
							}
							else
							{
								for (int num5 = 0; num5 < num; num5++)
								{
									items[num5] += array3[num5] * alpha;
								}
							}
							break;
						}
					}
					return;
				}
				int num6 = Animation.BinarySearch(array2, time);
				float[] array4 = array[num6 - 1];
				float[] array5 = array[num6];
				float num7 = array2[num6];
				float curvePercent = base.GetCurvePercent(num6 - 1, 1f - (time - num7) / (array2[num6 - 1] - num7));
				if (alpha == 1f)
				{
					if (blend == MixBlend.Add)
					{
						if (vertexAttachment.bones == null)
						{
							float[] vertices5 = vertexAttachment.vertices;
							for (int num8 = 0; num8 < num; num8++)
							{
								float num9 = array4[num8];
								items[num8] += num9 + (array5[num8] - num9) * curvePercent - vertices5[num8];
							}
						}
						else
						{
							for (int num10 = 0; num10 < num; num10++)
							{
								float num11 = array4[num10];
								items[num10] += num11 + (array5[num10] - num11) * curvePercent;
							}
						}
					}
					else
					{
						for (int num12 = 0; num12 < num; num12++)
						{
							float num13 = array4[num12];
							items[num12] = num13 + (array5[num12] - num13) * curvePercent;
						}
					}
				}
				else
				{
					switch (blend)
					{
					case MixBlend.Setup:
						if (vertexAttachment.bones == null)
						{
							float[] vertices6 = vertexAttachment.vertices;
							for (int num14 = 0; num14 < num; num14++)
							{
								float num15 = array4[num14];
								float num16 = vertices6[num14];
								items[num14] = num16 + (num15 + (array5[num14] - num15) * curvePercent - num16) * alpha;
							}
						}
						else
						{
							for (int num17 = 0; num17 < num; num17++)
							{
								float num18 = array4[num17];
								items[num17] = (num18 + (array5[num17] - num18) * curvePercent) * alpha;
							}
						}
						break;
					case MixBlend.First:
					case MixBlend.Replace:
						for (int num19 = 0; num19 < num; num19++)
						{
							float num20 = array4[num19];
							items[num19] += (num20 + (array5[num19] - num20) * curvePercent - items[num19]) * alpha;
						}
						break;
					case MixBlend.Add:
						if (vertexAttachment.bones == null)
						{
							float[] vertices7 = vertexAttachment.vertices;
							for (int num21 = 0; num21 < num; num21++)
							{
								float num22 = array4[num21];
								items[num21] += (num22 + (array5[num21] - num22) * curvePercent - vertices7[num21]) * alpha;
							}
						}
						else
						{
							for (int num23 = 0; num23 < num; num23++)
							{
								float num24 = array4[num23];
								items[num23] += (num24 + (array5[num23] - num24) * curvePercent) * alpha;
							}
						}
						break;
					}
				}
				return;
			}
		}

		// Token: 0x04000A4A RID: 2634
		internal int slotIndex;

		// Token: 0x04000A4B RID: 2635
		internal VertexAttachment attachment;

		// Token: 0x04000A4C RID: 2636
		internal float[] frames;

		// Token: 0x04000A4D RID: 2637
		internal float[][] frameVertices;
	}
}
