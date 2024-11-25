using System;

// Token: 0x02000951 RID: 2385
[SkipSaveFileSerialization]
public class Loner : StateMachineComponent<Loner.StatesInstance>
{
	// Token: 0x0600459E RID: 17822 RVA: 0x0018C9B4 File Offset: 0x0018ABB4
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020018B5 RID: 6325
	public class StatesInstance : GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.GameInstance
	{
		// Token: 0x060099A8 RID: 39336 RVA: 0x0036AB89 File Offset: 0x00368D89
		public StatesInstance(Loner master) : base(master)
		{
		}

		// Token: 0x060099A9 RID: 39337 RVA: 0x0036AB94 File Offset: 0x00368D94
		public bool IsAlone()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			MinionIdentity component = base.GetComponent<MinionIdentity>();
			foreach (object obj in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj;
				if (component != minionIdentity)
				{
					int myWorldId = minionIdentity.GetMyWorldId();
					if (id == myWorldId || parentWorldId == myWorldId)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x020018B6 RID: 6326
	public class States : GameStateMachine<Loner.States, Loner.StatesInstance, Loner>
	{
		// Token: 0x060099AA RID: 39338 RVA: 0x0036AC3C File Offset: 0x00368E3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(Loner.StatesInstance smi)
			{
				if (smi.IsAlone())
				{
					smi.GoTo(this.alone);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone());
			this.alone.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).ToggleEffect("Loner");
		}

		// Token: 0x04007739 RID: 30521
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State idle;

		// Token: 0x0400773A RID: 30522
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State alone;
	}
}
