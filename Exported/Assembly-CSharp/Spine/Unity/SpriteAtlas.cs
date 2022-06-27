using System;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x020001DF RID: 479
	public class SpriteAtlas : UnityEngine.Object
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0006E234 File Offset: 0x0006C434
		// (set) Token: 0x06000F77 RID: 3959 RVA: 0x0006E23C File Offset: 0x0006C43C
		public bool isVariant { get; private set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0006E248 File Offset: 0x0006C448
		// (set) Token: 0x06000F79 RID: 3961 RVA: 0x0006E250 File Offset: 0x0006C450
		public string tag { get; private set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0006E25C File Offset: 0x0006C45C
		// (set) Token: 0x06000F7B RID: 3963 RVA: 0x0006E264 File Offset: 0x0006C464
		public int spriteCount { get; private set; }

		// Token: 0x06000F7C RID: 3964 RVA: 0x0006E270 File Offset: 0x0006C470
		public bool CanBindTo(Sprite sprite)
		{
			return true;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0006E274 File Offset: 0x0006C474
		public Sprite GetSprite(string name)
		{
			return null;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0006E278 File Offset: 0x0006C478
		public int GetSprites(Sprite[] sprites)
		{
			return 0;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0006E27C File Offset: 0x0006C47C
		public int GetSprites(Sprite[] sprites, string name)
		{
			return 0;
		}
	}
}
