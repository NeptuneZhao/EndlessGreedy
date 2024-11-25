using System;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
	// Token: 0x06001F33 RID: 7987 RVA: 0x000AEE4E File Offset: 0x000AD04E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (FlopMonitor.Instance smi) => smi.ShouldBeginFlopping(), null);
	}

	// Token: 0x02001334 RID: 4916
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001335 RID: 4917
	public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
	{
		// Token: 0x06008631 RID: 34353 RVA: 0x0032879E File Offset: 0x0032699E
		public Instance(IStateMachineTarget master, FlopMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008632 RID: 34354 RVA: 0x003287A8 File Offset: 0x003269A8
		public bool ShouldBeginFlopping()
		{
			Vector3 position = base.transform.GetPosition();
			position.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			int num = Grid.PosToCell(position);
			return Grid.IsValidCell(num) && Grid.Solid[num] && !Grid.IsSubstantialLiquid(cell, 0.35f) && !Grid.IsLiquid(Grid.CellAbove(cell));
		}
	}
}
