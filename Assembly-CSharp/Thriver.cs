using System;

// Token: 0x02000B33 RID: 2867
[SkipSaveFileSerialization]
public class Thriver : StateMachineComponent<Thriver.StatesInstance>
{
	// Token: 0x0600557F RID: 21887 RVA: 0x001E8D37 File Offset: 0x001E6F37
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001B88 RID: 7048
	public class StatesInstance : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.GameInstance
	{
		// Token: 0x0600A3B0 RID: 41904 RVA: 0x0038A938 File Offset: 0x00388B38
		public StatesInstance(Thriver master) : base(master)
		{
		}

		// Token: 0x0600A3B1 RID: 41905 RVA: 0x0038A944 File Offset: 0x00388B44
		public bool IsStressed()
		{
			StressMonitor.Instance smi = base.master.GetSMI<StressMonitor.Instance>();
			return smi != null && smi.IsStressed();
		}
	}

	// Token: 0x02001B89 RID: 7049
	public class States : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver>
	{
		// Token: 0x0600A3B2 RID: 41906 RVA: 0x0038A968 File Offset: 0x00388B68
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.EventTransition(GameHashes.NotStressed, this.idle, null).EventTransition(GameHashes.Stressed, this.stressed, null).EventTransition(GameHashes.StressedHadEnough, this.stressed, null).Enter(delegate(Thriver.StatesInstance smi)
			{
				StressMonitor.Instance smi2 = smi.master.GetSMI<StressMonitor.Instance>();
				if (smi2 != null && smi2.IsStressed())
				{
					smi.GoTo(this.stressed);
				}
			});
			this.idle.DoNothing();
			this.stressed.ToggleEffect("Thriver");
			this.toostressed.DoNothing();
		}

		// Token: 0x04007FFD RID: 32765
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State idle;

		// Token: 0x04007FFE RID: 32766
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State stressed;

		// Token: 0x04007FFF RID: 32767
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State toostressed;
	}
}
