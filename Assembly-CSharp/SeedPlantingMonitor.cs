using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
	// Token: 0x06000462 RID: 1122 RVA: 0x0002320C File Offset: 0x0002140C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToPlantSeed, new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds), delegate(SeedPlantingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0002325D File Offset: 0x0002145D
	public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
	{
		return Time.time >= smi.nextSearchTime;
	}

	// Token: 0x0200109A RID: 4250
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005D3A RID: 23866
		public float searchMinInterval = 60f;

		// Token: 0x04005D3B RID: 23867
		public float searchMaxInterval = 300f;
	}

	// Token: 0x0200109B RID: 4251
	public new class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
	{
		// Token: 0x06007C6F RID: 31855 RVA: 0x003057DF File Offset: 0x003039DF
		public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x003057EF File Offset: 0x003039EF
		public void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x04005D3C RID: 23868
		public float nextSearchTime;
	}
}
