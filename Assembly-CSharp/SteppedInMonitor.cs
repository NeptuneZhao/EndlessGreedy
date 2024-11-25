using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020009A3 RID: 2467
public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
	// Token: 0x060047D8 RID: 18392 RVA: 0x0019B6F8 File Offset: 0x001998F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.carpetedFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.carpetedFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetCarpetFeet)).ToggleExpression(Db.Get().Expressions.Tickled, null).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetCarpetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet)), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetBody.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked), UpdateRate.SIM_1000ms, false).Transition(this.wetFloor, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged)), UpdateRate.SIM_200ms);
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x0019B891 File Offset: 0x00199A91
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetCarpetFeet(smi);
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x0019B89C File Offset: 0x00199A9C
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && !smi.effects.HasEffect("WetFeet") && smi.IsEffectAllowed("CarpetFeet"))
		{
			smi.effects.Add("CarpetFeet", true);
		}
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x0019B8EC File Offset: 0x00199AEC
	private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetWetFeet(smi);
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x0019B8F4 File Offset: 0x00199AF4
	private static void GetWetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && smi.IsEffectAllowed("WetFeet"))
		{
			smi.effects.Add("WetFeet", true);
		}
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x0019B927 File Offset: 0x00199B27
	private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetSoaked(smi);
	}

	// Token: 0x060047DE RID: 18398 RVA: 0x0019B930 File Offset: 0x00199B30
	private static void GetSoaked(SteppedInMonitor.Instance smi)
	{
		if (smi.effects.HasEffect("WetFeet"))
		{
			smi.effects.Remove("WetFeet");
		}
		if (smi.IsEffectAllowed("SoakingWet"))
		{
			smi.effects.Add("SoakingWet", true);
		}
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x0019B980 File Offset: 0x00199B80
	private static bool IsOnCarpet(SteppedInMonitor.Instance smi)
	{
		int cell = Grid.CellBelow(Grid.PosToCell(smi));
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 9];
		return Grid.IsValidCell(cell) && gameObject != null && gameObject.HasTag(GameTags.Carpeted);
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x0019B9D0 File Offset: 0x00199BD0
	private static bool IsFloorWet(SteppedInMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x0019B9FC File Offset: 0x00199BFC
	private static bool IsSubmerged(SteppedInMonitor.Instance smi)
	{
		int num = Grid.CellAbove(Grid.PosToCell(smi));
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x04002F04 RID: 12036
	public const string CARPET_EFFECT_NAME = "CarpetFeet";

	// Token: 0x04002F05 RID: 12037
	public const string WET_FEET_EFFECT_NAME = "WetFeet";

	// Token: 0x04002F06 RID: 12038
	public const string SOAK_EFFECT_NAME = "SoakingWet";

	// Token: 0x04002F07 RID: 12039
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F08 RID: 12040
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State carpetedFloor;

	// Token: 0x04002F09 RID: 12041
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;

	// Token: 0x04002F0A RID: 12042
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

	// Token: 0x0200198A RID: 6538
	public new class Instance : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06009D08 RID: 40200 RVA: 0x00373D77 File Offset: 0x00371F77
		// (set) Token: 0x06009D07 RID: 40199 RVA: 0x00373D6E File Offset: 0x00371F6E
		public string[] effectsAllowed { get; private set; }

		// Token: 0x06009D09 RID: 40201 RVA: 0x00373D7F File Offset: 0x00371F7F
		public Instance(IStateMachineTarget master) : this(master, new string[]
		{
			"CarpetFeet",
			"WetFeet",
			"SoakingWet"
		})
		{
		}

		// Token: 0x06009D0A RID: 40202 RVA: 0x00373DA6 File Offset: 0x00371FA6
		public Instance(IStateMachineTarget master, string[] effectsAllowed) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			this.effectsAllowed = effectsAllowed;
		}

		// Token: 0x06009D0B RID: 40203 RVA: 0x00373DC4 File Offset: 0x00371FC4
		public bool IsEffectAllowed(string effectName)
		{
			if (this.effectsAllowed == null || this.effectsAllowed.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.effectsAllowed.Length; i++)
			{
				if (this.effectsAllowed[i] == effectName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040079C6 RID: 31174
		public Effects effects;
	}
}
