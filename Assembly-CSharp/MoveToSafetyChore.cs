using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
	// Token: 0x06001740 RID: 5952 RVA: 0x0007DDD0 File Offset: 0x0007BFD0
	public MoveToSafetyChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020011D8 RID: 4568
	public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
	{
		// Token: 0x0600814B RID: 33099 RVA: 0x003168D8 File Offset: 0x00314AD8
		public StatesInstance(MoveToSafetyChore master, GameObject mover) : base(master)
		{
			base.sm.mover.Set(mover, base.smi, false);
			this.sensor = base.sm.mover.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>();
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x0600814C RID: 33100 RVA: 0x00316937 File Offset: 0x00314B37
		public void UpdateTargetCell()
		{
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x04006198 RID: 24984
		private SafeCellSensor sensor;

		// Token: 0x04006199 RID: 24985
		public int targetCell;
	}

	// Token: 0x020011D9 RID: 4569
	public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
	{
		// Token: 0x0600814D RID: 33101 RVA: 0x0031694C File Offset: 0x00314B4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.mover);
			this.root.ToggleTag(GameTags.Idle);
			this.move.Enter("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi)
			{
				smi.UpdateTargetCell();
			}).Update("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi, float dt)
			{
				smi.UpdateTargetCell();
			}, UpdateRate.SIM_200ms, false).MoveTo((MoveToSafetyChore.StatesInstance smi) => smi.targetCell, null, null, true);
		}

		// Token: 0x0400619A RID: 24986
		public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;

		// Token: 0x0400619B RID: 24987
		public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;
	}
}
