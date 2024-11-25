using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
	// Token: 0x0600174D RID: 5965 RVA: 0x0007E2C4 File Offset: 0x0007C4C4
	public RecoverBreathChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotABionic, null);
	}

	// Token: 0x020011E5 RID: 4581
	public class StatesInstance : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
	{
		// Token: 0x06008171 RID: 33137 RVA: 0x00317538 File Offset: 0x00315738
		public StatesInstance(RecoverBreathChore master, GameObject recoverer) : base(master)
		{
			base.sm.recoverer.Set(recoverer, base.smi, false);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float recover_BREATH_DELTA = DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, recover_BREATH_DELTA, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x003175AC File Offset: 0x003157AC
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x003175E4 File Offset: 0x003157E4
		public void UpdateLocator()
		{
			int num = base.sm.recoverer.GetSMI<BreathMonitor.Instance>(base.smi).GetRecoverCell();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.recoverer.Get<Transform>(base.smi).GetPosition());
			}
			Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x0031765C File Offset: 0x0031585C
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x06008175 RID: 33141 RVA: 0x00317688 File Offset: 0x00315888
		public void RemoveSuitIfNecessary()
		{
			Equipment equipment = base.sm.recoverer.Get<Equipment>(base.smi);
			if (equipment == null)
			{
				return;
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable == null)
			{
				return;
			}
			assignable.Unassign();
		}

		// Token: 0x040061B8 RID: 25016
		public AttributeModifier recoveringbreath;
	}

	// Token: 0x020011E6 RID: 4582
	public class States : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
	{
		// Token: 0x06008176 RID: 33142 RVA: 0x003176DC File Offset: 0x003158DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.recoverer);
			this.root.Enter("CreateLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverBreathChore.StatesInstance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit, null, null, null);
			this.remove_suit.GoTo(this.recover);
			this.recover.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (RecoverBreathChore.StatesInstance smi) => smi.recoveringbreath, null).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery, null).TriggerOnExit(GameHashes.EndBreathRecovery, null);
			this.recover.pre.PlayAnim("breathe_pre").OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop);
			this.recover.pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(null);
		}

		// Token: 0x040061B9 RID: 25017
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040061BA RID: 25018
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;

		// Token: 0x040061BB RID: 25019
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;

		// Token: 0x040061BC RID: 25020
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;

		// Token: 0x040061BD RID: 25021
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;
	}
}
