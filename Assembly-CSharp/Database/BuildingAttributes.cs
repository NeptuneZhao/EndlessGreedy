using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E50 RID: 3664
	public class BuildingAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06007444 RID: 29764 RVA: 0x002C80F0 File Offset: 0x002C62F0
		public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent)
		{
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.DecorRadius = base.Add(new Klei.AI.Attribute("DecorRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollution = base.Add(new Klei.AI.Attribute("NoisePollution", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollutionRadius = base.Add(new Klei.AI.Attribute("NoisePollutionRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Hygiene = base.Add(new Klei.AI.Attribute("Hygiene", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Comfort = base.Add(new Klei.AI.Attribute("Comfort", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature = base.Add(new Klei.AI.Attribute("OverheatTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
			this.FatalTemperature = base.Add(new Klei.AI.Attribute("FatalTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
		}

		// Token: 0x0400505C RID: 20572
		public Klei.AI.Attribute Decor;

		// Token: 0x0400505D RID: 20573
		public Klei.AI.Attribute DecorRadius;

		// Token: 0x0400505E RID: 20574
		public Klei.AI.Attribute NoisePollution;

		// Token: 0x0400505F RID: 20575
		public Klei.AI.Attribute NoisePollutionRadius;

		// Token: 0x04005060 RID: 20576
		public Klei.AI.Attribute Hygiene;

		// Token: 0x04005061 RID: 20577
		public Klei.AI.Attribute Comfort;

		// Token: 0x04005062 RID: 20578
		public Klei.AI.Attribute OverheatTemperature;

		// Token: 0x04005063 RID: 20579
		public Klei.AI.Attribute FatalTemperature;
	}
}
