using System;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
	// Token: 0x06003848 RID: 14408 RVA: 0x00133568 File Offset: 0x00131768
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), delegate(CallAdultMonitor.Instance smi)
		{
			smi.RefreshCallTime();
		});
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x001335B9 File Offset: 0x001317B9
	public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
	{
		return Time.time >= smi.nextCallTime;
	}

	// Token: 0x020016CC RID: 5836
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040070D8 RID: 28888
		public float callMinInterval = 120f;

		// Token: 0x040070D9 RID: 28889
		public float callMaxInterval = 240f;
	}

	// Token: 0x020016CD RID: 5837
	public new class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
	{
		// Token: 0x06009390 RID: 37776 RVA: 0x003599CB File Offset: 0x00357BCB
		public Instance(IStateMachineTarget master, CallAdultMonitor.Def def) : base(master, def)
		{
			this.RefreshCallTime();
		}

		// Token: 0x06009391 RID: 37777 RVA: 0x003599DB File Offset: 0x00357BDB
		public void RefreshCallTime()
		{
			this.nextCallTime = Time.time + UnityEngine.Random.value * (base.def.callMaxInterval - base.def.callMinInterval) + base.def.callMinInterval;
		}

		// Token: 0x040070DA RID: 28890
		public float nextCallTime;
	}
}
