using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spine
{
	// Token: 0x020001C0 RID: 448
	public class SkeletonBinary
	{
		// Token: 0x06000E20 RID: 3616 RVA: 0x000638A0 File Offset: 0x00061AA0
		public SkeletonBinary(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000638B0 File Offset: 0x00061AB0
		public SkeletonBinary(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader");
			}
			this.attachmentLoader = attachmentLoader;
			this.Scale = 1f;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00063914 File Offset: 0x00061B14
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x0006391C File Offset: 0x00061B1C
		public float Scale { get; set; }

		// Token: 0x06000E25 RID: 3621 RVA: 0x00063928 File Offset: 0x00061B28
		public SkeletonData ReadSkeletonData(string path)
		{
			SkeletonData result;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				SkeletonData skeletonData = this.ReadSkeletonData(fileStream);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				result = skeletonData;
			}
			return result;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0006398C File Offset: 0x00061B8C
		public static string GetVersionString(Stream file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			SkeletonBinary.SkeletonInput skeletonInput = new SkeletonBinary.SkeletonInput(file);
			return skeletonInput.GetVersionString();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000639B8 File Offset: 0x00061BB8
		public SkeletonData ReadSkeletonData(Stream file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			float scale = this.Scale;
			SkeletonData skeletonData = new SkeletonData();
			SkeletonBinary.SkeletonInput skeletonInput = new SkeletonBinary.SkeletonInput(file);
			skeletonData.hash = skeletonInput.ReadString();
			if (skeletonData.hash.Length == 0)
			{
				skeletonData.hash = null;
			}
			skeletonData.version = skeletonInput.ReadString();
			if (skeletonData.version.Length == 0)
			{
				skeletonData.version = null;
			}
			if ("3.8.75" == skeletonData.version)
			{
				throw new Exception("Unsupported skeleton data, please export with a newer version of Spine.");
			}
			skeletonData.x = skeletonInput.ReadFloat();
			skeletonData.y = skeletonInput.ReadFloat();
			skeletonData.width = skeletonInput.ReadFloat();
			skeletonData.height = skeletonInput.ReadFloat();
			bool flag = skeletonInput.ReadBoolean();
			if (flag)
			{
				skeletonData.fps = skeletonInput.ReadFloat();
				skeletonData.imagesPath = skeletonInput.ReadString();
				if (string.IsNullOrEmpty(skeletonData.imagesPath))
				{
					skeletonData.imagesPath = null;
				}
				skeletonData.audioPath = skeletonInput.ReadString();
				if (string.IsNullOrEmpty(skeletonData.audioPath))
				{
					skeletonData.audioPath = null;
				}
			}
			int num;
			skeletonInput.strings = new ExposedList<string>(num = skeletonInput.ReadInt(true));
			object[] items = skeletonInput.strings.Resize(num).Items;
			for (int i = 0; i < num; i++)
			{
				items[i] = skeletonInput.ReadString();
			}
			items = skeletonData.bones.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int j = 0; j < num; j++)
			{
				string name = skeletonInput.ReadString();
				BoneData parent = (j != 0) ? skeletonData.bones.Items[skeletonInput.ReadInt(true)] : null;
				BoneData boneData = new BoneData(j, name, parent);
				boneData.rotation = skeletonInput.ReadFloat();
				boneData.x = skeletonInput.ReadFloat() * scale;
				boneData.y = skeletonInput.ReadFloat() * scale;
				boneData.scaleX = skeletonInput.ReadFloat();
				boneData.scaleY = skeletonInput.ReadFloat();
				boneData.shearX = skeletonInput.ReadFloat();
				boneData.shearY = skeletonInput.ReadFloat();
				boneData.length = skeletonInput.ReadFloat() * scale;
				boneData.transformMode = SkeletonBinary.TransformModeValues[skeletonInput.ReadInt(true)];
				boneData.skinRequired = skeletonInput.ReadBoolean();
				if (flag)
				{
					skeletonInput.ReadInt();
				}
				items[j] = boneData;
			}
			items = skeletonData.slots.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int k = 0; k < num; k++)
			{
				string name2 = skeletonInput.ReadString();
				BoneData boneData2 = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				SlotData slotData = new SlotData(k, name2, boneData2);
				int num2 = skeletonInput.ReadInt();
				slotData.r = (float)(((long)num2 & (long)((ulong)-16777216)) >> 24) / 255f;
				slotData.g = (float)((num2 & 16711680) >> 16) / 255f;
				slotData.b = (float)((num2 & 65280) >> 8) / 255f;
				slotData.a = (float)(num2 & 255) / 255f;
				int num3 = skeletonInput.ReadInt();
				if (num3 != -1)
				{
					slotData.hasSecondColor = true;
					slotData.r2 = (float)((num3 & 16711680) >> 16) / 255f;
					slotData.g2 = (float)((num3 & 65280) >> 8) / 255f;
					slotData.b2 = (float)(num3 & 255) / 255f;
				}
				slotData.attachmentName = skeletonInput.ReadStringRef();
				slotData.blendMode = (BlendMode)skeletonInput.ReadInt(true);
				items[k] = slotData;
			}
			items = skeletonData.ikConstraints.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int l = 0; l < num; l++)
			{
				IkConstraintData ikConstraintData = new IkConstraintData(skeletonInput.ReadString());
				ikConstraintData.order = skeletonInput.ReadInt(true);
				ikConstraintData.skinRequired = skeletonInput.ReadBoolean();
				int num4;
				object[] items2 = ikConstraintData.bones.Resize(num4 = skeletonInput.ReadInt(true)).Items;
				for (int m = 0; m < num4; m++)
				{
					items2[m] = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				}
				ikConstraintData.target = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				ikConstraintData.mix = skeletonInput.ReadFloat();
				ikConstraintData.softness = skeletonInput.ReadFloat() * scale;
				ikConstraintData.bendDirection = (int)skeletonInput.ReadSByte();
				ikConstraintData.compress = skeletonInput.ReadBoolean();
				ikConstraintData.stretch = skeletonInput.ReadBoolean();
				ikConstraintData.uniform = skeletonInput.ReadBoolean();
				items[l] = ikConstraintData;
			}
			items = skeletonData.transformConstraints.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int n = 0; n < num; n++)
			{
				TransformConstraintData transformConstraintData = new TransformConstraintData(skeletonInput.ReadString());
				transformConstraintData.order = skeletonInput.ReadInt(true);
				transformConstraintData.skinRequired = skeletonInput.ReadBoolean();
				int num5;
				object[] items3 = transformConstraintData.bones.Resize(num5 = skeletonInput.ReadInt(true)).Items;
				for (int num6 = 0; num6 < num5; num6++)
				{
					items3[num6] = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				}
				transformConstraintData.target = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				transformConstraintData.local = skeletonInput.ReadBoolean();
				transformConstraintData.relative = skeletonInput.ReadBoolean();
				transformConstraintData.offsetRotation = skeletonInput.ReadFloat();
				transformConstraintData.offsetX = skeletonInput.ReadFloat() * scale;
				transformConstraintData.offsetY = skeletonInput.ReadFloat() * scale;
				transformConstraintData.offsetScaleX = skeletonInput.ReadFloat();
				transformConstraintData.offsetScaleY = skeletonInput.ReadFloat();
				transformConstraintData.offsetShearY = skeletonInput.ReadFloat();
				transformConstraintData.rotateMix = skeletonInput.ReadFloat();
				transformConstraintData.translateMix = skeletonInput.ReadFloat();
				transformConstraintData.scaleMix = skeletonInput.ReadFloat();
				transformConstraintData.shearMix = skeletonInput.ReadFloat();
				items[n] = transformConstraintData;
			}
			items = skeletonData.pathConstraints.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int num7 = 0; num7 < num; num7++)
			{
				PathConstraintData pathConstraintData = new PathConstraintData(skeletonInput.ReadString());
				pathConstraintData.order = skeletonInput.ReadInt(true);
				pathConstraintData.skinRequired = skeletonInput.ReadBoolean();
				int num8;
				object[] items4 = pathConstraintData.bones.Resize(num8 = skeletonInput.ReadInt(true)).Items;
				for (int num9 = 0; num9 < num8; num9++)
				{
					items4[num9] = skeletonData.bones.Items[skeletonInput.ReadInt(true)];
				}
				pathConstraintData.target = skeletonData.slots.Items[skeletonInput.ReadInt(true)];
				pathConstraintData.positionMode = (PositionMode)((int)Enum.GetValues(typeof(PositionMode)).GetValue(skeletonInput.ReadInt(true)));
				pathConstraintData.spacingMode = (SpacingMode)((int)Enum.GetValues(typeof(SpacingMode)).GetValue(skeletonInput.ReadInt(true)));
				pathConstraintData.rotateMode = (RotateMode)((int)Enum.GetValues(typeof(RotateMode)).GetValue(skeletonInput.ReadInt(true)));
				pathConstraintData.offsetRotation = skeletonInput.ReadFloat();
				pathConstraintData.position = skeletonInput.ReadFloat();
				if (pathConstraintData.positionMode == PositionMode.Fixed)
				{
					pathConstraintData.position *= scale;
				}
				pathConstraintData.spacing = skeletonInput.ReadFloat();
				if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
				{
					pathConstraintData.spacing *= scale;
				}
				pathConstraintData.rotateMix = skeletonInput.ReadFloat();
				pathConstraintData.translateMix = skeletonInput.ReadFloat();
				items[num7] = pathConstraintData;
			}
			Skin skin = this.ReadSkin(skeletonInput, skeletonData, true, flag);
			if (skin != null)
			{
				skeletonData.defaultSkin = skin;
				skeletonData.skins.Add(skin);
			}
			int num10 = skeletonData.skins.Count;
			items = skeletonData.skins.Resize(num = num10 + skeletonInput.ReadInt(true)).Items;
			while (num10 < num)
			{
				items[num10] = this.ReadSkin(skeletonInput, skeletonData, false, flag);
				num10++;
			}
			num = this.linkedMeshes.Count;
			for (int num11 = 0; num11 < num; num11++)
			{
				SkeletonJson.LinkedMesh linkedMesh = this.linkedMeshes[num11];
				Skin skin2 = (linkedMesh.skin != null) ? skeletonData.FindSkin(linkedMesh.skin) : skeletonData.DefaultSkin;
				if (skin2 == null)
				{
					throw new Exception("Skin not found: " + linkedMesh.skin);
				}
				Attachment attachment = skin2.GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
				if (attachment == null)
				{
					throw new Exception("Parent mesh not found: " + linkedMesh.parent);
				}
				linkedMesh.mesh.DeformAttachment = ((!linkedMesh.inheritDeform) ? linkedMesh.mesh : ((VertexAttachment)attachment));
				linkedMesh.mesh.ParentMesh = (MeshAttachment)attachment;
				linkedMesh.mesh.UpdateUVs();
			}
			this.linkedMeshes.Clear();
			items = skeletonData.events.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int num12 = 0; num12 < num; num12++)
			{
				EventData eventData = new EventData(skeletonInput.ReadStringRef());
				eventData.Int = skeletonInput.ReadInt(false);
				eventData.Float = skeletonInput.ReadFloat();
				eventData.String = skeletonInput.ReadString();
				eventData.AudioPath = skeletonInput.ReadString();
				if (eventData.AudioPath != null)
				{
					eventData.Volume = skeletonInput.ReadFloat();
					eventData.Balance = skeletonInput.ReadFloat();
				}
				items[num12] = eventData;
			}
			items = skeletonData.animations.Resize(num = skeletonInput.ReadInt(true)).Items;
			for (int num13 = 0; num13 < num; num13++)
			{
				items[num13] = this.ReadAnimation(skeletonInput.ReadString(), skeletonInput, skeletonData);
			}
			return skeletonData;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00064424 File Offset: 0x00062624
		private Skin ReadSkin(SkeletonBinary.SkeletonInput input, SkeletonData skeletonData, bool defaultSkin, bool nonessential)
		{
			int num;
			Skin skin;
			if (defaultSkin)
			{
				num = input.ReadInt(true);
				if (num == 0)
				{
					return null;
				}
				skin = new Skin("default");
			}
			else
			{
				skin = new Skin(input.ReadStringRef());
				object[] items = skin.bones.Resize(input.ReadInt(true)).Items;
				int i = 0;
				int count = skin.bones.Count;
				while (i < count)
				{
					items[i] = skeletonData.bones.Items[input.ReadInt(true)];
					i++;
				}
				int j = 0;
				int num2 = input.ReadInt(true);
				while (j < num2)
				{
					skin.constraints.Add(skeletonData.ikConstraints.Items[input.ReadInt(true)]);
					j++;
				}
				int k = 0;
				int num3 = input.ReadInt(true);
				while (k < num3)
				{
					skin.constraints.Add(skeletonData.transformConstraints.Items[input.ReadInt(true)]);
					k++;
				}
				int l = 0;
				int num4 = input.ReadInt(true);
				while (l < num4)
				{
					skin.constraints.Add(skeletonData.pathConstraints.Items[input.ReadInt(true)]);
					l++;
				}
				skin.constraints.TrimExcess();
				num = input.ReadInt(true);
			}
			for (int m = 0; m < num; m++)
			{
				int slotIndex = input.ReadInt(true);
				int n = 0;
				int num5 = input.ReadInt(true);
				while (n < num5)
				{
					string text = input.ReadStringRef();
					Attachment attachment = this.ReadAttachment(input, skeletonData, skin, slotIndex, text, nonessential);
					if (attachment != null)
					{
						skin.SetAttachment(slotIndex, text, attachment);
					}
					n++;
				}
			}
			return skin;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x000645EC File Offset: 0x000627EC
		private Attachment ReadAttachment(SkeletonBinary.SkeletonInput input, SkeletonData skeletonData, Skin skin, int slotIndex, string attachmentName, bool nonessential)
		{
			float scale = this.Scale;
			string text = input.ReadStringRef();
			if (text == null)
			{
				text = attachmentName;
			}
			switch (input.ReadByte())
			{
			case 0:
			{
				string text2 = input.ReadStringRef();
				float rotation = input.ReadFloat();
				float num = input.ReadFloat();
				float num2 = input.ReadFloat();
				float scaleX = input.ReadFloat();
				float scaleY = input.ReadFloat();
				float num3 = input.ReadFloat();
				float num4 = input.ReadFloat();
				int num5 = input.ReadInt();
				if (text2 == null)
				{
					text2 = text;
				}
				RegionAttachment regionAttachment = this.attachmentLoader.NewRegionAttachment(skin, text, text2);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = text2;
				regionAttachment.x = num * scale;
				regionAttachment.y = num2 * scale;
				regionAttachment.scaleX = scaleX;
				regionAttachment.scaleY = scaleY;
				regionAttachment.rotation = rotation;
				regionAttachment.width = num3 * scale;
				regionAttachment.height = num4 * scale;
				regionAttachment.r = (float)(((long)num5 & (long)((ulong)-16777216)) >> 24) / 255f;
				regionAttachment.g = (float)((num5 & 16711680) >> 16) / 255f;
				regionAttachment.b = (float)((num5 & 65280) >> 8) / 255f;
				regionAttachment.a = (float)(num5 & 255) / 255f;
				regionAttachment.UpdateOffset();
				return regionAttachment;
			}
			case 1:
			{
				int num6 = input.ReadInt(true);
				SkeletonBinary.Vertices vertices = this.ReadVertices(input, num6);
				if (nonessential)
				{
					input.ReadInt();
				}
				BoundingBoxAttachment boundingBoxAttachment = this.attachmentLoader.NewBoundingBoxAttachment(skin, text);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.worldVerticesLength = num6 << 1;
				boundingBoxAttachment.vertices = vertices.vertices;
				boundingBoxAttachment.bones = vertices.bones;
				return boundingBoxAttachment;
			}
			case 2:
			{
				string text3 = input.ReadStringRef();
				int num7 = input.ReadInt();
				int num8 = input.ReadInt(true);
				float[] regionUVs = this.ReadFloatArray(input, num8 << 1, 1f);
				int[] triangles = this.ReadShortArray(input);
				SkeletonBinary.Vertices vertices2 = this.ReadVertices(input, num8);
				int num9 = input.ReadInt(true);
				int[] edges = null;
				float num10 = 0f;
				float num11 = 0f;
				if (nonessential)
				{
					edges = this.ReadShortArray(input);
					num10 = input.ReadFloat();
					num11 = input.ReadFloat();
				}
				if (text3 == null)
				{
					text3 = text;
				}
				MeshAttachment meshAttachment = this.attachmentLoader.NewMeshAttachment(skin, text, text3);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = text3;
				meshAttachment.r = (float)(((long)num7 & (long)((ulong)-16777216)) >> 24) / 255f;
				meshAttachment.g = (float)((num7 & 16711680) >> 16) / 255f;
				meshAttachment.b = (float)((num7 & 65280) >> 8) / 255f;
				meshAttachment.a = (float)(num7 & 255) / 255f;
				meshAttachment.bones = vertices2.bones;
				meshAttachment.vertices = vertices2.vertices;
				meshAttachment.WorldVerticesLength = num8 << 1;
				meshAttachment.triangles = triangles;
				meshAttachment.regionUVs = regionUVs;
				meshAttachment.UpdateUVs();
				meshAttachment.HullLength = num9 << 1;
				if (nonessential)
				{
					meshAttachment.Edges = edges;
					meshAttachment.Width = num10 * scale;
					meshAttachment.Height = num11 * scale;
				}
				return meshAttachment;
			}
			case 3:
			{
				string text4 = input.ReadStringRef();
				int num12 = input.ReadInt();
				string skin2 = input.ReadStringRef();
				string parent = input.ReadStringRef();
				bool inheritDeform = input.ReadBoolean();
				float num13 = 0f;
				float num14 = 0f;
				if (nonessential)
				{
					num13 = input.ReadFloat();
					num14 = input.ReadFloat();
				}
				if (text4 == null)
				{
					text4 = text;
				}
				MeshAttachment meshAttachment2 = this.attachmentLoader.NewMeshAttachment(skin, text, text4);
				if (meshAttachment2 == null)
				{
					return null;
				}
				meshAttachment2.Path = text4;
				meshAttachment2.r = (float)(((long)num12 & (long)((ulong)-16777216)) >> 24) / 255f;
				meshAttachment2.g = (float)((num12 & 16711680) >> 16) / 255f;
				meshAttachment2.b = (float)((num12 & 65280) >> 8) / 255f;
				meshAttachment2.a = (float)(num12 & 255) / 255f;
				if (nonessential)
				{
					meshAttachment2.Width = num13 * scale;
					meshAttachment2.Height = num14 * scale;
				}
				this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(meshAttachment2, skin2, slotIndex, parent, inheritDeform));
				return meshAttachment2;
			}
			case 4:
			{
				bool closed = input.ReadBoolean();
				bool constantSpeed = input.ReadBoolean();
				int num15 = input.ReadInt(true);
				SkeletonBinary.Vertices vertices3 = this.ReadVertices(input, num15);
				float[] array = new float[num15 / 3];
				int i = 0;
				int num16 = array.Length;
				while (i < num16)
				{
					array[i] = input.ReadFloat() * scale;
					i++;
				}
				if (nonessential)
				{
					input.ReadInt();
				}
				PathAttachment pathAttachment = this.attachmentLoader.NewPathAttachment(skin, text);
				if (pathAttachment == null)
				{
					return null;
				}
				pathAttachment.closed = closed;
				pathAttachment.constantSpeed = constantSpeed;
				pathAttachment.worldVerticesLength = num15 << 1;
				pathAttachment.vertices = vertices3.vertices;
				pathAttachment.bones = vertices3.bones;
				pathAttachment.lengths = array;
				return pathAttachment;
			}
			case 5:
			{
				float rotation2 = input.ReadFloat();
				float num17 = input.ReadFloat();
				float num18 = input.ReadFloat();
				if (nonessential)
				{
					input.ReadInt();
				}
				PointAttachment pointAttachment = this.attachmentLoader.NewPointAttachment(skin, text);
				if (pointAttachment == null)
				{
					return null;
				}
				pointAttachment.x = num17 * scale;
				pointAttachment.y = num18 * scale;
				pointAttachment.rotation = rotation2;
				return pointAttachment;
			}
			case 6:
			{
				int num19 = input.ReadInt(true);
				int num20 = input.ReadInt(true);
				SkeletonBinary.Vertices vertices4 = this.ReadVertices(input, num20);
				if (nonessential)
				{
					input.ReadInt();
				}
				ClippingAttachment clippingAttachment = this.attachmentLoader.NewClippingAttachment(skin, text);
				if (clippingAttachment == null)
				{
					return null;
				}
				clippingAttachment.EndSlot = skeletonData.slots.Items[num19];
				clippingAttachment.worldVerticesLength = num20 << 1;
				clippingAttachment.vertices = vertices4.vertices;
				clippingAttachment.bones = vertices4.bones;
				return clippingAttachment;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00064C04 File Offset: 0x00062E04
		private SkeletonBinary.Vertices ReadVertices(SkeletonBinary.SkeletonInput input, int vertexCount)
		{
			float scale = this.Scale;
			int num = vertexCount << 1;
			SkeletonBinary.Vertices vertices = new SkeletonBinary.Vertices();
			if (!input.ReadBoolean())
			{
				vertices.vertices = this.ReadFloatArray(input, num, scale);
				return vertices;
			}
			ExposedList<float> exposedList = new ExposedList<float>(num * 3 * 3);
			ExposedList<int> exposedList2 = new ExposedList<int>(num * 3);
			for (int i = 0; i < vertexCount; i++)
			{
				int num2 = input.ReadInt(true);
				exposedList2.Add(num2);
				for (int j = 0; j < num2; j++)
				{
					exposedList2.Add(input.ReadInt(true));
					exposedList.Add(input.ReadFloat() * scale);
					exposedList.Add(input.ReadFloat() * scale);
					exposedList.Add(input.ReadFloat());
				}
			}
			vertices.vertices = exposedList.ToArray();
			vertices.bones = exposedList2.ToArray();
			return vertices;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00064CE4 File Offset: 0x00062EE4
		private float[] ReadFloatArray(SkeletonBinary.SkeletonInput input, int n, float scale)
		{
			float[] array = new float[n];
			if (scale == 1f)
			{
				for (int i = 0; i < n; i++)
				{
					array[i] = input.ReadFloat();
				}
			}
			else
			{
				for (int j = 0; j < n; j++)
				{
					array[j] = input.ReadFloat() * scale;
				}
			}
			return array;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00064D44 File Offset: 0x00062F44
		private int[] ReadShortArray(SkeletonBinary.SkeletonInput input)
		{
			int num = input.ReadInt(true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ((int)input.ReadByte() << 8 | (int)input.ReadByte());
			}
			return array;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00064D88 File Offset: 0x00062F88
		private Animation ReadAnimation(string name, SkeletonBinary.SkeletonInput input, SkeletonData skeletonData)
		{
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>(32);
			float scale = this.Scale;
			float num = 0f;
			int i = 0;
			int num2 = input.ReadInt(true);
			while (i < num2)
			{
				int slotIndex = input.ReadInt(true);
				int j = 0;
				int num3 = input.ReadInt(true);
				while (j < num3)
				{
					int num4 = (int)input.ReadByte();
					int num5 = input.ReadInt(true);
					switch (num4)
					{
					case 0:
					{
						AttachmentTimeline attachmentTimeline = new AttachmentTimeline(num5);
						attachmentTimeline.slotIndex = slotIndex;
						for (int k = 0; k < num5; k++)
						{
							attachmentTimeline.SetFrame(k, input.ReadFloat(), input.ReadStringRef());
						}
						exposedList.Add(attachmentTimeline);
						num = Math.Max(num, attachmentTimeline.frames[num5 - 1]);
						break;
					}
					case 1:
					{
						ColorTimeline colorTimeline = new ColorTimeline(num5);
						colorTimeline.slotIndex = slotIndex;
						for (int l = 0; l < num5; l++)
						{
							float time = input.ReadFloat();
							int num6 = input.ReadInt();
							float r = (float)(((long)num6 & (long)((ulong)-16777216)) >> 24) / 255f;
							float g = (float)((num6 & 16711680) >> 16) / 255f;
							float b = (float)((num6 & 65280) >> 8) / 255f;
							float a = (float)(num6 & 255) / 255f;
							colorTimeline.SetFrame(l, time, r, g, b, a);
							if (l < num5 - 1)
							{
								this.ReadCurve(input, l, colorTimeline);
							}
						}
						exposedList.Add(colorTimeline);
						num = Math.Max(num, colorTimeline.frames[(num5 - 1) * 5]);
						break;
					}
					case 2:
					{
						TwoColorTimeline twoColorTimeline = new TwoColorTimeline(num5);
						twoColorTimeline.slotIndex = slotIndex;
						for (int m = 0; m < num5; m++)
						{
							float time2 = input.ReadFloat();
							int num7 = input.ReadInt();
							float r2 = (float)(((long)num7 & (long)((ulong)-16777216)) >> 24) / 255f;
							float g2 = (float)((num7 & 16711680) >> 16) / 255f;
							float b2 = (float)((num7 & 65280) >> 8) / 255f;
							float a2 = (float)(num7 & 255) / 255f;
							int num8 = input.ReadInt();
							float r3 = (float)((num8 & 16711680) >> 16) / 255f;
							float g3 = (float)((num8 & 65280) >> 8) / 255f;
							float b3 = (float)(num8 & 255) / 255f;
							twoColorTimeline.SetFrame(m, time2, r2, g2, b2, a2, r3, g3, b3);
							if (m < num5 - 1)
							{
								this.ReadCurve(input, m, twoColorTimeline);
							}
						}
						exposedList.Add(twoColorTimeline);
						num = Math.Max(num, twoColorTimeline.frames[(num5 - 1) * 8]);
						break;
					}
					}
					j++;
				}
				i++;
			}
			int n = 0;
			int num9 = input.ReadInt(true);
			while (n < num9)
			{
				int boneIndex = input.ReadInt(true);
				int num10 = 0;
				int num11 = input.ReadInt(true);
				while (num10 < num11)
				{
					int num12 = (int)input.ReadByte();
					int num13 = input.ReadInt(true);
					switch (num12)
					{
					case 0:
					{
						RotateTimeline rotateTimeline = new RotateTimeline(num13);
						rotateTimeline.boneIndex = boneIndex;
						for (int num14 = 0; num14 < num13; num14++)
						{
							rotateTimeline.SetFrame(num14, input.ReadFloat(), input.ReadFloat());
							if (num14 < num13 - 1)
							{
								this.ReadCurve(input, num14, rotateTimeline);
							}
						}
						exposedList.Add(rotateTimeline);
						num = Math.Max(num, rotateTimeline.frames[(num13 - 1) * 2]);
						break;
					}
					case 1:
					case 2:
					case 3:
					{
						float num15 = 1f;
						TranslateTimeline translateTimeline;
						if (num12 == 2)
						{
							translateTimeline = new ScaleTimeline(num13);
						}
						else if (num12 == 3)
						{
							translateTimeline = new ShearTimeline(num13);
						}
						else
						{
							translateTimeline = new TranslateTimeline(num13);
							num15 = scale;
						}
						translateTimeline.boneIndex = boneIndex;
						for (int num16 = 0; num16 < num13; num16++)
						{
							translateTimeline.SetFrame(num16, input.ReadFloat(), input.ReadFloat() * num15, input.ReadFloat() * num15);
							if (num16 < num13 - 1)
							{
								this.ReadCurve(input, num16, translateTimeline);
							}
						}
						exposedList.Add(translateTimeline);
						num = Math.Max(num, translateTimeline.frames[(num13 - 1) * 3]);
						break;
					}
					}
					num10++;
				}
				n++;
			}
			int num17 = 0;
			int num18 = input.ReadInt(true);
			while (num17 < num18)
			{
				int ikConstraintIndex = input.ReadInt(true);
				int num19 = input.ReadInt(true);
				IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(num19)
				{
					ikConstraintIndex = ikConstraintIndex
				};
				for (int num20 = 0; num20 < num19; num20++)
				{
					ikConstraintTimeline.SetFrame(num20, input.ReadFloat(), input.ReadFloat(), input.ReadFloat() * scale, (int)input.ReadSByte(), input.ReadBoolean(), input.ReadBoolean());
					if (num20 < num19 - 1)
					{
						this.ReadCurve(input, num20, ikConstraintTimeline);
					}
				}
				exposedList.Add(ikConstraintTimeline);
				num = Math.Max(num, ikConstraintTimeline.frames[(num19 - 1) * 6]);
				num17++;
			}
			int num21 = 0;
			int num22 = input.ReadInt(true);
			while (num21 < num22)
			{
				int transformConstraintIndex = input.ReadInt(true);
				int num23 = input.ReadInt(true);
				TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(num23);
				transformConstraintTimeline.transformConstraintIndex = transformConstraintIndex;
				for (int num24 = 0; num24 < num23; num24++)
				{
					transformConstraintTimeline.SetFrame(num24, input.ReadFloat(), input.ReadFloat(), input.ReadFloat(), input.ReadFloat(), input.ReadFloat());
					if (num24 < num23 - 1)
					{
						this.ReadCurve(input, num24, transformConstraintTimeline);
					}
				}
				exposedList.Add(transformConstraintTimeline);
				num = Math.Max(num, transformConstraintTimeline.frames[(num23 - 1) * 5]);
				num21++;
			}
			int num25 = 0;
			int num26 = input.ReadInt(true);
			while (num25 < num26)
			{
				int num27 = input.ReadInt(true);
				PathConstraintData pathConstraintData = skeletonData.pathConstraints.Items[num27];
				int num28 = 0;
				int num29 = input.ReadInt(true);
				while (num28 < num29)
				{
					int num30 = (int)input.ReadSByte();
					int num31 = input.ReadInt(true);
					switch (num30)
					{
					case 0:
					case 1:
					{
						float num32 = 1f;
						PathConstraintPositionTimeline pathConstraintPositionTimeline;
						if (num30 == 1)
						{
							pathConstraintPositionTimeline = new PathConstraintSpacingTimeline(num31);
							if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
							{
								num32 = scale;
							}
						}
						else
						{
							pathConstraintPositionTimeline = new PathConstraintPositionTimeline(num31);
							if (pathConstraintData.positionMode == PositionMode.Fixed)
							{
								num32 = scale;
							}
						}
						pathConstraintPositionTimeline.pathConstraintIndex = num27;
						for (int num33 = 0; num33 < num31; num33++)
						{
							pathConstraintPositionTimeline.SetFrame(num33, input.ReadFloat(), input.ReadFloat() * num32);
							if (num33 < num31 - 1)
							{
								this.ReadCurve(input, num33, pathConstraintPositionTimeline);
							}
						}
						exposedList.Add(pathConstraintPositionTimeline);
						num = Math.Max(num, pathConstraintPositionTimeline.frames[(num31 - 1) * 2]);
						break;
					}
					case 2:
					{
						PathConstraintMixTimeline pathConstraintMixTimeline = new PathConstraintMixTimeline(num31);
						pathConstraintMixTimeline.pathConstraintIndex = num27;
						for (int num34 = 0; num34 < num31; num34++)
						{
							pathConstraintMixTimeline.SetFrame(num34, input.ReadFloat(), input.ReadFloat(), input.ReadFloat());
							if (num34 < num31 - 1)
							{
								this.ReadCurve(input, num34, pathConstraintMixTimeline);
							}
						}
						exposedList.Add(pathConstraintMixTimeline);
						num = Math.Max(num, pathConstraintMixTimeline.frames[(num31 - 1) * 3]);
						break;
					}
					}
					num28++;
				}
				num25++;
			}
			int num35 = 0;
			int num36 = input.ReadInt(true);
			while (num35 < num36)
			{
				Skin skin = skeletonData.skins.Items[input.ReadInt(true)];
				int num37 = 0;
				int num38 = input.ReadInt(true);
				while (num37 < num38)
				{
					int slotIndex2 = input.ReadInt(true);
					int num39 = 0;
					int num40 = input.ReadInt(true);
					while (num39 < num40)
					{
						VertexAttachment vertexAttachment = (VertexAttachment)skin.GetAttachment(slotIndex2, input.ReadStringRef());
						bool flag = vertexAttachment.bones != null;
						float[] vertices = vertexAttachment.vertices;
						int num41 = (!flag) ? vertices.Length : (vertices.Length / 3 * 2);
						int num42 = input.ReadInt(true);
						DeformTimeline deformTimeline = new DeformTimeline(num42);
						deformTimeline.slotIndex = slotIndex2;
						deformTimeline.attachment = vertexAttachment;
						for (int num43 = 0; num43 < num42; num43++)
						{
							float time3 = input.ReadFloat();
							int num44 = input.ReadInt(true);
							float[] array;
							if (num44 == 0)
							{
								array = ((!flag) ? vertices : new float[num41]);
							}
							else
							{
								array = new float[num41];
								int num45 = input.ReadInt(true);
								num44 += num45;
								if (scale == 1f)
								{
									for (int num46 = num45; num46 < num44; num46++)
									{
										array[num46] = input.ReadFloat();
									}
								}
								else
								{
									for (int num47 = num45; num47 < num44; num47++)
									{
										array[num47] = input.ReadFloat() * scale;
									}
								}
								if (!flag)
								{
									int num48 = 0;
									int num49 = array.Length;
									while (num48 < num49)
									{
										array[num48] += vertices[num48];
										num48++;
									}
								}
							}
							deformTimeline.SetFrame(num43, time3, array);
							if (num43 < num42 - 1)
							{
								this.ReadCurve(input, num43, deformTimeline);
							}
						}
						exposedList.Add(deformTimeline);
						num = Math.Max(num, deformTimeline.frames[num42 - 1]);
						num39++;
					}
					num37++;
				}
				num35++;
			}
			int num50 = input.ReadInt(true);
			if (num50 > 0)
			{
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(num50);
				int count = skeletonData.slots.Count;
				for (int num51 = 0; num51 < num50; num51++)
				{
					float time4 = input.ReadFloat();
					int num52 = input.ReadInt(true);
					int[] array2 = new int[count];
					for (int num53 = count - 1; num53 >= 0; num53--)
					{
						array2[num53] = -1;
					}
					int[] array3 = new int[count - num52];
					int num54 = 0;
					int num55 = 0;
					for (int num56 = 0; num56 < num52; num56++)
					{
						int num57 = input.ReadInt(true);
						while (num54 != num57)
						{
							array3[num55++] = num54++;
						}
						array2[num54 + input.ReadInt(true)] = num54++;
					}
					while (num54 < count)
					{
						array3[num55++] = num54++;
					}
					for (int num58 = count - 1; num58 >= 0; num58--)
					{
						if (array2[num58] == -1)
						{
							array2[num58] = array3[--num55];
						}
					}
					drawOrderTimeline.SetFrame(num51, time4, array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[num50 - 1]);
			}
			int num59 = input.ReadInt(true);
			if (num59 > 0)
			{
				EventTimeline eventTimeline = new EventTimeline(num59);
				for (int num60 = 0; num60 < num59; num60++)
				{
					float time5 = input.ReadFloat();
					EventData eventData = skeletonData.events.Items[input.ReadInt(true)];
					Event @event = new Event(time5, eventData)
					{
						Int = input.ReadInt(false),
						Float = input.ReadFloat(),
						String = ((!input.ReadBoolean()) ? eventData.String : input.ReadString())
					};
					if (@event.data.AudioPath != null)
					{
						@event.volume = input.ReadFloat();
						@event.balance = input.ReadFloat();
					}
					eventTimeline.SetFrame(num60, @event);
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[num59 - 1]);
			}
			exposedList.TrimExcess();
			return new Animation(name, exposedList, num);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000659B4 File Offset: 0x00063BB4
		private void ReadCurve(SkeletonBinary.SkeletonInput input, int frameIndex, CurveTimeline timeline)
		{
			byte b = input.ReadByte();
			if (b != 1)
			{
				if (b == 2)
				{
					timeline.SetCurve(frameIndex, input.ReadFloat(), input.ReadFloat(), input.ReadFloat(), input.ReadFloat());
				}
			}
			else
			{
				timeline.SetStepped(frameIndex);
			}
		}

		// Token: 0x04000C0F RID: 3087
		public const int BONE_ROTATE = 0;

		// Token: 0x04000C10 RID: 3088
		public const int BONE_TRANSLATE = 1;

		// Token: 0x04000C11 RID: 3089
		public const int BONE_SCALE = 2;

		// Token: 0x04000C12 RID: 3090
		public const int BONE_SHEAR = 3;

		// Token: 0x04000C13 RID: 3091
		public const int SLOT_ATTACHMENT = 0;

		// Token: 0x04000C14 RID: 3092
		public const int SLOT_COLOR = 1;

		// Token: 0x04000C15 RID: 3093
		public const int SLOT_TWO_COLOR = 2;

		// Token: 0x04000C16 RID: 3094
		public const int PATH_POSITION = 0;

		// Token: 0x04000C17 RID: 3095
		public const int PATH_SPACING = 1;

		// Token: 0x04000C18 RID: 3096
		public const int PATH_MIX = 2;

		// Token: 0x04000C19 RID: 3097
		public const int CURVE_LINEAR = 0;

		// Token: 0x04000C1A RID: 3098
		public const int CURVE_STEPPED = 1;

		// Token: 0x04000C1B RID: 3099
		public const int CURVE_BEZIER = 2;

		// Token: 0x04000C1C RID: 3100
		private AttachmentLoader attachmentLoader;

		// Token: 0x04000C1D RID: 3101
		private List<SkeletonJson.LinkedMesh> linkedMeshes = new List<SkeletonJson.LinkedMesh>();

		// Token: 0x04000C1E RID: 3102
		public static readonly TransformMode[] TransformModeValues = new TransformMode[]
		{
			TransformMode.Normal,
			TransformMode.OnlyTranslation,
			TransformMode.NoRotationOrReflection,
			TransformMode.NoScale,
			TransformMode.NoScaleOrReflection
		};

		// Token: 0x020001C1 RID: 449
		internal class Vertices
		{
			// Token: 0x04000C20 RID: 3104
			public int[] bones;

			// Token: 0x04000C21 RID: 3105
			public float[] vertices;
		}

		// Token: 0x020001C2 RID: 450
		internal class SkeletonInput
		{
			// Token: 0x06000E30 RID: 3632 RVA: 0x00065A14 File Offset: 0x00063C14
			public SkeletonInput(Stream input)
			{
				this.input = input;
			}

			// Token: 0x06000E31 RID: 3633 RVA: 0x00065A48 File Offset: 0x00063C48
			public byte ReadByte()
			{
				return (byte)this.input.ReadByte();
			}

			// Token: 0x06000E32 RID: 3634 RVA: 0x00065A58 File Offset: 0x00063C58
			public sbyte ReadSByte()
			{
				int num = this.input.ReadByte();
				if (num == -1)
				{
					throw new EndOfStreamException();
				}
				return (sbyte)num;
			}

			// Token: 0x06000E33 RID: 3635 RVA: 0x00065A80 File Offset: 0x00063C80
			public bool ReadBoolean()
			{
				return this.input.ReadByte() != 0;
			}

			// Token: 0x06000E34 RID: 3636 RVA: 0x00065A94 File Offset: 0x00063C94
			public float ReadFloat()
			{
				this.input.Read(this.bytesBigEndian, 0, 4);
				this.chars[3] = this.bytesBigEndian[0];
				this.chars[2] = this.bytesBigEndian[1];
				this.chars[1] = this.bytesBigEndian[2];
				this.chars[0] = this.bytesBigEndian[3];
				return BitConverter.ToSingle(this.chars, 0);
			}

			// Token: 0x06000E35 RID: 3637 RVA: 0x00065B04 File Offset: 0x00063D04
			public int ReadInt()
			{
				this.input.Read(this.bytesBigEndian, 0, 4);
				return ((int)this.bytesBigEndian[0] << 24) + ((int)this.bytesBigEndian[1] << 16) + ((int)this.bytesBigEndian[2] << 8) + (int)this.bytesBigEndian[3];
			}

			// Token: 0x06000E36 RID: 3638 RVA: 0x00065B50 File Offset: 0x00063D50
			public int ReadInt(bool optimizePositive)
			{
				int num = this.input.ReadByte();
				int num2 = num & 127;
				if ((num & 128) != 0)
				{
					num = this.input.ReadByte();
					num2 |= (num & 127) << 7;
					if ((num & 128) != 0)
					{
						num = this.input.ReadByte();
						num2 |= (num & 127) << 14;
						if ((num & 128) != 0)
						{
							num = this.input.ReadByte();
							num2 |= (num & 127) << 21;
							if ((num & 128) != 0)
							{
								num2 |= (this.input.ReadByte() & 127) << 28;
							}
						}
					}
				}
				return (!optimizePositive) ? (num2 >> 1 ^ -(num2 & 1)) : num2;
			}

			// Token: 0x06000E37 RID: 3639 RVA: 0x00065C08 File Offset: 0x00063E08
			public string ReadString()
			{
				int num = this.ReadInt(true);
				int num2 = num;
				if (num2 == 0)
				{
					return null;
				}
				if (num2 != 1)
				{
					num--;
					byte[] array = this.chars;
					if (array.Length < num)
					{
						array = new byte[num];
					}
					this.ReadFully(array, 0, num);
					return Encoding.UTF8.GetString(array, 0, num);
				}
				return string.Empty;
			}

			// Token: 0x06000E38 RID: 3640 RVA: 0x00065C6C File Offset: 0x00063E6C
			public string ReadStringRef()
			{
				int num = this.ReadInt(true);
				return (num != 0) ? this.strings.Items[num - 1] : null;
			}

			// Token: 0x06000E39 RID: 3641 RVA: 0x00065C9C File Offset: 0x00063E9C
			public void ReadFully(byte[] buffer, int offset, int length)
			{
				while (length > 0)
				{
					int num = this.input.Read(buffer, offset, length);
					if (num <= 0)
					{
						throw new EndOfStreamException();
					}
					offset += num;
					length -= num;
				}
			}

			// Token: 0x06000E3A RID: 3642 RVA: 0x00065CDC File Offset: 0x00063EDC
			public string GetVersionString()
			{
				string @string;
				try
				{
					int num = this.ReadInt(true);
					if (num > 1)
					{
						this.input.Position += (long)(num - 1);
					}
					num = this.ReadInt(true);
					if (num <= 1)
					{
						throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.", "input");
					}
					num--;
					byte[] array = new byte[num];
					this.ReadFully(array, 0, num);
					@string = Encoding.UTF8.GetString(array, 0, num);
				}
				catch (Exception arg)
				{
					throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.\n" + arg, "input");
				}
				return @string;
			}

			// Token: 0x04000C22 RID: 3106
			private byte[] chars = new byte[32];

			// Token: 0x04000C23 RID: 3107
			private byte[] bytesBigEndian = new byte[4];

			// Token: 0x04000C24 RID: 3108
			internal ExposedList<string> strings;

			// Token: 0x04000C25 RID: 3109
			private Stream input;
		}
	}
}
