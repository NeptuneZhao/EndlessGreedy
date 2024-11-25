using System;

// Token: 0x02000776 RID: 1910
public class StorageMeter : KMonoBehaviour
{
	// Token: 0x060033A4 RID: 13220 RVA: 0x0011AE3A File Offset: 0x0011903A
	public void SetInterpolateFunction(Func<float, int, float> func)
	{
		this.interpolateFunction = func;
		if (this.meter != null)
		{
			this.meter.interpolateFunction = this.interpolateFunction;
		}
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x0011AE5C File Offset: 0x0011905C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x0011AE64 File Offset: 0x00119064
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_frame",
			"meter_level"
		});
		this.meter.interpolateFunction = this.interpolateFunction;
		this.UpdateMeter(null);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x0011AEE3 File Offset: 0x001190E3
	private void UpdateMeter(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x04001E96 RID: 7830
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001E97 RID: 7831
	private MeterController meter;

	// Token: 0x04001E98 RID: 7832
	private Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);
}
