using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class StatusItemStackTraceWatcher : IDisposable
{
	// Token: 0x060026DA RID: 9946 RVA: 0x000DD2AE File Offset: 0x000DB4AE
	public bool GetShouldWatch()
	{
		return this.shouldWatch;
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x000DD2B6 File Offset: 0x000DB4B6
	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch == shouldWatch)
		{
			return;
		}
		this.shouldWatch = shouldWatch;
		this.Refresh();
	}

	// Token: 0x060026DC RID: 9948 RVA: 0x000DD2CF File Offset: 0x000DB4CF
	public Option<StatusItemGroup> GetTarget()
	{
		return this.currentTarget;
	}

	// Token: 0x060026DD RID: 9949 RVA: 0x000DD2D8 File Offset: 0x000DB4D8
	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if (this.currentTarget.IsNone() && nextTarget.IsNone())
		{
			return;
		}
		if (this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
		{
			return;
		}
		this.currentTarget = nextTarget;
		this.Refresh();
	}

	// Token: 0x060026DE RID: 9950 RVA: 0x000DD334 File Offset: 0x000DB534
	private void Refresh()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
		if (!this.shouldWatch)
		{
			return;
		}
		if (this.currentTarget.IsSome())
		{
			StatusItemGroup target = this.currentTarget.Unwrap();
			Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
			{
				this.entryIdToStackTraceMap[entry.id] = new StackTrace(true);
			};
			StatusItemGroup target3 = target;
			target3.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(target3.OnAddStatusItem, onAddStatusItem);
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				StatusItemGroup target2 = target;
				target2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(target2.OnAddStatusItem, onAddStatusItem);
			}));
			StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
			destroyListener.owner = this;
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				if (destroyListener.IsNullOrDestroyed())
				{
					return;
				}
				UnityEngine.Object.Destroy(destroyListener);
			}));
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				this.entryIdToStackTraceMap.Clear();
			}));
		}
	}

	// Token: 0x060026DF RID: 9951 RVA: 0x000DD451 File Offset: 0x000DB651
	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x000DD465 File Offset: 0x000DB665
	public void Dispose()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
	}

	// Token: 0x0400164F RID: 5711
	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	// Token: 0x04001650 RID: 5712
	private Option<StatusItemGroup> currentTarget;

	// Token: 0x04001651 RID: 5713
	private bool shouldWatch;

	// Token: 0x04001652 RID: 5714
	private System.Action onCleanup;

	// Token: 0x02001409 RID: 5129
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		// Token: 0x06008928 RID: 35112 RVA: 0x0032FB48 File Offset: 0x0032DD48
		private void OnDestroy()
		{
			bool flag = this.owner != null;
			bool flag2 = this.owner.currentTarget.IsSome() && this.owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (flag && flag2)
			{
				this.owner.SetTarget(Option.None);
			}
		}

		// Token: 0x040068B3 RID: 26803
		public StatusItemStackTraceWatcher owner;
	}
}
