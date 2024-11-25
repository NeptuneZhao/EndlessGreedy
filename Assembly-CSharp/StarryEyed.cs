using System;

// Token: 0x02000B19 RID: 2841
[SkipSaveFileSerialization]
public class StarryEyed : StateMachineComponent<StarryEyed.StatesInstance>
{
	// Token: 0x06005492 RID: 21650 RVA: 0x001E3EF5 File Offset: 0x001E20F5
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001B65 RID: 7013
	public class StatesInstance : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.GameInstance
	{
		// Token: 0x0600A35E RID: 41822 RVA: 0x003899BE File Offset: 0x00387BBE
		public StatesInstance(StarryEyed master) : base(master)
		{
		}

		// Token: 0x0600A35F RID: 41823 RVA: 0x003899C8 File Offset: 0x00387BC8
		public bool IsInSpace()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			return myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
		}
	}

	// Token: 0x02001B66 RID: 7014
	public class States : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed>
	{
		// Token: 0x0600A360 RID: 41824 RVA: 0x00389A08 File Offset: 0x00387C08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(StarryEyed.StatesInstance smi)
			{
				if (smi.IsInSpace())
				{
					smi.GoTo(this.inSpace);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.inSpace, (StarryEyed.StatesInstance smi) => smi.IsInSpace());
			this.inSpace.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.idle, (StarryEyed.StatesInstance smi) => !smi.IsInSpace()).ToggleEffect("StarryEyed");
		}

		// Token: 0x04007F92 RID: 32658
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State idle;

		// Token: 0x04007F93 RID: 32659
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State inSpace;
	}
}
