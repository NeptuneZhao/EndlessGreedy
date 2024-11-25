using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class PeeChore : Chore<PeeChore.StatesInstance>
{
	// Token: 0x06001745 RID: 5957 RVA: 0x0007DF78 File Offset: 0x0007C178
	public PeeChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Pee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PeeChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020011DC RID: 4572
	public class StatesInstance : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.GameInstance
	{
		// Token: 0x06008153 RID: 33107 RVA: 0x00316AD8 File Offset: 0x00314CD8
		public StatesInstance(PeeChore master, GameObject worker) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(worker);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(worker);
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x00316B7D File Offset: 0x00314D7D
		public bool IsDonePeeing()
		{
			return this.bladder.value <= 0f;
		}

		// Token: 0x06008155 RID: 33109 RVA: 0x00316B94 File Offset: 0x00314D94
		public void SpawnDirtyWater(float dt)
		{
			int gameCell = Grid.PosToCell(base.sm.worker.Get<KMonoBehaviour>(base.smi));
			byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE);
			float num = dt * -this.bladder.GetDelta() / this.bladder.GetMax();
			if (num > 0f)
			{
				float mass = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_FLOOR_PEE * num;
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(SimHashes.DirtyWater, mass, this.bodyTemperature.value, index, Mathf.CeilToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE * num), false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(gameCell, SimHashes.DirtyWater, CellEventLogger.Instance.Vomit, mass, this.bodyTemperature.value, index, Mathf.CeilToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE * num), true, -1);
			}
		}

		// Token: 0x040061A2 RID: 24994
		public Notification stressfullyEmptyingBladder = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x040061A3 RID: 24995
		public AmountInstance bladder;

		// Token: 0x040061A4 RID: 24996
		private AmountInstance bodyTemperature;
	}

	// Token: 0x020011DD RID: 4573
	public class States : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore>
	{
		// Token: 0x06008156 RID: 33110 RVA: 0x00316CA4 File Offset: 0x00314EA4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.running;
			base.Target(this.worker);
			this.running.ToggleAnims("anim_expel_kanim", 0f).ToggleEffect("StressfulyEmptyingBladder").DoNotification((PeeChore.StatesInstance smi) => smi.stressfullyEmptyingBladder).DoReport(ReportManager.ReportType.ToiletIncident, (PeeChore.StatesInstance smi) => 1f, (PeeChore.StatesInstance smi) => this.masterTarget.Get(smi).GetProperName()).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Transition(null, (PeeChore.StatesInstance smi) => smi.IsDonePeeing(), UpdateRate.SIM_200ms).Update("SpawnDirtyWater", delegate(PeeChore.StatesInstance smi, float dt)
			{
				smi.SpawnDirtyWater(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop).ToggleTag(GameTags.MakingMess).Enter(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled())
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
					AmountInstance amountInstance = smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id);
					RadiationMonitor.Instance smi2 = smi.master.gameObject.GetSMI<RadiationMonitor.Instance>();
					if (smi2 != null)
					{
						float num = Math.Min(amountInstance.value, 100f * smi2.difficultySettingMod);
						smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
						if (num >= 1f)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
						}
					}
				}
			});
		}

		// Token: 0x040061A5 RID: 24997
		public StateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.TargetParameter worker;

		// Token: 0x040061A6 RID: 24998
		public GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.State running;
	}
}
