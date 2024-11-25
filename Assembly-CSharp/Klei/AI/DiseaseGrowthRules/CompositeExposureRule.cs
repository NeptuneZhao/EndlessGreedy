using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F84 RID: 3972
	public class CompositeExposureRule
	{
		// Token: 0x060079C7 RID: 31175 RVA: 0x0030144E File Offset: 0x002FF64E
		public string Name()
		{
			return this.name;
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x00301456 File Offset: 0x002FF656
		public void Overlay(ExposureRule rule)
		{
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x00301483 File Offset: 0x002FF683
		public float GetHalfLifeForCount(int count)
		{
			return this.populationHalfLife;
		}

		// Token: 0x04005AE4 RID: 23268
		public string name;

		// Token: 0x04005AE5 RID: 23269
		public float populationHalfLife;
	}
}
