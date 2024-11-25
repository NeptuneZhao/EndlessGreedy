using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F7E RID: 3966
	public class ElementGrowthRule : GrowthRule
	{
		// Token: 0x060079B3 RID: 31155 RVA: 0x00301190 File Offset: 0x002FF390
		public ElementGrowthRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x060079B4 RID: 31156 RVA: 0x0030119F File Offset: 0x002FF39F
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x060079B5 RID: 31157 RVA: 0x003011AF File Offset: 0x002FF3AF
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x04005AD6 RID: 23254
		public SimHashes element;
	}
}
