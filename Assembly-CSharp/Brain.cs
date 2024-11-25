using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	// Token: 0x06001676 RID: 5750 RVA: 0x00078B2B File Offset: 0x00076D2B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x00078B33 File Offset: 0x00076D33
	protected override void OnSpawn()
	{
		this.prefabId = base.GetComponent<KPrefabID>();
		this.choreConsumer = base.GetComponent<ChoreConsumer>();
		this.running = true;
		Components.Brains.Add(this);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001678 RID: 5752 RVA: 0x00078B60 File Offset: 0x00076D60
	// (remove) Token: 0x06001679 RID: 5753 RVA: 0x00078B98 File Offset: 0x00076D98
	public event System.Action onPreUpdate;

	// Token: 0x0600167A RID: 5754 RVA: 0x00078BCD File Offset: 0x00076DCD
	public virtual void UpdateBrain()
	{
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (this.IsRunning())
		{
			this.UpdateChores();
		}
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x00078BF0 File Offset: 0x00076DF0
	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return this.choreConsumer.FindNextChore(ref context);
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x00078C00 File Offset: 0x00076E00
	private void UpdateChores()
	{
		if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context chore = default(Chore.Precondition.Context);
		if (this.FindBetterChore(ref chore))
		{
			if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				base.Trigger(1485595942, null);
				return;
			}
			this.choreConsumer.choreDriver.SetChore(chore);
		}
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x00078C62 File Offset: 0x00076E62
	public bool IsRunning()
	{
		return this.running && !this.suspend;
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x00078C77 File Offset: 0x00076E77
	public void Reset(string reason)
	{
		this.Stop("Reset");
		this.running = true;
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x00078C8B File Offset: 0x00076E8B
	public void Stop(string reason)
	{
		base.GetComponent<ChoreDriver>().StopChore();
		this.running = false;
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x00078C9F File Offset: 0x00076E9F
	public void Resume(string caller)
	{
		this.suspend = false;
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x00078CA8 File Offset: 0x00076EA8
	public void Suspend(string caller)
	{
		this.suspend = true;
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x00078CB1 File Offset: 0x00076EB1
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.Stop("OnCmpDisable");
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x00078CC4 File Offset: 0x00076EC4
	protected override void OnCleanUp()
	{
		this.Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}

	// Token: 0x04000C93 RID: 3219
	private bool running;

	// Token: 0x04000C94 RID: 3220
	private bool suspend;

	// Token: 0x04000C95 RID: 3221
	protected KPrefabID prefabId;

	// Token: 0x04000C96 RID: 3222
	protected ChoreConsumer choreConsumer;
}
