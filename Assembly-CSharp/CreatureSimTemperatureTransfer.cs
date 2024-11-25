using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007F8 RID: 2040
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	// Token: 0x06003867 RID: 14439 RVA: 0x00133F44 File Offset: 0x00132144
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20, true);
		this.averageTemperatureTransferPerSecond = new AttributeModifier(this.temperatureAttributeName + "Delta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, false, true, false);
		this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00133FBC File Offset: 0x001321BC
	protected override void OnSpawn()
	{
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.skinThickness, this.skinThicknessAttributeModifierName, false, false, true);
		attributeInstance.Add(modifier);
		base.OnSpawn();
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06003869 RID: 14441 RVA: 0x0013401D File Offset: 0x0013221D
	public bool LastTemperatureRecordIsReliable
	{
		get
		{
			return Time.time - this.lastTemperatureRecordTime < 2f && this.average_kilowatts_exchanged.HasEverHadValidValues && this.average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
		}
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00134054 File Offset: 0x00132254
	protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
	{
		if (Time.time < this.lastTemperatureRecordTime + 0.2f)
		{
			return;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(this.simHandle);
			if (Game.Instance.simData.elementChunks[handleIndex].deltaKJ == 0f)
			{
				return;
			}
			this.average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
			this.lastTemperatureRecordTime = Time.time;
		}
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x001340ED File Offset: 0x001322ED
	private void Update()
	{
		this.unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x001340FC File Offset: 0x001322FC
	public void Sim200ms(float dt)
	{
		this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetUnweightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier attributeModifier in this.NonSimTemperatureModifiers)
		{
			num += attributeModifier.Value;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			float num2 = num * (this.primaryElement.Mass * 1000f) * this.primaryElement.Element.specificHeatCapacity * 0.001f;
			float delta_kj = num2 * dt;
			SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kj);
			this.heatEffect.SetHeatBeingProducedValue(num2);
			return;
		}
		this.heatEffect.SetHeatBeingProducedValue(0f);
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x001341F4 File Offset: 0x001323F4
	public void RefreshRegistration()
	{
		base.SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier);
		this.thickness = attributeInstance.GetTotalValue();
		this.simHandle = -1;
		base.SimRegister();
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x00134240 File Offset: 0x00132440
	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}

	// Token: 0x040021DF RID: 8671
	public string temperatureAttributeName = "Temperature";

	// Token: 0x040021E0 RID: 8672
	public float skinThickness = DUPLICANTSTATS.STANDARD.Temperature.SKIN_THICKNESS;

	// Token: 0x040021E1 RID: 8673
	public string skinThicknessAttributeModifierName = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x040021E2 RID: 8674
	public AttributeModifier averageTemperatureTransferPerSecond;

	// Token: 0x040021E3 RID: 8675
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x040021E4 RID: 8676
	private PrimaryElement primaryElement;

	// Token: 0x040021E5 RID: 8677
	public RunningWeightedAverage average_kilowatts_exchanged;

	// Token: 0x040021E6 RID: 8678
	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

	// Token: 0x040021E7 RID: 8679
	private float lastTemperatureRecordTime;
}
