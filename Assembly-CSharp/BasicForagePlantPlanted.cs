using System;
using UnityEngine;

// Token: 0x020009EB RID: 2539
public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance>
{
	// Token: 0x06004995 RID: 18837 RVA: 0x001A56C9 File Offset: 0x001A38C9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x001A56DC File Offset: 0x001A38DC
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04003027 RID: 12327
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04003028 RID: 12328
	[MyCmpReq]
	private SeedProducer seedProducer;

	// Token: 0x04003029 RID: 12329
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x020019EF RID: 6639
	public class StatesInstance : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.GameInstance
	{
		// Token: 0x06009E76 RID: 40566 RVA: 0x0037801D File Offset: 0x0037621D
		public StatesInstance(BasicForagePlantPlanted smi) : base(smi)
		{
		}
	}

	// Token: 0x020019F0 RID: 6640
	public class States : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted>
	{
		// Token: 0x06009E77 RID: 40567 RVA: 0x00378028 File Offset: 0x00376228
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.idle.PlayAnim("idle").EventTransition(GameHashes.Harvest, this.alive.harvest, null).Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.harvestable.SetCanBeHarvested(true);
			});
			this.alive.harvest.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.seedProducer.DropSeed(null);
			}).GoTo(this.dead);
			this.dead.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.animController.StopAndClear();
				UnityEngine.Object.Destroy(smi.master.animController);
				smi.master.DestroySelf(null);
			});
		}

		// Token: 0x04007AE1 RID: 31457
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State seed_grow;

		// Token: 0x04007AE2 RID: 31458
		public BasicForagePlantPlanted.States.AliveStates alive;

		// Token: 0x04007AE3 RID: 31459
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State dead;

		// Token: 0x020025D0 RID: 9680
		public class AliveStates : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.PlantAliveSubState
		{
			// Token: 0x0400A855 RID: 43093
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State idle;

			// Token: 0x0400A856 RID: 43094
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvest;
		}
	}
}
