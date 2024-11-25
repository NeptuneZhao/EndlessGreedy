using System;
using Klei.AI;

// Token: 0x020004BB RID: 1211
public class SafeCellSensor : Sensor
{
	// Token: 0x06001A10 RID: 6672 RVA: 0x0008A9BC File Offset: 0x00088BBC
	public SafeCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
		this.traits = base.GetComponent<Traits>();
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x0008AA0C File Offset: 0x00088C0C
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		bool flag = this.HasSafeCell();
		this.RunSafeCellQuery(false);
		bool flag2 = this.HasSafeCell();
		if (flag2 != flag)
		{
			if (flag2)
			{
				this.sensors.Trigger(982561777, null);
				return;
			}
			this.sensors.Trigger(506919987, null);
		}
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x0008AA76 File Offset: 0x00088C76
	public void RunSafeCellQuery(bool avoid_light)
	{
		this.cell = this.RunAndGetSafeCellQueryResult(avoid_light);
		if (this.cell == Grid.PosToCell(this.navigator))
		{
			this.cell = Grid.InvalidCell;
		}
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x0008AAA4 File Offset: 0x00088CA4
	public int RunAndGetSafeCellQueryResult(bool avoid_light)
	{
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light);
		this.navigator.RunQuery(safeCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = safeCellQuery.GetResultCell();
		return this.cell;
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x0008AAFE File Offset: 0x00088CFE
	public int GetSensorCell()
	{
		return this.cell;
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x0008AB06 File Offset: 0x00088D06
	public int GetCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(false);
		}
		return this.cell;
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x0008AB22 File Offset: 0x00088D22
	public int GetSleepCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
		}
		return this.cell;
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x0008AB53 File Offset: 0x00088D53
	public bool HasSafeCell()
	{
		return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell(this.sensors);
	}

	// Token: 0x04000EDA RID: 3802
	private MinionBrain brain;

	// Token: 0x04000EDB RID: 3803
	private Navigator navigator;

	// Token: 0x04000EDC RID: 3804
	private KPrefabID prefabid;

	// Token: 0x04000EDD RID: 3805
	private Traits traits;

	// Token: 0x04000EDE RID: 3806
	private int cell = Grid.InvalidCell;
}
