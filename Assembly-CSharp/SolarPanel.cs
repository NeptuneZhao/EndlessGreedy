using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000767 RID: 1895
[SerializationConfig(MemberSerialization.OptIn)]
public class SolarPanel : Generator
{
	// Token: 0x060032FE RID: 13054 RVA: 0x001181BC File Offset: 0x001163BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SolarPanel>(824508782, SolarPanel.OnActiveChangedDelegate);
		this.smi = new SolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x00118256 File Offset: 0x00116456
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x00118284 File Offset: 0x00116484
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x001182DC File Offset: 0x001164DC
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.SolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.SolarPanelWattage, this);
		}
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x00118370 File Offset: 0x00116570
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		foreach (CellOffset offset in this.solarCellOffsets)
		{
			int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), offset)];
			num += (float)num2 * 0.00053f;
		}
		this.operational.SetActive(num > 0f, false);
		num = Mathf.Clamp(num, 0f, 380f);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 380f);
		this.UpdateStatusItem();
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06003303 RID: 13059 RVA: 0x00118488 File Offset: 0x00116688
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001E1C RID: 7708
	private MeterController meter;

	// Token: 0x04001E1D RID: 7709
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001E1E RID: 7710
	private SolarPanel.StatesInstance smi;

	// Token: 0x04001E1F RID: 7711
	private Guid statusHandle;

	// Token: 0x04001E20 RID: 7712
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-3, 2),
		new CellOffset(-2, 2),
		new CellOffset(-1, 2),
		new CellOffset(0, 2),
		new CellOffset(1, 2),
		new CellOffset(2, 2),
		new CellOffset(3, 2),
		new CellOffset(-3, 1),
		new CellOffset(-2, 1),
		new CellOffset(-1, 1),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(2, 1),
		new CellOffset(3, 1)
	};

	// Token: 0x04001E21 RID: 7713
	private static readonly EventSystem.IntraObjectHandler<SolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<SolarPanel>(delegate(SolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x020015F3 RID: 5619
	public class StatesInstance : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.GameInstance
	{
		// Token: 0x0600907D RID: 36989 RVA: 0x0034BBD5 File Offset: 0x00349DD5
		public StatesInstance(SolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x020015F4 RID: 5620
	public class States : GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel>
	{
		// Token: 0x0600907E RID: 36990 RVA: 0x0034BBDE File Offset: 0x00349DDE
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DoNothing();
		}

		// Token: 0x04006E43 RID: 28227
		public GameStateMachine<SolarPanel.States, SolarPanel.StatesInstance, SolarPanel, object>.State idle;
	}
}
