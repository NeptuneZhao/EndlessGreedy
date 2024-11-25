using System;
using System.Collections;

// Token: 0x02000414 RID: 1044
public class Promise : IEnumerator
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06001622 RID: 5666 RVA: 0x00077F40 File Offset: 0x00076140
	public bool IsResolved
	{
		get
		{
			return this.m_is_resolved;
		}
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x00077F48 File Offset: 0x00076148
	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			this.Resolve();
		});
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00077F62 File Offset: 0x00076162
	public Promise()
	{
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x00077F6A File Offset: 0x0007616A
	public void EnsureResolved()
	{
		if (this.IsResolved)
		{
			return;
		}
		this.Resolve();
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x00077F7B File Offset: 0x0007617B
	public void Resolve()
	{
		DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
		this.m_is_resolved = true;
		if (this.on_complete != null)
		{
			this.on_complete();
			this.on_complete = null;
		}
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00077FB1 File Offset: 0x000761B1
	public Promise Then(System.Action callback)
	{
		if (this.m_is_resolved)
		{
			callback();
		}
		else
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, callback);
		}
		return this;
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00077FDC File Offset: 0x000761DC
	public Promise ThenWait(Func<Promise> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00078024 File Offset: 0x00076224
	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600162A RID: 5674 RVA: 0x0007806A File Offset: 0x0007626A
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x0007806D File Offset: 0x0007626D
	bool IEnumerator.MoveNext()
	{
		return !this.IsResolved;
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x00078078 File Offset: 0x00076278
	void IEnumerator.Reset()
	{
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x0007807A File Offset: 0x0007627A
	static Promise()
	{
		Promise.m_instant.Resolve();
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600162E RID: 5678 RVA: 0x00078090 File Offset: 0x00076290
	public static Promise Instant
	{
		get
		{
			return Promise.m_instant;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600162F RID: 5679 RVA: 0x00078097 File Offset: 0x00076297
	public static Promise Fail
	{
		get
		{
			return new Promise();
		}
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x000780A0 File Offset: 0x000762A0
	public static Promise All(params Promise[] promises)
	{
		Promise.<>c__DisplayClass21_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass21_0();
		CS$<>8__locals1.promises = promises;
		if (CS$<>8__locals1.promises == null || CS$<>8__locals1.promises.Length == 0)
		{
			return Promise.Instant;
		}
		CS$<>8__locals1.all_resolved_promise = new Promise();
		Promise[] promises2 = CS$<>8__locals1.promises;
		for (int i = 0; i < promises2.Length; i++)
		{
			promises2[i].Then(new System.Action(CS$<>8__locals1.<All>g__TryResolve|0));
		}
		return CS$<>8__locals1.all_resolved_promise;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0007810C File Offset: 0x0007630C
	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise.<>c__DisplayClass22_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass22_0();
		CS$<>8__locals1.make_promise_fns = make_promise_fns;
		CS$<>8__locals1.all_resolve_promise = new Promise();
		CS$<>8__locals1.current_promise_fn_index = 0;
		CS$<>8__locals1.<Chain>g__TryNext|0();
		return CS$<>8__locals1.all_resolve_promise;
	}

	// Token: 0x04000C7F RID: 3199
	private System.Action on_complete;

	// Token: 0x04000C80 RID: 3200
	private bool m_is_resolved;

	// Token: 0x04000C81 RID: 3201
	private static Promise m_instant = new Promise();
}
