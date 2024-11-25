using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F7B RID: 3963
	public struct ElemGrowthInfo
	{
		// Token: 0x060079A9 RID: 31145 RVA: 0x00300EE4 File Offset: 0x002FF0E4
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.underPopulationDeathRate);
			writer.Write(this.populationHalfLife);
			writer.Write(this.overPopulationHalfLife);
			writer.Write(this.diffusionScale);
			writer.Write(this.minCountPerKG);
			writer.Write(this.maxCountPerKG);
			writer.Write(this.minDiffusionCount);
			writer.Write(this.minDiffusionInfestationTickCount);
		}

		// Token: 0x060079AA RID: 31146 RVA: 0x00300F54 File Offset: 0x002FF154
		public static void SetBulk(ElemGrowthInfo[] info, Func<Element, bool> test, ElemGrowthInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x060079AB RID: 31147 RVA: 0x00300F90 File Offset: 0x002FF190
		public float CalculateDiseaseCountDelta(int disease_count, float kg, float dt)
		{
			float num = this.minCountPerKG * kg;
			float num2 = this.maxCountPerKG * kg;
			float result;
			if (num <= (float)disease_count && (float)disease_count <= num2)
			{
				result = (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
			}
			else if ((float)disease_count < num)
			{
				result = -this.underPopulationDeathRate * dt;
			}
			else
			{
				result = (Disease.HalfLifeToGrowthRate(this.overPopulationHalfLife, dt) - 1f) * (float)disease_count;
			}
			return result;
		}

		// Token: 0x04005AC5 RID: 23237
		public float underPopulationDeathRate;

		// Token: 0x04005AC6 RID: 23238
		public float populationHalfLife;

		// Token: 0x04005AC7 RID: 23239
		public float overPopulationHalfLife;

		// Token: 0x04005AC8 RID: 23240
		public float diffusionScale;

		// Token: 0x04005AC9 RID: 23241
		public float minCountPerKG;

		// Token: 0x04005ACA RID: 23242
		public float maxCountPerKG;

		// Token: 0x04005ACB RID: 23243
		public int minDiffusionCount;

		// Token: 0x04005ACC RID: 23244
		public byte minDiffusionInfestationTickCount;
	}
}
