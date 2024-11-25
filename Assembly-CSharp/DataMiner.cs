using System;
using UnityEngine;

// Token: 0x0200082E RID: 2094
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class DataMiner : ComplexFabricator
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06003A3B RID: 14907 RVA: 0x0013E3AB File Offset: 0x0013C5AB
	public float OperatingTemp
	{
		get
		{
			return this.pe.Temperature;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x0013E3B8 File Offset: 0x0013C5B8
	public float TemperatureScaleFactor
	{
		get
		{
			return 1f - DataMinerConfig.TEMPERATURE_SCALING_RANGE.LerpFactorClamped(this.OperatingTemp);
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06003A3D RID: 14909 RVA: 0x0013E3D0 File Offset: 0x0013C5D0
	public float EfficiencyRate
	{
		get
		{
			return DataMinerConfig.PRODUCTION_RATE_SCALE.Lerp(this.TemperatureScaleFactor);
		}
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x0013E3E2 File Offset: 0x0013C5E2
	protected override float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		return base.ComputeWorkProgress(dt, recipe) * this.EfficiencyRate;
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x0013E3F3 File Offset: 0x0013C5F3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DataMinerEfficiency, this);
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x0013E42B File Offset: 0x0013C62B
	public override void Sim1000ms(float dt)
	{
		base.Sim1000ms(dt);
		this.meter.SetPositionPercent(this.TemperatureScaleFactor);
	}

	// Token: 0x040022FF RID: 8959
	[MyCmpReq]
	private PrimaryElement pe;

	// Token: 0x04002300 RID: 8960
	private MeterController meter;
}
