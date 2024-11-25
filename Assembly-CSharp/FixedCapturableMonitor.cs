using System;

// Token: 0x0200054D RID: 1357
public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	// Token: 0x06001F31 RID: 7985 RVA: 0x000AEDB8 File Offset: 0x000ACFB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (FixedCapturableMonitor.Instance smi) => smi.ShouldGoGetCaptured(), null).Enter(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}

	// Token: 0x02001331 RID: 4913
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001332 RID: 4914
	public new class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
	{
		// Token: 0x06008629 RID: 34345 RVA: 0x003286CC File Offset: 0x003268CC
		public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.Navigator = base.GetComponent<Navigator>();
			this.PrefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
		}

		// Token: 0x0600862A RID: 34346 RVA: 0x00328720 File Offset: 0x00326920
		public bool ShouldGoGetCaptured()
		{
			return this.targetCapturePoint != null && this.targetCapturePoint.IsRunning() && this.targetCapturePoint.shouldCreatureGoGetCaptured && (!this.isBaby || this.targetCapturePoint.def.allowBabies);
		}

		// Token: 0x040065D3 RID: 26067
		public FixedCapturePoint.Instance targetCapturePoint;

		// Token: 0x040065D4 RID: 26068
		public ChoreConsumer ChoreConsumer;

		// Token: 0x040065D5 RID: 26069
		public Navigator Navigator;

		// Token: 0x040065D6 RID: 26070
		public Tag PrefabTag;

		// Token: 0x040065D7 RID: 26071
		public bool isBaby;
	}
}
