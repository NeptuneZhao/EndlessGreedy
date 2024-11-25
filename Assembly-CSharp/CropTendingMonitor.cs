using System;
using UnityEngine;

// Token: 0x020007FE RID: 2046
public class CropTendingMonitor : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>
{
	// Token: 0x06003890 RID: 14480 RVA: 0x00134BE1 File Offset: 0x00132DE1
	private bool InterestedInTendingCrops(CropTendingMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.Hungry) || UnityEngine.Random.value <= smi.def.unsatisfiedTendChance;
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x00134C08 File Offset: 0x00132E08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.cooldown.ParamTransition<float>(this.cooldownTimer, this.lookingForCrop, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && this.InterestedInTendingCrops(smi)).ParamTransition<float>(this.cooldownTimer, this.reset, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && !this.InterestedInTendingCrops(smi)).Update(delegate(CropTendingMonitor.Instance smi, float dt)
		{
			this.cooldownTimer.Delta(-dt, smi);
		}, UpdateRate.SIM_1000ms, false);
		this.lookingForCrop.ToggleBehaviour(GameTags.Creatures.WantsToTendCrops, (CropTendingMonitor.Instance smi) => true, delegate(CropTendingMonitor.Instance smi)
		{
			smi.GoTo(this.reset);
		});
		this.reset.Exit(delegate(CropTendingMonitor.Instance smi)
		{
			this.cooldownTimer.Set(600f / smi.def.numCropsTendedPerCycle, smi, false);
		}).GoTo(this.cooldown);
	}

	// Token: 0x040021F6 RID: 8694
	private StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.FloatParameter cooldownTimer;

	// Token: 0x040021F7 RID: 8695
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State cooldown;

	// Token: 0x040021F8 RID: 8696
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State lookingForCrop;

	// Token: 0x040021F9 RID: 8697
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State reset;

	// Token: 0x020016E4 RID: 5860
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400711A RID: 28954
		public float numCropsTendedPerCycle = 8f;

		// Token: 0x0400711B RID: 28955
		public float unsatisfiedTendChance = 0.5f;
	}

	// Token: 0x020016E5 RID: 5861
	public new class Instance : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.GameInstance
	{
		// Token: 0x060093DC RID: 37852 RVA: 0x0035A3D4 File Offset: 0x003585D4
		public Instance(IStateMachineTarget master, CropTendingMonitor.Def def) : base(master, def)
		{
		}
	}
}
