using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000160 RID: 352
public class CachedTextObject<T> where T : IEquatable<T>
{
	// Token: 0x06000A18 RID: 2584 RVA: 0x000538FC File Offset: 0x00051AFC
	public CachedTextObject(GameObject parent, string path, T initialValue)
	{
		this.cachedObject = new CachedObject<Text>(parent, path);
		this.cachedValue = initialValue;
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00053918 File Offset: 0x00051B18
	public void SetValue(T value)
	{
		if (value.Equals(this.cachedValue))
		{
			return;
		}
		this.cachedValue = value;
		this.cachedObject.Object.text = value.ToString();
	}

	// Token: 0x040009C8 RID: 2504
	private CachedObject<Text> cachedObject;

	// Token: 0x040009C9 RID: 2505
	private T cachedValue;
}
