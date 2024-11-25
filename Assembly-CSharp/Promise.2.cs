using System;
using System.Collections;

// Token: 0x02000415 RID: 1045
public class Promise<T> : IEnumerator
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06001633 RID: 5683 RVA: 0x0007813F File Offset: 0x0007633F
	public bool IsResolved
	{
		get
		{
			return this.promise.IsResolved;
		}
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x0007814C File Offset: 0x0007634C
	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			this.Resolve(value);
		});
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x00078171 File Offset: 0x00076371
	public Promise()
	{
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00078184 File Offset: 0x00076384
	public void EnsureResolved(T value)
	{
		this.result = value;
		this.promise.EnsureResolved();
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00078198 File Offset: 0x00076398
	public void Resolve(T value)
	{
		this.result = value;
		this.promise.Resolve();
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000781AC File Offset: 0x000763AC
	public Promise<T> Then(Action<T> fn)
	{
		this.promise.Then(delegate
		{
			fn(this.result);
		});
		return this;
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x000781E6 File Offset: 0x000763E6
	public Promise ThenWait(Func<Promise> fn)
	{
		return this.promise.ThenWait(fn);
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x000781F4 File Offset: 0x000763F4
	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return this.promise.ThenWait<T>(fn);
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600163B RID: 5691 RVA: 0x00078202 File Offset: 0x00076402
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00078205 File Offset: 0x00076405
	bool IEnumerator.MoveNext()
	{
		return !this.promise.IsResolved;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x00078215 File Offset: 0x00076415
	void IEnumerator.Reset()
	{
	}

	// Token: 0x04000C82 RID: 3202
	private Promise promise = new Promise();

	// Token: 0x04000C83 RID: 3203
	private T result;
}
