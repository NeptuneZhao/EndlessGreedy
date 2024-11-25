using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000739 RID: 1849
[SerializationConfig(MemberSerialization.OptIn)]
public class ModuleSolarPanel : Generator
{
	// Token: 0x0600311C RID: 12572 RVA: 0x0010F1F4 File Offset: 0x0010D3F4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.IsVirtual = true;
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x0010F204 File Offset: 0x0010D404
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		base.Subscribe<ModuleSolarPanel>(824508782, ModuleSolarPanel.OnActiveChangedDelegate);
		this.smi = new ModuleSolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		Grid.PosToCell(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x0010F2DA File Offset: 0x0010D4DA
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x0010F308 File Offset: 0x0010D508
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x0010F360 File Offset: 0x0010D560
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
		}
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x0010F3F4 File Offset: 0x0010D5F4
	public override void EnergySim200ms(float dt)
	{
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, true);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (Grid.IsValidCell(Grid.PosToCell(this)) && Grid.WorldIdx[Grid.PosToCell(this)] != 255)
		{
			foreach (CellOffset offset in this.solarCellOffsets)
			{
				int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), offset)];
				num += (float)num2 * 0.00053f;
			}
		}
		else
		{
			num = 60f;
		}
		num = Mathf.Clamp(num, 0f, 60f);
		this.operational.SetActive(num > 0f, false);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 60f);
		this.UpdateStatusItem();
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06003122 RID: 12578 RVA: 0x0010F532 File Offset: 0x0010D732
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001CE2 RID: 7394
	private MeterController meter;

	// Token: 0x04001CE3 RID: 7395
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001CE4 RID: 7396
	private ModuleSolarPanel.StatesInstance smi;

	// Token: 0x04001CE5 RID: 7397
	private Guid statusHandle;

	// Token: 0x04001CE6 RID: 7398
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04001CE7 RID: 7399
	private static readonly EventSystem.IntraObjectHandler<ModuleSolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ModuleSolarPanel>(delegate(ModuleSolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x0200159B RID: 5531
	public class StatesInstance : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.GameInstance
	{
		// Token: 0x06008F3C RID: 36668 RVA: 0x00346DC9 File Offset: 0x00344FC9
		public StatesInstance(ModuleSolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x0200159C RID: 5532
	public class States : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel>
	{
		// Token: 0x06008F3D RID: 36669 RVA: 0x00346DD2 File Offset: 0x00344FD2
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.EventTransition(GameHashes.DoLaunchRocket, this.launch, null).DoNothing();
			this.launch.EventTransition(GameHashes.RocketLanded, this.idle, null);
		}

		// Token: 0x04006D6A RID: 28010
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State idle;

		// Token: 0x04006D6B RID: 28011
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State launch;
	}
}
