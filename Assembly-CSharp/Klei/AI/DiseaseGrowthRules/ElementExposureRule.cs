using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F83 RID: 3971
	public class ElementExposureRule : ExposureRule
	{
		// Token: 0x060079C4 RID: 31172 RVA: 0x0030141D File Offset: 0x002FF61D
		public ElementExposureRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x0030142C File Offset: 0x002FF62C
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x0030143C File Offset: 0x002FF63C
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x04005AE3 RID: 23267
		public SimHashes element;
	}
}
