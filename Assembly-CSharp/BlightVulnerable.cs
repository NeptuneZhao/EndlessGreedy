using System;

// Token: 0x020007F2 RID: 2034
[SkipSaveFileSerialization]
public class BlightVulnerable : StateMachineComponent<BlightVulnerable.StatesInstance>
{
	// Token: 0x0600383B RID: 14395 RVA: 0x001333F9 File Offset: 0x001315F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00133401 File Offset: 0x00131601
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x00133414 File Offset: 0x00131614
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0013341C File Offset: 0x0013161C
	public void MakeBlighted()
	{
		Debug.Log("Blighting plant", this);
		base.smi.sm.isBlighted.Set(true, base.smi, false);
	}

	// Token: 0x040021C8 RID: 8648
	private SchedulerHandle handle;

	// Token: 0x040021C9 RID: 8649
	public bool prefersDarkness;

	// Token: 0x020016C9 RID: 5833
	public class StatesInstance : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.GameInstance
	{
		// Token: 0x06009389 RID: 37769 RVA: 0x0035989C File Offset: 0x00357A9C
		public StatesInstance(BlightVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x020016CA RID: 5834
	public class States : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable>
	{
		// Token: 0x0600938A RID: 37770 RVA: 0x003598A8 File Offset: 0x00357AA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.comfortable.ParamTransition<bool>(this.isBlighted, this.blighted, GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.IsTrue);
			this.blighted.TriggerOnEnter(GameHashes.BlightChanged, (BlightVulnerable.StatesInstance smi) => true).Enter(delegate(BlightVulnerable.StatesInstance smi)
			{
				smi.GetComponent<SeedProducer>().seedInfo.seedId = RotPileConfig.ID;
			}).ToggleTag(GameTags.Blighted).Exit(delegate(BlightVulnerable.StatesInstance smi)
			{
				GameplayEventManager.Instance.Trigger(-1425542080, smi.gameObject);
			});
		}

		// Token: 0x040070D4 RID: 28884
		public StateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.BoolParameter isBlighted;

		// Token: 0x040070D5 RID: 28885
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State comfortable;

		// Token: 0x040070D6 RID: 28886
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State blighted;
	}
}
