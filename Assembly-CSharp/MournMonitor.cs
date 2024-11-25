using System;
using Klei.AI;

// Token: 0x0200098F RID: 2447
public class MournMonitor : GameStateMachine<MournMonitor, MournMonitor.Instance>
{
	// Token: 0x0600475A RID: 18266 RVA: 0x0019862C File Offset: 0x0019682C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.EffectAdded, new GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.OnEffectAdded)).Enter(delegate(MournMonitor.Instance smi)
		{
			if (this.ShouldMourn(smi))
			{
				smi.GoTo(this.needsToMourn);
			}
		});
		this.needsToMourn.ToggleChore((MournMonitor.Instance smi) => new MournChore(smi.master), this.idle);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x001986A0 File Offset: 0x001968A0
	private bool ShouldMourn(MournMonitor.Instance smi)
	{
		Effect effect = Db.Get().effects.Get("Mourning");
		return smi.master.GetComponent<Effects>().HasEffect(effect);
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x001986D3 File Offset: 0x001968D3
	private void OnEffectAdded(MournMonitor.Instance smi, object data)
	{
		if (this.ShouldMourn(smi))
		{
			smi.GoTo(this.needsToMourn);
		}
	}

	// Token: 0x04002E99 RID: 11929
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002E9A RID: 11930
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State needsToMourn;

	// Token: 0x02001957 RID: 6487
	public new class Instance : GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C36 RID: 39990 RVA: 0x00371E62 File Offset: 0x00370062
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
