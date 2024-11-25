using System;

// Token: 0x020000DA RID: 218
public class HiveEatingMonitor : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>
{
	// Token: 0x060003F9 RID: 1017 RVA: 0x00020599 File Offset: 0x0001E799
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToEat, new StateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.Transition.ConditionCallback(HiveEatingMonitor.ShouldEat), null);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x000205C1 File Offset: 0x0001E7C1
	public static bool ShouldEat(HiveEatingMonitor.Instance smi)
	{
		return smi.storage.FindFirst(smi.def.consumedOre) != null;
	}

	// Token: 0x02001055 RID: 4181
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C99 RID: 23705
		public Tag consumedOre;
	}

	// Token: 0x02001056 RID: 4182
	public new class Instance : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.GameInstance
	{
		// Token: 0x06007BBF RID: 31679 RVA: 0x0030420F File Offset: 0x0030240F
		public Instance(IStateMachineTarget master, HiveEatingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x04005C9A RID: 23706
		[MyCmpReq]
		public Storage storage;
	}
}
