using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F7C RID: 3964
	public class GrowthRule
	{
		// Token: 0x060079AC RID: 31148 RVA: 0x00300FFC File Offset: 0x002FF1FC
		public void Apply(ElemGrowthInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				Element element = elements[i];
				if (element.id != SimHashes.Vacuum && this.Test(element))
				{
					ElemGrowthInfo elemGrowthInfo = infoList[i];
					if (this.underPopulationDeathRate != null)
					{
						elemGrowthInfo.underPopulationDeathRate = this.underPopulationDeathRate.Value;
					}
					if (this.populationHalfLife != null)
					{
						elemGrowthInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					if (this.overPopulationHalfLife != null)
					{
						elemGrowthInfo.overPopulationHalfLife = this.overPopulationHalfLife.Value;
					}
					if (this.diffusionScale != null)
					{
						elemGrowthInfo.diffusionScale = this.diffusionScale.Value;
					}
					if (this.minCountPerKG != null)
					{
						elemGrowthInfo.minCountPerKG = this.minCountPerKG.Value;
					}
					if (this.maxCountPerKG != null)
					{
						elemGrowthInfo.maxCountPerKG = this.maxCountPerKG.Value;
					}
					if (this.minDiffusionCount != null)
					{
						elemGrowthInfo.minDiffusionCount = this.minDiffusionCount.Value;
					}
					if (this.minDiffusionInfestationTickCount != null)
					{
						elemGrowthInfo.minDiffusionInfestationTickCount = this.minDiffusionInfestationTickCount.Value;
					}
					infoList[i] = elemGrowthInfo;
				}
			}
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x00301158 File Offset: 0x002FF358
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x0030115B File Offset: 0x002FF35B
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x04005ACD RID: 23245
		public float? underPopulationDeathRate;

		// Token: 0x04005ACE RID: 23246
		public float? populationHalfLife;

		// Token: 0x04005ACF RID: 23247
		public float? overPopulationHalfLife;

		// Token: 0x04005AD0 RID: 23248
		public float? diffusionScale;

		// Token: 0x04005AD1 RID: 23249
		public float? minCountPerKG;

		// Token: 0x04005AD2 RID: 23250
		public float? maxCountPerKG;

		// Token: 0x04005AD3 RID: 23251
		public int? minDiffusionCount;

		// Token: 0x04005AD4 RID: 23252
		public byte? minDiffusionInfestationTickCount;
	}
}
