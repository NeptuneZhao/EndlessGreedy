using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x020008D2 RID: 2258
[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	// Token: 0x06004053 RID: 16467 RVA: 0x0016C67C File Offset: 0x0016A87C
	public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
	{
		GeyserConfigurator.GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = GeyserConfigurator.geyserTypes.Find((GeyserConfigurator.GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a geyser with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return geyserType;
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x0016C6E5 File Offset: 0x0016A8E5
	public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x0016C700 File Offset: 0x0016A900
	private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserConfigurator.GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = this.Roll(randomSource, min, max),
			iterationLengthRoll = this.Roll(randomSource, 0f, 1f),
			iterationPercentRoll = this.Roll(randomSource, min, max),
			yearLengthRoll = this.Roll(randomSource, 0f, 1f),
			yearPercentRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x0016C7AD File Offset: 0x0016A9AD
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04002A77 RID: 10871
	private static List<GeyserConfigurator.GeyserType> geyserTypes;

	// Token: 0x04002A78 RID: 10872
	public HashedString presetType;

	// Token: 0x04002A79 RID: 10873
	public float presetMin;

	// Token: 0x04002A7A RID: 10874
	public float presetMax = 1f;

	// Token: 0x0200180E RID: 6158
	public enum GeyserShape
	{
		// Token: 0x040074DB RID: 29915
		Gas,
		// Token: 0x040074DC RID: 29916
		Liquid,
		// Token: 0x040074DD RID: 29917
		Molten
	}

	// Token: 0x0200180F RID: 6159
	public class GeyserType
	{
		// Token: 0x06009746 RID: 38726 RVA: 0x00365124 File Offset: 0x00363324
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.DlcID = DlcID;
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x06009747 RID: 38727 RVA: 0x003651EF File Offset: 0x003633EF
		public GeyserConfigurator.GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		// Token: 0x06009748 RID: 38728 RVA: 0x003651FC File Offset: 0x003633FC
		public GeyserType()
		{
			this.id = "Blank";
			this.element = SimHashes.Void;
			this.temperature = 0f;
			this.minRatePerCycle = 0f;
			this.maxRatePerCycle = 0f;
			this.maxPressure = 0f;
			this.minIterationLength = 0f;
			this.maxIterationLength = 0f;
			this.minIterationPercent = 0f;
			this.maxIterationPercent = 0f;
			this.minYearLength = 0f;
			this.maxYearLength = 0f;
			this.minYearPercent = 0f;
			this.maxYearPercent = 0f;
			this.geyserTemperature = 0f;
			this.DlcID = "";
		}

		// Token: 0x040074DE RID: 29918
		public string id;

		// Token: 0x040074DF RID: 29919
		public HashedString idHash;

		// Token: 0x040074E0 RID: 29920
		public SimHashes element;

		// Token: 0x040074E1 RID: 29921
		public GeyserConfigurator.GeyserShape shape;

		// Token: 0x040074E2 RID: 29922
		public float temperature;

		// Token: 0x040074E3 RID: 29923
		public float minRatePerCycle;

		// Token: 0x040074E4 RID: 29924
		public float maxRatePerCycle;

		// Token: 0x040074E5 RID: 29925
		public float maxPressure;

		// Token: 0x040074E6 RID: 29926
		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x040074E7 RID: 29927
		public float minIterationLength;

		// Token: 0x040074E8 RID: 29928
		public float maxIterationLength;

		// Token: 0x040074E9 RID: 29929
		public float minIterationPercent;

		// Token: 0x040074EA RID: 29930
		public float maxIterationPercent;

		// Token: 0x040074EB RID: 29931
		public float minYearLength;

		// Token: 0x040074EC RID: 29932
		public float maxYearLength;

		// Token: 0x040074ED RID: 29933
		public float minYearPercent;

		// Token: 0x040074EE RID: 29934
		public float maxYearPercent;

		// Token: 0x040074EF RID: 29935
		public float geyserTemperature;

		// Token: 0x040074F0 RID: 29936
		public string DlcID;

		// Token: 0x040074F1 RID: 29937
		public const string BLANK_ID = "Blank";

		// Token: 0x040074F2 RID: 29938
		public const SimHashes BLANK_ELEMENT = SimHashes.Void;

		// Token: 0x040074F3 RID: 29939
		public const string BLANK_DLCID = "";
	}

	// Token: 0x02001810 RID: 6160
	[Serializable]
	public class GeyserInstanceConfiguration
	{
		// Token: 0x06009749 RID: 38729 RVA: 0x003652CA File Offset: 0x003634CA
		public Geyser.GeyserModification GetModifier()
		{
			return this.modifier;
		}

		// Token: 0x0600974A RID: 38730 RVA: 0x003652D4 File Offset: 0x003634D4
		public void Init(bool reinit = false)
		{
			if (this.didInit && !reinit)
			{
				return;
			}
			this.didInit = true;
			this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
			this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
			this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
			this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
			this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
		}

		// Token: 0x0600974B RID: 38731 RVA: 0x003653BC File Offset: 0x003635BC
		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x0600974C RID: 38732 RVA: 0x003653C5 File Offset: 0x003635C5
		public GeyserConfigurator.GeyserType geyserType
		{
			get
			{
				return GeyserConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600974D RID: 38733 RVA: 0x003653D4 File Offset: 0x003635D4
		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			if (method != Geyser.ModificationMethod.Values)
			{
				if (method == Geyser.ModificationMethod.Percentages)
				{
					num += geyserVariable * modifier;
				}
			}
			else
			{
				num += modifier;
			}
			return num;
		}

		// Token: 0x0600974E RID: 38734 RVA: 0x003653F7 File Offset: 0x003635F7
		public float GetMaxPressure()
		{
			return this.GetModifiedValue(this.geyserType.maxPressure, this.modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		// Token: 0x0600974F RID: 38735 RVA: 0x0036541A File Offset: 0x0036361A
		public float GetIterationLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledIterationLength, this.modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		// Token: 0x06009750 RID: 38736 RVA: 0x0036543F File Offset: 0x0036363F
		public float GetIterationPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledIterationPercent, this.modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x06009751 RID: 38737 RVA: 0x00365473 File Offset: 0x00363673
		public float GetOnDuration()
		{
			return this.GetIterationLength() * this.GetIterationPercent();
		}

		// Token: 0x06009752 RID: 38738 RVA: 0x00365482 File Offset: 0x00363682
		public float GetOffDuration()
		{
			return this.GetIterationLength() * (1f - this.GetIterationPercent());
		}

		// Token: 0x06009753 RID: 38739 RVA: 0x00365497 File Offset: 0x00363697
		public float GetMassPerCycle()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledRate, this.modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		// Token: 0x06009754 RID: 38740 RVA: 0x003654BC File Offset: 0x003636BC
		public float GetEmitRate()
		{
			float num = 600f / this.GetIterationLength();
			return this.GetMassPerCycle() / num / this.GetOnDuration();
		}

		// Token: 0x06009755 RID: 38741 RVA: 0x003654E5 File Offset: 0x003636E5
		public float GetYearLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledYearLength, this.modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		// Token: 0x06009756 RID: 38742 RVA: 0x0036550A File Offset: 0x0036370A
		public float GetYearPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledYearPercent, this.modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x06009757 RID: 38743 RVA: 0x0036553E File Offset: 0x0036373E
		public float GetYearOnDuration()
		{
			return this.GetYearLength() * this.GetYearPercent();
		}

		// Token: 0x06009758 RID: 38744 RVA: 0x0036554D File Offset: 0x0036374D
		public float GetYearOffDuration()
		{
			return this.GetYearLength() * (1f - this.GetYearPercent());
		}

		// Token: 0x06009759 RID: 38745 RVA: 0x00365562 File Offset: 0x00363762
		public SimHashes GetElement()
		{
			if (!this.modifier.modifyElement || this.modifier.newElement == (SimHashes)0)
			{
				return this.geyserType.element;
			}
			return this.modifier.newElement;
		}

		// Token: 0x0600975A RID: 38746 RVA: 0x00365595 File Offset: 0x00363795
		public float GetTemperature()
		{
			return this.GetModifiedValue(this.geyserType.temperature, this.modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		// Token: 0x0600975B RID: 38747 RVA: 0x003655B8 File Offset: 0x003637B8
		public byte GetDiseaseIdx()
		{
			return this.geyserType.diseaseInfo.idx;
		}

		// Token: 0x0600975C RID: 38748 RVA: 0x003655CA File Offset: 0x003637CA
		public int GetDiseaseCount()
		{
			return this.geyserType.diseaseInfo.count;
		}

		// Token: 0x0600975D RID: 38749 RVA: 0x003655DC File Offset: 0x003637DC
		public float GetAverageEmission()
		{
			float num = this.GetEmitRate() * this.GetOnDuration();
			return this.GetYearOnDuration() / this.GetIterationLength() * num / this.GetYearLength();
		}

		// Token: 0x0600975E RID: 38750 RVA: 0x00365610 File Offset: 0x00363810
		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (-Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}

		// Token: 0x040074F4 RID: 29940
		public HashedString typeId;

		// Token: 0x040074F5 RID: 29941
		public float rateRoll;

		// Token: 0x040074F6 RID: 29942
		public float iterationLengthRoll;

		// Token: 0x040074F7 RID: 29943
		public float iterationPercentRoll;

		// Token: 0x040074F8 RID: 29944
		public float yearLengthRoll;

		// Token: 0x040074F9 RID: 29945
		public float yearPercentRoll;

		// Token: 0x040074FA RID: 29946
		public float scaledRate;

		// Token: 0x040074FB RID: 29947
		public float scaledIterationLength;

		// Token: 0x040074FC RID: 29948
		public float scaledIterationPercent;

		// Token: 0x040074FD RID: 29949
		public float scaledYearLength;

		// Token: 0x040074FE RID: 29950
		public float scaledYearPercent;

		// Token: 0x040074FF RID: 29951
		private bool didInit;

		// Token: 0x04007500 RID: 29952
		private Geyser.GeyserModification modifier;
	}
}
