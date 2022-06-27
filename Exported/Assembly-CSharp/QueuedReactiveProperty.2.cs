using System;

// Token: 0x02000167 RID: 359
public class QueuedReactiveProperty<T> : QueuedReactiveProperty, IDisposable where T : IEquatable<T>, new()
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x00053D9C File Offset: 0x00051F9C
	public QueuedReactiveProperty() : this(default(T))
	{
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00053DB8 File Offset: 0x00051FB8
	public QueuedReactiveProperty(T initialValue)
	{
		this.value = initialValue;
		QueuedReactiveProperty.queuedReactiveProperties.Add(this);
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000A47 RID: 2631 RVA: 0x00053DD4 File Offset: 0x00051FD4
	// (remove) Token: 0x06000A48 RID: 2632 RVA: 0x00053DF0 File Offset: 0x00051FF0
	private event ReactiveProperty<T>.Changed valueChanged;

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00053E0C File Offset: 0x0005200C
	// (set) Token: 0x06000A4A RID: 2634 RVA: 0x00053E14 File Offset: 0x00052014
	public T Value
	{
		get
		{
			return this.value;
		}
		set
		{
			if ((this.value == null && value != null) || (this.value != null && !this.value.Equals(value)))
			{
				this.value = value;
				this.dirty = true;
			}
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00053E74 File Offset: 0x00052074
	public void AddLateListener(ReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Combine(this.valueChanged, e);
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00053E90 File Offset: 0x00052090
	public void RemoveLateListener(ReactiveProperty<T>.Changed e)
	{
		this.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Remove(this.valueChanged, e);
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00053EAC File Offset: 0x000520AC
	public void RemoveAllListeners()
	{
		this.valueChanged = null;
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x00053EB8 File Offset: 0x000520B8
	protected override void Invoke()
	{
		if (this.valueChanged != null)
		{
			this.valueChanged(this.value);
		}
		this.dirty = false;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00053EE0 File Offset: 0x000520E0
	protected virtual void Dispose(bool disposing)
	{
		if (this.disposed)
		{
			return;
		}
		if (disposing)
		{
			QueuedReactiveProperty.queuedReactiveProperties.Remove(this);
		}
		this.disposed = true;
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00053F08 File Offset: 0x00052108
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00053F18 File Offset: 0x00052118
	public override string ToString()
	{
		T t = this.Value;
		return t.ToString();
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00053F3C File Offset: 0x0005213C
	public static QueuedReactiveProperty<T>operator +(QueuedReactiveProperty<T> c1, ReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Combine(c1.valueChanged, c2);
		c2(c1.value);
		return c1;
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00053F70 File Offset: 0x00052170
	public static QueuedReactiveProperty<T>operator -(QueuedReactiveProperty<T> c1, ReactiveProperty<T>.Changed c2)
	{
		c1.valueChanged = (ReactiveProperty<T>.Changed)Delegate.Remove(c1.valueChanged, c2);
		return c1;
	}

	// Token: 0x040009D7 RID: 2519
	private bool disposed;

	// Token: 0x040009D8 RID: 2520
	private T value;
}
