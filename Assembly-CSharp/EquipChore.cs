using System;

// Token: 0x0200043A RID: 1082
public class EquipChore : Chore<EquipChore.StatesInstance>
{
	// Token: 0x060016F7 RID: 5879 RVA: 0x0007C098 File Offset: 0x0007A298
	public EquipChore(IStateMachineTarget equippable) : base(Db.Get().ChoreTypes.Equip, equippable, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EquipChore.StatesInstance(this);
		base.smi.sm.equippable_source.Set(equippable.gameObject, base.smi, false);
		base.smi.sm.requested_units.Set(1f, base.smi, false);
		this.showAvailabilityInHoverText = false;
		Prioritizable.AddRef(equippable.gameObject);
		Game.Instance.Trigger(1980521255, equippable.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, equippable.GetComponent<Assignable>());
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, equippable.GetComponent<Pickupable>());
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x0007C16C File Offset: 0x0007A36C
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			Debug.LogError("EquipChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			Debug.LogError("EquipChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			Debug.LogError("EquipChore null smi.sm");
			return;
		}
		if (base.smi.sm.equippable_source == null)
		{
			Debug.LogError("EquipChore null smi.sm.equippable_source");
			return;
		}
		base.smi.sm.equipper.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x020011B7 RID: 4535
	public class StatesInstance : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.GameInstance
	{
		// Token: 0x060080DC RID: 32988 RVA: 0x003134B9 File Offset: 0x003116B9
		public StatesInstance(EquipChore master) : base(master)
		{
		}
	}

	// Token: 0x020011B8 RID: 4536
	public class States : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore>
	{
		// Token: 0x060080DD RID: 32989 RVA: 0x003134C4 File Offset: 0x003116C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.equipper);
			this.root.DoNothing();
			this.fetch.InitializeStates(this.equipper, this.equippable_source, this.equippable_result, this.requested_units, this.actual_units, this.equip, null);
			this.equip.ToggleWork<EquippableWorkable>(this.equippable_result, null, null, null);
		}

		// Token: 0x0400611A RID: 24858
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equipper;

		// Token: 0x0400611B RID: 24859
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_source;

		// Token: 0x0400611C RID: 24860
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_result;

		// Token: 0x0400611D RID: 24861
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter requested_units;

		// Token: 0x0400611E RID: 24862
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter actual_units;

		// Token: 0x0400611F RID: 24863
		public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FetchSubState fetch;

		// Token: 0x04006120 RID: 24864
		public EquipChore.States.Equip equip;

		// Token: 0x020023BB RID: 9147
		public class Equip : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State
		{
			// Token: 0x04009F85 RID: 40837
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pre;

			// Token: 0x04009F86 RID: 40838
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pst;
		}
	}
}
