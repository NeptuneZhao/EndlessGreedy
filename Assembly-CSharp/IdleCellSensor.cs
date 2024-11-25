using System;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class IdleCellSensor : Sensor
{
	// Token: 0x06001A06 RID: 6662 RVA: 0x0008A78C File Offset: 0x0008898C
	public IdleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
	}

	// Token: 0x06001A07 RID: 6663 RVA: 0x0008A7BC File Offset: 0x000889BC
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60));
		this.navigator.RunQuery(idleCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = idleCellQuery.GetResultCell();
	}

	// Token: 0x06001A08 RID: 6664 RVA: 0x0008A836 File Offset: 0x00088A36
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x04000ED0 RID: 3792
	private MinionBrain brain;

	// Token: 0x04000ED1 RID: 3793
	private Navigator navigator;

	// Token: 0x04000ED2 RID: 3794
	private KPrefabID prefabid;

	// Token: 0x04000ED3 RID: 3795
	private int cell;
}
