using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class CachedObject<T> where T : Component
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x00053888 File Offset: 0x00051A88
	public CachedObject(GameObject parent, string path)
	{
		this.parent = parent;
		this.path = path;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x000538A0 File Offset: 0x00051AA0
	public CachedObject(T obj)
	{
		this.obj = obj;
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x000538B0 File Offset: 0x00051AB0
	public T Object
	{
		get
		{
			if (this.obj == null)
			{
				Transform transform = this.parent.transform.Find(this.path);
				this.obj = transform.GetComponent<T>();
			}
			return this.obj;
		}
	}

	// Token: 0x040009C5 RID: 2501
	private T obj;

	// Token: 0x040009C6 RID: 2502
	private string path;

	// Token: 0x040009C7 RID: 2503
	private GameObject parent;
}
