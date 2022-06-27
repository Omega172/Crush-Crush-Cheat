using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000210 RID: 528
	[Serializable]
	public class MeshGenerator
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x00076498 File Offset: 0x00074698
		public MeshGenerator()
		{
			this.submeshes.TrimExcess();
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00076564 File Offset: 0x00074764
		public int VertexCount
		{
			get
			{
				return this.vertexBuffer.Count;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00076574 File Offset: 0x00074774
		public MeshGeneratorBuffers Buffers
		{
			get
			{
				return new MeshGeneratorBuffers
				{
					vertexCount = this.VertexCount,
					vertexBuffer = this.vertexBuffer.Items,
					uvBuffer = this.uvBuffer.Items,
					colorBuffer = this.colorBuffer.Items,
					meshGenerator = this
				};
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000765D8 File Offset: 0x000747D8
		public static void GenerateSingleSubmeshInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Material material)
		{
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			int count = drawOrder.Count;
			instructionOutput.Clear();
			ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
			instructionOutput.attachments.Resize(count);
			Attachment[] items = instructionOutput.attachments.Items;
			int num = 0;
			SubmeshInstruction submeshInstruction = default(SubmeshInstruction);
			SubmeshInstruction submeshInstruction2 = submeshInstruction;
			submeshInstruction2.skeleton = skeleton;
			submeshInstruction2.preActiveClippingSlotSource = -1;
			submeshInstruction2.startSlot = 0;
			submeshInstruction2.rawFirstVertexIndex = 0;
			submeshInstruction2.material = material;
			submeshInstruction2.forceSeparate = false;
			submeshInstruction2.endSlot = count;
			submeshInstruction = submeshInstruction2;
			object obj = null;
			bool hasActiveClipping = false;
			Slot[] items2 = drawOrder.Items;
			for (int i = 0; i < count; i++)
			{
				Slot slot = items2[i];
				if (slot.bone.active)
				{
					Attachment attachment = slot.attachment;
					items[i] = attachment;
					RegionAttachment regionAttachment = attachment as RegionAttachment;
					int num2;
					int num3;
					if (regionAttachment != null)
					{
						obj = regionAttachment.RendererObject;
						num2 = 4;
						num3 = 6;
					}
					else
					{
						MeshAttachment meshAttachment = attachment as MeshAttachment;
						if (meshAttachment != null)
						{
							obj = meshAttachment.RendererObject;
							num2 = meshAttachment.worldVerticesLength >> 1;
							num3 = meshAttachment.triangles.Length;
						}
						else
						{
							ClippingAttachment clippingAttachment = attachment as ClippingAttachment;
							if (clippingAttachment != null)
							{
								submeshInstruction.hasClipping = true;
								hasActiveClipping = true;
							}
							num2 = 0;
							num3 = 0;
						}
					}
					submeshInstruction.rawTriangleCount += num3;
					submeshInstruction.rawVertexCount += num2;
					num += num2;
				}
			}
			if (material == null && obj != null)
			{
				submeshInstruction.material = (Material)((AtlasRegion)obj).page.rendererObject;
			}
			instructionOutput.hasActiveClipping = hasActiveClipping;
			instructionOutput.rawVertexCount = num;
			if (num > 0)
			{
				submeshInstructions.Resize(1);
				submeshInstructions.Items[0] = submeshInstruction;
			}
			else
			{
				submeshInstructions.Resize(0);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000767C4 File Offset: 0x000749C4
		public static bool RequiresMultipleSubmeshesByDrawOrder(Skeleton skeleton)
		{
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			int count = drawOrder.Count;
			Slot[] items = drawOrder.Items;
			Material x = null;
			for (int i = 0; i < count; i++)
			{
				Slot slot = items[i];
				if (slot.bone.active)
				{
					Attachment attachment = slot.attachment;
					IHasRendererObject hasRendererObject = attachment as IHasRendererObject;
					if (hasRendererObject != null)
					{
						AtlasRegion atlasRegion = (AtlasRegion)hasRendererObject.RendererObject;
						Material material = (Material)atlasRegion.page.rendererObject;
						if (x != material)
						{
							if (x != null)
							{
								return true;
							}
							x = material;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00076874 File Offset: 0x00074A74
		public static void GenerateSkeletonRendererInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Dictionary<Slot, Material> customSlotMaterials, List<Slot> separatorSlots, bool generateMeshOverride, bool immutableTriangles = false)
		{
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			int count = drawOrder.Count;
			instructionOutput.Clear();
			ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
			instructionOutput.attachments.Resize(count);
			Attachment[] items = instructionOutput.attachments.Items;
			int num = 0;
			bool hasActiveClipping = false;
			SubmeshInstruction submeshInstruction = default(SubmeshInstruction);
			SubmeshInstruction submeshInstruction2 = submeshInstruction;
			submeshInstruction2.skeleton = skeleton;
			submeshInstruction2.preActiveClippingSlotSource = -1;
			submeshInstruction = submeshInstruction2;
			bool flag = customSlotMaterials != null && customSlotMaterials.Count > 0;
			int num2 = (separatorSlots != null) ? separatorSlots.Count : 0;
			bool flag2 = num2 > 0;
			int num3 = -1;
			int preActiveClippingSlotSource = -1;
			SlotData slotData = null;
			int num4 = 0;
			Slot[] items2 = drawOrder.Items;
			for (int i = 0; i < count; i++)
			{
				Slot slot = items2[i];
				if (slot.bone.active)
				{
					Attachment attachment = slot.attachment;
					items[i] = attachment;
					int num5 = 0;
					int num6 = 0;
					object obj = null;
					bool flag3 = false;
					RegionAttachment regionAttachment = attachment as RegionAttachment;
					if (regionAttachment != null)
					{
						obj = regionAttachment.RendererObject;
						num5 = 4;
						num6 = 6;
					}
					else
					{
						MeshAttachment meshAttachment = attachment as MeshAttachment;
						if (meshAttachment != null)
						{
							obj = meshAttachment.RendererObject;
							num5 = meshAttachment.worldVerticesLength >> 1;
							num6 = meshAttachment.triangles.Length;
						}
						else
						{
							ClippingAttachment clippingAttachment = attachment as ClippingAttachment;
							if (clippingAttachment != null)
							{
								slotData = clippingAttachment.endSlot;
								num3 = i;
								submeshInstruction.hasClipping = true;
								hasActiveClipping = true;
							}
							flag3 = true;
						}
					}
					if (flag2)
					{
						submeshInstruction.forceSeparate = false;
						for (int j = 0; j < num2; j++)
						{
							if (object.ReferenceEquals(slot, separatorSlots[j]))
							{
								submeshInstruction.forceSeparate = true;
								break;
							}
						}
					}
					if (flag3)
					{
						if (submeshInstruction.forceSeparate && generateMeshOverride)
						{
							submeshInstruction.endSlot = i;
							submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
							submeshInstructions.Resize(num4 + 1);
							submeshInstructions.Items[num4] = submeshInstruction;
							num4++;
							submeshInstruction.startSlot = i;
							preActiveClippingSlotSource = num3;
							submeshInstruction.rawTriangleCount = 0;
							submeshInstruction.rawVertexCount = 0;
							submeshInstruction.rawFirstVertexIndex = num;
							submeshInstruction.hasClipping = (num3 >= 0);
						}
					}
					else
					{
						Material material;
						if (flag)
						{
							if (!customSlotMaterials.TryGetValue(slot, out material))
							{
								material = (Material)((AtlasRegion)obj).page.rendererObject;
							}
						}
						else
						{
							material = (Material)((AtlasRegion)obj).page.rendererObject;
						}
						if (submeshInstruction.forceSeparate || (submeshInstruction.rawVertexCount > 0 && !object.ReferenceEquals(submeshInstruction.material, material)))
						{
							submeshInstruction.endSlot = i;
							submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
							submeshInstructions.Resize(num4 + 1);
							submeshInstructions.Items[num4] = submeshInstruction;
							num4++;
							submeshInstruction.startSlot = i;
							preActiveClippingSlotSource = num3;
							submeshInstruction.rawTriangleCount = 0;
							submeshInstruction.rawVertexCount = 0;
							submeshInstruction.rawFirstVertexIndex = num;
							submeshInstruction.hasClipping = (num3 >= 0);
						}
						submeshInstruction.material = material;
						submeshInstruction.rawTriangleCount += num6;
						submeshInstruction.rawVertexCount += num5;
						submeshInstruction.rawFirstVertexIndex = num;
						num += num5;
					}
					if (slotData != null && slot.data == slotData && i != num3)
					{
						slotData = null;
						num3 = -1;
					}
				}
			}
			if (submeshInstruction.rawVertexCount > 0)
			{
				submeshInstruction.endSlot = count;
				submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
				submeshInstruction.forceSeparate = false;
				submeshInstructions.Resize(num4 + 1);
				submeshInstructions.Items[num4] = submeshInstruction;
			}
			instructionOutput.hasActiveClipping = hasActiveClipping;
			instructionOutput.rawVertexCount = num;
			instructionOutput.immutableTriangles = immutableTriangles;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00076C60 File Offset: 0x00074E60
		public static void TryReplaceMaterials(ExposedList<SubmeshInstruction> workingSubmeshInstructions, Dictionary<Material, Material> customMaterialOverride)
		{
			SubmeshInstruction[] items = workingSubmeshInstructions.Items;
			for (int i = 0; i < workingSubmeshInstructions.Count; i++)
			{
				Material material = items[i].material;
				Material material2;
				if (customMaterialOverride.TryGetValue(material, out material2))
				{
					items[i].material = material2;
				}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00076CB4 File Offset: 0x00074EB4
		public void Begin()
		{
			this.vertexBuffer.Clear(false);
			this.colorBuffer.Clear(false);
			this.uvBuffer.Clear(false);
			this.clipper.ClipEnd();
			this.meshBoundsMin.x = float.PositiveInfinity;
			this.meshBoundsMin.y = float.PositiveInfinity;
			this.meshBoundsMax.x = float.NegativeInfinity;
			this.meshBoundsMax.y = float.NegativeInfinity;
			this.meshBoundsThickness = 0f;
			this.submeshIndex = 0;
			this.submeshes.Count = 1;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00076D50 File Offset: 0x00074F50
		public void AddSubmesh(SubmeshInstruction instruction, bool updateTriangles = true)
		{
			MeshGenerator.Settings settings = this.settings;
			int num = this.submeshIndex + 1;
			if (this.submeshes.Items.Length < num)
			{
				this.submeshes.Resize(num);
			}
			this.submeshes.Count = num;
			ExposedList<int> exposedList = this.submeshes.Items[this.submeshIndex];
			if (exposedList == null)
			{
				exposedList = (this.submeshes.Items[this.submeshIndex] = new ExposedList<int>());
			}
			exposedList.Clear(false);
			Skeleton skeleton = instruction.skeleton;
			Slot[] items = skeleton.drawOrder.Items;
			Color32 color = default(Color32);
			float a = skeleton.a;
			float r = skeleton.r;
			float g = skeleton.g;
			float b = skeleton.b;
			Vector2 vector = this.meshBoundsMin;
			Vector2 vector2 = this.meshBoundsMax;
			float zSpacing = settings.zSpacing;
			bool pmaVertexColors = settings.pmaVertexColors;
			bool tintBlack = settings.tintBlack;
			bool flag = settings.useClipping && instruction.hasClipping;
			if (flag && instruction.preActiveClippingSlotSource >= 0)
			{
				Slot slot = items[instruction.preActiveClippingSlotSource];
				this.clipper.ClipStart(slot, slot.attachment as ClippingAttachment);
			}
			for (int i = instruction.startSlot; i < instruction.endSlot; i++)
			{
				Slot slot2 = items[i];
				if (!slot2.bone.active)
				{
					this.clipper.ClipEnd(slot2);
				}
				else
				{
					Attachment attachment = slot2.attachment;
					float z = zSpacing * (float)i;
					float[] array = this.tempVerts;
					Color color2 = default(Color);
					RegionAttachment regionAttachment = attachment as RegionAttachment;
					float[] array2;
					int[] array3;
					int num2;
					int num3;
					if (regionAttachment != null)
					{
						regionAttachment.ComputeWorldVertices(slot2.bone, array, 0, 2);
						array2 = regionAttachment.uvs;
						array3 = this.regionTriangles;
						color2.r = regionAttachment.r;
						color2.g = regionAttachment.g;
						color2.b = regionAttachment.b;
						color2.a = regionAttachment.a;
						num2 = 4;
						num3 = 6;
					}
					else
					{
						MeshAttachment meshAttachment = attachment as MeshAttachment;
						if (meshAttachment == null)
						{
							if (flag)
							{
								ClippingAttachment clippingAttachment = attachment as ClippingAttachment;
								if (clippingAttachment != null)
								{
									this.clipper.ClipStart(slot2, clippingAttachment);
									goto IL_85A;
								}
							}
							this.clipper.ClipEnd(slot2);
							goto IL_85A;
						}
						int worldVerticesLength = meshAttachment.worldVerticesLength;
						if (array.Length < worldVerticesLength)
						{
							array = new float[worldVerticesLength];
							this.tempVerts = array;
						}
						meshAttachment.ComputeWorldVertices(slot2, 0, worldVerticesLength, array, 0, 2);
						array2 = meshAttachment.uvs;
						array3 = meshAttachment.triangles;
						color2.r = meshAttachment.r;
						color2.g = meshAttachment.g;
						color2.b = meshAttachment.b;
						color2.a = meshAttachment.a;
						num2 = worldVerticesLength >> 1;
						num3 = meshAttachment.triangles.Length;
					}
					if (pmaVertexColors)
					{
						color.a = (byte)(a * slot2.a * color2.a * 255f);
						color.r = (byte)(r * slot2.r * color2.r * (float)color.a);
						color.g = (byte)(g * slot2.g * color2.g * (float)color.a);
						color.b = (byte)(b * slot2.b * color2.b * (float)color.a);
						if (slot2.data.blendMode == BlendMode.Additive)
						{
							color.a = 0;
						}
					}
					else
					{
						color.a = (byte)(a * slot2.a * color2.a * 255f);
						color.r = (byte)(r * slot2.r * color2.r * 255f);
						color.g = (byte)(g * slot2.g * color2.g * 255f);
						color.b = (byte)(b * slot2.b * color2.b * 255f);
					}
					if (flag && this.clipper.IsClipping)
					{
						this.clipper.ClipTriangles(array, num2 << 1, array3, num3, array2);
						array = this.clipper.clippedVertices.Items;
						num2 = this.clipper.clippedVertices.Count >> 1;
						array3 = this.clipper.clippedTriangles.Items;
						num3 = this.clipper.clippedTriangles.Count;
						array2 = this.clipper.clippedUVs.Items;
					}
					if (num2 != 0 && num3 != 0)
					{
						if (tintBlack)
						{
							float num4 = slot2.r2;
							float num5 = slot2.g2;
							float num6 = slot2.b2;
							if (pmaVertexColors)
							{
								float num7 = a * slot2.a * color2.a;
								num4 *= num7;
								num5 *= num7;
								num6 *= num7;
							}
							this.AddAttachmentTintBlack(num4, num5, num6, num2);
						}
						int count = this.vertexBuffer.Count;
						int num8 = count + num2;
						int num9 = this.vertexBuffer.Items.Length;
						if (num8 > num9)
						{
							int num10 = (int)((float)num9 * 1.3f);
							if (num10 < num8)
							{
								num10 = num8;
							}
							Array.Resize<Vector3>(ref this.vertexBuffer.Items, num10);
							Array.Resize<Vector2>(ref this.uvBuffer.Items, num10);
							Array.Resize<Color32>(ref this.colorBuffer.Items, num10);
						}
						this.vertexBuffer.Count = (this.uvBuffer.Count = (this.colorBuffer.Count = num8));
						Vector3[] items2 = this.vertexBuffer.Items;
						Vector2[] items3 = this.uvBuffer.Items;
						Color32[] items4 = this.colorBuffer.Items;
						if (count == 0)
						{
							for (int j = 0; j < num2; j++)
							{
								int num11 = count + j;
								int num12 = j << 1;
								float num13 = array[num12];
								float num14 = array[num12 + 1];
								items2[num11].x = num13;
								items2[num11].y = num14;
								items2[num11].z = z;
								items3[num11].x = array2[num12];
								items3[num11].y = array2[num12 + 1];
								items4[num11] = color;
								if (num13 < vector.x)
								{
									vector.x = num13;
								}
								if (num13 > vector2.x)
								{
									vector2.x = num13;
								}
								if (num14 < vector.y)
								{
									vector.y = num14;
								}
								if (num14 > vector2.y)
								{
									vector2.y = num14;
								}
							}
						}
						else
						{
							for (int k = 0; k < num2; k++)
							{
								int num15 = count + k;
								int num16 = k << 1;
								float num17 = array[num16];
								float num18 = array[num16 + 1];
								items2[num15].x = num17;
								items2[num15].y = num18;
								items2[num15].z = z;
								items3[num15].x = array2[num16];
								items3[num15].y = array2[num16 + 1];
								items4[num15] = color;
								if (num17 < vector.x)
								{
									vector.x = num17;
								}
								else if (num17 > vector2.x)
								{
									vector2.x = num17;
								}
								if (num18 < vector.y)
								{
									vector.y = num18;
								}
								else if (num18 > vector2.y)
								{
									vector2.y = num18;
								}
							}
						}
						if (updateTriangles)
						{
							int count2 = exposedList.Count;
							int num19 = count2 + num3;
							if (num19 > exposedList.Items.Length)
							{
								Array.Resize<int>(ref exposedList.Items, num19);
							}
							exposedList.Count = num19;
							int[] items5 = exposedList.Items;
							for (int l = 0; l < num3; l++)
							{
								items5[count2 + l] = array3[l] + count;
							}
						}
					}
					this.clipper.ClipEnd(slot2);
				}
				IL_85A:;
			}
			this.clipper.ClipEnd();
			this.meshBoundsMin = vector;
			this.meshBoundsMax = vector2;
			this.meshBoundsThickness = (float)instruction.endSlot * zSpacing;
			int[] items6 = exposedList.Items;
			int m = exposedList.Count;
			int num20 = items6.Length;
			while (m < num20)
			{
				items6[m] = 0;
				m++;
			}
			this.submeshIndex++;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00077638 File Offset: 0x00075838
		public void BuildMesh(SkeletonRendererInstruction instruction, bool updateTriangles)
		{
			SubmeshInstruction[] items = instruction.submeshInstructions.Items;
			int i = 0;
			int count = instruction.submeshInstructions.Count;
			while (i < count)
			{
				this.AddSubmesh(items[i], updateTriangles);
				i++;
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00077684 File Offset: 0x00075884
		public void BuildMeshWithArrays(SkeletonRendererInstruction instruction, bool updateTriangles)
		{
			MeshGenerator.Settings settings = this.settings;
			int rawVertexCount = instruction.rawVertexCount;
			if (rawVertexCount > this.vertexBuffer.Items.Length)
			{
				Array.Resize<Vector3>(ref this.vertexBuffer.Items, rawVertexCount);
				Array.Resize<Vector2>(ref this.uvBuffer.Items, rawVertexCount);
				Array.Resize<Color32>(ref this.colorBuffer.Items, rawVertexCount);
			}
			this.vertexBuffer.Count = (this.uvBuffer.Count = (this.colorBuffer.Count = rawVertexCount));
			Color32 color = default(Color32);
			int num = 0;
			float[] array = this.tempVerts;
			Vector2 vector = this.meshBoundsMin;
			Vector2 vector2 = this.meshBoundsMax;
			Vector3[] items = this.vertexBuffer.Items;
			Vector2[] items2 = this.uvBuffer.Items;
			Color32[] items3 = this.colorBuffer.Items;
			int num2 = 0;
			int i = 0;
			int count = instruction.submeshInstructions.Count;
			while (i < count)
			{
				SubmeshInstruction submeshInstruction = instruction.submeshInstructions.Items[i];
				Skeleton skeleton = submeshInstruction.skeleton;
				Slot[] items4 = skeleton.drawOrder.Items;
				float a = skeleton.a;
				float r = skeleton.r;
				float g = skeleton.g;
				float b = skeleton.b;
				int endSlot = submeshInstruction.endSlot;
				int startSlot = submeshInstruction.startSlot;
				num2 = endSlot;
				if (settings.tintBlack)
				{
					int num3 = num;
					Vector2 vector3;
					vector3.y = 1f;
					if (this.uv2 == null)
					{
						this.uv2 = new ExposedList<Vector2>();
						this.uv3 = new ExposedList<Vector2>();
					}
					if (rawVertexCount > this.uv2.Items.Length)
					{
						Array.Resize<Vector2>(ref this.uv2.Items, rawVertexCount);
						Array.Resize<Vector2>(ref this.uv3.Items, rawVertexCount);
					}
					this.uv2.Count = (this.uv3.Count = rawVertexCount);
					Vector2[] items5 = this.uv2.Items;
					Vector2[] items6 = this.uv3.Items;
					for (int j = startSlot; j < endSlot; j++)
					{
						Slot slot = items4[j];
						if (slot.bone.active)
						{
							Attachment attachment = slot.attachment;
							Vector2 vector4;
							vector4.x = slot.r2;
							vector4.y = slot.g2;
							vector3.x = slot.b2;
							RegionAttachment regionAttachment = attachment as RegionAttachment;
							if (regionAttachment != null)
							{
								if (settings.pmaVertexColors)
								{
									float num4 = a * slot.a * regionAttachment.a;
									vector4.x *= num4;
									vector4.y *= num4;
									vector3.x *= num4;
								}
								items5[num3] = vector4;
								items5[num3 + 1] = vector4;
								items5[num3 + 2] = vector4;
								items5[num3 + 3] = vector4;
								items6[num3] = vector3;
								items6[num3 + 1] = vector3;
								items6[num3 + 2] = vector3;
								items6[num3 + 3] = vector3;
								num3 += 4;
							}
							else
							{
								MeshAttachment meshAttachment = attachment as MeshAttachment;
								if (meshAttachment != null)
								{
									if (settings.pmaVertexColors)
									{
										float num5 = a * slot.a * meshAttachment.a;
										vector4.x *= num5;
										vector4.y *= num5;
										vector3.x *= num5;
									}
									int worldVerticesLength = meshAttachment.worldVerticesLength;
									for (int k = 0; k < worldVerticesLength; k += 2)
									{
										items5[num3] = vector4;
										items6[num3] = vector3;
										num3++;
									}
								}
							}
						}
					}
				}
				for (int l = startSlot; l < endSlot; l++)
				{
					Slot slot2 = items4[l];
					if (slot2.bone.active)
					{
						Attachment attachment2 = slot2.attachment;
						float z = (float)l * settings.zSpacing;
						RegionAttachment regionAttachment2 = attachment2 as RegionAttachment;
						if (regionAttachment2 != null)
						{
							regionAttachment2.ComputeWorldVertices(slot2.bone, array, 0, 2);
							float num6 = array[0];
							float num7 = array[1];
							float num8 = array[2];
							float num9 = array[3];
							float num10 = array[4];
							float num11 = array[5];
							float num12 = array[6];
							float num13 = array[7];
							items[num].x = num6;
							items[num].y = num7;
							items[num].z = z;
							items[num + 1].x = num12;
							items[num + 1].y = num13;
							items[num + 1].z = z;
							items[num + 2].x = num8;
							items[num + 2].y = num9;
							items[num + 2].z = z;
							items[num + 3].x = num10;
							items[num + 3].y = num11;
							items[num + 3].z = z;
							if (settings.pmaVertexColors)
							{
								color.a = (byte)(a * slot2.a * regionAttachment2.a * 255f);
								color.r = (byte)(r * slot2.r * regionAttachment2.r * (float)color.a);
								color.g = (byte)(g * slot2.g * regionAttachment2.g * (float)color.a);
								color.b = (byte)(b * slot2.b * regionAttachment2.b * (float)color.a);
								if (slot2.data.blendMode == BlendMode.Additive)
								{
									color.a = 0;
								}
							}
							else
							{
								color.a = (byte)(a * slot2.a * regionAttachment2.a * 255f);
								color.r = (byte)(r * slot2.r * regionAttachment2.r * 255f);
								color.g = (byte)(g * slot2.g * regionAttachment2.g * 255f);
								color.b = (byte)(b * slot2.b * regionAttachment2.b * 255f);
							}
							items3[num] = color;
							items3[num + 1] = color;
							items3[num + 2] = color;
							items3[num + 3] = color;
							float[] uvs = regionAttachment2.uvs;
							items2[num].x = uvs[0];
							items2[num].y = uvs[1];
							items2[num + 1].x = uvs[6];
							items2[num + 1].y = uvs[7];
							items2[num + 2].x = uvs[2];
							items2[num + 2].y = uvs[3];
							items2[num + 3].x = uvs[4];
							items2[num + 3].y = uvs[5];
							if (num6 < vector.x)
							{
								vector.x = num6;
							}
							if (num6 > vector2.x)
							{
								vector2.x = num6;
							}
							if (num8 < vector.x)
							{
								vector.x = num8;
							}
							else if (num8 > vector2.x)
							{
								vector2.x = num8;
							}
							if (num10 < vector.x)
							{
								vector.x = num10;
							}
							else if (num10 > vector2.x)
							{
								vector2.x = num10;
							}
							if (num12 < vector.x)
							{
								vector.x = num12;
							}
							else if (num12 > vector2.x)
							{
								vector2.x = num12;
							}
							if (num7 < vector.y)
							{
								vector.y = num7;
							}
							if (num7 > vector2.y)
							{
								vector2.y = num7;
							}
							if (num9 < vector.y)
							{
								vector.y = num9;
							}
							else if (num9 > vector2.y)
							{
								vector2.y = num9;
							}
							if (num11 < vector.y)
							{
								vector.y = num11;
							}
							else if (num11 > vector2.y)
							{
								vector2.y = num11;
							}
							if (num13 < vector.y)
							{
								vector.y = num13;
							}
							else if (num13 > vector2.y)
							{
								vector2.y = num13;
							}
							num += 4;
						}
						else
						{
							MeshAttachment meshAttachment2 = attachment2 as MeshAttachment;
							if (meshAttachment2 != null)
							{
								int worldVerticesLength2 = meshAttachment2.worldVerticesLength;
								if (array.Length < worldVerticesLength2)
								{
									array = (this.tempVerts = new float[worldVerticesLength2]);
								}
								meshAttachment2.ComputeWorldVertices(slot2, array);
								if (settings.pmaVertexColors)
								{
									color.a = (byte)(a * slot2.a * meshAttachment2.a * 255f);
									color.r = (byte)(r * slot2.r * meshAttachment2.r * (float)color.a);
									color.g = (byte)(g * slot2.g * meshAttachment2.g * (float)color.a);
									color.b = (byte)(b * slot2.b * meshAttachment2.b * (float)color.a);
									if (slot2.data.blendMode == BlendMode.Additive)
									{
										color.a = 0;
									}
								}
								else
								{
									color.a = (byte)(a * slot2.a * meshAttachment2.a * 255f);
									color.r = (byte)(r * slot2.r * meshAttachment2.r * 255f);
									color.g = (byte)(g * slot2.g * meshAttachment2.g * 255f);
									color.b = (byte)(b * slot2.b * meshAttachment2.b * 255f);
								}
								float[] uvs2 = meshAttachment2.uvs;
								if (num == 0)
								{
									float num14 = array[0];
									float num15 = array[1];
									if (num14 < vector.x)
									{
										vector.x = num14;
									}
									if (num14 > vector2.x)
									{
										vector2.x = num14;
									}
									if (num15 < vector.y)
									{
										vector.y = num15;
									}
									if (num15 > vector2.y)
									{
										vector2.y = num15;
									}
								}
								for (int m = 0; m < worldVerticesLength2; m += 2)
								{
									float num16 = array[m];
									float num17 = array[m + 1];
									items[num].x = num16;
									items[num].y = num17;
									items[num].z = z;
									items3[num] = color;
									items2[num].x = uvs2[m];
									items2[num].y = uvs2[m + 1];
									if (num16 < vector.x)
									{
										vector.x = num16;
									}
									else if (num16 > vector2.x)
									{
										vector2.x = num16;
									}
									if (num17 < vector.y)
									{
										vector.y = num17;
									}
									else if (num17 > vector2.y)
									{
										vector2.y = num17;
									}
									num++;
								}
							}
						}
					}
				}
				i++;
			}
			this.meshBoundsMin = vector;
			this.meshBoundsMax = vector2;
			this.meshBoundsThickness = (float)num2 * settings.zSpacing;
			int count2 = instruction.submeshInstructions.Count;
			this.submeshes.Count = count2;
			if (updateTriangles)
			{
				if (this.submeshes.Items.Length < count2)
				{
					this.submeshes.Resize(count2);
					int n = 0;
					int num18 = count2;
					while (n < num18)
					{
						ExposedList<int> exposedList = this.submeshes.Items[n];
						if (exposedList == null)
						{
							this.submeshes.Items[n] = new ExposedList<int>();
						}
						else
						{
							exposedList.Clear(false);
						}
						n++;
					}
				}
				SubmeshInstruction[] items7 = instruction.submeshInstructions.Items;
				int num19 = 0;
				for (int num20 = 0; num20 < count2; num20++)
				{
					SubmeshInstruction submeshInstruction2 = items7[num20];
					ExposedList<int> exposedList2 = this.submeshes.Items[num20];
					int rawTriangleCount = submeshInstruction2.rawTriangleCount;
					if (rawTriangleCount > exposedList2.Items.Length)
					{
						Array.Resize<int>(ref exposedList2.Items, rawTriangleCount);
					}
					else if (rawTriangleCount < exposedList2.Items.Length)
					{
						int[] items8 = exposedList2.Items;
						int num21 = rawTriangleCount;
						int num22 = items8.Length;
						while (num21 < num22)
						{
							items8[num21] = 0;
							num21++;
						}
					}
					exposedList2.Count = rawTriangleCount;
					int[] items9 = exposedList2.Items;
					int num23 = 0;
					Skeleton skeleton2 = submeshInstruction2.skeleton;
					Slot[] items10 = skeleton2.drawOrder.Items;
					int num24 = submeshInstruction2.startSlot;
					int endSlot2 = submeshInstruction2.endSlot;
					while (num24 < endSlot2)
					{
						Slot slot3 = items10[num24];
						if (slot3.bone.active)
						{
							Attachment attachment3 = items10[num24].attachment;
							if (attachment3 is RegionAttachment)
							{
								items9[num23] = num19;
								items9[num23 + 1] = num19 + 2;
								items9[num23 + 2] = num19 + 1;
								items9[num23 + 3] = num19 + 2;
								items9[num23 + 4] = num19 + 3;
								items9[num23 + 5] = num19 + 1;
								num23 += 6;
								num19 += 4;
							}
							else
							{
								MeshAttachment meshAttachment3 = attachment3 as MeshAttachment;
								if (meshAttachment3 != null)
								{
									int[] triangles = meshAttachment3.triangles;
									int num25 = 0;
									int num26 = triangles.Length;
									while (num25 < num26)
									{
										items9[num23] = num19 + triangles[num25];
										num25++;
										num23++;
									}
									num19 += meshAttachment3.worldVerticesLength >> 1;
								}
							}
						}
						num24++;
					}
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00078514 File Offset: 0x00076714
		public void ScaleVertexData(float scale)
		{
			Vector3[] items = this.vertexBuffer.Items;
			int i = 0;
			int count = this.vertexBuffer.Count;
			while (i < count)
			{
				items[i] *= scale;
				i++;
			}
			this.meshBoundsMin *= scale;
			this.meshBoundsMax *= scale;
			this.meshBoundsThickness *= scale;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00078598 File Offset: 0x00076798
		private void AddAttachmentTintBlack(float r2, float g2, float b2, int vertexCount)
		{
			Vector2 vector = new Vector2(r2, g2);
			Vector2 vector2 = new Vector2(b2, 1f);
			int count = this.vertexBuffer.Count;
			int num = count + vertexCount;
			if (this.uv2 == null)
			{
				this.uv2 = new ExposedList<Vector2>();
				this.uv3 = new ExposedList<Vector2>();
			}
			if (num > this.uv2.Items.Length)
			{
				Array.Resize<Vector2>(ref this.uv2.Items, num);
				Array.Resize<Vector2>(ref this.uv3.Items, num);
			}
			this.uv2.Count = (this.uv3.Count = num);
			Vector2[] items = this.uv2.Items;
			Vector2[] items2 = this.uv3.Items;
			for (int i = 0; i < vertexCount; i++)
			{
				items[count + i] = vector;
				items2[count + i] = vector2;
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00078694 File Offset: 0x00076894
		public void FillVertexData(Mesh mesh)
		{
			Vector3[] items = this.vertexBuffer.Items;
			Vector2[] items2 = this.uvBuffer.Items;
			Color32[] items3 = this.colorBuffer.Items;
			int num = items.Length;
			int count = this.vertexBuffer.Count;
			Vector3 zero = Vector3.zero;
			for (int i = count; i < num; i++)
			{
				items[i] = zero;
			}
			mesh.vertices = items;
			mesh.uv = items2;
			mesh.colors32 = items3;
			if (float.IsInfinity(this.meshBoundsMin.x))
			{
				mesh.bounds = default(Bounds);
			}
			else
			{
				float num2 = (this.meshBoundsMax.x - this.meshBoundsMin.x) * 0.5f;
				float num3 = (this.meshBoundsMax.y - this.meshBoundsMin.y) * 0.5f;
				mesh.bounds = new Bounds
				{
					center = new Vector3(this.meshBoundsMin.x + num2, this.meshBoundsMin.y + num3),
					extents = new Vector3(num2, num3, this.meshBoundsThickness * 0.5f)
				};
			}
			if (this.settings.addNormals)
			{
				int num4 = 0;
				if (this.normals == null)
				{
					this.normals = new Vector3[num];
				}
				else
				{
					num4 = this.normals.Length;
				}
				if (num4 != num)
				{
					Array.Resize<Vector3>(ref this.normals, num);
					Vector3[] array = this.normals;
					for (int j = num4; j < num; j++)
					{
						array[j] = Vector3.back;
					}
				}
				mesh.normals = this.normals;
			}
			if (this.settings.tintBlack && this.uv2 != null)
			{
				if (num != this.uv2.Items.Length)
				{
					Array.Resize<Vector2>(ref this.uv2.Items, num);
					Array.Resize<Vector2>(ref this.uv3.Items, num);
					this.uv2.Count = (this.uv3.Count = num);
				}
				mesh.uv2 = this.uv2.Items;
				mesh.uv3 = this.uv3.Items;
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000788F0 File Offset: 0x00076AF0
		public void FillLateVertexData(Mesh mesh)
		{
			if (this.settings.calculateTangents)
			{
				int count = this.vertexBuffer.Count;
				ExposedList<int>[] items = this.submeshes.Items;
				int count2 = this.submeshes.Count;
				Vector3[] items2 = this.vertexBuffer.Items;
				Vector2[] items3 = this.uvBuffer.Items;
				MeshGenerator.SolveTangents2DEnsureSize(ref this.tangents, ref this.tempTanBuffer, count, items2.Length);
				for (int i = 0; i < count2; i++)
				{
					int[] items4 = items[i].Items;
					int count3 = items[i].Count;
					MeshGenerator.SolveTangents2DTriangles(this.tempTanBuffer, items4, count3, items2, items3, count);
				}
				MeshGenerator.SolveTangents2DBuffer(this.tangents, this.tempTanBuffer, count);
				mesh.tangents = this.tangents;
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000789BC File Offset: 0x00076BBC
		public void FillTriangles(Mesh mesh)
		{
			int count = this.submeshes.Count;
			ExposedList<int>[] items = this.submeshes.Items;
			mesh.subMeshCount = count;
			for (int i = 0; i < count; i++)
			{
				mesh.SetTriangles(items[i].Items, i, false);
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00078A0C File Offset: 0x00076C0C
		public void EnsureVertexCapacity(int minimumVertexCount, bool inlcudeTintBlack = false, bool includeTangents = false, bool includeNormals = false)
		{
			if (minimumVertexCount > this.vertexBuffer.Items.Length)
			{
				Array.Resize<Vector3>(ref this.vertexBuffer.Items, minimumVertexCount);
				Array.Resize<Vector2>(ref this.uvBuffer.Items, minimumVertexCount);
				Array.Resize<Color32>(ref this.colorBuffer.Items, minimumVertexCount);
				if (inlcudeTintBlack)
				{
					if (this.uv2 == null)
					{
						this.uv2 = new ExposedList<Vector2>(minimumVertexCount);
						this.uv3 = new ExposedList<Vector2>(minimumVertexCount);
					}
					this.uv2.Resize(minimumVertexCount);
					this.uv3.Resize(minimumVertexCount);
				}
				if (includeNormals)
				{
					if (this.normals == null)
					{
						this.normals = new Vector3[minimumVertexCount];
					}
					else
					{
						Array.Resize<Vector3>(ref this.normals, minimumVertexCount);
					}
				}
				if (includeTangents)
				{
					if (this.tangents == null)
					{
						this.tangents = new Vector4[minimumVertexCount];
					}
					else
					{
						Array.Resize<Vector4>(ref this.tangents, minimumVertexCount);
					}
				}
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00078B00 File Offset: 0x00076D00
		public void TrimExcess()
		{
			this.vertexBuffer.TrimExcess();
			this.uvBuffer.TrimExcess();
			this.colorBuffer.TrimExcess();
			if (this.uv2 != null)
			{
				this.uv2.TrimExcess();
			}
			if (this.uv3 != null)
			{
				this.uv3.TrimExcess();
			}
			int newSize = this.vertexBuffer.Items.Length;
			if (this.normals != null)
			{
				Array.Resize<Vector3>(ref this.normals, newSize);
			}
			if (this.tangents != null)
			{
				Array.Resize<Vector4>(ref this.tangents, newSize);
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00078B98 File Offset: 0x00076D98
		internal static void SolveTangents2DEnsureSize(ref Vector4[] tangentBuffer, ref Vector2[] tempTanBuffer, int vertexCount, int vertexBufferLength)
		{
			if (tangentBuffer == null || tangentBuffer.Length != vertexBufferLength)
			{
				tangentBuffer = new Vector4[vertexBufferLength];
			}
			if (tempTanBuffer == null || tempTanBuffer.Length < vertexCount * 2)
			{
				tempTanBuffer = new Vector2[vertexCount * 2];
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00078BDC File Offset: 0x00076DDC
		internal static void SolveTangents2DTriangles(Vector2[] tempTanBuffer, int[] triangles, int triangleCount, Vector3[] vertices, Vector2[] uvs, int vertexCount)
		{
			for (int i = 0; i < triangleCount; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				Vector3 vector = vertices[num];
				Vector3 vector2 = vertices[num2];
				Vector3 vector3 = vertices[num3];
				Vector2 vector4 = uvs[num];
				Vector2 vector5 = uvs[num2];
				Vector2 vector6 = uvs[num3];
				float num4 = vector2.x - vector.x;
				float num5 = vector3.x - vector.x;
				float num6 = vector2.y - vector.y;
				float num7 = vector3.y - vector.y;
				float num8 = vector5.x - vector4.x;
				float num9 = vector6.x - vector4.x;
				float num10 = vector5.y - vector4.y;
				float num11 = vector6.y - vector4.y;
				float num12 = num8 * num11 - num9 * num10;
				float num13 = (num12 != 0f) ? (1f / num12) : 0f;
				Vector2 vector7;
				vector7.x = (num11 * num4 - num10 * num5) * num13;
				vector7.y = (num11 * num6 - num10 * num7) * num13;
				tempTanBuffer[num] = (tempTanBuffer[num2] = (tempTanBuffer[num3] = vector7));
				Vector2 vector8;
				vector8.x = (num8 * num5 - num9 * num4) * num13;
				vector8.y = (num8 * num7 - num9 * num6) * num13;
				tempTanBuffer[vertexCount + num] = (tempTanBuffer[vertexCount + num2] = (tempTanBuffer[vertexCount + num3] = vector8));
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00078DDC File Offset: 0x00076FDC
		internal static void SolveTangents2DBuffer(Vector4[] tangents, Vector2[] tempTanBuffer, int vertexCount)
		{
			Vector4 vector;
			vector.z = 0f;
			for (int i = 0; i < vertexCount; i++)
			{
				Vector2 vector2 = tempTanBuffer[i];
				float num = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
				if ((double)num > 1E-05)
				{
					float num2 = 1f / num;
					vector2.x *= num2;
					vector2.y *= num2;
				}
				Vector2 vector3 = tempTanBuffer[vertexCount + i];
				vector.x = vector2.x;
				vector.y = vector2.y;
				vector.w = (float)((vector2.y * vector3.x <= vector2.x * vector3.y) ? -1 : 1);
				tangents[i] = vector;
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00078EE0 File Offset: 0x000770E0
		public static void FillMeshLocal(Mesh mesh, RegionAttachment regionAttachment)
		{
			if (mesh == null)
			{
				return;
			}
			if (regionAttachment == null)
			{
				return;
			}
			MeshGenerator.AttachmentVerts.Clear();
			float[] offset = regionAttachment.Offset;
			MeshGenerator.AttachmentVerts.Add(new Vector3(offset[0], offset[1]));
			MeshGenerator.AttachmentVerts.Add(new Vector3(offset[2], offset[3]));
			MeshGenerator.AttachmentVerts.Add(new Vector3(offset[4], offset[5]));
			MeshGenerator.AttachmentVerts.Add(new Vector3(offset[6], offset[7]));
			MeshGenerator.AttachmentUVs.Clear();
			float[] uvs = regionAttachment.UVs;
			MeshGenerator.AttachmentUVs.Add(new Vector2(uvs[2], uvs[3]));
			MeshGenerator.AttachmentUVs.Add(new Vector2(uvs[4], uvs[5]));
			MeshGenerator.AttachmentUVs.Add(new Vector2(uvs[6], uvs[7]));
			MeshGenerator.AttachmentUVs.Add(new Vector2(uvs[0], uvs[1]));
			MeshGenerator.AttachmentColors32.Clear();
			Color32 item = new Color(regionAttachment.r, regionAttachment.g, regionAttachment.b, regionAttachment.a);
			for (int i = 0; i < 4; i++)
			{
				MeshGenerator.AttachmentColors32.Add(item);
			}
			MeshGenerator.AttachmentIndices.Clear();
			MeshGenerator.AttachmentIndices.AddRange(new int[]
			{
				0,
				2,
				1,
				0,
				3,
				2
			});
			mesh.Clear();
			mesh.name = regionAttachment.Name;
			mesh.SetVertices(MeshGenerator.AttachmentVerts);
			mesh.SetUVs(0, MeshGenerator.AttachmentUVs);
			mesh.SetColors(MeshGenerator.AttachmentColors32);
			mesh.SetTriangles(MeshGenerator.AttachmentIndices, 0);
			mesh.RecalculateBounds();
			MeshGenerator.AttachmentVerts.Clear();
			MeshGenerator.AttachmentUVs.Clear();
			MeshGenerator.AttachmentColors32.Clear();
			MeshGenerator.AttachmentIndices.Clear();
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000790A8 File Offset: 0x000772A8
		public static void FillMeshLocal(Mesh mesh, MeshAttachment meshAttachment, SkeletonData skeletonData)
		{
			if (mesh == null)
			{
				return;
			}
			if (meshAttachment == null)
			{
				return;
			}
			int num = meshAttachment.WorldVerticesLength / 2;
			MeshGenerator.AttachmentVerts.Clear();
			if (meshAttachment.IsWeighted())
			{
				int worldVerticesLength = meshAttachment.WorldVerticesLength;
				int[] bones = meshAttachment.bones;
				int i = 0;
				float[] vertices = meshAttachment.vertices;
				int j = 0;
				int num2 = 0;
				while (j < worldVerticesLength)
				{
					float num3 = 0f;
					float num4 = 0f;
					int num5 = bones[i++];
					num5 += i;
					while (i < num5)
					{
						BoneMatrix boneMatrix = BoneMatrix.CalculateSetupWorld(skeletonData.bones.Items[bones[i]]);
						float num6 = vertices[num2];
						float num7 = vertices[num2 + 1];
						float num8 = vertices[num2 + 2];
						num3 += (num6 * boneMatrix.a + num7 * boneMatrix.b + boneMatrix.x) * num8;
						num4 += (num6 * boneMatrix.c + num7 * boneMatrix.d + boneMatrix.y) * num8;
						i++;
						num2 += 3;
					}
					MeshGenerator.AttachmentVerts.Add(new Vector3(num3, num4));
					j += 2;
				}
			}
			else
			{
				float[] vertices2 = meshAttachment.Vertices;
				Vector3 item = default(Vector3);
				for (int k = 0; k < num; k++)
				{
					int num9 = k * 2;
					item.x = vertices2[num9];
					item.y = vertices2[num9 + 1];
					MeshGenerator.AttachmentVerts.Add(item);
				}
			}
			float[] uvs = meshAttachment.uvs;
			Vector2 item2 = default(Vector2);
			Color32 item3 = new Color(meshAttachment.r, meshAttachment.g, meshAttachment.b, meshAttachment.a);
			MeshGenerator.AttachmentUVs.Clear();
			MeshGenerator.AttachmentColors32.Clear();
			for (int l = 0; l < num; l++)
			{
				int num10 = l * 2;
				item2.x = uvs[num10];
				item2.y = uvs[num10 + 1];
				MeshGenerator.AttachmentUVs.Add(item2);
				MeshGenerator.AttachmentColors32.Add(item3);
			}
			MeshGenerator.AttachmentIndices.Clear();
			MeshGenerator.AttachmentIndices.AddRange(meshAttachment.triangles);
			mesh.Clear();
			mesh.name = meshAttachment.Name;
			mesh.SetVertices(MeshGenerator.AttachmentVerts);
			mesh.SetUVs(0, MeshGenerator.AttachmentUVs);
			mesh.SetColors(MeshGenerator.AttachmentColors32);
			mesh.SetTriangles(MeshGenerator.AttachmentIndices, 0);
			mesh.RecalculateBounds();
			MeshGenerator.AttachmentVerts.Clear();
			MeshGenerator.AttachmentUVs.Clear();
			MeshGenerator.AttachmentColors32.Clear();
			MeshGenerator.AttachmentIndices.Clear();
		}

		// Token: 0x04000DE8 RID: 3560
		private const float BoundsMinDefault = float.PositiveInfinity;

		// Token: 0x04000DE9 RID: 3561
		private const float BoundsMaxDefault = float.NegativeInfinity;

		// Token: 0x04000DEA RID: 3562
		public MeshGenerator.Settings settings = MeshGenerator.Settings.Default;

		// Token: 0x04000DEB RID: 3563
		[NonSerialized]
		private readonly ExposedList<Vector3> vertexBuffer = new ExposedList<Vector3>(4);

		// Token: 0x04000DEC RID: 3564
		[NonSerialized]
		private readonly ExposedList<Vector2> uvBuffer = new ExposedList<Vector2>(4);

		// Token: 0x04000DED RID: 3565
		[NonSerialized]
		private readonly ExposedList<Color32> colorBuffer = new ExposedList<Color32>(4);

		// Token: 0x04000DEE RID: 3566
		[NonSerialized]
		private readonly ExposedList<ExposedList<int>> submeshes = new ExposedList<ExposedList<int>>
		{
			new ExposedList<int>(6)
		};

		// Token: 0x04000DEF RID: 3567
		[NonSerialized]
		private Vector2 meshBoundsMin;

		// Token: 0x04000DF0 RID: 3568
		[NonSerialized]
		private Vector2 meshBoundsMax;

		// Token: 0x04000DF1 RID: 3569
		[NonSerialized]
		private float meshBoundsThickness;

		// Token: 0x04000DF2 RID: 3570
		[NonSerialized]
		private int submeshIndex;

		// Token: 0x04000DF3 RID: 3571
		[NonSerialized]
		private SkeletonClipping clipper = new SkeletonClipping();

		// Token: 0x04000DF4 RID: 3572
		[NonSerialized]
		private float[] tempVerts = new float[8];

		// Token: 0x04000DF5 RID: 3573
		[NonSerialized]
		private int[] regionTriangles = new int[]
		{
			0,
			1,
			2,
			2,
			3,
			0
		};

		// Token: 0x04000DF6 RID: 3574
		[NonSerialized]
		private Vector3[] normals;

		// Token: 0x04000DF7 RID: 3575
		[NonSerialized]
		private Vector4[] tangents;

		// Token: 0x04000DF8 RID: 3576
		[NonSerialized]
		private Vector2[] tempTanBuffer;

		// Token: 0x04000DF9 RID: 3577
		[NonSerialized]
		private ExposedList<Vector2> uv2;

		// Token: 0x04000DFA RID: 3578
		[NonSerialized]
		private ExposedList<Vector2> uv3;

		// Token: 0x04000DFB RID: 3579
		private static List<Vector3> AttachmentVerts = new List<Vector3>();

		// Token: 0x04000DFC RID: 3580
		private static List<Vector2> AttachmentUVs = new List<Vector2>();

		// Token: 0x04000DFD RID: 3581
		private static List<Color32> AttachmentColors32 = new List<Color32>();

		// Token: 0x04000DFE RID: 3582
		private static List<int> AttachmentIndices = new List<int>();

		// Token: 0x02000211 RID: 529
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700031E RID: 798
			// (get) Token: 0x0600110B RID: 4363 RVA: 0x00079358 File Offset: 0x00077558
			public static MeshGenerator.Settings Default
			{
				get
				{
					return new MeshGenerator.Settings
					{
						pmaVertexColors = true,
						zSpacing = 0f,
						useClipping = true,
						tintBlack = false,
						calculateTangents = false,
						addNormals = false,
						immutableTriangles = false
					};
				}
			}

			// Token: 0x04000DFF RID: 3583
			public bool useClipping;

			// Token: 0x04000E00 RID: 3584
			[Range(-0.1f, 0f)]
			[Space]
			public float zSpacing;

			// Token: 0x04000E01 RID: 3585
			[Space]
			[Header("Vertex Data")]
			public bool pmaVertexColors;

			// Token: 0x04000E02 RID: 3586
			public bool tintBlack;

			// Token: 0x04000E03 RID: 3587
			public bool calculateTangents;

			// Token: 0x04000E04 RID: 3588
			public bool addNormals;

			// Token: 0x04000E05 RID: 3589
			public bool immutableTriangles;
		}
	}
}
