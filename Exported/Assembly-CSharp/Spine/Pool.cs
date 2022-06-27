using System;
using System.Collections.Generic;

namespace Spine
{
	// Token: 0x02000189 RID: 393
	public class Pool<T> where T : class, new()
	{
		// Token: 0x06000B9E RID: 2974 RVA: 0x0005A608 File Offset: 0x00058808
		public Pool(int initialCapacity = 16, int max = 2147483647)
		{
			this.freeObjects = new Stack<T>(initialCapacity);
			this.max = max;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0005A624 File Offset: 0x00058824
		public int Count
		{
			get
			{
				return this.freeObjects.Count;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0005A634 File Offset: 0x00058834
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0005A63C File Offset: 0x0005883C
		public int Peak { get; private set; }

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0005A648 File Offset: 0x00058848
		public T Obtain()
		{
			return (this.freeObjects.Count != 0) ? this.freeObjects.Pop() : Activator.CreateInstance<T>();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0005A670 File Offset: 0x00058870
		public void Free(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "obj cannot be null");
			}
			if (this.freeObjects.Count < this.max)
			{
				this.freeObjects.Push(obj);
				this.Peak = Math.Max(this.Peak, this.freeObjects.Count);
			}
			this.Reset(obj);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0005A6E0 File Offset: 0x000588E0
		public void Clear()
		{
			this.freeObjects.Clear();
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0005A6F0 File Offset: 0x000588F0
		protected void Reset(T obj)
		{
			Pool<T>.IPoolable poolable = obj as Pool<T>.IPoolable;
			if (poolable != null)
			{
				poolable.Reset();
			}
		}

		// Token: 0x04000AC3 RID: 2755
		public readonly int max;

		// Token: 0x04000AC4 RID: 2756
		private readonly Stack<T> freeObjects;

		// Token: 0x0200018A RID: 394
		public interface IPoolable
		{
			// Token: 0x06000BA6 RID: 2982
			void Reset();
		}
	}
}
