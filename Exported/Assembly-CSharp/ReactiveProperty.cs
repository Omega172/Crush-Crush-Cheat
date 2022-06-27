using System;

// Token: 0x02000168 RID: 360
public class ReactiveProperty<T> where T : IEquatable<T>, new()
{
	// Token: 0x06000A54 RID: 2644 RVA: 0x00053F8C File Offset: 0x0005218C
	public ReactiveProperty() : this(default(T))
	{
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00053FA8 File Offset: 0x000521A8
	public ReactiveProperty(T initialValue)
	{
		this.value = initialValue;
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000A56 RID: 2646 RVA: 0x00053FB8 File Offset: 0x000521B8
	// (remove) Token: 0x06000A57 RID: 2647 RVA: 0x00053FD4 File Offset: 0x000521D4
	private event ReactiveProperty<T>.Changed valueChanged;

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00053FF0 File Offset: 0x000521F0
	// (set) Token: 0x06000A59 RID: 2649 RVA: 0x00053FF8 File Offset: 0x000521F8
	public T Value
	{
		get
		{
			return this.value;
		}
		set
		{
			if ((this.value == null && value != null) || !this.value.Equals(value))
			{
				this.value = value;
				if (this.valueChanged != null)
				{
					this.valueChanged(this.value);
				}
			}
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0005405C File Offset: 0x0005225C
	public void Force(T value)
	{
		this.value = value;
		if (this.valueChanged != null)
		{
			this.valueChanged(this.value);
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00054084 File Offset: 0x00052284
	public void AddLateListener(ReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Combine(this.valueChanged, e);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000540A0 File Offset: 0x000522A0
	public void RemoveLateListener(ReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Remove(this.valueChanged, e);
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000540BC File Offset: 0x000522BC
	public void RemoveAllListeners()
	{
		this.valueChanged = null;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x000540C8 File Offset: 0x000522C8
	public ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty()
	{
		return new ReadOnlyReactiveProperty<T>(this);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x000540D0 File Offset: 0x000522D0
	public override string ToString()
	{
		T t = this.Value;
		return t.ToString();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x000540F4 File Offset: 0x000522F4
	public static ReactiveProperty<T>operator +(ReactiveProperty<T> c1, ReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Combine(c1.valueChanged, c2);
		c2(c1.value);
		return c1;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00054128 File Offset: 0x00052328
	public static ReactiveProperty<T>operator -(ReactiveProperty<T> c1, ReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Remove(c1.valueChanged, c2);
		return c1;
	}

	// Token: 0x040009DA RID: 2522
	private T value;

	// Token: 0x0200023A RID: 570
	// (Invoke) Token: 0x060011EC RID: 4588
	public delegate void Changed(T prop);
}
