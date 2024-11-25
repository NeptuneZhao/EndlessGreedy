using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E5B RID: 3675
	public class CritterAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06007470 RID: 29808 RVA: 0x002D22E4 File Offset: 0x002D04E4
		public CritterAttributes(ResourceSet parent) : base("CritterAttributes", parent)
		{
			this.Happiness = base.Add(new Klei.AI.Attribute("Happiness", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.NAME"), "", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.TOOLTIP"), 0f, Klei.AI.Attribute.Display.General, false, "ui_icon_happiness", null, null));
			this.Happiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Metabolism = base.Add(new Klei.AI.Attribute("Metabolism", false, Klei.AI.Attribute.Display.Details, false, 100f, "ui_icon_metabolism", null, null, null));
			this.Metabolism.SetFormatter(new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04005298 RID: 21144
		public Klei.AI.Attribute Happiness;

		// Token: 0x04005299 RID: 21145
		public Klei.AI.Attribute Metabolism;
	}
}
