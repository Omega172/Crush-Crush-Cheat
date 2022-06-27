using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000229 RID: 553
	public static class SkeletonExtensions
	{
		// Token: 0x06001172 RID: 4466 RVA: 0x0007BBF0 File Offset: 0x00079DF0
		public static Color GetColor(this Skeleton s)
		{
			return new Color(s.r, s.g, s.b, s.a);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x0007BC10 File Offset: 0x00079E10
		public static Color GetColor(this RegionAttachment a)
		{
			return new Color(a.r, a.g, a.b, a.a);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0007BC30 File Offset: 0x00079E30
		public static Color GetColor(this MeshAttachment a)
		{
			return new Color(a.r, a.g, a.b, a.a);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0007BC50 File Offset: 0x00079E50
		public static Color GetColor(this Slot s)
		{
			return new Color(s.r, s.g, s.b, s.a);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0007BC70 File Offset: 0x00079E70
		public static Color GetColorTintBlack(this Slot s)
		{
			return new Color(s.r2, s.g2, s.b2, 1f);
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0007BC90 File Offset: 0x00079E90
		public static void SetColor(this Skeleton skeleton, Color color)
		{
			skeleton.A = color.a;
			skeleton.R = color.r;
			skeleton.G = color.g;
			skeleton.B = color.b;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0007BCD4 File Offset: 0x00079ED4
		public static void SetColor(this Skeleton skeleton, Color32 color)
		{
			skeleton.A = (float)color.a * 0.003921569f;
			skeleton.R = (float)color.r * 0.003921569f;
			skeleton.G = (float)color.g * 0.003921569f;
			skeleton.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0007BD34 File Offset: 0x00079F34
		public static void SetColor(this Slot slot, Color color)
		{
			slot.A = color.a;
			slot.R = color.r;
			slot.G = color.g;
			slot.B = color.b;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0007BD78 File Offset: 0x00079F78
		public static void SetColor(this Slot slot, Color32 color)
		{
			slot.A = (float)color.a * 0.003921569f;
			slot.R = (float)color.r * 0.003921569f;
			slot.G = (float)color.g * 0.003921569f;
			slot.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0007BDD8 File Offset: 0x00079FD8
		public static void SetColor(this RegionAttachment attachment, Color color)
		{
			attachment.A = color.a;
			attachment.R = color.r;
			attachment.G = color.g;
			attachment.B = color.b;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0007BE1C File Offset: 0x0007A01C
		public static void SetColor(this RegionAttachment attachment, Color32 color)
		{
			attachment.A = (float)color.a * 0.003921569f;
			attachment.R = (float)color.r * 0.003921569f;
			attachment.G = (float)color.g * 0.003921569f;
			attachment.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0007BE7C File Offset: 0x0007A07C
		public static void SetColor(this MeshAttachment attachment, Color color)
		{
			attachment.A = color.a;
			attachment.R = color.r;
			attachment.G = color.g;
			attachment.B = color.b;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0007BEC0 File Offset: 0x0007A0C0
		public static void SetColor(this MeshAttachment attachment, Color32 color)
		{
			attachment.A = (float)color.a * 0.003921569f;
			attachment.R = (float)color.r * 0.003921569f;
			attachment.G = (float)color.g * 0.003921569f;
			attachment.B = (float)color.b * 0.003921569f;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0007BF20 File Offset: 0x0007A120
		public static void SetLocalScale(this Skeleton skeleton, Vector2 scale)
		{
			skeleton.ScaleX = scale.x;
			skeleton.ScaleY = scale.y;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0007BF3C File Offset: 0x0007A13C
		public static Matrix4x4 GetMatrix4x4(this Bone bone)
		{
			return new Matrix4x4
			{
				m00 = bone.a,
				m01 = bone.b,
				m03 = bone.worldX,
				m10 = bone.c,
				m11 = bone.d,
				m13 = bone.worldY,
				m33 = 1f
			};
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0007BFAC File Offset: 0x0007A1AC
		public static void SetLocalPosition(this Bone bone, Vector2 position)
		{
			bone.X = position.x;
			bone.Y = position.y;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0007BFC8 File Offset: 0x0007A1C8
		public static void SetLocalPosition(this Bone bone, Vector3 position)
		{
			bone.X = position.x;
			bone.Y = position.y;
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0007BFE4 File Offset: 0x0007A1E4
		public static Vector2 GetLocalPosition(this Bone bone)
		{
			return new Vector2(bone.x, bone.y);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0007BFF8 File Offset: 0x0007A1F8
		public static Vector2 GetSkeletonSpacePosition(this Bone bone)
		{
			return new Vector2(bone.worldX, bone.worldY);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0007C00C File Offset: 0x0007A20C
		public static Vector2 GetSkeletonSpacePosition(this Bone bone, Vector2 boneLocal)
		{
			Vector2 result;
			bone.LocalToWorld(boneLocal.x, boneLocal.y, out result.x, out result.y);
			return result;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0007C03C File Offset: 0x0007A23C
		public static Vector3 GetWorldPosition(this Bone bone, Transform spineGameObjectTransform)
		{
			return spineGameObjectTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY));
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0007C058 File Offset: 0x0007A258
		public static Vector3 GetWorldPosition(this Bone bone, Transform spineGameObjectTransform, float positionScale)
		{
			return spineGameObjectTransform.TransformPoint(new Vector3(bone.worldX * positionScale, bone.worldY * positionScale));
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0007C078 File Offset: 0x0007A278
		public static Quaternion GetQuaternion(this Bone bone)
		{
			float f = Mathf.Atan2(bone.c, bone.a) * 0.5f;
			return new Quaternion(0f, 0f, Mathf.Sin(f), Mathf.Cos(f));
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0007C0B8 File Offset: 0x0007A2B8
		public static Quaternion GetLocalQuaternion(this Bone bone)
		{
			float f = bone.rotation * 0.017453292f * 0.5f;
			return new Quaternion(0f, 0f, Mathf.Sin(f), Mathf.Cos(f));
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0007C0F4 File Offset: 0x0007A2F4
		public static Vector2 GetLocalScale(this Skeleton skeleton)
		{
			return new Vector2(skeleton.ScaleX, skeleton.ScaleY);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0007C108 File Offset: 0x0007A308
		public static void GetWorldToLocalMatrix(this Bone bone, out float ia, out float ib, out float ic, out float id)
		{
			float a = bone.a;
			float b = bone.b;
			float c = bone.c;
			float d = bone.d;
			float num = 1f / (a * d - b * c);
			ia = num * d;
			ib = num * -b;
			ic = num * -c;
			id = num * a;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0007C15C File Offset: 0x0007A35C
		public static Vector2 WorldToLocal(this Bone bone, Vector2 worldPosition)
		{
			Vector2 result;
			bone.WorldToLocal(worldPosition.x, worldPosition.y, out result.x, out result.y);
			return result;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0007C18C File Offset: 0x0007A38C
		public static Vector2 SetPositionSkeletonSpace(this Bone bone, Vector2 skeletonSpacePosition)
		{
			if (bone.parent == null)
			{
				bone.SetLocalPosition(skeletonSpacePosition);
				return skeletonSpacePosition;
			}
			Bone parent = bone.parent;
			Vector2 vector = parent.WorldToLocal(skeletonSpacePosition);
			bone.SetLocalPosition(vector);
			return vector;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0007C1C4 File Offset: 0x0007A3C4
		public static Material GetMaterial(this Attachment a)
		{
			object obj = null;
			IHasRendererObject hasRendererObject = a as IHasRendererObject;
			if (hasRendererObject != null)
			{
				obj = hasRendererObject.RendererObject;
			}
			if (obj == null)
			{
				return null;
			}
			return (Material)((AtlasRegion)obj).page.rendererObject;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0007C204 File Offset: 0x0007A404
		public static Vector2[] GetLocalVertices(this VertexAttachment va, Slot slot, Vector2[] buffer)
		{
			int worldVerticesLength = va.worldVerticesLength;
			int num = worldVerticesLength >> 1;
			buffer = (buffer ?? new Vector2[num]);
			if (buffer.Length < num)
			{
				throw new ArgumentException(string.Format("Vector2 buffer too small. {0} requires an array of size {1}. Use the attachment's .WorldVerticesLength to get the correct size.", va.Name, worldVerticesLength), "buffer");
			}
			if (va.bones == null)
			{
				float[] vertices = va.vertices;
				for (int i = 0; i < num; i++)
				{
					int num2 = i * 2;
					buffer[i] = new Vector2(vertices[num2], vertices[num2 + 1]);
				}
			}
			else
			{
				float[] array = new float[worldVerticesLength];
				va.ComputeWorldVertices(slot, array);
				Bone bone = slot.bone;
				float worldX = bone.worldX;
				float worldY = bone.worldY;
				float num3;
				float num4;
				float num5;
				float num6;
				bone.GetWorldToLocalMatrix(out num3, out num4, out num5, out num6);
				for (int j = 0; j < num; j++)
				{
					int num7 = j * 2;
					float num8 = array[num7] - worldX;
					float num9 = array[num7 + 1] - worldY;
					buffer[j] = new Vector2(num8 * num3 + num9 * num4, num8 * num5 + num9 * num6);
				}
			}
			return buffer;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0007C334 File Offset: 0x0007A534
		public static Vector2[] GetWorldVertices(this VertexAttachment a, Slot slot, Vector2[] buffer)
		{
			int worldVerticesLength = a.worldVerticesLength;
			int num = worldVerticesLength >> 1;
			buffer = (buffer ?? new Vector2[num]);
			if (buffer.Length < num)
			{
				throw new ArgumentException(string.Format("Vector2 buffer too small. {0} requires an array of size {1}. Use the attachment's .WorldVerticesLength to get the correct size.", a.Name, worldVerticesLength), "buffer");
			}
			float[] array = new float[worldVerticesLength];
			a.ComputeWorldVertices(slot, array);
			int i = 0;
			int num2 = worldVerticesLength >> 1;
			while (i < num2)
			{
				int num3 = i * 2;
				buffer[i] = new Vector2(array[num3], array[num3 + 1]);
				i++;
			}
			return buffer;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0007C3D0 File Offset: 0x0007A5D0
		public static Vector3 GetWorldPosition(this PointAttachment attachment, Slot slot, Transform spineGameObjectTransform)
		{
			Vector3 position;
			position.z = 0f;
			attachment.ComputeWorldPosition(slot.bone, out position.x, out position.y);
			return spineGameObjectTransform.TransformPoint(position);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0007C40C File Offset: 0x0007A60C
		public static Vector3 GetWorldPosition(this PointAttachment attachment, Bone bone, Transform spineGameObjectTransform)
		{
			Vector3 position;
			position.z = 0f;
			attachment.ComputeWorldPosition(bone, out position.x, out position.y);
			return spineGameObjectTransform.TransformPoint(position);
		}

		// Token: 0x04000E39 RID: 3641
		private const float ByteToFloat = 0.003921569f;
	}
}
