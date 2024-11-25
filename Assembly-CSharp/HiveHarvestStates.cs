using System;

// Token: 0x020000DD RID: 221
public class HiveHarvestStates : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>
{
	// Token: 0x06000402 RID: 1026 RVA: 0x00020767 File Offset: 0x0001E967
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x0200105D RID: 4189
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200105E RID: 4190
	public new class Instance : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>.GameInstance
	{
		// Token: 0x06007BCB RID: 31691 RVA: 0x003042B2 File Offset: 0x003024B2
		public Instance(Chore<HiveHarvestStates.Instance> chore, HiveHarvestStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.HarvestHiveBehaviour);
		}
	}
}
