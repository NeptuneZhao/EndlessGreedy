using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class BionicGunkSpillChore : Chore<BionicGunkSpillChore.StatesInstance>
{
	// Token: 0x060016D2 RID: 5842 RVA: 0x0007A558 File Offset: 0x00078758
	public static void ExpellOilUpdate(BionicGunkSpillChore.StatesInstance smi, float dt)
	{
		float num = GunkMonitor.GUNK_CAPACITY * (dt / 10f);
		if (num >= smi.gunkMonitor.CurrentGunkMass)
		{
			smi.GoTo(smi.sm.pst);
			return;
		}
		smi.gunkMonitor.ExpellGunk(num, null);
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x0007A5A0 File Offset: 0x000787A0
	public BionicGunkSpillChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ExpellGunk, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicGunkSpillChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x04000CB7 RID: 3255
	public const float EVENT_DURATION = 10f;

	// Token: 0x04000CB8 RID: 3256
	public const string PRE_ANIM_NAME = "oiloverload_pre";

	// Token: 0x04000CB9 RID: 3257
	public const string LOOP_ANIM_NAME = "oiloverload_loop";

	// Token: 0x04000CBA RID: 3258
	public const string PST_ANIM_NAME = "overload_pst";

	// Token: 0x020011A4 RID: 4516
	public class States : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore>
	{
		// Token: 0x06008077 RID: 32887 RVA: 0x00310FF8 File Offset: 0x0030F1F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			base.Target(this.worker);
			this.root.ToggleAnims("anim_bionic_oil_overload_kanim", 0f).ToggleEffect("ExpellingGunk").ToggleTag(GameTags.MakingMess).DoNotification((BionicGunkSpillChore.StatesInstance smi) => smi.stressfullyEmptyingGunk).Enter(delegate(BionicGunkSpillChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			});
			this.enter.PlayAnim("oiloverload_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.running.PlayAnim("oiloverload_loop", KAnim.PlayMode.Loop).Update(new Action<BionicGunkSpillChore.StatesInstance, float>(BionicGunkSpillChore.ExpellOilUpdate), UpdateRate.SIM_200ms, false);
			this.pst.PlayAnim("overload_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x040060AB RID: 24747
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State enter;

		// Token: 0x040060AC RID: 24748
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State running;

		// Token: 0x040060AD RID: 24749
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State pst;

		// Token: 0x040060AE RID: 24750
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State complete;

		// Token: 0x040060AF RID: 24751
		public StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.TargetParameter worker;
	}

	// Token: 0x020011A5 RID: 4517
	public class StatesInstance : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.GameInstance
	{
		// Token: 0x06008079 RID: 32889 RVA: 0x00311100 File Offset: 0x0030F300
		public StatesInstance(BionicGunkSpillChore master, GameObject worker) : base(master)
		{
			this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x040060B0 RID: 24752
		public Notification stressfullyEmptyingGunk = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x040060B1 RID: 24753
		public GunkMonitor.Instance gunkMonitor;
	}
}
