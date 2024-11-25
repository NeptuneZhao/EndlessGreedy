using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F81 RID: 3969
	public struct ElemExposureInfo
	{
		// Token: 0x060079BD RID: 31165 RVA: 0x00301349 File Offset: 0x002FF549
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.populationHalfLife);
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x00301358 File Offset: 0x002FF558
		public static void SetBulk(ElemExposureInfo[] info, Func<Element, bool> test, ElemExposureInfo settings)
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

		// Token: 0x060079BF RID: 31167 RVA: 0x00301393 File Offset: 0x002FF593
		public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
		{
			return (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
		}

		// Token: 0x04005AE1 RID: 23265
		public float populationHalfLife;
	}
}
