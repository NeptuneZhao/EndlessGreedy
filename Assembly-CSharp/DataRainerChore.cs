using System;
using TUNING;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class DataRainerChore : Chore<DataRainerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060016E6 RID: 5862 RVA: 0x0007BA1C File Offset: 0x00079C1C
	public DataRainerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DataRainerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x0007BAB6 File Offset: 0x00079CB6
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000CF3 RID: 3315
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x020011AA RID: 4522
	public class States : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore>
	{
		// Token: 0x060080B3 RID: 32947 RVA: 0x00312408 File Offset: 0x00310608
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.dataRainer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.goToStand, (DataRainerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((DataRainerChore.StatesInstance smi) => smi.GetTargetCell(), this.raining, this.idle, false);
			this.raining.ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.raining.loop).Update(delegate(DataRainerChore.StatesInstance smi, float dt)
			{
				this.nextBankTimer.Delta(dt, smi);
				if (this.nextBankTimer.Get(smi) >= DataRainer.databankSpawnInterval)
				{
					this.nextBankTimer.Delta(-DataRainer.databankSpawnInterval, smi);
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("PowerStationTools"), smi.master.transform.position + Vector3.up);
					gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Iron, true);
					gameObject.SetActive(true);
					KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
					float num = (float)component.currentFrame / (float)component.GetCurrentNumFrames();
					Vector2 initial_velocity = new Vector2((num < 0.5f) ? -2.5f : 2.5f, 4f);
					if (GameComps.Fallers.Has(gameObject))
					{
						GameComps.Fallers.Remove(gameObject);
					}
					GameComps.Fallers.Add(gameObject, initial_velocity);
					DataRainer.Instance smi2 = this.dataRainer.Get(smi).GetSMI<DataRainer.Instance>();
					DataRainer sm = smi2.sm;
					sm.databanksCreated.Set(sm.databanksCreated.Get(smi2) + 1, smi2, false);
				}
			}, UpdateRate.SIM_33ms, false);
			this.raining.loop.PlayAnim("makeitrain2", KAnim.PlayMode.Loop);
		}

		// Token: 0x040060EF RID: 24815
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.TargetParameter dataRainer;

		// Token: 0x040060F0 RID: 24816
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter nextBankTimer = new StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter(DataRainer.databankSpawnInterval / 2f);

		// Token: 0x040060F1 RID: 24817
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State idle;

		// Token: 0x040060F2 RID: 24818
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State goToStand;

		// Token: 0x040060F3 RID: 24819
		public DataRainerChore.States.RainingStates raining;

		// Token: 0x020023B1 RID: 9137
		public class RainingStates : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State
		{
			// Token: 0x04009F63 RID: 40803
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pre;

			// Token: 0x04009F64 RID: 40804
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State loop;

			// Token: 0x04009F65 RID: 40805
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pst;
		}
	}

	// Token: 0x020011AB RID: 4523
	public class StatesInstance : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.GameInstance
	{
		// Token: 0x060080B6 RID: 32950 RVA: 0x00312630 File Offset: 0x00310830
		public StatesInstance(DataRainerChore master, GameObject dataRainer) : base(master)
		{
			this.dataRainer = dataRainer;
			base.sm.dataRainer.Set(dataRainer, base.smi, false);
		}

		// Token: 0x060080B7 RID: 32951 RVA: 0x00312659 File Offset: 0x00310859
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060080B8 RID: 32952 RVA: 0x0031267C File Offset: 0x0031087C
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x040060F4 RID: 24820
		private GameObject dataRainer;
	}
}
