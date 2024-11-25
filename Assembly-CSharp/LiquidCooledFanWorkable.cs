using System;
using UnityEngine;

// Token: 0x020006FB RID: 1787
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
	// Token: 0x06002DB1 RID: 11697 RVA: 0x0010057F File Offset: 0x000FE77F
	private LiquidCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x0010058E File Offset: 0x000FE78E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = null;
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x0010059D File Offset: 0x000FE79D
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06002DB4 RID: 11700 RVA: 0x001005DB File Offset: 0x000FE7DB
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002DB5 RID: 11701 RVA: 0x001005EA File Offset: 0x000FE7EA
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x001005F9 File Offset: 0x000FE7F9
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001A97 RID: 6807
	[MyCmpGet]
	private Operational operational;
}
