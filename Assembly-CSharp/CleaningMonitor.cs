using System;

// Token: 0x02000543 RID: 1347
public class CleaningMonitor : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
	// Token: 0x06001EF5 RID: 7925 RVA: 0x000AD568 File Offset: 0x000AB768
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.clean;
		this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (CleaningMonitor.Instance smi) => smi.CanCleanElementState(), delegate(CleaningMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CleaningMonitor.Instance smi) => smi.def.coolDown, this.clean);
	}

	// Token: 0x04001177 RID: 4471
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;

	// Token: 0x04001178 RID: 4472
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

	// Token: 0x0200130D RID: 4877
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006569 RID: 25961
		public Element.State elementState = Element.State.Liquid;

		// Token: 0x0400656A RID: 25962
		public CellOffset[] cellOffsets;

		// Token: 0x0400656B RID: 25963
		public float coolDown = 30f;
	}

	// Token: 0x0200130E RID: 4878
	public new class Instance : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance
	{
		// Token: 0x060085AE RID: 34222 RVA: 0x00326CAE File Offset: 0x00324EAE
		public Instance(IStateMachineTarget master, CleaningMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060085AF RID: 34223 RVA: 0x00326CB8 File Offset: 0x00324EB8
		public bool CanCleanElementState()
		{
			int num = Grid.PosToCell(base.smi.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (!Grid.IsLiquid(num) && base.smi.def.elementState == Element.State.Liquid)
			{
				return false;
			}
			if (Grid.DiseaseCount[num] > 0)
			{
				return true;
			}
			if (base.smi.def.cellOffsets != null)
			{
				foreach (CellOffset offset in base.smi.def.cellOffsets)
				{
					int num2 = Grid.OffsetCell(num, offset);
					if (Grid.IsValidCell(num2) && Grid.DiseaseCount[num2] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
