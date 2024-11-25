using System;
using Klei.AI;

// Token: 0x02000BDB RID: 3035
public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005C64 RID: 23652 RVA: 0x0021D048 File Offset: 0x0021B248
	public GermResistanceAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005C65 RID: 23653 RVA: 0x0021D052 File Offset: 0x0021B252
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
	}
}
