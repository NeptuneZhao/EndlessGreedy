using System;
using System.Collections.Generic;

// Token: 0x02000972 RID: 2418
public class ColonyRationMonitor : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance>
{
	// Token: 0x060046E2 RID: 18146 RVA: 0x001955B8 File Offset: 0x001937B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Update("UpdateOutOfRations", delegate(ColonyRationMonitor.Instance smi, float dt)
		{
			smi.UpdateIsOutOfRations();
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.ParamTransition<bool>(this.isOutOfRations, this.outofrations, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsTrue).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
		this.outofrations.ParamTransition<bool>(this.isOutOfRations, this.satisfied, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsFalse).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
	}

	// Token: 0x04002E29 RID: 11817
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E2A RID: 11818
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State outofrations;

	// Token: 0x04002E2B RID: 11819
	private StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isOutOfRations = new StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001909 RID: 6409
	public new class Instance : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009AFA RID: 39674 RVA: 0x0036E7AB File Offset: 0x0036C9AB
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.UpdateIsOutOfRations();
		}

		// Token: 0x06009AFB RID: 39675 RVA: 0x0036E7BC File Offset: 0x0036C9BC
		public void UpdateIsOutOfRations()
		{
			bool value = true;
			using (List<Edible>.Enumerator enumerator = Components.Edibles.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent<Pickupable>().UnreservedAmount > 0f)
					{
						value = false;
						break;
					}
				}
			}
			base.smi.sm.isOutOfRations.Set(value, base.smi, false);
		}

		// Token: 0x06009AFC RID: 39676 RVA: 0x0036E840 File Offset: 0x0036CA40
		public bool IsOutOfRations()
		{
			return base.smi.sm.isOutOfRations.Get(base.smi);
		}
	}
}
