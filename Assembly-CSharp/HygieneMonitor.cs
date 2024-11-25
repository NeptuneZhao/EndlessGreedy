using System;
using Klei.AI;

// Token: 0x02000987 RID: 2439
public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
	// Token: 0x0600473E RID: 18238 RVA: 0x00197A10 File Offset: 0x00195C10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.needsshower;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (HygieneMonitor.Instance smi) => smi.NeedsShower());
		this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (HygieneMonitor.Instance smi) => !smi.NeedsShower()).ToggleUrge(Db.Get().Urges.Shower).Enter(delegate(HygieneMonitor.Instance smi)
		{
			smi.SetDirtiness(1f);
		});
	}

	// Token: 0x04002E7D RID: 11901
	public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;

	// Token: 0x04002E7E RID: 11902
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;

	// Token: 0x04002E7F RID: 11903
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

	// Token: 0x02001944 RID: 6468
	public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009BEA RID: 39914 RVA: 0x003714EF File Offset: 0x0036F6EF
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06009BEB RID: 39915 RVA: 0x00371504 File Offset: 0x0036F704
		public float GetDirtiness()
		{
			return base.sm.dirtiness.Get(this);
		}

		// Token: 0x06009BEC RID: 39916 RVA: 0x00371517 File Offset: 0x0036F717
		public void SetDirtiness(float dirtiness)
		{
			base.sm.dirtiness.Set(dirtiness, this, false);
		}

		// Token: 0x06009BED RID: 39917 RVA: 0x0037152D File Offset: 0x0036F72D
		public bool NeedsShower()
		{
			return !this.effects.HasEffect(Shower.SHOWER_EFFECT);
		}

		// Token: 0x040078F7 RID: 30967
		private Effects effects;
	}
}
