using System;
using System.Collections.Generic;
using System.IO;

namespace Spine
{
	// Token: 0x020001C7 RID: 455
	public class SkeletonJson
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x00067544 File Offset: 0x00065744
		public SkeletonJson(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00067554 File Offset: 0x00065754
		public SkeletonJson(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader", "attachmentLoader cannot be null.");
			}
			this.attachmentLoader = attachmentLoader;
			this.Scale = 1f;
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00067590 File Offset: 0x00065790
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x00067598 File Offset: 0x00065798
		public float Scale { get; set; }

		// Token: 0x06000E99 RID: 3737 RVA: 0x000675A4 File Offset: 0x000657A4
		public SkeletonData ReadSkeletonData(string path)
		{
			SkeletonData result;
			using (StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				SkeletonData skeletonData = this.ReadSkeletonData(streamReader);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				result = skeletonData;
			}
			return result;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0006760C File Offset: 0x0006580C
		public SkeletonData ReadSkeletonData(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader", "reader cannot be null.");
			}
			float scale = this.Scale;
			SkeletonData skeletonData = new SkeletonData();
			Dictionary<string, object> dictionary = Json.Deserialize(reader) as Dictionary<string, object>;
			if (dictionary == null)
			{
				throw new Exception("Invalid JSON.");
			}
			if (dictionary.ContainsKey("skeleton"))
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["skeleton"];
				skeletonData.hash = (string)dictionary2["hash"];
				skeletonData.version = (string)dictionary2["spine"];
				if ("3.8.75" == skeletonData.version)
				{
					throw new Exception("Unsupported skeleton data, please export with a newer version of Spine.");
				}
				skeletonData.x = SkeletonJson.GetFloat(dictionary2, "x", 0f);
				skeletonData.y = SkeletonJson.GetFloat(dictionary2, "y", 0f);
				skeletonData.width = SkeletonJson.GetFloat(dictionary2, "width", 0f);
				skeletonData.height = SkeletonJson.GetFloat(dictionary2, "height", 0f);
				skeletonData.fps = SkeletonJson.GetFloat(dictionary2, "fps", 30f);
				skeletonData.imagesPath = SkeletonJson.GetString(dictionary2, "images", null);
				skeletonData.audioPath = SkeletonJson.GetString(dictionary2, "audio", null);
			}
			if (dictionary.ContainsKey("bones"))
			{
				foreach (object obj in ((List<object>)dictionary["bones"]))
				{
					Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj;
					BoneData boneData = null;
					if (dictionary3.ContainsKey("parent"))
					{
						boneData = skeletonData.FindBone((string)dictionary3["parent"]);
						if (boneData == null)
						{
							throw new Exception("Parent bone not found: " + dictionary3["parent"]);
						}
					}
					BoneData boneData2 = new BoneData(skeletonData.Bones.Count, (string)dictionary3["name"], boneData);
					boneData2.length = SkeletonJson.GetFloat(dictionary3, "length", 0f) * scale;
					boneData2.x = SkeletonJson.GetFloat(dictionary3, "x", 0f) * scale;
					boneData2.y = SkeletonJson.GetFloat(dictionary3, "y", 0f) * scale;
					boneData2.rotation = SkeletonJson.GetFloat(dictionary3, "rotation", 0f);
					boneData2.scaleX = SkeletonJson.GetFloat(dictionary3, "scaleX", 1f);
					boneData2.scaleY = SkeletonJson.GetFloat(dictionary3, "scaleY", 1f);
					boneData2.shearX = SkeletonJson.GetFloat(dictionary3, "shearX", 0f);
					boneData2.shearY = SkeletonJson.GetFloat(dictionary3, "shearY", 0f);
					string @string = SkeletonJson.GetString(dictionary3, "transform", TransformMode.Normal.ToString());
					boneData2.transformMode = (TransformMode)((int)Enum.Parse(typeof(TransformMode), @string, true));
					boneData2.skinRequired = SkeletonJson.GetBoolean(dictionary3, "skin", false);
					skeletonData.bones.Add(boneData2);
				}
			}
			if (dictionary.ContainsKey("slots"))
			{
				foreach (object obj2 in ((List<object>)dictionary["slots"]))
				{
					Dictionary<string, object> dictionary4 = (Dictionary<string, object>)obj2;
					string name = (string)dictionary4["name"];
					string text = (string)dictionary4["bone"];
					BoneData boneData3 = skeletonData.FindBone(text);
					if (boneData3 == null)
					{
						throw new Exception("Slot bone not found: " + text);
					}
					SlotData slotData = new SlotData(skeletonData.Slots.Count, name, boneData3);
					if (dictionary4.ContainsKey("color"))
					{
						string hexString = (string)dictionary4["color"];
						slotData.r = SkeletonJson.ToColor(hexString, 0, 8);
						slotData.g = SkeletonJson.ToColor(hexString, 1, 8);
						slotData.b = SkeletonJson.ToColor(hexString, 2, 8);
						slotData.a = SkeletonJson.ToColor(hexString, 3, 8);
					}
					if (dictionary4.ContainsKey("dark"))
					{
						string hexString2 = (string)dictionary4["dark"];
						slotData.r2 = SkeletonJson.ToColor(hexString2, 0, 6);
						slotData.g2 = SkeletonJson.ToColor(hexString2, 1, 6);
						slotData.b2 = SkeletonJson.ToColor(hexString2, 2, 6);
						slotData.hasSecondColor = true;
					}
					slotData.attachmentName = SkeletonJson.GetString(dictionary4, "attachment", null);
					if (dictionary4.ContainsKey("blend"))
					{
						slotData.blendMode = (BlendMode)((int)Enum.Parse(typeof(BlendMode), (string)dictionary4["blend"], true));
					}
					else
					{
						slotData.blendMode = BlendMode.Normal;
					}
					skeletonData.slots.Add(slotData);
				}
			}
			if (dictionary.ContainsKey("ik"))
			{
				foreach (object obj3 in ((List<object>)dictionary["ik"]))
				{
					Dictionary<string, object> dictionary5 = (Dictionary<string, object>)obj3;
					IkConstraintData ikConstraintData = new IkConstraintData((string)dictionary5["name"]);
					ikConstraintData.order = SkeletonJson.GetInt(dictionary5, "order", 0);
					ikConstraintData.skinRequired = SkeletonJson.GetBoolean(dictionary5, "skin", false);
					if (dictionary5.ContainsKey("bones"))
					{
						foreach (object obj4 in ((List<object>)dictionary5["bones"]))
						{
							string text2 = (string)obj4;
							BoneData boneData4 = skeletonData.FindBone(text2);
							if (boneData4 == null)
							{
								throw new Exception("IK bone not found: " + text2);
							}
							ikConstraintData.bones.Add(boneData4);
						}
					}
					string text3 = (string)dictionary5["target"];
					ikConstraintData.target = skeletonData.FindBone(text3);
					if (ikConstraintData.target == null)
					{
						throw new Exception("IK target bone not found: " + text3);
					}
					ikConstraintData.mix = SkeletonJson.GetFloat(dictionary5, "mix", 1f);
					ikConstraintData.softness = SkeletonJson.GetFloat(dictionary5, "softness", 0f) * scale;
					ikConstraintData.bendDirection = ((!SkeletonJson.GetBoolean(dictionary5, "bendPositive", true)) ? -1 : 1);
					ikConstraintData.compress = SkeletonJson.GetBoolean(dictionary5, "compress", false);
					ikConstraintData.stretch = SkeletonJson.GetBoolean(dictionary5, "stretch", false);
					ikConstraintData.uniform = SkeletonJson.GetBoolean(dictionary5, "uniform", false);
					skeletonData.ikConstraints.Add(ikConstraintData);
				}
			}
			if (dictionary.ContainsKey("transform"))
			{
				foreach (object obj5 in ((List<object>)dictionary["transform"]))
				{
					Dictionary<string, object> dictionary6 = (Dictionary<string, object>)obj5;
					TransformConstraintData transformConstraintData = new TransformConstraintData((string)dictionary6["name"]);
					transformConstraintData.order = SkeletonJson.GetInt(dictionary6, "order", 0);
					transformConstraintData.skinRequired = SkeletonJson.GetBoolean(dictionary6, "skin", false);
					if (dictionary6.ContainsKey("bones"))
					{
						foreach (object obj6 in ((List<object>)dictionary6["bones"]))
						{
							string text4 = (string)obj6;
							BoneData boneData5 = skeletonData.FindBone(text4);
							if (boneData5 == null)
							{
								throw new Exception("Transform constraint bone not found: " + text4);
							}
							transformConstraintData.bones.Add(boneData5);
						}
					}
					string text5 = (string)dictionary6["target"];
					transformConstraintData.target = skeletonData.FindBone(text5);
					if (transformConstraintData.target == null)
					{
						throw new Exception("Transform constraint target bone not found: " + text5);
					}
					transformConstraintData.local = SkeletonJson.GetBoolean(dictionary6, "local", false);
					transformConstraintData.relative = SkeletonJson.GetBoolean(dictionary6, "relative", false);
					transformConstraintData.offsetRotation = SkeletonJson.GetFloat(dictionary6, "rotation", 0f);
					transformConstraintData.offsetX = SkeletonJson.GetFloat(dictionary6, "x", 0f) * scale;
					transformConstraintData.offsetY = SkeletonJson.GetFloat(dictionary6, "y", 0f) * scale;
					transformConstraintData.offsetScaleX = SkeletonJson.GetFloat(dictionary6, "scaleX", 0f);
					transformConstraintData.offsetScaleY = SkeletonJson.GetFloat(dictionary6, "scaleY", 0f);
					transformConstraintData.offsetShearY = SkeletonJson.GetFloat(dictionary6, "shearY", 0f);
					transformConstraintData.rotateMix = SkeletonJson.GetFloat(dictionary6, "rotateMix", 1f);
					transformConstraintData.translateMix = SkeletonJson.GetFloat(dictionary6, "translateMix", 1f);
					transformConstraintData.scaleMix = SkeletonJson.GetFloat(dictionary6, "scaleMix", 1f);
					transformConstraintData.shearMix = SkeletonJson.GetFloat(dictionary6, "shearMix", 1f);
					skeletonData.transformConstraints.Add(transformConstraintData);
				}
			}
			if (dictionary.ContainsKey("path"))
			{
				foreach (object obj7 in ((List<object>)dictionary["path"]))
				{
					Dictionary<string, object> dictionary7 = (Dictionary<string, object>)obj7;
					PathConstraintData pathConstraintData = new PathConstraintData((string)dictionary7["name"]);
					pathConstraintData.order = SkeletonJson.GetInt(dictionary7, "order", 0);
					pathConstraintData.skinRequired = SkeletonJson.GetBoolean(dictionary7, "skin", false);
					if (dictionary7.ContainsKey("bones"))
					{
						foreach (object obj8 in ((List<object>)dictionary7["bones"]))
						{
							string text6 = (string)obj8;
							BoneData boneData6 = skeletonData.FindBone(text6);
							if (boneData6 == null)
							{
								throw new Exception("Path bone not found: " + text6);
							}
							pathConstraintData.bones.Add(boneData6);
						}
					}
					string text7 = (string)dictionary7["target"];
					pathConstraintData.target = skeletonData.FindSlot(text7);
					if (pathConstraintData.target == null)
					{
						throw new Exception("Path target slot not found: " + text7);
					}
					pathConstraintData.positionMode = (PositionMode)((int)Enum.Parse(typeof(PositionMode), SkeletonJson.GetString(dictionary7, "positionMode", "percent"), true));
					pathConstraintData.spacingMode = (SpacingMode)((int)Enum.Parse(typeof(SpacingMode), SkeletonJson.GetString(dictionary7, "spacingMode", "length"), true));
					pathConstraintData.rotateMode = (RotateMode)((int)Enum.Parse(typeof(RotateMode), SkeletonJson.GetString(dictionary7, "rotateMode", "tangent"), true));
					pathConstraintData.offsetRotation = SkeletonJson.GetFloat(dictionary7, "rotation", 0f);
					pathConstraintData.position = SkeletonJson.GetFloat(dictionary7, "position", 0f);
					if (pathConstraintData.positionMode == PositionMode.Fixed)
					{
						pathConstraintData.position *= scale;
					}
					pathConstraintData.spacing = SkeletonJson.GetFloat(dictionary7, "spacing", 0f);
					if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
					{
						pathConstraintData.spacing *= scale;
					}
					pathConstraintData.rotateMix = SkeletonJson.GetFloat(dictionary7, "rotateMix", 1f);
					pathConstraintData.translateMix = SkeletonJson.GetFloat(dictionary7, "translateMix", 1f);
					skeletonData.pathConstraints.Add(pathConstraintData);
				}
			}
			if (dictionary.ContainsKey("skins"))
			{
				foreach (object obj9 in ((List<object>)dictionary["skins"]))
				{
					Dictionary<string, object> dictionary8 = (Dictionary<string, object>)obj9;
					Skin skin = new Skin((string)dictionary8["name"]);
					if (dictionary8.ContainsKey("bones"))
					{
						foreach (object obj10 in ((List<object>)dictionary8["bones"]))
						{
							string text8 = (string)obj10;
							BoneData boneData7 = skeletonData.FindBone(text8);
							if (boneData7 == null)
							{
								throw new Exception("Skin bone not found: " + text8);
							}
							skin.bones.Add(boneData7);
						}
					}
					if (dictionary8.ContainsKey("ik"))
					{
						foreach (object obj11 in ((List<object>)dictionary8["ik"]))
						{
							string text9 = (string)obj11;
							IkConstraintData ikConstraintData2 = skeletonData.FindIkConstraint(text9);
							if (ikConstraintData2 == null)
							{
								throw new Exception("Skin IK constraint not found: " + text9);
							}
							skin.constraints.Add(ikConstraintData2);
						}
					}
					if (dictionary8.ContainsKey("transform"))
					{
						foreach (object obj12 in ((List<object>)dictionary8["transform"]))
						{
							string text10 = (string)obj12;
							TransformConstraintData transformConstraintData2 = skeletonData.FindTransformConstraint(text10);
							if (transformConstraintData2 == null)
							{
								throw new Exception("Skin transform constraint not found: " + text10);
							}
							skin.constraints.Add(transformConstraintData2);
						}
					}
					if (dictionary8.ContainsKey("path"))
					{
						foreach (object obj13 in ((List<object>)dictionary8["path"]))
						{
							string text11 = (string)obj13;
							PathConstraintData pathConstraintData2 = skeletonData.FindPathConstraint(text11);
							if (pathConstraintData2 == null)
							{
								throw new Exception("Skin path constraint not found: " + text11);
							}
							skin.constraints.Add(pathConstraintData2);
						}
					}
					if (dictionary8.ContainsKey("attachments"))
					{
						foreach (KeyValuePair<string, object> keyValuePair in ((Dictionary<string, object>)dictionary8["attachments"]))
						{
							int slotIndex = skeletonData.FindSlotIndex(keyValuePair.Key);
							foreach (KeyValuePair<string, object> keyValuePair2 in ((Dictionary<string, object>)keyValuePair.Value))
							{
								try
								{
									Attachment attachment = this.ReadAttachment((Dictionary<string, object>)keyValuePair2.Value, skin, slotIndex, keyValuePair2.Key, skeletonData);
									if (attachment != null)
									{
										skin.SetAttachment(slotIndex, keyValuePair2.Key, attachment);
									}
								}
								catch (Exception innerException)
								{
									throw new Exception(string.Concat(new object[]
									{
										"Error reading attachment: ",
										keyValuePair2.Key,
										", skin: ",
										skin
									}), innerException);
								}
							}
						}
					}
					skeletonData.skins.Add(skin);
					if (skin.name == "default")
					{
						skeletonData.defaultSkin = skin;
					}
				}
			}
			int i = 0;
			int count = this.linkedMeshes.Count;
			while (i < count)
			{
				SkeletonJson.LinkedMesh linkedMesh = this.linkedMeshes[i];
				Skin skin2 = (linkedMesh.skin != null) ? skeletonData.FindSkin(linkedMesh.skin) : skeletonData.defaultSkin;
				if (skin2 == null)
				{
					throw new Exception("Slot not found: " + linkedMesh.skin);
				}
				Attachment attachment2 = skin2.GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
				if (attachment2 == null)
				{
					throw new Exception("Parent mesh not found: " + linkedMesh.parent);
				}
				linkedMesh.mesh.DeformAttachment = ((!linkedMesh.inheritDeform) ? linkedMesh.mesh : ((VertexAttachment)attachment2));
				linkedMesh.mesh.ParentMesh = (MeshAttachment)attachment2;
				linkedMesh.mesh.UpdateUVs();
				i++;
			}
			this.linkedMeshes.Clear();
			if (dictionary.ContainsKey("events"))
			{
				foreach (KeyValuePair<string, object> keyValuePair3 in ((Dictionary<string, object>)dictionary["events"]))
				{
					Dictionary<string, object> map = (Dictionary<string, object>)keyValuePair3.Value;
					EventData eventData = new EventData(keyValuePair3.Key);
					eventData.Int = SkeletonJson.GetInt(map, "int", 0);
					eventData.Float = SkeletonJson.GetFloat(map, "float", 0f);
					eventData.String = SkeletonJson.GetString(map, "string", string.Empty);
					eventData.AudioPath = SkeletonJson.GetString(map, "audio", null);
					if (eventData.AudioPath != null)
					{
						eventData.Volume = SkeletonJson.GetFloat(map, "volume", 1f);
						eventData.Balance = SkeletonJson.GetFloat(map, "balance", 0f);
					}
					skeletonData.events.Add(eventData);
				}
			}
			if (dictionary.ContainsKey("animations"))
			{
				foreach (KeyValuePair<string, object> keyValuePair4 in ((Dictionary<string, object>)dictionary["animations"]))
				{
					try
					{
						this.ReadAnimation((Dictionary<string, object>)keyValuePair4.Value, keyValuePair4.Key, skeletonData);
					}
					catch (Exception innerException2)
					{
						throw new Exception("Error reading animation: " + keyValuePair4.Key, innerException2);
					}
				}
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00068AF8 File Offset: 0x00066CF8
		private Attachment ReadAttachment(Dictionary<string, object> map, Skin skin, int slotIndex, string name, SkeletonData skeletonData)
		{
			float scale = this.Scale;
			name = SkeletonJson.GetString(map, "name", name);
			string @string = SkeletonJson.GetString(map, "type", "region");
			AttachmentType attachmentType = (AttachmentType)((int)Enum.Parse(typeof(AttachmentType), @string, true));
			string string2 = SkeletonJson.GetString(map, "path", name);
			switch (attachmentType)
			{
			case AttachmentType.Region:
			{
				RegionAttachment regionAttachment = this.attachmentLoader.NewRegionAttachment(skin, name, string2);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = string2;
				regionAttachment.x = SkeletonJson.GetFloat(map, "x", 0f) * scale;
				regionAttachment.y = SkeletonJson.GetFloat(map, "y", 0f) * scale;
				regionAttachment.scaleX = SkeletonJson.GetFloat(map, "scaleX", 1f);
				regionAttachment.scaleY = SkeletonJson.GetFloat(map, "scaleY", 1f);
				regionAttachment.rotation = SkeletonJson.GetFloat(map, "rotation", 0f);
				regionAttachment.width = SkeletonJson.GetFloat(map, "width", 32f) * scale;
				regionAttachment.height = SkeletonJson.GetFloat(map, "height", 32f) * scale;
				if (map.ContainsKey("color"))
				{
					string hexString = (string)map["color"];
					regionAttachment.r = SkeletonJson.ToColor(hexString, 0, 8);
					regionAttachment.g = SkeletonJson.ToColor(hexString, 1, 8);
					regionAttachment.b = SkeletonJson.ToColor(hexString, 2, 8);
					regionAttachment.a = SkeletonJson.ToColor(hexString, 3, 8);
				}
				regionAttachment.UpdateOffset();
				return regionAttachment;
			}
			case AttachmentType.Boundingbox:
			{
				BoundingBoxAttachment boundingBoxAttachment = this.attachmentLoader.NewBoundingBoxAttachment(skin, name);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				this.ReadVertices(map, boundingBoxAttachment, SkeletonJson.GetInt(map, "vertexCount", 0) << 1);
				return boundingBoxAttachment;
			}
			case AttachmentType.Mesh:
			case AttachmentType.Linkedmesh:
			{
				MeshAttachment meshAttachment = this.attachmentLoader.NewMeshAttachment(skin, name, string2);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = string2;
				if (map.ContainsKey("color"))
				{
					string hexString2 = (string)map["color"];
					meshAttachment.r = SkeletonJson.ToColor(hexString2, 0, 8);
					meshAttachment.g = SkeletonJson.ToColor(hexString2, 1, 8);
					meshAttachment.b = SkeletonJson.ToColor(hexString2, 2, 8);
					meshAttachment.a = SkeletonJson.ToColor(hexString2, 3, 8);
				}
				meshAttachment.Width = SkeletonJson.GetFloat(map, "width", 0f) * scale;
				meshAttachment.Height = SkeletonJson.GetFloat(map, "height", 0f) * scale;
				string string3 = SkeletonJson.GetString(map, "parent", null);
				if (string3 != null)
				{
					this.linkedMeshes.Add(new SkeletonJson.LinkedMesh(meshAttachment, SkeletonJson.GetString(map, "skin", null), slotIndex, string3, SkeletonJson.GetBoolean(map, "deform", true)));
					return meshAttachment;
				}
				float[] floatArray = SkeletonJson.GetFloatArray(map, "uvs", 1f);
				this.ReadVertices(map, meshAttachment, floatArray.Length);
				meshAttachment.triangles = SkeletonJson.GetIntArray(map, "triangles");
				meshAttachment.regionUVs = floatArray;
				meshAttachment.UpdateUVs();
				if (map.ContainsKey("hull"))
				{
					meshAttachment.HullLength = SkeletonJson.GetInt(map, "hull", 0) * 2;
				}
				if (map.ContainsKey("edges"))
				{
					meshAttachment.Edges = SkeletonJson.GetIntArray(map, "edges");
				}
				return meshAttachment;
			}
			case AttachmentType.Path:
			{
				PathAttachment pathAttachment = this.attachmentLoader.NewPathAttachment(skin, name);
				if (pathAttachment == null)
				{
					return null;
				}
				pathAttachment.closed = SkeletonJson.GetBoolean(map, "closed", false);
				pathAttachment.constantSpeed = SkeletonJson.GetBoolean(map, "constantSpeed", true);
				int @int = SkeletonJson.GetInt(map, "vertexCount", 0);
				this.ReadVertices(map, pathAttachment, @int << 1);
				pathAttachment.lengths = SkeletonJson.GetFloatArray(map, "lengths", scale);
				return pathAttachment;
			}
			case AttachmentType.Point:
			{
				PointAttachment pointAttachment = this.attachmentLoader.NewPointAttachment(skin, name);
				if (pointAttachment == null)
				{
					return null;
				}
				pointAttachment.x = SkeletonJson.GetFloat(map, "x", 0f) * scale;
				pointAttachment.y = SkeletonJson.GetFloat(map, "y", 0f) * scale;
				pointAttachment.rotation = SkeletonJson.GetFloat(map, "rotation", 0f);
				return pointAttachment;
			}
			case AttachmentType.Clipping:
			{
				ClippingAttachment clippingAttachment = this.attachmentLoader.NewClippingAttachment(skin, name);
				if (clippingAttachment == null)
				{
					return null;
				}
				string string4 = SkeletonJson.GetString(map, "end", null);
				if (string4 != null)
				{
					SlotData slotData = skeletonData.FindSlot(string4);
					if (slotData == null)
					{
						throw new Exception("Clipping end slot not found: " + string4);
					}
					clippingAttachment.EndSlot = slotData;
				}
				this.ReadVertices(map, clippingAttachment, SkeletonJson.GetInt(map, "vertexCount", 0) << 1);
				return clippingAttachment;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00068FB8 File Offset: 0x000671B8
		private void ReadVertices(Dictionary<string, object> map, VertexAttachment attachment, int verticesLength)
		{
			attachment.WorldVerticesLength = verticesLength;
			float[] floatArray = SkeletonJson.GetFloatArray(map, "vertices", 1f);
			float scale = this.Scale;
			if (verticesLength == floatArray.Length)
			{
				if (scale != 1f)
				{
					for (int i = 0; i < floatArray.Length; i++)
					{
						floatArray[i] *= scale;
					}
				}
				attachment.vertices = floatArray;
				return;
			}
			ExposedList<float> exposedList = new ExposedList<float>(verticesLength * 3 * 3);
			ExposedList<int> exposedList2 = new ExposedList<int>(verticesLength * 3);
			int j = 0;
			int num = floatArray.Length;
			while (j < num)
			{
				int num2 = (int)floatArray[j++];
				exposedList2.Add(num2);
				int num3 = j + num2 * 4;
				while (j < num3)
				{
					exposedList2.Add((int)floatArray[j]);
					exposedList.Add(floatArray[j + 1] * this.Scale);
					exposedList.Add(floatArray[j + 2] * this.Scale);
					exposedList.Add(floatArray[j + 3]);
					j += 4;
				}
			}
			attachment.bones = exposedList2.ToArray();
			attachment.vertices = exposedList.ToArray();
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000690D4 File Offset: 0x000672D4
		private void ReadAnimation(Dictionary<string, object> map, string name, SkeletonData skeletonData)
		{
			float scale = this.Scale;
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
			float num = 0f;
			if (map.ContainsKey("slots"))
			{
				foreach (KeyValuePair<string, object> keyValuePair in ((Dictionary<string, object>)map["slots"]))
				{
					string key = keyValuePair.Key;
					int slotIndex = skeletonData.FindSlotIndex(key);
					Dictionary<string, object> dictionary = (Dictionary<string, object>)keyValuePair.Value;
					foreach (KeyValuePair<string, object> keyValuePair2 in dictionary)
					{
						List<object> list = (List<object>)keyValuePair2.Value;
						string key2 = keyValuePair2.Key;
						if (key2 == "attachment")
						{
							AttachmentTimeline attachmentTimeline = new AttachmentTimeline(list.Count);
							attachmentTimeline.slotIndex = slotIndex;
							int num2 = 0;
							foreach (object obj in list)
							{
								Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
								float @float = SkeletonJson.GetFloat(dictionary2, "time", 0f);
								attachmentTimeline.SetFrame(num2++, @float, (string)dictionary2["name"]);
							}
							exposedList.Add(attachmentTimeline);
							num = Math.Max(num, attachmentTimeline.frames[attachmentTimeline.FrameCount - 1]);
						}
						else if (key2 == "color")
						{
							ColorTimeline colorTimeline = new ColorTimeline(list.Count);
							colorTimeline.slotIndex = slotIndex;
							int num3 = 0;
							foreach (object obj2 in list)
							{
								Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj2;
								float float2 = SkeletonJson.GetFloat(dictionary3, "time", 0f);
								string hexString = (string)dictionary3["color"];
								colorTimeline.SetFrame(num3, float2, SkeletonJson.ToColor(hexString, 0, 8), SkeletonJson.ToColor(hexString, 1, 8), SkeletonJson.ToColor(hexString, 2, 8), SkeletonJson.ToColor(hexString, 3, 8));
								SkeletonJson.ReadCurve(dictionary3, colorTimeline, num3);
								num3++;
							}
							exposedList.Add(colorTimeline);
							num = Math.Max(num, colorTimeline.frames[(colorTimeline.FrameCount - 1) * 5]);
						}
						else
						{
							if (!(key2 == "twoColor"))
							{
								throw new Exception(string.Concat(new string[]
								{
									"Invalid timeline type for a slot: ",
									key2,
									" (",
									key,
									")"
								}));
							}
							TwoColorTimeline twoColorTimeline = new TwoColorTimeline(list.Count);
							twoColorTimeline.slotIndex = slotIndex;
							int num4 = 0;
							foreach (object obj3 in list)
							{
								Dictionary<string, object> dictionary4 = (Dictionary<string, object>)obj3;
								float float3 = SkeletonJson.GetFloat(dictionary4, "time", 0f);
								string hexString2 = (string)dictionary4["light"];
								string hexString3 = (string)dictionary4["dark"];
								twoColorTimeline.SetFrame(num4, float3, SkeletonJson.ToColor(hexString2, 0, 8), SkeletonJson.ToColor(hexString2, 1, 8), SkeletonJson.ToColor(hexString2, 2, 8), SkeletonJson.ToColor(hexString2, 3, 8), SkeletonJson.ToColor(hexString3, 0, 6), SkeletonJson.ToColor(hexString3, 1, 6), SkeletonJson.ToColor(hexString3, 2, 6));
								SkeletonJson.ReadCurve(dictionary4, twoColorTimeline, num4);
								num4++;
							}
							exposedList.Add(twoColorTimeline);
							num = Math.Max(num, twoColorTimeline.frames[(twoColorTimeline.FrameCount - 1) * 8]);
						}
					}
				}
			}
			if (map.ContainsKey("bones"))
			{
				foreach (KeyValuePair<string, object> keyValuePair3 in ((Dictionary<string, object>)map["bones"]))
				{
					string key3 = keyValuePair3.Key;
					int num5 = skeletonData.FindBoneIndex(key3);
					if (num5 == -1)
					{
						throw new Exception("Bone not found: " + key3);
					}
					Dictionary<string, object> dictionary5 = (Dictionary<string, object>)keyValuePair3.Value;
					foreach (KeyValuePair<string, object> keyValuePair4 in dictionary5)
					{
						List<object> list2 = (List<object>)keyValuePair4.Value;
						string key4 = keyValuePair4.Key;
						if (key4 == "rotate")
						{
							RotateTimeline rotateTimeline = new RotateTimeline(list2.Count);
							rotateTimeline.boneIndex = num5;
							int num6 = 0;
							foreach (object obj4 in list2)
							{
								Dictionary<string, object> dictionary6 = (Dictionary<string, object>)obj4;
								rotateTimeline.SetFrame(num6, SkeletonJson.GetFloat(dictionary6, "time", 0f), SkeletonJson.GetFloat(dictionary6, "angle", 0f));
								SkeletonJson.ReadCurve(dictionary6, rotateTimeline, num6);
								num6++;
							}
							exposedList.Add(rotateTimeline);
							num = Math.Max(num, rotateTimeline.frames[(rotateTimeline.FrameCount - 1) * 2]);
						}
						else
						{
							if (!(key4 == "translate") && !(key4 == "scale") && !(key4 == "shear"))
							{
								throw new Exception(string.Concat(new string[]
								{
									"Invalid timeline type for a bone: ",
									key4,
									" (",
									key3,
									")"
								}));
							}
							float num7 = 1f;
							float defaultValue = 0f;
							TranslateTimeline translateTimeline;
							if (key4 == "scale")
							{
								translateTimeline = new ScaleTimeline(list2.Count);
								defaultValue = 1f;
							}
							else if (key4 == "shear")
							{
								translateTimeline = new ShearTimeline(list2.Count);
							}
							else
							{
								translateTimeline = new TranslateTimeline(list2.Count);
								num7 = scale;
							}
							translateTimeline.boneIndex = num5;
							int num8 = 0;
							foreach (object obj5 in list2)
							{
								Dictionary<string, object> dictionary7 = (Dictionary<string, object>)obj5;
								float float4 = SkeletonJson.GetFloat(dictionary7, "time", 0f);
								float float5 = SkeletonJson.GetFloat(dictionary7, "x", defaultValue);
								float float6 = SkeletonJson.GetFloat(dictionary7, "y", defaultValue);
								translateTimeline.SetFrame(num8, float4, float5 * num7, float6 * num7);
								SkeletonJson.ReadCurve(dictionary7, translateTimeline, num8);
								num8++;
							}
							exposedList.Add(translateTimeline);
							num = Math.Max(num, translateTimeline.frames[(translateTimeline.FrameCount - 1) * 3]);
						}
					}
				}
			}
			if (map.ContainsKey("ik"))
			{
				foreach (KeyValuePair<string, object> keyValuePair5 in ((Dictionary<string, object>)map["ik"]))
				{
					IkConstraintData item = skeletonData.FindIkConstraint(keyValuePair5.Key);
					List<object> list3 = (List<object>)keyValuePair5.Value;
					IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(list3.Count);
					ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
					int num9 = 0;
					foreach (object obj6 in list3)
					{
						Dictionary<string, object> dictionary8 = (Dictionary<string, object>)obj6;
						ikConstraintTimeline.SetFrame(num9, SkeletonJson.GetFloat(dictionary8, "time", 0f), SkeletonJson.GetFloat(dictionary8, "mix", 1f), SkeletonJson.GetFloat(dictionary8, "softness", 0f) * scale, (!SkeletonJson.GetBoolean(dictionary8, "bendPositive", true)) ? -1 : 1, SkeletonJson.GetBoolean(dictionary8, "compress", false), SkeletonJson.GetBoolean(dictionary8, "stretch", false));
						SkeletonJson.ReadCurve(dictionary8, ikConstraintTimeline, num9);
						num9++;
					}
					exposedList.Add(ikConstraintTimeline);
					num = Math.Max(num, ikConstraintTimeline.frames[(ikConstraintTimeline.FrameCount - 1) * 6]);
				}
			}
			if (map.ContainsKey("transform"))
			{
				foreach (KeyValuePair<string, object> keyValuePair6 in ((Dictionary<string, object>)map["transform"]))
				{
					TransformConstraintData item2 = skeletonData.FindTransformConstraint(keyValuePair6.Key);
					List<object> list4 = (List<object>)keyValuePair6.Value;
					TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(list4.Count);
					transformConstraintTimeline.transformConstraintIndex = skeletonData.transformConstraints.IndexOf(item2);
					int num10 = 0;
					foreach (object obj7 in list4)
					{
						Dictionary<string, object> dictionary9 = (Dictionary<string, object>)obj7;
						transformConstraintTimeline.SetFrame(num10, SkeletonJson.GetFloat(dictionary9, "time", 0f), SkeletonJson.GetFloat(dictionary9, "rotateMix", 1f), SkeletonJson.GetFloat(dictionary9, "translateMix", 1f), SkeletonJson.GetFloat(dictionary9, "scaleMix", 1f), SkeletonJson.GetFloat(dictionary9, "shearMix", 1f));
						SkeletonJson.ReadCurve(dictionary9, transformConstraintTimeline, num10);
						num10++;
					}
					exposedList.Add(transformConstraintTimeline);
					num = Math.Max(num, transformConstraintTimeline.frames[(transformConstraintTimeline.FrameCount - 1) * 5]);
				}
			}
			if (map.ContainsKey("path"))
			{
				foreach (KeyValuePair<string, object> keyValuePair7 in ((Dictionary<string, object>)map["path"]))
				{
					int num11 = skeletonData.FindPathConstraintIndex(keyValuePair7.Key);
					if (num11 == -1)
					{
						throw new Exception("Path constraint not found: " + keyValuePair7.Key);
					}
					PathConstraintData pathConstraintData = skeletonData.pathConstraints.Items[num11];
					Dictionary<string, object> dictionary10 = (Dictionary<string, object>)keyValuePair7.Value;
					foreach (KeyValuePair<string, object> keyValuePair8 in dictionary10)
					{
						List<object> list5 = (List<object>)keyValuePair8.Value;
						string key5 = keyValuePair8.Key;
						if (key5 == "position" || key5 == "spacing")
						{
							float num12 = 1f;
							PathConstraintPositionTimeline pathConstraintPositionTimeline;
							if (key5 == "spacing")
							{
								pathConstraintPositionTimeline = new PathConstraintSpacingTimeline(list5.Count);
								if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
								{
									num12 = scale;
								}
							}
							else
							{
								pathConstraintPositionTimeline = new PathConstraintPositionTimeline(list5.Count);
								if (pathConstraintData.positionMode == PositionMode.Fixed)
								{
									num12 = scale;
								}
							}
							pathConstraintPositionTimeline.pathConstraintIndex = num11;
							int num13 = 0;
							foreach (object obj8 in list5)
							{
								Dictionary<string, object> dictionary11 = (Dictionary<string, object>)obj8;
								pathConstraintPositionTimeline.SetFrame(num13, SkeletonJson.GetFloat(dictionary11, "time", 0f), SkeletonJson.GetFloat(dictionary11, key5, 0f) * num12);
								SkeletonJson.ReadCurve(dictionary11, pathConstraintPositionTimeline, num13);
								num13++;
							}
							exposedList.Add(pathConstraintPositionTimeline);
							num = Math.Max(num, pathConstraintPositionTimeline.frames[(pathConstraintPositionTimeline.FrameCount - 1) * 2]);
						}
						else if (key5 == "mix")
						{
							PathConstraintMixTimeline pathConstraintMixTimeline = new PathConstraintMixTimeline(list5.Count);
							pathConstraintMixTimeline.pathConstraintIndex = num11;
							int num14 = 0;
							foreach (object obj9 in list5)
							{
								Dictionary<string, object> dictionary12 = (Dictionary<string, object>)obj9;
								pathConstraintMixTimeline.SetFrame(num14, SkeletonJson.GetFloat(dictionary12, "time", 0f), SkeletonJson.GetFloat(dictionary12, "rotateMix", 1f), SkeletonJson.GetFloat(dictionary12, "translateMix", 1f));
								SkeletonJson.ReadCurve(dictionary12, pathConstraintMixTimeline, num14);
								num14++;
							}
							exposedList.Add(pathConstraintMixTimeline);
							num = Math.Max(num, pathConstraintMixTimeline.frames[(pathConstraintMixTimeline.FrameCount - 1) * 3]);
						}
					}
				}
			}
			if (map.ContainsKey("deform"))
			{
				foreach (KeyValuePair<string, object> keyValuePair9 in ((Dictionary<string, object>)map["deform"]))
				{
					Skin skin = skeletonData.FindSkin(keyValuePair9.Key);
					foreach (KeyValuePair<string, object> keyValuePair10 in ((Dictionary<string, object>)keyValuePair9.Value))
					{
						int num15 = skeletonData.FindSlotIndex(keyValuePair10.Key);
						if (num15 == -1)
						{
							throw new Exception("Slot not found: " + keyValuePair10.Key);
						}
						foreach (KeyValuePair<string, object> keyValuePair11 in ((Dictionary<string, object>)keyValuePair10.Value))
						{
							List<object> list6 = (List<object>)keyValuePair11.Value;
							VertexAttachment vertexAttachment = (VertexAttachment)skin.GetAttachment(num15, keyValuePair11.Key);
							if (vertexAttachment == null)
							{
								throw new Exception("Deform attachment not found: " + keyValuePair11.Key);
							}
							bool flag = vertexAttachment.bones != null;
							float[] vertices = vertexAttachment.vertices;
							int num16 = (!flag) ? vertices.Length : (vertices.Length / 3 * 2);
							DeformTimeline deformTimeline = new DeformTimeline(list6.Count);
							deformTimeline.slotIndex = num15;
							deformTimeline.attachment = vertexAttachment;
							int num17 = 0;
							foreach (object obj10 in list6)
							{
								Dictionary<string, object> dictionary13 = (Dictionary<string, object>)obj10;
								float[] array;
								if (!dictionary13.ContainsKey("vertices"))
								{
									array = ((!flag) ? vertices : new float[num16]);
								}
								else
								{
									array = new float[num16];
									int @int = SkeletonJson.GetInt(dictionary13, "offset", 0);
									float[] floatArray = SkeletonJson.GetFloatArray(dictionary13, "vertices", 1f);
									Array.Copy(floatArray, 0, array, @int, floatArray.Length);
									if (scale != 1f)
									{
										int i = @int;
										int num18 = i + floatArray.Length;
										while (i < num18)
										{
											array[i] *= scale;
											i++;
										}
									}
									if (!flag)
									{
										for (int j = 0; j < num16; j++)
										{
											array[j] += vertices[j];
										}
									}
								}
								deformTimeline.SetFrame(num17, SkeletonJson.GetFloat(dictionary13, "time", 0f), array);
								SkeletonJson.ReadCurve(dictionary13, deformTimeline, num17);
								num17++;
							}
							exposedList.Add(deformTimeline);
							num = Math.Max(num, deformTimeline.frames[deformTimeline.FrameCount - 1]);
						}
					}
				}
			}
			if (map.ContainsKey("drawOrder") || map.ContainsKey("draworder"))
			{
				List<object> list7 = (List<object>)map[(!map.ContainsKey("drawOrder")) ? "draworder" : "drawOrder"];
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(list7.Count);
				int count = skeletonData.slots.Count;
				int num19 = 0;
				foreach (object obj11 in list7)
				{
					Dictionary<string, object> dictionary14 = (Dictionary<string, object>)obj11;
					int[] array2 = null;
					if (dictionary14.ContainsKey("offsets"))
					{
						array2 = new int[count];
						for (int k = count - 1; k >= 0; k--)
						{
							array2[k] = -1;
						}
						List<object> list8 = (List<object>)dictionary14["offsets"];
						int[] array3 = new int[count - list8.Count];
						int l = 0;
						int num20 = 0;
						foreach (object obj12 in list8)
						{
							Dictionary<string, object> dictionary15 = (Dictionary<string, object>)obj12;
							int num21 = skeletonData.FindSlotIndex((string)dictionary15["slot"]);
							if (num21 == -1)
							{
								throw new Exception("Slot not found: " + dictionary15["slot"]);
							}
							while (l != num21)
							{
								array3[num20++] = l++;
							}
							int num22 = l + (int)((float)dictionary15["offset"]);
							array2[num22] = l++;
						}
						while (l < count)
						{
							array3[num20++] = l++;
						}
						for (int m = count - 1; m >= 0; m--)
						{
							if (array2[m] == -1)
							{
								array2[m] = array3[--num20];
							}
						}
					}
					drawOrderTimeline.SetFrame(num19++, SkeletonJson.GetFloat(dictionary14, "time", 0f), array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[drawOrderTimeline.FrameCount - 1]);
			}
			if (map.ContainsKey("events"))
			{
				List<object> list9 = (List<object>)map["events"];
				EventTimeline eventTimeline = new EventTimeline(list9.Count);
				int num23 = 0;
				foreach (object obj13 in list9)
				{
					Dictionary<string, object> dictionary16 = (Dictionary<string, object>)obj13;
					EventData eventData = skeletonData.FindEvent((string)dictionary16["name"]);
					if (eventData == null)
					{
						throw new Exception("Event not found: " + dictionary16["name"]);
					}
					Event @event = new Event(SkeletonJson.GetFloat(dictionary16, "time", 0f), eventData)
					{
						intValue = SkeletonJson.GetInt(dictionary16, "int", eventData.Int),
						floatValue = SkeletonJson.GetFloat(dictionary16, "float", eventData.Float),
						stringValue = SkeletonJson.GetString(dictionary16, "string", eventData.String)
					};
					if (@event.data.AudioPath != null)
					{
						@event.volume = SkeletonJson.GetFloat(dictionary16, "volume", eventData.Volume);
						@event.balance = SkeletonJson.GetFloat(dictionary16, "balance", eventData.Balance);
					}
					eventTimeline.SetFrame(num23++, @event);
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[eventTimeline.FrameCount - 1]);
			}
			exposedList.TrimExcess();
			skeletonData.animations.Add(new Animation(name, exposedList, num));
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0006A6F0 File Offset: 0x000688F0
		private static void ReadCurve(Dictionary<string, object> valueMap, CurveTimeline timeline, int frameIndex)
		{
			if (!valueMap.ContainsKey("curve"))
			{
				return;
			}
			object obj = valueMap["curve"];
			if (obj is string)
			{
				timeline.SetStepped(frameIndex);
			}
			else
			{
				timeline.SetCurve(frameIndex, (float)obj, SkeletonJson.GetFloat(valueMap, "c2", 0f), SkeletonJson.GetFloat(valueMap, "c3", 1f), SkeletonJson.GetFloat(valueMap, "c4", 1f));
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0006A770 File Offset: 0x00068970
		private static float[] GetFloatArray(Dictionary<string, object> map, string name, float scale)
		{
			List<object> list = (List<object>)map[name];
			float[] array = new float[list.Count];
			if (scale == 1f)
			{
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					array[i] = (float)list[i];
					i++;
				}
			}
			else
			{
				int j = 0;
				int count2 = list.Count;
				while (j < count2)
				{
					array[j] = (float)list[j] * scale;
					j++;
				}
			}
			return array;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0006A804 File Offset: 0x00068A04
		private static int[] GetIntArray(Dictionary<string, object> map, string name)
		{
			List<object> list = (List<object>)map[name];
			int[] array = new int[list.Count];
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				array[i] = (int)((float)list[i]);
				i++;
			}
			return array;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0006A854 File Offset: 0x00068A54
		private static float GetFloat(Dictionary<string, object> map, string name, float defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (float)map[name];
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0006A870 File Offset: 0x00068A70
		private static int GetInt(Dictionary<string, object> map, string name, int defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (int)((float)map[name]);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0006A890 File Offset: 0x00068A90
		private static bool GetBoolean(Dictionary<string, object> map, string name, bool defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (bool)map[name];
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0006A8AC File Offset: 0x00068AAC
		private static string GetString(Dictionary<string, object> map, string name, string defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (string)map[name];
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0006A8C8 File Offset: 0x00068AC8
		private static float ToColor(string hexString, int colorIndex, int expectedLength = 8)
		{
			if (hexString.Length != expectedLength)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Color hexidecimal length must be ",
					expectedLength,
					", recieved: ",
					hexString
				}), "hexString");
			}
			return (float)Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 16) / 255f;
		}

		// Token: 0x04000C4B RID: 3147
		private AttachmentLoader attachmentLoader;

		// Token: 0x04000C4C RID: 3148
		private List<SkeletonJson.LinkedMesh> linkedMeshes = new List<SkeletonJson.LinkedMesh>();

		// Token: 0x020001C8 RID: 456
		internal class LinkedMesh
		{
			// Token: 0x06000EA6 RID: 3750 RVA: 0x0006A92C File Offset: 0x00068B2C
			public LinkedMesh(MeshAttachment mesh, string skin, int slotIndex, string parent, bool inheritDeform)
			{
				this.mesh = mesh;
				this.skin = skin;
				this.slotIndex = slotIndex;
				this.parent = parent;
				this.inheritDeform = inheritDeform;
			}

			// Token: 0x04000C4E RID: 3150
			internal string parent;

			// Token: 0x04000C4F RID: 3151
			internal string skin;

			// Token: 0x04000C50 RID: 3152
			internal int slotIndex;

			// Token: 0x04000C51 RID: 3153
			internal MeshAttachment mesh;

			// Token: 0x04000C52 RID: 3154
			internal bool inheritDeform;
		}
	}
}
