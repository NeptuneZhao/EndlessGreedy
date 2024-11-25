using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000974 RID: 2420
public class CoughMonitor : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
	// Token: 0x060046E6 RID: 18150 RVA: 0x001956DC File Offset: 0x001938DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>(this.shouldCough, this.coughing, (CoughMonitor.Instance smi, bool bShouldCough) => bShouldCough);
		this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing, null).ToggleReactable((CoughMonitor.Instance smi) => smi.GetReactable()).ParamTransition<bool>(this.shouldCough, this.idle, (CoughMonitor.Instance smi, bool bShouldCough) => !bShouldCough);
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x001957B8 File Offset: 0x001939B8
	private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
	{
		float timeInCycles = GameClock.Instance.GetTimeInCycles();
		if (timeInCycles > 0.1f && timeInCycles - smi.lastCoughTime <= 0.1f)
		{
			return;
		}
		Sim.MassConsumedCallback massConsumedCallback = (Sim.MassConsumedCallback)data;
		float num = (smi.lastConsumeTime <= 0f) ? 0f : (timeInCycles - smi.lastConsumeTime);
		smi.lastConsumeTime = timeInCycles;
		smi.amountConsumed -= 0.05f * num;
		smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0f);
		smi.amountConsumed += massConsumedCallback.mass;
		if (smi.amountConsumed >= 1f)
		{
			this.shouldCough.Set(true, smi, false);
			smi.lastConsumeTime = 0f;
			smi.amountConsumed = 0f;
		}
	}

	// Token: 0x04002E30 RID: 11824
	private const float amountToCough = 1f;

	// Token: 0x04002E31 RID: 11825
	private const float decayRate = 0.05f;

	// Token: 0x04002E32 RID: 11826
	private const float coughInterval = 0.1f;

	// Token: 0x04002E33 RID: 11827
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;

	// Token: 0x04002E34 RID: 11828
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;

	// Token: 0x04002E35 RID: 11829
	public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

	// Token: 0x0200190E RID: 6414
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200190F RID: 6415
	public new class Instance : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance
	{
		// Token: 0x06009B0C RID: 39692 RVA: 0x0036EAC2 File Offset: 0x0036CCC2
		public Instance(IStateMachineTarget master, CoughMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009B0D RID: 39693 RVA: 0x0036EACC File Offset: 0x0036CCCC
		public Reactable GetReactable()
		{
			Emote cough_Small = Db.Get().Emotes.Minion.Cough_Small;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "BadAirCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(cough_Small);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable.RegisterEmoteStepCallbacks("react_small", null, new Action<GameObject>(this.FinishedCoughing));
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x0036EB58 File Offset: 0x0036CD58
		private void FinishedCoughing(GameObject cougher)
		{
			cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
			base.sm.shouldCough.Set(false, base.smi, false);
			base.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
		}

		// Token: 0x04007841 RID: 30785
		[Serialize]
		public float lastCoughTime;

		// Token: 0x04007842 RID: 30786
		[Serialize]
		public float lastConsumeTime;

		// Token: 0x04007843 RID: 30787
		[Serialize]
		public float amountConsumed;
	}
}
