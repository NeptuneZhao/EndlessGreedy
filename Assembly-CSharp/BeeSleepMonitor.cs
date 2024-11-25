using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class BeeSleepMonitor : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>
{
	// Token: 0x06000376 RID: 886 RVA: 0x0001CD1A File Offset: 0x0001AF1A
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<BeeSleepMonitor.Instance, float>(this.UpdateCO2Exposure), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.BeeWantsToSleep, new StateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.Transition.ConditionCallback(this.ShouldSleep), null);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001CD55 File Offset: 0x0001AF55
	public bool ShouldSleep(BeeSleepMonitor.Instance smi)
	{
		return smi.CO2Exposure >= 5f;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0001CD68 File Offset: 0x0001AF68
	public void UpdateCO2Exposure(BeeSleepMonitor.Instance smi, float dt)
	{
		if (this.IsInCO2(smi))
		{
			smi.CO2Exposure += 1f;
		}
		else
		{
			smi.CO2Exposure -= 0.5f;
		}
		smi.CO2Exposure = Mathf.Clamp(smi.CO2Exposure, 0f, 10f);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
	public bool IsInCO2(BeeSleepMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.gameObject);
		return Grid.IsValidCell(num) && Grid.Element[num].id == SimHashes.CarbonDioxide;
	}

	// Token: 0x02001000 RID: 4096
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001001 RID: 4097
	public new class Instance : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06007AFC RID: 31484 RVA: 0x00302EB9 File Offset: 0x003010B9
		public Instance(IStateMachineTarget master, BeeSleepMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x04005BF3 RID: 23539
		public float CO2Exposure;
	}
}
