using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class MoltStates : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
	// Token: 0x06000436 RID: 1078 RVA: 0x00021CB4 File Offset: 0x0001FEB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moltpre;
		GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.MOLTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.MOLTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moltpre.Enter(new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt)).QueueAnim("lay_egg_pre", false, null).OnAnimQueueComplete(this.moltpst);
		this.moltpst.QueueAnim("lay_egg_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown, false);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00021D71 File Offset: 0x0001FF71
	private static void Molt(MoltStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00021D90 File Offset: 0x0001FF90
	private static int GetMoveAsideCell(MoltStates.Instance smi)
	{
		int num = 1;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 8;
		}
		int cell = Grid.PosToCell(smi);
		if (Grid.IsValidCell(cell))
		{
			int num2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			int num3 = Grid.OffsetCell(cell, -num, 0);
			if (Grid.IsValidCell(num3))
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x040002DA RID: 730
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;

	// Token: 0x040002DB RID: 731
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;

	// Token: 0x040002DC RID: 732
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

	// Token: 0x0200107F RID: 4223
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001080 RID: 4224
	public new class Instance : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
	{
		// Token: 0x06007C22 RID: 31778 RVA: 0x00304BFE File Offset: 0x00302DFE
		public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ScalesGrown);
		}

		// Token: 0x04005CF5 RID: 23797
		public Vector3 eggPos;
	}
}
