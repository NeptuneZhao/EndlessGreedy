using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public struct InfraredVisualizerData
{
	// Token: 0x06002043 RID: 8259 RVA: 0x000B4F8C File Offset: 0x000B318C
	public void Update()
	{
		float num = 0f;
		if (this.temperatureAmount != null)
		{
			num = this.temperatureAmount.value;
		}
		else if (this.structureTemperature.IsValid())
		{
			num = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
		else if (this.primaryElement != null)
		{
			num = this.primaryElement.Temperature;
		}
		else if (this.temperatureVulnerable != null)
		{
			num = this.temperatureVulnerable.InternalTemperature;
		}
		else if (this.critterTemperatureMonitorInstance != null)
		{
			num = this.critterTemperatureMonitorInstance.GetTemperatureInternal();
		}
		if (num < 0f)
		{
			return;
		}
		Color32 c = SimDebugView.Instance.NormalizedTemperature(num);
		this.controller.OverlayColour = c;
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000B5054 File Offset: 0x000B3254
	public InfraredVisualizerData(GameObject go)
	{
		this.controller = go.GetComponent<KBatchedAnimController>();
		if (this.controller != null)
		{
			this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
			this.primaryElement = go.GetComponent<PrimaryElement>();
			this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
			this.critterTemperatureMonitorInstance = go.GetSMI<CritterTemperatureMonitor.Instance>();
			return;
		}
		this.temperatureAmount = null;
		this.structureTemperature = HandleVector<int>.InvalidHandle;
		this.primaryElement = null;
		this.temperatureVulnerable = null;
		this.critterTemperatureMonitorInstance = null;
	}

	// Token: 0x04001235 RID: 4661
	public KAnimControllerBase controller;

	// Token: 0x04001236 RID: 4662
	public AmountInstance temperatureAmount;

	// Token: 0x04001237 RID: 4663
	public HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001238 RID: 4664
	public PrimaryElement primaryElement;

	// Token: 0x04001239 RID: 4665
	public TemperatureVulnerable temperatureVulnerable;

	// Token: 0x0400123A RID: 4666
	public CritterTemperatureMonitor.Instance critterTemperatureMonitorInstance;
}
