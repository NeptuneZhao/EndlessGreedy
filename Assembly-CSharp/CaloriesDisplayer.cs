using System;
using Klei.AI;

// Token: 0x02000BD0 RID: 3024
public class CaloriesDisplayer : StandardAmountDisplayer
{
	// Token: 0x06005C3D RID: 23613 RVA: 0x0021C487 File Offset: 0x0021A687
	public CaloriesDisplayer() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new CaloriesDisplayer.CaloriesAttributeFormatter();
	}

	// Token: 0x02001CBE RID: 7358
	public class CaloriesAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600A6D8 RID: 42712 RVA: 0x00398E57 File Offset: 0x00397057
		public CaloriesAttributeFormatter() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
		{
		}

		// Token: 0x0600A6D9 RID: 42713 RVA: 0x00398E61 File Offset: 0x00397061
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			if (modifier.IsMultiplier)
			{
				return GameUtil.GetFormattedPercent(-modifier.Value * 100f, GameUtil.TimeSlice.None);
			}
			return base.GetFormattedModifier(modifier);
		}
	}
}
