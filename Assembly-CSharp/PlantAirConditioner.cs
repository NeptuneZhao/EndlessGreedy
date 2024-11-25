using System;
using KSerialization;

// Token: 0x0200074B RID: 1867
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
	// Token: 0x060031B2 RID: 12722 RVA: 0x001118DC File Offset: 0x0010FADC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
		base.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x00111906 File Offset: 0x0010FB06
	private void OnFertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, true);
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x00111919 File Offset: 0x0010FB19
	private void OnUnfertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, false);
	}

	// Token: 0x04001D45 RID: 7493
	private static readonly Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);

	// Token: 0x04001D46 RID: 7494
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnFertilized(data);
	});

	// Token: 0x04001D47 RID: 7495
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnUnfertilized(data);
	});
}
