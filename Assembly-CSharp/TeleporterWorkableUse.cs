using System;

// Token: 0x02000782 RID: 1922
public class TeleporterWorkableUse : Workable
{
	// Token: 0x06003440 RID: 13376 RVA: 0x0011D3F6 File Offset: 0x0011B5F6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x0011D3FE File Offset: 0x0011B5FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x0011D418 File Offset: 0x0011B618
	protected override void OnStartWork(WorkerBase worker)
	{
		Teleporter component = base.GetComponent<Teleporter>();
		Teleporter teleporter = component.FindTeleportTarget();
		component.SetTeleportTarget(teleporter);
		TeleportalPad.StatesInstance smi = teleporter.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.targetTeleporter.Trigger(smi);
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x0011D450 File Offset: 0x0011B650
	protected override void OnStopWork(WorkerBase worker)
	{
		TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
	}
}
