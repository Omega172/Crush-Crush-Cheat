using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001DB RID: 475
	[CreateAssetMenu(fileName = "New Spine Atlas Asset", menuName = "Spine/Spine Atlas Asset")]
	public class SpineAtlasAsset : AtlasAssetBase
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0006D5F4 File Offset: 0x0006B7F4
		public override bool IsLoaded
		{
			get
			{
				return this.atlas != null;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0006D604 File Offset: 0x0006B804
		public override IEnumerable<Material> Materials
		{
			get
			{
				return this.materials;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0006D60C File Offset: 0x0006B80C
		public override int MaterialCount
		{
			get
			{
				return (this.materials != null) ? this.materials.Length : 0;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0006D628 File Offset: 0x0006B828
		public override Material PrimaryMaterial
		{
			get
			{
				return this.materials[0];
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0006D634 File Offset: 0x0006B834
		public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Material[] materials, bool initialize)
		{
			SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
			spineAtlasAsset.Reset();
			spineAtlasAsset.atlasFile = atlasText;
			spineAtlasAsset.materials = materials;
			if (initialize)
			{
				spineAtlasAsset.GetAtlas();
			}
			return spineAtlasAsset;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0006D66C File Offset: 0x0006B86C
		public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Material materialPropertySource, bool initialize)
		{
			string text = atlasText.text;
			text = text.Replace("\r", string.Empty);
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i].Trim().Length == 0)
				{
					list.Add(array[i + 1].Trim().Replace(".png", string.Empty));
				}
			}
			Material[] array2 = new Material[list.Count];
			int j = 0;
			int count = list.Count;
			while (j < count)
			{
				Material material = null;
				string a = list[j];
				int k = 0;
				int num = textures.Length;
				while (k < num)
				{
					if (string.Equals(a, textures[k].name, StringComparison.OrdinalIgnoreCase))
					{
						material = new Material(materialPropertySource);
						material.mainTexture = textures[k];
						break;
					}
					k++;
				}
				if (!(material != null))
				{
					throw new ArgumentException("Could not find matching atlas page in the texture array.");
				}
				array2[j] = material;
				j++;
			}
			return SpineAtlasAsset.CreateRuntimeInstance(atlasText, array2, initialize);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0006D7A0 File Offset: 0x0006B9A0
		public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Shader shader, bool initialize)
		{
			if (shader == null)
			{
				shader = Shader.Find("Spine/Skeleton");
			}
			Material materialPropertySource = new Material(shader);
			return SpineAtlasAsset.CreateRuntimeInstance(atlasText, textures, materialPropertySource, initialize);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0006D7D8 File Offset: 0x0006B9D8
		private void Reset()
		{
			this.Clear();
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0006D7E0 File Offset: 0x0006B9E0
		public override void Clear()
		{
			this.atlas = null;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0006D7EC File Offset: 0x0006B9EC
		public override Atlas GetAtlas()
		{
			if (this.atlasFile == null)
			{
				Debug.LogError("Atlas file not set for atlas asset: " + base.name, this);
				this.Clear();
				return null;
			}
			if (this.materials == null || this.materials.Length == 0)
			{
				Debug.LogError("Materials not set for atlas asset: " + base.name, this);
				this.Clear();
				return null;
			}
			if (this.atlas != null)
			{
				return this.atlas;
			}
			Atlas result;
			try
			{
				this.atlas = new Atlas(new StringReader(this.atlasFile.text), string.Empty, new MaterialsTextureLoader(this));
				this.atlas.FlipV();
				result = this.atlas;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error reading atlas file for atlas asset: ",
					base.name,
					"\n",
					ex.Message,
					"\n",
					ex.StackTrace
				}), this);
				result = null;
			}
			return result;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0006D920 File Offset: 0x0006BB20
		public Mesh GenerateMesh(string name, Mesh mesh, out Material material, float scale = 0.01f)
		{
			AtlasRegion atlasRegion = this.atlas.FindRegion(name);
			material = null;
			if (atlasRegion != null)
			{
				if (mesh == null)
				{
					mesh = new Mesh();
					mesh.name = name;
				}
				Vector3[] array = new Vector3[4];
				Vector2[] array2 = new Vector2[4];
				Color[] colors = new Color[]
				{
					Color.white,
					Color.white,
					Color.white,
					Color.white
				};
				int[] triangles = new int[]
				{
					0,
					1,
					2,
					2,
					3,
					0
				};
				float num = (float)atlasRegion.width / -2f;
				float x = num * -1f;
				float num2 = (float)atlasRegion.height / 2f;
				float y = num2 * -1f;
				array[0] = new Vector3(num, y, 0f) * scale;
				array[1] = new Vector3(num, num2, 0f) * scale;
				array[2] = new Vector3(x, num2, 0f) * scale;
				array[3] = new Vector3(x, y, 0f) * scale;
				float u = atlasRegion.u;
				float v = atlasRegion.v;
				float u2 = atlasRegion.u2;
				float v2 = atlasRegion.v2;
				if (!atlasRegion.rotate)
				{
					array2[0] = new Vector2(u, v2);
					array2[1] = new Vector2(u, v);
					array2[2] = new Vector2(u2, v);
					array2[3] = new Vector2(u2, v2);
				}
				else
				{
					array2[0] = new Vector2(u2, v2);
					array2[1] = new Vector2(u, v2);
					array2[2] = new Vector2(u, v);
					array2[3] = new Vector2(u2, v);
				}
				mesh.triangles = new int[0];
				mesh.vertices = array;
				mesh.uv = array2;
				mesh.colors = colors;
				mesh.triangles = triangles;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				material = (Material)atlasRegion.page.rendererObject;
			}
			else
			{
				mesh = null;
			}
			return mesh;
		}

		// Token: 0x04000CB0 RID: 3248
		public TextAsset atlasFile;

		// Token: 0x04000CB1 RID: 3249
		public Material[] materials;

		// Token: 0x04000CB2 RID: 3250
		protected Atlas atlas;
	}
}
