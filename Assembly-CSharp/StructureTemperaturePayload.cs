using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B22 RID: 2850
public struct StructureTemperaturePayload
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x060054D8 RID: 21720 RVA: 0x001E4F15 File Offset: 0x001E3115
	// (set) Token: 0x060054D9 RID: 21721 RVA: 0x001E4F1D File Offset: 0x001E311D
	public PrimaryElement primaryElement
	{
		get
		{
			return this.primaryElementBacking;
		}
		set
		{
			if (this.primaryElementBacking != value)
			{
				this.primaryElementBacking = value;
				this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
			}
		}
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x001E4F48 File Offset: 0x001E3148
	public StructureTemperaturePayload(GameObject go)
	{
		this.simHandleCopy = -1;
		this.enabled = true;
		this.bypass = false;
		this.overrideExtents = false;
		this.overriddenExtents = default(Extents);
		this.primaryElementBacking = go.GetComponent<PrimaryElement>();
		this.overheatable = ((this.primaryElementBacking != null) ? this.primaryElementBacking.GetComponent<Overheatable>() : null);
		this.building = go.GetComponent<Building>();
		this.operational = go.GetComponent<Operational>();
		this.heatEffect = go.GetComponent<KBatchedAnimHeatPostProcessingEffect>();
		this.pendingEnergyModifications = 0f;
		this.maxTemperature = 10000f;
		this.energySourcesKW = null;
		this.isActiveStatusItemSet = false;
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x060054DB RID: 21723 RVA: 0x001E4FF4 File Offset: 0x001E31F4
	public float TotalEnergyProducedKW
	{
		get
		{
			if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.energySourcesKW.Count; i++)
			{
				num += this.energySourcesKW[i].value;
			}
			return num;
		}
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x001E504D File Offset: 0x001E324D
	public void OverrideExtents(Extents newExtents)
	{
		this.overrideExtents = true;
		this.overriddenExtents = newExtents;
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x001E505D File Offset: 0x001E325D
	public Extents GetExtents()
	{
		if (!this.overrideExtents)
		{
			return this.building.GetExtents();
		}
		return this.overriddenExtents;
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x060054DE RID: 21726 RVA: 0x001E5079 File Offset: 0x001E3279
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x060054DF RID: 21727 RVA: 0x001E5086 File Offset: 0x001E3286
	public float ExhaustKilowatts
	{
		get
		{
			return this.building.Def.ExhaustKilowattsWhenActive;
		}
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x060054E0 RID: 21728 RVA: 0x001E5098 File Offset: 0x001E3298
	public float OperatingKilowatts
	{
		get
		{
			if (!(this.operational != null) || !this.operational.IsActive)
			{
				return 0f;
			}
			return this.building.Def.SelfHeatKilowattsWhenActive;
		}
	}

	// Token: 0x0400379B RID: 14235
	public int simHandleCopy;

	// Token: 0x0400379C RID: 14236
	public bool enabled;

	// Token: 0x0400379D RID: 14237
	public bool bypass;

	// Token: 0x0400379E RID: 14238
	public bool isActiveStatusItemSet;

	// Token: 0x0400379F RID: 14239
	public bool overrideExtents;

	// Token: 0x040037A0 RID: 14240
	private PrimaryElement primaryElementBacking;

	// Token: 0x040037A1 RID: 14241
	public Overheatable overheatable;

	// Token: 0x040037A2 RID: 14242
	public Building building;

	// Token: 0x040037A3 RID: 14243
	public Operational operational;

	// Token: 0x040037A4 RID: 14244
	public KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x040037A5 RID: 14245
	public List<StructureTemperaturePayload.EnergySource> energySourcesKW;

	// Token: 0x040037A6 RID: 14246
	public float pendingEnergyModifications;

	// Token: 0x040037A7 RID: 14247
	public float maxTemperature;

	// Token: 0x040037A8 RID: 14248
	public Extents overriddenExtents;

	// Token: 0x02001B70 RID: 7024
	public class EnergySource
	{
		// Token: 0x0600A36B RID: 41835 RVA: 0x00389D19 File Offset: 0x00387F19
		public EnergySource(float kj, string source)
		{
			this.source = source;
			this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600A36C RID: 41836 RVA: 0x00389D48 File Offset: 0x00387F48
		public float value
		{
			get
			{
				return this.kw_accumulator.AverageValue;
			}
		}

		// Token: 0x0600A36D RID: 41837 RVA: 0x00389D55 File Offset: 0x00387F55
		public void Accumulate(float value)
		{
			this.kw_accumulator.AddSample(value);
		}

		// Token: 0x04007FC4 RID: 32708
		public string source;

		// Token: 0x04007FC5 RID: 32709
		public RunningAverage kw_accumulator;
	}
}
