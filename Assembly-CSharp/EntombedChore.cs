using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
	// Token: 0x060016F6 RID: 5878 RVA: 0x0007C050 File Offset: 0x0007A250
	public EntombedChore(IStateMachineTarget target, string entombedAnimOverride) : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EntombedChore.StatesInstance(this, target.gameObject, entombedAnimOverride);
	}

	// Token: 0x020011B5 RID: 4533
	public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
	{
		// Token: 0x060080D8 RID: 32984 RVA: 0x00313352 File Offset: 0x00311552
		public StatesInstance(EntombedChore master, GameObject entombable, string entombedAnimOverride) : base(master)
		{
			base.sm.entombable.Set(entombable, base.smi, false);
			this.entombedAnimOverride = entombedAnimOverride;
		}

		// Token: 0x060080D9 RID: 32985 RVA: 0x0031337C File Offset: 0x0031157C
		public void UpdateFaceEntombed()
		{
			int num = Grid.CellAbove(Grid.PosToCell(base.transform.GetPosition()));
			base.sm.isFaceEntombed.Set(Grid.IsValidCell(num) && Grid.Solid[num], base.smi, false);
		}

		// Token: 0x04006115 RID: 24853
		public string entombedAnimOverride;
	}

	// Token: 0x020011B6 RID: 4534
	public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
	{
		// Token: 0x060080DA RID: 32986 RVA: 0x003133D0 File Offset: 0x003115D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.entombedbody;
			base.Target(this.entombable);
			this.root.ToggleAnims((EntombedChore.StatesInstance smi) => smi.entombedAnimOverride).Update("IsFaceEntombed", delegate(EntombedChore.StatesInstance smi, float dt)
			{
				smi.UpdateFaceEntombed();
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore, null);
			this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
			this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>(this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
		}

		// Token: 0x04006116 RID: 24854
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;

		// Token: 0x04006117 RID: 24855
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;

		// Token: 0x04006118 RID: 24856
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;

		// Token: 0x04006119 RID: 24857
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;
	}
}
