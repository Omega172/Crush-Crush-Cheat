using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spine
{
	// Token: 0x020001AD RID: 429
	[DebuggerDisplay("Count={Count}")]
	public class ExposedList<T> : IEnumerable, IEnumerable<T>
	{
		// Token: 0x06000D14 RID: 3348 RVA: 0x0005E188 File Offset: 0x0005C388
		public ExposedList()
		{
			this.Items = ExposedList<T>.EmptyArray;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0005E19C File Offset: 0x0005C39C
		public ExposedList(IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 == null)
			{
				this.Items = ExposedList<T>.EmptyArray;
				this.AddEnumerable(collection);
			}
			else
			{
				this.Items = new T[collection2.Count];
				this.AddCollection(collection2);
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0005E1F4 File Offset: 0x0005C3F4
		public ExposedList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.Items = new T[capacity];
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0005E228 File Offset: 0x0005C428
		internal ExposedList(T[] data, int size)
		{
			this.Items = data;
			this.Count = size;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005E250 File Offset: 0x0005C450
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005E260 File Offset: 0x0005C460
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0005E270 File Offset: 0x0005C470
		public void Add(T item)
		{
			if (this.Count == this.Items.Length)
			{
				this.GrowIfNeeded(1);
			}
			this.Items[this.Count++] = item;
			this.version++;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0005E2C4 File Offset: 0x0005C4C4
		public void GrowIfNeeded(int addedCount)
		{
			int num = this.Count + addedCount;
			if (num > this.Items.Length)
			{
				this.Capacity = Math.Max(Math.Max(this.Capacity * 2, 4), num);
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0005E304 File Offset: 0x0005C504
		public ExposedList<T> Resize(int newSize)
		{
			int num = this.Items.Length;
			T[] items = this.Items;
			if (newSize > num)
			{
				Array.Resize<T>(ref this.Items, newSize);
			}
			else if (newSize < num)
			{
				for (int i = newSize; i < num; i++)
				{
					items[i] = default(T);
				}
			}
			this.Count = newSize;
			return this;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0005E36C File Offset: 0x0005C56C
		public void EnsureCapacity(int min)
		{
			if (this.Items.Length < min)
			{
				int num = (this.Items.Length != 0) ? (this.Items.Length * 2) : 4;
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0005E3B8 File Offset: 0x0005C5B8
		private void CheckRange(int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > this.Count)
			{
				throw new ArgumentException("index and count exceed length of list");
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0005E3F8 File Offset: 0x0005C5F8
		private void AddCollection(ICollection<T> collection)
		{
			int count = collection.Count;
			if (count == 0)
			{
				return;
			}
			this.GrowIfNeeded(count);
			collection.CopyTo(this.Items, this.Count);
			this.Count += count;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005E43C File Offset: 0x0005C63C
		private void AddEnumerable(IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				this.Add(item);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0005E49C File Offset: 0x0005C69C
		public void AddRange(ExposedList<T> list)
		{
			this.CheckCollection(list);
			int count = list.Count;
			if (count == 0)
			{
				return;
			}
			this.GrowIfNeeded(count);
			list.CopyTo(this.Items, this.Count);
			this.Count += count;
			this.version++;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0005E4F4 File Offset: 0x0005C6F4
		public void AddRange(IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				this.AddCollection(collection2);
			}
			else
			{
				this.AddEnumerable(collection);
			}
			this.version++;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0005E538 File Offset: 0x0005C738
		public int BinarySearch(T item)
		{
			return Array.BinarySearch<T>(this.Items, 0, this.Count, item);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0005E550 File Offset: 0x0005C750
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return Array.BinarySearch<T>(this.Items, 0, this.Count, item, comparer);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0005E568 File Offset: 0x0005C768
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			this.CheckRange(index, count);
			return Array.BinarySearch<T>(this.Items, index, count, item, comparer);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0005E584 File Offset: 0x0005C784
		public void Clear(bool clearArray = true)
		{
			if (clearArray)
			{
				Array.Clear(this.Items, 0, this.Items.Length);
			}
			this.Count = 0;
			this.version++;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0005E5B8 File Offset: 0x0005C7B8
		public bool Contains(T item)
		{
			return Array.IndexOf<T>(this.Items, item, 0, this.Count) != -1;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0005E5D4 File Offset: 0x0005C7D4
		public ExposedList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			ExposedList<TOutput> exposedList = new ExposedList<TOutput>(this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				exposedList.Items[i] = converter(this.Items[i]);
			}
			exposedList.Count = this.Count;
			return exposedList;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0005E640 File Offset: 0x0005C840
		public void CopyTo(T[] array)
		{
			Array.Copy(this.Items, 0, array, 0, this.Count);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005E658 File Offset: 0x0005C858
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.Items, 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0005E670 File Offset: 0x0005C870
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			this.CheckRange(index, count);
			Array.Copy(this.Items, index, array, arrayIndex, count);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005E68C File Offset: 0x0005C88C
		public bool Exists(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetIndex(0, this.Count, match) != -1;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0005E6A8 File Offset: 0x0005C8A8
		public T Find(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int index = this.GetIndex(0, this.Count, match);
			return (index == -1) ? default(T) : this.Items[index];
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0005E6EC File Offset: 0x0005C8EC
		private static void CheckMatch(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0005E700 File Offset: 0x0005C900
		public ExposedList<T> FindAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.FindAllList(match);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0005E710 File Offset: 0x0005C910
		private ExposedList<T> FindAllList(Predicate<T> match)
		{
			ExposedList<T> exposedList = new ExposedList<T>();
			for (int i = 0; i < this.Count; i++)
			{
				if (match(this.Items[i]))
				{
					exposedList.Add(this.Items[i]);
				}
			}
			return exposedList;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0005E764 File Offset: 0x0005C964
		public int FindIndex(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetIndex(0, this.Count, match);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0005E77C File Offset: 0x0005C97C
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckIndex(startIndex);
			return this.GetIndex(startIndex, this.Count - startIndex, match);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0005E79C File Offset: 0x0005C99C
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckRange(startIndex, count);
			return this.GetIndex(startIndex, count, match);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0005E7B8 File Offset: 0x0005C9B8
		private int GetIndex(int startIndex, int count, Predicate<T> match)
		{
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this.Items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0005E7F8 File Offset: 0x0005C9F8
		public T FindLast(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int lastIndex = this.GetLastIndex(0, this.Count, match);
			return (lastIndex != -1) ? this.Items[lastIndex] : default(T);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0005E83C File Offset: 0x0005CA3C
		public int FindLastIndex(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			return this.GetLastIndex(0, this.Count, match);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0005E854 File Offset: 0x0005CA54
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			this.CheckIndex(startIndex);
			return this.GetLastIndex(0, startIndex + 1, match);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0005E870 File Offset: 0x0005CA70
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int num = startIndex - count + 1;
			this.CheckRange(num, count);
			return this.GetLastIndex(num, count, match);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0005E89C File Offset: 0x0005CA9C
		private int GetLastIndex(int startIndex, int count, Predicate<T> match)
		{
			int num = startIndex + count;
			while (num != startIndex)
			{
				if (match(this.Items[--num]))
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0005E8D8 File Offset: 0x0005CAD8
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			for (int i = 0; i < this.Count; i++)
			{
				action(this.Items[i]);
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0005E920 File Offset: 0x0005CB20
		public ExposedList<T>.Enumerator GetEnumerator()
		{
			return new ExposedList<T>.Enumerator(this);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0005E928 File Offset: 0x0005CB28
		public ExposedList<T> GetRange(int index, int count)
		{
			this.CheckRange(index, count);
			T[] array = new T[count];
			Array.Copy(this.Items, index, array, 0, count);
			return new ExposedList<T>(array, count);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0005E95C File Offset: 0x0005CB5C
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this.Items, item, 0, this.Count);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0005E974 File Offset: 0x0005CB74
		public int IndexOf(T item, int index)
		{
			this.CheckIndex(index);
			return Array.IndexOf<T>(this.Items, item, index, this.Count - index);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0005E994 File Offset: 0x0005CB94
		public int IndexOf(T item, int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > this.Count)
			{
				throw new ArgumentOutOfRangeException("index and count exceed length of list");
			}
			return Array.IndexOf<T>(this.Items, item, index, count);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0005E9EC File Offset: 0x0005CBEC
		private void Shift(int start, int delta)
		{
			if (delta < 0)
			{
				start -= delta;
			}
			if (start < this.Count)
			{
				Array.Copy(this.Items, start, this.Items, start + delta, this.Count - start);
			}
			this.Count += delta;
			if (delta < 0)
			{
				Array.Clear(this.Items, this.Count, -delta);
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0005EA58 File Offset: 0x0005CC58
		private void CheckIndex(int index)
		{
			if (index < 0 || index > this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0005EA78 File Offset: 0x0005CC78
		public void Insert(int index, T item)
		{
			this.CheckIndex(index);
			if (this.Count == this.Items.Length)
			{
				this.GrowIfNeeded(1);
			}
			this.Shift(index, 1);
			this.Items[index] = item;
			this.version++;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0005EACC File Offset: 0x0005CCCC
		private void CheckCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0005EAE0 File Offset: 0x0005CCE0
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			this.CheckCollection(collection);
			this.CheckIndex(index);
			if (collection == this)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				this.GrowIfNeeded(this.Count);
				this.Shift(index, array.Length);
				Array.Copy(array, 0, this.Items, index, array.Length);
			}
			else
			{
				ICollection<T> collection2 = collection as ICollection<T>;
				if (collection2 != null)
				{
					this.InsertCollection(index, collection2);
				}
				else
				{
					this.InsertEnumeration(index, collection);
				}
			}
			this.version++;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0005EB74 File Offset: 0x0005CD74
		private void InsertCollection(int index, ICollection<T> collection)
		{
			int count = collection.Count;
			this.GrowIfNeeded(count);
			this.Shift(index, count);
			collection.CopyTo(this.Items, index);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0005EBA4 File Offset: 0x0005CDA4
		private void InsertEnumeration(int index, IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				this.Insert(index++, item);
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0005EC08 File Offset: 0x0005CE08
		public int LastIndexOf(T item)
		{
			return Array.LastIndexOf<T>(this.Items, item, this.Count - 1, this.Count);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0005EC24 File Offset: 0x0005CE24
		public int LastIndexOf(T item, int index)
		{
			this.CheckIndex(index);
			return Array.LastIndexOf<T>(this.Items, item, index, index + 1);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0005EC40 File Offset: 0x0005CE40
		public int LastIndexOf(T item, int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", index, "index is negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "count is negative");
			}
			if (index - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "count is too large");
			}
			return Array.LastIndexOf<T>(this.Items, item, index, count);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0005ECB8 File Offset: 0x0005CEB8
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
			return num != -1;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0005ECE4 File Offset: 0x0005CEE4
		public int RemoveAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			int i;
			for (i = 0; i < this.Count; i++)
			{
				if (match(this.Items[i]))
				{
					break;
				}
			}
			if (i == this.Count)
			{
				return 0;
			}
			this.version++;
			int j;
			for (j = i + 1; j < this.Count; j++)
			{
				if (!match(this.Items[j]))
				{
					this.Items[i++] = this.Items[j];
				}
			}
			if (j - i > 0)
			{
				Array.Clear(this.Items, i, j - i);
			}
			this.Count = i;
			return j - i;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0005EDB8 File Offset: 0x0005CFB8
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Shift(index, -1);
			Array.Clear(this.Items, this.Count, 1);
			this.version++;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0005EE0C File Offset: 0x0005D00C
		public T Pop()
		{
			if (this.Count == 0)
			{
				throw new InvalidOperationException("List is empty. Nothing to pop.");
			}
			int num = this.Count - 1;
			T result = this.Items[num];
			this.Items[num] = default(T);
			this.Count--;
			this.version++;
			return result;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0005EE78 File Offset: 0x0005D078
		public void RemoveRange(int index, int count)
		{
			this.CheckRange(index, count);
			if (count > 0)
			{
				this.Shift(index, -count);
				Array.Clear(this.Items, this.Count, count);
				this.version++;
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0005EEC0 File Offset: 0x0005D0C0
		public void Reverse()
		{
			Array.Reverse(this.Items, 0, this.Count);
			this.version++;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0005EEF0 File Offset: 0x0005D0F0
		public void Reverse(int index, int count)
		{
			this.CheckRange(index, count);
			Array.Reverse(this.Items, index, count);
			this.version++;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0005EF18 File Offset: 0x0005D118
		public void Sort()
		{
			Array.Sort<T>(this.Items, 0, this.Count, Comparer<T>.Default);
			this.version++;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0005EF40 File Offset: 0x0005D140
		public void Sort(IComparer<T> comparer)
		{
			Array.Sort<T>(this.Items, 0, this.Count, comparer);
			this.version++;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0005EF64 File Offset: 0x0005D164
		public void Sort(Comparison<T> comparison)
		{
			Array.Sort<T>(this.Items, comparison);
			this.version++;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0005EF80 File Offset: 0x0005D180
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			this.CheckRange(index, count);
			Array.Sort<T>(this.Items, index, count, comparer);
			this.version++;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0005EFB4 File Offset: 0x0005D1B4
		public T[] ToArray()
		{
			T[] array = new T[this.Count];
			Array.Copy(this.Items, array, this.Count);
			return array;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0005EFE0 File Offset: 0x0005D1E0
		public void TrimExcess()
		{
			this.Capacity = this.Count;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0005EFF0 File Offset: 0x0005D1F0
		public bool TrueForAll(Predicate<T> match)
		{
			ExposedList<T>.CheckMatch(match);
			for (int i = 0; i < this.Count; i++)
			{
				if (!match(this.Items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0005F034 File Offset: 0x0005D234
		// (set) Token: 0x06000D5A RID: 3418 RVA: 0x0005F040 File Offset: 0x0005D240
		public int Capacity
		{
			get
			{
				return this.Items.Length;
			}
			set
			{
				if (value < this.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				Array.Resize<T>(ref this.Items, value);
			}
		}

		// Token: 0x04000B9D RID: 2973
		private const int DefaultCapacity = 4;

		// Token: 0x04000B9E RID: 2974
		public T[] Items;

		// Token: 0x04000B9F RID: 2975
		public int Count;

		// Token: 0x04000BA0 RID: 2976
		private static readonly T[] EmptyArray = new T[0];

		// Token: 0x04000BA1 RID: 2977
		private int version;

		// Token: 0x020001AE RID: 430
		public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T>
		{
			// Token: 0x06000D5B RID: 3419 RVA: 0x0005F060 File Offset: 0x0005D260
			internal Enumerator(ExposedList<T> l)
			{
				this.l = l;
				this.ver = l.version;
			}

			// Token: 0x06000D5C RID: 3420 RVA: 0x0005F078 File Offset: 0x0005D278
			void IEnumerator.Reset()
			{
				this.VerifyState();
				this.next = 0;
			}

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x06000D5D RID: 3421 RVA: 0x0005F088 File Offset: 0x0005D288
			object IEnumerator.Current
			{
				get
				{
					this.VerifyState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException();
					}
					return this.current;
				}
			}

			// Token: 0x06000D5E RID: 3422 RVA: 0x0005F0B0 File Offset: 0x0005D2B0
			public void Dispose()
			{
				this.l = null;
			}

			// Token: 0x06000D5F RID: 3423 RVA: 0x0005F0BC File Offset: 0x0005D2BC
			private void VerifyState()
			{
				if (this.l == null)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.ver != this.l.version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
			}

			// Token: 0x06000D60 RID: 3424 RVA: 0x0005F110 File Offset: 0x0005D310
			public bool MoveNext()
			{
				this.VerifyState();
				if (this.next < 0)
				{
					return false;
				}
				if (this.next < this.l.Count)
				{
					this.current = this.l.Items[this.next++];
					return true;
				}
				this.next = -1;
				return false;
			}

			// Token: 0x17000235 RID: 565
			// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0005F178 File Offset: 0x0005D378
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x04000BA2 RID: 2978
			private ExposedList<T> l;

			// Token: 0x04000BA3 RID: 2979
			private int next;

			// Token: 0x04000BA4 RID: 2980
			private int ver;

			// Token: 0x04000BA5 RID: 2981
			private T current;
		}
	}
}
