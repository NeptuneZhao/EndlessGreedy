using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class ChoreConsumerState
{
	// Token: 0x0600180E RID: 6158 RVA: 0x000808A4 File Offset: 0x0007EAA4
	public ChoreConsumerState(ChoreConsumer consumer)
	{
		this.consumer = consumer;
		this.navigator = consumer.GetComponent<Navigator>();
		this.prefabid = consumer.GetComponent<KPrefabID>();
		this.ownable = consumer.GetComponent<Ownable>();
		this.gameObject = consumer.gameObject;
		this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
		this.hasSolidTransferArm = (this.solidTransferArm != null);
		this.resume = consumer.GetComponent<MinionResume>();
		this.choreDriver = consumer.GetComponent<ChoreDriver>();
		this.schedulable = consumer.GetComponent<Schedulable>();
		this.traits = consumer.GetComponent<Traits>();
		this.choreProvider = consumer.GetComponent<ChoreProvider>();
		MinionIdentity component = consumer.GetComponent<MinionIdentity>();
		if (component != null)
		{
			if (component.assignableProxy == null)
			{
				component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, component);
			}
			this.assignables = component.GetSoleOwner();
			this.equipment = component.GetEquipment();
		}
		else
		{
			this.assignables = consumer.GetComponent<Assignables>();
			this.equipment = consumer.GetComponent<Equipment>();
		}
		this.storage = consumer.GetComponent<Storage>();
		this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
		this.worker = consumer.GetComponent<WorkerBase>();
		this.selectable = consumer.GetComponent<KSelectable>();
		if (this.schedulable != null)
		{
			this.scheduleBlock = this.schedulable.GetSchedule().GetCurrentScheduleBlock();
		}
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000809F8 File Offset: 0x0007EBF8
	public void Refresh()
	{
		if (this.schedulable != null)
		{
			Schedule schedule = this.schedulable.GetSchedule();
			if (schedule != null)
			{
				this.scheduleBlock = schedule.GetCurrentScheduleBlock();
			}
		}
	}

	// Token: 0x04000D4C RID: 3404
	public KPrefabID prefabid;

	// Token: 0x04000D4D RID: 3405
	public GameObject gameObject;

	// Token: 0x04000D4E RID: 3406
	public ChoreConsumer consumer;

	// Token: 0x04000D4F RID: 3407
	public ChoreProvider choreProvider;

	// Token: 0x04000D50 RID: 3408
	public Navigator navigator;

	// Token: 0x04000D51 RID: 3409
	public Ownable ownable;

	// Token: 0x04000D52 RID: 3410
	public Assignables assignables;

	// Token: 0x04000D53 RID: 3411
	public MinionResume resume;

	// Token: 0x04000D54 RID: 3412
	public ChoreDriver choreDriver;

	// Token: 0x04000D55 RID: 3413
	public Schedulable schedulable;

	// Token: 0x04000D56 RID: 3414
	public Traits traits;

	// Token: 0x04000D57 RID: 3415
	public Equipment equipment;

	// Token: 0x04000D58 RID: 3416
	public Storage storage;

	// Token: 0x04000D59 RID: 3417
	public ConsumableConsumer consumableConsumer;

	// Token: 0x04000D5A RID: 3418
	public KSelectable selectable;

	// Token: 0x04000D5B RID: 3419
	public WorkerBase worker;

	// Token: 0x04000D5C RID: 3420
	public SolidTransferArm solidTransferArm;

	// Token: 0x04000D5D RID: 3421
	public bool hasSolidTransferArm;

	// Token: 0x04000D5E RID: 3422
	public ScheduleBlock scheduleBlock;
}
