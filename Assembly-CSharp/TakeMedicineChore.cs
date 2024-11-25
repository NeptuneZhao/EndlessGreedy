using System;
using STRINGS;

// Token: 0x0200045C RID: 1116
public class TakeMedicineChore : Chore<TakeMedicineChore.StatesInstance>
{
	// Token: 0x0600177F RID: 6015 RVA: 0x0007F5C0 File Offset: 0x0007D7C0
	public TakeMedicineChore(MedicinalPillWorkable master) : base(Db.Get().ChoreTypes.TakeMedicine, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.medicine = master;
		this.pickupable = this.medicine.GetComponent<Pickupable>();
		base.smi = new TakeMedicineChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, this.pickupable);
		this.AddPrecondition(TakeMedicineChore.CanCure, this);
		this.AddPrecondition(TakeMedicineChore.IsConsumptionPermitted, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x0007F654 File Offset: 0x0007D854
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.source.Set(this.pickupable.gameObject, base.smi, false);
		base.smi.sm.requestedpillcount.Set(1f, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		new TakeMedicineChore(this.medicine);
	}

	// Token: 0x04000D1C RID: 3356
	private Pickupable pickupable;

	// Token: 0x04000D1D RID: 3357
	private MedicinalPillWorkable medicine;

	// Token: 0x04000D1E RID: 3358
	public static readonly Chore.Precondition CanCure = new Chore.Precondition
	{
		id = "CanCure",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CURE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((TakeMedicineChore)data).medicine.CanBeTakenBy(context.consumerState.gameObject);
		}
	};

	// Token: 0x04000D1F RID: 3359
	public static readonly Chore.Precondition IsConsumptionPermitted = new Chore.Precondition
	{
		id = "IsConsumptionPermitted",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CONSUMPTION_PERMITTED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			TakeMedicineChore takeMedicineChore = (TakeMedicineChore)data;
			ConsumableConsumer consumableConsumer = context.consumerState.consumableConsumer;
			return consumableConsumer == null || consumableConsumer.IsPermitted(takeMedicineChore.medicine.PrefabID().Name);
		}
	};

	// Token: 0x0200120A RID: 4618
	public class StatesInstance : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.GameInstance
	{
		// Token: 0x060081FF RID: 33279 RVA: 0x0031B0F1 File Offset: 0x003192F1
		public StatesInstance(TakeMedicineChore master) : base(master)
		{
		}
	}

	// Token: 0x0200120B RID: 4619
	public class States : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore>
	{
		// Token: 0x06008200 RID: 33280 RVA: 0x0031B0FC File Offset: 0x003192FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.fetch.InitializeStates(this.eater, this.source, this.chunk, this.requestedpillcount, this.actualpillcount, this.takemedicine, null);
			this.takemedicine.ToggleAnims("anim_eat_floor_kanim", 0f).ToggleTag(GameTags.TakingMedicine).ToggleWork("TakeMedicine", delegate(TakeMedicineChore.StatesInstance smi)
			{
				MedicinalPillWorkable workable = this.chunk.Get<MedicinalPillWorkable>(smi);
				this.eater.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (TakeMedicineChore.StatesInstance smi) => this.chunk.Get<MedicinalPill>(smi) != null, null, null);
		}

		// Token: 0x04006233 RID: 25139
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter eater;

		// Token: 0x04006234 RID: 25140
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter source;

		// Token: 0x04006235 RID: 25141
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter chunk;

		// Token: 0x04006236 RID: 25142
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter requestedpillcount;

		// Token: 0x04006237 RID: 25143
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter actualpillcount;

		// Token: 0x04006238 RID: 25144
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FetchSubState fetch;

		// Token: 0x04006239 RID: 25145
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.State takemedicine;
	}
}
