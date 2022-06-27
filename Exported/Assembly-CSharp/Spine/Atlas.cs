using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Spine
{
	// Token: 0x0200018E RID: 398
	public class Atlas : IEnumerable, IEnumerable<AtlasRegion>
	{
		// Token: 0x06000BB4 RID: 2996 RVA: 0x0005A94C File Offset: 0x00058B4C
		public Atlas(TextReader reader, string dir, TextureLoader textureLoader)
		{
			this.Load(reader, dir, textureLoader);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0005A974 File Offset: 0x00058B74
		public Atlas(List<AtlasPage> pages, List<AtlasRegion> regions)
		{
			this.pages = pages;
			this.regions = regions;
			this.textureLoader = null;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0005A9A8 File Offset: 0x00058BA8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.regions.GetEnumerator();
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0005A9BC File Offset: 0x00058BBC
		public IEnumerator<AtlasRegion> GetEnumerator()
		{
			return this.regions.GetEnumerator();
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0005A9D0 File Offset: 0x00058BD0
		private void Load(TextReader reader, string imagesDir, TextureLoader textureLoader)
		{
			if (textureLoader == null)
			{
				throw new ArgumentNullException("textureLoader", "textureLoader cannot be null.");
			}
			this.textureLoader = textureLoader;
			string[] array = new string[4];
			AtlasPage atlasPage = null;
			for (;;)
			{
				string text = reader.ReadLine();
				if (text == null)
				{
					break;
				}
				if (text.Trim().Length == 0)
				{
					atlasPage = null;
				}
				else if (atlasPage == null)
				{
					atlasPage = new AtlasPage();
					atlasPage.name = text;
					if (Atlas.ReadTuple(reader, array) == 2)
					{
						atlasPage.width = int.Parse(array[0], CultureInfo.InvariantCulture);
						atlasPage.height = int.Parse(array[1], CultureInfo.InvariantCulture);
						Atlas.ReadTuple(reader, array);
					}
					atlasPage.format = (Format)((int)Enum.Parse(typeof(Format), array[0], false));
					Atlas.ReadTuple(reader, array);
					atlasPage.minFilter = (TextureFilter)((int)Enum.Parse(typeof(TextureFilter), array[0], false));
					atlasPage.magFilter = (TextureFilter)((int)Enum.Parse(typeof(TextureFilter), array[1], false));
					string a = Atlas.ReadValue(reader);
					atlasPage.uWrap = TextureWrap.ClampToEdge;
					atlasPage.vWrap = TextureWrap.ClampToEdge;
					if (a == "x")
					{
						atlasPage.uWrap = TextureWrap.Repeat;
					}
					else if (a == "y")
					{
						atlasPage.vWrap = TextureWrap.Repeat;
					}
					else if (a == "xy")
					{
						atlasPage.uWrap = (atlasPage.vWrap = TextureWrap.Repeat);
					}
					textureLoader.Load(atlasPage, Path.Combine(imagesDir, text));
					this.pages.Add(atlasPage);
				}
				else
				{
					AtlasRegion atlasRegion = new AtlasRegion();
					atlasRegion.name = text;
					atlasRegion.page = atlasPage;
					string text2 = Atlas.ReadValue(reader);
					if (text2 == "true")
					{
						atlasRegion.degrees = 90;
					}
					else if (text2 == "false")
					{
						atlasRegion.degrees = 0;
					}
					else
					{
						atlasRegion.degrees = int.Parse(text2);
					}
					atlasRegion.rotate = (atlasRegion.degrees == 90);
					Atlas.ReadTuple(reader, array);
					int num = int.Parse(array[0], CultureInfo.InvariantCulture);
					int num2 = int.Parse(array[1], CultureInfo.InvariantCulture);
					Atlas.ReadTuple(reader, array);
					int num3 = int.Parse(array[0], CultureInfo.InvariantCulture);
					int num4 = int.Parse(array[1], CultureInfo.InvariantCulture);
					atlasRegion.u = (float)num / (float)atlasPage.width;
					atlasRegion.v = (float)num2 / (float)atlasPage.height;
					if (atlasRegion.rotate)
					{
						atlasRegion.u2 = (float)(num + num4) / (float)atlasPage.width;
						atlasRegion.v2 = (float)(num2 + num3) / (float)atlasPage.height;
					}
					else
					{
						atlasRegion.u2 = (float)(num + num3) / (float)atlasPage.width;
						atlasRegion.v2 = (float)(num2 + num4) / (float)atlasPage.height;
					}
					atlasRegion.x = num;
					atlasRegion.y = num2;
					atlasRegion.width = Math.Abs(num3);
					atlasRegion.height = Math.Abs(num4);
					if (Atlas.ReadTuple(reader, array) == 4)
					{
						atlasRegion.splits = new int[]
						{
							int.Parse(array[0], CultureInfo.InvariantCulture),
							int.Parse(array[1], CultureInfo.InvariantCulture),
							int.Parse(array[2], CultureInfo.InvariantCulture),
							int.Parse(array[3], CultureInfo.InvariantCulture)
						};
						if (Atlas.ReadTuple(reader, array) == 4)
						{
							atlasRegion.pads = new int[]
							{
								int.Parse(array[0], CultureInfo.InvariantCulture),
								int.Parse(array[1], CultureInfo.InvariantCulture),
								int.Parse(array[2], CultureInfo.InvariantCulture),
								int.Parse(array[3], CultureInfo.InvariantCulture)
							};
							Atlas.ReadTuple(reader, array);
						}
					}
					atlasRegion.originalWidth = int.Parse(array[0], CultureInfo.InvariantCulture);
					atlasRegion.originalHeight = int.Parse(array[1], CultureInfo.InvariantCulture);
					Atlas.ReadTuple(reader, array);
					atlasRegion.offsetX = (float)int.Parse(array[0], CultureInfo.InvariantCulture);
					atlasRegion.offsetY = (float)int.Parse(array[1], CultureInfo.InvariantCulture);
					atlasRegion.index = int.Parse(Atlas.ReadValue(reader), CultureInfo.InvariantCulture);
					this.regions.Add(atlasRegion);
				}
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0005AE28 File Offset: 0x00059028
		private static string ReadValue(TextReader reader)
		{
			string text = reader.ReadLine();
			int num = text.IndexOf(':');
			if (num == -1)
			{
				throw new Exception("Invalid line: " + text);
			}
			return text.Substring(num + 1).Trim();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0005AE6C File Offset: 0x0005906C
		private static int ReadTuple(TextReader reader, string[] tuple)
		{
			string text = reader.ReadLine();
			int num = text.IndexOf(':');
			if (num == -1)
			{
				throw new Exception("Invalid line: " + text);
			}
			int i = 0;
			int num2 = num + 1;
			while (i < 3)
			{
				int num3 = text.IndexOf(',', num2);
				if (num3 == -1)
				{
					break;
				}
				tuple[i] = text.Substring(num2, num3 - num2).Trim();
				num2 = num3 + 1;
				i++;
			}
			tuple[i] = text.Substring(num2).Trim();
			return i + 1;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0005AEFC File Offset: 0x000590FC
		public void FlipV()
		{
			int i = 0;
			int count = this.regions.Count;
			while (i < count)
			{
				AtlasRegion atlasRegion = this.regions[i];
				atlasRegion.v = 1f - atlasRegion.v;
				atlasRegion.v2 = 1f - atlasRegion.v2;
				i++;
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0005AF58 File Offset: 0x00059158
		public AtlasRegion FindRegion(string name)
		{
			int i = 0;
			int count = this.regions.Count;
			while (i < count)
			{
				if (this.regions[i].name == name)
				{
					return this.regions[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0005AFB0 File Offset: 0x000591B0
		public void Dispose()
		{
			if (this.textureLoader == null)
			{
				return;
			}
			int i = 0;
			int count = this.pages.Count;
			while (i < count)
			{
				this.textureLoader.Unload(this.pages[i].rendererObject);
				i++;
			}
		}

		// Token: 0x04000ACC RID: 2764
		private readonly List<AtlasPage> pages = new List<AtlasPage>();

		// Token: 0x04000ACD RID: 2765
		private List<AtlasRegion> regions = new List<AtlasRegion>();

		// Token: 0x04000ACE RID: 2766
		private TextureLoader textureLoader;
	}
}
