using System;

// Token: 0x0200068F RID: 1679
public class Campfire : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>
{
	// Token: 0x060029E4 RID: 10724 RVA: 0x000EC25C File Offset: 0x000EA45C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.needsFuel);
		this.operational.needsFuel.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.working, new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel)).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.working.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.EnableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.needsFuel, GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Not(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel))).PlayAnim("on", KAnim.PlayMode.Loop).Exit(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission));
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000EC37C File Offset: 0x000EA57C
	public static bool HasFuel(Campfire.Instance smi)
	{
		return smi.HasFuel;
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000EC384 File Offset: 0x000EA584
	public static void EnableHeatEmission(Campfire.Instance smi)
	{
		smi.EnableHeatEmission();
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000EC38C File Offset: 0x000EA58C
	public static void DisableHeatEmission(Campfire.Instance smi)
	{
		smi.DisableHeatEmission();
	}

	// Token: 0x0400181F RID: 6175
	public const string LIT_ANIM_NAME = "on";

	// Token: 0x04001820 RID: 6176
	public const string UNLIT_ANIM_NAME = "off";

	// Token: 0x04001821 RID: 6177
	public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State noOperational;

	// Token: 0x04001822 RID: 6178
	public Campfire.OperationalStates operational;

	// Token: 0x04001823 RID: 6179
	public StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.BoolParameter WarmAuraEnabled;

	// Token: 0x02001475 RID: 5237
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040069CE RID: 27086
		public Tag fuelTag;

		// Token: 0x040069CF RID: 27087
		public float initialFuelMass;
	}

	// Token: 0x02001476 RID: 5238
	public class OperationalStates : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State
	{
		// Token: 0x040069D0 RID: 27088
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State needsFuel;

		// Token: 0x040069D1 RID: 27089
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State working;
	}

	// Token: 0x02001477 RID: 5239
	public new class Instance : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.GameInstance
	{
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06008ABA RID: 35514 RVA: 0x003348AA File Offset: 0x00332AAA
		public bool HasFuel
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06008ABB RID: 35515 RVA: 0x003348BE File Offset: 0x00332ABE
		public bool IsAuraEnabled
		{
			get
			{
				return base.sm.WarmAuraEnabled.Get(this);
			}
		}

		// Token: 0x06008ABC RID: 35516 RVA: 0x003348D1 File Offset: 0x00332AD1
		public Instance(IStateMachineTarget master, Campfire.Def def) : base(master, def)
		{
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x003348DC File Offset: 0x00332ADC
		public void EnableHeatEmission()
		{
			this.operational.SetActive(true, false);
			this.light.enabled = true;
			this.heater.EnableEmission = true;
			this.decorProvider.SetValues(CampfireConfig.DECOR_ON);
			this.decorProvider.Refresh();
		}

		// Token: 0x06008ABE RID: 35518 RVA: 0x0033492C File Offset: 0x00332B2C
		public void DisableHeatEmission()
		{
			this.operational.SetActive(false, false);
			this.light.enabled = false;
			this.heater.EnableEmission = false;
			this.decorProvider.SetValues(CampfireConfig.DECOR_OFF);
			this.decorProvider.Refresh();
		}

		// Token: 0x040069D2 RID: 27090
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040069D3 RID: 27091
		[MyCmpGet]
		public Storage storage;

		// Token: 0x040069D4 RID: 27092
		[MyCmpGet]
		public RangeVisualizer rangeVisualizer;

		// Token: 0x040069D5 RID: 27093
		[MyCmpGet]
		public Light2D light;

		// Token: 0x040069D6 RID: 27094
		[MyCmpGet]
		public DirectVolumeHeater heater;

		// Token: 0x040069D7 RID: 27095
		[MyCmpGet]
		public DecorProvider decorProvider;
	}
}
