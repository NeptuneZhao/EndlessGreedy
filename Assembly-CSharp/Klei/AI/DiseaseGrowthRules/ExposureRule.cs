using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F82 RID: 3970
	public class ExposureRule
	{
		// Token: 0x060079C0 RID: 31168 RVA: 0x003013AC File Offset: 0x002FF5AC
		public void Apply(ElemExposureInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (this.Test(elements[i]))
				{
					ElemExposureInfo elemExposureInfo = infoList[i];
					if (this.populationHalfLife != null)
					{
						elemExposureInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					infoList[i] = elemExposureInfo;
				}
			}
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x0030140F File Offset: 0x002FF60F
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x00301412 File Offset: 0x002FF612
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x04005AE2 RID: 23266
		public float? populationHalfLife;
	}
}
