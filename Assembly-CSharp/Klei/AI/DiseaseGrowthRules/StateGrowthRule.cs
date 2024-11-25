using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F7D RID: 3965
	public class StateGrowthRule : GrowthRule
	{
		// Token: 0x060079B0 RID: 31152 RVA: 0x00301166 File Offset: 0x002FF366
		public StateGrowthRule(Element.State state)
		{
			this.state = state;
		}

		// Token: 0x060079B1 RID: 31153 RVA: 0x00301175 File Offset: 0x002FF375
		public override bool Test(Element e)
		{
			return e.IsState(this.state);
		}

		// Token: 0x060079B2 RID: 31154 RVA: 0x00301183 File Offset: 0x002FF383
		public override string Name()
		{
			return Element.GetStateString(this.state);
		}

		// Token: 0x04005AD5 RID: 23253
		public Element.State state;
	}
}
