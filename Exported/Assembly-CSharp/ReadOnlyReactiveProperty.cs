using System;

// Token: 0x02000169 RID: 361
public class ReadOnlyReactiveProperty<T> : IDisposable where T : IEquatable<T>, new()
{
	// Token: 0x06000A62 RID: 2658 RVA: 0x00054144 File Offset: 0x00052344
	public ReadOnlyReactiveProperty(ReactiveProperty<T> parent)
	{
		this.parent = parent;
		this.parent += new ReactiveProperty<T>.Changed(this.OnParentChanged);
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000A63 RID: 2659 RVA: 0x0005417C File Offset: 0x0005237C
	// (remove) Token: 0x06000A64 RID: 2660 RVA: 0x00054198 File Offset: 0x00052398
	private event ReadOnlyReactiveProperty<T>.Changed valueChanged;

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000541B4 File Offset: 0x000523B4
	public T Value
	{
		get
		{
			return this.parent.Value;
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000541C4 File Offset: 0x000523C4
	public void Dispose()
	{
		if (this.parent != null)
		{
			this.parent -= new ReactiveProperty<T>.Changed(this.OnParentChanged);
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x000541FC File Offset: 0x000523FC
	private void OnParentChanged(T value)
	{
		if (this.valueChanged != null)
		{
			this.valueChanged(value);
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x00054218 File Offset: 0x00052418
	public void AddLateListener(ReadOnlyReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReadOnlyReactiveProperty<T>.Changed)Delegate.Combine(this.valueChanged, e);
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00054234 File Offset: 0x00052434
	public void RemoveLateListener(ReadOnlyReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReadOnlyReactiveProperty<T>.Changed)Delegate.Remove(this.valueChanged, e);
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00054250 File Offset: 0x00052450
	public void RemoveAllListeners()
	{
		this.valueChanged = null;
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0005425C File Offset: 0x0005245C
	public override string ToString()
	{
		T value = this.Value;
		return value.ToString();
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x00054280 File Offset: 0x00052480
	public static ReadOnlyReactiveProperty<T>operator +(ReadOnlyReactiveProperty<T> c1, ReadOnlyReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReadOnlyReactiveProperty<T>.Changed)Delegate.Combine(c1.valueChanged, c2);
		c2(c1.parent.Value);
		return c1;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000542AC File Offset: 0x000524AC
	public static ReadOnlyReactiveProperty<T>operator -(ReadOnlyReactiveProperty<T> c1, ReadOnlyReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReadOnlyReactiveProperty<T>.Changed)Delegate.Remove(c1.valueChanged, c2);
		return c1;
	}

	// Token: 0x040009DC RID: 2524
	private ReactiveProperty<T> parent;

	// Token: 0x0200023B RID: 571
	// (Invoke) Token: 0x060011F0 RID: 4592
	public delegate void Changed(T prop);
}
