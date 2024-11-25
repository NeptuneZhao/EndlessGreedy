using System;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

// Token: 0x020007D9 RID: 2009
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
	// Token: 0x0600376C RID: 14188 RVA: 0x0012E390 File Offset: 0x0012C590
	private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
	{
		ElemGrowthInfo result;
		if (disease_idx != 255)
		{
			result = Db.Get().Diseases[(int)disease_idx].elemGrowthInfo[(int)elem_idx];
		}
		else
		{
			result = Disease.DEFAULT_GROWTH_INFO;
		}
		return result;
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x0012E3CA File Offset: 0x0012C5CA
	public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager) : base(0)
	{
		this.temperatureManager = temperature_manager;
	}

	// Token: 0x0600376E RID: 14190 RVA: 0x0012E3DC File Offset: 0x0012C5DC
	public HandleVector<int>.Handle Allocate(HandleVector<int>.Handle temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
		ConduitDiseaseManager.Data initial_data = new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount);
		return base.Allocate(initial_data);
	}

	// Token: 0x0600376F RID: 14191 RVA: 0x0012E418 File Offset: 0x0012C618
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		ConduitDiseaseManager.Data data = base.GetData(handle);
		data.diseaseCount = contents.diseaseCount;
		if (contents.diseaseIdx != data.diseaseIdx)
		{
			data.diseaseIdx = contents.diseaseIdx;
			ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
			data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
		}
		base.SetData(handle, data);
	}

	// Token: 0x06003770 RID: 14192 RVA: 0x0012E47C File Offset: 0x0012C67C
	public void Sim200ms(float dt)
	{
		using (new KProfiler.Region("ConduitDiseaseManager.SimUpdate", null))
		{
			for (int i = 0; i < this.data.Count; i++)
			{
				ConduitDiseaseManager.Data data = this.data[i];
				if (data.diseaseIdx != 255)
				{
					float num = data.accumulatedError;
					num += data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
					Disease disease = Db.Get().Diseases[(int)data.diseaseIdx];
					float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
					num += (float)data.diseaseCount * num2 - (float)data.diseaseCount;
					int num3 = (int)num;
					data.accumulatedError = num - (float)num3;
					data.diseaseCount += num3;
					if (data.diseaseCount <= 0)
					{
						data.diseaseCount = 0;
						data.diseaseIdx = byte.MaxValue;
						data.accumulatedError = 0f;
					}
					this.data[i] = data;
				}
			}
		}
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x0012E5CC File Offset: 0x0012C7CC
	public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
		if (data.diseaseCount == 0)
		{
			data.diseaseIdx = byte.MaxValue;
		}
		base.SetData(h, data);
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x0012E614 File Offset: 0x0012C814
	public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
		data.diseaseIdx = diseaseInfo.idx;
		data.diseaseCount = diseaseInfo.count;
		base.SetData(h, data);
	}

	// Token: 0x04002167 RID: 8551
	private ConduitTemperatureManager temperatureManager;

	// Token: 0x0200169E RID: 5790
	public struct Data
	{
		// Token: 0x06009305 RID: 37637 RVA: 0x00357695 File Offset: 0x00355895
		public Data(HandleVector<int>.Handle temperature_handle, ushort elem_idx, float mass, byte disease_idx, int disease_count)
		{
			this.diseaseIdx = disease_idx;
			this.elemIdx = elem_idx;
			this.mass = mass;
			this.diseaseCount = disease_count;
			this.accumulatedError = 0f;
			this.temperatureHandle = temperature_handle;
			this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
		}

		// Token: 0x04007031 RID: 28721
		public byte diseaseIdx;

		// Token: 0x04007032 RID: 28722
		public ushort elemIdx;

		// Token: 0x04007033 RID: 28723
		public int diseaseCount;

		// Token: 0x04007034 RID: 28724
		public float accumulatedError;

		// Token: 0x04007035 RID: 28725
		public float mass;

		// Token: 0x04007036 RID: 28726
		public HandleVector<int>.Handle temperatureHandle;

		// Token: 0x04007037 RID: 28727
		public ElemGrowthInfo growthInfo;
	}
}
