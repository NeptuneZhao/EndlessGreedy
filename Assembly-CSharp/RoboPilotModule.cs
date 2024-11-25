using System;

// Token: 0x02000ADA RID: 2778
public class RoboPilotModule : KMonoBehaviour
{
	// Token: 0x0600528F RID: 21135 RVA: 0x001D9878 File Offset: 0x001D7A78
	protected override void OnSpawn()
	{
		this.databankStorage = base.GetComponent<Storage>();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.UpdateMeter(null);
		this.databankStorage.SetOffsets(RoboPilotModule.dataDeliveryOffsets);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Subscribe(-887025858, new Action<object>(this.OnRocketLanded));
		base.GetComponent<RocketModuleCluster>().CraftInterface.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
	}

	// Token: 0x06005290 RID: 21136 RVA: 0x001D994C File Offset: 0x001D7B4C
	private void OnLaunchConditionChanged(object data)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component.CraftInterface.IsLaunchRequested())
		{
			component.CraftInterface.GetComponent<Clustercraft>().Launch(false);
		}
	}

	// Token: 0x06005291 RID: 21137 RVA: 0x001D9980 File Offset: 0x001D7B80
	private void OnRocketLanded(object o)
	{
		if (this.consumeDataBanksOnLand)
		{
			LaunchConditionManager lcm = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcm);
			float amount = Math.Min((float)(SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id).OneBasedDistance * this.dataBankConsumption), this.databankStorage.MassStored());
			this.databankStorage.ConsumeIgnoringDisease(this.dataBankType, amount);
		}
	}

	// Token: 0x06005292 RID: 21138 RVA: 0x001D99ED File Offset: 0x001D7BED
	public void ConsumeDataBanksInFlight()
	{
		this.databankStorage.ConsumeIgnoringDisease(this.dataBankType, (float)this.dataBankConsumption);
	}

	// Token: 0x06005293 RID: 21139 RVA: 0x001D9A07 File Offset: 0x001D7C07
	private void UpdateMeter(object data = null)
	{
		this.meter.SetPositionPercent(this.databankStorage.MassStored() / this.databankStorage.Capacity());
	}

	// Token: 0x06005294 RID: 21140 RVA: 0x001D9A2B File Offset: 0x001D7C2B
	public bool HasResourcesToMove(int distance)
	{
		return this.databankStorage.MassStored() > (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x06005295 RID: 21141 RVA: 0x001D9A43 File Offset: 0x001D7C43
	public float GetDataBanksStored()
	{
		if (!(this.databankStorage != null))
		{
			return 0f;
		}
		return this.databankStorage.MassStored();
	}

	// Token: 0x06005296 RID: 21142 RVA: 0x001D9A64 File Offset: 0x001D7C64
	public float FlightEfficiencyModifier()
	{
		if (this.GetDataBanksStored() > 0f)
		{
			return this.flightEfficiencyModifier;
		}
		return 0f;
	}

	// Token: 0x04003670 RID: 13936
	private MeterController meter;

	// Token: 0x04003671 RID: 13937
	private Storage databankStorage;

	// Token: 0x04003672 RID: 13938
	public int dataBankConsumption = 2;

	// Token: 0x04003673 RID: 13939
	public Tag dataBankType;

	// Token: 0x04003674 RID: 13940
	private float flightEfficiencyModifier = 0.1f;

	// Token: 0x04003675 RID: 13941
	public bool consumeDataBanksOnLand;

	// Token: 0x04003676 RID: 13942
	private static CellOffset[] dataDeliveryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(2, 0),
		new CellOffset(3, 0),
		new CellOffset(-1, 0),
		new CellOffset(-2, 0),
		new CellOffset(-3, 0)
	};
}
