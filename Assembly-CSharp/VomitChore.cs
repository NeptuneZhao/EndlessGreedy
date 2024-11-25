using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class VomitChore : Chore<VomitChore.StatesInstance>
{
	// Token: 0x06001784 RID: 6020 RVA: 0x0007F800 File Offset: 0x0007DA00
	public VomitChore(ChoreType chore_type, IStateMachineTarget target, StatusItem status_item, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), true, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new VomitChore.StatesInstance(this, target.gameObject, status_item, notification);
	}

	// Token: 0x02001211 RID: 4625
	public class StatesInstance : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.GameInstance
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06008212 RID: 33298 RVA: 0x0031B630 File Offset: 0x00319830
		// (set) Token: 0x06008211 RID: 33297 RVA: 0x0031B627 File Offset: 0x00319827
		public SimHashes elementToVomit { get; private set; } = SimHashes.DirtyWater;

		// Token: 0x06008213 RID: 33299 RVA: 0x0031B638 File Offset: 0x00319838
		public StatesInstance(VomitChore master, GameObject vomiter, StatusItem status_item, Notification notification) : base(master)
		{
			base.sm.vomiter.Set(vomiter, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
			this.statusItem = status_item;
			this.notification = notification;
			this.vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, base.GetComponent<KMonoBehaviour>(), 10);
			MinionIdentity component = vomiter.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				this.elementToVomit = SimHashes.LiquidGunk;
			}
		}

		// Token: 0x06008214 RID: 33300 RVA: 0x0031B6EC File Offset: 0x003198EC
		private static bool CanEmitLiquid(int cell)
		{
			bool result = true;
			if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (Grid.Properties[cell] & 2) != 0)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06008215 RID: 33301 RVA: 0x0031B722 File Offset: 0x00319922
		public void SpawnDirtyWater(float dt)
		{
			this.SpawnVomitLiquid(dt, SimHashes.DirtyWater);
		}

		// Token: 0x06008216 RID: 33302 RVA: 0x0031B730 File Offset: 0x00319930
		public void SpawnVomitLiquid(float dt, SimHashes element)
		{
			if (dt > 0f)
			{
				float totalTime = base.GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
				float num = dt / totalTime;
				Sicknesses sicknesses = base.master.GetComponent<MinionModifiers>().sicknesses;
				SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
				int num2 = 0;
				while (num2 < sicknesses.Count && sicknesses[num2].modifier.sicknessType != Sickness.SicknessType.Pathogen)
				{
					num2++;
				}
				Facing component = base.sm.vomiter.Get(base.smi).GetComponent<Facing>();
				int num3 = Grid.PosToCell(component.transform.GetPosition());
				int num4 = component.GetFrontCell();
				if (!VomitChore.StatesInstance.CanEmitLiquid(num4))
				{
					num4 = num3;
				}
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(element, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(num4, element, CellEventLogger.Instance.Vomit, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, true, -1);
			}
		}

		// Token: 0x06008217 RID: 33303 RVA: 0x0031B858 File Offset: 0x00319A58
		public int GetVomitCell()
		{
			this.vomitCellQuery.Reset();
			Navigator component = base.GetComponent<Navigator>();
			component.RunQuery(this.vomitCellQuery);
			int num = this.vomitCellQuery.GetResultCell();
			if (Grid.InvalidCell == num)
			{
				num = Grid.PosToCell(component);
			}
			return num;
		}

		// Token: 0x04006244 RID: 25156
		public StatusItem statusItem;

		// Token: 0x04006245 RID: 25157
		private AmountInstance bodyTemperature;

		// Token: 0x04006246 RID: 25158
		public Notification notification;

		// Token: 0x04006247 RID: 25159
		private SafetyQuery vomitCellQuery;
	}

	// Token: 0x02001212 RID: 4626
	public class States : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore>
	{
		// Token: 0x06008218 RID: 33304 RVA: 0x0031B8A0 File Offset: 0x00319AA0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveto;
			base.Target(this.vomiter);
			this.root.ToggleAnims("anim_emotes_default_kanim", 0f);
			this.moveto.TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_vomiter_kanim", 0f).MoveTo((VomitChore.StatesInstance smi) => smi.GetVomitCell(), this.vomit, this.vomit, false);
			this.vomit.DefaultState(this.vomit.buildup).ToggleAnims("anim_vomit_kanim", 0f).ToggleStatusItem((VomitChore.StatesInstance smi) => smi.statusItem, null).DoNotification((VomitChore.StatesInstance smi) => smi.notification).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Enter(delegate(VomitChore.StatesInstance smi)
			{
				if (smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(VomitChore.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
				float num = Mathf.Min(smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value, 20f);
				smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
				if (num >= 1f)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
				}
			});
			this.vomit.buildup.PlayAnim("vomit_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.vomit.release);
			this.vomit.release.ToggleEffect("Vomiting").PlayAnim("vomit_loop", KAnim.PlayMode.Once).Update("SpawnVomitLiquid", delegate(VomitChore.StatesInstance smi, float dt)
			{
				smi.SpawnVomitLiquid(dt, smi.elementToVomit);
			}, UpdateRate.SIM_200ms, false).OnAnimQueueComplete(this.vomit.release_pst);
			this.vomit.release_pst.PlayAnim("vomit_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover);
			this.recover.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true, null).ScheduleGoTo(8f, this.recover_pst);
			this.recover_pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04006249 RID: 25161
		public StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.TargetParameter vomiter;

		// Token: 0x0400624A RID: 25162
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State moveto;

		// Token: 0x0400624B RID: 25163
		public VomitChore.States.VomitState vomit;

		// Token: 0x0400624C RID: 25164
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover;

		// Token: 0x0400624D RID: 25165
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover_pst;

		// Token: 0x0400624E RID: 25166
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State complete;

		// Token: 0x020023F3 RID: 9203
		public class VomitState : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State
		{
			// Token: 0x0400A07F RID: 41087
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State buildup;

			// Token: 0x0400A080 RID: 41088
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release;

			// Token: 0x0400A081 RID: 41089
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release_pst;
		}
	}
}
