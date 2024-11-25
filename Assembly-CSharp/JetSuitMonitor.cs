using System;
using UnityEngine;

// Token: 0x02000932 RID: 2354
public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
	// Token: 0x06004461 RID: 17505 RVA: 0x001855F4 File Offset: 0x001837F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.Target(this.owner);
		this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
		this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06004462 RID: 17506 RVA: 0x00185690 File Offset: 0x00183890
	public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
	{
		return smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;
	}

	// Token: 0x06004463 RID: 17507 RVA: 0x001856AF File Offset: 0x001838AF
	public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
	{
		return !smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;
	}

	// Token: 0x06004464 RID: 17508 RVA: 0x001856D1 File Offset: 0x001838D1
	public static void StartFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x001856D3 File Offset: 0x001838D3
	public static void StopFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x001856D8 File Offset: 0x001838D8
	public static void Emit(JetSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float num = 0.1f * dt;
		num = Mathf.Min(num, smi.jet_suit_tank.amount);
		smi.jet_suit_tank.amount -= num;
		float num2 = num * 3f;
		if (num2 > 1E-45f)
		{
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementConsumerSimUpdate, num2, 473.15f, byte.MaxValue, 0, true, -1);
		}
		if (smi.jet_suit_tank.amount == 0f)
		{
			smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
			smi.navigator.SetCurrentNavType(NavType.Floor);
		}
	}

	// Token: 0x04002CC1 RID: 11457
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04002CC2 RID: 11458
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;

	// Token: 0x04002CC3 RID: 11459
	public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x0200188E RID: 6286
	public new class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060098DF RID: 39135 RVA: 0x0036914A File Offset: 0x0036734A
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.jet_suit_tank = master.GetComponent<JetSuitTank>();
		}

		// Token: 0x04007687 RID: 30343
		public Navigator navigator;

		// Token: 0x04007688 RID: 30344
		public JetSuitTank jet_suit_tank;
	}
}
