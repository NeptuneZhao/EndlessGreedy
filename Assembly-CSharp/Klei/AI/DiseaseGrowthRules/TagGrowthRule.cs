using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000F7F RID: 3967
	public class TagGrowthRule : GrowthRule
	{
		// Token: 0x060079B6 RID: 31158 RVA: 0x003011C1 File Offset: 0x002FF3C1
		public TagGrowthRule(Tag tag)
		{
			this.tag = tag;
		}

		// Token: 0x060079B7 RID: 31159 RVA: 0x003011D0 File Offset: 0x002FF3D0
		public override bool Test(Element e)
		{
			return e.HasTag(this.tag);
		}

		// Token: 0x060079B8 RID: 31160 RVA: 0x003011DE File Offset: 0x002FF3DE
		public override string Name()
		{
			return this.tag.ProperName();
		}

		// Token: 0x04005AD7 RID: 23255
		public Tag tag;
	}
}
