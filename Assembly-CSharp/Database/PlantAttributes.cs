using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E74 RID: 3700
	public class PlantAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x060074C7 RID: 29895 RVA: 0x002D872C File Offset: 0x002D692C
		public PlantAttributes(ResourceSet parent) : base("PlantAttributes", parent)
		{
			this.WiltTempRangeMod = base.Add(new Klei.AI.Attribute("WiltTempRangeMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.WiltTempRangeMod.SetFormatter(new PercentAttributeFormatter());
			this.YieldAmount = base.Add(new Klei.AI.Attribute("YieldAmount", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.YieldAmount.SetFormatter(new PercentAttributeFormatter());
			this.HarvestTime = base.Add(new Klei.AI.Attribute("HarvestTime", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.HarvestTime.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Time, GameUtil.TimeSlice.None));
			this.DecorBonus = base.Add(new Klei.AI.Attribute("DecorBonus", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DecorBonus.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.MinLightLux = base.Add(new Klei.AI.Attribute("MinLightLux", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinLightLux.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Lux, GameUtil.TimeSlice.None));
			this.FertilizerUsageMod = base.Add(new Klei.AI.Attribute("FertilizerUsageMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.FertilizerUsageMod.SetFormatter(new PercentAttributeFormatter());
			this.MinRadiationThreshold = base.Add(new Klei.AI.Attribute("MinRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
			this.MaxRadiationThreshold = base.Add(new Klei.AI.Attribute("MaxRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MaxRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
		}

		// Token: 0x0400544B RID: 21579
		public Klei.AI.Attribute WiltTempRangeMod;

		// Token: 0x0400544C RID: 21580
		public Klei.AI.Attribute YieldAmount;

		// Token: 0x0400544D RID: 21581
		public Klei.AI.Attribute HarvestTime;

		// Token: 0x0400544E RID: 21582
		public Klei.AI.Attribute DecorBonus;

		// Token: 0x0400544F RID: 21583
		public Klei.AI.Attribute MinLightLux;

		// Token: 0x04005450 RID: 21584
		public Klei.AI.Attribute FertilizerUsageMod;

		// Token: 0x04005451 RID: 21585
		public Klei.AI.Attribute MinRadiationThreshold;

		// Token: 0x04005452 RID: 21586
		public Klei.AI.Attribute MaxRadiationThreshold;
	}
}
