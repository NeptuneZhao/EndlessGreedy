using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F80 RID: 3968
	public class CompositeGrowthRule
	{
		// Token: 0x060079B9 RID: 31161 RVA: 0x003011EB File Offset: 0x002FF3EB
		public string Name()
		{
			return this.name;
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x003011F4 File Offset: 0x002FF3F4
		public void Overlay(GrowthRule rule)
		{
			if (rule.underPopulationDeathRate != null)
			{
				this.underPopulationDeathRate = rule.underPopulationDeathRate.Value;
			}
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			if (rule.overPopulationHalfLife != null)
			{
				this.overPopulationHalfLife = rule.overPopulationHalfLife.Value;
			}
			if (rule.diffusionScale != null)
			{
				this.diffusionScale = rule.diffusionScale.Value;
			}
			if (rule.minCountPerKG != null)
			{
				this.minCountPerKG = rule.minCountPerKG.Value;
			}
			if (rule.maxCountPerKG != null)
			{
				this.maxCountPerKG = rule.maxCountPerKG.Value;
			}
			if (rule.minDiffusionCount != null)
			{
				this.minDiffusionCount = rule.minDiffusionCount.Value;
			}
			if (rule.minDiffusionInfestationTickCount != null)
			{
				this.minDiffusionInfestationTickCount = rule.minDiffusionInfestationTickCount.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x00301304 File Offset: 0x002FF504
		public float GetHalfLifeForCount(int count, float kg)
		{
			int num = (int)(this.minCountPerKG * kg);
			int num2 = (int)(this.maxCountPerKG * kg);
			if (count < num)
			{
				return this.populationHalfLife;
			}
			if (count < num2)
			{
				return this.populationHalfLife;
			}
			return this.overPopulationHalfLife;
		}

		// Token: 0x04005AD8 RID: 23256
		public string name;

		// Token: 0x04005AD9 RID: 23257
		public float underPopulationDeathRate;

		// Token: 0x04005ADA RID: 23258
		public float populationHalfLife;

		// Token: 0x04005ADB RID: 23259
		public float overPopulationHalfLife;

		// Token: 0x04005ADC RID: 23260
		public float diffusionScale;

		// Token: 0x04005ADD RID: 23261
		public float minCountPerKG;

		// Token: 0x04005ADE RID: 23262
		public float maxCountPerKG;

		// Token: 0x04005ADF RID: 23263
		public int minDiffusionCount;

		// Token: 0x04005AE0 RID: 23264
		public byte minDiffusionInfestationTickCount;
	}
}
