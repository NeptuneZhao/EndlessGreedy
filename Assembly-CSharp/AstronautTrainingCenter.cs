using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000682 RID: 1666
[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
	// Token: 0x0600293A RID: 10554 RVA: 0x000E93A1 File Offset: 0x000E75A1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.chore = this.CreateChore();
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000E93B8 File Offset: 0x000E75B8
	private Chore CreateChore()
	{
		return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, this, null, true, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x000E93EB File Offset: 0x000E75EB
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x000E9401 File Offset: 0x000E7601
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker == null;
		return true;
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000E940C File Offset: 0x000E760C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.chore != null && !this.chore.isComplete)
		{
			this.chore.Cancel("completed but not complete??");
		}
		this.chore = this.CreateChore();
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000E9446 File Offset: 0x000E7646
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x000E945C File Offset: 0x000E765C
	public override float GetPercentComplete()
	{
		base.worker == null;
		return 0f;
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x000E9470 File Offset: 0x000E7670
	public AstronautTrainingCenter()
	{
		Chore.Precondition isNotMarkedForDeconstruction = default(Chore.Precondition);
		isNotMarkedForDeconstruction.id = "IsNotMarkedForDeconstruction";
		isNotMarkedForDeconstruction.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		isNotMarkedForDeconstruction.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		this.IsNotMarkedForDeconstruction = isNotMarkedForDeconstruction;
		base..ctor();
	}

	// Token: 0x040017BD RID: 6077
	public float daysToMasterRole;

	// Token: 0x040017BE RID: 6078
	private Chore chore;

	// Token: 0x040017BF RID: 6079
	public Chore.Precondition IsNotMarkedForDeconstruction;
}
