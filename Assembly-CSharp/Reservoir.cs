using System;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
	// Token: 0x06004CE0 RID: 19680 RVA: 0x001B74DC File Offset: 0x001B56DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_fill",
			"meter_OL"
		});
		base.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
		this.OnStorageChange(null);
	}

	// Token: 0x06004CE1 RID: 19681 RVA: 0x001B753B File Offset: 0x001B573B
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x04003318 RID: 13080
	private MeterController meter;

	// Token: 0x04003319 RID: 13081
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400331A RID: 13082
	private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate(Reservoir component, object data)
	{
		component.OnStorageChange(data);
	});
}
