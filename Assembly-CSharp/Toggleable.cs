using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
[AddComponentMenu("KMonoBehaviour/Workable/Toggleable")]
public class Toggleable : Workable
{
	// Token: 0x0600243F RID: 9279 RVA: 0x000CA283 File Offset: 0x000C8483
	protected Toggleable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000CA298 File Offset: 0x000C8498
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.targets = new List<KeyValuePair<IToggleHandler, Chore>>();
		base.SetWorkTime(3f);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Toggling;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000CA302 File Offset: 0x000C8502
	public int SetTarget(IToggleHandler handler)
	{
		this.targets.Add(new KeyValuePair<IToggleHandler, Chore>(handler, null));
		return this.targets.Count - 1;
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x000CA324 File Offset: 0x000C8524
	public IToggleHandler GetToggleHandlerForWorker(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1)
		{
			return this.targets[targetForWorker].Key;
		}
		return null;
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x000CA354 File Offset: 0x000C8554
	private int GetTargetForWorker(WorkerBase worker)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Value != null && this.targets[i].Value.driver != null && this.targets[i].Value.driver.gameObject == worker.gameObject)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x000CA3DC File Offset: 0x000C85DC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1 && this.targets[targetForWorker].Key != null)
		{
			this.targets[targetForWorker] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetForWorker].Key, null);
			this.targets[targetForWorker].Key.HandleToggle();
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x000CA468 File Offset: 0x000C8668
	private void QueueToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value == null)
		{
			if (DebugHandler.InstantBuildMode)
			{
				this.targets[targetIdx].Key.HandleToggle();
				return;
			}
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, new WorkChore<Toggleable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true));
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x000CA518 File Offset: 0x000C8718
	public void Toggle(int targetIdx)
	{
		if (targetIdx >= this.targets.Count)
		{
			return;
		}
		if (this.targets[targetIdx].Value == null)
		{
			this.QueueToggle(targetIdx);
			return;
		}
		this.CancelToggle(targetIdx);
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x000CA55C File Offset: 0x000C875C
	private void CancelToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value != null)
		{
			this.targets[targetIdx].Value.Cancel("Toggle cancelled");
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000CA5E0 File Offset: 0x000C87E0
	public bool IsToggleQueued(int targetIdx)
	{
		return this.targets[targetIdx].Value != null;
	}

	// Token: 0x040014AB RID: 5291
	private List<KeyValuePair<IToggleHandler, Chore>> targets;
}
