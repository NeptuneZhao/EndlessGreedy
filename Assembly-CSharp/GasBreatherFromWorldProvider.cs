using System;

// Token: 0x020008CA RID: 2250
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	// Token: 0x06003FF3 RID: 16371 RVA: 0x0016A694 File Offset: 0x00168894
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor = new SuffocationMonitor.Instance(oxygen_breather);
		this.suffocationMonitor.StartSM();
		this.safeCellMonitor = new SafeCellMonitor.Instance(oxygen_breather);
		this.safeCellMonitor.StartSM();
		this.oxygenBreather = oxygen_breather;
		this.nav = this.oxygenBreather.GetComponent<Navigator>();
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x0016A6E7 File Offset: 0x001688E7
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor.StopSM("Removed gas provider");
		this.safeCellMonitor.StopSM("Removed gas provider");
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x0016A709 File Offset: 0x00168909
	public bool ShouldEmitCO2()
	{
		return this.nav.CurrentNavType != NavType.Tube;
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x0016A71C File Offset: 0x0016891C
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x0016A71F File Offset: 0x0016891F
	public bool IsLowOxygen()
	{
		return this.oxygenBreather.IsLowOxygenAtMouthCell();
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x0016A72C File Offset: 0x0016892C
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.nav.CurrentNavType != NavType.Tube)
		{
			SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
			if (getBreathableElement == SimHashes.Vacuum)
			{
				return false;
			}
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback), this, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed, 3, handle.index);
		}
		return true;
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x0016A790 File Offset: 0x00168990
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((GasBreatherFromWorldProvider)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x0016A7A0 File Offset: 0x001689A0
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (this.oxygenBreather == null || this.oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			return;
		}
		if (ElementLoader.elements[(int)mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
		{
			this.oxygenBreather.Trigger(-935848905, mass_cb_info);
		}
		Game.Instance.accumulators.Accumulate(this.oxygenBreather.O2Accumulator, mass_cb_info.mass);
		float value = -mass_cb_info.mass;
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, this.oxygenBreather.GetProperName(), null);
		this.oxygenBreather.Consume(mass_cb_info);
	}

	// Token: 0x04002A40 RID: 10816
	private SuffocationMonitor.Instance suffocationMonitor;

	// Token: 0x04002A41 RID: 10817
	private SafeCellMonitor.Instance safeCellMonitor;

	// Token: 0x04002A42 RID: 10818
	private OxygenBreather oxygenBreather;

	// Token: 0x04002A43 RID: 10819
	private Navigator nav;
}
