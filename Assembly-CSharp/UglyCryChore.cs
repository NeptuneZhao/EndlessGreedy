﻿using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
	// Token: 0x06001783 RID: 6019 RVA: 0x0007F7B8 File Offset: 0x0007D9B8
	public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200120F RID: 4623
	public class StatesInstance : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
	{
		// Token: 0x0600820C RID: 33292 RVA: 0x0031B34B File Offset: 0x0031954B
		public StatesInstance(UglyCryChore master, GameObject crier) : base(master)
		{
			base.sm.crier.Set(crier, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
		}

		// Token: 0x0600820D RID: 33293 RVA: 0x0031B388 File Offset: 0x00319588
		public void ProduceTears(float dt)
		{
			if (dt <= 0f)
			{
				return;
			}
			int gameCell = Grid.PosToCell(base.smi.master.gameObject);
			Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
			if (equippable != null)
			{
				equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, false, true);
				return;
			}
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.Water, CellEventLogger.Instance.Tears, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x0400623F RID: 25151
		private AmountInstance bodyTemperature;
	}

	// Token: 0x02001210 RID: 4624
	public class States : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
	{
		// Token: 0x0600820E RID: 33294 RVA: 0x0031B430 File Offset: 0x00319630
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.cry;
			base.Target(this.crier);
			this.uglyCryingEffect = new Effect("UglyCrying", DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, false, false, true));
			Db.Get().effects.Add(this.uglyCryingEffect);
			this.cry.defaultState = this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim", 0f);
			this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, this.cry.cry_loop);
			this.cry.cry_loop.ToggleAnims("anim_cry_kanim", 0f).Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
			}).ScheduleGoTo(18f, this.cry.cry_pst).ToggleEffect((UglyCryChore.StatesInstance smi) => this.uglyCryingEffect).Update(delegate(UglyCryChore.StatesInstance smi, float dt)
			{
				smi.ProduceTears(dt);
			}, UpdateRate.SIM_200ms, false);
			this.cry.cry_pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.AddEffect("CryFace").Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04006240 RID: 25152
		public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;

		// Token: 0x04006241 RID: 25153
		public UglyCryChore.States.Cry cry;

		// Token: 0x04006242 RID: 25154
		public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;

		// Token: 0x04006243 RID: 25155
		private Effect uglyCryingEffect;

		// Token: 0x020023F1 RID: 9201
		public class Cry : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
		{
			// Token: 0x0400A078 RID: 41080
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;

			// Token: 0x0400A079 RID: 41081
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;

			// Token: 0x0400A07A RID: 41082
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
		}
	}
}
